using System;
using Geo;

namespace XamarinLogic.Models
{

    public class Location
    {
        public double Latitude { get; }
        public double Longitude { get; }

        /*
        public Location() // to attach whesn any value. remoev and use delegate!!
        {

        }
        */

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }


        public override string ToString()
        {
            return Latitude + ", " + Longitude;
        }

        // has to be static to prevent lat 0 lon 0 scenario

        public double MetersTo(Location to) 
        {

            if(to == null)
            {
                return 0;
            }

            // what does route do in geo nuget package?
            var f = new Coordinate(Latitude, Longitude);
            var t = new Coordinate(to.Latitude, to.Longitude);
            // there are 4 different ways to distance calculation, pick the right one
            var geodeticLine = Geo.Geodesy.GeodeticCalculations.CalculateShortestLine(f, t); // <-- seems correct
            if(geodeticLine == null)
            {
                return 0;
            }
            var d = geodeticLine.Distance.Value; // unit is 'M'
            return d;
        }

        public string ToHttpString() => "lat=" + Latitude + "&lon=" + Longitude;
    }


}
