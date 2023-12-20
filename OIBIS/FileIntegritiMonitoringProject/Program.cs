using System;
using System.ServiceModel;
using System.Threading.Tasks;
using CertificationManager;
using Common;

namespace FileIntegrityMonitoringProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Print username of a user who is running a service
            Formatter.PrintCurrentUser();
            using (ServiceHost host = new ServiceHost(typeof(FileIntegrityMonitoringService)))
            {
                try
                {
                    host.Open();
                    CustomConsole.WriteLine("File integrity monitoring service started. Press <Esc> to exit...", MessageType.Info);
                    FileIntegrityMonitoring fim = new FileIntegrityMonitoring();

                    fim.StartMonitoring();
                }
                catch (Exception ex)
                {
                    CustomConsole.WriteLine(ex.Message, MessageType.Error);
                }
                finally
                {
                    host.Close();
                }
                Console.ReadKey(intercept: true);
            }
        }
    }

}
