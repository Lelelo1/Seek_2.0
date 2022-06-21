using Android.Content;
using Android.Graphics;

using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Seek.Controls;
using Seek.Droid.Controls.Camera2Forms.Camera2;
/* cred to vtserej: https://github.com/vtserej/Camera2Forms/blob/master/Camera2Forms/Camera2Forms/CustomViews/CameraPreview.cs */
[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]
namespace Seek.Droid.Controls.Camera2Forms.Camera2
{
    public class CameraViewRenderer : ViewRenderer
	{

        public CameraViewRenderer(Context context) : base(context)
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

            if(e.NewElement != null) {
                var formsCameraView = e.NewElement as CameraView;
                Console.WriteLine("formsCameraView: " + formsCameraView);
                
                formsCameraView.OnStart += () =>
                {
                    Console.WriteLine("Start");

                    var cameraView = CameraDroid.Get();
                    SetNativeControl(cameraView);
                    cameraView.StartPreview();
                    
                };
                formsCameraView.OnStop += () =>
                {
                    Console.WriteLine("Stop");
                    CameraDroid.Get().CameraDevice.Close();
                    //throw new NotImplementedException("CameraView Stop not implemented in Android CameraViewRender");
                };
            
            }
		}

        
	}
    public enum CameraOptions
    {
        Rear,
        Front
    }
}
