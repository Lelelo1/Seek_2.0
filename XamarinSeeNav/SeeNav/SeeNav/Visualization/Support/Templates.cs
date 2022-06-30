using System;
using LogicLibrary;
using LogicLibrary.Services.PermissionRequired;
using LogicLibrary.ViewModels;
using Xamarin.Forms;

namespace SeeNav.Visualization.Support
{
    public class Templates
    {
        public Bubble Bubble { get; set; }

        public Templates(Bubble bubble)
        {
            Bubble = bubble;
        }


        /*
        DataTemplate _smallTemplate;
        public DataTemplate SmallTemplate
        {
            get
            {
                if(_smallTemplate == null)
                {
                    __smallTemplate = new DataTemplate(() =>
                    {
                        return new StackLayout() { }
                    });
                }
            }
        }
        */

        Label Label { get; set; } = new Label() { HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, MaxLines = 2 };
        /* Reuse same the same content, so name label is the same -and animates more nicely */
        // properties of the controls can be overwritten in Small.xaml.cs 
        DataTemplate _title;
        public DataTemplate Title
        {
            get
            {
                if (_title == null)
                {
                    _title = new DataTemplate(() =>
                    {
                        Label.Text = Bubble.Place.Name;
                        return Label;
                    });
                }
                return _title;
            }
        }

        DataTemplate _navigateSymbol = null;
        public DataTemplate NavigateSymbol
        {
            get
            {
                if(_navigateSymbol == null)
                {
                    return new DataTemplate(() => new Image() { Source = "icon_navigate.png", Opacity = 0.58 }); 
                }
                return _navigateSymbol;
            }
        }
        
        DataTemplate _distance;

        public DataTemplate GetDistance(MainViewModel main)
        {
            if (_distance == null)
            {
                _distance = new DataTemplate(() =>
                {

                    var distance = new Label()
                    {
                        Text = main.DistanceToString(Bubble.Place),
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = 10,
                        BindingContext = this,
                        VerticalTextAlignment = TextAlignment.Center//,
                        //AutoFit = AutoFit.Lines,
                    };

                    Logic.DependencyBox.Get<LocationService>().Location.Subscribe((location, _) =>
                    {
                        var distanceLabel = main.DistanceToString(Bubble.Place);
                        Logic.MainThread.Invoke(() =>
                        {
                            distance.Text = distanceLabel;
                        });
                    });

                    return distance;
                });
            }
            return _distance;
        }

        // templates that are shared between large content's

        DataTemplate _navigate;
        public DataTemplate Navigate
        {
            get
            {
                if (_navigate == null)
                {
                    _navigate = new DataTemplate(() =>
                    {
                        // grid for with percentage
                        var withPercentage = new AbsoluteLayout() { VerticalOptions = LayoutOptions.EndAndExpand, Margin = new Thickness(0, 0, 0, 0) };
                        // for some reason imagebutton was not working on ios.

                        View navigate = NavigateControl((View view) =>
                        {
                            Logic.Log("navigate");
                            //Arrow.Navigate(Visual);
                            
                        });
                        
                        AbsoluteLayout.SetLayoutFlags(navigate, AbsoluteLayoutFlags.All);
                        AbsoluteLayout.SetLayoutBounds(navigate, new Rectangle(0.5, 0, 0.70, 1));

                        withPercentage.Children.Add(navigate);
                        return withPercentage;
                    });
                }
                return _navigate;
            }
            
        }

        /* needing seperate controls here due to clicked event of imagebutton on ios not firing combined
           with the needed ios gestures on visual - and being
           unable to use button with icon on android due to it being displayed wrong. The Android probelm
           seems to only arise using the custom png, built from 2 icons.
        */
        View NavigateControl(Action<View> clicked)
        {
            View commonButton = null;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var button = new Button() { ImageSource = "navigate_walk.png", HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 43, BackgroundColor = Color.FromRgba(70, 133, 212, 118), Margin = new Thickness(0, 0, 0, 0), InputTransparent = false};
                button.Clicked += (object sender, EventArgs e) => clicked.Invoke(button);
                commonButton = button;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                var imageButton = new ImageButton() { Source = "navigate_walk.png", HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 43, BackgroundColor = Color.FromRgba(70, 133, 212, 118), Margin = new Thickness(0, 0, 0, 0) };
                imageButton.Clicked += (object sender, EventArgs e) => clicked.Invoke(imageButton);
                commonButton = imageButton;
            }
            return commonButton;
            
        }
        
        DataTemplate _close;
        public DataTemplate Close
        {
            get
            {
                if(_close == null)
                {
                    _close = new DataTemplate(() =>
                    {
                        var close = new ImageButton() { Source = "icon_abort.png", Margin = new Thickness(0, 0, 0, 0), Opacity = 0.62, BackgroundColor = Color.Transparent};
                        return close;
                    });
                }
                return _close;
            }
        }
    }
}
