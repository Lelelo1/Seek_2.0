using System;
using System.Collections.Generic;
using Seek.Display;
using Seek.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOS_StatusBar))]
namespace Seek.iOS
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