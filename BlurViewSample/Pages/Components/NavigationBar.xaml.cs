using System.Windows.Input;
using Xamarin.Forms;

namespace BlurViewSample.Pages.Components {
    public partial class NavigationBar : StackLayout {

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(NavigationBar), null,
                                    propertyChanged: (bindable, oldValue, newValue) => {
                                        ((NavigationBar)bindable).TitleLabel.Text = (string)newValue;
                                    });

        public static readonly BindableProperty BackCommandProperty =
            BindableProperty.Create(nameof(BackCommand), typeof(ICommand), typeof(NavigationBar), null,
                                    propertyChanged: (bindable, oldValue, newValue) => {
                                        ((NavigationBar)bindable).UpdateBackCommand((ICommand)newValue);
                                    });

        public NavigationBar() {
            InitializeComponent();
            UpdateBackCommand(BackCommand);
        }

        public string Title {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public ICommand BackCommand {
            get => (ICommand)GetValue(BackCommandProperty);
            set => SetValue(BackCommandProperty, value);
        }

        void UpdateBackCommand(ICommand backCommand) {
            BackButton.Command = backCommand;
            BackButton.IsVisible = backCommand != null;
        }

    }
}
