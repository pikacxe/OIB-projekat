﻿using System;
using System.ServiceModel;
using System.Threading.Tasks;
using CertificationManager;

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
                Console.WriteLine("File integrity monitoring service started. Press Esc to exit...");
                host.Open();
                FileIntegrityMonitoring fim = new FileIntegrityMonitoring();
                Task.Run(() => { while (Console.ReadKey(intercept: true).Key != ConsoleKey.Escape) ; fim.StopMonitoring(); });
                fim.StartMonitoring();
                host.Close();
            }
        }
    }

}
