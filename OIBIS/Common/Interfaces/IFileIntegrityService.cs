using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    [ServiceKnownType(typeof(MonitoredFile))]
    public interface IFileIntegrityService
    {
        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void AddFile(IFile file);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void UpdateFile(IFile file);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void RemoveFile(string fileName);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        IFile ReadFile(string fileName);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        List<string> ReadFileNames();
    }
}
