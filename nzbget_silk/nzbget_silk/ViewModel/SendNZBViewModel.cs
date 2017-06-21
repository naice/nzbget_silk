using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nzbget_silk.ViewModel
{
    public class SendNZBViewModel : PageViewModel
    {
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
        private string _ResultImagePath;
        public string ResultImagePath
        {
            get { return _ResultImagePath; }
            set
            {
                if (value != _ResultImagePath)
                {
                    _ResultImagePath = value;
                    RaisePropertyChanged(nameof(ResultImagePath));
                }
            }
        }
        public RelayCommand SendCommand { get; private set; }

        private readonly string _fileName;
        private readonly byte[] _fileContent;

        public SendNZBViewModel(string fileName, byte[] fileContent)
        {
            Title = "Upload";

            Description = $"{System.IO.Path.GetFileNameWithoutExtension(fileName)} @ {Tools.GetBytesReadable(fileContent.Length)}";

            _fileContent = fileContent;
            _fileName = fileName;
            SendCommand = new RelayCommand(() => SendExecute());
        }

        public async void SendExecute()
        {
            ResultImagePath = null;
            SendCommand.IsEnabled = false;
            IsIndicatorVisible = true;

            Service.NZBGetService service = null;
            await App.CurrentApp.GlobalStorage.Perform((data) => service = new Service.NZBGetService(data.CurrentServer));

            var result = await service.Append(_fileName, _fileContent);
            bool success = result != null && result.result == 0;

            if (success)
            {
                ResultImagePath = "Images/SendNZBSuccess.png";

                await Task.Delay(500).ConfigureAwait(true);

                Navigator.Remove(Page);
            }
            else
            {
                ResultImagePath = "Images/SendNZBFailure.png";
            }

            IsIndicatorVisible = false;
            SendCommand.IsEnabled = true;
        }
    }
}
