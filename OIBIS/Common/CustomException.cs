using System.Runtime.Serialization;
using System.ServiceModel;

namespace Common
{
    [DataContract]
    public class CustomException : FaultException
    {
        private string message;

        public CustomException(string message)
        {
            this.message = message;
        }
        [DataMember]
        public string FaultMessage { get => message;}
    }
}
