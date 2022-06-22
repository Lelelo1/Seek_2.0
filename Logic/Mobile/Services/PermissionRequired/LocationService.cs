using System;
using XamarinLogic.Utils;
using XamarinLogic.Native;
using XamarinLogic.Models;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Essentials;

namespace XamarinLogic.Services.PermissionRequired
{
    // only access this service after it has been started, currently in 'MainViewModel'
    public class LocationService : IBase
    {

        static TaskCompletionSource<IBase> Initializing { get; } = new TaskCompletionSource<IBase>();

        public static Task<IBase> Init()
        {
            Initializing.SetResult(new LocationService());

            return Initializing.Task;
        }

        protected LocationService()
        {
        }

        public Observable<Models.Location> Location { get; } = new Observable<Models.Location>(null); // init to a value

        //public TaskCompletionSource<bool> HasLocation = new TaskCompletionSource<bool>();

        CancellationTokenSource Cancellation { get; set; }

        public bool IsActive => Cancellation != null;

        public void Start()
        {
            var locationUpdater = new PeriodicTask(TimeSpan.FromSeconds(1));
            Cancellation = locationUpdater.CancellationTokenSource;

            _ = locationUpdater.Run(async() =>
            {
                var location = await Geolocation.GetLocationAsync();
                if(location == null)
                {
                    return;
                }
                Console.WriteLine(location);
                Location.Set(new Models.Location(location.Latitude, location.Longitude));
            });
        }
        public void Stop()
        {
            Cancellation.Cancel();
            Cancellation = null;
        }

        public void SetLatitude(double latitude)
        {
            Location.Set(new Models.Location(latitude, Location.Value.Longitude));
        }

        public void SetLongitude(double longitude)
        {
            Location.Set(new Models.Location(Location.Value.Latitude, longitude));
        }

    }
}
