using Seek.Pages;
using System;
using System.Collections.Generic;
using Seek.Visualization;
using System.Linq;
using Seek.Display.Internal;
using Logic.ViewModels;
using Logic;
using Logic.Utils;
using System.Numerics;
using Logic.Game;

namespace Seek.Display
{
    public class SensorFading
    {

        public static Observable<Orientation> CurrentOrientation { get; } = new Observable<Orientation>(Orientation.None);

        // cameraview should be underneath, should fade opposite and should be over the MainContent 
        // arrow should be over maincontent, and have default fading

        static MainViewModel MainViewModel { get; set; }
        
        static List<ExtendedView> ExtendedViews { get; set; } = new List<ExtendedView>();

        public static void Init(params ExtendedView[] extendedViews)
        {
            ExtendedViews.AddRange(extendedViews);
            MainViewModel = Logic.DependencyBox.Get<MainViewModel>();
            MainViewModel.Orientation.Subscribe(FadeOnPortaitCameraPose);
        }

        static double sensitivity = 1;
        static double lastR;
        static double minAngle = 48;
        static double maxAngle = 55;

        public static double CurrentValue { get; private set; }

        /* Extracts pitch interval value*/
        static void FadeOnPortaitCameraPose(Quaternion orientation, int iteration)
        {
            if(Visualize.IsEmpty)
            {
                // UI should freeze to their initial opacity, and fading to be skipped (when there is no places)
                return;
            }

            /* x and y variable does not correspond to roll pith really :/ use a and instead */
            // var x = Math.Abs(AhrsService.MobileCordinateSystem.Roll);
            // var y = Math.Abs(AhrsService.MobileCordinateSystem.Pitch);
            var q = orientation;
            var a = Math.Abs(q.TransitionRoll());
            var b = Math.Abs(q.TransitionPitch());
            /* make transition euler in projection helpers for now! */
            var r = a > b ? a : b;


            if ((Math.Abs(r - lastR)) > sensitivity)
            {
                lastR = r;
                double value = 1;
                if (r >= minAngle && r <= maxAngle) // also sheck if device is not upside dow.
                {
                    var percent = (r - minAngle) / (maxAngle - minAngle); // interval to percentage: https://math.stackexchange.com/questions/2121174/the-percentage-of-a-number-within-an-interval-range-of-numbers
                    value = percent;
                    // https://stackoverflow.com/questions/16242259/reverse-number-in-a-range
                    SetWhenChanged(Orientation.InBetween);
                }
                else if (r < minAngle)
                {
                    value = 0;
                    SetWhenChanged(Orientation.Down);
                }
                else if (r > maxAngle && r < 180) // add y conition here
                {
                    value = 1;
                    SetWhenChanged(Orientation.Up);
                }
                // OrientationTracker.OrientationChanged(orientation);
                CurrentValue = 1 - value;
                var fadingViews = ExtendedViews.Where(ev => ev.Fading).ToList();
                Logic.MainThread.Invoke(() =>
                {
                    // so that arrow for instance are input transperent when faded out
                    fadingViews.ForEach(fv => fv.View.InputTransparent = CurrentOrientation.Value == Orientation.Up);

                    OnValueChanged(fadingViews);
                });

            }
        }

        static void OnValueChanged(List<ExtendedView> fadingViews) // OnValueChange
        {
            fadingViews.ForEach(fv => fv.View.Opacity = CurrentValue);
        }

        // the reactive subjects trigger subscribe when setting with same value!
        static void SetWhenChanged(Orientation orientation)
        {
            if(CurrentOrientation.Value != orientation)
            {
                CurrentOrientation.Set(orientation);
            }
        }
    }

    public enum Orientation // remove somehow?
    {
        Down,
        InBetween,
        Up,
        None // needed as comparison against null enum fails, making down not count unless upp first
    }
    
    /*
    public class OrientationTracker : ReactiveObject
    {
        public int HasBeenDownTimes { get; set; }
        public int HasBeenUpTimes { get; set; } // <-- has to be placed here so that arrow get i'ts count that are reset each times it it created
        public Func<bool> StartRequire { get; set; }
        public Orientation CurrentOrientation = Orientation.None;

        // for turtorial only
        public Action OnOnceUp { get; set; }
        public Action OnOnceDown { get; set; }
        public void OrientationChanged(Orientation newOrientation)
        {

            if (CurrentOrientation != newOrientation)
            {
                CurrentOrientation = newOrientation;

                if (CurrentOrientation == Orientation.Down)
                {
                    HasBeenDownTimes++;
                    if (OnOnceDown != null)
                    {
                        OnOnceDown.Invoke();
                        OnOnceDown = null;
                    }
                    // Log.Message("HasBeenDownTimes: " + HasBeenDownTimes);
                }
                else if (CurrentOrientation == Orientation.Up)
                {
                    HasBeenUpTimes++;
                    if(OnOnceUp != null)
                    {
                        OnOnceUp.Invoke();
                        OnOnceUp = null;
                    }
                    // Log.Message("HasBeenUpTimes: " + HasBeenUpTimes);
                }
                // Log.Message("newOrientation: " + CurrentOrientation);
            }
        }

        public bool Hidden { get; set; } = false;

        public void Reset()
        {
            HasBeenDownTimes = 0;
            HasBeenUpTimes = 0;
        }
    }
    */

}
