using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Intrusion
    {
        [DataMember]
        public DateTime TimeStamp { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public CompromiseLevel CompromiseLevel { get; set; }
    }
}
