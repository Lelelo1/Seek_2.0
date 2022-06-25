using System;

using Seek.Visualization.Support;
using Xamarin.Forms;
using LogicLibrary.ViewModels;
using LogicLibrary;
// Place, Custom, Commute/Checkpoint, Contact
namespace Seek.Visualization.Contents.Large
{
    public partial class Google : ContentView, IContent
    {
        MainViewModel MainViewModel { get; }

        public Google()
        {
            MainViewModel = Logic.DependencyBox.Get<MainViewModel>();
            InitializeComponent();
        }

        View NavigateButton { get; set; }

        public void Set(Templates templates)
        {
            container.Children.Insert(0, new Title(templates.Close.CreateContent(), templates.Title.CreateContent()));
            NavigateButton = (View)templates.Navigate.CreateContent();
            /*
            MainViewModel.Get().Destination.Subscribe((d) =>
            {
                if (d == templates.Visual)
                {
                    NavigateButton.IsEnabled = false;
                    NavigateButton.Opacity = 0.38;
                }
                else
                {
                    NavigateButton.IsEnabled = true;
                    NavigateButton.Opacity = 1;
                }
            });
            */

            container.Children.Add(NavigateButton);
            var distance = (View)templates.GetDistance(MainViewModel).CreateContent();
            distance.VerticalOptions = LayoutOptions.End;
            distance.Margin = new Thickness(0, 3, 0, 5);
            container.Children.Add(distance);
            BindingContext = templates.Bubble.Place;
        }
    }

}
