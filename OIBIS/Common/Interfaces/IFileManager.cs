using System.ServiceModel;
using System.Xml.Linq;

namespace Common
{
    [ServiceContract]
    public interface IFileManager
    {
        [OperationContract]
        [FaultContract(typeof(CustomException))]
        bool RequestRemoval(string fileName);
        void UpdateConfig(XDocument config);
        string CalculateChecksum(IFile file);
        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void AddFile(IFile newFile);
        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void UpdateFile(IFile updatedFile);
        void DeleteFile(string fileName);
    }
}
