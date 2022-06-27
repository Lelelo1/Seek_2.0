using System;
using LogicLibrary.Game.Models;

namespace LogicLibrary.Models
{
    public interface IBubble
    {
        Place Place { get; set; }
        Spatial Spatial { get; set; }
    }
}
