using System;
using System.Numerics;
using System.Threading.Tasks;
using XamarinLogic.Game.Models;
using XamarinLogic.Models;
using XamarinLogic.Native;
using XamarinLogic.Utils;
using XamarinLogic.ViewModels;


namespace XamarinLogic.Game
{
    public class Projector : IBase
    {

        static TaskCompletionSource<IBase> Initializing { get; } = new TaskCompletionSource<IBase>();

        public static Task<IBase> Init(ProjectorConfig projectorConfig)
        {
            var angleOfView = N.Get<IAngleOfView>();

            Initializing.SetResult(new Projector(angleOfView, projectorConfig));

            return Initializing.Task;
        }

        protected Projector(IAngleOfView angleOfView, ProjectorConfig projectorConfig)
        {
            ProjectorConfig = projectorConfig;
            ProjectionMatrix = CreateProjectionMatrix(angleOfView);
        }


        Matrix4x4 CreateProjectionMatrix(IAngleOfView angleOfView)
        {
            var fovRads = (float)(Calc.ToRad(angleOfView.Value.Vertical));
            return Matrix4x4.CreatePerspectiveFieldOfView(fovRads, (float)ProjectorConfig.ViewSize.AspectRatio(), 0.1f, 1000f);
        }

        Matrix4x4 ProjectionMatrix { get; }

        public Quaternion DeviceOrientation { get; set;}

        public ProjectorConfig ProjectorConfig { get; }

        public Projection Project(Spatial spatial, ProjectorConfig projectorConfig)
        {
            var view = projectorConfig.ViewSize;

            var spatialDirection = Vector3.Transform(Vector3.UnitZ, spatial.Orientation.Value);

            var spatialDirectionRelativeCamera = Vector3.Transform(spatialDirection, Quaternion.Inverse(CameraOrientation));

            var position = Vector3.Transform(-spatialDirectionRelativeCamera, ProjectionMatrix);

            var pos = PositionInView(To2D(position),
                spatial.Rectangle.Width, spatial.Rectangle.Height, view);
            var size = spatial.Rectangle;

            var projectedRect = new Rectangle(pos.X, pos.Y, size.Width, size.Height);

            return new Projection(projectedRect);
        }

        Vector2 Center { get; set; } = new Vector2(0.5f, 0.5f);

        Vector2 To2D(Vector3 position)
        {
            /*
            position.Y = -position.Y;

            return Center += Center * new Vector2(position.X, position.Y);
            */

            var tmpPosition = position; // added 

                float tmpWidth = 0.5f;
                float tmpHeight = 0.5f;

                Vector2 tmp2DPos = Vector2.Zero;
                tmp2DPos.X = tmpWidth + (tmpWidth * tmpPosition.X);
                // basically anchors the objects along the y axis. Tthey othwerwse go off screen mo immedtly when up down device
                tmp2DPos.Y = tmpHeight + (tmpHeight * -tmpPosition.Y);
            return tmp2DPos;// + _position2D;
        }

        Vector2 PositionInView(Vector2 position, double width, double height, Size view) 
        {
            var viewX = (view.Width * position.X) - (width * 0.5);
            var viewY = (view.Height * position.Y) - (height * 0.5);
            var viewPoint = new Vector2((float)viewX, (float)viewY);
            return viewPoint;
        }

        static Quaternion GetDeviceOrientation()
        {
            var main = Logic.DependencyBox.Get<MainViewModel>();
            var mobileAttitude = main.Orientation.Value;

            return mobileAttitude;
        }

        
        public static Quaternion CameraOrientation // madwick quaternion w, x, y, z
        {
            get
            {
                // prev has checks, always assume mobile now


                //var deviceOrientation = GetDeviceOrientationForProjection(mobileAttitude);

                // old
                //DeviceOrientation = new Quaternion(-o.Y, o.Z, -o.X, o.W); // correct axises fitting cpp game
                // https://forums.ogre3d.org/viewtopic.php?t=32251


                //var cameraOrientation = deviceOrientation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, Calc.ToRad(-90.0f));
                return Quaternion.CreateFromAxisAngle(Vector3.UnitX, Calc.ToRad(-90.0f)) * GetDeviceOrientation();//cameraOrientation;
            }
        }
        
        /* place out the visual explicitly to the noth instead - that way it shoul be possibly to see exactly what type of Q
           the projection needs!
         */
        static Quaternion GetDeviceOrientationForProjection(Quaternion mobileAttitude) // mobile coordinate axis rotation system
        {
            // this is the transformation to fit common 3d game devlopment axies, and i'ts rotation
            return new Quaternion(mobileAttitude.X, mobileAttitude.Z, -mobileAttitude.Y, mobileAttitude.W);
        }
    }
    
}


// with corrected axises madgwick:
//  pitch: -y
//  roll: x
//  yaw:  z
