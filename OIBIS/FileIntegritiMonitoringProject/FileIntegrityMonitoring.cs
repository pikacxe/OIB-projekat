using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        private ChannelFactory<IIntrusionPreventionSystem> cf;
        private IIntrusionPreventionSystem intrusionPreventionSystem;
        private bool cancelationToken;

        public FileIntegrityMonitoring()
        {
            cancelationToken = true;
            cf = new ChannelFactory<IIntrusionPreventionSystem>("IIntrusionPreventionSystem");
            intrusionPreventionSystem = cf.CreateChannel();
        }

        public void StartMonitoring()
        {
            //ucitavanje configa, potpisivanje jednog po jednog fajla i
            //proveravanje njihovih hash vrednosti
            do
            {
                XDocument config = XDocument.Load("config.xml");
                XElement files = config.Element("files");

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

                        intrusionPreventionSystem.LogIntrusion(DateTime.Now, filename, folderPath, (CompromiseLevel)counter);
                    }
                }
                config.Save("config.xml");

                Thread.Sleep(monitoringPeriod);
            } while (cancelationToken);
        }

        public void StopMonitoring()
        {
            cancelationToken = false;
        }

        //metoda za kreiranje pocetnog config fajla od predefinisanog
        //foldera sa fajlovima
        //debuging purposes
        public void CreateConfig()
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

            xmlDocument.Save("config.xml");
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
