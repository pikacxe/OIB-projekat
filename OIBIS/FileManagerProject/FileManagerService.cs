using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public class FileManagerService : IFileManager
    {
        private ChannelFactory<IFileIntegrityService> cf;
        private IFileIntegrityService fileIntegrityService;

        public FileManagerService()
        {
            cf = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
            fileIntegrityService = cf.CreateChannel();
        }

        public bool RequestRemoval(string fileName)
        {
            try
            {
                // Debugging purposes
                DeleteFile("");
                CalculateChecksum();
                UpdateConfig();
                // TODO update config file
                cf.Open();
                fileIntegrityService.ConfigChanged();
                Console.WriteLine("Config updated!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                cf.Close();
            }
            return true;
        }
        public void AddFile(string fileName)
        {
            Console.WriteLine("File added");
        }

        public string CalculateChecksum()
        {
            return "Checksum calculated!";
        }

        public void DeleteFile(string fileName)
        {
            Console.WriteLine("File deleted");
        }

        public void UpdateConfig()
        {
            Console.WriteLine("Config updated!");
        }

        public void UpdateFile(string fileName)
        {
            Console.WriteLine("File updated");
        }
    }
}
