using CertificationManager;
using Common;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace FileIntegrityMonitoringProject
{
    public class FileIntegrityMonitoring
    {
        string folderPath = ConfigurationManager.AppSettings["MonitoredPath"];
        int monitoringPeriod = int.Parse(ConfigurationManager.AppSettings["MonitoringPeriod"]);
        string srvCertCN = ConfigurationManager.AppSettings["srvCertCN"];
        string signCertCN = ConfigurationManager.AppSettings["signCertCN"];
        private IIntrusionPreventionSystem ips;
        private ChannelFactory<IIntrusionPreventionSystem> channelFactory;
        private bool cancelationToken;
        private X509Certificate2 certificateSign;
        private string key;

        public FileIntegrityMonitoring()
        {
            /// Create private key
            SymmetricAlgorithm symalg = TripleDESCryptoServiceProvider.Create();
            key = Encoding.ASCII.GetString(symalg.Key);
            cancelationToken = false;

            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            certificateSign = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);

            if(certificateSign == null)
            {
                CustomConsole.WriteLine("No certificate for signing found", MessageType.Error);
                throw new Exception("No certificate for signing found");
            }

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:6002/IIntrusionPreventionSystem"),
                                      new X509CertificateEndpointIdentity(srvCert));
            channelFactory = new ChannelFactory<IIntrusionPreventionSystem>(binding, address);
            channelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            channelFactory.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            channelFactory.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            channelFactory.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            ips = channelFactory.CreateChannel();
        }

        public void StartMonitoring()
        {
            //ucitavanje configa, potpisivanje jednog po jednog fajla i
            //proveravanje njihovih hash vrednosti
            do
            {
                CustomConsole.WriteLine("Started scan...", MessageType.Info);
                foreach (XElement element in ConfigManager.GetInstance().GetFiles)
                {
                    string filename = element.Attribute("filename").Value;

                    byte[] data = File.ReadAllBytes(Path.Combine(folderPath, filename));
                    if (!DigitalSignature.Verify(data, Convert.FromBase64String(element.Attribute("hash").Value), certificateSign))
                    {
                        int counter = int.Parse(element.Attribute("counter").Value);
                        counter = counter + 1 > 3 ? 3 : counter + 1;

                        CustomConsole.WriteLine("-----------------------------------------------------", MessageType.Warning);
                        CustomConsole.WriteLine(filename + " - " + counter, MessageType.Warning);
                        CustomConsole.WriteLine("-----------------------------------------------------", MessageType.Warning);

                        element.Attribute("counter").Value = counter.ToString();

                        string hash = Convert.ToBase64String(DigitalSignature.Create(data, certificateSign));

                        element.Attribute("hash").Value = hash;


                        ConfigManager.GetInstance().UpdateEntry(filename, hash);

                        Intrusion intrusion = new Intrusion()
                        {
                            TimeStamp = DateTime.Now,
                            FileName = filename,
                            Location = folderPath,
                            CompromiseLevel = (CompromiseLevel)counter,
                        };

                        ips.LogIntrusion(TripleDesAlgorithm.Encrypt(intrusion, key), key);
                    }
                }
                CustomConsole.WriteLine("Scan finished...", MessageType.Info);

                Thread.Sleep(monitoringPeriod);
            } while (!cancelationToken);
        }

        public void StopMonitoring()
        {
            cancelationToken = false;
        }
    }
}
