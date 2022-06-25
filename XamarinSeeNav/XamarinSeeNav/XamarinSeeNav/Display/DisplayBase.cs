using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Seek.Visualization.Support;
namespace Seek.Display
{
    public class DisplayBase
    {
        protected Rectangle Screen => GetScreen();

        static Rectangle GetScreen()
        {
            var info = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            return new Rectangle(0, 0, info.Width, info.Height);
        }

        static Size GetScreenSize()
        {
            var info = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            return new Size(info.Width, info.Height);
        }

        public static  Logic.Game.Models.Size LogicScreen => GetScreenSize().ToPerceptualSize();

        // can also makde safe area property, by places things below status bar etc
        protected DisplayBase()
        {
            

        }
    }

}
