using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using XamarinLogic.Services.PermissionRequired;
using XamarinLogic.Testing;
using XamarinLogic.Remote.Apis;
using XamarinLogic.Native;
using XamarinLogic.Services;
using XamarinLogic.Models;
using XamarinLogic.Utils;
using System.Linq;

namespace XamarinLogic.ViewModels
{
    public class SearchViewModel : IBase
    {

        static TaskCompletionSource<IBase> Initializing { get; } = new TaskCompletionSource<IBase>();
        public static async Task<IBase> Init(Task<AnalyticsService> initAnalyticsService, Task<SearchService> initSearchService)
        {
            var provider = N.Get<IPlacesService>(); // new MockProvider();

            Initializing.SetResult(new SearchViewModel(provider, await initAnalyticsService, await initSearchService));
            return await Initializing.Task;
        }

        protected SearchViewModel(IProvider provider, AnalyticsService analyticsService, SearchService searchService)
        {

            Provider = provider;


            // blur view is shown when clearing out all text by nulling places
            CurrentInputText.Subscribe(ClearPlacesOnEmptySearchText);

            // can go back into live search->places by subscribing on 'InputText'

            /*
                Observable.Sample(InputText, TimeSpan.FromMilliseconds(200)).Subscribe(
                    (string inputText) => MainThread.Invoke(() => SetSearchWithResults(inputText)) // has to invoked on mainthread to prevent error in uikit (from nativeplaces)
                    );
                */
            SearchService = searchService;
        }

        public SearchService SearchService { get; }

        public IProvider Provider { get; set; }

        public Observable<List<Place>> Places { get; } = new Observable<List<Place>>(null);

        public bool HasPlaces => !Ext.IsNullOrEmpty(Places.Value);

        public int Limit { get; } = 10;

        public Observable<string> CurrentInputText { get; } = new Observable<string>(null);

        public Location CurrentLocation => Logic.DependencyBox.Get<LocationService>().Location.Value;

        public double CrowsDistance => CurrentLocation == null ? 0 : CurrentLocation.MetersTo(SearchLocation); 

        // could be potenially be null, and be initialized when having tapped keyboard
        // is the sessions saved input text
        public List<string> SessionTextHistory { get; } = new List<string>();

        int UniqueCount { get; } = 2;

        public void ClearPlacesOnEmptySearchText(string inputText, int iteration)
        {
            if(string.IsNullOrEmpty(inputText))
            {
                Places.Set(null);
            }
        }

        public async Task<List<Place>> Search(string phrase)
        {

            if(string.IsNullOrEmpty(phrase))
            {
                Places.Set(null);
                return new List<Place>();
            }

            var places = await GetPlaces(phrase);
            if(Ext.IsNullOrEmpty(places)) // (can be be empty list)
            {
                places = null;
                Places.Set(places);
                return places;
            }

            places = NearestTake(NearestDistinct(places));

            Places.Set(places);
            SearchLocation = CurrentLocation;

            return places;
        }

        List<Place> NearestDistinct(List<Place> places)
        {
            return places.OrderBy(g => CurrentLocation.MetersTo(g.Location)).GroupBy(g => g.Name).SelectMany(LocationFilter).ToList();
        }


        // various adjustment. 1 place always shown. many same name places inside radius
        IEnumerable<Place> LocationFilter(IGrouping<string, Place> grouping)
        {
            if (grouping.Count() == 1)
            {
                return grouping;
            }

            var places = grouping.Take(UniqueCount).ToList();

            var nearestPlace = places[0];

            places.RemoveAll(p => CurrentLocation.MetersTo(p.Location) > Radius);

            if (places.Count == 0)
            {
                return new List<Place>() { nearestPlace };
            }

            return places;
        }

        List<Place> NearestTake(List<Place> places)
        {
            return places.OrderBy(g => CurrentLocation.MetersTo(g.Location)).Take(Limit).ToList();
        }

        // Place should contain task of detailsresult
        Task<List<Place>> GetPlaces(string inputText)
        {
            //Log.Line("inputText: " + inputText);

            bool isSupportedProvider = Provider is MockProvider || Provider is IPlacesService;
            if(!isSupportedProvider)
            {
                Logic.Log("you are trying to use provider in search view model that is not supported, currently only native places and mock is supported");
                throw new NotSupportedException("you are trying to use provider in search view model that is not supported, currently only native places and mock is supported");
            }

            if (Provider is MockProvider mock) // is local check (no interpretade json)
            {
                // has to take the instance in saved in searches!
                return Task.FromResult(mock.Places(Limit));
            }

            var nativePlaces = (IPlacesService)Provider;

            var search = nativePlaces.Results(inputText, CurrentLocation, Radius, Limit);

            return search;
        }

        public ObservableCollection<string> TestList { get; set; } = new ObservableCollection<string>() { "one", "two", "three",
            "four", "five", "six", "seven", "eight", "nine", "ten"};


        // Point location = new Point(57.699120, 11.943500); Masthugget
        public int Radius { get; set; } = 3000;

        // place DetailsResult is available after Details with about 0.5 - 1 sec
        // public ObservableCollection<IPlace> Places { get; private set; } = new ObservableCollection<IPlace>();

        // for analytics to track when navigation started
        Location SearchLocation { get; set; }

        public async Task<List<Place>> Autocomplete(string phrase)
        {
            return await SearchService.Suggestions(CurrentLocation, phrase);
        }

        public async Task<List<string>> AppleAutocomplete(string phrase)
        {
            //return await SearchService.Suggestions(CurrentLocation, phrase);
            var nativePlaces = (IPlacesService)Provider;
            var suggestions = (await nativePlaces.Autocomplete(phrase, CurrentLocation, Radius, 15)).Distinct().ToList();
            return suggestions;
        }
    }
}
