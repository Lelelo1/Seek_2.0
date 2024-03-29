﻿using System;
using System.Numerics;
using System.Threading.Tasks;
using LogicLibrary.Game.Models;
using LogicLibrary.Models;
using LogicLibrary.Utils;
using LogicLibrary.ViewModels;


namespace LogicLibrary.Game
{
    public class Projector : IBase
    {

        static TaskCompletionSource<IBase> Initializing { get; } = new TaskCompletionSource<IBase>();

        public static Task<IBase> Init()
        {

            Initializing.SetResult(new Projector());

            return Initializing.Task;
        }

        protected Projector()
        {
            ProjectionMatrix = CreateProjectionMatrix();
        }


        Matrix4x4 CreateProjectionMatrix()
        {
            var fovRads = Calc.ToRad(Logic.FrameworkContext.ProjectionAngle.Vertical);
            var projectionArea = Logic.FrameworkContext.ProjectionArea;
            return Matrix4x4.CreatePerspectiveFieldOfView(fovRads, projectionArea.AspectRatio(), 0.1f, 1000f);
        }

        Matrix4x4 ProjectionMatrix { get; }

        public Quaternion DeviceOrientation { get; set;}


        public Projection Project(Spatial spatial)
        {
            var view = Logic.FrameworkContext.ProjectionArea;

            var spatialDirection = Vector3.Transform(Vector3.UnitZ, spatial.Orientation.Value);

            var spatialDirectionRelativeCamera = Vector3.Transform(spatialDirection, Quaternion.Inverse(CameraOrientation));

            var position = Vector3.Transform(-spatialDirectionRelativeCamera, ProjectionMatrix);

            var pos = PositionInProjectionArea(To2D(position),
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

        Vector2 PositionInProjectionArea(Vector2 position, double width, double height, Size view) 
        {
            var viewX = (view.Width * position.X) - (width * 0.5);
            var viewY = (view.Height * position.Y) - (height * 0.5);
            var viewPoint = new Vector2((float)viewX, (float)viewY);
            return viewPoint;
        }

        static Quaternion GetDeviceOrientation()
        {
            return Logic.DependencyBox.Get<MainViewModel>().Orientation.Value;
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
