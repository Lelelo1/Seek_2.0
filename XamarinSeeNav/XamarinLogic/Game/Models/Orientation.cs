using System;
using System.Numerics;
using XamarinLogic.Models;
using XamarinLogic.Services.PermissionRequired;
using XamarinLogic.ViewModels;

namespace XamarinLogic.Game.Models
{
    public class Orientation
    {
        double Bearing { get; set; }
        public Quaternion Value
        {
            get
            {
                /* might not need this setup below */

                // during animation when select/deselect
                if (Special.HasValue)
                {
                    return Special.Value;
                }
                if (Origin == Source.Camera)
                {
                    return Projector.CameraOrientation;
                }
                return Offset.HasValue ? Bearing.AsOrientation() * Offset.Value : Bearing.AsOrientation();
            }
        }

        Source _origin;
        public Source Origin { get => _origin; set { _origin = value; Special = null; } }

        // for special occasion such has when animating deselect select
        public Quaternion? Special { get; private set; }
        public void Set(Quaternion q)
        {
            Special = q;
        }

        public Quaternion? Offset { get; set; }

        // a new orientationbearing
        public Quaternion New(Source from)
        {
            if (from == Source.Bearing)
            {
                return Offset.HasValue ? Bearing.AsOrientation() * Offset.Value : Bearing.AsOrientation(); // or remove offset here?
            }
            else if (from == Source.Camera)
            {
                return Projector.CameraOrientation;
            }

            throw new NotSupportedException("unknown source added to New method in Orientation.cs");
        }

        public Orientation(Location userLocation, Location placeLocation)
        {
            Set(userLocation, placeLocation);
        }
        void Set(Location userLocation, Location placeLocation)
        {
            Bearing = Logic.DependencyBox.Get<MainViewModel>().GetBearing(userLocation, placeLocation);

            Logic.DependencyBox.Get<LocationService>().Location.Subscribe((location, _) =>
            {
                Bearing = Logic.DependencyBox.Get<MainViewModel>().GetBearing(location, placeLocation);
            });
        }
    }
    public enum Source
    {
        Bearing,
        Camera
    }
}
