using Common;
using System;
using System.Diagnostics;

namespace IntrusionPreventionSystemProject
{
    public class Audit:IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "IntrusionPreventionSystem.Audit";
        const string LogName = "IntrusionLog";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public static void LogIntrusion(Intrusion intrusion)
        {
            if (customLog != null)
            {
                string message = $"[{intrusion.TimeStamp}] [{intrusion.CompromiseLevel.ToString()}] - Intrusion logged for file '{intrusion.FileName}' at '{intrusion.Location}'";
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event to event log."));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
