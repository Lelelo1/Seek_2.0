using System;
using LogicLibrary;
using LogicLibrary.Services;
//using SeeNav.Controls;
using SeeNav.Display;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;
using LogicLibrary.Native;
using SeeNav.Pages;
using SeeNav.Test;
using Xamarin.CommunityToolkit.UI.Views;
using XamarinSeeNav.Display;

namespace SeeNav.Content
{
    public class Camera : IContent
    {
        static Camera instance;
        public static Camera Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Camera();
                }
                return instance;
            }
        }
        public AbsoluteLayout Layout { get; } = new AbsoluteLayout()
        {
            Padding = 0
        };

        // note that when camera view stop working, stop in camera feed - it shows a white screen (iOS). So it does not help
        // to put a backgroundcolor on 'Layout' to visualize it

        public CameraView View { get; }

        Color PlaceholderColor { get; set; } = Color.Black;

        StackLayout Overlay { get; } = new StackLayout()
        {
            BackgroundColor = new Color(255, 255, 255, 0.80),
        };

        protected Camera()
        {

            // Layout.Fill(AbsoluteLayoutFlags.All, 0, 0, 1, 1);
            // Has to set bounds once for the view and match it with th  size set in logic game projection
            // With LayoutFlags it has to gotten out from 3 renders. So better to sett definte size on once init

            var displaySize = DisplayUtils.GetDisplaySize();
            Layout.WidthRequest = displaySize.Width;
            Layout.HeightRequest = displaySize.Height;

            Layout.BackgroundColor = PlaceholderColor; // when debugging shows black screen instead of white to make a better distinction to 'Arrow' content

            View = new CameraView();
            View.Fill(AbsoluteLayoutFlags.All, 0, 0, 1, 1);
            View.BackgroundColor = PlaceholderColor;

            Layout.Children.Add(View);

            var runtime = N.Get<IUtilitiesService>().Runtime;
            if (runtime == Runtime.Debug || runtime == Runtime.Testing)
            {
                AssignTestTapper();
            }

            Overlay.Fill(AbsoluteLayoutFlags.All, 0, 0, 1, 1);
            Layout.Children.Add(Overlay);
        }

        void AssignTestTapper()
        {
            var tapRecognizer = new TapGestureRecognizer() { NumberOfTapsRequired = 2 };
            tapRecognizer.Tapped += TapRecognizer_Tapped;
            Layout.GestureRecognizers.Add(tapRecognizer);
        }

        void TapRecognizer_Tapped(object sender, EventArgs e)
        {
            // manual test orientationbearing, projection and detection. should move, and seperate if about to
            // to start overlaping, while have maintaining correct bearing. When animating back to location, they
            // should have be in the same direction as before (although possible with changed height)

            //ServiceExtensions.FadeLocationThereAndBack(TimeSpan.FromSeconds(16));

            // open testusereventspage
            ARPage.Instance.Navigation.PushAsync(new TestUserEventsPage());
        }

        public void ShowOverlay()
        {
            Layout.AddWhenNotExist(Overlay);
        }

        public void HideOverlay()
        {
            Layout.RemoveWhenExist(Overlay);
        }
    }


}
