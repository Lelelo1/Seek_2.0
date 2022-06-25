using System;
using Logic.Native;

namespace Seek.Display
{
    public interface IStatusBar : INative
    {
        int GetHeight();
    }
}
