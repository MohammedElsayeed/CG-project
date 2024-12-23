using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    //assigned to mohamed
    public class ExtremePoints : Algorithm
    {
        // Variables to keep track of points
        private int point1, point2, point3, checkPoint;

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // Iterate through all possible triangles formed by points
            for (point1 = 0; point1 < points.Count; point1++)
            {
                for (point2 = point1 + 1; point2 < points.Count; point2++)
                {
                    for (point3 = point2 + 1; point3 < points.Count; point3++)
                    {
                        // For each triangle, check if the other points are inside it
                        for (checkPoint = 0; checkPoint < points.Count; checkPoint++)
                        {
                            // Skip if the point is one of the triangle's vertices
                            if (points[checkPoint] != points[point1] && points[checkPoint] != points[point2] && points[checkPoint] != points[point3])
                            {
                                // Check if the point is inside the triangle
                                if (HelperMethods.PointInTriangle(points[checkPoint], points[point1], points[point2], points[point3]) != Enums.PointInPolygon.Outside)
                                {
                                    // Remove the point from the list if it is inside the triangle
                                    points.Remove(points[checkPoint]);

                                    // Update the indices after removal
                                    UpdateIndices(ref point1, ref point2, ref point3, checkPoint);
                                }
                            }
                        }
                    }
                }
            }
            outPoints = points; // Return  remaining points
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }

        // update indices after removing a point
        public static void UpdateIndices(ref int index1, ref int index2, ref int index3, int removedIndex)
        {
            // If the removed index affects indexA
            if (removedIndex < index1)
            {
                index1--;
            }

            // If removed index affects indexB
            if (removedIndex < index2)
            {
                index2--;
            }

            // removed index affects indexC
            if (removedIndex < index3)
            {
                index3--;
            }
        }
    }
}
