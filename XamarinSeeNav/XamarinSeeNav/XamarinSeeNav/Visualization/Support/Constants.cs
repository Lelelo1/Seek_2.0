using System;
using Logic.Game.Models;

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

    public static class RectangleExtensions
    {
        public static Rectangle ToPerceptualRectangle(this Xamarin.Forms.Rectangle rectangle)
        {
            return new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Size ToPerceptualSize(this Xamarin.Forms.Size size)
        {
            return new Size(size.Width, size.Height);
        }

        public static Xamarin.Forms.Rectangle ToFormsRectangle(this Rectangle rectangle)
        {
            return new Xamarin.Forms.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}
