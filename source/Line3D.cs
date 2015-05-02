using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L4
{
    struct Line3D
    {
        private static readonly Point3f ZERO = new Point3f();
        public readonly Point3f P1;
        public readonly Point3f P2;

        /// <summary>
        /// P1 is always (0,0,0)
        /// </summary>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Line3D(Point3f p1, Point3f p2)
        {
            this.P1 = p1;
            this.P2 = p2;
        }

        public double getLength()
        {
            var dX = P2.X - P1.X;
            var dY = P2.Y - P1.Y;
            var dZ = P2.Z - P1.Z;
            return Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
        }

        public bool passesZero()
        {
            float lineCenterX = (P1.X + P2.X) / 2;
            float lineCenterY = (P1.Y + P2.Y) / 2;
            float lineCenterZ = (P1.Z + P2.Z) / 2;
            var lineCenter = new Point3f(lineCenterX, lineCenterY, lineCenterZ);
            return ZERO == lineCenter;
        }
    }
}
