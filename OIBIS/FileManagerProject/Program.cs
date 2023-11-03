using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace FileManagerProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(FileManagerService)))
            {
                FileManager fm = new FileManager();
                host.Open();
                Console.WriteLine("File manager service started. Press Esc to exit...");
                fm.Menu();
                host.Close();
            }
        }
    }
}
