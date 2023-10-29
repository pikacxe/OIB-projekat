using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileManagerProject
{
    public class FileManagerService : IFileManager
    {
        private ChannelFactory<IFileIntegrityService> cf;
        private IFileIntegrityService fileIntegrityService;

        public FileManagerService()
        {
            cf = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
            fileIntegrityService = cf.CreateChannel();
        }

        public void AddFile(IFile newFile)
        {
            throw new NotImplementedException();
        }

        public string CalculateChecksum(IFile file)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool RequestRemoval(string fileName)
        {
            throw new NotImplementedException();
        }

        public void UpdateConfig(XDocument config)
        {
            throw new NotImplementedException();
        }

        public void UpdateFile(IFile updatedFile)
        {
            throw new NotImplementedException();
        }
    }
}
