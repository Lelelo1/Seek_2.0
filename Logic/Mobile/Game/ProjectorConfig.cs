using System;
using Logic.Game.Models;

namespace Logic.Game
{
    public class ProjectorConfig
    {
        public Size ItemSize { get; }
        public Size ViewSize { get; }
        public ProjectorConfig(Size item, Size view)
        {
            ItemSize = item;
            ViewSize = view;
        }
    }
}
