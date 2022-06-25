using System;
using System.Collections.Generic;
using System.Numerics;
using CoreMotion;
using Foundation;
using LogicLibrary.Utils;
using Seek.iOS;
using Xamarin.Forms;
/*
[assembly: Dependency(typeof(iOS_Ahrs))]
namespace Seek.iOS
{
    public class iOS_Ahrs// : IAhrsService
    {
        public Quaternion Orientation { get; private set; } = new Quaternion();
        NSOperationQueue Queue { get; set; } = new NSOperationQueue();

        static List<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
        public void Register(EventRegistration registration)
        {
            Registrations.Add(registration);
            _ = registration.PeriodicTask.Run(() =>
            {
                registration.OnReadingChanged?.Invoke();
            });
        }

        public void Start()
        {
            if(!iOS_Sensors.MotionManager.DeviceMotionActive)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    iOS_Sensors.MotionManager.StartDeviceMotionUpdates(CMAttitudeReferenceFrame.XTrueNorthZVertical, Queue, Handler);
                });
            }

        }

        void Handler(CMDeviceMotion data, NSError error)
        {
            if (data == null)
            {
                return;
            }
                

            var field = data.Attitude.Quaternion;

            // the quaternion returned by the MotionManager refers to a frame where the X axis points north ("iOS frame")
            var q = new Quaternion((float)field.x, (float)field.y, (float)field.z, (float)field.w);

            // in Xamarin, the reference frame is defined such that Y points north and Z is vertical,
            // so we first rotate by 90 degrees around the Z axis, in order to get from the Xamarin frame to the iOS frame
            var qz90 = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)(Math.PI / 2.0));

            // on top of this rotation, we apply the actual quaternion obtained from the MotionManager,
            // so that the final quaternion will take us from the earth frame in Xamarin convention to the phone frame
            Orientation = Quaternion.Multiply(qz90, q);
            
        }

        public void Stop()
        {
            if (!iOS_Sensors.MotionManager.DeviceMotionActive)
            {
                // Sensors.MotionManager.StopDeviceMotionUpdates(); // also stops sensors like calib magnetometer I think..
            }
            
        }
    }
}
*/
