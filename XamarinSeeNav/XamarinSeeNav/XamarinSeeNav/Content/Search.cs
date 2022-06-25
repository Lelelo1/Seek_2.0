using System;
using Seek.Display;
using Xamarin.Forms;
using LogicLibrary.Utils;
using LogicLibrary.ViewModels;
using LogicLibrary;
using LogicLibrary.Services;
using LogicLibrary.Models.Analytics;
using Color = Xamarin.Forms.Color;
//using Seek.Main;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using LogicLibrary.Services.PermissionRequired;
using System.Collections.Generic;
using Seek.Controls; 
using System.Linq;
using LogicLibrary.Models;
using Seek.Pages;
// for dumb reasons SearcBar needs to be both manualy sized and positioned to use it inside a AbsoluteLayout
// this means I have to know the size of SearchBar before using 'AbsoluteLayout.SetLayoutBounds'
// this means I will use another layout, and this will break 'SearchEntry', switch back if needed.
// The SearchBar actual size is fixed, and setting size larger it simply created white/grey areas around itself


namespace Seek.Content
{
    public class Search
    {
        static Search instance;
        public static Search Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Search();
                }
                return instance;
            }
        }

        public StackLayout Layout { get; } = new StackLayout();
        //public ColoredArea Background { get; }
        //public SearchEntry SearchEntry { get; }

        public Seek.Controls.SearchBar SearchBar { get; }

        Autocomplete Autocomplete { get; set; }

        SearchViewModel SearchViewModel { get; }

        // new Color(105, 92, 55, 72); // the color prviously used with 'SearchEntry' is being too aparent
        //Color BackgroundColor { get; } = Color.Default;//Color.FromRgba(255, 255, 255, 80);
        // default looks super cool and lean on iOS
        // color default and transparent does the same. It looks really nice to blend it in into the
        // searchbar in in to the camera blur

        Color GetBackgroundColor(bool display)
        {
            return display ? Color.FromRgba(255, 255, 255, 120) : Color.Default;
        }

        Color GetTextColor(bool display)
        {
            return display ? Color.Black : Color.White;

        }

        Color PlaceholderColor { get; } = Color.FromRgba(0, 0, 0, 40); // tried 15 and 250 not altering unicode icon transperency

        Color GetSearchIconColor(bool display)
        {
            return GetTextColor(display);
        }

        protected Search()
        {
            SearchBar = new Seek.Controls.SearchBar() {
                EnableBorder = true,
                TextColor = GetTextColor(IsDisplayed),
                PlaceholderColor = PlaceholderColor,
                SearchIconColor = GetSearchIconColor(isDisplayed)
            };

            var searchBar = SearchBar;//SearchEntry.Entry; // <-- shift back to custom entry here!
            
            SearchViewModel = Logic.DependencyBox.Get<SearchViewModel>();

            searchBar.TextChanged += OnTextChange;
            searchBar.SearchButtonPressed += SubmitText;


            //var clear = SearchEntry.Clear;

            //Layout.Add(Background, entry, clear);
            //ar farme = new SearchBar(frame)
            /*
            var statusBarHeight = MainPage.Instance.StatusBarHeight;
            AbsoluteLayout.SetLayoutBounds(searchBar, new Rectangle(0, statusBarHeight, Screen.Dimension.Width, 120));
            */

            searchBar.Margin = new Thickness(0, ARPage.Instance.StatusBarHeight, 0, 0);
            
            Layout.Add(searchBar);
            SearchBar.Focused += OnKeyboardFocus;

            //switch between the colors when search! looks nice

            SearchBar.On<iOS>().SetShadowColor(Color.Black);
            SearchBar.On<iOS>().SetShadowOffset(new Size(5, 5));
            SearchBar.On<iOS>().SetShadowRadius(7); // higher values waters it out

            SearchBar.On<iOS>().SetIsShadowEnabled(true);

            Autocomplete = new Autocomplete(SuggestionSearch);
            Layout.Add(Autocomplete.Layout);
        }

        async void SuggestionSearch(string suggestion)
        {
            Logic.Log("SuggestionSearch: " + suggestion);
            //change text without firing textchange event: https://stackoverflow.com/questions/27763110/changing-textbox-text-without-firing-textchanged-event
            SearchBar.TextChanged -= OnTextChange;
            SearchBar.Text = suggestion;
            SearchBar.TextChanged += OnTextChange;

            var results = await SearchViewModel.Search(suggestion);

            SendSearchAnalyticsEvent(results, SearchBar.Text, SearchTrigger.Autocomplete);

            CloseAutomplete();
        }

        public void OnTextChange(object sender, TextChangedEventArgs e)
        {
            
            var text = e.NewTextValue;
            var lastText = SearchViewModel.SessionTextHistory.LastOrDefault();

            SearchViewModel.SessionTextHistory.Add(text);

 
            Autocomplete.SetSuggestions(text, GetSuggestionTextColor());

        }

        Color GetSuggestionTextColor()
        {
            return SearchViewModel.Places.Value?.Count > 0 ? Color.White : Color.Black;
        }

        void OnKeyboardFocus(object sender, FocusEventArgs e)
        {
            SearchBar.Placeholder = null;

            Logic.DependencyBox.Get<AnalyticsService>().Track
            (
                new KeyboardOpened()
            );
        }

        async void SubmitText(object sender, EventArgs e)
        {
            var searchBar = (Controls.SearchBar)sender;

            var places = await SearchViewModel.Search(searchBar.Text);
            SendSearchAnalyticsEvent(places, searchBar.Text, SearchTrigger.Submit);

            CloseAutomplete();
        }

        void SendSearchAnalyticsEvent(List<Place> results, string inputText, SearchTrigger searchType)
        {
            
            // null places indicates failture in the. it will show up as -1 int in analytics 
            int count = Ext.HasValue(results) ? results.Count : -1;
            var location = Logic.DependencyBox.Get<LocationService>().Location.Value;
            var analyticsEvent = new LogicLibrary.Models.Analytics.Search(inputText, count, location.ToString(), searchType.ToString());

            Logic.DependencyBox.Get<AnalyticsService>().Track(analyticsEvent);
        }

        // isDisplayed should maybe be renamed, i'ts set with hasplaces
        bool isDisplayed = true;
        public bool IsDisplayed 
        {
            get => isDisplayed;
            set
            {
                SearchBar.TextFieldColor = GetBackgroundColor(value);
                SearchBar.TextColor = GetTextColor(value);
                SearchBar.SearchIconColor = GetSearchIconColor(value);
                isDisplayed = value;
            }
        }

        void CloseAutomplete()
        {
            Autocomplete.SetSuggestions(null, GetSuggestionTextColor());
        }

        // backgroudn color is broken on ios
        // here is some platform effects that can be used for shadow and backrgound: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/platform/ios/searchbar-style
        // they don't work
        // SearchBar.On<iOS>().SetSearchBarStyle(UISearchBarStyle.Minimal);
    }
}
