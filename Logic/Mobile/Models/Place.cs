
using System;
using XamarinLogic.Services.PermissionRequired;


namespace XamarinLogic.Models
{

	// maybe also cache the responses


	// [Observable] does not work to on classes it seems
	public class Place : IPlace
	{
		public string Search { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public Location Location { get; set; }
		//public ObservableCollectionExtended<string> Photos { get; set; }
		public bool IsDestination { get; set; }
		public bool SeenTimes { get; set; }
		public double InitialDistance { get; set; }
		public double CurrentDistance { get; set; }

		public Place(string search, IResult result) // is there a reason to keep 'IResult' ?
		{
			Search = search;
			Id = result.Id;
			Name = result.Name;
			Location = result.Location;
			IsDestination = false;

			UpdateCurrentDistance();
		}

		public Place(string search, string id, string name, Location location)
		{
			Search = search;
			Id = id;
			Name = name;
			Location = location;
			IsDestination = false;

			UpdateCurrentDistance();
		}

		void UpdateCurrentDistance()
		{
			// track initial and currnt distance to the place, for analtyics and maybe also rest of application
			var userLocation = Logic.DependencyBox.Get<LocationService>().Location;
			InitialDistance = userLocation.Value.MetersTo(Location);
			userLocation.Subscribe((newUserLocation, _) =>
			{
				CurrentDistance = newUserLocation.MetersTo(Location);
			});
		}
	}

	public interface IPlace
	{
		string Search { get; set; }
		string Id { get; set; }
		string Name { get; set; }
		Location Location { get; set; }
		//ObservableCollectionExtended<string> Photos { get; set; }
		bool IsDestination { get; set; }
		bool SeenTimes { get; set; }
		double InitialDistance { get; set; }
		double CurrentDistance { get; set; }
	}
	/*
	public enum Kind
	{
		Place, // everything should be plave right now
		Custom,
		Contact,
		Commute
	}
	*/

}