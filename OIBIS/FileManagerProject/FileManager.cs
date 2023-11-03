using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public class FileManager
    {
        Regex r = new Regex(@"[a-zA-Z0-9]\.[a-zA-Z0-9]+$");
        private ChannelFactory<IFileIntegrityService> cf;
        private IFileIntegrityService fileIntegrityService;
        private string pathReader = ConfigurationManager.AppSettings["MonitoredPath"];

        public FileManager()
        {
            cf = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
            fileIntegrityService = cf.CreateChannel();
        }

        public void Menu()
        {
            string key = string.Empty;
            do
            {
                Console.Clear();
                Console.WriteLine("---------------------\nOptions:\n\tA - Add file\n\tU - update file\n\tQ - Quit process\n\nPick: ");
                key = Console.ReadLine().ToUpper();
                switch (key)
                {
                    case "A": AddFile(); break;
                    //case "U": Update("update"); break;
                    default: break;
                }
            } while (key != "Q");
            cf.Close();
        }

        public void AddFile()
        {
            Console.WriteLine("Please enter a file name: ");
            string name = Console.ReadLine();
            if (!r.IsMatch(name))
            {
                name += ".txt";
            }
            //TODO: Check entered name with regex
            try
            {
                FileStream fs = File.Open(Path.Combine(pathReader, name), FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("I am here...");
                sw.Close();
                fs.Close();
                fileIntegrityService.ConfigChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
