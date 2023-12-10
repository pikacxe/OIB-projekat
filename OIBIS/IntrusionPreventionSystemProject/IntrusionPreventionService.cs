using Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
            try
            {
                Intrusion intrusion = TripleDesAlgorithm.Decrypt(data, secret_key);
                CustomConsole.WriteLine($"'{intrusion.TimeStamp}' - Intrusion level '{intrusion.CompromiseLevel}' logged for file '{intrusion.FileName}' at '{intrusion.Location}'",MessageType.Warning);
                Audit.LogIntrusion(intrusion);
                if (intrusion.CompromiseLevel == CompromiseLevel.Critical)
                {
                    CustomConsole.WriteLine("Requesting file removal...", MessageType.Info);
                    if (fileManager.RequestRemoval(intrusion.FileName))
                    {
                        CustomConsole.WriteLine("File removed successfully", MessageType.Success);
                    }
                    else
                    {
                        CustomConsole.WriteLine("File removal failed!", MessageType.Error);
                    }
                }
            }
            catch (FaultException<CustomException> fe)
            {
                CustomConsole.WriteLine(fe.Detail.Message, MessageType.Error);
            }
            catch (Exception e)
            {
                CustomConsole.WriteLine(e.Message, MessageType.Error);
            }
        }
    }
}
