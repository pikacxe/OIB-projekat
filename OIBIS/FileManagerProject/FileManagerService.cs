using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.ServiceModel;

namespace FileManagerProject
{
    public class FileManagerService : IFileManager, IClient
    {
        private ChannelFactory<IFileIntegrityService> channel;
        private IFileIntegrityService proxy;

        [PrincipalPermission(SecurityAction.Demand, Role = "OIBIS_Management")]
        [ServiceKnownType(typeof(MonitoredFile))]
        public void AddFile(IFile file)
        {
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

        public IFile ReadFile(string fileName)
        {
            try
            {
                channel = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
                proxy = channel.CreateChannel();
                return proxy.ReadFile(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                channel.Close();
            }
            return new MonitoredFile();
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

        [PrincipalPermission(SecurityAction.Demand, Role = "OIBIS_Administrator")]
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
        [OperationBehavior(AutoDisposeParameters = true)]
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
