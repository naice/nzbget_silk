using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ScrollView), typeof(nzbget_silk.UWP.CustomRenderer.ScrollViewDisableRenderer))]

namespace nzbget_silk.UWP.CustomRenderer
{
    public class ScrollViewDisableRenderer : ScrollViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Control.HorizontalScrollBarVisibility = Windows.UI.Xaml.Controls.ScrollBarVisibility.Hidden;
            Control.VerticalScrollBarVisibility = Windows.UI.Xaml.Controls.ScrollBarVisibility.Hidden;
        }
    }
}
