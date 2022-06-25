using System;
namespace Logic.Game.Models
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
