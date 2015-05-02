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
            else if (new Line3D(from, to).passesZero())
            {
                // TODO the case has my dick on it
                return null;
            }
            else // multi edge
            {
                var plain = new Plain3D(from, to);
                var points = getPointsWherePlaneCrossesEdges(plain);
                return calculateBestPaths(from, points, to);
            }
        }

        private static List<List<Point3f>> calculateBestPaths(Point3f from, Point3f[] through, Point3f to)
        {
            var paths = new List<List<Point3f>>();

            foreach (var p1 in through)
            {
                if (isOnOneEdge(from, p1))
                {

                    // three edge case
                    if (isOnOneEdge(p1, to)) 
                    {
                        var newList = new List<Point3f>(MAX_PATH_SIZE);
                        newList.Add(from);
                        newList.Add(p1);
                        newList.Add(to);
                        paths.Add(newList);
                    }
                    else
                    {
                        foreach (var p2 in through)
                        {
                            if (p1 != p2 && isOnOneEdge(p1, p2) && isOnOneEdge(p2, to))
                            {
                                var newList = new List<Point3f>(MAX_PATH_SIZE);
                                newList.Add(from);
                                newList.Add(p1);
                                newList.Add(p2);
                                newList.Add(to);
                                paths.Add(newList);
                                break;
                            }
                        }
                    }
                }
            }

            filterLongPaths(paths);            
            return paths;
        }

        private static void filterLongPaths(List<List<Point3f>> paths)
        {
            double minLength = Double.MaxValue;
            for (int i = 0; i < paths.Count; i++)
            {
                double length = 0;
                for (int j = 0; j < paths[i].Count - 1; j++)
                {
                    length += new Line3D(paths[i][j], paths[i][j + 1]).getLength();
                }
                if (length - minLength <= ACCEPTABLE_DELTA)
                {
                    minLength = length; //TODO not the smartest way (but fastest) to reasign minlength
                }
                else
                {
                    paths.RemoveAt(i);
                    --i;
                }
            }
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
    }
}
