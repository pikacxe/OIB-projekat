using CertificationManager;
using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileIntegrityMonitoringProject
{
    public class FileIntegrityMonitoring
    {
        string folderPath = ConfigurationManager.AppSettings["MonitoredPath"];
        int monitoringPeriod = int.Parse(ConfigurationManager.AppSettings["MonitoringPeriod"]);
        string srvCertCN = ConfigurationManager.AppSettings["srvCertCN"];
        private IIntrusionPreventionSystem ips;
        private ChannelFactory<IIntrusionPreventionSystem> channelFactory;
        private bool cancelationToken;

        public FileIntegrityMonitoring()
        {
            cancelationToken = false;

            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:6002/IIntrusionPreventionSystem"),
                                      new X509CertificateEndpointIdentity(srvCert));
            channelFactory = new ChannelFactory<IIntrusionPreventionSystem>(binding,address);
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
                Console.WriteLine("Started scan...");
                foreach (XElement element in ConfigManager.GetInstance().GetFiles)
                {
                    //Console.WriteLine(1);
                    string filename = element.Attribute("filename").Value;
                    string hash = ConfigManager.GetInstance().CalculateChecksum(filename);
                    if (hash != element.Attribute("hash").Value)
                    {
                        int counter = int.Parse(element.Attribute("counter").Value);
                        counter++;
                        element.Attribute("counter").Value = counter.ToString();
                        element.Attribute("hash").Value = hash;

                        Console.WriteLine("\n-----------------------------------------------------");
                        Console.WriteLine(filename + " - " + counter);
                        Console.WriteLine("-----------------------------------------------------");

                        ips.LogIntrusion(DateTime.Now, filename, folderPath, (CompromiseLevel)counter);
                    }
                }
                ConfigManager.GetInstance().Save();
                Console.WriteLine("Scan finished...");

                Thread.Sleep(monitoringPeriod);
            } while (!cancelationToken);
        }

        public void StopMonitoring()
        {
            cancelationToken = false;
        }
    }
}
