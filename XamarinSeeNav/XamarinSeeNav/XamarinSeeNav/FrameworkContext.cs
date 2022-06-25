using System;
using System.Numerics;
using System.Threading.Tasks;
using LogicLibrary;
using LogicLibrary.Models;

namespace XamarinSeeNav
{
    public class FrameworkContext : IFrameworkContext
    {
        public FrameworkContext()
        {
        }

        public Task<PermissionStatus> GetCameraPermissionAsync()
        {
            throw new NotImplementedException();
        }

        public string GetDistanceImperial(double meters)
        {
            throw new NotImplementedException();
        }

        public string GetDistanceMetric(double meters)
        {
            throw new NotImplementedException();
        }

        public Task<Location> GetLocationAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PermissionStatus> GetLocationWhenInUsePermissionAsync()
        {
            throw new NotImplementedException();
        }

        public Quaternion GetOrientation()
        {
            throw new NotImplementedException();
        }

        public double MetersBetween(Location a, Location b)
        {
            throw new NotImplementedException();
        }

        public void ReportCrash(Exception exc, string message)
        {
            throw new NotImplementedException();
        }
    }
}

