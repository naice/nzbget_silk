using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nzbget_silk.Model
{
    public class StorageAppSettings
    {
        public bool IsFirstStart { get; set; }
        public List<NZBGetServer> Servers { get; set; }
        public NZBGetServer CurrentServer { get; set; }

        public StorageAppSettings()
        {
            IsFirstStart = false;
            Servers = new List<NZBGetServer>();
        }
    }
}
