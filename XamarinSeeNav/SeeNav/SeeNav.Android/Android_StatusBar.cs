using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SeeNav.Display;
using SeeNav.Droid;
using Xamarin.Forms;


[assembly: Dependency(typeof(Android_StatusBar))]
namespace SeeNav.Droid
{
    class Android_StatusBar : IStatusBar
    {
        public static Activity Activity { get; set; }

        public int GetHeight()
        {
            int statusBarHeight = -1;
            int resourceId = Activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                statusBarHeight = Activity.Resources.GetDimensionPixelSize(resourceId);
            }

            return statusBarHeight;
        }
    }
}