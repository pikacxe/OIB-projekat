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
        string signCertCN = ConfigurationManager.AppSettings["signCertCN"];

        [OperationBehavior(AutoDisposeParameters = true)]
        public void AddFile(IFile file)
        {
            if (File.Exists(Path.Combine(monitoredPath, file.Name)))
            {
                string message = $"File {file.Name} already exists";
                CustomConsole.WriteLine(message, MessageType.Error);
                throw new FaultException<CustomException>(new CustomException(message),message);
            }
            else
            {
                using (FileStream fs = File.Create(Path.Combine(monitoredPath, file.Name)))
                {
                    file.File.WriteTo(fs);
                    X509Certificate2 certificateSign = CertManager.GetCertificateFromStorage(StoreName.My,
                    StoreLocation.LocalMachine, signCertCN);

                    if (certificateSign == null)
                    {
                        throw new FaultException<CustomException>(new CustomException("No signature for signing was found!"));
                    }

                    string hash = Convert.ToBase64String(DigitalSignature.Create(file.File.ToArray(), certificateSign));
                    // Console.WriteLine(hash);
                    ConfigManager.GetInstance().AddEntry(file.Name, hash);
                    CustomConsole.WriteLine($"File {file.Name} added", MessageType.Success);
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
                X509Certificate2 certificateSign = CertManager.GetCertificateFromStorage(StoreName.My,
                StoreLocation.LocalMachine, signCertCN);

                if (certificateSign == null)
                {
                    throw new FaultException<CustomException>(new CustomException("No signature for signing was found!"));
                }

                string hash = Convert.ToBase64String(DigitalSignature.Create(file.File.ToArray(), certificateSign));

                ConfigManager.GetInstance().UpdateEntry(file.Name, hash);   
                CustomConsole.WriteLine($"File {file.Name} updated", MessageType.Success);

            }
            else
            {
                string message = $"File {file.Name} does not exist";
                CustomConsole.WriteLine(message, MessageType.Error);
                throw new FaultException<CustomException>(new CustomException(message),message);
            }
        }
        public void RemoveFile(string fileName)
        {
            string path = Path.Combine(monitoredPath, fileName);
            if (File.Exists(path))
            {
                ConfigManager.GetInstance().RemoveEntry(fileName);
                
                File.Delete(path);
                CustomConsole.WriteLine($"File {fileName} deleted", MessageType.Success);
            }
            else
            {
                string message = $"File {fileName} does not exist";
                CustomConsole.WriteLine(message, MessageType.Error);
                throw new FaultException<CustomException>(new CustomException(message), message);
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
                CustomConsole.WriteLine($"File {fileName} forwarded", MessageType.Info);
                return mf;
            }
            else
            {
                string message = $"File {fileName} does not exists";
                CustomConsole.WriteLine(message, MessageType.Error);
                throw new FaultException<CustomException>(new CustomException(message),message);
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
