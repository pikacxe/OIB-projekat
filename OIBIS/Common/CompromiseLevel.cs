using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public enum CompromiseLevel
    {
        [EnumMember]
        Info = 1,
        [EnumMember]
        Warning = 2,
        [EnumMember]
        Critical = 3
    };
}
