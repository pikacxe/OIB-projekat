using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;

namespace FileIntegrityMonitoringProject
{
    public class FileIntegrityMonitoringService : IFileIntegrityService
    {
        [OperationBehavior(AutoDisposeParameters = true)]
        public bool ConfigChanged(IFile file)
        {
            // TODO update config and reset monitoring
            Console.WriteLine($"[RECEIVED] - {file.Name} with hash = {file.Hash}");
            return true;
        }

    }
}
