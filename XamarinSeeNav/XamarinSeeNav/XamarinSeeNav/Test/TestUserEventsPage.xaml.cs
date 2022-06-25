using System;
using Xamarin.Forms;
using System.Linq;
using Logic.Native;
using Logic;
using Logic.Services;

// page to see history of analtics events for testing

// should use 'User' instead of native 'IUser'. maybe make test for both......
namespace Seek.Test
{
    public partial class TestUserEventsPage : ContentPage
    {
        public TestUserEventsPage()
        {
            InitializeComponent();
        }

        Label ToLabel(string text)
        {
            return new Label() { Text = text };
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            /*
            var list = Logic.DependencyBox.Get<HistoryService>().Read();
            list.ToList().ForEach(e =>
            {
                listA.Children.Add(ToLabel(e.ToString()));
            });
            */
        }

    }
}
