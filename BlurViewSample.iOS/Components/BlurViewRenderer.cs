using BlurViewSample.Components;
using BlurViewSample.iOS.Components;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BlurView), typeof(BlurViewRenderer))]
namespace BlurViewSample.iOS.Components {
    public class BlurViewRenderer : ViewRenderer<BlurView, UIVisualEffectView> {

        UIView _backgroundView;
        UIColor _defaultColor;
        bool _disposed;

        public BlurViewRenderer() {
            AutoPackage = false;
        }

        protected override UIVisualEffectView CreateNativeControl() {
            var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Light);

            return new UIVisualEffectView(blurEffect) {
                UserInteractionEnabled = false
            };
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BlurView> e) {
            if (e.NewElement != null) {
                if (Control == null) {
                    SetNativeControl(CreateNativeControl());
                }
                if (_backgroundView == null) {
                    _backgroundView = new UIView {
                        AutoresizingMask = UIViewAutoresizing.FlexibleDimensions
                    };
                    _defaultColor = _backgroundView.BackgroundColor;
                    _backgroundView.BackgroundColor = e.NewElement.BackgroundColor.ToUIColor();
                    AddSubview(_backgroundView);
                }
            }

            base.OnElementChanged(e);
        }

        protected override void Dispose(bool disposing) {
            if (!_disposed) {
                _disposed = true;

                if (disposing && _backgroundView != null) {
                    _backgroundView.RemoveFromSuperview();
                    _backgroundView.Dispose();
                    _backgroundView = null;
                }
            }

            base.Dispose(disposing);
        }

        protected override void SetBackgroundColor(Color color) {
            if (_backgroundView != null) {
                if (color == Color.Default) {
                    _backgroundView.BackgroundColor = _defaultColor;
                } else {
                    _backgroundView.BackgroundColor = color.ToUIColor();
                }
            }
        }

    }
}
