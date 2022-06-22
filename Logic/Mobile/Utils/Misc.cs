using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XamarinLogic;
using Xamarin.Essentials;

namespace XamarinLogic.Utils
{

    public class Calc
    {
        static float floatPi = (float)Math.PI;
        public static float ToRad(float deg)
        {
            return deg * (floatPi / 180);
        }
        public static float ToDeg(float rad)
        {
            return rad * (180 / floatPi);
        }
        // same but for double
        public static double ToRad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        public static double ToDeg(double rad)
        {
            return rad * (180 / Math.PI);
        }

        public static float Normalize(float angle)
        {
            angle %= 360;
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }
        public static double Normalize(double angle)
        {
            angle %= 360;
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }
    }

    public class Ext
    {
        public static bool IsNullOrEmpty<T>(List<T> list)
        {
            if (list == null)
                return true;
            else if (list.Count == 0)
                return true;
            else
            {
                return false;
            }
        }

        public static bool HasValue<T>(T t)
        {
            return t != null;
        }

        public static Dictionary<K, V> CreateDictionary<K, V>(IEnumerable<K> keys, IEnumerable<V> values)
        {
            var dictionary = new Dictionary<K, V>();

            var keysList = keys.ToList();
            if(keysList.Count == 0)
            {
                return dictionary;
            }

            var valuesList = values.ToList();

            for (int i = 0; i < keys.Count(); i ++)
            {
                var k = keysList[i];
                var v = valuesList[i];

                if(!dictionary.ContainsKey(k))
                {
                    dictionary.Add(k, v);
                }
            }

            return dictionary;
        }
    }


    // to structure all 'Error properties' given in app analtyics
    public class Error
    {
        static string Situation { get; } // tryin to give context to what was happening
        public static Dictionary<string, string> Properties(string situation) // later on add as optional, and add property (name) PlacesCount eg
        {
            // check if App center can take object. I assume a value can't be a list
            var properties = new Dictionary<string, string>()
            {
                { nameof(Situation), situation }
            };
            return properties;
        }
    }

    public class MiscException : Exception
    {
        public MiscException(string message) : base(message) { }
    }

    public class Observable<T>
    {
        public delegate void ValueChanged(T value, int number);
        event ValueChanged Changed;

        // this is needed to prevent an intial ghost even trigger in 'PermissionViewModel'. A trigger of
        // the subcription without triggering the event - which don't occur elsewhere or in seperate console project
        // https://stackoverflow.com/questions/70729432/eventhandler-called-once-without-event-fired
        // it can be ignored elsewhere 
        public int Iteration { get; private set; } = 0;

        public T Value { get; private set; }

        //bool TriggerUpdateOnFirstValue

        public Observable(T t)
        {
            Value = t;
        }

        public Observable(T t, ValueChanged subscription)
        {
            Value = t;
            Changed += subscription;
        }

        public void Set(T value)
        {
            if (Equals(value, Value))
            {
                return;
            }
            Value = value;
            Iteration = 2;
            Changed?.Invoke(Value, Iteration);

        }


        public void Subscribe(ValueChanged action, bool initialUpdate = false)
        {
            Changed += action;
            

            if(initialUpdate)
            {
                Changed.Invoke(Value, Iteration);
                Iteration++;
            }
        }

    }
    public static class UtilsExtensions
    {
        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                         .GetProperty(item.Key)
                         .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static Dictionary<string, object> ToDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }

        public static string GetClassName(this object obj)
        {
            // note that it seem to take the most base class implementing the interface (IAnalyticsEvent)
            return obj.GetType().Name;

            // https://stackoverflow.com/questions/1159906/finding-the-concrete-type-behind-an-interface-instance
        }
        public static List<Type> GetSubclasses(this Type type)
        {
            return type.Assembly.GetTypes().Where(t => t.IsSubclassOf(type)).ToList();
        }

        

    }

    public class PermissionUtils
    {
        public static Task<Xamarin.Essentials.PermissionStatus> GetCamera() => Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.Camera>();
        public static Task<Xamarin.Essentials.PermissionStatus> GetLocationWhenInUse() => Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.LocationWhenInUse>();

        public static async Task<bool> HasPermissions()
        {
            var camera = await GetCamera();
            var locationWhenInUse = await GetLocationWhenInUse();

            return camera == PermissionStatus.Granted && locationWhenInUse == PermissionStatus.Granted;
        }
    }

}

