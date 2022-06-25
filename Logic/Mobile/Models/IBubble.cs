using System;
using Logic.Game.Models;

namespace Logic.Models
{
    public interface IBubble
    {
        Place Place { get; set; }
        Spatial Spatial { get; set; }
    }
}
