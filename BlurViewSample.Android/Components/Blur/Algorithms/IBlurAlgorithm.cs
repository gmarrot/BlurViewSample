using System;
using Android.Graphics;

namespace BlurViewSample.Droid.Components.Blur.Algorithms {
    public interface IBlurAlgorithm : IDisposable {

        bool CanModifyBitMap { get; }
        Bitmap.Config SupportedBitmapConfig { get; }

        Bitmap Blur(Bitmap bitmap, float blurRadius);

    }
}
