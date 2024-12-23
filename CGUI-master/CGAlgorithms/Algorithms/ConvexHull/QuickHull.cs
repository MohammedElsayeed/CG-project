using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    /*
     check editting -> yousef
    check edit -> kero
    check edit ->kero  distance function
    check edit ->mo
     
     
     */
    public class QuickHull : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outputPoints, ref List<Line> outputLines, ref List<Polygon> outputPolygons)
        {
            // Find the leftmost and rightmost points
            Point leftmostPoint = new Point(100000, 0);
            Point rightmostPoint = new Point(-100000, 0);
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < leftmostPoint.X)
                    leftmostPoint = points[i];
                if (points[i].X > rightmostPoint.X)
                    rightmostPoint = points[i];
            }

            // Compute the convex hull for both directions (Right and Left)
            List<Point> rightHull = quickHull(points, leftmostPoint, rightmostPoint, "Right");
            List<Point> leftHull = quickHull(points, leftmostPoint, rightmostPoint, "Left");

            // Merge both hulls
            for (int i = 0; i < leftHull.Count; ++i)
                rightHull.Add(leftHull[i]);

            // Add the points of the hull to the output list, //ensuring no duplicates
            for (int i = 0; i < rightHull.Count; i++)
            {
                if (!outputPoints.Contains(rightHull[i]))
                    outputPoints.Add(rightHull[i]);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }

        // calculate the distance of a point from the line formed by two points (a, b)
        public double dist(Point p1, Point p2, Point p3)
        {
            // Returns the area 
            return Math.Abs((p3.Y - p1.Y) * (p2.X - p1.X) -
                       (p2.Y - p1.Y) * (p3.X - p1.X));
        }

        //  compute the convex hull using the Quick Hull algorithm
        public List<Point> quickHull(List<Point> points, Point leftmost, Point rightmost, string direction)
        {
            // turn direction (left or right)
            var turnDirection = Enums.TurnType.Left;
            if (direction == "Right")
            {
                turnDirection = Enums.TurnType.Right;
            }
            else
            {
                turnDirection = Enums.TurnType.Left;
            }

            int farthestPointIndex = -1;
            double maxDist = -1;
            List<Point> hull = new List<Point>();

            // Return empty hull if no points
            if (points.Count == 0)
                return hull;

            // Find the farthest point from the line formed by leftmost and rightmost points
            for (int i = 0; i < points.Count; i++)
            {
                double distance = dist(leftmost, rightmost, points[i]);
                if (CGUtilities.HelperMethods.CheckTurn(new Line(leftmost.X, leftmost.Y, rightmost.X, rightmost.Y), points[i]) == turnDirection && distance > maxDist)
                {
                    farthestPointIndex = i;
                    maxDist = distance;
                }
            }

            // If no point is found, return the two boundary points
            if (farthestPointIndex == -1)
            {
                hull.Add(leftmost);
                hull.Add(rightmost);
                return hull;
            }

            List<Point> part1, part2;
            // Recursively compute the convex hull on the left and right of the farthest point
            if (CGUtilities.HelperMethods.CheckTurn(new Line(points[farthestPointIndex].X, points[farthestPointIndex].Y, leftmost.X, leftmost.Y), rightmost) == Enums.TurnType.Right)
            {
                part1 = quickHull(points, points[farthestPointIndex], leftmost, "Left");
            }
            else
            {
                part1 = quickHull(points, points[farthestPointIndex], leftmost, "Right");
            }

            if (CGUtilities.HelperMethods.CheckTurn(new Line(points[farthestPointIndex].X, points[farthestPointIndex].Y, rightmost.X, rightmost.Y), leftmost) == Enums.TurnType.Right)
            {
                part2 = quickHull(points, points[farthestPointIndex], rightmost, "Left");
            }
            else
            {
                part2 = quickHull(points, points[farthestPointIndex], rightmost, "Right");
            }

            // Merge the two parts of the hull
            for (int i = 0; i < part2.Count; ++i)
                part1.Add(part2[i]);

            return part1;
        }
    }
}
