using System;
using XamarinLogic.Native;

namespace Seek.Display
{
    public interface IStatusBar : INative
    {
        int GetHeight();
    }
}
