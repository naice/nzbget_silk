using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nzbget_silk
{
    public class Tools
    {
        public static long DoubleInt2Long(int a1, int a2)
        {
            long b = a2;
            b = b << 32;
            b = b | (uint)a1;
            return b;
        }
    }
}
