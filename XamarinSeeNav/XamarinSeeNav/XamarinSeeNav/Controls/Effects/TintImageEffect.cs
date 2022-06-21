using System;
using Xamarin.Forms;
namespace Seek.Controls.Effects
{
    /* Not used. Keep just remmber how to do effect */
    public class TintImageEffect : RoutingEffect
    {
        // https://byteloom.marek-mierzwa.com/mobile/2018/02/07/setting-tint-color-in-xamarin-form-image.html
        public const string GroupName = "Seek";// "MyCompany";
        public const string Name = "TintImageEffect";

        public Color TintColor { get; set; }

        public TintImageEffect() : base($"{GroupName}.{Name}") { }
    }
}
