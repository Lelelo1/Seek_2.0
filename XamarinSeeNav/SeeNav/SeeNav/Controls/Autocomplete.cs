using System;
using System.Collections.Generic;
using LogicLibrary;
using LogicLibrary.Utils;
using LogicLibrary.ViewModels;
using Xamarin.Forms;
using Xamarin.Essentials;
using SeeNav.Content;

using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using ScrollView = Xamarin.Forms.ScrollView;
using LogicLibrary.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SeeNav.Controls
{
    public class Autocomplete
    {
        static Xamarin.Forms.Color BackgroundColor { get; } = new Color(255, 255, 255, 0);

        public ScrollView Layout { get; } = new ScrollView()
        {
            BackgroundColor = BackgroundColor,
            VerticalScrollBarVisibility = ScrollBarVisibility.Always
        };

        StackLayout Content { get; } = new StackLayout()
        {
            BackgroundColor = BackgroundColor,
            Spacing = 0
        };

        // to align suggestions somewhat after the distance between textfield and searchbar in 'Search'
        static double LeftMargin { get; } = 12;

        Action<string> OnTapSuggestion { get; }

        SearchViewModel SearchViewModel { get;  }

        public Autocomplete(Action<string> onTapSuggesion)
        {
            SearchViewModel = Logic.DependencyBox.Get<SearchViewModel>();
            OnTapSuggestion = onTapSuggesion;
            Layout.Content = Content;
            Content.HorizontalOptions = LayoutOptions.Start; // so that you cn't select suggestion by tapping to the right of suggestion text

            //Layout.Margin = new Thickness(0, 0, 0, MainPage.Instance.On<iOS>().SafeAreaInsets().Bottom);
        }

        public async void SetSuggestions(string term, Xamarin.Forms.Color textColor)
        {
            Content.Children.Clear();

            if (string.IsNullOrEmpty(term))
            {
                // when count is 0 list is still in place
                return;
            }

            var suggestions = await SearchViewModel.AppleAutocomplete(term); // can suggestions here come in wrong order when typing fast?

            //suggestions.ForEach(p => Content.Children.Add(CreateSuggestionLabel(p.Name, textColor)));
            suggestions.ForEach(s => Content.Children.Add(CreateSuggestionLabel(s, textColor)));
        }

        Label CreateSuggestionLabel(string text, Xamarin.Forms.Color textColor)
        {
            var label = new Label() {
                Text = text,
                Margin = new Thickness(LeftMargin, 5, 7, 12),
                FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label)),
                TextColor = textColor
            };

            var tapReco = new TapGestureRecognizer();
            tapReco.Tapped += (object sender, EventArgs e) => OnTapSuggestion?.Invoke(text);

            label.GestureRecognizers.Add(tapReco);

            return label;
        }

    }
}
