using System;

namespace nzbget_silk.ViewModel
{
    public class PageViewModel : BaseViewModel
    {
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (value != _Title)
                {
                    _Title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }
    }
}