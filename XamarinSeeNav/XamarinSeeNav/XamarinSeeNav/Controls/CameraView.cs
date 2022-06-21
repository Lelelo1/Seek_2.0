using System;
using Xamarin.Forms;

// NOT USED
namespace Seek.Controls
{

    public class CameraView : StackLayout
    {
        public bool IsActive = false;

        public delegate void StartEvent();
        public event StartEvent OnStart;

        public void Start()
        {

            if(Device.RuntimePlatform == Device.iOS)
            {
                InternalStart();
                return;
            }


            if(Device.RuntimePlatform == Device.Android)
            {
                /* potentially fix in android impl so handler isen't neccesary */
                SizeChanged += (object sender, EventArgs e) =>
                {
                    InternalStart();
                    return;
                };
            }

            
        }

        void InternalStart()
        {
            OnStart();
            IsActive = true;
        }

        /* tapping on non active camera view could led to app permission settings or maybe not. I don't want users to get confused
           with visuals without camera :/ */

        public delegate void StopEvent();
        public event StopEvent OnStop;

        public void Stop()
        {
            IsActive = false;
            OnStop();
        }

        public static double FullOverlayOpacityValue { get; } = 0.845;
        public double OverlayOpacity { get => (double)GetValue(OverlayOpacityProperty); set => SetValue(OverlayOpacityProperty, value); }
        public static BindableProperty OverlayOpacityProperty = BindableProperty.CreateAttached(nameof(OverlayOpacity), typeof(double), typeof(CameraView), FullOverlayOpacityValue);
    }
}
