using System;
namespace LogicLibrary.Models
{
    public class Rectangle
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        override
        public string ToString() => "x: " + X + ", y: " + Y + " width: " + Width + " height: " + Height; 
    }
}
