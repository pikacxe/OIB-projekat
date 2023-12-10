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
                CustomConsole.WriteLine($"File " + file.Name + " successfully added.", MessageType.Success);
            }
            catch (FaultException<CustomException> fe)
            {
                CustomConsole.WriteLine(fe.Detail.Message, MessageType.Error);
                throw fe;
            }
            catch (Exception e)
            {
                CustomConsole.WriteLine(e.Message, MessageType.Error);
            }
            finally
            {
                // Ensure the channel is properly closed
                if (channel.State == CommunicationState.Faulted)
                {
                    channel.Abort();
                }
                else
                {
                    channel.Close();
                }
            }
        }

        public IFile ReadFile(string fileName)
        {
            try
            {
                channel = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
                proxy = channel.CreateChannel();
                var x = proxy.ReadFile(fileName);
                CustomConsole.WriteLine($"File {fileName} was read successfully.", MessageType.Success);
                return x;
            }
            catch (FaultException<CustomException> fe)
            {
                CustomConsole.WriteLine(fe.Detail.Message, MessageType.Error);
                throw fe;
            }
            catch (Exception e)
            {
                CustomConsole.WriteLine(e.Message, MessageType.Error);
            }
            finally
            {
                // Ensure the channel is properly closed
                if (channel.State == CommunicationState.Faulted)
                {
                    channel.Abort();
                }
                else
                {
                    channel.Close();
                }
            }
            return new MonitoredFile();
        }

        public List<string> ReadFiles()
        {
            try
            {
                channel = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
                proxy = channel.CreateChannel();
                var x = proxy.ReadFileNames();

                CustomConsole.WriteLine($"Read {x.Count} files from service.", MessageType.Success);

                return x;
            }
            catch (FaultException<CustomException> fe)
            {
                CustomConsole.WriteLine(fe.Detail.Message, MessageType.Error);
                throw fe;
            }
            catch (Exception e)
            {
                CustomConsole.WriteLine(e.Message, MessageType.Error);
            }
            finally
            {
                // Ensure the channel is properly closed
                if (channel.State == CommunicationState.Faulted)
                {
                    channel.Abort();
                }
                else
                {
                    channel.Close();
                }
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
                CustomConsole.WriteLine($"Removal of {fileName} successfully requested.", MessageType.Success);
                return true;
            }
            catch (FaultException<CustomException> fe)
            {
                CustomConsole.WriteLine(fe.Detail.Message, MessageType.Error);
                throw fe;
            }
            catch (Exception e)
            {
                CustomConsole.WriteLine(e.Message, MessageType.Error);
            }
            finally
            {
                // Ensure the channel is properly closed
                if (channel.State == CommunicationState.Faulted)
                {
                    channel.Abort();
                }
                else
                {
                    channel.Close();
                }
            }
            CustomConsole.WriteLine($"Removal of {fileName} not successfully requested !", MessageType.Warning);
            return false;
        }
        [OperationBehavior(AutoDisposeParameters = true)]
        public void UpdateFile(IFile file)
        {
            try
            {
                channel = new ChannelFactory<IFileIntegrityService>("IFileMonitoring");
                proxy = channel.CreateChannel();
                proxy.UpdateFile(file);
                CustomConsole.WriteLine($"File {file.Name} successfully updated.", MessageType.Success);
            }
            catch (FaultException<CustomException> fe)
            {
                CustomConsole.WriteLine(fe.Detail.Message, MessageType.Error);
                throw fe;
            }
            catch (Exception e)
            {
                CustomConsole.WriteLine(e.Message, MessageType.Error);
            }
            finally
            {
                // Ensure the channel is properly closed
                if (channel.State == CommunicationState.Faulted)
                {
                    channel.Abort();
                }
                else
                {
                    channel.Close();
                }
            }
        }
    }
}
