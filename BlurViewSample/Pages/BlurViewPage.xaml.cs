using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BlurViewSample.Pages {
    public partial class BlurViewPage : ContentPage {

        readonly bool _isModal;
        ICommand _backCommand;

        public BlurViewPage(bool isModal) {
            _isModal = isModal;

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = this;
        }

        public ICommand BackCommand => _backCommand ?? (_backCommand = new Command(async (obj) => {
            await GoBack();
        }));

        Task GoBack() {
            if (_isModal) {
                return Navigation.PopModalAsync();
            }

            return Navigation.PopAsync();
        }

    }
}
