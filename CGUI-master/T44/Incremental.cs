using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    //assigned to mohamed
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> inputPoints, List<Line> lines, List<Polygon> polygons, ref List<Point> outputPoints, ref List<Line> outputLines, ref List<Polygon> outputPolygons)
        {
            // Remove duplicates and sort points by X and then by Y
            inputPoints = inputPoints.Distinct().OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            // If there are 2 or fewer points, the convex hull is the set of input points
            if (inputPoints.Count <= 2)
            {
                outputPoints = inputPoints; // All points are the convex hull if <= 2 points
                return;
            }

            //Initialize the convex hull
            List<Point> convexHull = new List<Point>();

            // Construct the lower hull
            foreach (var currentPoint in inputPoints)
            {
                // Remove the last point if it makes a right turn with the current point
                while (convexHull.Count >= 2 && IsRightTurn(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], currentPoint))
                {
                    convexHull.RemoveAt(convexHull.Count - 1); 
                }
                convexHull.Add(currentPoint);
            }

            // Construct the upper hull
            int lowerHullSize = convexHull.Count;
            for (int i = inputPoints.Count - 1; i >= 0; i--)
            {
                Point currentPoint = inputPoints[i];
                // Remove the last point if it makes a right turn with the current point
                while (convexHull.Count > lowerHullSize && IsRightTurn(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], currentPoint))
                {
                    convexHull.RemoveAt(convexHull.Count - 1);
                }
                convexHull.Add(currentPoint); 
            }

            // Remove the last point since it's the same as the first point
            convexHull.RemoveAt(convexHull.Count - 1);

            
            outputPoints = convexHull;
        }

        // check if the turn is a "right turn" three points
        private bool IsRightTurn(Point p1, Point p2, Point p3)
        {
            // Calculate the cross product to determine the orientation
            double crossProduct = (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
            return crossProduct <= 0; // True if it's a right turn or collinear
        }


        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
