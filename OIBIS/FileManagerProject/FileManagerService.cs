using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Configuration;
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

        [PrincipalPermission(SecurityAction.Demand, Role = "Management")]
        public void AddFile(IFile file)
        {
            file.Hash = CalculateChecksum(file);
            try
            {
                channel = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
                proxy = channel.CreateChannel();
                proxy.AddFile(file);
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

        public List<string> ReadFiles()
        {
            try
            {
                channel = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
                proxy = channel.CreateChannel();
                return proxy.ReadFileNames();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                channel.Close();
            }

            return Enumerable.Empty<string>().ToList();
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

        [PrincipalPermission(SecurityAction.Demand, Role = "Administration")]
        public bool RequestRemoval(string fileName)
        {
            try
            {
                channel = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
                proxy = channel.CreateChannel();
                proxy.RemoveFile(fileName);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                channel.Close();
            }

            return false;
        }

        public void UpdateFile(IFile file, string old_filename)
        {
            try
            {
                channel = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
                proxy = channel.CreateChannel();
                proxy.UpdateFile(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                channel.Close();
            }
        }
    }
}
