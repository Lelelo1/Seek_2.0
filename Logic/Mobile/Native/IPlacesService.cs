using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinLogic.Models;
using XamarinLogic.Remote.Apis;

namespace XamarinLogic.Native
{
	public interface IPlacesService : INative, IProvider
	{
		Task<List<Place>> Results(string inputText, Location location, int radius, int maxAmountOfResults);
		Task<List<string>> Autocomplete(string inputText, Location location, int radius, int maxAmountOfResults);
	}

}