using System;
namespace Seek.Display
{
    public class Lifecycle
    {
        public delegate void AppStart();
        public static event AppStart OnAppStart;

        public static void InvokeAppStart()
        {
            OnAppStart?.Invoke();
        }

        public delegate void AppResume();
        public static event AppResume OnAppResume;

        public static void InvokeAppResume()
        {
            OnAppResume?.Invoke();
        }
    }
}
