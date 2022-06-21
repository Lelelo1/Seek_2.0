using System;
using Xamarin.Forms;
using Seek.Pages;
using ImageButton = Xamarin.Forms.ImageButton;
using Seek.Display;
using XamarinLogic.ViewModels;
using XamarinLogic;

namespace Seek.Controls
{
    public class SearchEntry : DisplayBase
    { 
        /* (It looks nicer if alfa is 0 though) */
        public Color Color { get; set; }
        /*
        public Settings.ViewModel Settings { get; set; } = Seek.Settings.ViewModel.Get();
        public bool Disabled
        {
            get => Settings.Transition.EntryDisabled.Value;
            set => Settings.Transition.EntryDisabled.OnNext(value);
        }
        */
        // public TransparencyHandler TransparencyHandler { get; set; }

        public object BindingContext { get => Entry.BindingContext; set => Entry.BindingContext = value; }

        public Seek.Controls.Entry Entry { get; }
        public ImageButton Clear { get; }

        double Size { get; set; } = 40;
        double ClearButtonSpaceFromText { get; set; } = 50;

        //double CornerRadius { get => Entry.CornerRadius; set { Entry.CornerRadius = (float)value; Background.CornerRadius = value; } }
        Thickness margin = new Thickness(2, 10, 2, 0);
        Thickness Margin { get => margin; set { margin = value; } }

        public SearchEntry(Seek.Controls.Entry entry, ImageButton clear)
        {

            Entry = entry;
            Clear = clear;

            if (Device.RuntimePlatform == Device.iOS)
            {
                //CornerRadius = 7f;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                //CornerRadius = 15f;
            }

            SetBounds(Screen);

        }

        public void SetBounds (Rectangle bounds)
        {
            // on android all content, camera view and mainContent are not placed under the statusbar. only on ios.
            //Logic.Utils.Log.Message("screen bounds was: " + bounds);

            var statusBarHeight = ARPage.Instance.StatusBarHeight;
            AbsoluteLayout.SetLayoutBounds(Entry, new Rectangle(Margin.Left + 0, Margin.Top + statusBarHeight, GetInputTextWidthLimit(bounds.Width - Margin.Right), Size - Margin.Bottom));

            // setting background to same bounds as entry initially
            //AbsoluteLayout.SetLayoutBounds(Background, new Rectangle(Margin.Left + 0, Margin.Top + statusBarHeight, bounds.Width - Margin.Right, Size - Margin.Bottom));

            // handling dynamic clear button
            Entry.TextChanged += (object sender, TextChangedEventArgs e) => SetClear(bounds);

            Clear.Clicked += (object sender, EventArgs e) => Logic.DependencyBox.Get<SearchViewModel>().CurrentInputText.Set(null);

            SetClear(bounds);

            /* possibly enlarge clear button when cameraview ? */
        }

        public void SetClear(Rectangle bounds)
        {
            bool hasText = !string.IsNullOrEmpty(Entry.Text);

            bool textReachedMaxWidth = Entry.CurrentTextWidth >= GetInputTextWidthLimit(bounds.Width);
            if (!textReachedMaxWidth)
            {
                var textWidth = Entry.CurrentTextWidth + Entry.TextLeftMargin;
                var clearSize = 25;
                var y = ARPage.Instance.StatusBarHeight + (Size / 2) - (clearSize / 2);
                AbsoluteLayout.SetLayoutBounds(Clear, new Rectangle(Margin.Left + (textWidth + ClearButtonSpaceFromText), Margin.Top + y, clearSize - Margin.Right, clearSize - margin.Bottom));
            }

            Clear.IsVisible = hasText;
        }

        // so that text enetered never pushes clear button out of the screen. needs setting once only
        public double GetInputTextWidthLimit(double screenWidth)
        {
            return screenWidth - (ClearButtonSpaceFromText + Size);
        }
        //public new Thickness Margin { get => this.set; }
    }
}
