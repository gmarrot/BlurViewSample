using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BlurViewSample.Droid.Components.Blur.Controllers;

namespace BlurViewSample.Droid.Components.Blur {
    public class BlurView : FrameLayout {

        static readonly string TAG = typeof(BlurView).Name;

        IBlurController _blurController = new NoOpController();
        Color _overlayColor;
        bool _disposed;

        public BlurView(Context context) : base(context) {
            Init(null, 0, 0);
        }

        public BlurView(Context context, IAttributeSet attrs) : base(context, attrs) {
            Init(attrs, 0, 0);
        }

        public BlurView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) {
            Init(attrs, defStyleAttr, 0);
        }

        public BlurView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) {
            Init(attrs, defStyleAttr, defStyleRes);
        }

        protected BlurView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
        }

        void Init(IAttributeSet attrs, int defStyleAttr, int defStyleRes) {
            TypedArray attrValues = Context.ObtainStyledAttributes(attrs, Resource.Styleable.BlurView, defStyleAttr, defStyleRes);
            _overlayColor = attrValues.GetColor(Resource.Styleable.BlurView_overlayColor, Color.Transparent);
            attrValues.Recycle();
        }

        public override void Draw(Canvas canvas) {
            _blurController.Draw(canvas);
            base.Draw(canvas);
        }

        protected override void OnAttachedToWindow() {
            base.OnAttachedToWindow();
            if (!IsHardwareAccelerated) {
                Log.Error(TAG, "BlurView can't be used in not hardware-accelerated window!");
            } else {
                _blurController.SetBlurAutoUpdate(true);
            }
        }

        protected override void OnDetachedFromWindow() {
            base.OnDetachedFromWindow();
            _blurController.SetBlurAutoUpdate(false);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh) {
            base.OnSizeChanged(w, h, oldw, oldh);
            _blurController.UpdateBlurViewSize();
        }

        protected override void Dispose(bool disposing) {
            if (!_disposed) {
                _disposed = true;
                if (disposing && _blurController != null) {
                    _blurController.Dispose();
                    _blurController = null;
                }
            }

            base.Dispose(disposing);
        }

        public IBlurViewFacade SetupWith(View rootView) {
            _blurController?.Dispose();
            _blurController = new BlockingBlurController(this, rootView, _overlayColor);

            return _blurController;
        }

        public IBlurViewFacade SetBlurRadius(float radius) {
            return _blurController.SetBlurRadius(radius);
        }

        public IBlurViewFacade SetOverlayColor(Color overlayColor) {
            _overlayColor = overlayColor;
            return _blurController.SetOverlayColor(overlayColor);
        }

        public IBlurViewFacade SetBlurAutoUpdate(bool enabled) {
            return _blurController.SetBlurAutoUpdate(enabled);
        }

        public IBlurViewFacade SetBlurEnabled(bool enabled) {
            return _blurController.SetBlurEnabled(enabled);
        }

    }
}
