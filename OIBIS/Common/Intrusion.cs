using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Intrusion
    {
        public DateTime TimeStamp { get; set; }
        public string FileName { get; set; }
        public string Location { get; set; }
        public CompromiseLevel CompromiseLevel { get; set; }
    }
}
