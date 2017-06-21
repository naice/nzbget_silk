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
        private FormattedString _Description;
        public FormattedString Description
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

            var fs = new FormattedString();
            fs.Spans.Add(new Span { Text = System.IO.Path.GetFileNameWithoutExtension(fileName) });
            fs.Spans.Add(new Span { Text = $" @ {Tools.GetBytesReadable(fileContent.Length)}", FontAttributes = FontAttributes.Bold });
            Description = fs;

            _fileContent = fileContent;
            _fileName = fileName;
            SendCommand = new RelayCommand(() => SendExecute());
        }

        public async void SendExecute()
        {
            ResultImagePath = null;
            SendCommand.IsEnabled = false;
            IsIndicatorVisible = true;

            await Task.Delay(500).ConfigureAwait(true);

            Service.NZBGetService service = null;
            await App.CurrentApp.GlobalStorage.Perform(
                (data) => service = new Service.NZBGetService(data.CurrentServer)).ConfigureAwait(true);

            var result = await service.Append(_fileName, _fileContent).ConfigureAwait(true);
            bool success = result != null && result.result > 0;

            //await Page.DisplayAlert("result", Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented), "Ok");

            if (success)
            {
                ResultImagePath = "Images/SendNZBSuccess.png";

                await Task.Delay(1000).ConfigureAwait(true);

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
