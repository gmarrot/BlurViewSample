using System;
using Android.Content;
using Android.Graphics;
using Android.Renderscripts;
using Android.Runtime;

namespace BlurViewSample.Droid.Components.Blur.Algorithms {
    public class RenderScriptBlur : Java.Lang.Object, IBlurAlgorithm {

        readonly RenderScript _renderScript;
        readonly ScriptIntrinsicBlur _blurScript;

        Allocation _outAllocation;

        int _lastBitmapWidth = -1;
        int _lastBitmapHeight = -1;

        bool _disposed;

        public RenderScriptBlur(Context context) {
            _renderScript = RenderScript.Create(context);
            _blurScript = ScriptIntrinsicBlur.Create(_renderScript, Element.U8_4(_renderScript));
        }

        public RenderScriptBlur(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) {
        }

        public bool CanModifyBitMap => true;

        public Bitmap.Config SupportedBitmapConfig => Bitmap.Config.Argb8888;

        public Bitmap Blur(Bitmap bitmap, float blurRadius) {
            // Allocation will use the same backing array of pixels as bitmap if created with USAGE_SHARED flag
            using (var inAllocation = Allocation.CreateFromBitmap(_renderScript, bitmap)) {
                if (!CanReuseAllocation(bitmap)) {
                    if (_outAllocation != null) {
                        _outAllocation.Destroy();
                        _outAllocation.Dispose();
                    }

                    _outAllocation = Allocation.CreateTyped(_renderScript, inAllocation.Type);
                    _lastBitmapWidth = bitmap.Width;
                    _lastBitmapHeight = bitmap.Height;
                }

                _blurScript.SetRadius(blurRadius);
                _blurScript.SetInput(inAllocation);
                // Do not use inAllocation in forEach, it will caue visual artifacts on blurred bitmap
                _blurScript.ForEach(_outAllocation);
                _outAllocation.CopyTo(bitmap);

                inAllocation.Destroy();
            }

            return bitmap;
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                _disposed = true;
                if (disposing) {
                    _renderScript.Destroy();
                    _renderScript.Dispose();

                    _blurScript.Destroy();
                    _blurScript.Dispose();

                    if (_outAllocation != null) {
                        _outAllocation.Destroy();
                        _outAllocation.Dispose();
                        _outAllocation = null;
                    }
                }
            }

            base.Dispose(disposing);
        }

        bool CanReuseAllocation(Bitmap bitmap) {
            return bitmap.Width == _lastBitmapWidth && bitmap.Height == _lastBitmapHeight;
        }

    }
}
