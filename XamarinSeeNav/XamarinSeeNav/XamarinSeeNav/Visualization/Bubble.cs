using System;
using System.Numerics;
using LogicLibrary.Services.PermissionRequired;
using Xamarin.Forms;
using Seek.Visualization.Support;
using Seek.Visualization.Contents;
using System.Threading.Tasks;
using Rectangle = LogicLibrary.Game.Models.Rectangle;
using Seek.Content;
using LogicLibrary.ViewModels;
using LogicLibrary.Services;
using LogicLibrary.Models;
using LogicLibrary.Models.Analytics;
using LogicLibrary;
using LogicLibrary.Game.Models;
using LogicLibrary.Game;
using Xamarin.Essentials;

namespace Seek.Visualization
{
    // make classes from all interfaces probably, so that properties is always accssed through observable, and hidden from direct manipulation
    public class Bubble : Frame, IBubble, ISelectable
    {
        MainViewModel MainViewModel { get; }
        AnalyticsService AnalyticsService { get; }
        LocationService LocationService { get; }
        SearchViewModel SearchViewModel { get; }

        public Place Place { get; set; }
        public Spatial Spatial { get; set; }
        public Animator Animator { get; }

        public bool IsSelected { get; private set; }
        public void SetIsSelected(bool selected) // make sure all calls to SetIsSelected is made on the ui thread
        {
            IsSelected = selected;
            Place.IsDestination = IsSelected;
            ActiveReleaseGesture = !IsSelected; // press on
            Selection();
        }
        public event Select OnSelect;
        public delegate void Select(Bubble visual, TaskCompletionSource<bool> animationCompletion, uint? animationMilliseconds);

        public event Deselect OnDeselect;
        public delegate void Deselect(Bubble visual, Quaternion toQ, TaskCompletionSource<bool> animationCompletion, uint? animationMilliseconds);

        public bool ActiveReleaseGesture { get; set; } = true;

        public bool IgnoreGyro { get; set; }

        public _Content _Content { get; set; }

        /*
        public new bool IsVisible
        {
            get
            {
                return base.IsVisible;
            }
            set
            {
                base.InputTransparent = !value;
                base.IsVisible = value;
            }
        }
        */
        static bool HasShownTiltDownForArrowTurtorial { get; set; } = false;

        static uint AnimationTime { get; set; } = 2000u;

        public Bubble(Place place)
        {
            MainViewModel = Logic.DependencyBox.Get<MainViewModel>();
            AnalyticsService = Logic.DependencyBox.Get<AnalyticsService>();
            LocationService = Logic.DependencyBox.Get<LocationService>();
            SearchViewModel = Logic.DependencyBox.Get<SearchViewModel>();

            Place = place;

            var userLocation = Logic.DependencyBox.Get<LocationService>().Location.Value;

            Spatial = new Spatial(userLocation, Place.Location, Constants.Size, true);

            Animator = new Animator(this);

            _Content = new _Content(this);
            Content = _Content.Get;

            var tapReco = new TapGestureRecognizer();
            tapReco.Tapped += (object sender, EventArgs args) =>
            {
                if (Arrow.Instance.DestinationAborting != null) // tap could cancel deselect animation otherwise
                {
                    return;
                }

                SetIsSelected(!IsSelected);

                var destination = MainViewModel.Destination.Value;
            };

            this.GestureRecognizers.Add(tapReco);

            Padding = 0; // all style handled inside the specific contents

            Spatial.Rectangle = new Rectangle(-1, -1, Constants.Size.Width, Constants.Size.Height); // could give a size here

            CornerRadius = 45;

            BackgroundColor = new Color(255, 255, 255, 0.88); // ios not as transparent as android from reason
            Opacity = 0.88;
            HasShadow = false;

               
        }

        float DefaultIncrease { get; } = 0.025f;
        float TurtorialIncrease { get; } = 0.005f;

        public Task AnimateToBearingOrientation()
        {
            //var increase = Preferences.ContainsKey(AnalyticsService.BubbleShownFlag) ? DefaultIncrease : TurtorialIncrease;
            var increase = DefaultIncrease;

            Spatial.Orientation.Set(Projector.CameraOrientation);
            return Animator.Start(Spatial.Orientation.Value, Quaternion.Slerp(Projector.CameraOrientation, Spatial.Orientation.New(Source.Bearing), 1f), increase);
            
        }

        void Selection()
        {

            var animations = Animations.Get;
            if (animations.IsRunning(this))
            {
                animations.AbortAll(this);
            }

            if (IsSelected) // should also move visual toward middle of screen by animated orinetation quaternion
            {
                Spatial.IgnoreCollision = true;

                // Content = _Content.Get.Content; // adds the enlarged content

                // https://github.com/ScienceSoft-Inc/ViewGestures

                // VisualEffects.OnTouch.Invoke(this); // animate to enlarged visual

                MainViewModel.Destination.Set(Place);

                var bubbleTapped = new BubbleTapped(Place.Name, MainViewModel.GetDistance(Place), MainViewModel.DistanceToString(Place));
                AnalyticsService.Track(bubbleTapped);

                //Arrow.Navigate(this);
            }
            else
            {
                MainViewModel.Destination.Set(null);
            }
        }

        bool IsShown {
            get; set; }
        public void SetBubbleShown(Rectangle cameraView)
        {
            var isShown = Spatial.GetIsSeen(cameraView);
            if (isShown != IsShown && isShown)
            {
                var bubbleShown = new BubbleShown(Place.Name, MainViewModel.GetDistance(Place), MainViewModel.DistanceToString(Place));
                AnalyticsService.Track(bubbleShown);
            }
            IsShown = isShown;
        }
    }

}

