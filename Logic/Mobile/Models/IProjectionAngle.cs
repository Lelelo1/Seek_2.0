using System;
using LogicLibrary.Native;

namespace LogicLibrary.Models
{
    public interface IProjectionAngle : INative
    {
        public float Vertical { get; }
        public float Horizontal { get; }
    }
}

