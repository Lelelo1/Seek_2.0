using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Seek.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Seek.Controls.ColoredArea), typeof(Seek.Droid.Controls.Android_ColoredArea))]
namespace Seek.Droid.Controls
{
    public class Android_ColoredArea : ViewRenderer
    {
        public Android_ColoredArea(Android.Content.Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement is ColoredArea coloredArea)
            {
                SetColor(coloredArea);
                SetCornerRadius(coloredArea);
                coloredArea.PropertyChanged += ColoredArea_PropertyChanged;
            }
        }

        private void ColoredArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is ColoredArea coloredArea)
            {
                if(nameof(coloredArea.Color) == e.PropertyName)
                {
                    SetColor(coloredArea);
                }
                else if(nameof(coloredArea.CornerRadius) == e.PropertyName)
                {
                    SetCornerRadius(coloredArea);
                }
            }
        }

        void SetColor(ColoredArea forms)
        {

            var c = forms.Color;
            var filter = new PorterDuffColorFilter(Android.Graphics.Color.Argb(c.A, c.R, c.G, c.B), PorterDuff.Mode.SrcIn);
            if(Background == null)
            {
                Background = new GradientDrawable();
            }
            Background.SetColorFilter(filter);
        }
        void SetCornerRadius(ColoredArea forms)
        {
            if (Background == null)
            {
                Background = new GradientDrawable();
            }
            var gradientDrawable = (GradientDrawable)Background;
            gradientDrawable.SetCornerRadius((float)forms.CornerRadius);
        }
    }
}
