using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.ComponentModel;
using Foundation;
using Logic;

// needs testing testing ios lower than 13, it could cause the textfieldcolor to not be set, see 'SetTextFieldColor'

[assembly: Xamarin.Forms.ExportRenderer(typeof(Seek.Controls.SearchBar), typeof(Seek.iOS.Controls.iOS_SearchBar))]
namespace Seek.iOS.Controls
{
    public class iOS_SearchBar : Xamarin.Forms.Platform.iOS.SearchBarRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null && e.NewElement is Seek.Controls.SearchBar searchBar)
            {
                var uiSearchBar = Control;

                // can' be set with 'searchBar.On<iOS>().SetSearchBarStyle' platform specific, in forms.
                uiSearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
                SetTextFieldColor(searchBar, uiSearchBar);
                // barbackground is used if SearchBarStyle is not set to 'Minimal'
                SetBarBackgroundColor(searchBar, uiSearchBar);
                SetEnableBorder(searchBar, uiSearchBar);

                SetSearchIconColor(searchBar, uiSearchBar);
                //SetPlaceholderColor(searchBar, uiSearchBar); // don't matter
                // ugly square
                //uiSearchBar.SearchTextField.Layer.BorderWidth = 1.2f;
                //uiSearchBar.SearchTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var uiSearchBar = Control;
            if(uiSearchBar == null)
            {
                return;
            }

            // https://gist.github.com/xleon/9f94a8482162460ceaf9
            Control.ShowsCancelButton = false; // can't hide cancel button in 'OnElementChanged'

            var searchBar = (Seek.Controls.SearchBar)sender;

            if (e.PropertyName == nameof(searchBar.TextFieldColor))
            {
                SetTextFieldColor(searchBar, uiSearchBar);
            }
            else if (e.PropertyName == nameof(searchBar.BarBackgroundColor))
            {
                SetBarBackgroundColor(searchBar, uiSearchBar);
            }
            else if (e.PropertyName == nameof(searchBar.EnableBorder))
            {
                SetEnableBorder(searchBar, uiSearchBar);
            }
            else if (e.PropertyName == nameof(searchBar.SearchIconColor))
            {
                SetSearchIconColor(searchBar, uiSearchBar);
            }
            else if (e.PropertyName == nameof(searchBar.PlaceholderColor))
            {
                //SetPlaceholderColor(searchBar, uiSearchBar);
            }
        }


        void SetTextFieldColor(Seek.Controls.SearchBar searchBar, UISearchBar uiSearchBar)
        {
            var uiColor = searchBar.TextFieldColor.ToUIColor();
            // https://stackoverflow.com/questions/57927576/ios-13-uisearchbar-appearance-and-behaviour
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0)) // is 13 or above
            {
                uiSearchBar.SearchTextField.BackgroundColor = uiColor;
                return;
            }

            uiSearchBar.BackgroundColor = uiColor;
        }

        void SetBarBackgroundColor(Seek.Controls.SearchBar searchBar, UISearchBar uiSearchBar)
        {
            var uiColor = searchBar.BackgroundColor.ToUIColor();
            uiSearchBar.BarTintColor = uiColor;
        }

        // when being transparent
        void SetEnableBorder(Seek.Controls.SearchBar searchBar, UISearchBar uiSearchBar)
        {
            // https://stackoverflow.com/questions/58830159/nsinvalidargumentexception-reason-uisearchbar-searchtextfield-unrecogni
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0) == false)
            {
                return;
            }

            // some sort of bug causing it to not being able to be reset to 'RoundedRect'
            uiSearchBar.SearchTextField.BorderStyle = searchBar.EnableBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;

            // some ways not working to remove the partially transperent siluett of the textfield
            // uiSearchBar.SearchTextField.Layer.BorderWidth = searchBar.EnableBorder ? iOSBorderWidth : 0;
            // uiSearchBar.SearchTextField.Layer.BorderColor = searchBar.EnableBorder ? iOSColor : CGColor.CreateSrgb(0, 0, 0, 0);
            // uiSearchBar.BackgroundImage = new UIImage();     
            // uiSearchBar.SearchTextField.Layer.Hidden = !searchBar.EnableBorder;
        }

        void SetSearchIconColor(Seek.Controls.SearchBar searchBar, UISearchBar uiSearchBar)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0) == false)
            {
                return;
            }

            uiSearchBar.SearchTextField.LeftView.TintColor = searchBar.SearchIconColor.ToUIColor();
        }
        void SetPlaceholderColor(Seek.Controls.SearchBar searchBar, UISearchBar uiSearchBar)
        {
            var uiColor = searchBar.PlaceholderColor.ToUIColor();
            var text = new NSAttributedString(searchBar.Placeholder, foregroundColor: uiColor);
            // https://stackoverflow.com/questions/57927576/ios-13-uisearchbar-appearance-and-behaviour
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0)) // is 13 or above
            {
                // https://stackoverflow.com/questions/60877192/how-can-i-change-the-placeholder-text-color-in-xamarin
                uiSearchBar.SearchTextField.AttributedPlaceholder = text;
                return;
            }

            // there is no need for a solution below ios 13

            //uiSearchBar.att
        }
    }

}
