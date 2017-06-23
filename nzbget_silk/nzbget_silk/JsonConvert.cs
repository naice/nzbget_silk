using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk
{
    class JsonConvert : NcodedXMobile.Converter.IJsonConvert
    {
        public T DeserializeObject<T>(string jsonString) where T : class
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
        }

        public string SerializeObject(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
