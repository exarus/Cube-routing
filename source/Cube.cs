using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    public sealed class Cube
    {
        private const string DELIMITER = " -> ";
		private const int EDGE_COUNT = 12;
        private static readonly Point3f center = new Point3f();
		private static readonly float[] EDGE_POSSIBLE_COORDINATES = { -1, 1 };

        private Point3f A { get; set; }
        private Point3f B { get; set; }
        private Point3f C { get; set; }
        private Point3f D { get; set; }
        private Point3f A1 { get; set; }
        private Point3f B1 { get; set; }
        private Point3f C1 { get; set; }
        private Point3f D1 { get; set; }

        public Cube() 
        {
            A =  new Point3f(0, 0, 0);
            B =  new Point3f(0, 1, 0);
            C =  new Point3f(1, 1, 0);
            D =  new Point3f(1, 0, 0);
            A1 = new Point3f(0, 0, 1);
            B1 = new Point3f(0, 1, 1);
            C1 = new Point3f(1, 1, 1);
            D1 = new Point3f(1, 0, 1);
        }

        /// <summary>
        /// Converts a 3D point to the name of the cube point.
        /// The start of the coordinates is in the point "A". 
        /// The sides are placed according to the cube.jpg.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public string getName(Point3f point)
        {
            if (point.Z == 0)
                if (point.Y == 0)
                    if (point.X == 0)
                        return "A";
                    else
                        return "D";
                else
                    if (point.X == 0)
                        return "B";
                    else
                        return "C";
            else
                if (point.Y == 0)
                    if (point.X == 0)
                        return "A₁";
                    else
                        return "D₁";
                else
                    if (point.X == 0)
                        return "B₁";
                    else
                        return "C₁";
        }

        public Point3f toPoint(string name)
        {
            switch (name)
            {
                case "A": return A;
                case "B": return B;
                case "C": return C;
                case "D": return D;
                case "A₁": return A1;
                case "B₁": return B1;
                case "C₁": return C1;
                case "D₁": return D1;
                default: throw new FormatException("Expected: A-D or A₁-D₁. Instead got: " + name + ".");
            }
        }

        public string getPath(string from, string to)
        {
            var path = new StringBuilder();
            var cursor = this.toPoint(from);
            path.Append(getName(cursor));

            var subtractPoint = this.toPoint(to).Subtract(cursor);
            if (subtractPoint.X != 0) {
                cursor.X += subtractPoint.X;
                path.Append(DELIMITER + getName(cursor));
            }
            if (subtractPoint.Y != 0)
            {
                cursor.Y += subtractPoint.Y;
                path.Append(DELIMITER + getName(cursor));
            }
            if (subtractPoint.Z != 0)
            {
                cursor.Z += subtractPoint.Z;
                path.Append(DELIMITER + getName(cursor));
            }

            return path.ToString();
        }

        public List<Point3f>[] getPath(Point3f from, Point3f to)
        {
            var path = new List<Point3f>();
            path.Add(from);

            // 1 edge case
            if (isOnOneEdge(from, to))
            {
                path.Add(to);
                return new List<Point3f>[] { path };
            }
            else if (passesCenter(new Line3D(from, to)))
            {
                
            }
            else // multi edge one-path case
            {
                var plain = new Plain3D(from, to);
                var points = getPointsWherePlaneCrossesEdges(plain);
                for (int i = 0; i < points.Length; i++)
                {
                    Console.WriteLine(points[i].X + " " + points[i].Y + " " + points[i].Z);
                }
            }

            return null;
        }
        
        private static Point3f[] getPointsWherePlaneCrossesEdges(Plain3D plain)
        {
			var list = new Point3f[EDGE_COUNT];
			var i = 0;
			foreach (var c1 in EDGE_POSSIBLE_COORDINATES) {
				foreach (var c2 in EDGE_POSSIBLE_COORDINATES) {

					// will occur 4 times

                    var x = (c1 * (plain.P3.X * plain.P2.Z - plain.P2.X * plain.P3.Z)
                        + c1 * (plain.P2.X * plain.P3.Y - plain.P3.X * plain.P2.Y))
						/ (plain.P3.Y * plain.P2.Z - plain.P2.Y * plain.P3.Z);
                    var y = (c1 * (plain.P2.Y * plain.P3.Z - plain.P3.Y * plain.P2.Z)
						+ c2 * (plain.P2.X * plain.P3.Y - plain.P3.X * plain.P2.Y))
						/ (plain.P2.X * plain.P3.Z - plain.P3.X * plain.P2.Z);
					var z = (c1 * (plain.P2.Y * plain.P3.Z - plain.P3.Y * plain.P2.Z)
						+ c2 * (plain.P2.Z * plain.P3.X - plain.P2.X * plain.P3.Z))
						/ (plain.P3.X * plain.P2.Y - plain.P2.X * plain.P3.Y);

					if (Math.Abs(x) <= 1)
						list[i++] = new Point3f(x, c1, c2);
					if (Math.Abs(y) <= 1)
						list[i++] = new Point3f(c1, y, c2);
					if (Math.Abs(z) <= 1)
						list[i++] = new Point3f(c1, c2, z);
				}
			}

			var result = new Point3f[i];
			Array.Copy(list, result, i);
			return result;
        }

        private static bool isOnOneEdge(Point3f from, Point3f to)
        {
            return Math.Abs(from.X) == 1 && from.X == to.X ||
                Math.Abs(from.Y) == 1 && from.Y == to.Y ||
                Math.Abs(from.Z) == 1 && from.Z == to.Z;
        }

        private static bool passesCenter(Line3D line)
        {
            float lineCenterX = (line.P1.X + line.P2.X) / 2;
            float lineCenterY = (line.P1.Y + line.P2.Y) / 2;
            float lineCenterZ = (line.P1.Z + line.P2.Z) / 2;
            var lineCenter = new Point3f(lineCenterX, lineCenterY, lineCenterZ);
            return center == lineCenter;
        }
    }
}
