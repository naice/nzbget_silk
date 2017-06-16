using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string _Status;
        public string Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    RaisePropertyChanged("Status");
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


        public NZBGroupViewModel(long NZBID)
        {
            this.NZBID = NZBID;
        }

        public void Update(Model.JsonRPCGroup group)
        {
            this.Name = "unknown";
            if (!string.IsNullOrEmpty(group.NZBName))
            {
                this.Name = group.NZBName.Substring(0, 32); // cut name length...
            }

            switch (group.Status)
            {
                case "LOADING_PARS":         // stage of parcheck;
                case "VERIFYING_SOURCES":    // stage of parcheck;
                case "VERIFYING_REPAIRED":   // stage of parcheck;
                case "REPAIRING":            // stage of parcheck;
                    // REPAIRING
                    Status = "Reparing";
                    break;
                case "UNPACKING":            // being unpacked;
                    // UNPACKING
                    Status = "Unpacking";
                    break;
                case "MOVING":               // moving files from intermediate directory into destination directory;
                case "EXECUTING_SCRIPT":     // executing post processing script;
                case "PP_QUEUED":            // queued for post  processing(completely downloaded);
                case "PP_FINISHED":          // post processing is finished, the item is about to be moved to history.
                    // DONE
                    Status = "Done";
                    break;
                case "PAUSED":               // paused;
                    // PAUSED
                    Status = "Paused";
                    break;
                case "DOWNLOADING":          // item is being downloaded;
                    // DOWNLOADING
                    Status = "Downloading";
                    break;
                case "QUEUED":               // queued for download;
                case "FETCHING":             // nzb file is being fetched from URL (Kind = URL);
                case "RENAMING":             // processed by parrenamer;
                default:
                    Status = "Queued";
                    // QUEUED
                    break;
            }

            StatusImageSource = $"Images/ico_{Status.ToLower()}.png";
            var health = (double)group.Health / 1000d;
            Health = health > 1 ? 1 : health < 0 ? 0 : health;

            TotalSize = Tools.DoubleInt2Long((int)group.FileSizeLo, (int)group.FileSizeHi);
            TotalSizeText = Tools.GetBytesReadable(TotalSize);
            RemainingSize = Tools.DoubleInt2Long((int)group.RemainingSizeLo, (int)group.RemainingSizeHi);
            RemainingSizeText = Tools.GetBytesReadable(RemainingSize);
            long downloadedSize = TotalSize - RemainingSize;
            Progress = (double)downloadedSize / (double)TotalSize;
            IsProgressVisible = Progress > 0;
        }

        public static NZBGroupViewModel Create(Model.JsonRPCGroup group)
        {
            var m = new NZBGroupViewModel(group.NZBID);
            m.Update(group);

            return m;
        }
    }
}
