using System;
using XamarinLogic;
using XamarinLogic.ViewModels;
using Seek.Visualization.Support;
using Xamarin.Forms;

namespace Seek.Visualization.Contents.Large
{
    public partial class Default : ContentView, IContent
    {
        MainViewModel MainViewModel { get; }
        public Default()
        {
            InitializeComponent();
            MainViewModel = Logic.DependencyBox.Get<MainViewModel>();
        }

        View NavigateButton { get; set; }
        public void Set(Templates templates)
        {
            container.Children.Add(new Title(templates.Close.CreateContent(), templates.Title.CreateContent()));
            NavigateButton = (View)templates.Navigate.CreateContent();

            /*
            MainViewModel.Get().WhenAnyValue((m) => m.Destination).Subscribe((d) =>
            {
                if (d.Value == templates.Visual)
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
        }


    }
}
