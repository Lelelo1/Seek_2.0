using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Utils;



namespace Logic.Models
{
    public class Geometry
    {
        public IList<double> coordinates { get; set; }
        public string type { get; set; }

        // not in photon the order is reversed for some reason
        public Location ToLocation() => new Location(coordinates[1], coordinates[0]);
    }

    public class Properties
    {
        public object osm_id { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string countrycode { get; set; }
        public string postcode { get; set; }
        public string locality { get; set; }
        public string county { get; set; }
        public string type { get; set; }
        public string osm_type { get; set; }
        public string osm_key { get; set; }
        public string housenumber { get; set; }
        public string street { get; set; }
        public string district { get; set; }
        public string osm_value { get; set; }
        public string name { get; set; }
        public IList<double> extent { get; set; }
    }

    public class Feature
    {
        public Geometry geometry { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }

        public int HashIndex { get; set; }
    }

    public class PhotonResponse
    {
        public IList<Feature> features { get; set; }
        public string type { get; set; }

        // public List<Place> ToPlaces(string phrase) => features.Select(f => new Place(phrase, "id", f.properties?.name, f.geometry.ToLocation())).ToList();
        
        public List<OsmPlace> GetOsmPlaces()
        {

            var b = SetHashIndexes(RemoveProvinces(features)).GroupBy(f => f, new MergeComparer()).Select(p => Merge(p)).ToList();
            
            return b;
        }

        public OsmPlace Merge(IEnumerable<Feature> features)
        {

            var name = features.Select(f => f.properties.name).FirstOrDefault() ?? "";
            var location = features.Select(f => f.geometry.ToLocation()).FirstOrDefault() ?? new Location(0, 0); // photon feature should never be missing location!

            var keys = features.Select(f => f.properties.osm_key);
            var values = features.Select(f => f.properties.osm_value);

            return new OsmPlace(name, location, Ext.CreateDictionary(keys, values));
        }

        public static IEnumerable<Feature> SetHashIndexes(IEnumerable<Feature> features)
        {
            var featuresList = features.ToList();
            for (int i = 0; i < featuresList.Count; i++)
            {
                featuresList[i].HashIndex = i;
            }
            return featuresList;
        }

        public static IEnumerable<Feature> RemoveProvinces(IEnumerable<Feature> features)
        {
            return features.Where(f => f.properties.type != "state");
        }
    }

    public class MergeComparer : IEqualityComparer<Feature>
    {
        double Near { get; } = 500;
        public bool Equals(Feature x, Feature y)
        {
            var xLocation = x.geometry.ToLocation();
            var yLocation = y.geometry.ToLocation();
            var xName = x.properties.name;
            var yName = y.properties.name;

            return Logic.FrameworkContext.MetersBetween(xLocation, yLocation) <= Near && xName == yName;
        }

        public int GetHashCode(Feature obj)
        {
            return 0;
        }
    }
}
