using NcodedXMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nzbget_silk.ViewModel
{
    public class NZBGroupViewModel : BaseViewModel
    {
        public long NZBID { get; private set; }

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
        private string _StatusText;
        public string StatusText
        {
            get { return _StatusText; }
            set
            {
                if (value != _StatusText)
                {
                    _StatusText = value;
                    RaisePropertyChanged(nameof(StatusText));
                }
            }
        }
        private GroupStatus _Status;
        public GroupStatus Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    RaisePropertyChanged(nameof(Status));
                }
            }
        }
        private string _StatusImageSource;
        public string StatusImageSource
        {
            get { return _StatusImageSource; }
            set
            {
                if (value != _StatusImageSource)
                {
                    _StatusImageSource = value;
                    RaisePropertyChanged("StatusImageSource");
                }
            }
        }
        private double _Health;
        /// <summary>
        /// 0 - 1
        /// </summary>
        public double Health
        {
            get { return _Health; }
            set
            {
                if (value != _Health)
                {
                    _Health = value;
                    RaisePropertyChanged("Health");
                }
            }
        }
        private long _TotalSize;
        public long TotalSize
        {
            get { return _TotalSize; }
            set
            {
                if (value != _TotalSize)
                {
                    _TotalSize = value;
                    RaisePropertyChanged("TotalSize");
                }
            }
        }
        private string _TotalSizeText;
        public string TotalSizeText
        {
            get { return _TotalSizeText; }
            set
            {
                if (value != _TotalSizeText)
                {
                    _TotalSizeText = value;
                    RaisePropertyChanged("TotalSizeText");
                }
            }
        }
        private long _RemainingSize;
        public long RemainingSize
        {
            get { return _RemainingSize; }
            set
            {
                if (value != _RemainingSize)
                {
                    _RemainingSize = value;
                    RaisePropertyChanged("RemainingSize");
                }
            }
        }
        private string _RemainingSizeText;
        public string RemainingSizeText
        {
            get { return _RemainingSizeText; }
            set
            {
                if (value != _RemainingSizeText)
                {
                    _RemainingSizeText = value;
                    RaisePropertyChanged("RemainingSizeText");
                }
            }
        }
        private double _Progress;
        public double Progress
        {
            get { return _Progress; }
            set
            {
                if (value != _Progress)
                {
                    _Progress = value;
                    RaisePropertyChanged("Progress");
                }
            }
        }
        private bool _IsProgressVisible;
        public bool IsProgressVisible
        {
            get { return _IsProgressVisible; }
            set
            {
                if (value != _IsProgressVisible)
                {
                    _IsProgressVisible = value;
                    RaisePropertyChanged("IsProgressVisible");
                }
            }
        }
        private Color _HealthColor;
        public Color HealthColor
        {
            get { return _HealthColor; }
            set
            {
                if (value != _HealthColor)
                {
                    _HealthColor = value;
                    RaisePropertyChanged(nameof(HealthColor));
                }
            }
        }
        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand; }
            set
            {
                if (value != _DeleteCommand)
                {
                    _DeleteCommand = value;
                    RaisePropertyChanged(nameof(DeleteCommand));
                }
            }
        }
        private RelayCommand _MoveToTopCommand;
        public RelayCommand MoveToTopCommand
        {
            get { return _MoveToTopCommand; }
            set
            {
                if (value != _MoveToTopCommand)
                {
                    _MoveToTopCommand = value;
                    RaisePropertyChanged(nameof(MoveToTopCommand));
                }
            }
        }

        private readonly Model.NZBGetServer _server;
        private GroupStatus _status;

        public NZBGroupViewModel(long NZBID, Model.NZBGetServer server)
        {
            _server = server;
            this.NZBID = NZBID;

            DeleteCommand = new RelayCommand(() => DeleteExecute());
            MoveToTopCommand = new RelayCommand(() => MoveToTopExecute());
        }

        private async void MoveToTopExecute()
        {
            MoveToTopCommand.IsEnabled = false;

            var service = new Service.NZBGetService(_server);
            await service.EditQueue(Service.NZBGetService.EDIT_Q_GROUP_MOVETOP, "", (int)NZBID);

            MoveToTopCommand.IsEnabled = true;
        }

        private async void DeleteExecute()
        {
            DeleteCommand.IsEnabled = false;

            var service = new Service.NZBGetService(_server);
            await service.EditQueue(Service.NZBGetService.EDIT_Q_GROUP_DELETE, "", (int)NZBID);

            DeleteCommand.IsEnabled = true;
        }
        public enum GroupStatus { Repairing, Unpacking, Done, Paused, Downloading, Queued }
        private GroupStatus GetGroupStatus(string status)
        {
            switch (status)
            {
                case "LOADING_PARS":         // stage of parcheck;
                case "VERIFYING_SOURCES":    // stage of parcheck;
                case "VERIFYING_REPAIRED":   // stage of parcheck;
                case "REPAIRING":            // stage of parcheck;
                    // REPAIRING
                    return GroupStatus.Repairing;
                case "UNPACKING":            // being unpacked;
                    // UNPACKING
                    return GroupStatus.Unpacking;
                case "MOVING":               // moving files from intermediate directory into destination directory;
                case "EXECUTING_SCRIPT":     // executing post processing script;
                case "PP_QUEUED":            // queued for post  processing(completely downloaded);
                case "PP_FINISHED":          // post processing is finished, the item is about to be moved to history.
                    // DONE
                    return GroupStatus.Done;
                case "PAUSED":               // paused;
                    // PAUSED
                    return GroupStatus.Paused;
                case "DOWNLOADING":          // item is being downloaded;
                    // DOWNLOADING
                    return GroupStatus.Downloading;
                case "QUEUED":               // queued for download;
                case "FETCHING":             // nzb file is being fetched from URL (Kind = URL);
                case "RENAMING":             // processed by parrenamer;
                default:
                    // QUEUED
                    return GroupStatus.Queued;
            }
        }
        public void Update(Model.JsonRPCGroup group)
        {
            this.Name = "unknown";
            if (!string.IsNullOrEmpty(group.NZBName))
            {
                this.Name = group.NZBName.Substring(0, 32); // cut name length...
            }
            Status = GetGroupStatus(group.Status);
            StatusText = Status.ToString();

            long fileSize = Tools.DoubleInt2Long((int)group.FileSizeLo, (int)group.FileSizeHi);
            long pausedSize = Tools.DoubleInt2Long((int)group.PausedSizeLo, (int)group.PausedSizeHi);
            long remainingSize = Tools.DoubleInt2Long((int)group.RemainingSizeLo, (int)group.RemainingSizeHi);

            StatusImageSource = $"Images/ico_{StatusText.ToLower()}.png";
            var health = (double)group.Health / 1000d;
            Health = health > 1 ? 1 : health < 0 ? 0 : health;
            HealthColor = Tools.InterpolateColor(
                new Color[] { Color.Red, Color.Yellow, Color.Green }, 
                Health);
            TotalSize = fileSize - pausedSize;
            TotalSizeText = Tools.GetBytesReadable(TotalSize);
            RemainingSize = remainingSize - pausedSize;
            RemainingSizeText = Tools.GetBytesReadable(RemainingSize);
            long downloadedSize = TotalSize - RemainingSize;

            if (Status == GroupStatus.Downloading)
            {
                Progress = (double)downloadedSize / (double)TotalSize;
            }
            else if (Status == GroupStatus.Repairing || Status == GroupStatus.Unpacking)
            {
                Progress = (double)group.PostStageProgress / 1000d;
            }
            IsProgressVisible =  Status == GroupStatus.Downloading ||
                                 Status == GroupStatus.Repairing   ||
                                 Status == GroupStatus.Unpacking;
        }

        public static NZBGroupViewModel Create(Model.JsonRPCGroup group, Model.NZBGetServer server)
        {
            var m = new NZBGroupViewModel(group.NZBID, server);
            m.Update(group);

            return m;
        }
    }
}
