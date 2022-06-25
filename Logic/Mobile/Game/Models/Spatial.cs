using System;
using System.Numerics;
using Logic.Models;

namespace Logic.Game.Models
{
    public class Spatial
    {
        public Orientation Orientation { get; set; }
        public Rectangle Rectangle { get; set; }

        public bool IsBehindPlane => Vector3.Dot(-CameraDirection, Direction) > 0;
        Vector3 CameraDirection => Vector3.Transform(Vector3.UnitZ, Projector.CameraOrientation);

        public Vector3 Direction => Vector3.Transform(Vector3.UnitZ, Orientation.Value);


        //public Spatial Spatial { get; set; }
        public bool IgnoreCollision { get; set; }

        public Spatial(Location userLocation, Location placeLocation, Size size, bool ignoreCollision)
        {
            Orientation = new Orientation(userLocation, placeLocation);
            Rectangle = new Rectangle(0, 0, size.Width, size.Height);
            IgnoreCollision = ignoreCollision;

        }

        public Quaternion GetRelativeOrientation()
        {
            return  Orientation.Value / Projector.CameraOrientation;
        }

        public bool GetIsSeen(Rectangle cameraView)
        {
            if(IsBehindPlane)
            {
                return false;
            }
            return Rectangle.IsInside(cameraView);
        }
    }
}
