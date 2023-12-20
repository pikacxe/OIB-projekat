using System;
using System.ServiceModel;
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
                try
                {
                    host.Open();
                    CustomConsole.WriteLine("File manager service started. Press <Esc> to exit...", MessageType.Info);
                    while (Console.ReadKey().Key != ConsoleKey.Escape) ;
                    host.Close();
                }
                catch (Exception ex)
                {
                    CustomConsole.WriteLine(ex.Message, MessageType.Error);
                }
            }
        }
    }
}
