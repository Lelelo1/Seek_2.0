using UIKit;
using Xamarin.Forms;
using XamarinLogic.Native;
[assembly: Dependency(typeof(Seek.iOS.iOS_AngleOfView))]
namespace Seek.iOS
{
    public class iOS_AngleOfView : IAngleOfView
    {
        /*
        // https://developer.apple.com/documentation/scenekit/scncamera/2867510-fieldofview?language=objc
        // when phone is vertical in portrait...
        public double Horizontal { get; private set; }
        public double Vertical { get; private set; }
        */

        public AngleOfView Value { get; } = new AngleOfView();

        public iOS_AngleOfView()
        {
            var camera = new SceneKit.SCNCamera();
            // Seek.iOS.AngleOfView
            Value.Vertical = camera.FieldOfView;
            // screen: https://stackoverflow.com/questions/38891654/get-current-screen-width-in-xamarin-forms
            var w = UIScreen.MainScreen.Bounds.Width;
            var h = UIScreen.MainScreen.Bounds.Height;
            Value.Horizontal = Value.Vertical * (w / h);

            // both vertical and horizontal is correct
        }
    }
}
// Got same values for vertical and horizontal irregardless of projectdirection set.
// So "little angle" is calculated with percentage