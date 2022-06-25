using System;
using System.Numerics;
using System.Threading.Tasks;
using LogicLibrary;
using LogicLibrary.Models;
using LogicLibrary.Utils;
using LogicLibrary.ViewModels;
using UnitsNet;


namespace XamarinSeeNav
{
    public class FrameworkContext : IFrameworkContext
    {

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
                return miles + " mi";
            }
            return yards + " yd";
        }

        public string GetDistanceMetric(double meters)
        {
            meters = Math.Round(meters);
            Logic.Log("" + meters);
            if (meters > 1000 + 200)
            {
                return Length.FromKilometers(meters / 1000).ToString();
            }

            return Length.FromMeters(meters).ToString();
        }

        public async Task<Location> GetLocationAsync()
        {
            var location = await Xamarin.Essentials.Geolocation.GetLocationAsync();
            return new Location(location.Latitude, location.Latitude);
        }

        public Quaternion GetOrientation()
        {
            throw new NotImplementedException();
        }

        public double MetersBetween(Location a, Location b)
        {
            var essentialsLocationA = new Xamarin.Essentials.Location(a.Latitude, a.Longitude);
            var essentialsLocationB = new Xamarin.Essentials.Location(b.Latitude, b.Longitude);

            return Xamarin.Essentials.Location.CalculateDistance(essentialsLocationA, essentialsLocationB, Xamarin.Essentials.DistanceUnits.Kilometers) * 1000;
        }

        public void ReportCrash(Exception exc, string message)
        {
            Microsoft.AppCenter.Crashes.Crashes.TrackError(exc, Error.Properties(message));
        }
    }
}

