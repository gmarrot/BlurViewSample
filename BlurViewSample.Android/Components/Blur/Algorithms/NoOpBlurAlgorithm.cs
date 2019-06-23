using System;
using Android.Graphics;
using Android.Runtime;

namespace BlurViewSample.Droid.Components.Blur.Algorithms {
    public class NoOpBlurAlgorithm : Java.Lang.Object, IBlurAlgorithm {

        public bool CanModifyBitMap => true;

        public Bitmap.Config SupportedBitmapConfig => Bitmap.Config.Argb8888;

        public NoOpBlurAlgorithm() {
        }

        public NoOpBlurAlgorithm(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) {
        }

        public Bitmap Blur(Bitmap bitmap, float blurRadius) {
            return bitmap;
        }

    }
}
