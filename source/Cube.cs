using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    public sealed class Cube
    {
        private const double ACCEPTABLE_DELTA = 0.1;
		private const int EDGE_COUNT = 12;
        private const int MAX_PATH_SIZE = 4;
        private static readonly Point3f center = new Point3f();
		private static readonly float[] EDGE_POSSIBLE_COORDINATES = { -1, 1 };

        public List<List<Point3f>> getPath(Point3f from, Point3f to)
        {
            // 1 edge case
            if (isOnOneEdge(from, to))
            {
                var lists = new List<List<Point3f>>();
                var list = new List<Point3f>(new Point3f[] {from, to});
                lists.Add(list);
                return lists;
            }
            else if (passesCenter(new Line3D(from, to)))
            {
                // TODO the case has my dick on it
                return null;
            }
            else // multi edge
            {
                var plain = new Plain3D(from, to);
                var points = getPointsWherePlaneCrossesEdges(plain);
                return calculateBestPath(from, points, to);
            }
        }

        private static List<List<Point3f>> calculateBestPath(Point3f from, Point3f[] through, Point3f to)
        {
            var lists = new List<List<Point3f>>();
            Console.WriteLine("from" + from);
            for (int i = 0; i < through.Length; i++)
            {
                Console.WriteLine(through[i]);
            }
            Console.WriteLine("to: " + to);

            foreach (var p1 in through)
            {
                if (isOnOneEdge(from, p1))
                {
                    var newList = new List<Point3f>(MAX_PATH_SIZE);
                    newList.Add(from);
                    newList.Add(p1);

                    // three edge case
                    if (!isOnOneEdge(p1, to)) 
                    {
                        foreach (var p2 in through)
                        {
                            if (p1 != p2 && isOnOneEdge(p1, p2) && isOnOneEdge(p2, to))
                            {
                                newList.Add(p2);
                                break;
                            }
                        }
                    }

                    newList.Add(to);
                    lists.Add(newList);
                }
            }

            /*
            float length = 0;
            foreach (var list in lists)
            {
                var p1 = list[0];
                for (int i = 0; i < list.Count; i++)
                {
                    var
                }
            }*/

            return lists;
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
