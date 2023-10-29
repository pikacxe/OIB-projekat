using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IFile: IDisposable
    {
        MemoryStream File { get; set; }
        string Name { get; set; }
        string Hash { get; set; }
    }
}
