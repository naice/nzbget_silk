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

        public SendNZBPage(string fileName, byte[] fileContent)
        {
            InitializeComponent();

            BindingContext = new ViewModel.SendNZBViewModel(fileName, fileContent);
        }

        private async void Send_Clicked(object sender, EventArgs e)
        {
            await ViewModel.Send();

            Navigation.RemovePage(this);
        }
    }
}