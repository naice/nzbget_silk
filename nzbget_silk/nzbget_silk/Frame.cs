using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nzbget_silk
{
    class Frame : NavigationPage
    {
        public Frame(Page root) : base(root)
        {
            BarBackgroundColor = ColorSet.Shade50;
            BarTextColor = ColorSet.PrimaryText;
        }
    }
}
