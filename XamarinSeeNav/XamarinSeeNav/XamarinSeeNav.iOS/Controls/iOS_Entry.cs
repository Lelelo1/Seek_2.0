using System;
using System.ComponentModel;
using CoreGraphics;
using Seek.Controls;
using Seek.iOS.Controls;
using UIKit;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Seek.Controls.Entry), typeof(Seek.iOS.Controls.iOS_Entry))]
namespace Seek.iOS.Controls
{
    public class iOS_Entry : Xamarin.Forms.Platform.iOS.EntryRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);
            
            if(Control != null && e.NewElement is Entry entry)
            {
                /*
                var c = entry.Color;
                Control.BackgroundColor = UIKit.UIColor.FromRGBA(c.R, c.G, c.B, c.A);
                Control.Layer.CornerRadius = entry.CornerRadius;
                */
                SetBorders(Control, entry.HiddenBorders);

                entry.PropertyChanged += Entry_PropertyChanged;

                entry.TextChanged += Entry_TextChanged;
            }
        }
        CGRect EditingRect { get; set; } 
        private void Entry_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (sender is Entry entry && Control != null)
            {
                if(string.IsNullOrEmpty(Control.Text))
                {
                    return;
                }

                var length = Control.Text.StringSize(Control.Font);
                entry.TextLeftMargin = Control.EditingRect(Control.Bounds).X;
                if (entry.CharacterWidth == 0 && entry.Text.Length == 1)
                {
                    entry.CharacterWidth = length.Width;
                }
                entry.CurrentTextWidth = length.Width;
                
            }
        }

        private void Entry_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is Entry entry && Control != null)
            {
                if (e.PropertyName == "Color")
                {
                    /*
                    var c = entry.Color;
                    Control.BackgroundColor = UIKit.UIColor.FromRGBA(c.R, c.G, c.B, c.A);
                    */
                }
                else if (e.PropertyName == "CornerRadius")
                {
                    Control.Layer.CornerRadius = entry.CornerRadius;
                }
                else if(e.PropertyName == "HiddenBorders")
                {
                    SetBorders(Control, entry.HiddenBorders);
                }
            }
        }
        void SetBorders(UIKit.UITextField textField, bool hidden)
        {
            if(hidden)
            {
                textField.BorderStyle = UIKit.UITextBorderStyle.None;
            }
        }
        
    }
}
