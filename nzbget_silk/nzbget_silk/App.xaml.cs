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
        public NcodedXMobile.Model.Storage<Model.StorageAppSettings> GlobalStorage =>
            new NcodedXMobile.Model.Storage<Model.StorageAppSettings>("config");
        public NcodedXMobile.ViewModel.Navigator Navigator { private set; get; }

        private Model.NZBFile _nzbFile = null;
        private bool _isInitAsyncDone = false;

        public App()
        {
            // setup default dependencys for NcodedXMoblie Framework.
            NcodedXMobile.Configuration.Begin()
                .Set(new StorageIO())
                .Set(new JsonConvert());

            InitializeComponent();

            var startPage = new StartPage();
            MainPage = new NavigationPage(startPage);
            Navigator = new NcodedXMobile.ViewModel.Navigator(MainPage.Navigation);

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
                            await Navigator.Push<SendNZBPage, ViewModel.SendNZBViewModel>(
                                _nzbFile.FileName, _nzbFile.FileContent);
                        });

                        isProcessingStartupNZBFile = false;
                    }
                    await Task.Delay(50);
                }
            });
        }
        
        async void OnFirstNewServerCreated(Model.NZBGetServer server)
        {
            await Navigator.Push<MainPage, ViewModel.MainPageViewModel>(server);
            _isInitAsyncDone = true;
        }
        async void InitializeAsync(Page startPage)
        {
            // Navigate to a apropriate landing page after launch.
            Model.NZBGetServer server = null;
            await GlobalStorage.Perform((data) => server = data.CurrentServer);

            if (server == null)
            {
                // connect to server.
                Action<Model.NZBGetServer> onFirstNewServerCreated = OnFirstNewServerCreated;
                await Navigator.Push<AddServerPage, ViewModel.AddServerViewModel>(
                    true, null, onFirstNewServerCreated);
            }
            else
            {
                // show mainpage
                await Navigator.Push<MainPage, ViewModel.MainPageViewModel>(server);
                _isInitAsyncDone = true;
            }

            // remove the startPage from Navigation stack.
            Navigator.Remove(startPage);
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
