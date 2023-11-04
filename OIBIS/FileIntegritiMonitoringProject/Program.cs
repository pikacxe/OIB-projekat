using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;
using FileIntegritiMonitoringProject;

namespace FileIntegrityMonitoringProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(FileIntegrityMonitoringService)))
            {
                Console.WriteLine("File integrity monitoring service started. Press Esc to exit...");
                FileIntegrityMonitoring fim = new FileIntegrityMonitoring();
                Task.Run(() => { while (Console.ReadKey(intercept: true).Key != ConsoleKey.Escape) ; fim.StopMonitoring(); });
                fim.StartMonitoring();
                host.Open();
                host.Close();
                fim.StartMonitoring();
            }
        }
    }

}
