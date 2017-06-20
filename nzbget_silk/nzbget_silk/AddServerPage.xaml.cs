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
        public ViewModel.AddServerViewModel ViewModel => BindingContext as ViewModel.AddServerViewModel;

        private readonly Action<Model.NZBGetServer> _promise;

        public AddServerPage(bool forceCreation, Model.NZBGetServer server = null, Action<Model.NZBGetServer> promise = null)
        {
            _promise = promise;

            InitializeComponent();

            BindingContext = new ViewModel.AddServerViewModel(forceCreation, server, promise);
        }
    }
}