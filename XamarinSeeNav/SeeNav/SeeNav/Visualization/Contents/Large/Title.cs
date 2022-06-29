using System;

using Xamarin.Forms;
using Forms9Patch;
using AbsoluteLayout = Xamarin.Forms.AbsoluteLayout;
using Label = Forms9Patch.Label;
using ContentView = Xamarin.Forms.ContentView;

namespace SeeNav.Visualization.Contents.Large
{
    public class Title : ContentView
    {
        public Title(object close, object title) // reuse title area for both small and large content?
        {

            var layout = new AbsoluteLayout();
            // layout.Children.Add((View)close); // close by tapping oustide enlarged visual (large content) instead
            layout.Children.Add((View)title); 

            var height = 60;
            layout.HeightRequest = height;
            var size = height / 1.65;
            var margin = (height - size) / 2;
            // 17.5 is half the remaining distance given for the size and heightrequest
            AbsoluteLayout.SetLayoutBounds((View)close, new Rectangle(margin, margin, size, size));

            Label _title = (Label)title;
            //_title.FontAttributes = FontAttributes.Bold;
            _title.FontSize = Device.GetNamedSize(NamedSize.Medium, _title);

            AbsoluteLayout.SetLayoutFlags((View) title, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds((View)title, new Rectangle(0.5, 0, 0.5, 1));
            Content = layout;
        }
    }
}

