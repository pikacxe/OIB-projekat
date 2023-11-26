using System;
using System.IO;

namespace Common
{
    public interface IFile: IDisposable
    {
        MemoryStream File { get; set; }
        string Name { get; set; }
        string Hash { get; set; }
    }
}
