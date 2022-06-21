using System.Linq;
using Android.Graphics;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FormsTintImageEffect = Seek.Controls.Effects.TintImageEffect;

[assembly: ResolutionGroupName(Seek.Controls.Effects.TintImageEffect.GroupName)]
[assembly: ExportEffect(typeof(Seek.Droid.Controls.Effects.TintImageEffect), Seek.Controls.Effects.TintImageEffect.Name)]
namespace Seek.Droid.Controls.Effects
{
    public class TintImageEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                /*
                var effect = (FormsTintImageEffect)Element.Effects.FirstOrDefault(e => e is FormsTintImageEffect);

                if (effect == null || !(Control is Android.Support.V7.Widget.AppCompatImageButton imageButton)) // Control is ImageView image
                    return;
                
                var c = effect.TintColor;
                var filter = new PorterDuffColorFilter(new Android.Graphics.Color(c.R, c.G, c.B, c.A), PorterDuff.Mode.SrcIn);
                // image.SetColorFilter(filter);
                imageButton.SetColorFilter(filter);
                */
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"An error occurred when setting the {typeof(TintImageEffect)} effect: {ex.Message}\n{ex.StackTrace}");
            }
        }

        protected override void OnDetached() { }
    }
    
}
/* There is no Control property ?!
namespace Test
{
    public class See : ImageButtonRenderer
    {
        public See(Android.Content.Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ImageButton> e)
        {
            base.OnElementChanged(e);
            this.
        }
    }
}*/