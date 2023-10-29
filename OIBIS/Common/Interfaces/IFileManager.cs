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
        string CalculateChecksum(IFile file);
        void AddFile();
        void UpdateFile();
        void DeleteFile(string fileName);
    }
}
