using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

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


        private bool _IsInitializingLazy;
        /// <summary>
        /// Inidicator when InitLazy() is processing.
        /// </summary>
        public bool IsInitializingLazy
        {
            get { return _IsInitializingLazy; }
            set
            {
                if (value != _IsInitializingLazy)
                {
                    _IsInitializingLazy = value;
                    RaisePropertyChanged(nameof(IsInitializingLazy));
                }
            }
        }
        
        private Page _Page;
        /// <summary>
        /// The Page that this VM was initially bound to.
        /// </summary>
        public Page Page
        {
            get { return _Page; }
            private set
            {
                if (value != _Page)
                {
                    _Page = value ?? throw new ArgumentException(nameof(Page));
                    _Page.Disappearing += Page_Disappearing;
                    _Page.Appearing += Page_Appearing;
                }
            }
        }

        /// <summary>
        /// The Navigator. Initialized AFTER construction. (not available in Init())
        /// </summary>
        public Navigator Navigator { private set; get; }

        private bool isInitLazyDone = false;
        private bool isInitLazy = false;
        
        private async void Page_Appearing(object sender, EventArgs e)
        {
            if (sender != Page) throw new Exception("UNEXPECTED BEHAVIOR");

            if (!isInitLazy)
            {
                isInitLazy = true;
                IsInitializingLazy = true;
                await InitLazy().ConfigureAwait(true);
                IsInitializingLazy = false;
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