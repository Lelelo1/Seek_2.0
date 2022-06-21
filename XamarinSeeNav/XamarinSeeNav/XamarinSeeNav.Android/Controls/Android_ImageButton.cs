using System;
using Xamarin.Forms.Platform.Android;

using ImageButton = Seek.Controls.ImageButton;

using Android.Widget;
using Android.Graphics;
using Android.Support.V7.Widget;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Seek.Controls.ImageButton), typeof(Seek.Droid.Controls.Android_ImageButton))]
namespace Seek.Droid.Controls
{
    /* Note there is no Control property on ImageButtonRenderer it inherits AppCompatImageButton
       https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Platform.Android/AppCompat/ImageButtonRenderer.cs
       */

    public class Android_ImageButton : ImageButtonRenderer
    {
        public Android_ImageButton(Android.Content.Context context) : base(context)
        {

        }
        bool Initalized = false;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ImageButton> e)
        {
            base.OnElementChanged(e);

            if(!Initalized && e.NewElement is ImageButton imageButton)
            {
                SetColor(this, imageButton);

                imageButton.PropertyChanged += ImageButton_PropertyChanged;
                Initalized = true;
            }
        }

        private void ImageButton_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(sender is ImageButton imageButton && nameof(imageButton.Color) == e.PropertyName)
            {
                SetColor(this, imageButton);
            }
        }

        void SetColor(AppCompatImageButton native, ImageButton forms)
        {
            var c = forms.Color;
            var filter = new PorterDuffColorFilter(Android.Graphics.Color.Argb(c.A, c.R, c.G, c.B), PorterDuff.Mode.SrcIn);
            native.SetColorFilter(filter);
        }
    }
}
