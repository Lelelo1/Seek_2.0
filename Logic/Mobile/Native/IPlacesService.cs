using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Models;
using Logic.Remote.Apis;

namespace Logic.Native
{
    public interface IPlacesService : INative, IProvider
    {
        Task<List<Place>> Results(string inputText, Location location, int radius, int maxAmountOfResults);
        Task<List<string>> Autocomplete(string inputText, Location location, int radius, int maxAmountOfResults);
    }

}
