using UIKit;
using Xamarin.Forms;
using LogicLibrary.Native;
using System;
using LogicLibrary.Models;
using SeeNav.iOS;

[assembly: Dependency(typeof(iOS_ProjectionAngle))]
namespace SeeNav.iOS
{
    public class iOS_ProjectionAngle : IProjectionAngle
    {

        // https://developer.apple.com/documentation/scenekit/scncamera/2867510-fieldofview?language=objc

        public float Vertical { get;  }

        public float Horizontal { get; }

        public iOS_ProjectionAngle()
        {
            // try to understand this code better at ome point
      
            var camera = new SceneKit.SCNCamera();
            // SeeNav.iOS.AngleOfView
            Vertical = (float)camera.FieldOfView;
            // screen: https://stackoverflow.com/questions/38891654/get-current-screen-width-in-xamarin-forms
            var w = UIScreen.MainScreen.Bounds.Width;
            var h = UIScreen.MainScreen.Bounds.Height;
            Horizontal = (float)(Vertical * (w / h));

            // both vertical and horizontal is correct
        }
    }
}
// Got same values for vertical and horizontal irregardless of projectdirection set.
// So "little angle" is calculated with percentage