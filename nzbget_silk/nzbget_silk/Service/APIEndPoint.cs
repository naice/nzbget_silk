using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk.Service
{
    public class APIEndPoint
    {
        public static bool DEBUG = false;

        private readonly string productiveEndPoint;
        private readonly string debugEndPoint;

        public string EndPoint { get { return DEBUG ? debugEndPoint : productiveEndPoint; } }

        public APIEndPoint(string productiveEndPoint, string debugEndPoint)
        {
            this.productiveEndPoint = productiveEndPoint;
            this.debugEndPoint = debugEndPoint;
        }
    }
}
