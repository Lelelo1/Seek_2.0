using System;
using Xamarin.Forms;
//using Xamarin.Forms.PlatformConfiguration.iOSSpecific; // UISearchBarStyle

namespace Seek.Controls
{
    public class SearchBar : Xamarin.Forms.SearchBar
    {
        // can't be changed dynamically (in runtime)
        public bool EnableBorder
        {
            get => (bool)GetValue(EnableBorderProperty);
            set => SetValue(EnableBorderProperty, value);
        }

        public static BindableProperty EnableBorderProperty = BindableProperty.CreateAttached(nameof(EnableBorder), typeof(bool), typeof(SearchBar), true);

        // I am effectivly not using these as it will take the color of the background (blur camera veiw). The border (property above)
        // needs to hidden or else the shape of the searchbar textfield is shown when haing places
        public Xamarin.Forms.Color TextFieldColor { get => (Xamarin.Forms.Color)GetValue(TextFieldColorProperty); set => SetValue(TextFieldColorProperty, value); }
        public static BindableProperty TextFieldColorProperty = BindableProperty.CreateAttached(nameof(TextFieldColor), typeof(Xamarin.Forms.Color), typeof(SearchBar), Xamarin.Forms.Color.Default);

        public Xamarin.Forms.Color SearchIconColor { get => (Xamarin.Forms.Color)GetValue(SearchIconColorProperty); set => SetValue(SearchIconColorProperty, value); }
        public static BindableProperty SearchIconColorProperty = BindableProperty.CreateAttached(nameof(SearchIconColor), typeof(Xamarin.Forms.Color), typeof(SearchBar), Xamarin.Forms.Color.Default);

        public Xamarin.Forms.Color BarBackgroundColor { get => (Xamarin.Forms.Color)GetValue(BarBackgroundColorProperty); set => SetValue(BarBackgroundColorProperty, value); }
        public static BindableProperty BarBackgroundColorProperty = BindableProperty.CreateAttached(nameof(BarBackgroundColor), typeof(Xamarin.Forms.Color), typeof(SearchBar), Xamarin.Forms.Color.Default);

        // It is not possible to have emojis transparent, but I leave below code incase I need to customize in the future 
        /*
        public new Xamarin.Forms.Color PlaceholderColor { get => (Xamarin.Forms.Color)GetValue(PlaceholderColorProperty); set => SetValue(PlaceholderColorProperty, value); }
        public static new BindableProperty PlaceholderColorProperty = BindableProperty.CreateAttached(nameof(PlaceholderColor), typeof(Xamarin.Forms.Color), typeof(SearchBar), Xamarin.Forms.Color.Default);
        */

    }
}
