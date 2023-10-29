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
        [FaultContract(typeof(CustomException))]
        bool ConfigChanged();
        void LoadConfig(string path);
        bool StartMonitoring();

    }
}
