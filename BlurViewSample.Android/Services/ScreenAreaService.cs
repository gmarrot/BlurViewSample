using BlurViewSample.Droid.Services;
using BlurViewSample.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenAreaService))]
namespace BlurViewSample.Droid.Services {
    public class ScreenAreaService : IScreenAreaService {

        public EdgeInsets GetScreenAreaInsets() {
            return new EdgeInsets(0);
        }

    }
}
