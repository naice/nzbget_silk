using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk.ViewModel
{
    public class SendNZBViewModel : PageViewModel
    {
        private bool _IsSendButtonEnabled = true;
        public bool IsSendButtonEnabled
        {
            get { return _IsSendButtonEnabled; }
            set
            {
                if (value != _IsSendButtonEnabled)
                {
                    _IsSendButtonEnabled = value;
                    RaisePropertyChanged("IsSendButtonEnabled");
                }
            }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }
        private bool _IsIndicatorVisible;
        public bool IsIndicatorVisible
        {
            get { return _IsIndicatorVisible; }
            set
            {
                if (value != _IsIndicatorVisible)
                {
                    _IsIndicatorVisible = value;
                    RaisePropertyChanged("IsIndicatorVisible");
                }
            }
        }

        private readonly string _fileName;
        private readonly byte[] _fileContent;

        public SendNZBViewModel(string fileName, byte[] fileContent)
        {
            Title = "Upload";

            Description = $"{System.IO.Path.GetFileNameWithoutExtension(fileName)} @ {Tools.GetBytesReadable(fileContent.Length)}";

            _fileContent = fileContent;
            _fileName = fileName;
        }

        public async Task<bool> Send()
        {
            IsSendButtonEnabled = false;
            IsIndicatorVisible = true;

            Service.NZBGetService service = null;
            await App.CurrentApp.GlobalStorage.Perform((data) => service = new Service.NZBGetService(data.CurrentServer));

            var result = await service.Append(_fileName, _fileContent);
            bool success = result != null && result.result == 0;

            IsIndicatorVisible = false;
            IsSendButtonEnabled = true;

            return success;
        }
    }
}
