using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using BlurViewSample.Droid.Components.Blur.Algorithms;

namespace BlurViewSample.Droid.Components.Blur.Controllers {
    public class NoOpController : Java.Lang.Object, IBlurController {

        public NoOpController() {
        }

        public NoOpController(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) {
        }

        public void Draw(Canvas canvas) {
            // Nothing to do for no-op controller
        }

        public void UpdateBlurViewSize() {
            // Nothing to do for no-op controller
        }

        public IBlurViewFacade SetBlurRadius(float radius) {
            return this;
        }

        public IBlurViewFacade SetBlurAlgorithm(IBlurAlgorithm algorithm) {
            return this;
        }

        public IBlurViewFacade SetOverlayColor(Color overlayColor) {
            return this;
        }

        public IBlurViewFacade SetFrameClearDrawable(Drawable windowBackground) {
            return this;
        }

        public IBlurViewFacade SetBlurEnabled(bool enabled) {
            return this;
        }

        public IBlurViewFacade SetBlurAutoUpdate(bool enabled) {
            return this;
        }

        public IBlurViewFacade SetHasFixedTransformationMatrix(bool hasFixedTransformationMatrix) {
            return this;
        }

    }
}
