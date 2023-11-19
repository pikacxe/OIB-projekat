using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CertificationManager;
using Common;

namespace FileManagerProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Print username of a user who is running a service
            Formatter.PrintCurrentUser();
            using (ServiceHost host = new ServiceHost(typeof(FileManagerService)))
            {
                host.Open();
                Console.WriteLine("File manager service started. Press Esc to exit...");
                host.Close();
            }
        }
    }
}
