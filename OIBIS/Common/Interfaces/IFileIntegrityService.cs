using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IFileIntegrityService
    {
        [OperationContract]
        void AddFile(IFile file);

        [OperationContract]
        void UpdateFile(IFile file);

        [OperationContract]
        void RemoveFile(string fileName);

        [OperationContract]
        IFile ReadFile(string fileName);

        [OperationContract]
        List<string> ReadFileNames();
    }
}
