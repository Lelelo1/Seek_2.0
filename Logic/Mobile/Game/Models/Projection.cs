using System;
namespace LogicLibrary.Game.Models
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
