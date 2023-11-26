using System;
using System.ServiceModel;
using CertificationManager;

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
                while (Console.ReadKey().Key != ConsoleKey.Escape);
                host.Close();
            }
        }
    }
}
