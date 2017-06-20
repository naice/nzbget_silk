using nzbget_silk.Model;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace nzbget_silk.ViewModel
{
    public class MainPageViewModel : PageViewModel
    {
        private bool _IsNameVisible;
        public bool IsNameVisible
        {
            get { return _IsNameVisible; }
            set
            {
                if (value != _IsNameVisible)
                {
                    _IsNameVisible = value;
                    RaisePropertyChanged("IsNameVisible");
                }
            }
        }
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        private string _ServerDomain;
        public string ServerDomain
        {
            get { return _ServerDomain; }
            set
            {
                if (value != _ServerDomain)
                {
                    _ServerDomain = value;
                    RaisePropertyChanged("ServerDomain");
                }
            }
        }
        private double _TotalDownloadProgress;
        public double TotalDownloadProgress
        {
            get { return _TotalDownloadProgress; }
            set
            {
                if (value != _TotalDownloadProgress)
                {
                    _TotalDownloadProgress = value;
                    RaisePropertyChanged("TotalDownloadProgress");
                }
            }
        }
        private string _TotalDownloadProgressText;
        public string TotalDownloadProgressText
        {
            get { return _TotalDownloadProgressText; }
            set
            {
                if (value != _TotalDownloadProgressText)
                {
                    _TotalDownloadProgressText = value;
                    RaisePropertyChanged("TotalDownloadProgressText");
                }
            }
        }
        private string _DownloadLimitText;
        public string DownloadLimitText
        {
            get { return _DownloadLimitText; }
            set
            {
                if (value != _DownloadLimitText)
                {
                    _DownloadLimitText = value;
                    RaisePropertyChanged("DownloadLimitText");
                }
            }
        }
        private string _DownloadSpeedText;
        public string DownloadSpeedText
        {
            get { return _DownloadSpeedText; }
            set
            {
                if (value != _DownloadSpeedText)
                {
                    _DownloadSpeedText = value;
                    RaisePropertyChanged("DownloadSpeedText");
                }
            }
        }
        private string _FreeDiskSpaceText;
        public string FreeDiskSpaceText
        {
            get { return _FreeDiskSpaceText; }
            set
            {
                if (value != _FreeDiskSpaceText)
                {
                    _FreeDiskSpaceText = value;
                    RaisePropertyChanged("FreeDiskSpaceText");
                }
            }
        }
        public ObservableCollection<NZBGroupViewModel> Groups { get; private set; } = new ObservableCollection<NZBGroupViewModel>();
        
        private string _ToggleDownloadButtonText;
        public string ToggleDownloadButtonText
        {
            get { return _ToggleDownloadButtonText; }
            set
            {
                if (value != _ToggleDownloadButtonText)
                {
                    _ToggleDownloadButtonText = value;
                    RaisePropertyChanged("ToggleDownloadButtonText");
                }
            }
        }
        private RelayCommand _ToggleDownloadButtonCommand;
        public RelayCommand ToggleDownloadButtonCommand
        {
            get { return _ToggleDownloadButtonCommand; }
            set
            {
                if (value != _ToggleDownloadButtonCommand)
                {
                    _ToggleDownloadButtonCommand = value;
                    RaisePropertyChanged(nameof(ToggleDownloadButtonCommand));
                }
            }
        }
        private bool _IsToggleDownloadButtonEnabled = true;
        public bool IsToggleDownloadButtonEnabled
        {
            get { return _IsToggleDownloadButtonEnabled; }
            set
            {
                if (value != _IsToggleDownloadButtonEnabled)
                {
                    _IsToggleDownloadButtonEnabled = value;
                    RaisePropertyChanged("IsToggleDownloadButtonEnabled");
                }
            }
        }

        private readonly NZBGetServer _server;
        private bool _isUpdateEnabled;
        private bool _isDownloading = false;

        public MainPageViewModel(NZBGetServer server)
        {
            Title = "Server";// server.Domain;
            ServerDomain =  $"{server.Domain}:{server.Port}";
            Name = server.Name;
            IsNameVisible = !string.IsNullOrWhiteSpace(Name);

            _server = server;

            ToggleDownloadButtonCommand = new RelayCommand(
                ()=> ToggleDownloadButtonExecute());
        }

        private async void ToggleDownloadButtonExecute()
        {
            ToggleDownloadButtonCommand.IsEnabled = false;

            var service = new Service.NZBGetService(_server);

            if (_isDownloading)
                await service.PauseDownload();
            else
                await service.ResumeDownload();

            ToggleDownloadButtonCommand.IsEnabled = true;
        }

        public void StartUpdate()
        {
            if (_isUpdateEnabled) return;
            _isUpdateEnabled = true;

            Task.Run(async () => {

                while (_isUpdateEnabled)
                {
                    var service = new Service.NZBGetService(_server);
                    var status = await service.Status().ConfigureAwait(true);

                    if (status != null && status.result != null)
                        OnUI(() => Update(status.result));

                    var groups = await service.ListGroups().ConfigureAwait(true);

                    if (groups != null && groups.result != null)
                        OnUI(()=>Update(groups.result));

                    await Task.Delay(2000);
                }
            });
        }

        private void OnUI(Action action)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(action);
        }

        private void Update(List<JsonRPCGroup> groups)
        {
            foreach (var newGroup in groups)
            {
                var group = this.Groups.FirstOrDefault(g => g.NZBID == newGroup.NZBID);
                if (group == null)
                {
                    this.Groups.Add(NZBGroupViewModel.Create(newGroup, _server));
                }
                else
                {
                    group.Update(newGroup);
                }
            }
        }
        
        public void Update(JsonRPCStatus status)
        {
            long freeDiskSpace = Tools.DoubleInt2Long((int)status.FreeDiskSpaceLo, (int)status.FreeDiskSpaceHi);
            long downloadedSize = Tools.DoubleInt2Long((int)status.DownloadedSizeLo, (int)status.DownloadedSizeHi);
            long remainingSize = Tools.DoubleInt2Long((int)status.RemainingSizeLo, (int)status.RemainingSizeHi);
            long totalDownloadedSize = downloadedSize + remainingSize;
            
            TotalDownloadProgress = (double)downloadedSize / (double)totalDownloadedSize;
            TotalDownloadProgressText = 
                Tools.GetBytesReadable(downloadedSize) + 
                " / " + 
                Tools.GetBytesReadable(totalDownloadedSize);
            DownloadSpeedText = Tools.GetBytesReadable(status.DownloadRate) + "/s";

            if (status.DownloadLimit > 0)
                DownloadLimitText = Tools.GetBytesReadable(status.DownloadLimit) + "/s";
            else
                DownloadLimitText = "no Limit";

            FreeDiskSpaceText = Tools.GetBytesReadable(freeDiskSpace);

            _isDownloading = status.DownloadPaused == false;
            ToggleDownloadButtonText = _isDownloading ? "Pause Download" : "Start Download";
        }

        public void StopUpdate()
        {
            _isUpdateEnabled = false;
        }
    }
}