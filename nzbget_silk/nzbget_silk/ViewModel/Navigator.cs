using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nzbget_silk.ViewModel
{
    public class Navigator
    {
        public static async Task PushPage<TPage, TViewModel>(params object[] args)
            where TPage : Page
            where TViewModel : PageViewModel
        {
            var page = PageFactory<TPage, TViewModel>(args);

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
        public static async Task PushModal<TPage, TViewModel>(params object[] args)
            where TPage : Page
            where TViewModel : PageViewModel
        {
            var page = PageFactory<TPage, TViewModel>(args);

            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        private static Page PageFactory<TPage, TViewModel>(object[] args)
            where TPage : Page
            where TViewModel : PageViewModel
        {
            List<object> argsList = new List<object>(args);

            TPage page = Activator.CreateInstance<TPage>();
            argsList.Add(page);
            page.BindingContext = Activator.CreateInstance(typeof(TViewModel), argsList.ToArray());

            return page;
        }
    }
}
