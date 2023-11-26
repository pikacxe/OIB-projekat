using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IFileManager
    {
        [OperationContract]
        [FaultContract(typeof(CustomException))]
        bool RequestRemoval(string fileName);
    }
}
