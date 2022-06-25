using System;
using System.Numerics;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Mobile
{
    public interface IFrameworkContext
    {
        void ReportCrash(Exception exc, string message);
        Task<Location> GetLocationAsync();
        Quaternion GetOrientation();

        string GetDistanceMetric(double meters);
        string GetDistanceImperial(double meters);
    }
}

