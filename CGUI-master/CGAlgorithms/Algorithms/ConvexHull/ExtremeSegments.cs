using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    //assigned to kero
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            //  when only one point exists
            if (points.Count == 1)
            {
                outPoints = points;
                return;
            }
            //point3 is acompare point
            int point1, point2, point3, rotation = 0, colinearPoints = 0;

            // Remove duplicate points
            removeDuplicatePoints(ref points);
            Line segment;
            List<Point> pointsToRemove = new List<Point>();
            HashSet<Point> convexHullPoints = new HashSet<Point>();

            // check convex hull segments
            for (point1 = 0; point1 < points.Count; point1++)
            {
                for (point2 = point1 + 1; point2 < points.Count; point2++)
                {
                    segment = new Line(points[point1], points[point2]);

                    // Check the direction (left, right, or colinear) 
                    for (point3 = 0; point3 < points.Count; point3++)
                    {

                        if (points[point3] != points[point1] && points[point3] != points[point2])
                        {

                            switch (HelperMethods.CheckTurn(segment, points[point3]))
                            {

                                case Enums.TurnType.Left:
                                    rotation--; break;

                                case Enums.TurnType.Right:
                                    rotation++; break;

                                default:
                                    colinearPoints++;

                                    // Check if the point is on the segment
                                    if (HelperMethods.PointOnSegment(points[point3], points[point1], points[point2]))
                                    {

                                        pointsToRemove.Add(points[point3]);
                                    }

                                    break;
                            }
                        }
                    }

                    // Check if the segment forms part of the convex hull
                    if (Math.Abs(rotation) == (points.Count - 2 - colinearPoints))
                    {
                        convexHullPoints.Add(points[point1]);
                        convexHullPoints.Add(points[point2]);
                    }

                    rotation = 0;
                    colinearPoints = 0;
                }
            }

            // Remove points that are colinear and not part of the hull
            foreach (var point in pointsToRemove)
            {

                convexHullPoints.Remove(point);
            }

            outPoints = convexHullPoints.ToList();
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }

        // Method to remove duplicate points from the list
        public static void removeDuplicatePoints(ref List<Point> inputPoints)
        {
            for (int i = 0; i < inputPoints.Count; i++)
            {
                
                for (int j = i + 1; j < inputPoints.Count; j++)
                {

                    if (inputPoints[i].Equals(inputPoints[j]))
                    {

                        inputPoints.RemoveAt(j);
                        j--;
                        break;
                    }
                }
            }
        }
    }
}
