using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;

namespace nzbget_silk.ViewModel
{
    public class Navigator
    {
        private INavigation _navigation;

        public Navigator(INavigation navigation)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        }
        public async Task Push<TPage, TViewModel>(params object[] args)
            where TPage : Page
            where TViewModel : PageViewModel
        {
            var page = PageFactory<TPage, TViewModel>(this, args);

            await _navigation.PushAsync(page);
        }
        public async Task PushModal<TPage, TViewModel>(params object[] args)
            where TPage : Page
            where TViewModel : PageViewModel
        {
            var page = PageFactory<TPage, TViewModel>(this, args);

            await _navigation.PushModalAsync(page);
        }
        public void Remove(Page page) => _navigation.RemovePage(page);
        public void InsertBefore(Page page, Page before) => _navigation.InsertPageBefore(page, before);
        public async Task<PageViewModel> Pop()
        {
            var pageVM = (await _navigation.PopAsync()).BindingContext as PageViewModel;

            return pageVM;
        }

        private static Page PageFactory<TPage, TViewModel>(Navigator nav, object[] args)
            where TPage : Page
            where TViewModel : PageViewModel
        {
            // Construct Page and Model
            TPage page = Activator.CreateInstance<TPage>();
            var pageViewModel = Activator.CreateInstance(typeof(TViewModel), args) as PageViewModel;

            if (pageViewModel == null)
                throw new InvalidOperationException($"{typeof(TViewModel).ToString()} have to derive from {nameof(PageViewModel)} in order to work properly.");

            // Register / Inject dependency
            pageViewModel.Register(page, nav);

            // Bind ViewModel to Page.
            page.BindingContext = pageViewModel;
            return page;
        }
    }
}
