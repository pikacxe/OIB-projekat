using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace FileManagerProject
{
    internal class Program
    {
        public static void Meni(object sender, EventArgs e)
        {
            Console.WriteLine("Test");
        }
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(FileManagerService)))
            {
                host.Opening += Meni;
                host.Open();
                Console.WriteLine("File manager service started. Press Esc to exit...");
                while (Console.ReadKey(intercept: true).Key != ConsoleKey.Escape) ;
                host.Close();
            }
        }
    }
}
