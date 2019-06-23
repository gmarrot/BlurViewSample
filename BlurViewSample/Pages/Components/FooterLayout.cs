using System;
using System.Collections.Generic;
using BlurViewSample.Services;
using Xamarin.Forms;

namespace BlurViewSample.Pages.Components {
    public class FooterLayout : View {

        readonly IScreenAreaService _screenAreaService;
        readonly Dictionary<Size, SizeRequest> _measureCache = new Dictionary<Size, SizeRequest>();

        public FooterLayout() {
            _screenAreaService = DependencyService.Resolve<IScreenAreaService>();
            HorizontalOptions = LayoutOptions.Fill;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint) {
            var constraintSize = new Size(widthConstraint, heightConstraint);
            if (_measureCache.TryGetValue(constraintSize, out SizeRequest cachedSizeRequest)) {
                return cachedSizeRequest;
            }

            SizeRequest baseSizeRequest = base.OnMeasure(widthConstraint, heightConstraint);
            SizeRequest desiredSizeRequest = ComputeDesiredSizeRequest(baseSizeRequest);
            if (desiredSizeRequest.Request.Width > 0 && desiredSizeRequest.Request.Height > 0) {
                _measureCache[constraintSize] = desiredSizeRequest;
            }

            return desiredSizeRequest;
        }

        protected override void InvalidateMeasure() {
            _measureCache.Clear();
            base.InvalidateMeasure();
        }

        SizeRequest ComputeDesiredSizeRequest(SizeRequest baseSizeRequest) {
            EdgeInsets safeAreaInsets = _screenAreaService.GetScreenAreaInsets();
            double desiredHeight = Math.Max(MinimumHeightRequest, safeAreaInsets.Bottom);

            var minimumSize = new Size(baseSizeRequest.Minimum.Width, desiredHeight);
            var requestedSize = new Size(baseSizeRequest.Request.Width, desiredHeight);

            return new SizeRequest(minimumSize, requestedSize);
        }


    }
}
