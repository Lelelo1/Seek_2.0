using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XamarinLogic.Models;
using Newtonsoft.Json;
using System.Linq;

namespace XamarinLogic.Services
{
	public class SearchService : IBase
	{
		static TaskCompletionSource<SearchService> Initializing { get; } = new TaskCompletionSource<SearchService>();

		public static async Task<IBase> Init()
		{
			Initializing.SetResult(new SearchService());
			return await Initializing.Task;
		}

		string BaseUrl { get; } = "https://photon.komoot.io/api/?";
		HttpClient Client { get; } = new HttpClient();

		protected SearchService()
		{

		}

		public async Task<List<Place>> _Search(Location userLocation, string phrase)
		{



			var url = BaseUrl + userLocation.ToHttpString() + "&q=" + phrase;
			Logic.Log(url);
			var json = await Client.GetStringAsync(url);

			if (string.IsNullOrEmpty(json))
			{
				return new List<Place>();
			}

			var osmplaces = JsonConvert.DeserializeObject<PhotonResponse>(json)?.GetOsmPlaces();


			return ToNearestUniquePlaces(userLocation, OsmPlace.ToPlaces(phrase, osmplaces));
		}



		static List<Place> ToNearestUniquePlaces(Location userLocation, List<Place> places, int uniqueBy = 2)
		{
			if(places?.Count == 0)
			{
				return places;
			}

			return places.OrderBy(p => userLocation.MetersTo(p.Location)).GroupBy(p => p.Name).Select(g => g.Take(uniqueBy)).SelectMany(g => g).ToList();

		}

		public async Task<List<Place>> Search(Location location, string phrase)
		{
			return await _Search(location, phrase);
		}

		public async Task<List<Place>> Suggestions(Location location, string phrase)
		{
			return await _Search(location, phrase);
		}
	}
}