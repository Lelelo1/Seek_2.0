using System;
namespace LogicLibrary.Models
{
    public class Size
    {

        public float Width { get; }
        public float Height { get; }

        public Size(double width, double height)
        {
            Width = (float)width;
            Height = (float)height;
        }

        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return "Width: " + Width + ", Height: " + Height;
        }
    }
}

