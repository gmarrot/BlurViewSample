using System.Windows.Input;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace BlurViewSample.Pages {
    public partial class PopupPage : Rg.Plugins.Popup.Pages.PopupPage {

        ICommand _backCommand;

        public PopupPage() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = this;

            BackgroundColor = Color.FromHex("#99000000");
            HasSystemPadding = Device.RuntimePlatform == Device.Android;
            Animation = new MoveAnimation();
        }

        public ICommand BackCommand => _backCommand ?? (_backCommand = new Command(async (obj) => {
            await PopupNavigation.Instance.PopAsync();
        }));

    }
}
