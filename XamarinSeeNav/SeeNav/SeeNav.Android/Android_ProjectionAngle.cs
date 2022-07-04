using System;
using Android.Hardware.Camera2;
using Xamarin.Forms;
using LogicLibrary.Native;
using Android.Content;
using LogicLibrary;
using LogicLibrary.Models;

[assembly: Dependency(typeof(SeeNav.Droid.Android_ProjectionAngle))]
namespace SeeNav.Droid
{
    public class Android_ProjectionAngle: IProjectionAngle
    {
        public float Vertical { get; }

        public float Horizontal { get; }

        public Android_ProjectionAngle()
        {
            // https://stackoverflow.com/questions/31172794/get-angle-of-view-of-android-camera-device
            var cameraManager = Android.App.Application.Context.GetSystemService(Context.CameraService) as CameraManager;
            var characteristics = cameraManager.GetCameraCharacteristics("0");

            var maxFocus = (float[]) characteristics.Get(CameraCharacteristics.LensInfoAvailableFocalLengths); // LENS_INFO_AVAILABLE_FOCAL_LENGTHS
            var size = (Android.Util.SizeF) characteristics.Get(CameraCharacteristics.SensorInfoPhysicalSize); // SENSOR_INFO_PHYSICAL_SIZE
            
            var w = size.Width;
            var h = size.Height;
            
            var toDeg = 180 / Math.PI;
            var fovW = toDeg * (2 * Math.Atan(w / (maxFocus[0] * 2.0)));
            var fovH = toDeg * (2 * Math.Atan(h / (maxFocus[0] * 2.0)));
            Logic.Log("fovW: " + fovW);
            Logic.Log("fovH: " + fovH);

            Vertical = (float)fovW;
            Horizontal = (float)fovH;
        }

        
    }
}
