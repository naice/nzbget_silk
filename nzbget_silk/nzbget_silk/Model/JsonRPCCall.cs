using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk.Model
{
    public class JsonRPCCall
    {
        public string jsonrpc { get; private set; } = "2.0";
        public string method { get; set; }
        // No named params!
        public object[] @params { get; set; }
        public string id { get; private set; } = Guid.NewGuid().ToString().Replace("-", "");

        public JsonRPCCall(string method, params object[] args)
        {
            this.method = method;
            this.@params = args;
        }
    }
}
