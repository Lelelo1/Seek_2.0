using System;
using Xamarin.Forms;

namespace SeeNav.Display.Internal
{
    // visiblity and sensorfade wrapper
    public class ExtendedView
    {
        public View View { get; }
        // initial
        double InitialOpacity { get; }
        bool InitialInputTransparent { get; }

        public Func<bool> ShouldFade { get; set; } = () => false;

        public bool Fading => IsVisible && ShouldFade.Invoke();

        bool isVisible;

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                SetVisibility(isVisible);
            }
        }

        public ExtendedView(View view)
        {
            View = view;
            InitialOpacity = view.Opacity;
            InitialInputTransparent = view.InputTransparent;
        }

        void SetVisibility(bool isVisible)
        {
            
            var setOpacity = isVisible ? InitialOpacity : 0;
            setOpacity = Fading ? SensorFading.CurrentValue : setOpacity;
            var setInputTransparent = isVisible ? InitialInputTransparent : true;

            View.Opacity = setOpacity;
            View.InputTransparent = setInputTransparent;

            // trigger a senorfade value update, otherwise slightly moving device is needed aften setting visiblity to false;
        }
    }
}
