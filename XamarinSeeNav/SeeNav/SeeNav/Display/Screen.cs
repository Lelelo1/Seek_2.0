using System;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace SeeNav.Display
{
    public class Screen
    {
        // make one with status bar taken into account etc, safe area

        public static Size Dimension
        {
            get
            {
                var info = DeviceDisplay.MainDisplayInfo;
                return new Size(info.Width, info.Height);
            }
        }

    }
}
