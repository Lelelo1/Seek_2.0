using System;
namespace Logic.Native
{
    public interface ILocationService : INative
    {
        void Start();
        double MillisecondsUpdateInterval { get; set; }
        void Stop();
        bool IsActive { get; }
        
    }
}
