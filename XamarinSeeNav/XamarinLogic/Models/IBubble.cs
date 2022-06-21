using System;
using XamarinLogic.Game.Models;

namespace XamarinLogic.Models
{
    public interface IBubble
    {
        Place Place { get; set; }
        Spatial Spatial { get; set; }
    }
}
