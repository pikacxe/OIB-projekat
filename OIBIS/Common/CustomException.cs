using System.Runtime.Serialization;
using System.ServiceModel;

namespace Common
{
    [DataContract]
    public class CustomException: FaultException
    {
        private string message;
        [DataMember]
        public string FaultMessage { get => message;set => message = value; }
    }
}
