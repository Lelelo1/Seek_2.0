using System;
using LogicLibrary.Native;

namespace Seek.Display
{
    public interface IStatusBar : INative
    {
        int GetHeight();
    }
}
