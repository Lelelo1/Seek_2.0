using System;
using Xamarin.Forms;
using System.Linq;
using LogicLibrary.Native;
using LogicLibrary;
using LogicLibrary.Services;

// page to see history of analtics events for testing

// should use 'User' instead of native 'IUser'. maybe make test for both......
namespace SeeNav.Test
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
            var list = LogicLibrary.DependencyBox.Get<HistoryService>().Read();
            list.ToList().ForEach(e =>
            {
                listA.Children.Add(ToLabel(e.ToString()));
            });
            */
        }

    }
}
