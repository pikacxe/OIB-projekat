using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileManagerProject
{
    public class FileManagerService : IFileManager
    {
        private ChannelFactory<IFileIntegrityService> cf;
        private IFileIntegrityService fileIntegrityService;
        private string pathReader = ConfigurationManager.AppSettings["MonitoredPath"];
        
        public FileManagerService()
        {
            cf = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
            fileIntegrityService = cf.CreateChannel();
        }
        public void AddFile()
        {
            Console.WriteLine("Please enter a file name: ");
            string name = Console.ReadLine();
            //TODO: Check entered name with regex
            try
            {
                FileStream fs = File.Open(Path.Combine(pathReader, name), FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Zlatko");
                sw.Close();
                fs.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public string CalculateChecksum(IFile file)
        {
            throw new NotImplementedException();
        }
        public void DeleteFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool RequestRemoval(string fileName)
        {
            throw new NotImplementedException();
        }
        public void UpdateFile()
        {
            Console.WriteLine("File updated!");
            //TODO : Update config
        }
    }
}
