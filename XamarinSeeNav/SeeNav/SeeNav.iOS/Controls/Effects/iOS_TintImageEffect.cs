using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsTintImageEffect = SeeNav.Controls.Effects.TintImageEffect;

[assembly: ResolutionGroupName(SeeNav.Controls.Effects.TintImageEffect.GroupName)]
[assembly: ExportEffect(typeof(SeeNav.iOS.Controls.Effects.TintImageEffect), SeeNav.Controls.Effects.TintImageEffect.Name)]
namespace SeeNav.iOS.Controls.Effects
{
    public class TintImageEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            /*
            try
            {
                var effect = (FormsTintImageEffect)Element.Effects.FirstOrDefault(e => e is FormsTintImageEffect);
                
                if (effect == null || !(Control is UIButton button)) // Control is UIImageView image || 
                    return;

                // image.Image = image.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                var c = effect.TintColor;
                // always templete makes the image assume the tintcolor of the button. For som reason the image can't be set tint directly
                button.SetImage(button.CurrentImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate), UIControlState.Normal);
                button.TintColor = UIColor.FromRGBA(c.R, c.G, c.B, c.A);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred when setting the {typeof(TintImageEffect)} effect: {ex.Message}\n{ex.StackTrace}");
            }
            */
        }

        protected override void OnDetached() { }
        /*
        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
        }
        */
    }
}

