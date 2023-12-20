using System;
using System.Runtime.Serialization;

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
