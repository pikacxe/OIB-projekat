using System.Runtime.Serialization;
using System.ServiceModel;

namespace Common
{
    [DataContract]
    public class CustomException
    {
        [DataMember]
        public string FaultMessage { get; set; }
    }
}
