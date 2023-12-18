using System.Runtime.Serialization;
using System.ServiceModel;

namespace Common
{
    [DataContract]
    public class CustomException : FaultException
    {

        public CustomException(string message): base(message)
        {
        }
        [DataMember]
        public string FaultMessage { get; set; }
    }
}
