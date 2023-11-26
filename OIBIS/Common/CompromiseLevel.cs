using System.Runtime.Serialization;

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
