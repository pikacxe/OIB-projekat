
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IFileManager
    {
        [OperationContract]
        bool RequestRemoval(string fileName);
        void UpdateConfig();
        string CalculateChecksum();
        void AddFile(string fileName);
        void UpdateFile(string fileName);
        void DeleteFile(string fileName);
    }
}
