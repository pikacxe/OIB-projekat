using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IntrusionPreventionSystemProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(IntrusionPreventionService)))
            {
                host.Open();
                Console.WriteLine("Intrusion prevention service started. Press Esc to exit...");
                while (Console.ReadKey(intercept: true).Key != ConsoleKey.Escape) ;
                host.Close();
            }
        }
    }
}
