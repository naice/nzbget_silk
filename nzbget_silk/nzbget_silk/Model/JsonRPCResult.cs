using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk.Model
{
    public class JsonRPCResult<RESULT>
    {
        public string version { get; set; }
        public RESULT result { get; set; }
        public JsonRPCError error { get; set; }
    }
}
