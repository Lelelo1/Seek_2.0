using System;
using System.Collections.Generic;
using SeeNav.Display;
using SeeNav.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOS_StatusBar))]
namespace SeeNav.iOS
{
    class iOS_StatusBar : IStatusBar
    {
        public int GetHeight()
        {
            WebKit.WKFindResult s;

            return (int)UIApplication.SharedApplication.StatusBarFrame.Height;
        }
    }
}