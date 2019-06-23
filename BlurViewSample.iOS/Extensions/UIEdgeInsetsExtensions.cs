using BlurViewSample.Services;
using UIKit;

namespace BlurViewSample.iOS.Extensions {
    public static class UIEdgeInsetsExtensions {

        public static EdgeInsets ToEdgeInsets(this UIEdgeInsets insets) {
            return new EdgeInsets(insets.Left, insets.Top, insets.Right, insets.Bottom);
        }

    }
}
