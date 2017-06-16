using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk.Model
{
    public class JsonRPCError
    {
        public string name { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }
}
