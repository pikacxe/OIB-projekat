using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace FileIntegrityMonitoringProject
{
    public class FileIntegrityMonitoringService : IFileIntegrityService
    {
        private ChannelFactory<IIntrusionPreventionSystem> cf;
        private IIntrusionPreventionSystem  intrusionPreventionSystem;

        public FileIntegrityMonitoringService()
        {
           cf = new ChannelFactory<IIntrusionPreventionSystem>("IIntrusionPreventionSystem");
           intrusionPreventionSystem = cf.CreateChannel();
        }
        public bool ConfigChanged()
        {
            // TODO update config and reset monitoring
            return true;
        }

        public void LoadConfig(string path)
        {
            // TODO: Initialize FileSystemWatcher with config
            throw new NotImplementedException();
        }

        public bool StartMonitoring()
        {
            Console.WriteLine("Radim");
            // TODO: Start monitoring files
            try
            {
                // Debugging purposes
                Console.WriteLine("Intrusion detected!");
                cf.Open();
                //intrusionPreventionSystem.LogIntrusion();
                Console.WriteLine("Intrusion logged!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                cf.Close();
            }
            return true;
        }
    }
}
