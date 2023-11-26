using System;
using System.ServiceModel;

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
