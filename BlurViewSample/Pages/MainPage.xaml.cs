using System;
using Xamarin.Forms;

namespace BlurViewSample.Pages {
    public partial class MainPage : ContentPage {

        public MainPage() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void HandleOnNavigateToBlurViewPage(object sender, EventArgs args) {
            await Navigation.PushAsync(new BlurViewPage(false));
        }

        async void HandleOnNavigateToModalBlurViewPage(object sender, EventArgs args) {
            await Navigation.PushModalAsync(new BlurViewPage(true));
        }

        async void HandleOnNavigateToPopupBlurViewPage(object sender, EventArgs args) {
            await Navigation.PushAsync(new PopupTestPage());
        }

    }
}
