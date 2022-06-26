using System;
using System.Numerics;
using System.Threading.Tasks;
using LogicLibrary;
using LogicLibrary.Models;
using LogicLibrary.Native;
using LogicLibrary.Utils;
using LogicLibrary.ViewModels;
using Seek.Display;
using Seek.Visualization.Support;
using UnitsNet;


namespace XamarinSeeNav
{
    public class FrameworkContext : IFrameworkContext
    {
        public Size BubbleProjectionSize { get; } = new Size(Constants.Size.Width, Constants.Size.Height);

        public Size ProjectionArea { get; } = Display.DisplayUtils.GetDisplaySize(); // wrong: "Width: 828, Height: 1792" new Size(DisplayBase.LogicLibraryScreen.Width, DisplayBase.LogicLibraryScreen.Height);

        public IProjectionAngle ProjectionAngle { get; } = N.Get<IProjectionAngle>();

        // camera sets: 414f, 896f after two renders, th eabsolute layout in Visualize

        public FrameworkContext()
        {
            Xamarin.Essentials.OrientationSensor.ReadingChanged += UpdateOrientation;
            Xamarin.Essentials.OrientationSensor.Start(Xamarin.Essentials.SensorSpeed.UI);
        }

        void UpdateOrientation(object sender, Xamarin.Essentials.OrientationSensorChangedEventArgs e)
        {
            Logic.DependencyBox.Get<MainViewModel>().Orientation.Set(e.Reading.Orientation); // could set required change level needed for update here
        }

        public string GetDistanceImperial(double meters)
        {
            var length = Length.FromMeters(meters);

            // UK and US uses are the same
            var yards = Math.Round(length.Yards);
            var yardsInAMile = 1760d;
            if (yards > yardsInAMile + 400)
            {
                var miles = (yards / 1760);
                miles = Math.Round(miles, 2);
                if (miles > 2.65d)
                {
                    miles = Math.Round(miles);
                }

                var milesString = miles + " mi";

                return milesString;
            }

            var yardsString = yards + " yd";

            return yardsString;
        }

        public string GetDistanceMetric(double meters)
        {
            meters = Math.Round(meters);
            
            var length = meters > 1000 + 200 ? Length.FromKilometers(meters / 1000) : Length.FromMeters(meters);

            return length.ToString();
        }

        public async Task<Location> GetLocationAsync()
        {
            var location = await Xamarin.Essentials.Geolocation.GetLocationAsync();
            return new Location(location.Latitude, location.Longitude);
        }

        public double MetersBetween(Location a, Location b)
        {
            var essentialsLocationA = new Xamarin.Essentials.Location(a.Latitude, a.Longitude);
            var essentialsLocationB = new Xamarin.Essentials.Location(b.Latitude, b.Longitude);

            var meters = Xamarin.Essentials.Location.CalculateDistance(essentialsLocationA, essentialsLocationB, Xamarin.Essentials.DistanceUnits.Kilometers) * 1000;

            return meters;
        }

        public void ReportCrash(Exception exc, string message)
        {
            Microsoft.AppCenter.Crashes.Crashes.TrackError(exc, Error.Properties(message));
        }
    }
}

