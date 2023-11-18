using CertificationManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ManagerClients
{
    internal class Program
    {
        static void Main(string[] args)
        {

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
        }

        static void AddFile()
        {
            Console.Write("Enter file name: ");
            string filename = Console.ReadLine();
            ConsoleFileEditor editor = new ConsoleFileEditor();
            editor.Edit();
            editor.SaveToFile(filename);
            Console.WriteLine($"{filename} created successfully...");
        }

        static void UpdateFile()
        {
            DirectoryInfo di = new DirectoryInfo(".");
            Console.WriteLine("\n----  Available files  ----\n");
            foreach (FileInfo fi in di.GetFiles())
            {
                Console.WriteLine($"  -\t{fi.Name}\n");
            }
            string filename = string.Empty;
            do
            {
                Console.Write("\nSelect file: ");
                filename = Console.ReadLine();
            } while (!File.Exists(filename));

            ConsoleFileEditor editor = new ConsoleFileEditor(File.ReadAllLines(filename));
            editor.Edit();
            editor.SaveToFile(filename);
            Console.WriteLine($"{filename} updated successfully...");
        }
    }
}
