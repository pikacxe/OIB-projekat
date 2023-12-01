using CertificationManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Xml.Linq;

namespace FileIntegrityMonitoringProject
{
    public class ConfigManager
    {
        public static ConfigManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ConfigManager();
            }
            return _instance;
        }

        static ConfigManager _instance;
        string folderPath = ConfigurationManager.AppSettings["MonitoredPath"];
        private string configFile = "config.xml";
        private XDocument xmlDocument = new XDocument(new XElement("files"));

        public IEnumerable<XElement> GetFiles { get => xmlDocument.Root.Elements("file"); }

        private ConfigManager()
        {
            // If config.xml does not exist, generate a default one
            if (!File.Exists(configFile))
            {
                CreateConfig();
            }
            else
            {
                xmlDocument = XDocument.Load(configFile);
            }
        }

        //metoda za kreiranje pocetnog config fajla od predefinisanog
        //foldera sa fajlovima
        //debuging purposes
        public void CreateConfig()
        {
            //sve fajlove pakujemo u xml config fajl
            foreach (string filePath in Directory.GetFiles("MonitoredFiles"))
            {
                string filename = Path.GetFileName(filePath);

                string hash = "";
                byte[] data = File.ReadAllBytes(Path.Combine(folderPath, filename));

                string signCertCN = "FIMCert";
                X509Certificate2 certificateSign = CertManager.GetCertificateFromStorage(StoreName.My,
                StoreLocation.LocalMachine, signCertCN);
                hash = Convert.ToBase64String(DigitalSignature.Create(data, certificateSign));


                //za svaki fajl pamtimo ime, hash i broj
                //neovlascenih izmena
                XElement fileElement = new XElement("file",
                    new XAttribute("filename", filename),
                    new XAttribute("hash", hash),
                    new XAttribute("counter", 0));
                xmlDocument.Root.Add(fileElement);
            }

            xmlDocument.Save(configFile);
        }

        public void AddEntry(string fileName, string checksum)
        {
            XElement fileElement = new XElement("file",
                new XAttribute("filename", fileName),
                new XAttribute("hash", checksum),
                new XAttribute("counter", 0));
            xmlDocument.Root.Add(fileElement);
        }

        public void UpdateEntry(string fileName, string checksum)
        {
            XElement element = GetFiles.FirstOrDefault(elem => elem.Attribute("filename").Value == fileName);
            element.Attribute("hash").Value = checksum;
        }

        public void RemoveEntry(string fileName)
        {
            GetFiles.FirstOrDefault(elem => elem.Attribute("filename").Value == fileName).Remove();
        }

        public void Save()
        {
            xmlDocument.Save(configFile);
        }

        public XElement ReadEntry(string fileName)
        {
            return GetFiles.FirstOrDefault(elem => elem.Attribute("filename").Value == fileName);
        }

        //Funkcija za racunanje checksum koristeci SHA1
        //debuging purposes
        public String CalculateChecksum(string fileName)
        {
            byte[] checksum;
            using (var stream = File.OpenRead(Path.Combine(folderPath, fileName)))
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                checksum = sha1.ComputeHash(stream);
            }

            //iz niza bajtova pretvaramo u string
            return BitConverter.ToString(checksum).Replace("-", string.Empty);
        }
    }
}
