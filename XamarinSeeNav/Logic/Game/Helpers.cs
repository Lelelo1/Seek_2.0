using System;
using System.Numerics;
using LogicLibrary.Game.Models;
using LogicLibrary.Models;
using LogicLibrary.Utils;

namespace LogicLibrary.Game
{
    /*
    // cpp ogre rws rectangle
    public class FloatRectangle
    {
        public float Left { get; set; }
        public float Right { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public FloatRectangle(float x, float y, float z, float w)
        {
            Left = x;
            Top = y;
            Right = z;
            Bottom = w;
        }
    }
    */
    /*
    public class Rect
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Rect(double x, double y, double width, double height)
        {
            X = x;
            Y = y; 
            Width = width;
            Height = height;
        }
        public Rect()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }
        public void Set(Vector2 vector)
        {
            X = vector.X;
            Y = vector.Y;
        }
        public Rectangle AsFormsRect()
        {
            return new Rectangle(X, Y, Width, Height);
        }
        public void Sum(Rect add)
        {
            X += add.X;
            Y += add.Y;
            Width += add.Width;
            Height += add.Height;
        }

    }
    */
    public static class Extras
    {
        public static float AspectRatio(this Size size)
        {
            return size.Width / size.Height;
        }
        public static Quaternion AsOrientation(this double bearing)
        {
            var b = 360 - bearing; // so that 90 bearing is to the right when facing north
            return Quaternion.CreateFromAxisAngle(Vector3.UnitY, Calc.ToRad((float)b));
        }

        public static double Roll(this Quaternion q)
        {
            q = Quaternion.Normalize(new Quaternion(0, q.Y, 0, q.W));
            return (float)(2 * Math.Acos(q.W));
        }
        public static double Pitch(this Quaternion q)
        {
            q = Quaternion.Normalize(new Quaternion(q.X, 0, 0, q.W));
            return (float)(2 * Math.Acos(q.W));
        }
        public static double Yaw(this Quaternion q)
        {
            q = Quaternion.Normalize(new Quaternion(0, 0, q.Z, q.W));
            return (float)(2 * Math.Acos(q.W));
        }

        public static string ExtractedPrint(this Quaternion q)
        {
            return nameof(q) + " extracted roll: " + Calc.ToDeg((float)q.Roll()) + ", pitch: " + Calc.ToDeg((float)q.Pitch()) + ", yaw: " + Calc.ToDeg((float)q.Yaw());
        }

        public static double EulerRoll(this Quaternion q)
        {
            var roll = Math.Atan2(2.0 * (q.X * q.Y + q.W * q.Z), q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z);
            return Calc.ToDeg((float)roll);
        }
        public static double EulerPitch(this Quaternion q)
        {
            var pitch = Math.Asin(-2.0 * (q.X * q.Z - q.W * q.Y));
            return Calc.ToDeg((float)pitch);
        }
        public static double EulerYaw(this Quaternion q)
        {
            var yaw = Math.Atan2(2.0 * (q.Y * q.Z + q.W * q.X), q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z);
            return Calc.ToDeg((float)yaw);
        }

        public static string EulerPrint(this Quaternion q, string varName)
        {
            return varName + " euler roll: " + q.EulerRoll() + ", pitch: " + q.EulerPitch() + ", yaw: " + q.EulerYaw();
        }

        // https://stackoverflow.com/questions/20798056/magnitude-of-rotation-between-two-quaternions
        public static float Rotation(this Quaternion q)
        {
            //q = Quaternion.Normalize(q); // else NaN, but should maybe be commented out
            var rotationRads = 2 * Math.Atan2(q.Length(), q.W);
            var rotation = Calc.Normalize(Calc.ToDeg((float)rotationRads));
            return rotation; // is 90 instead of 0 when working with IPerceptibles due to perceptibles initial offset
        }
        /* Produces to large values to be credible. Atan2 return rads. Don't know 100% above Rotation neither works*/
        // also https://stackoverflow.com/questions/20798056/magnitude-of-rotation-between-two-quaternions
        public static float MagnitudeTo(this Quaternion q1, Quaternion q2) // q1 start , q2 end
        {
            var qRot = Quaternion.Normalize(q2) * Quaternion.Inverse(Quaternion.Normalize(q1));
            var angle = 2 * Math.Atan2(qRot.Length(), qRot.W);
            // Console.WriteLine("angle: " + angle);
            return Calc.Normalize(Calc.ToDeg((float)(angle)));
        }

        public static Quaternion Difference(this Quaternion q1, Quaternion q2)
        {
            var diffQ = q1 * Quaternion.Inverse(q2); // // the actual difference: https://stackoverflow.com/questions/22157435/difference-between-the-two-quaternions
            return diffQ;
        }

        /*
         new instances so that multiple operations does not effect the same instance if it where not intended.
         It somehow has to to do with quatnerion being of struct type.
        */
        public static Quaternion New(this Quaternion q) 
        {
            return new Quaternion(q.X, q.Y, q.Z, q.W);
        }

        public static string Print(this Quaternion q)
        {
            return "x: " + q.X + ", y: " + q.Y + ", z: " + q.Z + ", w: " + q.W;
        }

        /* with MadgiwckAhrs.cs euler forumulas */
        /*
        Roll = Math.Atan2(2 * (y * z + x * w), -xx - yy + zz + ww);
            Pitch = -Math.Asin(2 * (x * z - y * w));
        Heading = Math.Atan2(2 * (x * y + z * w), xx - yy - zz + ww);
        */

        public static double Heading(this Quaternion q)
        {
            return Calc.Normalize(Calc.ToDeg(Math.Atan2(2 * (q.X * q.Y + q.Z * q.W),
                q.X * q.X - q.Y * q.Y - q.Z * q.Z + q.W * q.W)));
        }
        public static double TransitionRoll(this Quaternion q)
        {
            return Calc.ToDeg((float)Math.Atan2(2 * (q.Y * q.Z + q.X * q.W), -q.X * q.X - q.Y * q.Y + q.Z * q.Z + q.W * q.W));
        }
        public static double TransitionPitch(this Quaternion q)
        {
            return Calc.ToDeg((float)-Math.Asin(2 * (q.X * q.Z - q.Y * q.W)));
        }

        // prevously in project.cs
        public static Vector3 ZAxis(this Quaternion self)
        {
            float fTx = 2.0f * self.X;
            float fTy = 2.0f * self.Y;
            float fTz = 2.0f * self.Z;
            float fTwx = fTx * self.W;
            float fTwy = fTy * self.W;
            float fTxx = fTx * self.X;
            float fTxz = fTz * self.X;
            float fTyy = fTy * self.Y;
            float fTyz = fTz * self.Y;
            
            return new Vector3(fTxz + fTwy, fTyz - fTwx, 1.0f - (fTxx + fTyy));
        }

        // https://stackoverflow.com/questions/1171849/finding-quaternion-representing-the-rotation-from-one-vector-to-another
        public static Quaternion QuaternionTo(this Vector3 v, Vector3 to)
        {
            var v1 = v; var v2 = to;
            var a = Vector3.Cross(v1, v2);

            var w = (float)Math.Sqrt(Math.Pow(v1.Length(), 2) * Math.Pow(v2.Length(), 2)) + Vector3.Dot(v1, v2);

            return Quaternion.Normalize(new Quaternion(a, w));
        }

        public static bool IsInside(this Rectangle rectangle, Rectangle container) // tested ok
        {
            var rectanglePosX = rectangle.X;
            var rectanglePosY = rectangle.Y;

            var xInside = rectanglePosX >= -rectangle.Width && rectanglePosX <= container.Width;
            var yInside = rectanglePosY >= -rectangle.Height && rectanglePosY <= container.Height;

            return xInside && yInside;
        }

        public static bool IsNear(this Quaternion q1, Quaternion q2, float dot = 0.01f)
        {
            return Quaternion.Dot(q1, q2) < dot;
        }
    }


    /*NOT NEEDED use: ? infront of type!
    // structs can't be set as an optional parameter in C#, wrapper for Vector2, Vector3 etc
    public class Nullable<T> where T : struct
    {
        T anystruct;
        public T Get()
        {
            return anystruct;
        }
        public Nullable(T anystruct)
        {
            this.anystruct = anystruct;
        }
    }
    */


    /* System.numerics is working fine
        static void FromAngleAxis(ref Quaternion self, float rfAngle, Vector3 rkAxis)
        {
            // assert:  axis[] is unit length
            //
            // The quaternion representing the rotation is
            //   q = cos(A/2)+sin(A/2)*(x*i+y*j+z*k)

            float fHalfAngle = 0.5f * rfAngle;
            float fSin = (float)Math.Sin(Calc.Deg2Rad(fHalfAngle));
            self.W = (float)Math.Cos(Calc.Deg2Rad(fHalfAngle));
            self.X = fSin * rkAxis.X;
            self.Y = fSin * rkAxis.Y;
            self.Z = fSin * rkAxis.Z;
        }
        */


    /* old cpp ogre code not need - prevously in asd later called projection.cs file*/
    /*
    static Vector3 xAxis(Quaternion self)
    {
        //float fTx  = 2.0*x;
        float fTy = 2.0f * self.Y;
        float fTz = 2.0f * self.Z;
        float fTwy = fTy * self.W;
        float fTwz = fTz * self.W;
        float fTxy = fTy * self.X;
        float fTxz = fTz * self.X;
        float fTyy = fTy * self.Y;
        float fTzz = fTz * self.Z;

        return new Vector3(1.0f - (fTyy + fTzz), fTxy + fTwz, fTxz - fTwy);
    }

    static Vector3 yAxis(Quaternion self)
    {
        float fTx = 2.0f * self.X;
        float fTy = 2.0f * self.Y;
        float fTz = 2.0f * self.Z;
        float fTwx = fTx * self.W;
        float fTwz = fTz * self.W;
        float fTxx = fTx * self.X;
        float fTxy = fTy * self.X;
        float fTyz = fTz * self.Y;
        float fTzz = fTz * self.Z;

        return new Vector3(fTxy - fTwz, 1.0f - (fTxx + fTzz), fTyz + fTwx);
    }
    */
}
