using CertificationManager;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;

namespace ManagerClients
{
    internal class Program
    {
        private static ChannelFactory<IClient> cf;
        private static IClient proxy;
        static void Main(string[] args)
        {
            cf = new ChannelFactory<IClient>("Client");
            proxy = cf.CreateChannel();
            string key = string.Empty;
            do
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
            } while (key != "Q");
            if(cf != null)
            {
                cf.Close();
            }
        }

        static void AddFile()
        {
            try
            {
                Console.Write("Enter file name: ");
                string filename = Console.ReadLine();
                ConsoleFileEditor editor = new ConsoleFileEditor();
                editor.Edit();
                IFile file = editor.SaveToFile(filename);
                Console.WriteLine($"{filename} created successfully...");
                proxy.AddFile(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void UpdateFile()
        {
            try
            {
                Console.WriteLine("--------------- Available files ---------------");
                List<string> fileNames = proxy.ReadFiles();
                foreach (var x in fileNames)
                {
                    Console.WriteLine($"- {x}");
                }
                string filename = string.Empty;
                do
                {
                    Console.Write("\nSelect file: ");
                    filename = Console.ReadLine();
                } while (!fileNames.Contains(filename));

                Console.WriteLine("Not implemented fully");
                IFile f = proxy.ReadFile(filename);
                byte[] data = f.File.ToArray();
                ConsoleFileEditor editor = new ConsoleFileEditor(Encoding.ASCII.GetString(data).Split('\n'));
                editor.Edit();
                f = editor.SaveToFile(f.Name);
                proxy.UpdateFile(f, "");
                Console.WriteLine($"{filename} updated successfully...");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
