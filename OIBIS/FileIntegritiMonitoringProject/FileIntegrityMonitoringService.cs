using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.ServiceModel;
using CertificationManager;
using Common;

namespace FileIntegrityMonitoringProject
{
    public class FileIntegrityMonitoringService : IFileIntegrityService
    {
        string monitoredPath = ConfigurationManager.AppSettings["MonitoredPath"];

        [OperationBehavior(AutoDisposeParameters = true)]
        public void AddFile(IFile file)
        {
            if (File.Exists(Path.Combine(monitoredPath, file.Name)))
            {
                Console.WriteLine("Fajl vec postoji");
            }
            else
            {
                using (FileStream fs = File.Create(Path.Combine(monitoredPath, file.Name)))
                {
                    file.File.WriteTo(fs);
                    Console.WriteLine("File added");

                    string signCertCN = "FIMCert";
                    X509Certificate2 certificateSign = CertManager.GetCertificateFromStorage(StoreName.My,
                    StoreLocation.LocalMachine, signCertCN);
                    string hash = Convert.ToBase64String(DigitalSignature.Create(file.File.ToArray(), certificateSign));
                    Console.WriteLine(hash);
                    ConfigManager.GetInstance().AddEntry(file.Name, hash);
                }
            }
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void UpdateFile(IFile file)
        {
            string path = Path.Combine(monitoredPath, file.Name);
            if (File.Exists(path))
            {
                File.WriteAllBytes(path,file.File.ToArray());
                string signCertCN = "FIMCert";
                X509Certificate2 certificateSign = CertManager.GetCertificateFromStorage(StoreName.My,
                StoreLocation.LocalMachine, signCertCN);
                string hash = Convert.ToBase64String(DigitalSignature.Create(file.File.ToArray(), certificateSign));

                ConfigManager.GetInstance().UpdateEntry(file.Name, hash);
                ConfigManager.GetInstance().Save();
                Console.WriteLine("File updated");

            }
            else
            {
                Console.WriteLine($"Fajl {file.Name} ne postoji");
            }
        }
        public void RemoveFile(string fileName)
        {
            string path = Path.Combine(monitoredPath, fileName);
            if (File.Exists(path))
            {
                ConfigManager.GetInstance().RemoveEntry(fileName);
                File.Delete(path);
                Console.WriteLine("File deleted");
            }
            else
            {
                Console.WriteLine("Fajl ne postoji");
            }
        }

        public IFile ReadFile(string fileName)
        {
            string path = Path.Combine(monitoredPath, fileName);
            MonitoredFile mf = new MonitoredFile();

            if (File.Exists(path))
            {
                mf.Name = fileName;
                mf.Hash = String.Empty;

                using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(mf.File);
                }
                Console.WriteLine("File sent");
                return mf;
            }
            else
            {
                Console.WriteLine("Fajl ne postoji");
                return mf;
            }
        }

        public List<string> ReadFileNames()
        {
            List<string> fileNames = new List<string>();
            DirectoryInfo di = new DirectoryInfo(monitoredPath);

            foreach (FileInfo fi in di.GetFiles())
            {
                fileNames.Add(fi.Name);
            }

            return fileNames;
        }
    }
}
