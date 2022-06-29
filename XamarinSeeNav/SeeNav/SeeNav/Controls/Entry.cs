using System;
using Xamarin.Forms;

namespace SeeNav.Controls
{
    public class Entry : Xamarin.Forms.Entry
    {
        // in xamarin.forms.bounds eg unit

        // so that dynamic clear button can be handled
        public delegate void CurrentTextWidthChanged();
        public event CurrentTextWidthChanged OnCurrentTextWidthChanged;
        double _currentTextWidth; /* I still have no good way of event signaling, observable/reactivity in forms controls :( */
        public double CurrentTextWidth { get => _currentTextWidth; set { _currentTextWidth = value; OnCurrentTextWidthChanged?.Invoke(); } }

        public double TextLeftMargin { get; set; } // on iOS there is a defualt left margin - so it has to be added to CurrentTextWidth as well
        public double CharacterWidth { get; set; }

        public bool HasBeenGivenTextMaxLimit { get; set; }
        /*
        public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }
        public static BindableProperty ColorProperty = BindableProperty.CreateAttached("Color", typeof(Color), typeof(Entry), new Color(255, 255, 255));
        */
        // should probably 
        public float CornerRadius { get => (float)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
        public static BindableProperty CornerRadiusProperty = BindableProperty.CreateAttached("CornerRadius", typeof(float), typeof(Entry), 0f);

        // hides pixel edges on ios. removes underline on android
        public bool HiddenBorders { get => (bool)GetValue(HiddenBordersProperty); set => SetValue(HiddenBordersProperty, value); }
        public static BindableProperty HiddenBordersProperty = BindableProperty.CreateAttached("HiddenBorders", typeof(bool), typeof(Entry), true);

        public Entry()
        {
            // properties previsouly in xaml:
            TextColor = Xamarin.Forms.Color.White;
            PlaceholderColor = Xamarin.Forms.Color.White;
            HiddenBorders = true;
            CornerRadius = 0;
            Margin = 0;
            BackgroundColor = Xamarin.Forms.Color.Transparent;
        }

    }
}
