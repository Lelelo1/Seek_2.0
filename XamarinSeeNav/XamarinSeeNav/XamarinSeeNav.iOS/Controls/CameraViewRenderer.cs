using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AVFoundation;
using Seek.Controls;
using Seek.iOS.Controls;
using LogicLibrary.Utils;
using LogicLibrary;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]
namespace Seek.iOS.Controls
{
    public class CameraViewRenderer : ViewRenderer
    {
        CameraView CameraView { get; set; }
        iOSCameraView iOSCameraView { get; set; }
        UIVisualEffectView OverlayView { get; set; }

        // NativeView don't have a size when called from 'OnElementChanged'
        // https://forums.xamarin.com/discussion/58160/customrenderer-on-ios-sizing-to-the-bounds-of-the-custom-control

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if(Ext.HasValue(CameraView))
            {
                // could happen if using 'CameraView' in two places..?
                LogicLibrary.Log("warning, attempting to creating a new iOSCameraView was ignored");
                return;
            }

            // https://stackoverflow.com/questions/51013490/when-does-onelementchangedelementchangedeventargst-fire
            if (Ext.HasValue(e.NewElement))
            {
                SetiOSCameraViewSingleton(e.NewElement);
            }
            else if(Ext.HasValue(e.OldElement))
            {
                // retain singleton
            }
        }

        void SetiOSCameraViewSingleton(View newElement)
        {

            CameraView = (CameraView)newElement;
            CameraView.PropertyChanged += CameraView_PropertyChanged;
            CameraView.OnStart += StartCamera;
            CameraView.OnStop += StopCamera;

            iOSCameraView = new iOSCameraView();
            OverlayView = CreateOverlay();

            // property changed futher down is only fired when changed, not when given initial default value
            OverlayView.Alpha = (nfloat)CameraView.OverlayOpacity;
        }

        void CameraView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(CameraView.OverlayOpacity))
            {
                OverlayView.Alpha = (nfloat)CameraView.OverlayOpacity;
            }
        }

        void StartCamera()
        {
            try
            {
                iOSCameraView.Session.StartRunning();
            }
            catch(Exception exc)
            {
                var e = exc;
                var s = 12;
            }
        }

        void StopCamera()
        {
            iOSCameraView.Session.StopRunning();
        }


        UIVisualEffectView CreateOverlay()
        {
            var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.ExtraLight);

            UIVisualEffectView visualEffectView = new UIVisualEffectView(blurEffect);
            return visualEffectView;
        }

        public override void LayoutSubviews() // https://stackoverflow.com/questions/48682592/autoresize-uiview-in-xamarin-ios-viewrenderer
        {
            base.LayoutSubviews();
            iOSCameraView?.SetFrame(NativeView.Frame);
            if(Ext.HasValue(OverlayView)) // precausion, 'OnElementChanged' has not happened yet..
            {
                OverlayView.Frame = NativeView.Frame;
            }

            // adding iOSCameraView View to platform renderer view, once
            if (iOSCameraView.View.Superview != NativeView)
            {
                NativeView.Add(iOSCameraView.View);
            }
            if (OverlayView.Superview != NativeView)
            {
                NativeView.Add(OverlayView);
            }

        }
    }
    // https://stackoverflow.com/questions/51228006/remove-avcapturesession-and-previewlayer-properly-in-swift
    public class iOSCameraView : AVCaptureVideoDataOutputSampleBufferDelegate
    {
        public UIView View { get; }
        public AVCaptureSession Session { get; }
        AVCaptureVideoPreviewLayer PreviewLayer { get; }
        public iOSCameraView()
        {
            Session = new AVCaptureSession();
            //Console.WriteLine("Instantiated _CameraView, recievied uiview: " + view);
            /* There could be a forms bug here where dynamicly added
               content don't follow the safe area rule. Without explicitly setting UIView with cgrect -
               white areas that are extending the app bar and a white area at the botom of the page is created */

            /* checked in formsCameraView instead
            var hasPermission = await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVAuthorizationMediaType.Video); // crash
            Console.WriteLine("Has permission: " + hasPermission);
            */
            var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaTypes.Video);

            NSError error;
            var input = new AVCaptureDeviceInput(captureDevice, out error); // crashed here
            Session.AddInput(input);
            if (Ext.HasValue(error))
            {
                throw new Exception("Could not add camera device as input for session. NSError: " + error);
            }

            PreviewLayer = new AVCaptureVideoPreviewLayer(Session);
            PreviewLayer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;

            View = new UIView();
            View.Layer.AddSublayer(PreviewLayer);

        }
        public void SetFrame(CoreGraphics.CGRect frame)
        {
            // both has to be set, and previewlayer can't seem to be set to follow the View's frame
            View.Frame = frame;
            PreviewLayer.Frame = View.Frame;
        }

        // part of protocol AVCaptureVideoDataOutputSampleBufferDelegate
        public override void DidOutputSampleBuffer(AVCaptureOutput captureOutput, CoreMedia.CMSampleBuffer sampleBuffer, AVCaptureConnection connection)
        {
        }

        public override void DidDropSampleBuffer(AVCaptureOutput captureOutput, CoreMedia.CMSampleBuffer sampleBuffer, AVCaptureConnection connection)
        {

        }

    }
}