using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using System.Collections.Generic;
using LogicLibrary.Native;
using Seek.Droid.Services;
using LogicLibrary;


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
            
            Logic.SetLogger((string message) => System.Diagnostics.Debug.WriteLine(message));
            Logic.SetThreading(Device.BeginInvokeOnMainThread, Device.InvokeOnMainThreadAsync);
            Logic.SetNativeDependencies(GetNativeDependencies());

            //Forms9Patch.Droid.Settings.Initialize(this);
            TouchEffect.Android.TouchEffectPreserver.Preserve();
            Android_StatusBar.Activity = this;
            LoadApplication(new App());
        }

        // possibly needed for permissions with android
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



        static List<INative> GetNativeDependencies()
        {
            var searchProvider = new Android_PlacesService();// new MockProvider();

            var nativeDependencies = new List<INative>()
                {
                    new Android_Mixpanel(),
                    searchProvider,
                    new Android_ProjectionAngle(),
                    new Android_StatusBar(),

                    // later to async get/fetch a user id
                    new Android_Utilities()
                };

            return nativeDependencies;
        }
    }
}