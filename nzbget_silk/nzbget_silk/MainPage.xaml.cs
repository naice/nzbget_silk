using NcodedXMobile.Controls;
using nzbget_silk.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace nzbget_silk
{
    public partial class MainPage : ContentPage
    {
        private readonly TapGestureRecognizer _groupItemTapGestureRecognizer;

        public MainPage()
        {
            InitializeComponent();
            _groupItemTapGestureRecognizer = new TapGestureRecognizer();
            _groupItemTapGestureRecognizer.Tapped += (s,e)=> RepeaterView_ItemTapped(s as View, (s as View).BindingContext);
        }

        public ViewModel.MainPageViewModel ViewModel => BindingContext as ViewModel.MainPageViewModel;

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            ViewModel.StartUpdate();
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            ViewModel.StopUpdate();
        }

        private void RepeaterView_ItemCreated(object sender, RepeaterViewItemAddedEventArgs args)
        {
            args.View.GestureRecognizers.Add(_groupItemTapGestureRecognizer);
            var ctxMenu = args.View.FindByName<View>("ContextMenuView");
            ctxMenu.TranslateTo(ctxMenu.Width + 100, 0, 10);
        }

        private Task _contextMenuFadeTask;
        private void RepeaterView_ItemTapped(View view, object model)
        {
            if (_contextMenuFadeTask != null && !_contextMenuFadeTask.IsCompleted)
                return;

            var ctxMenu = view.FindByName<View>("ContextMenuView");

            if (ctxMenu.Opacity < 1)
            {
                _contextMenuFadeTask = ctxMenu.FadeTo(1, 250, Easing.CubicOut);
                ctxMenu.TranslateTo(0, 0, 250, Easing.CubicOut);
            }
            else
            {
                _contextMenuFadeTask = ctxMenu.FadeTo(0, 250, Easing.CubicOut);
                ctxMenu.TranslateTo(ctxMenu.Width + 100, 0, 250, Easing.CubicOut);
            }
        }
    }
}
