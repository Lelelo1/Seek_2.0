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

        string GetDistanceMetric(double meters);
        string GetDistanceImperial(double meters);

        double MetersBetween(Location a, Location b);
    }
}

