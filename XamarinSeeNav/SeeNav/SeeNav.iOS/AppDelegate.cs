using System;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using LogicLibrary;
using LogicLibrary.Services;
using LogicLibrary.Models.Analytics;
using Xamarin.Forms;
using Microsoft.AppCenter.Crashes;
using LogicLibrary.Utils;
using SeeNav.iOS.Services;
using System.Collections.Generic;
using LogicLibrary.Native;
using Microsoft.AppCenter;
using Device = Xamarin.Forms.Device;

namespace SeeNav.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            PreventLinkerFromStrippingCommonLocalizationReferences();
            global::Xamarin.Forms.Forms.Init();

            Logic.SetLogger(Console.WriteLine);

            // pre init, in which error and crashed can't be tracked by app center
            var utilities = new iOS_UtilitiesService();
            N.Add(utilities);
            SetAppCenter(); // having access to 'runtime' to use the correct app center project: 'production' and 'development'

            Logic.SetThreading(Device.BeginInvokeOnMainThread, Device.InvokeOnMainThreadAsync);

            Logic.SetNativeDependencies(GetNativeDependencies());



            LoadApplication(new App());
            //Forms9Patch.iOS.Settings.Initialize(this);
            return base.FinishedLaunching(app, options);
        }

        [Export("applicationWillTerminate:")]
        override public void WillTerminate(UIApplication application) // should not have 'new' in it! also 'async' don't work 
        {

            var appExit = new AppExit("user");

            Logic.DependencyBox.Get<AnalyticsService>()?.Track(appExit);

            Task.Delay(200).Wait();
            base.WillTerminate(application);
        }


        // native init!

        static void SetAppCenter()
        {
            var runtime = N.Get<iOS_UtilitiesService>().Runtime;

            var appcenterSecret = runtime == Runtime.Production ? Secret.Secrets.SeeNav.APPCENTER_PRODCUCTION_iOS : Secret.Secrets.SeeNav.APPCENTER_DEVELOPMENT_iOS;

            try
            {
                AppCenter.Start("ios=" + appcenterSecret, typeof(Crashes), typeof(Microsoft.AppCenter.Analytics.Analytics));
            }
            catch (Exception exc)
            {
                Logic.Log("could not start AppCenter: " + exc.Message);
            }
        }

        static List<INative> GetNativeDependencies()
        {
            // there should be a try catch gettings all native deps!

            // needs to call XamarinForms.Init before using the DependencyService
            // .. but it might not we needed..
            try
            {
                var searchProvider = new iOS_PlacesService();// new MockProvider();

                var nativeDependencies = new List<INative>()
                {
                    new iOS_Mixpanel(),
                    searchProvider,
                    new iOS_ProjectionAngle(),
                    new iOS_StatusBar()
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
        // https://stackoverflow.com/questions/54238139/not-a-valid-calendar-for-the-given-culture-parameter-name-value
        void PreventLinkerFromStrippingCommonLocalizationReferences()
        {
            _ = new System.Globalization.ChineseLunisolarCalendar();
            _ = new System.Globalization.GregorianCalendar();
            _ = new System.Globalization.HebrewCalendar();
            _ = new System.Globalization.HijriCalendar();
            _ = new System.Globalization.JapaneseCalendar();
            _ = new System.Globalization.JapaneseLunisolarCalendar();
            _ = new System.Globalization.JulianCalendar();
            _ = new System.Globalization.KoreanCalendar();
            _ = new System.Globalization.KoreanLunisolarCalendar();
            _ = new System.Globalization.PersianCalendar();
            _ = new System.Globalization.TaiwanCalendar();
            _ = new System.Globalization.TaiwanLunisolarCalendar();
            _ = new System.Globalization.ThaiBuddhistCalendar();
            _ = new System.Globalization.UmAlQuraCalendar();
        }
    }

}
