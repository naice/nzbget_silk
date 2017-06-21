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
            var pageViewModel = Activator.CreateInstance(typeof(TViewModel), args);

            // Inject Page.
            Inject(pageViewModel, nameof(PageViewModel.Page), page);
            // Inject Navigator.
            Inject(pageViewModel, nameof(PageViewModel.Navigator), nav);

            // Bind ViewModel to Page.
            page.BindingContext = pageViewModel;
            return page;
        }

        private static void Inject(object @in, string name, object value)
        {
            var type = @in.GetType();
            type.GetRuntimeProperty(name).SetValue(@in, value);
        }
    }
}
