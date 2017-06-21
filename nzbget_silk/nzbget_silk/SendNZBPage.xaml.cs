using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nzbget_silk
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendNZBPage : ContentPage
    {
        ViewModel.SendNZBViewModel ViewModel { get { return BindingContext as ViewModel.SendNZBViewModel; } }

        public SendNZBPage()
        {
            InitializeComponent();
        }
    }
}