using System;
using System.Collections.Generic;
using Logic.Models;
using UnitsNet;
using Logic.Services.PermissionRequired;
using Microsoft.AppCenter.Crashes;
using Logic.Utils;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Essentials;
using System.Numerics;
using Location = Logic.Models.Location;

namespace Logic.ViewModels
{

    public class MainViewModel : IBase
    {

        static TaskCompletionSource<IBase> Initializing { get; } = new TaskCompletionSource<IBase>();

        public static Task<IBase> Init()
        {
            Initializing.SetResult(new MainViewModel());

            return Initializing.Task;
        }

        protected MainViewModel()
        {
            OrientationSensor.ReadingChanged += UpdateOrientation;
            OrientationSensor.Start(SensorSpeed.UI);
        }

        public Observable<Quaternion> Orientation { get; } = new Observable<Quaternion>(Quaternion.Identity);

        void UpdateOrientation(object sender, OrientationSensorChangedEventArgs e)
        {
            Orientation.Set(e.Reading.Orientation); // could set required change level needed for update here
        }

        private Activities _currentActivity = Activities.Arrow;
        public Activities CurrentActivity
        {
            get => _currentActivity;
            set { _currentActivity = value; OnActivityChanged?.Invoke(this, value);}
        }
        public delegate void ActivityChanging(object sender, Activities toActivity);
        public event ActivityChanging OnActivityChanged;

        public string CurrentSeekImageDisplayed { get; set; } = "man_middle_lantern.jpg";
        public List<string> SeekImages { get; set; } =
            new List<string>() { "man_middle_lantern.jpg", "victorian_woman_lantern.png", "man_lamp.png" };
        public double Radius { get; set; } = 1000;

        // https://stackoverflow.com/questions/46590154/calculate-bearing-between-2-points-with-javascript
        double Bearing(Location userLocation, Location destLocation)
        {
            var startLat = userLocation.Latitude;
            var startLng = userLocation.Longitude;
            var destLat = destLocation.Latitude;
            var destLng = destLocation.Longitude;

            // to radians
            startLat = startLat * (Math.PI / 180);
            startLng = startLng * (Math.PI / 180);
            destLat = destLat * (Math.PI / 180);
            destLng = destLng * (Math.PI / 180);

            var y = Math.Sin(destLng - startLng) * Math.Cos(destLat);
            var x = Math.Cos(startLat) * Math.Sin(destLat) -
                Math.Sin(startLat) * Math.Cos(destLat) * Math.Cos(destLng - startLng);
            var brng = Math.Atan2(y, x);
            brng = brng * 180 / Math.PI;
            var b = (brng + 360) % 360;
            return b;
        }


        public double GetBearing(Location userLocation, Location destLocation)
        {
            // do something to all bearings here if needed

            /* could this be used either to see how much inaccuracy there is in attitude: https://stackoverflow.com/questions/64511933/accuracy-of-device-motion-attitude-quaternion ?, Or
               to adjust all bearings with it?  */
            // var headingAccuracy = DependencyService.Get<IMagnetometer>().HeadingAccuracy;

            return Bearing(userLocation, destLocation);
        }
        /*
        double Normalize(double brng) was needing when trying to use headingAccuracy with bearing..
        {
            return (brng + 360) % 360;
        }
        */

        public double GetDistance(IPlace place)
        {
            double meters = -1;
            try
            {
                meters = Logic.DependencyBox.Get<LocationService>().Location.Value.MetersTo(place.Location);
            }
            catch(Exception exc)
            {
                Logic.Log(exc.Message);
                Logic.FrameworkContext.ReportCrash(null, "When getting distance from user location and place location in meters");
            }
            return meters;
        }

        public Observable<Place> Destination { get; } = new Observable<Place>(null);

    }
    public enum Activities
    {
        Arrow,
        Vision,
        Preferences
    }
    // for showing additional developing related features in the app, like debugging window
    public enum DevConfig
    {
        None, // without dev related stuff
        Debug, // prev development // prev Simulation. Can now adjust provider, amount of places , bearing from  from settings view model
        Gather //gather test data that perfomence can be tested with [searchresults -> visuals]
    }
    // add a development enum value (instead?). which enables toggling between developer and user in settings

    public static class Extensions
    {

        static bool UseImperialUnits(string culture)
        {
            return culture == "en-US" || culture == "en-GB";
        }

        public static string DistanceToString(this MainViewModel mainViewModel, IPlace place)
        {
            if(place == null)
            {
                return "";
            }

            var meters = mainViewModel.GetDistance(place);
            var length = Length.FromMeters(meters);

            var culture = Thread.CurrentThread.CurrentUICulture.ToString();
            if(UseImperialUnits(culture))
            {
                return length.ImperialLengthToString();
            }

            return length.MetricSystemToString();
        }

    }

    public static class UnitExtensions
    {
        // metric system here also

        public static string MetricSystemToString(this Length length)
        {
            var meters = Math.Round(length.Meters);
            Logic.Log(""+meters);
            if (meters > 1000 + 200)
            {
                length = Length.FromKilometers(meters / 1000);
            }

            return length.ToString();
        }

        // UK and US uses are the same
        public static string ImperialLengthToString(this Length length)
        {
            var yards = Math.Round(length.Yards);
            var yardsInAMile = 1760d;
            if (yards > yardsInAMile + 400)
            {
                var miles = (yards / 1760);
                miles = Math.Round(miles, 2);
                if(miles > 2.65d)
                {
                    miles = Math.Round(miles);
                }
                return miles + " mi";
            }
            return yards + " yd";
        }


    }
    // 1.26
    // resturang 2112
    public class BuildException : Exception
    {
        public BuildException(string message) : base(message) { }
    }

}