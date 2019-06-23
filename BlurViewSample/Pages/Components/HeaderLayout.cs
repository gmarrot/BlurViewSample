using System.Collections.Generic;
using BlurViewSample.Services;
using Xamarin.Forms;

namespace BlurViewSample.Pages.Components {
    public class HeaderLayout : View {

        readonly IScreenAreaService _screenAreaService;
        readonly Dictionary<Size, SizeRequest> _measureCache = new Dictionary<Size, SizeRequest>();

        public HeaderLayout() {
            _screenAreaService = DependencyService.Resolve<IScreenAreaService>();
            HorizontalOptions = LayoutOptions.Fill;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint) {
            if (Device.RuntimePlatform != Device.iOS) {
                return base.OnMeasure(widthConstraint, heightConstraint);
            }

            var constraintSize = new Size(widthConstraint, heightConstraint);
            if (_measureCache.TryGetValue(constraintSize, out SizeRequest cachedSizeRequest)) {
                return cachedSizeRequest;
            }

            SizeRequest desiredSizeRequest = ComputeDesiredSizeRequest(widthConstraint);
            if (desiredSizeRequest.Request.Width > 0 && desiredSizeRequest.Request.Height > 0) {
                _measureCache[constraintSize] = desiredSizeRequest;
            }

            return desiredSizeRequest;
        }

        protected override void InvalidateMeasure() {
            _measureCache.Clear();
            base.InvalidateMeasure();
        }

        SizeRequest ComputeDesiredSizeRequest(double widthConstraint) {
            EdgeInsets safeAreaInsets = _screenAreaService.GetScreenAreaInsets();
            var desiredSize = new Size(widthConstraint, safeAreaInsets.Top);

            return new SizeRequest(desiredSize);
        }


    }
}

