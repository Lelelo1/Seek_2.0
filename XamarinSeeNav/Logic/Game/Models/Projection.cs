using System;
namespace LogicLibrary.Models
{
    public class Projection
    {
        public Rectangle Rectangle { get; }

        public Projection(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }
    }
}
