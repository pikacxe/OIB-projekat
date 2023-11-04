using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileManagerProject
{
    public class FileManagerService : IFileManager
    {
        string monitoredPath = ConfigurationManager.AppSettings["MonitoredPath"];
        public bool RequestRemoval(string fileName)
        {
            string filePath = Path.Combine(monitoredPath, fileName);
            // TODO check if file in use
            FileInfo fi = new FileInfo(filePath);
            while (IsFileLocked(fi)) ;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                // TODO notify Monitoring service about changes
                return true;
            }

            return false;
        }
        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
