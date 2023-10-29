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
        Info = 0,
        Warning = 1,
        Critical = 2
    };
}
