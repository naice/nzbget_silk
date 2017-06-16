using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk.Model
{
    public class NZBGetServer
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public int Port { get; set; } = 6789;
        public string Username { get; set; }
        public string Password { get; set; }

        public string GetURL(string function)
        {
            return GetURL() + function;
        }
        public string GetURL()
        {
            return "http://" + $"{Domain}:{Port}/{Username}:{Password}/jsonrpc/";
        }
    }
}
