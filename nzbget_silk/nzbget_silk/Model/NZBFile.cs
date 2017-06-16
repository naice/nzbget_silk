using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk.Model
{
    public class NZBFile
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}
