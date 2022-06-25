using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Native;

namespace Logic.Utils
{

    public class DependencyBox<T>
    {
        List<T> Dependencies { get; set; } = new List<T>();

        public void Add(T dependency)
        {
            Dependencies.Add(dependency);
        }

        public void AddRange(List<T> dependencies)
        {
            Dependencies.AddRange(dependencies);
            Logic.Log("Dependencies of " + typeof(T) + " was set with " + Dependencies.Count + " dependencies");
        }

        public D Get<D>() where D : T
        {
            if (Dependencies?.Count == 0)
            {
                Logic.Log("Can't access dependencies " + typeof(T) + " beacuse no dependencies have been added");
                return default(D);
            }

            D instance;
            try
            {
                instance = Dependencies.OfType<D>().First();
            }
            catch (Exception)
            {
                Logic.Log("There was no dependency of " + typeof(D).ToString() + " in DependencyBox");
                return default(D);
            }
            return instance;
        }

        public int Count => Dependencies.Count;
    }

}
