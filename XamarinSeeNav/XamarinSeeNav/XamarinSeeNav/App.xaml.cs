﻿using System;
using Xamarin.Forms;
using XamarinLogic;
using XamarinLogic.Native;
using XamarinLogic.Services;
using Microsoft.AppCenter.Crashes;
using XamarinLogic.Models.Analytics;
using XamarinLogic.ViewModels;
using XamarinLogic.Utils;
using XamarinLogic.Services.PermissionRequired;
using Seek.Visualization.Support;
using Seek.Display;
using XamarinLogic.Game;
using Seek.Pages;

namespace Seek
{
    public partial class App : Application
    {
        // assumes all native dependencies hae been initiated the native project
        public App()
        {
            InitializeComponent();
            
            var projectionConfig = new ProjectorConfig(Constants.Size, DisplayBase.LogicScreen);
            Logic.Init(projectionConfig); // possibly put in thread.. I don't get exceptions then though..?
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
