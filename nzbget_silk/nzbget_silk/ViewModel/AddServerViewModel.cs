using NcodedXMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nzbget_silk.ViewModel
{
    public class AddServerViewModel : PageViewModel
    {
        private string _Domain;
        public string Domain
        {
            get { return _Domain; }
            set
            {
                if (value != _Domain)
                {
                    _Domain = value;
                    RaisePropertyChanged("Domain");
                }
            }
        }
        private int _Port = 6789;
        public int Port
        {
            get { return _Port; }
            set
            {
                if (value != _Port)
                {
                    _Port = value;
                    RaisePropertyChanged("Port");
                }
            }
        } 
        private string _Username;
        public string Username
        {
            get { return _Username; }
            set
            {
                if (value != _Username)
                {
                    _Username = value;
                    RaisePropertyChanged("Username");
                }
            }
        }
        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                if (value != _Password)
                {
                    _Password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }


        private bool _IsAbortButtonVisible;
        public bool IsAbortButtonVisible
        {
            get { return _IsAbortButtonVisible; }
            set
            {
                if (value != _IsAbortButtonVisible)
                {
                    _IsAbortButtonVisible = value;
                    RaisePropertyChanged("IsAbortButtonVisible");
                }
            }
        }
        private bool _IsPasswordView = true;
        public bool IsPasswordView
        {
            get { return _IsPasswordView; }
            set
            {
                if (value != _IsPasswordView)
                {
                    _IsPasswordView = value;
                    RaisePropertyChanged("IsPasswordView");
                }
            }
        }
        private bool _IsConnectButtonEnabled = true;
        public bool IsConnectButtonEnabled
        {
            get { return _IsConnectButtonEnabled; }
            set
            {
                if (value != _IsConnectButtonEnabled)
                {
                    _IsConnectButtonEnabled = value;
                    RaisePropertyChanged("IsConnectButtonEnabled");
                }
            }
        }
        private RelayCommand _ConnectCommand;
        public RelayCommand ConnectCommand
        {
            get { return _ConnectCommand; }
            set
            {
                if (value != _ConnectCommand)
                {
                    _ConnectCommand = value;
                    RaisePropertyChanged(nameof(ConnectCommand));
                }
            }
        }
        private RelayCommand _AbortCommand;
        public RelayCommand AbortCommand
        {
            get { return _AbortCommand; }
            set
            {
                if (value != _AbortCommand)
                {
                    _AbortCommand = value;
                    RaisePropertyChanged(nameof(AbortCommand));
                }
            }
        }

        private Model.NZBGetServer CreateServer => new Model.NZBGetServer()
        {
            Domain = Domain, Password = Password, Port = Port, Username = Username,
        };

        private readonly Model.NZBGetServer _server;
        private readonly Action<Model.NZBGetServer> _serverCreatedCallback;

        public AddServerViewModel(bool forceCreate, Model.NZBGetServer server, Action<Model.NZBGetServer> serverCreatedCallback)
        {
            Title = "Connect";

            IsAbortButtonVisible = forceCreate == false;
            _server = server;
            _serverCreatedCallback = serverCreatedCallback;

            if (server != null)
            {
                Domain = server.Domain;
                Port = server.Port;
                Username = server.Username;
                Password = server.Password;
            }
        }

        public async Task<bool> Connect()
        {
            Service.NZBGetService service = new Service.NZBGetService(CreateServer);
            var version = await service.Version();

            return version != null;
        }

        public async Task<bool> Save()
        {
            try
            {
                if (await Connect())
                {
                    await App.CurrentApp.GlobalStorage.Perform((data) => data.CurrentServer = CreateServer, save: true);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return false;
        }
    }
}
