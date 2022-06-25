using System;
using System.Linq;
using Logic.Models;
using Logic.Native;
using Logic.Services.PermissionRequired;
using Xamarin.Forms;
using Logic;
using Logic.Game;

namespace Seek
{

    // methods to handle the logic project's services and use 'Xamarin.Forms' package only in this (forms) project

    public static class ServiceExtensions
    {


        // potentially add the rectangle conversions from constants, in here

        public static Detection Init(this Detection detection)
        {
            Detection.AngleOfView = DependencyService.Get<IAngleOfView>();
            return detection;
        }

        
        // previsouly UtilitiesExtensions in Logic project
        static string ShownTurtorial { get; set; } = "ShownTurtorial";

        public static bool HasShownTurtotial()
        {
            var keys = Application.Current.Properties.Keys.ToList().Find((key) => key == ShownTurtorial);
            return keys != null;
        }
        public static void SetHasShownTurtorial()
        {
            Application.Current.Properties.Add(ShownTurtorial, true);
        }
        
        static LocationService LocationService = Logic.DependencyBox.Get<LocationService>();
        // LocationService tests..
        // test switching to different locations by tapping a button. also disables underlaying location updated made per second
        static Location Lindholmen = new Location(57.706970, 11.938020);
        static Location Eriksberg = new Location(57.7027141, 11.916687);
        static bool use;
        public static Location SwitchLocation
        {
            get
            {
                if(LocationService.IsActive)
                {
                    LocationService.Stop();
                }
                use = !use; return use ? Lindholmen : Eriksberg;
            }
        }
        // test animating/transport between two locations during given time span
        static View AnimationTrigger = new Button();
        static void FadeLocation(this LocationService locationService, TimeSpan time, bool toThere, Action onFinished = null)
        {

            locationService.Stop();
            Animation latAnimation = null;
            Animation lonAnimation = null;

            Action<double> latSetter = (double latitude) => { locationService.SetLatitude(latitude); };
            Action<double> lonSetter = (double longitude) => { locationService.SetLongitude(longitude); };

            if (toThere)
            {
                latAnimation = new Animation(latSetter, Eriksberg.Latitude, Lindholmen.Latitude);
                lonAnimation = new Animation(lonSetter, Eriksberg.Longitude, Lindholmen.Longitude);
            }
            else
            {
                latAnimation = new Animation(latSetter, Lindholmen.Latitude, Eriksberg.Latitude);
                lonAnimation = new Animation(lonSetter, Lindholmen.Longitude, Eriksberg.Longitude);
            }
            AnimationTrigger.Animate(nameof(latAnimation), latAnimation, length: (uint)time.TotalMilliseconds);
            AnimationTrigger.Animate(nameof(lonAnimation), lonAnimation, length: (uint)time.TotalMilliseconds, finished: (d, b) =>
            {
                Console.WriteLine("finished animating location from eriksberg to lindholmen at: " + locationService.Location);
                onFinished?.Invoke();
            });
        }

        public static void FadeLocationThereAndBack(TimeSpan totalTime)
        {
            var partTime = TimeSpan.FromMilliseconds(totalTime.TotalMilliseconds / 2);
            FadeLocation(LocationService, partTime, true, () =>
            {
                FadeLocation(LocationService, partTime, false);
            });
        }
    
    }


    // Put all in IBase dependencies into
    /*
    public class T<A> where A : class
    {
        public static A Instance => DependencyService.Get<A>();
    }
    */
}

