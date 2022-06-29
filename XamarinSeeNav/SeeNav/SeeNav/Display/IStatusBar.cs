using System;
using LogicLibrary.Native;

namespace SeeNav.Display
{
    public interface IStatusBar : INative
    {
        int GetHeight();
    }
}
