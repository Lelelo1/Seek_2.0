using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;
using LogicLibrary.Native;
using Seek.Controls;
//using Xamarin.Essentials;
using Seek.Display;
using LogicLibrary;
using System.Threading.Tasks;
using LogicLibrary.Utils;
using LogicLibrary.Models;
using Xamarin.Essentials;

// It seems MediaElement in CommunityToolkit can't display image, source set to png

namespace Seek.Pages
{
    public partial class PermissionsPage : ContentPage
    {

        Image Image { get; set; } = new Image()
        {
            Source = ImageSource.FromFile("blue_citywalk_weather.PNG"),
            Aspect = Aspect.AspectFill
        };

        // try embedd the video on android
        
        // MediaSource.FromUri("https://storageaccountseekr9a43.blob.core.windows.net/seek-resources/AppPreview6.5.mp4")
        // ^ gets stuck in the video often
        MediaElement CreateVideoPlayer() => new MediaElement()
        {
            Source = MediaSource.FromFile("./Videos/AppPreview6.5.mp4"),
            IsLooping = true,
            Aspect = Aspect.AspectFill,
            AutoPlay = true,
            ShowsPlaybackControls = false, // are active on ios until interaction: https://docs.microsoft.com/en-us/xamarin/community-toolkit/views/mediaelement

        };

        public MediaElement CurrentVideoPlayer => screen.Children[0] as MediaElement;

        public void SetVideoPlayer(MediaElement videoElement)
        {
            screen.RemoveWhenExist(CurrentVideoPlayer);

            SetDimension(videoElement, AbsoluteLayoutFlags.All, Full);
            screen.Children.Insert(0, videoElement);
        }

        void SetDimension(BindableObject control, AbsoluteLayoutFlags flag, Xamarin.Forms.Rectangle rectangle)
        {
            AbsoluteLayout.SetLayoutFlags(control, flag);
            AbsoluteLayout.SetLayoutBounds(control, rectangle);
        }

        bool ImageIsDisplayed { get; set; } // rename is video footage, image footage etc or background

        void SetImageIsDisplayed(bool isDisplayed)
        {
            ImageIsDisplayed = isDisplayed;
            var opacity = ImageIsDisplayed ? 1 : 0;
            Image.Opacity = opacity;
        }

        Xamarin.Essentials.DisplayInfo DisplayInfo = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;

        Xamarin.Forms.Rectangle Full { get; } = new Xamarin.Forms.Rectangle(0, 0, 1, 1);

        Action StartARWithPermissions { get; }

        public PermissionsPage(Action startARWithPermissions)
        {
            InitializeComponent();

            StartARWithPermissions = startARWithPermissions;

            SetImageIsDisplayed(false); // display video default
 

            SetDimension(Image, AbsoluteLayoutFlags.All, Full);
            screen.Children.Insert(0, Image);

            SetVideoPlayer(CreateVideoPlayer());

            var top = N.Get<IStatusBar>().GetHeight();
            var padding = layout.Margin;
            layout.Padding = new Thickness(padding.Left, top, padding.Right, padding.Bottom);

            SetPermissionImage();
            SetPermissionLabel();
            SetDescriptionLabel();

            SetPermissionsButtonLayout();
            // animate opacity when video play ias made

            SetPermissionButtons();

            Lifecycle.OnAppResume += ReplayVideo;
        }

        // needs since the video stops playing (iOS) when resuming app
        void ReplayVideo()
        {
            // is still triggered when having left permission page
            // don't seem to be a problem but should be fixed for slight performence if not

            SetVideoPlayer(CreateVideoPlayer());
        }

        async void TryToPoceedToARPage ()
        {
            
            if(await PermissionUtils.HasPermissions())
            {
                StartARWithPermissions();
                await Navigation.PopAsync(true);
                CurrentVideoPlayer?.Stop();
            }

        }

        async void SetPermissionButtons()
        {

            var cameraPermissionButton = new PermissionButton("Camera", "./Icons/camera.png", async (Image statusImage) =>
            {
                var camera = await PermissionUtils.GetCamera();

                if (camera == PermissionStatus.Granted)
                {
                    return;
                }

                if (camera == PermissionStatus.Denied)
                {
                    Xamarin.Essentials.AppInfo.ShowSettingsUI();
                    return;
                }

                var status = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Camera>();

                HandlePermissionRequestResponse(status, statusImage);

            }, await PermissionUtils.GetCamera());

            permissionButtonsLayout.Children.Add(cameraPermissionButton);

            var locationPermissionButton = new PermissionButton("Location", "./Icons/location_map.png", async (Image statusImage) =>
            {
                var locationWhenInUse = await PermissionUtils.GetLocationWhenInUse();
                if (locationWhenInUse == PermissionStatus.Granted)
                {
                    return;
                }

                if (locationWhenInUse == PermissionStatus.Denied)
                {
                    Logic.Log("location" + " can't be asked more than once on iOS");
                    Xamarin.Essentials.AppInfo.ShowSettingsUI();
                    return;
                }

                var status = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationWhenInUse>();
                HandlePermissionRequestResponse(status, statusImage);

            }, await PermissionUtils.GetLocationWhenInUse());

            permissionButtonsLayout.Children.Add(locationPermissionButton);

        }

        void HandlePermissionRequestResponse(PermissionStatus status, Image statusImage)
        {
            if (status == PermissionStatus.Granted)
            {

                Logic.MainThread.Invoke(() =>
                {
                    statusImage.Source = PermissionButton.GetStatusImage(PermissionStatus.Granted).Source;
                });
                TryToPoceedToARPage();
            }
        }

        void SetPermissionImage()
        {
            // size don't become same on: (iphone 11) && (iPhone 12 Mini , iPhone 12 Pro Max)
            var size = DisplayInfo.Height * 0.05;
            permissionImage.HeightRequest = size;
            permissionImage.WidthRequest = size;

            var marginTop = DisplayInfo.Height * 0.020;
            permissionImage.Margin = new Thickness(0, marginTop, 0, 0);
        }

        void SetPermissionLabel()
        {
            var marginTop = DisplayInfo.Height * 0.007;
            permissionLabel.Margin = new Thickness(0, marginTop, 0, 0);
        }

        void SetDescriptionLabel()
        {
            var marginTop = DisplayInfo.Height * 0.032;
            var marginSide = DisplayInfo.Width * 0.022;
            descriptionLabel.Margin = new Thickness(marginSide, marginTop, marginSide, 0);
            /* text becomes too small on ipheon6 and is hard to get well, when using percentages
            var fontSize = Screen.Width * 0.02;
            descriptionLabel.FontSize = fontSize;
            */
        }

        void SetPermissionsButtonLayout()
        {
            var marginTop = DisplayInfo.Height * 0.042; // 0.10
            permissionButtonsLayout.Margin = new Thickness(0, marginTop, 0, 0);
            var spacing = DisplayInfo.Height * 0.03; // 0.06
            permissionButtonsLayout.Spacing = spacing;

        }

    }
}
