using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileManagerProject
{
    public class FileManagerService : IFileManager
    {
        public bool RequestRemoval(string fileName)
        {
            Console.WriteLine(fileName);
            return true;
        }
    }
}
