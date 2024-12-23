using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        //check edditing -> mohamed
        //check editinng ->kero
        //check editing -> kero
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count < 3)
            {
                // If fewer than 3 points, all points are part of the convex hull.
                outPoints = new List<Point>(points);
                return;
            }

            // Remove duplicate points
            points = points.Distinct().ToList();

            // Start with the leftmost point
            Point start = points.OrderBy(p => p.X).ThenBy(p => p.Y).First();
            Point current = start;

            do
            {
                // Add the current point 
                outPoints.Add(current);

                // Select the next point  (initialize with any point different from current)
                Point next = points[0];
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i] == current) continue;

                    // Determine turn type between current, next, and points[i]
                    Enums.TurnType turn = HelperMethods.CheckTurn(new Line(current, next), points[i]);

                    if (next == current || turn == Enums.TurnType.Right || (turn == Enums.TurnType.Colinear &&
                           Distance(current, points[i]) > Distance(current, next)))
                    {
                        // Update next point if:
                        // 1. The turn is to the right.
                        // 2. Colinear and points[i] is farther from current than next.
                        next = points[i];
                    }
                }

                // Move to the next point
                current = next;

            } while (current != start);

        }
        public static double Distance(Point p1, Point p2)  //mo editing
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        public static double DotProduct(Point a, Point b) //mo add
        {
            return a.X * b.X + a.Y * b.Y;
        }
        public static double GetAngle(Line a, Line b)  //kero fix
        {
            //Point a1 = GetVector(a);
            //Point b1 = GetVector(b);
            //double angle = Math.Atan2(b1.Y - a1.Y, b1.X - a1.X);
            Point vector_1 = new Point(a.End.X - a.Start.X, a.End.Y - a.Start.Y);
            Point vector_2 = new Point(b.End.X - b.Start.X, b.End.Y - b.Start.Y);
            double cross = HelperMethods.CrossProduct(vector_1, vector_2);
            double dot = DotProduct(vector_1, vector_2);
            double angle = Math.Atan2(cross, dot);
            if (angle < 0) angle += Math.PI;
            return angle;
        }
        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}