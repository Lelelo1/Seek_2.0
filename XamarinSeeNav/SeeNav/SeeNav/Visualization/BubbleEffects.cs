using System;
using System.Numerics;
using LogicLibrary.Game;
using LogicLibrary.Game.Models;
using LogicLibrary.Utils;
using SeeNav.Visualization.Support;
using Xamarin.Forms;

namespace SeeNav.Visualization
{
    /*
    public class BubbleEffects
    {

        public static uint Touch(Bubble bubble)
        {
            var length = 10u;// 200u;
            var toQ = bubble.Spatial.Orientation.New(Source.Camera);
            var fromQ = bubble.Spatial.Orientation.Value;

            // visual.MoveTop((AbsoluteLayout)visual.Parent);

            Animations.Get.RegisterOnce("pathAnimation");
            bubble.Animate("pathAnimation", PathAnimation(fromQ, toQ, bubble), easing: Easing.SinIn, length: length, finished: (d, b) =>
            {
                bubble.Spatial.Orientation.Origin = Source.Camera;
                // animationCompletion.SetResult(true);
            });

            var sizeChangeLength = TouchSizeChange(bubble);

            return length > sizeChangeLength ? length : sizeChangeLength;
        }

        static uint TouchSizeChange(Bubble bubble)
        {
            var length = 10u;//200u;
            Animation enlarge = new Animation((ratio) =>
            {
                Animations.Get.Enlarge(ratio, bubble);
            }, 0, 1);
            Animations.Get.RegisterOnce(nameof(enlarge));
            bubble.Animate(nameof(enlarge), enlarge, easing: Easing.SinIn, length: length);
            return length;
        }

        public static uint Release(Bubble bubble)
        {
            var fromQ = bubble.Spatial.Orientation.Value;

            var toQ = bubble.Spatial.Orientation.New(Source.Bearing);

            // increases depending distance so that more of shrink is shown when turned away from bearing orientation. path animation takes more time the further away it is.

            //twist and angleAwayFrom has for some reason started to count from 360, but everything seems to still work ok
            var awayQ = fromQ.Difference(toQ); // could be opposite difference
            var rollAngle = Calc.ToDeg((float)Math.Abs(awayQ.Roll()));
            var pitchAngle = Calc.ToDeg((float)Math.Abs(awayQ.Pitch()));

            // awayQ should be shortest angle away from, so 40 and 320 are both 40
            if (rollAngle > 180)
            {
                rollAngle = 360 - rollAngle;
            }
            if (pitchAngle > 180)
            {
                pitchAngle = 360 - pitchAngle;
            }

            var angleAwayFrom = (rollAngle > pitchAngle ? rollAngle : pitchAngle); // 360 -

            // angleAwayFrom -= (Spatial.FOV / 2);

            Console.WriteLine("angleAwayFrom: " + angleAwayFrom);
            var percentAwayFrom = angleAwayFrom / 180;
            // Console.WriteLine("percentAwayFrom: " + percentAwayFrom);
            var offset = 300;
            var addative = 1800;
            var length = (uint)(offset + (addative * percentAwayFrom));
            Console.WriteLine("deselect path animation length: " + length);

            Animations.Get.RegisterOnce("pathAnimation");
            bubble.Animate("pathAnimation", PathAnimation(fromQ, toQ, bubble), easing: Easing.SinIn, length: length, finished: (d, b) =>
            {
                bubble.Spatial.Orientation.Origin = Source.Bearing;
                bubble.Spatial.IgnoreCollision = false; // works
                bubble.IgnoreGyro = false;
            });

            var sizeChangeLength = ReleaseSizeChange(bubble);

            return length > sizeChangeLength ? length : sizeChangeLength;
        }

        static uint ReleaseSizeChange(Bubble bubble)
        {
           
            Animation shrink = new Animation((ratio) =>
            {
                Animations.Get.Shrink(ratio, bubble);
            }, 0, 1);

            var length = 700u;

            Animations.Get.RegisterOnce(nameof(shrink));
            bubble.Animate(nameof(shrink), shrink, length: length, easing: Easing.SinIn);

            Animation shrinkCornerRadius = new Animation((ratio) =>
            {
                Animations.Get.ShrinkCornerRadius(ratio, bubble);
            }, 0, 1);
            Animations.Get.RegisterOnce(nameof(shrinkCornerRadius));
            bubble.Animate(nameof(shrinkCornerRadius), shrinkCornerRadius, length: length, easing: Easing.SinOut);

            Animation shrinkShiftContent = new Animation((ratio) =>
            {
                Animations.Get.ShrinkShiftContent(ratio, bubble);
            }, 0, 1);
            Animations.Get.RegisterOnce(nameof(shrinkShiftContent));
            bubble.Animate(nameof(shrinkShiftContent), shrinkShiftContent, length: (uint)(length * Animations.Get.Values.ShiftPortion), easing: Easing.SinIn);

            return length;
        }

        // When animating a visual between to two orienation locations
        public static Animation PathAnimation(Quaternion fromQ, Quaternion toQ, Bubble bubble)
        {

            // t -= Spatial.FOV / 2;
            // float diffRoll = Calc.ToDeg((float)(fromQ.Roll() - toQ.Roll()));
            var diffQ = fromQ.Difference(toQ); // could be opposite difference

            // check positive max, use it as distance
            float diffRoll = Calc.ToDeg((float)diffQ.Roll());
            var twist = diffRoll;
            Console.WriteLine("twist: " + twist);
            // simulate the amount device is upside by how far away the visual is. Where 180deg is upsidedown
            var upsideDown = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, Calc.ToRad(twist));
            fromQ *= upsideDown; //  sides;

            // animating the deselected visual towards the correct direction
            Animation animation = new Animation((interpolate) => // (interpolate is the percent of the nearest distance between q1 q2)
            {
                if (bubble.IsSelected) // animating to the updated center of device when selected visual -> enlarging
                {
                    toQ = bubble.Spatial.Orientation.New(Source.Camera); // SelectedProjection.CameraOrientation;
                }

                var to = Quaternion.Slerp(fromQ, toQ, (float)interpolate);
                bubble.Spatial.Orientation.Set(to);

            }, 0, 1);
            return animation;

        }
        
    }
    */
}
