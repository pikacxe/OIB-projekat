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
    }
}
