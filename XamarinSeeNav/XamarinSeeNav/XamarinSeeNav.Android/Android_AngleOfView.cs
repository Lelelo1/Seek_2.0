using System;
using Android.Hardware.Camera2;

using Xamarin.Forms;
using Java.Lang.Reflect;
using Seek.Droid.Controls.Camera2Forms.Camera2;
using LogicLibrary.Native;

[assembly: Dependency(typeof(Seek.Droid.Android_AngleOfView))]
namespace Seek.Droid
{
    public class Android_AngleOfView : IAngleOfView
    {
        public AngleOfView Value { get; } = new AngleOfView();

        public Android_AngleOfView()
        {
            
            var cameraInstance = CameraDroid.Get();
            // It should be possible Hardware.Camera just to get fov  https://stackoverflow.com/questions/31172794/get-angle-of-view-of-android-camera-device/31640287
            //Android.Hardware.Camera.CameraInfo.
            // https://stackoverflow.com/questions/52828668/how-to-calculate-field-of-view-in-arcore

            var context = cameraInstance.Context;
            var characteristics = cameraInstance.CameraManager.GetCameraCharacteristics(cameraInstance.CameraId);


            var maxFocus = (float[]) characteristics.Get(CameraCharacteristics.LensInfoAvailableFocalLengths); // LENS_INFO_AVAILABLE_FOCAL_LENGTHS
            var size = (Android.Util.SizeF) characteristics.Get(CameraCharacteristics.SensorInfoPhysicalSize); // SENSOR_INFO_PHYSICAL_SIZE
            
            var w = size.Width;
            var h = size.Height;
            
            var toDeg = 180 / Math.PI;
            var fovW = toDeg * (2 * Math.Atan(w / (maxFocus[0] * 2.0)));
            var fovH = toDeg * (2 * Math.Atan(h / (maxFocus[0] * 2.0)));
            Console.WriteLine("fovW: " + fovW);
            Console.WriteLine("fovH: " + fovH);

            Value.Vertical = fovW;
            Value.Horizontal = fovH;
        }
    }
}
