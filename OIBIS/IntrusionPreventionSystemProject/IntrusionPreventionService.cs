using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IntrusionPreventionSystemProject
{
    public class IntrusionPreventionService : IIntrusionPreventionSystem
    {
        private ChannelFactory<IFileManager> cf;
        private IFileManager fileManager;

        public IntrusionPreventionService()
        {
            cf = new ChannelFactory<IFileManager>("IFileManager");
            fileManager = cf.CreateChannel();
        }

        public void LogIntrusion(DateTime timeStamp, string fileName, string location, CompromiseLevel compromiseLevel)
        {
            Console.WriteLine("Intrusion logged! Requesting file removal...");
            try
            {
                cf.Open();
                if (fileManager.RequestRemoval(fileName))
                {
                    Console.WriteLine("File removed successfully");
                }
                else
                {
                    Console.WriteLine("File removal failed!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
            finally
            {
                cf.Close();
            }
        }
    }
}
