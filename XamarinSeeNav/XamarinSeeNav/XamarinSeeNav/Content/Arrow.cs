using System;
using System.Threading.Tasks;
using LogicLibrary;
using LogicLibrary.Game;
using LogicLibrary.Models.Analytics;
using LogicLibrary.Services;
using LogicLibrary.Services.PermissionRequired;
using LogicLibrary.Utils;
using LogicLibrary.ViewModels;
using Seek.Display;
using Seek.Visualization;
using Xamarin.Forms;
using AbsoluteLayout = Xamarin.Forms.AbsoluteLayout;

namespace Seek.Content
{
    public class Arrow : IContent
    {
        static Arrow instance; // Self attribute somehow?
        public static Arrow Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Arrow();
                }
                return instance;
            }
        }

        public AbsoluteLayout Layout { get; } = new AbsoluteLayout()
        {
            BackgroundColor = Color.White
        };

        Image BackgroundImage { get; } = new Image()
        {
            Source = ImageSource.FromFile("arrow_background.jpg"),
            Aspect = Aspect.AspectFill,
            Opacity = 0.7
        };

        StackLayout Container { get; } = new StackLayout();

        Forms9Patch.Label NameLabel { get; } = new Forms9Patch.Label()
        {
            HorizontalOptions = LayoutOptions.Center,
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            VerticalTextAlignment = TextAlignment.End,
            HorizontalTextAlignment = TextAlignment.Center,
            Margin = 20,
        };

        Image ArrowImage { get; } = new Image()
        {
            Source = ImageSource.FromFile("arrow.png"),
            VerticalOptions = LayoutOptions.CenterAndExpand,
            Opacity = 0.78,
            Margin = new Thickness(0, 80, 0, 0)
        };

        Forms9Patch.Label DistanceLabel { get; } = new Forms9Patch.Label()
        {
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            VerticalOptions = LayoutOptions.StartAndExpand,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            Margin = new Thickness(0, 30, 0, 0)
        };

        ImageButton AbortButton { get; } = new ImageButton()
        {
            Source = ImageSource.FromFile("icon_abort.png"),
            BackgroundColor = Color.Transparent,
            Opacity = 0.60
        };

        MainViewModel MainViewModel { get; }

        protected Arrow()
        {
            MainViewModel = Logic.DependencyBox.Get<MainViewModel>();
            Set();
        }

        void Set()
        {
            Layout.Fill(AbsoluteLayoutFlags.All, 0, 0, 1, 1);

            // AbsoluteLayout.SetLayoutFlags(Container, AbsoluteLayoutFlags.SizeProportional);
            Container.Fill(AbsoluteLayoutFlags.All, 0, 0, 1, 1);
            var padding = Container.Padding;
            var paddingHeight = DependencyService.Get<IStatusBar>().GetHeight();
            Container.Padding = new Thickness(0, paddingHeight, 0, 0);

            BackgroundImage.Fill(AbsoluteLayoutFlags.All, 0, 0, 1, 1);

            MainViewModel.Destination.Subscribe((d, _) =>
            {
                if (d == null)
                {
                    return;
                }
                // new desination set
                ArrowImageRotation();

                Logic.MainThread(() =>
                {
                    NameLabel.Text = d.Name;
                    SetDistanceText(GetDistanceText());
                });
            });
            Logic.DependencyBox.Get<LocationService>().Location.Subscribe((location, _) => SetDistanceText(GetDistanceText()));
            // also need to add bearing below

            AbortButton.Fill(AbsoluteLayoutFlags.PositionProportional, 0.955, 0.12, 40, 40);

            AbortButton.Clicked += AbortButton_Clicked;

            AbsoluteLayout.SetLayoutFlags(Container, AbsoluteLayoutFlags.SizeProportional);

            Container.Add(NameLabel, ArrowImage, DistanceLabel);

            Layout.Add(BackgroundImage, Container, AbortButton);

            SensorFading.CurrentOrientation.Subscribe(SendArrowShown);
        }

        public void SendArrowShown(Orientation value, int iterator)
        {
            var destination = MainViewModel.Destination.Value;
            if (destination == null || value != Orientation.Down)
            {
                return;
            }
            var arrowShown = new ArrowShown(destination.Name, MainViewModel.GetDistance(destination), MainViewModel.DistanceToString(destination));
            Logic.DependencyBox.Get<AnalyticsService>().Track(arrowShown);
        }

        void SetDistanceText(string text)
        {
            Logic.MainThread.Invoke(() =>
            {
                DistanceLabel.Text = text;
            });
        }
        /*
        string DistanceText
        {
            get
            {
                var main = MainViewModel.Get();

                var destination = main.Destination;
                if (destination.Value == null)
                {
                    // should never be seen, as you can't have arrow open without a desination
                    return "No desination set"; 
                }

                return MainViewModel.Get().DistanceToString(destination.Value.Place);
            }
        }
        */

        string GetDistanceText()
        {
            var destination = MainViewModel.Destination;
            if (destination.Value == null)
            {
                // should never be seen, as you can't have arrow open without a desination
                return "No desination set";
            }

            return MainViewModel.DistanceToString(MainViewModel.Destination.Value);
        }


        EventRegistration CurrentArrowUpdater { get; set; }

        void ArrowImageRotation()
        {
             // stop previous navigation to destination

            CurrentArrowUpdater = new EventRegistration(100);
            _ = CurrentArrowUpdater.PeriodicTask.Run(() =>
            {
                var place = MainViewModel.Destination.Value;
                if (place == null) // when desination is 
                {
                    CurrentArrowUpdater?.PeriodicTask.CancellationTokenSource.Cancel();
                    return;
                }

                var heading = MainViewModel.Orientation.Value.Heading();
                var bearing = MainViewModel.GetBearing(Logic.DependencyBox.Get<LocationService>().Location.Value, place.Location);

                var normalizedHeading = heading; // [-180, 180] -> [0, 360]

                Logic.MainThread.Invoke(() =>
                {
                    ArrowImage.Rotation = normalizedHeading + bearing;
                });

            });
        }

        public Bubble DestinationAborting { get; private set; }
        async void AbortButton_Clicked(object sender, EventArgs e)
        {
            if (DestinationAborting != null)
            {
                return;
            }

            var main = Logic.DependencyBox.Get<MainViewModel>();
            DestinationAborting = Visualize.GetBubbleFor(main.Destination.Value);
            /* When entering arrow by via tilt deselection need to happen and is igly initial if not for delay */

            /* for enlarge visual to determine wether or not to show the navigate button and desintation/navigatesymbol on small visuals
               placed here so that navigte symbol isen't displayed while deselect animating visual */
            main.Destination.Set(null);
            //var touchTime = BubbleEffects.Touch(DestinationAborting);
            //await Task.Delay((int)(touchTime * 4)); // for some reason extra wait is needed to not 

            await Task.Delay(80);
            DestinationAborting.SetIsSelected(false); // <--
            //var rest = BubbleEffects.Release(DestinationAborting);
            //await Task.Delay((int)rest);
            DestinationAborting = null;
        }
    }
}
