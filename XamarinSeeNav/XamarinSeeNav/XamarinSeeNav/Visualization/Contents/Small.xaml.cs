using System;
using XamarinLogic;
using XamarinLogic.Models;
using XamarinLogic.ViewModels;
using Seek.Visualization.Support;
using Xamarin.Forms;


namespace Seek.Visualization.Contents
{
    public partial class Small : ContentView, IContent
    {
        MainViewModel MainViewModel { get; }
        public Small()
        {
            InitializeComponent();

            MainViewModel = Logic.DependencyBox.Get<MainViewModel>();

            MainViewModel.Destination.Subscribe(SetNavigationSymbol);
        }

        Templates Templates { get; set; }

        public void Set(Templates templates)
        {
            Templates = templates;

            SetNavigationSymbol(MainViewModel.Destination.Value, 0);

            var title = (View)templates.Title.CreateContent();
            // due to both large contents and arrow uses a larger fontsize it has to be set back
            ((Label)title).FontSize = Device.GetNamedSize(NamedSize.Small, title);
            title.VerticalOptions = LayoutOptions.EndAndExpand;
            layout.Children.Add(title);

            var distance = (View)templates.GetDistance(MainViewModel).CreateContent();
            distance.VerticalOptions = LayoutOptions.CenterAndExpand;
            //distance.Margin = new Thickness(0, 0, 0, 0);
            layout.Children.Add(distance);
        }

        View NavigateSymbol { get; set; }

        void SetNavigationSymbol(Place destination, int iterator)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                bool isDestination = destination == Templates.Bubble.Place;
                if (isDestination)
                {
                    var navigateSymbol = (Image)Templates.NavigateSymbol.CreateContent();
                    navigateSymbol.Margin = 0;

                    var v = Constants.Size.Width;
                    AbsoluteLayout.SetLayoutFlags(navigateSymbol, AbsoluteLayoutFlags.All); // note this en enlarged
                    // .. but absolute layout has to be justed to elements get centered from their middle 
                    AbsoluteLayout.SetLayoutBounds(navigateSymbol, new Rectangle(0.5, 0.1, 0.2, 0.2));

                    if (!absoluteLayout.Children.Contains(navigateSymbol))
                    {
                        absoluteLayout.Children.Add(navigateSymbol);
                        NavigateSymbol = navigateSymbol;
                    }
                }
                else
                {
                    if (NavigateSymbol != null)
                    {
                        if (absoluteLayout.Children.Contains(NavigateSymbol))
                        {
                            absoluteLayout.Children.Remove(NavigateSymbol);
                        }
                    }
                }
            });
        }
    }
}
