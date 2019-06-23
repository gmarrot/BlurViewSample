using System;
using Android.Graphics;

namespace BlurViewSample.Droid.Components.Blur.Controllers {
    public interface IBlurController : IBlurViewFacade, IDisposable {

        /// <summary>
        /// Draws blurred content on given canvas.
        /// </summary>
        void Draw(Canvas canvas);

        /// <summary>
        /// Must be used to notify Controller when BlurView's size has changed.
        /// </summary>
        void UpdateBlurViewSize();

    }
}
