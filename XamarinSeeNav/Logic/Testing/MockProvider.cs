using System;
using System.Collections.Generic;
using LogicLibrary.Services.PermissionRequired;
using System.Linq;
using LogicLibrary.Utils;
using LogicLibrary.Remote.Apis;
using LogicLibrary.Models;
using LogicLibrary.Native;

namespace LogicLibrary.Testing
{

    // previously in MockPlaces, which contained photos on azure
    /*
    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/bkburger.jpg",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/bkentry.jpg" +
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/bklogo.jpg",

    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/cnice.jpg",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/cstop.jpg",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/ctraffic.jpg"

    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/mcdmenu.jpg",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/mcdoutdoorsign.jpg"

    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/mbigthreeburgers.jpg",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/minside.png",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/mlogo.png",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/mvegan.jpg"

    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/uavatar.png",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/uguitar.JPG",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/uphoto.JPG"

    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/bkburger.jpg",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/bkentry.jpg" +
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/bklogo.jpg",

    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/cnice.jpg",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/cstop.jpg",
                    "https://storageaccountSeeNavr9a43.blob.core.windows.net/SeeNav-mocks/ctraffic.jpg"

    */
    public class MockProvider : /*IRemote,*/ IProvider
    {
        static double distance = 2000; //get distace from somewhere global
        public List<Place> Places(int amount)
        {

            var search = "mocksearch";
            var id = "noid";
            var places = new List<Place>()
            {
                new Place(search, id, "Burger Ching", MockLocation()),
                new Place(search,id,"Tram D stop", MockLocation()),
                new Place(search,id,"Duck Donalds", MockLocation()), // 10
                new Place(search,id,"Max", MockLocation()),
                new Place(search, id, "My best friend", MockLocation()),
                new Place(search, id, "Burger Ching1", MockLocation()),
                new Place(search,id,"Tram D stop1", MockLocation()),
                new Place(search,id,"Duck Donalds1", MockLocation()), // 10
                new Place(search,id,"Max1", MockLocation()),
                new Place(search, id, "My best friend1", MockLocation()),
                new Place(search, id, "Burger Ching2", MockLocation()),
                new Place(search,id,"Tram D sto2", MockLocation()),
                new Place(search,id,"Duck Donalds2", MockLocation()),
                new Place(search, id, "Burger Ching", MockLocation()),
                new Place(search,id,"Tram D stop", MockLocation()),
                new Place(search,id,"Duck Donalds", MockLocation()), // 10
                new Place(search,id,"Max", MockLocation()),
                new Place(search, id, "My best friend", MockLocation()),
                new Place(search, id, "Burger Ching1", MockLocation()),
                new Place(search,id,"Tram D stop1", MockLocation()),
                new Place(search,id,"Duck Donalds1", MockLocation()), // 10
                new Place(search,id,"Max1", MockLocation()),
                new Place(search, id, "My best friend1", MockLocation()),
                new Place(search, id, "Burger Ching2", MockLocation()),
                new Place(search,id,"Tram D sto2", MockLocation()),
                new Place(search,id,"Duck Donalds2", MockLocation()),
            };
            // show actual direction by avoiding collision detection with one mock place -> visual
            //list.Last().Location.IsFixed = false;

            return places.Take(amount).ToList(); // list.Take().ToList();
        }

        static Location Eriksberg = new Location(57.7027141, 11.916687);

        static Location MockLocation() // random location around the user
        {
            try
            {
                var userLocation = Logic.DependencyBox.Get<LocationService>().Location;
                // var destBearing = new Random().Next(0, 359);

                var start = 0;
                var range = 0;


                var destBearing = (double)new Random().Next(start, start + range);
                destBearing = Calc.Normalize((float)destBearing);

                //Log.Line("destBearing: " + destBearing); // is correct

                var destination = Simulate.Get().__GetRandomDestinationFor__(userLocation.Value, distance, destBearing);

                return destination;
            }
            catch(Exception exc)
            {
                Logic.Log("failed to contruct mock pplaces in mockprovider, creating locations");
                Logic.Log(exc.Message);
            }

            return Eriksberg;
        }
        /*
        public Endpoint Basic(string api, string text, Location location, int radius, int limit)
        {
            throw new NotImplementedException();
        }

        public Endpoint Details(string api, string id)
        {
            throw new NotImplementedException();
        }

        public Endpoint Autocomplete(string api, string text, Location location, int radius)
        {
            throw new NotImplementedException();
        }

        public Endpoint Photos(string api, string id, string photoReference)
        {
            throw new NotImplementedException();
        }
        */
    }
}
