using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IIntrusionPreventionSystem
    {
        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void LogIntrusion(DateTime timeStamp,string fileName,string location,CompromiseLevel compromiseLevel);
    }
}
