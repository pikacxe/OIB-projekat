using System.Collections.Generic;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    [ServiceKnownType(typeof(MonitoredFile))]
    public interface IClient
    {
        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void AddFile(IFile file);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        IFile ReadFile(string fileName);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void UpdateFile(IFile file);

        [OperationContract]
        [FaultContract(typeof(CustomException))]
        List<string> ReadFiles();
    }
}
