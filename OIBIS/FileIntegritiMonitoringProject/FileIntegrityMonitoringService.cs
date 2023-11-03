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
        public bool ConfigChanged(IFile file)
        {
            // TODO update config and reset monitoring
            return true;
        }

    }
}
