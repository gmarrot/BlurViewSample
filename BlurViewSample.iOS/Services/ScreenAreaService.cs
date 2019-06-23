using BlurViewSample.iOS.Extensions;
using BlurViewSample.iOS.Services;
using BlurViewSample.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenAreaService))]
namespace BlurViewSample.iOS.Services {
    public class ScreenAreaService : IScreenAreaService {

        const double STATUS_BAR_HEIGHT_BEFORE_IOS_11 = 20;

        public EdgeInsets GetScreenAreaInsets() {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0)) {
                return new EdgeInsets(0, STATUS_BAR_HEIGHT_BEFORE_IOS_11, 0, 0);
            }

            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            if (window == null || window.RootViewController == null) {
                return new EdgeInsets(0);
            }

            return window.RootViewController.View.SafeAreaInsets.ToEdgeInsets();
        }

    }
}
