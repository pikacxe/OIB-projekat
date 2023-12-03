using Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

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


        public void LogIntrusion(string data, string secret_key)
        {
            Intrusion intrusion = TripleDesAlgorithm.Decrypt(data, secret_key);
            Console.WriteLine($"[{intrusion.TimeStamp}] - Intrusion logged for file '{intrusion.FileName}' at '{intrusion.Location}'");
            try
            {
                if (intrusion.CompromiseLevel == CompromiseLevel.Critical)
                {
                    Console.WriteLine("Requesting file removal...");
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
                Console.WriteLine(e.Message);
            }
        }
    }
}
