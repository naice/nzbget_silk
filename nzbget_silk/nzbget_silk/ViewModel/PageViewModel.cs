using System;
using System.Threading;
using System.Threading.Tasks;

namespace nzbget_silk.ViewModel
{
    public class PageViewModel : BaseViewModel
    {
        private string _Title;
        /// <summary>
        /// Page Title
        /// </summary>
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
        
        /// <summary>
        /// Page.
        /// </summary>
        protected Xamarin.Forms.Page Page { private set; get; }
        private bool isInitLazyDone = false;
        private bool isInitLazy = false;

        public PageViewModel(Xamarin.Forms.Page page)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
            Page.Disappearing += Page_Disappearing;
            Page.Appearing += Page_Appearing;
            Init();
        }

        private async void Page_Appearing(object sender, EventArgs e)
        {
            if (sender != Page) throw new Exception("UNEXPECTED BEHAVIOR");

            if (!isInitLazy)
            {
                isInitLazy = true;
                await InitLazy().ConfigureAwait(true);
                isInitLazyDone = true;
            }

            if (isInitLazyDone)
            {
                OnAppearing();
            }
        }

        private void Page_Disappearing(object sender, EventArgs e)
        {
            if (sender != Page) throw new Exception("UNEXPECTED BEHAVIOR");

            OnDisappearing();
        }

        /// <summary>
        /// Called from constructor after Init and hooking events.
        /// </summary>
        protected virtual void Init() { }
        /// <summary>
        /// Called once before OnApprearing 
        /// </summary>
        protected virtual Task InitLazy() { return Task.Delay(0); }
        /// <summary>
        /// Called on disappering of page.
        /// </summary>
        protected virtual void OnDisappearing() { }
        /// <summary>
        /// Called on appearing of page.
        /// </summary>
        protected virtual void OnAppearing() { }
    }
}