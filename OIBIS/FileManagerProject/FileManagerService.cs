using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileManagerProject
{
    public class FileManagerService : IFileManager, IClient
    {

        private ChannelFactory<IFileIntegrityService> channel;
        private IFileIntegrityService proxy;

        public void AddFile(IFile file)
        {
            file.Hash = CalculateChecksum(file);
            try
            {
                channel = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
                proxy = channel.CreateChannel();
                proxy.ConfigChanged(file);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                channel.Close();
            }
        }

        public void ReadFiles()
        {
            throw new NotImplementedException();
        }

        public string CalculateChecksum(IFile filePath)
        {
            byte[] checksum;
            using (var stream = filePath.File)
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                checksum = sha1.ComputeHash(stream);
            }

            //iz niza bajtova pretvaramo u string
            return BitConverter.ToString(checksum).Replace("-", string.Empty);
        }

        public bool RequestRemoval(string fileName)
        {
            //TODO 
            return false;
        }

        public void UpdateFile(IFile file, string old_filename)
        {
            throw new NotImplementedException();
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
