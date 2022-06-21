using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Linq;
using Xamarin.Forms;
using System.Collections.Generic;
using XamarinLogic.Native;
using Seek.Droid.Services;
using XamarinLogic;
using Microsoft.AppCenter.Crashes;
using XamarinLogic.Utils;
using static XamarinLogic.Logic;

namespace Seek.Droid
{
    
    [Activity(Label = "Seek", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SetLogger();
            Logic.SetThreading(Device.BeginInvokeOnMainThread, Device.InvokeOnMainThreadAsync);
            Logic.SetNativeDependencies(GetNativeDependencies());

            Forms9Patch.Droid.Settings.Initialize(this);
            TouchEffect.Android.TouchEffectPreserver.Preserve();
            Android_StatusBar.Activity = this;
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        static void SetLogger()
        {
            LogDelegate logger = Logic.iOSLogger;

            if (Device.RuntimePlatform == Device.Android)
            {
                logger = Logic.AndroidLogger; // needed for logs to get to 'application output'
            }

            Logic.SetLogger(logger);
        }

        // ...
        /*
        static void SetAppCenter()
        {
            var runtime = N.Get<iOS_UtilitiesService>().Runtime;

            var appcenterSecret = runtime == Runtime.Production ? Secret.Secrets.Seek.APPCENTER_PRODCUCTION_iOS : Secret.Secrets.Seek.APPCENTER_DEVELOPMENT_iOS;

            try
            {
                AppCenter.Start("ios=" + appcenterSecret, typeof(Crashes), typeof(Microsoft.AppCenter.Analytics.Analytics));
            }
            catch (Exception exc)
            {
                Logic.Log("could not start AppCenter: " + exc.Message);
            }
        }
        */

        static List<INative> GetNativeDependencies()
        {
            // there should be a try catch gettings all native deps!

            // needs to call XamarinForms.Init before using the DependencyService
            // .. but it might not we needed..
            try
            {
                var searchProvider = new Android_PlacesService();// new MockProvider();

                var nativeDependencies = new List<INative>()
                {
                    new Android_Mixpanel(),
                    searchProvider,
                    new Android_AngleOfView(),
                    new Android_StatusBar(),

                    // later to async get/fetch a user id
                    new Android_Utilities()
                };

                return nativeDependencies;
            }
            catch (Exception exc)
            {
                Logic.Log("failed to initialize native dependencies");
                Crashes.TrackError(exc, Error.Properties("When initialising native dependencies of the iOS project"));
                // can native dependencies fail production when they don't in debug, should I account for it..?
                throw exc;
            }
        }
    }
}