using System;
using System.Numerics;
using System.Threading.Tasks;
using LogicLibrary.Models;

namespace LogicLibrary
{
    public interface IFrameworkContext
    {
        void ReportCrash(Exception exc, string message);
        Task<Location> GetLocationAsync();
        Quaternion GetOrientation();

        string GetDistanceMetric(double meters);
        string GetDistanceImperial(double meters);

        Task<PermissionStatus> GetCameraPermissionAsync();
        Task<PermissionStatus> GetLocationWhenInUsePermissionAsync();

        double MetersBetween(Location a, Location b);
    }
}

