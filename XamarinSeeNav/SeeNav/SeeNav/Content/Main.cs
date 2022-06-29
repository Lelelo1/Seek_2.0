using System;
using Xamarin.Forms;
using SeeNav.Display;
using LogicLibrary;

namespace SeeNav.Content
{
    public class Main : IContent
    {
        public bool Initialized => Layout.Children.Count > 0;

        static Main instance;
        public static Main Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Main();
                }
                return instance;
            }
        }

        public AbsoluteLayout Layout { get; } = new AbsoluteLayout();

        Image ContentImage { get; }

        Label AttributionLabel { get; } = new Label()
        {
            Text = "Icons by Icons8",
            FontAttributes = FontAttributes.Italic,
            TextColor = Color.FromHex("#807a0a"),
            Opacity = 0.68,
            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
        };

        protected Main()
        {
            var image = "man_middle_lantern.jpg";

            Layout.BackgroundColor = Color.Black;
            Layout.Fill(AbsoluteLayoutFlags.All, 0, 0, 1, 1);

            try
            {
                ContentImage = new Image() { Source = ImageSource.FromFile(image) };
            }
            catch(Exception exc)
            {
                Logic.Log("could not create ContentImage in MainContent, image path of " + image + " might have been invalid: " + exc.Message);
            }



            ContentImage.Fill(AbsoluteLayoutFlags.All, 0, 0, 1, 1);
            ContentImage.Aspect = Aspect.AspectFill;
            Layout.Add(ContentImage);

            AttributionLabel.Fill(AbsoluteLayoutFlags.All, 0.06, 0.96, 0.3, 0.04);
            Layout.Add(AttributionLabel);

        }
    }
}
