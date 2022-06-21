using System;
using Xamarin.Forms;
using FormsGestures;
// using System.Collections.Generic;
using XamarinLogic;

namespace Seek.Visualization.Support
{
    /*
    public interface IGestures
    {
        void Add(View view, Gesture gesture); //Action OnRelease
    }

    public class GestureHandler
    {
        public Gesture Gesture { get; set; }
        public iOS iOS { get; set; }
        public GestureHandler(Gesture gesture, Listener listener, EventHandler<DownUpEventArgs> eventHandler)
        {
            Gesture = gesture;
            iOS = new iOS(listener, eventHandler);
        }
        public void Remove()
        {
            if(Device.RuntimePlatform == Device.iOS)
            {
                if(Gesture.GestureType == GestureType.Release)
                {
                    iOS.Listener.Up -= iOS.EventHandler;
                }
            }
            Logic.Log("In Gestures.cs - missing android implementation");
        }
    }

    public class iOS
    {
        public Listener Listener { get; set; }
        public EventHandler<DownUpEventArgs> EventHandler { get; set; }
        public iOS(Listener listener, EventHandler<DownUpEventArgs> eventHandler)
        {
            Listener = listener;
            EventHandler = eventHandler;
        }
    }
    public class Gestures : IGestures
    {
        static Gestures instance;
        public static Gestures Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Gestures();
                }
                return instance;
            }
        }

        public void Add(View view, Gesture gesture)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                // can't interact with content using TouchEffect
                var listener = FormsGestures.Listener.For(view);
                System.EventHandler<FormsGestures.DownUpEventArgs> handler = (object sender, DownUpEventArgs e) => gesture.Action.Invoke();
                listener.Up += handler;
                // Logic.Utils.Log.Message("gesture was added on " + view);
            }
        }
    }
    public class Gesture
    {
        public GestureType GestureType { get; set; }
        public Action Action { get; set; }
        public Gesture(GestureType gestureType, Action action)
        {
            GestureType = gestureType;
            Action = action;
        }
    }
    public enum GestureType
    {
        Release
    }
    */
}
