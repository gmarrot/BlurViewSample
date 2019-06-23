using System;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace BlurViewSample.Pages {
    public partial class PopupTestPage : ContentPage {

        ICommand _backCommand;

        public PopupTestPage() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = this;
        }

        public ICommand BackCommand => _backCommand ?? (_backCommand = new Command(async (obj) => {
            await Navigation.PopAsync();
        }));

        async void HandleOnNavigateToBlurPopupPage(object sender, EventArgs args) {
            await PopupNavigation.Instance.PushAsync(new PopupPage());
        }

    }
}
