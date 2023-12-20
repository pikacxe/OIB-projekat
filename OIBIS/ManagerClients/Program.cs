using CertificationManager;
using Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;

namespace ManagerClients
{
    internal class Program
    {
        private static ChannelFactory<IClient> cf;
        private static IClient proxy;
        static void Main(string[] args)
        {
            try
            {
                /// Create a client proxy
                cf = new ChannelFactory<IClient>("Client");
                proxy = cf.CreateChannel();
                /// User menu
                string key = string.Empty;
                do
                {
                    try
                    {
                        // Print username of a user who is running a service
                        Formatter.PrintCurrentUser();
                        Console.Write("---------------------\nOptions:\n\tA - Add file\n\tU - update file\n\tQ - Quit process\n\nPick: ");
                        key = Console.ReadLine().ToUpper();
                        switch (key)
                        {
                            case "A": AddFile(); break;
                            case "U": UpdateFile(); break;
                            default: break;
                        }
                    }
                    catch (FaultException<CustomException> fe)
                    {
                        CustomConsole.WriteLine(fe.Detail.FaultMessage, MessageType.Error);
                        if (cf.State == CommunicationState.Faulted)
                        {
                            cf = new ChannelFactory<IClient>("Client");
                            proxy = cf.CreateChannel();
                        }
                    }
                    catch (Exception e)
                    {
                        CustomConsole.WriteLine(e.Message, MessageType.Error);
                        if (cf.State == CommunicationState.Faulted)
                        {
                            cf = new ChannelFactory<IClient>("Client");
                            proxy = cf.CreateChannel();
                        }
                    }
                } while (key != "Q");
            }
            catch (Exception e)
            {
                CustomConsole.WriteLine(e.Message, MessageType.Error);
            }
            finally
            {
                /// Close channel on exit
                if (cf != null)
                {
                    if (cf.State == CommunicationState.Faulted)
                    {
                        cf.Abort();
                    }
                    else
                    {
                        cf.Close();
                    }
                }
            }

        }

        static void AddFile()
        {

            Console.Write("Enter file name: ");
            string filename = Console.ReadLine();
            if (filename == "exit")
            {
                return;
            }
            /// Open empty file in console editor
            ConsoleFileEditor editor = new ConsoleFileEditor();
            editor.Edit();

            /// Save data from console editor to file
            IFile file = editor.SaveToFile(filename);
            Console.WriteLine($"{filename} created successfully...");

            /// forward file to service
            proxy.AddFile(file);
        }

        static void UpdateFile()
        {

            /// Read available files on service
            Console.WriteLine("--------------- Available files ---------------");
            List<string> fileNames = proxy.ReadFiles();
            foreach (var x in fileNames)
            {
                Console.WriteLine($"- {x}");
            }
            string filename = string.Empty;

            Regex r = new Regex(@".+\.[a-zA-Z]+$");
            /// Select file for editing
            do
            {
                Console.Write("\nSelect file: ");
                filename = Console.ReadLine();
                if (filename == "exit")
                {
                    return;
                }
                /// Add default extenstion if none is specified
                if (!r.IsMatch(filename))
                {
                    filename += ".txt";
                }
            } while (!fileNames.Contains(filename));

            /// Get file from service
            IFile f = proxy.ReadFile(filename);

            /// Read data from file in console editor
            byte[] data = f.File.ToArray();
            ConsoleFileEditor editor = new ConsoleFileEditor(Encoding.UTF8.GetString(data).Split('\n'));
            editor.Edit();

            /// Save updated file
            f = editor.SaveToFile(f.Name);

            /// Forward updated file to service
            proxy.UpdateFile(f);
        }
    }
}
