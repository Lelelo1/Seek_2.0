using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Seek.iOS;
using Logic.Sensors;
using UIKit;
using Xamarin.Forms;
using CoreLocation;
using Namespace;
[assembly: Dependency(typeof(Compass)) ]
namespace Seek.iOS
{
    // https://stackoverflow.com/questions/13358364/iphone-offset-with-compass-heading-values-between-landscape-right-and-landscape
    // https://github.com/xamarin/Essentials/blob/master/Xamarin.Essentials/DeviceDisplay/DeviceDisplay.ios.cs
    public class Compass : ICompass
    {
        NSObject observer;

        public double Heading { get; set; }
        Orientation _orientation = Orientation.Portrait;
        public Orientation Orientation { get => _orientation; set { _orientation = value;  setOrientation(_orientation); }}
        public event HeadingChanged OnHeadingChanged;
        public CLLocationManager locationManager = new CLLocationManager();

        public async void Start(double degreeSensitivity)
        {
            locationManager.HeadingFilter = degreeSensitivity;
            locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
            // var nativeView = await ((StackLayout)(await Main.MainPage.Get()).Content).On<Xamarin.Forms.PlatformConfiguration.iOS>().iOSAsync();
            // StartRotationListener();
            locationManager.UpdatedHeading += LocationManager_UpdatedHeading;
            locationManager.StartUpdatingHeading();
        }

        void setOrientation(Orientation orientation)
        {
            switch(orientation)
            {
                case Orientation.Portrait:
                    locationManager.HeadingOrientation = CLDeviceOrientation.Portrait;
                    break;
                case Orientation.LandscapeLeft:
                    locationManager.HeadingOrientation = CLDeviceOrientation.LandscapeLeft;
                    break;
                case Orientation.LandscapeRight:
                    locationManager.HeadingOrientation = CLDeviceOrientation.LandscapeRight;
                    break;
                case Orientation.UpsideDown:
                    locationManager.HeadingOrientation = CLDeviceOrientation.PortraitUpsideDown;
                    break;
            }
            // if unknown, precede with the last known orinetation
        }

        void StartRotationListener()
        {
            var notificationCenter = NSNotificationCenter.DefaultCenter;
            // var notification = UIApplication.DidChangeStatusBarOrientationNotification;
            // var notification = UIDevice.OrientationDidChangeNotification;
 
            // observer = notificationCenter.AddObserver(notification, OnRotationChanged);
        }

        void StopRotationListener()
        {
            observer?.Dispose();
            observer = null;
        }

        /* not recievnig any change
        void OnRotationChanged(NSNotification obj)
        {
            var uiOrientation = UIApplication.SharedApplication.StatusBarOrientation;
            var orientation = CLDeviceOrientation.Unknown;
            switch (uiOrientation)
            {
                case UIInterfaceOrientation.LandscapeLeft:
                    orientation = CLDeviceOrientation.LandscapeLeft;
                    break;
                case UIInterfaceOrientation.LandscapeRight:
                    orientation = CLDeviceOrientation.LandscapeRight;
                    break;
                case UIInterfaceOrientation.Portrait:
                    orientation = CLDeviceOrientation.Portrait;
                    break;
                case UIInterfaceOrientation.PortraitUpsideDown:
                    orientation = CLDeviceOrientation.PortraitUpsideDown;
                    break;
            }
            Console.WriteLine("Changed rotation of device to: " + orientation);
            locationManager.HeadingOrientation = orientation;
        }
        */
        // uidevice, stuck between Unknown and Portrait
        /*
        void OnRotationChanged(NSNotification obj)
        {
            var uiOrientation = UIDevice.CurrentDevice.Orientation;
            var orientation = CLDeviceOrientation.Unknown;
            switch(uiOrientation)
            {
                case UIDeviceOrientation.FaceDown:
                    orientation = CLDeviceOrientation.FaceDown;
                    break;
                case UIDeviceOrientation.FaceUp:
                    orientation = CLDeviceOrientation.FaceUp;
                    break;
                case UIDeviceOrientation.LandscapeLeft:
                    orientation = CLDeviceOrientation.LandscapeLeft;
                    break;
                case UIDeviceOrientation.LandscapeRight:
                    orientation = CLDeviceOrientation.LandscapeRight;
                    break;
                case UIDeviceOrientation.Portrait:
                    orientation = CLDeviceOrientation.Portrait;
                    break;
                case UIDeviceOrientation.PortraitUpsideDown:
                    orientation = CLDeviceOrientation.PortraitUpsideDown;
                    break;
            }
            Console.WriteLine("Changed rotation of device to: " + orientation);
            locationManager.HeadingOrientation = orientation;   
        }
        */
        private void LocationManager_UpdatedHeading(object sender, CLHeadingUpdatedEventArgs e)
        {
            Heading = e.NewHeading.TrueHeading;
            OnHeadingChanged();
        }
        public void Stop()
        {
            locationManager.UpdatedHeading -= LocationManager_UpdatedHeading;
            locationManager.StopUpdatingHeading();
            StopRotationListener();
        }
    }
}