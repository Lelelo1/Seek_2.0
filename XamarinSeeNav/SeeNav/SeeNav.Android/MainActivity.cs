using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using LogicLibrary;
using Xamarin.Forms;
using System.Collections.Generic;
using LogicLibrary.Native;
using LogicLibrary.Testing;

namespace SeeNav.Droid
{
    [Activity(Label = "SeeNav", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Logic.SetLogger((string message) => System.Diagnostics.Debug.WriteLine(message));
            Logic.SetThreading(Device.BeginInvokeOnMainThread, Device.InvokeOnMainThreadAsync);
            Logic.SetNativeDependencies(GetNativeDependencies());


            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        static List<INative> GetNativeDependencies()
        {

            var nativeDependencies = new List<INative>()
                {
                    new Android_Mixpanel(),
                    new Android_ProjectionAngle(),
                    new Android_StatusBar(),
                    new MockProvider(),
                    // later to async get/fetch a user id
                    new Android_Utilities()
                };

            return nativeDependencies;
        }
    }
}
