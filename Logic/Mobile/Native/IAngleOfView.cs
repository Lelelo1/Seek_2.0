using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinLogic.Native
{
    public interface IAngleOfView : INative
    {
        // When phone is in potrait. vertical angle is greater than horizontal
        AngleOfView Value { get; }
    }
    /* the camera of the device */
    public class AngleOfView
    {
        // When in potrait!
        public double Horizontal { get; set; } // 48,9999993685205
        public double Vertical { get; set; } // 63,0000011965777
    }
}
