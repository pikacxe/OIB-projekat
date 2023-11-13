using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IClient
    {
        void AddFile(IFile file);
        void UpdateFile(IFile file, string old_filename);
    }
}
