using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Models
{
    public class OsmPlace
    {
        public string Name { get; }
        public Location Location { get; }
        public Dictionary<string, string> Tags { get; }
        public OsmPlace(string name, Location location, Dictionary<string, string> tags)
        {
            Name = name;
            Location = location;
            Tags = tags;
        }

        public static List<Place> ToPlaces(string search, List<OsmPlace> osmPlaces)
        {
            return osmPlaces.Select(op => new Place(search, "", op.Name, op.Location)).ToList();
        }

    }
}
