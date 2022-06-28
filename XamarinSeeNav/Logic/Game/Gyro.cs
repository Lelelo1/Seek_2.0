using System;
using System.Numerics;
using LogicLibrary.Utils;
using LogicLibrary.ViewModels;

namespace LogicLibrary.Game
{
    /* Text/Labels always readable regardless of the device orientation */
    public class Gyro
    {
        static Gyro instance;
        public static Gyro Get() // only pass with milliseconds
        {
            if(instance == null)
            {
                instance = new Gyro();
                var defaultRegistration = new EventRegistration(10);
                _ = defaultRegistration.PeriodicTask.Run(() =>
                {
                    /* Creates a valid roll/gyrovalue for perceptuals that does not spin in some circumstances. */
                    // var o = AhrsService.MobileCordinateSystem.MadgwickAHRS.Quaternion;
                    //var o = MainViewModel.Get().Ahrs.Orientation;
                    /* axises prevously from https://github.com/psiphi75/ahrs/issues/9
                       no needed coordinate system transfrmation as ahrs q now alteady are in mobile coordinate system
                    var mobileOrientation = new Quaternion(-o.Y, -o.X, -o.Z, o.W); // // where axises correspond to the mobile doucmentation docs */

                    var main = Logic.DependencyBox.Get<MainViewModel>();
                    var q = main.Orientation.Value * Quaternion.CreateFromAxisAngle(Vector3.UnitY, Calc.ToRad(90));
                    //var roll = (float)q.EulerRoll();
                    //var pitch = (float)q.EulerPitch();
                    var yaw = (float)q.EulerYaw() - 90; // (for some reason)
                    instance.Angle = -yaw;//yaw;
                });
                registration = defaultRegistration;
            }
            return instance;
        }
        static EventRegistration registration;
        public void Stop()
        {
            registration.PeriodicTask.CancellationTokenSource.Cancel();
        }
        public double Angle { get; set; }
    }
}
