using System;
using System.Security.Cryptography;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IIntrusionPreventionSystem
    {
        [OperationContract]
        [FaultContract(typeof(CustomException))]
        void LogIntrusion(string data, string secret_key);
    }
}
