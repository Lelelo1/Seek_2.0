using System;
using Xamarin.Essentials;

namespace XamarinSeeNav.Display
{
    public class DisplayUtils
    {
        protected DisplayUtils()
        {
        }
        // https://stackoverflow.com/questions/38891654/get-current-screen-width-in-xamarin-forms
        public static LogicLibrary.Models.Size GetDisplaySize()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            // Width (in pixels)
            var width = mainDisplayInfo.Width;

            // Width (in xamarin.forms units)
            var xamarinWidth = width / mainDisplayInfo.Density;

            // Height (in pixels)
            var xamarinHeight = mainDisplayInfo.Height / mainDisplayInfo.Density;

            var logicSize = new LogicLibrary.Models.Size(xamarinWidth, xamarinHeight);

            // should match the device screen size and be 'Width: 414, Height: 896' on iPhone11 pro max
            //LogicLibrary.Logic.Log("displaySize: " + logicSize);

            return logicSize;
        }
    }
}

