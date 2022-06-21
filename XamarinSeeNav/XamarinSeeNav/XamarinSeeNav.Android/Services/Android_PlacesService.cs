using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinLogic.Models;
using XamarinLogic.Native;

namespace Seek.Droid.Services
{
    public class Android_PlacesService : IPlacesService
    {

        public Task<List<string>> Autocomplete(string inputText, Location location, int radius, int maxAmountOfResults)
        {
            return Task.FromResult(new List<string>());
        }

        public Task<List<Place>> Results(string inputText, Location location, int radius, int maxAmountOfResults)
        {
            return Task.FromResult(new List<Place>());
        }
    }
}
