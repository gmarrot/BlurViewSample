using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using BlurViewSample.Droid.Components.Blur.Algorithms;

namespace BlurViewSample.Droid.Components.Blur.Controllers {
    public class BlockingBlurController : Java.Lang.Object, IBlurController, ViewTreeObserver.IOnGlobalLayoutListener, ViewTreeObserver.IOnPreDrawListener {

        // Bitmap size should be divisible by 16 to meet stride requirement
        const int ROUNDING_VALUE = 16;

        const float SCALE_FACTOR = 8f;

        float _blurRadius = 16f;
        float _roundingWidthScaleFactor = 1f;
        float _roundingHeightScaleFactor = 1f;

        IBlurAlgorithm _blurAlgorithm;
        Canvas _internalCanvas;
        Bitmap _internalBitmap;

        readonly View _blurView;
        readonly View _rootView;

        Color _overlayColor;

        readonly int[] _rootLocation = new int[2];
        readonly int[] _blurViewLocation = new int[2];

        bool _blurEnabled;

        Drawable _frameClearDrawable;
        bool _hasFixedTransformationMatrix;
        readonly Paint paint = new Paint(PaintFlags.FilterBitmap);

        bool _disposed;

        public BlockingBlurController(View blurView, View rootView, Color overlayColor) {
            _blurView = blurView ?? throw new ArgumentNullException(nameof(blurView));
            _rootView = rootView ?? throw new ArgumentNullException(nameof(rootView));
            _overlayColor = overlayColor;
            _blurAlgorithm = new NoOpBlurAlgorithm();

            int measuredWidth = blurView.MeasuredWidth;
            int measuredHeight = blurView.MeasuredHeight;

            if (IsZeroSized(measuredWidth, measuredHeight)) {
                DeferBitmapCreation();
                return;
            }

            Init(measuredWidth, measuredHeight);
        }

        public BlockingBlurController(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) {
        }

        void Init(int measuredWidth, int measuredHeight) {
            if (IsZeroSized(measuredWidth, measuredHeight)) {
                _blurEnabled = false;
                _blurView.SetWillNotDraw(true);
                SetBlurAutoUpdateInternal(false);
                return;
            }

            _blurEnabled = true;
            _blurView.SetWillNotDraw(false);
            AllocateBitmap(measuredWidth, measuredHeight);
            _internalCanvas = new Canvas(_internalBitmap);
            SetBlurAutoUpdateInternal(true);
            if (_hasFixedTransformationMatrix) {
                SetupInternalCanvasMatrix();
            }
        }

        void DeferBitmapCreation() {
            _blurView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }

        void UpdateBlur() {
            if (!_blurEnabled) {
                return;
            }

            if (_frameClearDrawable == null) {
                _internalBitmap.EraseColor(Color.Transparent);
            } else {
                _frameClearDrawable.Draw(_internalCanvas);
            }

            if (_hasFixedTransformationMatrix) {
                _rootView.Draw(_internalCanvas);
            } else {
                _internalCanvas.Save();
                SetupInternalCanvasMatrix();
                _rootView.Draw(_internalCanvas);
                _internalCanvas.Restore();
            }

            BlurAndSave();
        }

        void AllocateBitmap(int measuredWidth, int measuredHeight) {
            int nonRoundedScaledWidth = DownScaleSize(measuredWidth);
            int nonRoundedScaledHeight = DownScaleSize(measuredHeight);

            int scaledWidth = RoundSize(nonRoundedScaledWidth);
            int scaledHeight = RoundSize(nonRoundedScaledHeight);

            _roundingHeightScaleFactor = (float)nonRoundedScaledHeight / scaledHeight;
            _roundingWidthScaleFactor = (float)nonRoundedScaledWidth / scaledWidth;

            _internalBitmap = Bitmap.CreateBitmap(scaledWidth, scaledHeight, _blurAlgorithm.SupportedBitmapConfig);
        }

        void SetupInternalCanvasMatrix() {
            _rootView.GetLocationOnScreen(_rootLocation);
            _blurView.GetLocationOnScreen(_blurViewLocation);

            int left = _blurViewLocation[0] - _rootLocation[0];
            int top = _blurViewLocation[1] - _rootLocation[1];

            float scaleFactorX = SCALE_FACTOR * _roundingWidthScaleFactor;
            float scaleFactorY = SCALE_FACTOR * _roundingHeightScaleFactor;

            float scaledLeftPosition = -left / scaleFactorX;
            float scaledTopPosition = -top / scaleFactorY;

            _internalCanvas.Translate(scaledLeftPosition, scaledTopPosition);
            _internalCanvas.Scale(1 / scaleFactorX, 1 / scaleFactorY);
        }

        public void Draw(Canvas canvas) {
            //draw only on system's hardware accelerated canvas
            if (!_blurEnabled || !canvas.IsHardwareAccelerated) {
                return;
            }

            UpdateBlur();

            canvas.Save();
            canvas.Scale(SCALE_FACTOR * _roundingWidthScaleFactor, SCALE_FACTOR * _roundingHeightScaleFactor);
            canvas.DrawBitmap(_internalBitmap, 0, 0, paint);
            canvas.Restore();

            if (_overlayColor != Color.Transparent) {
                canvas.DrawColor(_overlayColor);
            }
        }

        public void UpdateBlurViewSize() {
            int measuredWidth = _blurView.MeasuredWidth;
            int measuredHeight = _blurView.MeasuredHeight;

            Init(measuredWidth, measuredHeight);
        }

        protected override void Dispose(bool disposing) {
            if (!_disposed) {
                _disposed = true;
                if (disposing) {
                    SetBlurAutoUpdateInternal(false);
                    if (_blurAlgorithm != null) {
                        _blurAlgorithm.Dispose();
                        _blurAlgorithm = null;
                    }
                    if (_internalBitmap != null) {
                        _internalBitmap.Recycle();
                        _internalBitmap = null;
                    }
                }
            }

            base.Dispose(disposing);
        }

        void BlurAndSave() {
            _internalBitmap = _blurAlgorithm.Blur(_internalBitmap, _blurRadius);
            if (!_blurAlgorithm.CanModifyBitMap) {
                _internalCanvas.SetBitmap(_internalBitmap);
            }
        }

        public IBlurViewFacade SetBlurRadius(float radius) {
            _blurRadius = radius;
            return this;
        }


        public IBlurViewFacade SetBlurAlgorithm(IBlurAlgorithm algorithm) {
            _blurAlgorithm = algorithm;
            return this;
        }

        public IBlurViewFacade SetFrameClearDrawable(Drawable frameClearDrawable) {
            _frameClearDrawable = frameClearDrawable;
            return this;
        }

        void SetBlurEnabledInternal(bool enabled) {
            _blurEnabled = enabled;
            SetBlurAutoUpdateInternal(enabled);
            _blurView.Invalidate();
        }

        public IBlurViewFacade SetBlurEnabled(bool enabled) {
            _blurView.Post(() => {
                if (!_disposed) {
                    SetBlurEnabledInternal(enabled);
                }
            });
            return this;
        }

        void SetBlurAutoUpdateInternal(bool enabled) {
            _blurView.ViewTreeObserver.RemoveOnPreDrawListener(this);
            if (enabled) {
                _blurView.ViewTreeObserver.AddOnPreDrawListener(this);
            }
        }

        public IBlurViewFacade SetBlurAutoUpdate(bool enabled) {
            _blurView.Post(() => {
                if (!_disposed) {
                    SetBlurAutoUpdateInternal(enabled);
                }
            });
            return this;

        }

        public IBlurViewFacade SetHasFixedTransformationMatrix(bool hasFixedTransformationMatrix) {
            _hasFixedTransformationMatrix = hasFixedTransformationMatrix;
            return this;
        }

        public IBlurViewFacade SetOverlayColor(Color overlayColor) {
            if (_overlayColor != overlayColor) {
                _overlayColor = overlayColor;
                _blurView.Invalidate();
            }
            return this;
        }

        int DownScaleSize(float value) {
            return (int)Math.Ceiling(value / SCALE_FACTOR);
        }

        int RoundSize(int value) {
            if (value % ROUNDING_VALUE == 0) {
                return value;
            }

            return value - (value % ROUNDING_VALUE) + ROUNDING_VALUE;
        }

        bool IsZeroSized(int measuredWidth, int measuredHeight) {
            return DownScaleSize(measuredHeight) == 0 || DownScaleSize(measuredWidth) == 0;
        }

        #region ViewTreeObserver.IOnGlobalLayoutListener

        public void OnGlobalLayout() {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean) {
                _blurView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            } else {
#pragma warning disable CS0618 // Type or member is obsolete
                _blurView.ViewTreeObserver.RemoveGlobalOnLayoutListener(this);
#pragma warning restore CS0618 // Type or member is obsolete
            }

            int measuredWidth = _blurView.MeasuredWidth;
            int measuredHeight = _blurView.MeasuredHeight;

            Init(measuredWidth, measuredHeight);
        }

        #endregion

        #region ViewTreeObserver.IOnPreDrawListener

        public bool OnPreDraw() {
            // Not invalidating a View here, just updating the Bitmap.
            // This relies on the HW accelerated bitmap drawing behavior in Android
            // If the bitmap was drawn on HW accelerated canvas, it holds a reference to it and on next
            // drawing pass the updated content of the bitmap will be rendered on the screen
            UpdateBlur();

            return true;
        }

        #endregion

    }
}
