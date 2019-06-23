using Android.Graphics;
using Android.Graphics.Drawables;
using BlurViewSample.Droid.Components.Blur.Algorithms;

namespace BlurViewSample.Droid.Components.Blur {
    public interface IBlurViewFacade {

        /// <summary>
        /// Enables / disables the blur. Enabled by default.
        /// </summary>
        IBlurViewFacade SetBlurEnabled(bool enabled);

        /// <summary>
        /// Can be used to stop blur auto update or resume if it was stopped before.
        /// Enabled by default.
        /// </summary>
        IBlurViewFacade SetBlurAutoUpdate(bool enabled);

        /// <summary>
        /// Can be set to true to optimize position calculation before blur.
        /// By default, BlurView calculates its translation, rotation and scale before each draw call.
        /// If you are not changing these properties (for example, during animation), this behavior can be changed
        /// to calculate them only once during initialization.
        /// </summary>
        IBlurViewFacade SetHasFixedTransformationMatrix(bool hasFixedTransformationMatrix);

        /// <summary>
        /// Sets the drawable to draw before view hierarchy.
        /// Can be used to draw Activity's window background if your root layout doesn't provide any background.
        /// Optional, by default frame is cleared with a transparent color.
        /// </summary>
        IBlurViewFacade SetFrameClearDrawable(Drawable frameClearDrawable);

        /// <summary>
        /// Sets the blur radius.
        /// </summary>
        IBlurViewFacade SetBlurRadius(float radius);

        /// <summary>
        /// Sets the blur algorithm.
        /// </summary>
        IBlurViewFacade SetBlurAlgorithm(IBlurAlgorithm algorithm);

        /// <summary>
        /// Sets the color overlay to be drawn on top of blurred content.
        /// </summary>
        IBlurViewFacade SetOverlayColor(Color overlayColor);

    }
}
