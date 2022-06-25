using System;
using System.Threading.Tasks;
using MapKit;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using LogicLibrary.Models;
using LogicLibrary.Utils;
using Microsoft.AppCenter.Crashes;
using LogicLibrary.Native;
using LogicLibrary;
using Foundation;

[assembly: Dependency(typeof(Seek.iOS.Services.iOS_PlacesService))]
namespace Seek.iOS.Services
{
    public class iOS_PlacesService : IPlacesService
    {

        // note that the bias/region is not 100% restrictive in the api

        // returns null if there is a if there is a problem, empty list for no search results
        // https://developer.apple.com/documentation/mapkit/mklocalsearchrequest#//apple_ref/doc/uid/TP40012892
        public async Task<List<Place>> Results(string inputText, Location location, int radius, int maxAmountOfResults)
        {

            var region = GetMKCoordinateRegion(location, radius);

            region.Span = new MKCoordinateSpan(1, 1);
   
            var request = new MKLocalSearchRequest();
            request.NaturalLanguageQuery = inputText;

            request.Region = region;

            var search = new MKLocalSearch(request);
            MKLocalSearchResponse response = null;

            try
            {
                response = await search.StartAsync();
            } // // slow 'Zzzz' eg throws mkerror when not found Code=4: https://developer.apple.com/documentation/mapkit/mkerror/code/placemarknotfound
            catch (Exception exc) // 'Zzz' does not throw, even though no places found (in both cases.. weird)
            {
                //placemarkNotFound
                return null;
            }

            if(response == null)
            {
                var errorProperties = Error.Properties("Native iOS search did not get a response from MKLocalSearchAPI");
                errorProperties.Add("inputText", inputText);
                errorProperties.Add("location", location.ToString());
                Crashes.TrackError(new Exception("iOS PlacesService did not get a response for the request"), errorProperties);
                return null;
            }

            if(response.MapItems == null || response.MapItems.Count() == 0)
            {
                return new List<Place>();
            }

            var items = response.MapItems.ToList();


            // so it matches autocomplete
            //if(items.Count > 1)
            //{
            //    items = HardCustomBiasRadius(items, location, radius);
            //}

            // tak enearest

            List<Place> places = null;

            try
            {
                places = items.Select((item) => new Place(inputText, "noID", item.Name, new Location(item.Placemark.Coordinate.Latitude, item.Placemark.Coordinate.Longitude))).ToList();
            }
            catch(Exception exc)
            {
                var msg = "problem occued in ios native places service when creating list of places from list of MKMapItems";
                LogicLibrary.Log(msg);
                var errorProperties = Error.Properties("Creating Places");
                errorProperties.Add("message", msg);
                Crashes.TrackError(exc, errorProperties);
                return null;
            }

            return places;
        }
        List<MKMapItem> HardCustomBiasRadius(List<MKMapItem> items, Location location, int radius)
        {
            return items.Where((item) => // manual filter, as some places showing outside given radius as mentioned
            {
                var placeLocation = new Location(item.Placemark.Location.Coordinate.Latitude, item.Placemark.Location.Coordinate.Longitude);
                var metersDistance = location.MetersTo(placeLocation);
                return metersDistance < radius;
            }).ToList();
        }

        /*
        // https://stackoverflow.com/questions/7477003/calculating-new-longitude-latitude-from-old-n-meters
        static double EarthRadiusKm { get; set; } = 6378;
        public double LatUnit(Location location)
        {
            return (2 * Math.PI / 360) * EarthRadiusKm * Math.Cos(location.Latitude) / 1000;
        }
        public double LonUnit()
        {
            return (2 * Math.PI / 360) * EarthRadiusKm / 1000;
        }
        */

        // https://stackoverflow.com/questions/33380711/how-to-implement-auto-complete-for-address-using-apple-map-kit
        public async Task<List<string>> Autocomplete(string inputText, Location location, int radius, int maxAmountOfResults)
        {
            var searchCompleter = new MKLocalSearchCompleter();
            var autocompleteSearch = new Autocompleter();
            searchCompleter.Delegate = autocompleteSearch;
            searchCompleter.Region = GetMKCoordinateRegion(location, radius);

            searchCompleter.QueryFragment = inputText;

            return await autocompleteSearch.PossibleResults.Task;
        }

        MKCoordinateRegion GetMKCoordinateRegion(Location location, int radius)
        {
            var center = new CoreLocation.CLLocationCoordinate2D(location.Latitude, location.Longitude);
            // MKCoordinateRegion.FromDistance settings meters is broken

            var locationbias = MKCoordinateRegion.FromDistance(center, radius, radius);

            return locationbias;
        }
    }

    public class Autocompleter : MKLocalSearchCompleterDelegate
    {

        public TaskCompletionSource<List<string>> PossibleResults { get; }

        public Autocompleter()
        {
            PossibleResults = new TaskCompletionSource<List<string>>();
        }

        [Export("completerDidUpdateResults:")]
        public void DidUpdateResults(MKLocalSearchCompleter completer)
        {
            PossibleResults.SetResult(completer.Results.Select(a => a.Title).ToList());
        }
    }
}
