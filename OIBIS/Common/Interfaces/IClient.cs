using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IClient
    {
        [OperationContract]
        void AddFile(IFile file);
        [OperationContract]
        void UpdateFile(IFile file, string old_filename);
        [OperationContract]
        List<string> ReadFiles();
    }
}
