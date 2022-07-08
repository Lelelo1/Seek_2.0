using System;
using CoreMotion;
using Foundation;
using LogicLibrary.Services;
using LogicLibrary;
using System.Numerics;

namespace SeeNav.iOS
{
    public class iOS_Sensors
    {
        CMMotionManager MotionManager { get; }

        public iOS_Sensors()
        {

            MotionManager.StartDeviceMotionUpdates(CMAttitudeReferenceFrame.XTrueNorthZVertical, NSOperationQueue.CurrentQueue, CMDeviceMotionHandler);
        }

        

        CMDeviceMotionHandler CMDeviceMotionHandler { get; } = new CMDeviceMotionHandler(SetSensorReading);

        static SensorFusion SensorFusion => Logic.DependencyBox.Get<SensorFusion>();

        static void SetSensorReading(CMDeviceMotion deviceMotion, NSError error)
        {
            var g = deviceMotion.RotationRate;
            var m = deviceMotion.MagneticField.Field;
            SensorFusion.SetGyroscope(new Vector3((float)g.x, (float)g.y, (float)g.z));
            SensorFusion.SetMagnetometer(new Vector3((float)m.X, (float)m.Y, (float)m.Z));
        }
    }
}

