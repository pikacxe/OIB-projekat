using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
