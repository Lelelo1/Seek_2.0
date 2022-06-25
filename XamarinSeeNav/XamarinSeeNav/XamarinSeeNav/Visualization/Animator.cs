using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using LogicLibrary.Game;
using LogicLibrary.Game.Models;
using LogicLibrary.Utils;

namespace Seek.Visualization
{
    public class Animator
    {
        Bubble Bubble { get; }

        public bool IsAnimating { get; set; } = true;

        public Animator(Bubble bubble)
        {
            Bubble = bubble;
        }

        static TimeSpan SensorTime { get; } = TimeSpan.FromMilliseconds(80);


        float CurrentInterpolation { get; set; }
        CancellationTokenSource Cancellation { get; set; }
        public Task Start(Quaternion from, Quaternion to, float interpolationChange)
        {
            CurrentInterpolation = 0;

            PeriodicTask animator = new PeriodicTask(SensorTime);
            Cancellation = animator.CancellationTokenSource;

            from = MakeAnimateAllDirections(from, to);

            // (y is bearing)

            return animator.Run(() =>
            {
                Bubble.Spatial.Orientation.Set(Quaternion.Slerp(from, to, CurrentInterpolation));
                CurrentInterpolation += interpolationChange;

                if (CurrentInterpolation >= 1f)
                {
                    Cancellation.Cancel();
                }
            });
        }

        // animating bubbles to opposite bearing won't show, and bubbles can't animate more than sidewayswithout this below modifcation
        Quaternion MakeAnimateAllDirections(Quaternion from, Quaternion to)
        {
            // t -= Spatial.FOV / 2;
            // float diffRoll = Calc.ToDeg((float)(fromQ.Roll() - toQ.Roll()));
            var diffQ = from.Difference(to); // could be opposite difference

            // check positive max, use it as distance
            float diffRoll = Calc.ToDeg((float)diffQ.Roll());
            var twist = diffRoll;
            Console.WriteLine("twist: " + twist);
            // simulate the amount device is upside by how far away the visual is. Where 180deg is upsidedown
            var upsideDown = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, Calc.ToRad(twist));
            from *= upsideDown; //  sides;

            return from;
        }
    }
}
