using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nzbget_silk
{
    public partial class App : Application
    {
        public static App CurrentApp => Current as App;
        public Model.Storage GlobalStorage => new Model.Storage("config", PCLStorage.FileSystem.Current);

        private Model.NZBFile _nzbFile = null;
        private bool _isInitAsyncDone = false;

        public App()
        {
            //_CurrentFileSystem = currentFileSystem;

            InitializeComponent();

            var startPage = new StartPage();
            MainPage = new NavigationPage(startPage);
            InitializeAsync(startPage);
        }

        public void RegisterStartupNZBFile(Model.NZBFile nzbFile)
        {
            _nzbFile = nzbFile;
            ProcessStartupNZBFile();
        }

        bool isProcessingStartupNZBFile;
        private async void ProcessStartupNZBFile()
        {
            if (isProcessingStartupNZBFile) return;
            isProcessingStartupNZBFile = true;

            await Task.Run(async() => {

                while (isProcessingStartupNZBFile)
                {
                    if (_isInitAsyncDone)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            // show upload for nzb file
                            await MainPage.Navigation.PushAsync(
                                new SendNZBPage(_nzbFile.FileName, _nzbFile.FileContent));
                        });

                        isProcessingStartupNZBFile = false;
                    }
                    await Task.Delay(50);
                }
            });
        }

        public async Task<bool> HasCurrentServer()
        {
            bool hasCurrentServer = false;

            await GlobalStorage.Perform((data) => hasCurrentServer = data.CurrentServer != null);

            return hasCurrentServer;
        }

        async void InitializeAsync(Page startPage)
        {
            // Navigate to a apropriate landing page after launch.
            Model.NZBGetServer server = null;
            await GlobalStorage.Perform((data) => server = data.CurrentServer);

            if (server == null)
            {
                // connect to server.
                await MainPage.Navigation.PushAsync(
                    new AddServerPage(true,
                        new Model.NZBGetServer() { Domain = "emmuss.ddns.net", Password = "jensm1985", Port = 6789, Username = "nzbget" },
                        async (newServer) =>
                        {
                            // this promise will be executed if a connection was made.
                            await MainPage.Navigation.PushAsync(new MainPage(newServer));
                            _isInitAsyncDone = true;
                        }));
            }
            else
            {
                // show mainpage
                await MainPage.Navigation.PushAsync(
                    new MainPage(server));

                _isInitAsyncDone = true;
            }

            // remove the startPage from Navigation stack.
            MainPage.Navigation.RemovePage(startPage);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
