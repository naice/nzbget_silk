using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nzbget_silk
{
    static class ColorSet
    {
        public static Color Shade25 { get { return GetRessource<Color>(); } }
        public static Color Shade50 { get { return GetRessource<Color>(); } }
        public static Color Shade75 { get { return GetRessource<Color>(); } }
        public static Color Shade100 { get { return GetRessource<Color>(); } }
        public static Color Accent1 { get { return GetRessource<Color>(); } }
        public static Color Accent2 { get { return GetRessource<Color>(); } }

        public static Color PrimaryText { get { return GetRessource<Color>(); } }

        private static T GetRessource<T>([CallerMemberName] string key = null) 
        {
            key = key ?? throw new ArgumentException(nameof(key));

            return (T)App.Current.Resources[key];
        }
    }
}
