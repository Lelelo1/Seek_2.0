using System;
using System.Collections.Generic;
using XamarinLogic.Utils;

namespace XamarinLogic.Native
{
    public class N
    {
        static DependencyBox<INative> NativeDependencies { get; } = new DependencyBox<INative>();
        public static void Add(INative native)
        {
            NativeDependencies.Add(native);
        }

        public static void AddRange(List<INative> natives)
        {
            NativeDependencies.AddRange(natives);
        }
        public static D Get<D>() where D : INative
        {
            return NativeDependencies.Get<D>();
        }
    }
}
