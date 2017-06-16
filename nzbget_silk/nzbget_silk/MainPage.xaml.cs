using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nzbget_silk
{
    public partial class MainPage : ContentPage
    {
        public MainPage(Model.NZBGetServer server)
        {
            InitializeComponent();

            BindingContext = new ViewModel.MainPageViewModel(server);
        }
        public ViewModel.MainPageViewModel ViewModel => BindingContext as ViewModel.MainPageViewModel;

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            ViewModel.StartUpdate();
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            ViewModel.StopUpdate();
        }
    }
}
