using Android.App;
using Android.Content;
using BlurViewSample.Components;
using BlurViewSample.Droid.Components;
using BlurViewSample.Droid.Components.Blur.Algorithms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ABlurView = BlurViewSample.Droid.Components.Blur.BlurView;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(BlurView), typeof(BlurViewRenderer))]
namespace BlurViewSample.Droid.Components {
    public class BlurViewRenderer : ViewRenderer<BlurView, ABlurView> {

        public BlurViewRenderer(Context context) : base(context) {
            AutoPackage = false;
        }

        protected override ABlurView CreateNativeControl() {
            return new ABlurView(Context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BlurView> e) {
            base.OnElementChanged(e);

            if (e.NewElement != null) {
                if (Control == null) {
                    SetNativeControl(CreateNativeControl());
                }

                UpdateBlur();
            }
        }

        void UpdateBlur() {
            if (Control == null) {
                return;
            }

            AView decorView = ((Activity)Context).Window.DecorView;

            Control.SetupWith(decorView.RootView)
                   .SetFrameClearDrawable(decorView.Background)
                   .SetBlurAlgorithm(new RenderScriptBlur(Context))
                   .SetHasFixedTransformationMatrix(true);
        }

    }
}
