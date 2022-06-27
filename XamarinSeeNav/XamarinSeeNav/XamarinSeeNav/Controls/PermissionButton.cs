using System;
using LogicLibrary.Models;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Seek.Controls
{
	public class PermissionButton : Frame
	{
		StackLayout Container { get; set; } // to support many children

		Image PermissionImage(string permissionIcon) => new Image() { Source = permissionIcon, WidthRequest = 35, HeightRequest = 35, Opacity = 0.85 };

		Label Name(string permissionName) => new Label()
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontSize = 16,
            Text = permissionName,
            HeightRequest = 30,
            Margin = new Thickness(10, 0, 0, 0)
        };

        static string StatusImageKey { get; set; } = "icon_key.png";
        static string StatusImageCheck { get; set; } = "icon_check.png";

        public static Image GetStatusImage(PermissionStatus status)
        {
            var statusImage = new Image()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(0, 0, 40, 0),
            };

            var isAccepted = status == PermissionStatus.Granted;

            var icon = isAccepted ? StatusImageCheck : StatusImageKey;
            statusImage.Source = icon;

            var size = isAccepted ? 35 : 25;
            statusImage.WidthRequest = size;
            statusImage.HeightRequest = size;

            var opacity = isAccepted ? 0.95 : 0.85;
            statusImage.Opacity = opacity;

            return statusImage;

        }

        public Image StatusImage { get; }

        public PermissionButton(string permissionName, string permissionIcon, Action<Image> onTap, PermissionStatus initialPermissionStatus)
		{

            var permissionImage = PermissionImage(permissionIcon);

            var name = Name(permissionName);

            var statusImage = GetStatusImage(initialPermissionStatus);

            Container = new StackLayout() { Orientation = StackOrientation.Horizontal };
            Container.Children.Add(permissionImage);
            Container.Children.Add(name);
            StatusImage = statusImage;
            Container.Children.Add(statusImage);

            TouchEffect.SetNativeAnimation(this, true);
            CornerRadius = 15;
            Margin = new Thickness(20, 0, 20, 0);
            Padding = new Thickness(20, 0, 20, 0);
            HeightRequest = 43;
            Opacity = 1;
            HasShadow = false;
            Content = Container;
            BackgroundColor = Xamarin.Forms.Color.FromHex("#7Adeda5f");

            // can pass this event handler in as contructor parameter in the future..
            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += (object sender, EventArgs e) => onTap?.Invoke(StatusImage);
            GestureRecognizers.Add(tapRecognizer);
        }

	}
}

