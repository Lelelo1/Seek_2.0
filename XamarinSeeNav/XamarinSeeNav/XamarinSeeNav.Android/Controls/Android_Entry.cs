using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Util;
using Android.Widget;
using Seek.Controls;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Seek.Controls.Entry), typeof(Seek.Droid.Controls.Android_Entry))]
namespace Seek.Droid.Controls
{
    public class Android_Entry : EntryRenderer
    {
        public Android_Entry(Android.Content.Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);
            
            if (Control != null && e.NewElement is Entry entry)
            {
                // init
                Console.WriteLine("init entry renderer android");

                var drawable = new GradientDrawable();

                /* // color is handled by entry's background property, coloredarea instead 
                var c = entry.Color;
                drawable.SetColor(Android.Graphics.Color.Argb(c.A, c.R, c.G, c.B));
                */
                drawable.SetCornerRadius(entry.CornerRadius);

                // https://forums.xamarin.com/discussion/180103/entry-vertical-text-alignment-inconsistency-between-ios-and-android
                // Control.Gravity = Android.Views.GravityFlags.CenterVertical;
                Control.SetPadding(0, 0, 3, 0); // centers placeholder text vertically, atleast on Samsung GalaxyA20e

                Control.Background = drawable;
                
                entry.PropertyChanged += Entry_PropertyChanged;

                entry.TextChanged += Entry_TextChanged;

                VerticallyAlignText(Control);
            }
        }

        private void Entry_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if(sender is Entry entry && Control != null)
            {
                double width = 0;
                Rect bounds = new Rect();
                var text = e.NewTextValue;
                if(!string.IsNullOrEmpty(text)) // can't get text bounds with no text 
                {
                    Control.Paint.GetTextBounds(text, 0, text.Length, bounds);
                    width = bounds.Width() / Resources.DisplayMetrics.ScaledDensity;
                }

                if(entry.CharacterWidth == 0 && text.Length == 1)
                {
                    entry.CharacterWidth = width;
                }
                entry.CurrentTextWidth = width;

            }
        }

        private void Entry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is Entry entry && Control != null)
            {
                if (e.PropertyName == "Color")
                {
                    /*
                    var c = entry.Color;
                    // Console.WriteLine("Android_Entry A: " + c.A);
                    ((GradientDrawable)Control.Background).SetColor(Android.Graphics.Color.Argb(c.A, c.R, c.G, c.B));
                    */
                }
                else if (e.PropertyName == "CornerRadius")
                {
                    ((GradientDrawable)Control.Background).SetCornerRadius(entry.CornerRadius);// .CornerRadius = entry.CornerRadius;
   
                }
                else if (e.PropertyName == "HiddenBorders")
                {
                    SetBorders(Control, entry.HiddenBorders);
                }
            }
        }

        void SetBorders(EditText editText, bool hidden)
        {
            if (!hidden)
            {
                // somehow use the original background drawable or stroke the one
                // drawable.SetStroke()
            }
            else
            {
                // are already hidden by creating a new drawable that is set to background
            }
        }
        void VerticallyAlignText(EditText editText)
        {
            editText.Gravity = Android.Views.GravityFlags.CenterVertical;
        }
    }
}
