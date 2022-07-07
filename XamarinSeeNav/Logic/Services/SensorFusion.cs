using System;
using System.Numerics; // <-- is this mapped in rem oebjects for different platforms?

using LogicLibrary.Models;
using LogicLibrary;
using LogicLibrary.ViewModels;
using LogicLibrary.Game;

namespace LogicLibrary.Services
{
    public class SensorFusion : IBase
    {
        Vector3 MagnetometerReading { get; set; }

        public void SetMagnetometer(Vector3 calibratedReading)
        {
            MagnetometerReading = calibratedReading;
        }

        Vector3 GyroscopeReading { get; set; }

        public void SetGyroscope(Vector3 calibratedReading)
        {
            GyroscopeReading = calibratedReading;
        }

        Quaternion CurrentGyroOrientation { get; set; } = new Quaternion(0.001f, 0.001f, 0.001f, 1f);

        public void Orientation()
        {

            CurrentGyroOrientation *= Quaternion.Normalize(DeltaRotation(GyroscopeReading));
            CurrentGyroOrientation = Quaternion.Normalize(CurrentGyroOrientation);
            Console.WriteLine(CurrentGyroOrientation);
            Console.WriteLine(Extras.Heading(CurrentGyroOrientation));

            Logic.DependencyBox.Get<MainViewModel>().UpdateOrientation(CurrentGyroOrientation);
        }

        

        // https://stackoverflow.com/questions/24197182/efficient-quaternion-angular-velocity/24201879#24201879
        Quaternion DeltaRotation(Vector3 em)
        {
            var deltaTime = 0.02f;
            Vector3 ha = em * deltaTime * 0.5f; // vector of half angle
            var l = ha.Length(); // magnitude
            if (l > 0)
            {
                ha *= (float)(Math.Sin(l) / l);
                return new Quaternion(ha.X, ha.Y, ha.Z, (float)Math.Cos(l));
            }
            else
            {
                // never seem to run
                return new Quaternion(ha.X, ha.Y, ha.Z, 1.0f);
            }
        }
    }
}

