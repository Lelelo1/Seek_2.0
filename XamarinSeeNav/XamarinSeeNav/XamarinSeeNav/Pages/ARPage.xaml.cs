using System;
using System.ComponentModel;
using Xamarin.Forms;
using XamarinLogic.Services.PermissionRequired;
using XamarinLogic.Utils;
using Seek.Content;
using Seek.Display;
using XamarinLogic.Models;
using System.Collections.Generic;
using Seek.Controls;
using Seek.Visualization;
using Microsoft.AppCenter.Crashes;
using XamarinLogic.ViewModels;
using XamarinLogic;
using XamarinLogic.Native;
using XamarinLogic.Models.Analytics;
using XamarinLogic.Services;
using Search = Seek.Content.Search;
using XamarinLogic.Game;
using Xamarin.Essentials;

namespace Seek.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ARPage : ContentPage
    {
        /*
        public static Label label;
        public static StackLayout MainContent { get; set; }
        */
        static ARPage instance;
        public static ARPage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ARPage(); // this new is neccesary or crash..
                }
                return instance;
            }
        }

        //public Label DebugLabel => debugLabel;

        public AbsoluteLayout Screen => screen;

        public double StatusBarHeight => Device.RuntimePlatform == Device.iOS ? DependencyService.Get<IStatusBar>().GetHeight() : 0;

        AnalyticsService AnalyticsService { get; set; }
        MainViewModel MainViewModel { get; set; }
        SearchViewModel SearchViewModel { get; set; }

        public ARPage()
        {
            instance = this;
            InitializeComponent();
            BackgroundColor = Xamarin.Forms.Color.Black;
            SetLoadingContent();
            Start();
        }


        // during initialization time of 'Logic' - show same image as in the splash screen
        StackLayout LoadingContent { get; set; }
        void SetLoadingContent()
        {
            LoadingContent = new StackLayout();
            LoadingContent.Spacing = 0;
            var loadingImage = new Image() { Source = ImageSource.FromFile("app_icon.jpeg") };
            loadingImage.HorizontalOptions = LayoutOptions.Center;
            loadingImage.VerticalOptions = LayoutOptions.CenterAndExpand;
            LoadingContent.Add(loadingImage);

            AbsoluteLayout.SetLayoutFlags(LoadingContent, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(LoadingContent, new Rectangle(0, 0, 1, 1));
            Screen.Children.Add(LoadingContent);
        }

        async void Start()
        {
            var successfull = await Logic.SafeInitialization.Task;
            if(successfull == false)
            {
                return; 
            }


            AnalyticsService = Logic.DependencyBox.Get<AnalyticsService>();
            MainViewModel = Logic.DependencyBox.Get<MainViewModel>();
            SearchViewModel = Logic.DependencyBox.Get<SearchViewModel>();

            var camera = await PermissionUtils.GetCamera();
            var locationWhenInUse = await PermissionUtils.GetLocationWhenInUse();
            AnalyticsService.Track(new AppStart(camera.ToString(), locationWhenInUse.ToString()));

            BindingContext = MainViewModel;
            Visualize.Init();

            Detection.Get().Init().Start(new EventRegistration(20));

            if(await PermissionUtils.HasPermissions())
            {
                StartWithPermissions();
            }
            else
            {
                // wait for change user to give permissions in permissions page
                _ = Navigation.PushAsync(new PermissionsPage(StartWithPermissions));
                // note that chaning permissions outside app will restart app
            }
        }


        void StartWithPermissions()
        {
            Logic.DependencyBox.Get<LocationService>().Start();
            SetupContent();
        }

        async void SetupContent() // has all permissions
        {
            Screen.Clear(); // remove loadingContent

            var camera = Camera.Instance;

            var search = Search.Instance; // should always be able to stop or enter new search
            //Extended.Get(search.Background).ShouldFade = () => SearchViewModel.HasPlaces; // <-- needed for 'SearchEntry'

            var arrow = Arrow.Instance;
            Extended.Get(arrow.Layout).IsVisible = false;

            // default visbility set to false, so can there is no delay flickering from 'DestinationChange' and 'PlacesChange'
            Screen.Children.Add(camera.Layout);
            Screen.Children.Add(search.Layout);
            Screen.Children.Add(arrow.Layout);

            // sensor fading configuration
            Extended.Get(arrow.Layout).ShouldFade = () => true;
            SensorFading.Init(/*Extended.Get(search.Background),*/ Extended.Get(arrow.Layout));
            
            SearchViewModel.CurrentInputText.Subscribe(InputTextChange);
            SearchViewModel.Places.Subscribe(PlacesChange);
            MainViewModel.Destination.Subscribe(DestinationChange);
            
            //camera.View.Start(); // can only start when native control has been initialized, by adding it to layout

            var cameraPermission = await PermissionUtils.GetCamera();
            var locationWhenInUsePermission = await PermissionUtils.GetLocationWhenInUse();
            AnalyticsService.Track(new ARPageShown(cameraPermission.ToString(), locationWhenInUsePermission.ToString()));

            Search.Instance.SearchBar.Focus();
        }

        
        public void InputTextChange(string text, int iterator) // deselect arrow when there is no inputtext
        {
            var empty = string.IsNullOrEmpty(text);
            if(empty)
            {
                MainViewModel.Destination.Set(null);
            }
        }
        
        public void DestinationChange(Place destination, int iterator)
        {
            Extended.Get(Arrow.Instance.Layout).IsVisible = Ext.HasValue(destination);
        }
        
        public void PlacesChange(List<Place> places, int iterator) // runs twice on start..?
        {
            var hasPlaces = !Ext.IsNullOrEmpty(places);
            // check to getting from dependencyboc instead!
            //Extended.Get(Search.Instance.Background).IsVisible = !hasPlaces;

            Logic.MainThread.Invoke(() =>
            {
                if(hasPlaces)
                {
                    Camera.Instance.HideOverlay();
                }
                else
                {
                    Camera.Instance.ShowOverlay();
                }
                Search.Instance.IsDisplayed = !hasPlaces;
                Search.Instance.SearchBar.EnableBorder = !hasPlaces;

            });
            
        } 
       
    }
}

// xaml segmentedcontrol (iOS) only:
// xmlns:seg="clr-namespace:Plugin.Segmented.Control;assembly=Plugin.Segmented"
