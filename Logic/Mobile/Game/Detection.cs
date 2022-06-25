using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using LogicLibrary.Utils;
using LogicLibrary.Native;
using LogicLibrary.Game.Models;

namespace LogicLibrary.Game
{

    // south perceptibles don't seperate on the height for some reason only in width // <-- is still valid?
    public class Detection
    {

        static Detection instance = null;
        public static Detection Get()
        {
            if (instance == null)
            {
                instance = new Detection();
            }
            return instance;
        }

        public static IAngleOfView AngleOfView { get; set; }

        public bool HasStarted { get; set; }
        EventRegistration EventRegistration { get; set; }
        public void Start(EventRegistration eventRegistration)
        {
            if(AngleOfView == null)
            {
                throw new Exception("Detection was not initiated with a AngleOfView before calling start");

            }

            EventRegistration = eventRegistration;
            _ = EventRegistration.PeriodicTask.Run(() =>
            {
                Update();
            });
            HasStarted = true;
        }
        public void Stop()
        {
            EventRegistration.PeriodicTask.CancellationTokenSource.Cancel();
            HasStarted = false;
        }

        public AngleSize AngleSize { get; set; }
        void HandleAngleSize()
        {
            if(!TrackPerceptibles.Any())
            {
                return;
            }

            if(AngleSize == null)
            {
                var rectangle = TrackPerceptibles.First().Rectangle;
                AngleSize = new AngleSize(rectangle);
            }

            if(AngleSize != null)
            {
                AngleSize.Value = 1f;
            }

        }
        List<Spatial> _trackPerceptibles = new List<Spatial>();
        public List<Spatial> TrackPerceptibles
        {
            get => _trackPerceptibles;
            set { _trackPerceptibles.Clear(); _trackPerceptibles = value; HandleAngleSize(); }
            // if not cleared first. the visuals being moved interferes with the new visuals
        }
        // probably have to count each times p1 has collieded with other uniqe visuals.
        void Update()
        {
            var objects = TrackPerceptibles;

            // https://gamedev.stackexchange.com/questions/72458/c-separating-overlapping-rectangles
            for (int i = 0; i < objects.Count; i ++)
            {
                for(int j = i + 1; j < objects.Count; j ++)
                {
                    bool shouldIgnoreCollision = objects[i].IgnoreCollision || objects[j].IgnoreCollision;
                    if(!shouldIgnoreCollision)
                    {
                        Apart(objects[i], objects[j]);
                    }
                }

            }
            // is below better or wuicker in any way?
            /*
            foreach(var p in TrackPerceptibles)
            {
                var ordered = TrackPerceptibles.Where((any) => any != p).OrderBy((any) => Math.Abs(CompareMax(p.Orientation, any.Orientation)));
                var nearest = ordered.First();
                Apart(p, nearest);
            }
            */

        }
        //

        /* Note that entering text causing new visuals to apear does nor clear the cold ones */
        void Apart(Spatial p1, Spatial p2)
        {

            var q1 = p1.Orientation.Value;
            var q2 = p2.Orientation.Value;

            var currentAngle = Math.Abs(Distance(q1, q2));

            var angleSize = AngleSize.Value;
            if (currentAngle < angleSize)
            {

                var angle = (angleSize) - currentAngle;//1f;//0.2f;
                if (angle == 0)
                {
                    angle = 0.000001f;
                }

                angle = Calc.ToRad(angle);
                var portion = new Portion(angle);
                // var portion2 = new Portion(angle / 2);
                var q1Change = Quaternion.CreateFromAxisAngle(Vector3.UnitX, portion.A) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, portion.B);
                Set(p1, q1Change);
                var q2Change = Quaternion.CreateFromAxisAngle(Vector3.UnitX, -portion.A) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, -portion.B);
                Set(p2, q2Change);
                // Log.Message("distance became: " + Distance(p1.Orientation.Value, p2.Orientation.Value));
            }

        }
        void Set(Spatial p, Quaternion change)
        {
            if(!p.Orientation.Offset.HasValue)
            {
                p.Orientation.Offset = change;
            }
            else
            {
                p.Orientation.Offset *= change;
            }
        }
        // https://stackoverflow.com/questions/22157435/difference-between-the-two-quaternions
        float Distance(Quaternion q1, Quaternion q2)
        {
            var qd = Quaternion.Inverse(q1) * q2;
            var ang1 = Angle(qd);
            var ang2 = 360 - Angle(qd);
            var angle = Math.Min(ang1, ang2);

            return angle;
        }
        float Angle(Quaternion q)
        {
            var radAngle = 2 * Math.Atan2(new Vector3(q.X, q.Y, q.Z).Length(), q.W);
            return Calc.ToDeg((float)radAngle);
        }
    }
    // Dividing up the Aparts solves percitbled ending up far away, and mainatins clusters when being multiple - most of the time...
    public class AngleSize
    {
        /* could be the collided rects size / 2 for each given collision. (If at any point decide to have different sizes on IPerceptblies)
            It is now fixed corresponding to the first given instantiated with rentangle */
        float _max;// = 8f;
        float _increase = 0.5f;
        float _value = 0.01f;
        public float Value
        {
            get
            {
                bool shouldIncrease = _value < _max * 1.75f;//0.75;
                if(shouldIncrease)
                {
                    _value += _increase;
                }
                return _value;
            }
            set => _value = value;
        }
        // calcuting actual angle size from
        public AngleSize(Rectangle rectangle)
        {
            var fovs = Detection.AngleOfView.Value;
            // is mobile
            var view = Logic.DependencyBox.Get<Projector>().ProjectorConfig.ViewSize;
            if(view == null)
            {
                throw new Exception("Exception view was not ready in AngleSize, Detection.cs when taken from Detection.cs");
            }
            var p = rectangle;
            var x = p.Width / view.Width;
            var y = p.Height / view.Height;

            var fromSmallest = Math.Min(fovs.Horizontal * x, fovs.Vertical * y);
            _max = (float)fromSmallest;
        }
    }

    public class Portion
    {
        public float A { get; }
        public float B { get; }
        public Portion(double angle)
        {
            var makePortions = new Random().NextDouble();
            A = (float)(angle * makePortions);
            B = (float)(angle * (1 - makePortions));
        }
    }
}