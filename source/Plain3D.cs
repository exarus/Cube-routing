using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    public struct Plain3D
    {
        public readonly Point3f P1;
        public readonly Point3f P2;
        public readonly Point3f P3;

        /// <summary>
        /// P1 is always (0,0,0)
        /// </summary>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Plain3D(Point3f p2, Point3f p3)
        {
            this.P1 = new Point3f(0, 0, 0);
            this.P2 = p2;
            this.P3 = p3;
        }
    }
}
