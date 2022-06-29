using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
//using SeeNav.Controls;
using System.Collections.Concurrent;
using System.Numerics;
using LogicLibrary.Utils;
using SeeNav.Visualization.Support;
using SeeNav.Content;
using Microsoft.AppCenter.Crashes;
using LogicLibrary.ViewModels;
using LogicLibrary;
using LogicLibrary.Models;
using LogicLibrary.Native;
using Constants = SeeNav.Visualization.Support.Constants;
using LogicLibrary.Game;
using System.Threading.Tasks;
using LogicLibrary.Game.Models;
using Xamarin.CommunityToolkit.UI.Views;
using SeeNav.Display;

namespace SeeNav.Visualization
{

    public class Visualize
    {

        // volantile keywoard lost when making it a property instead of field
        public static ConcurrentDictionary<int, Bubble> VisualsMap { get; } = new ConcurrentDictionary<int, Bubble>();

        static AbsoluteLayout CameraLayout => Camera.Instance.Layout;


        static Projector Projector { get; set; }
        static MainViewModel MainViewModel { get; set; }
        static SearchViewModel SearchViewModel { get; set; }

        public static void Init()
        {
            MainViewModel = Logic.DependencyBox.Get<MainViewModel>();
            SearchViewModel = Logic.DependencyBox.Get<SearchViewModel>();

            MainViewModel.Orientation.Subscribe(Visualization_OnReadingChanged);

            SearchViewModel.Places.Subscribe(AddVisualsSafely);
            
            Projector = Logic.DependencyBox.Get<Projector>();
        }


        public static bool IsEmpty { get; private set; }

        public static void AddVisualsSafely(List<Place> places, int iterator)
        {
            
            try
            {
                AddBubbles(places);
            }
            catch(Exception exc)
            {
                Crashes.TrackError(exc, Error.Properties("Trying to create Visuals and adding them to VisualsMap, by invoking on mainthread"));
                Logic.Log("failed adding visuals from places, in visualize");
            }
        }

        static void AddBubbles(List<Place> places)
        {

            VisualsMap.Clear();
            Logic.MainThread.Invoke(() =>
            {
                CameraLayout.Clear();
            });

            if (Ext.IsNullOrEmpty(places))
            {
                return;
            }

            // So that active navigation with arrow is cleared when making new searches. Has to be placed here for some reason
            if (MainViewModel.Destination.Value != null)
            {
                MainViewModel.Destination.Set(null);
            }

            int i = 0;
            foreach (var p in places) //is new targets
            {
                var bubble = new Bubble(p);
                VisualsMap.TryAdd(i, bubble);
                i++;
            }

            if(places.Count == 1)
            {
                MainViewModel.Destination.Set(places.First());
            }

            /* Collision detection with the new visuals */
            Detection.Get().TrackPerceptibles = VisualsMap.Values.Select(b => b.Spatial).ToList();

            Animate(VisualsMap.Values);
        }

        static async void Animate(IEnumerable<Bubble> bubbles)
        {
            await Task.WhenAll(bubbles.Select(b => b.AnimateToBearingOrientation()));
            bubbles.ToList().ForEach(SetOnReachedBearing);
            
        }

        static void SetOnReachedBearing(Bubble bubble)
        {
            bubble.Spatial.IgnoreCollision = false;
            bubble.Spatial.Orientation.Origin = Source.Bearing;
            bubble.Animator.IsAnimating = false;
            var bounds = CameraLayout.Bounds;
            bubble.SetBubbleShown(new LogicLibrary.Models.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height));
        }


        static void Visualization_OnReadingChanged(Quaternion orientation, int iterator)
        {
            var q = orientation;
            // w, x, y, z --> x, y, z, w
            //MainPage.Instance.DebugLabel.Text = q.Print();
            GyroEffect();
            
            VisualsMap.Values.ToList().ForEach(SafeProject);
            
        }

        static void SafeProject(Bubble bubble)
        {
            var projection = Projector.Project(bubble.Spatial);

            bubble.Spatial.Rectangle = projection.Rectangle;

            try
            {
                Logic.MainThread.Invoke(() =>
                {
                    Project(bubble);
                });
            }
            catch(Exception exc)
            {
                Logic.Log("failed to project " + bubble.Place.Name + ": " + exc.Message);
                var v = exc;
            }
        }

        static void Project(Bubble bubble) // VisualItem
        {
            if (bubble.Spatial.IsBehindPlane)
            {
                CameraLayout.RemoveWhenExist(bubble);
                return;
            }

            // projected items are mirrored, so appear in 0 deg and 180 deg which is normal
            //don't exactly understand why these isinde works both in true and not true

            if(!bubble.Animator.IsAnimating)
            {
                var bounds = CameraLayout.Bounds;
                bubble.SetBubbleShown(new LogicLibrary.Models.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height));
            }

            CameraLayout.AddWhenNotExist(bubble);

            var rectangle = bubble.Spatial.Rectangle;
            AbsoluteLayout.SetLayoutBounds(bubble, new Xamarin.Forms.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height));

        }

        static void GyroEffect()
        {
            // It takes the yaw out of a turned mobileorientation
            
            var gyro = Gyro.Get().Angle;
            foreach (var visual in VisualsMap)
            {
                var v = visual.Value;
                if(!v.IgnoreGyro)
                {
                    //var g = v.IsSelected ? 0 : gyro;//  yaw;
                    var isDestinationAborting = Arrow.Instance.DestinationAborting == v; // <----
                    visual.Value.Rotation = isDestinationAborting ? 0 : gyro; ; //gyro;//g; // <----
                }
            }
            
        }

        public static Bubble GetBubbleFor(Place place)
        {
            return VisualsMap.Values.ToList().Find(b => b.Place.Name == place?.Name);
        }
    }

    public static class Extensions
    {
        /*
        public static Point ToPoint(this Vector2 position)
        {
            return new Point(position.X, position.Y);
        }
        */
        // other visuals should not appear over a selected (enlarged) visual:
        public static void Insert(this AbsoluteLayout cameraLayout, Bubble visual)
        {
            var selectedVisual = cameraLayout.Children.FirstOrDefault((view) =>
            {
                if (view is Bubble check)
                {
                    return check.IsSelected;
                }
                return false;
            });

            if (selectedVisual == null)
            {
                cameraLayout.Children.Add(visual);
            }
            else
            {
                var index = cameraLayout.Children.IndexOf(selectedVisual);
                cameraLayout.Children.Insert(index, visual);
            }

        }

        public static void MoveTop(this Bubble visual, AbsoluteLayout layout)
        {
            if (!(layout.Children.Last() == visual)) // just for minor perf increase
            {
                layout.Children.Remove(visual);
                layout.Children.Add(visual);
            }
        }

        // removes all visuals from cameralayout
        public static void Clear(this AbsoluteLayout cameraLayout)
        {
            foreach (var view in cameraLayout.Children?.ToList())
            {
                if (!typeof(CameraView).IsInstanceOfType(view))
                {
                    try
                    {
                        cameraLayout.Children.Remove(view);
                    }
                    catch (Exception)
                    {
                        //MainPage.Instance.DebugLabel.Text = "could not remove: " + e.Message;
                    }
                }
            }
        }

        /* Xamarin Froms control instances can't be present in more than one View at the same time and needs a new generated id to appear */

    }

    public class VisualizationException : Exception
    {
        public VisualizationException(string message) : base(message) { }
    }
}

