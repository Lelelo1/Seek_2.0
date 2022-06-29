using System;
using Xamarin.Forms;
using LogicLibrary;
using LogicLibrary.Native;
using LogicLibrary.Services;
using Microsoft.AppCenter.Crashes;
using LogicLibrary.Models.Analytics;
using LogicLibrary.ViewModels;
using LogicLibrary.Utils;
using LogicLibrary.Services.PermissionRequired;
using SeeNav.Visualization.Support;
using SeeNav.Display;
using LogicLibrary.Game;
using SeeNav.Pages;
using XamarinSeeNav;

namespace SeeNav
{
    public partial class App : Application
    {
        // assumes all native dependencies hae been initiated the native project
        public App()
        {
            InitializeComponent();
            Logic.Init(new FrameworkContext());
            Xamarin.Essentials.DeviceDisplay.KeepScreenOn = true;
            MainPage = new NavigationPage(ARPage.Instance);
            
        }

        protected override void OnStart()
        {
            Lifecycle.InvokeAppStart();
        }

        protected override void OnSleep()
        {
            
            var searchViewModel = Logic.DependencyBox.Get<SearchViewModel>();
            var sessionText = searchViewModel.SessionTextHistory;

            var currentLocation = Logic.DependencyBox.Get<LocationService>().Location.Value;

            var crowsDistance = searchViewModel.CrowsDistance;

            var appLeavingEvent = new AppLeaving(AppLeaving.ToText(sessionText), currentLocation?.ToString(), crowsDistance);
            
            Logic.DependencyBox.Get<AnalyticsService>().Track(appLeavingEvent);

        }


        protected override void OnResume()
        {
            Lifecycle.InvokeAppResume();
            // Handle when your app resumes
            Logic.DependencyBox.Get<AnalyticsService>().Track(new AppResume());
            
        }
    }
}
