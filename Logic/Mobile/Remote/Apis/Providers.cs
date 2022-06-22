using System;
using System.Collections.Generic;
using XamarinLogic.Models;
using XamarinLogic.Native;



namespace XamarinLogic.Remote.Apis
{

	public interface IProvider
	{

	}

	// provider is not needed anymore, and it is better to use existing url builder, as a nuget. But try to fuse upcoming
	// seek-search with INative (and the existing apple mapkit native implementation)

	/*
	// for google and foursquare
	public interface IRemote
	{
		// made a search method here for each type of search
		Endpoint Basic(string api, string text, Location location, int radius, int limit);
		Endpoint Details(string api, string id); // id of place or venue
		Endpoint Autocomplete(string api, string text, Location location, int radius);
		Endpoint Photos(string api, string id, string photoReference);
	}
	*/
	/*
	public class Endpoint
	{
		public string BaseUrl { get; set; }
		public Dictionary<string, string> MandatoryParams { get; set; }
		public Dictionary<string, string> ExtraParams { get; set; }
		public List<string> Extras { get; set; }
		public Endpoint(IArguments args)
		{
			BaseUrl = args?.BaseUrl;
			MandatoryParams = args?.MandatoryParams;
			ExtraParams = args?.ExtraParams;
			Extras = args?.Extras;
		}
	}
	*/

}