using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    public struct Point3f
    {
        public float X;
        public float Y;
        public float Z;
        public Point3f(float x, float y, float z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point3f operator +(Point3f o1, Point3f o2)
        {
            return new Point3f(o1.X + o2.X, o1.Y + o2.Y, o1.Z + o2.Z);
        }

        public static Point3f operator -(Point3f o1, Point3f o2)
        {
            return new Point3f(o1.X - o2.X, o1.Y - o2.Y, o1.Z - o2.Z);
        }

        public Point3f Add(Point3f other)
        {
            return new Point3f(this.X + other.X, this.Y + other.Y, this.Z + other.Z);
        }

        public Point3f Add(float x, float y, float z)
        {
            return new Point3f(this.X + x, this.Y + y, this.Z + z);
        }

        public Point3f Subtract(Point3f other)
        {
            return new Point3f (this.X - other.X, this.Y - other.Y, this.Z - other.Z);
        }

        public Point3f Subtract(float x, float y, float z)
        {
            return new Point3f(this.X - x, this.Y - y, this.Z - z);
        }

        public float[] get()
        {
            return new float[] { X, Y, Z };
        }

        override public String ToString()
        {
            return "(" + X.ToString() + "; " + Y.ToString() + "; " + Z.ToString() + ")";
        }
    }
}
