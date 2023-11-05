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

namespace FileIntegritiMonitoringProject
{
    public class FileIntegrityMonitoring
    {
        string folderPath = ConfigurationManager.AppSettings["MonitoredPath"];
        int monitoringPeriod = int.Parse(ConfigurationManager.AppSettings["MonitoringPeriod"]);
        string srvCertCN = ConfigurationManager.AppSettings["srvCertCN"];
        private IIntrusionPreventionSystem ips;
        private ChannelFactory<IIntrusionPreventionSystem> channelFactory;
        private bool cancelationToken;
        private string configFile = "config.xml";

        public FileIntegrityMonitoring()
        {
            cancelationToken = false;
            // If config.xml does not exist, generate a default one
            if (!File.Exists(configFile))
            {
                CreateConfig();
            }

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
                XDocument config = XDocument.Load(configFile);
                XElement files = config.Element("files");

                Console.WriteLine("Started scan...");
                foreach (XElement element in files.Elements())
                {
                    string filename = element.Attribute("filename").Value;
                    string hash = CalculateChecksum(Path.Combine(folderPath, filename));
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
                Console.WriteLine("Scan finished...");
                config.Save("config.xml");

                Thread.Sleep(monitoringPeriod);
            } while (!cancelationToken);
        }

        public void StopMonitoring()
        {
            cancelationToken = false;
        }

        //metoda za kreiranje pocetnog config fajla od predefinisanog
        //foldera sa fajlovima
        //debuging purposes
        private void CreateConfig()
        {
            XDocument xmlDocument = new XDocument(new XElement("files"));

            //sve fajlove pakujemo u xml config fajl
            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                string fileName = Path.GetFileName(filePath);
                string checksum = CalculateChecksum(filePath);

                //za svaki fajl pamtimo ime, hash i broj
                //neovlascenih izmena
                XElement fileElement = new XElement("file",
                    new XAttribute("filename", fileName),
                    new XAttribute("hash", checksum),
                    new XAttribute("counter", 0));
                xmlDocument.Root.Add(fileElement);
            }

            xmlDocument.Save(configFile);
        }

        //Funkcija za racunanje checksum koristeci SHA1
        //debuging purposes
        public String CalculateChecksum(String filePath)
        {
            byte[] checksum;
            using (var stream = File.OpenRead(filePath))
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                checksum = sha1.ComputeHash(stream);
            }

            //iz niza bajtova pretvaramo u string
            return BitConverter.ToString(checksum).Replace("-", string.Empty);
        }
    }
}
