using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L4
{
    struct Line3D
    {
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
    }
}
