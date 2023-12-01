using Common;
using System;
using System.ServiceModel;

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

        public void LogIntrusion(Intrusion intrusion)
        {
            Console.WriteLine("Intrusion logged! Requesting file removal...");
            try
            {
                if (intrusion.CompromiseLevel == CompromiseLevel.Critical)
                { 
                    if (fileManager.RequestRemoval(intrusion.FileName))
                    {
                        Console.WriteLine("File removed successfully");
                    }
                    else
                    {
                        Console.WriteLine("File removal failed!");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }
    }
}
