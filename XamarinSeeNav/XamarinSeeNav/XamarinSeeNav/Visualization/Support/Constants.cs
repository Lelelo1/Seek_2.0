using System;
using LogicLibrary.Game.Models;
using LogicLibrary.Models;

//using Xamarin.Forms;

// refactor this when possible!!
namespace Seek.Visualization.Support
{
    public class Constants
    {
        // might increase size somewhat on ios or take percentage of screen size and use it
        static double size = 110;
        public static Size Size = new Size(size, size);
        public static float CornerRadius_Normal = 45;
        public static float CornerRadius_Enlarged = 0;
        public class Animation
        {
            // milliseconds
  
            //public static uint Size_Deselect_Duration = 700;

            //public static uint Path_Select_Duration = 200;


            // public static uint Path_Deselect_Offset_Duration = 300;


        }

    }

}
