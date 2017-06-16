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
    public partial class AddServerPage : ContentPage
    {
        public ViewModel.NZBGetServerViewModel ViewModel => BindingContext as ViewModel.NZBGetServerViewModel;

        private readonly Action<Model.NZBGetServer> _promise;

        public AddServerPage(bool forceCreation, Model.NZBGetServer server = null, Action<Model.NZBGetServer> promise = null)
        {
            _promise = promise;

            InitializeComponent();

            BindingContext = new ViewModel.NZBGetServerViewModel(forceCreation, server);
        }

        private async void Connect_Clicked(object sender, EventArgs e)
        {
            ViewModel.IsConnectButtonEnabled = false;
            if (await ViewModel.Save())
            {
                _promise(ViewModel.CreateServer);
                Navigation.RemovePage(this);
            }
            ViewModel.IsConnectButtonEnabled = true;
        }
    }
}