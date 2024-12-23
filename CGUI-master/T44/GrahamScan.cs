using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    //assigned to yousef
    //assigned to yousef
    public class GrahamScan : Algorithm
    {

        // إذا كان هناك نقطة واحدة فقط، نعيدها مباشرة
        //  تبديل نقطتين
        // إيجاد النقطة الأصغر (النقطة 2
        // تبديل النقطة 2 إلى أول قائمة النقاط
        // ترتيب النقاط 
        // تحديد النقاط التي تبقى في محيط الشكل 
        // فحص الاتجاهات لتحديد ما إذا كان يجب إضافة النقطة أو حذفها
        // إذا كانت الزاوية على اليسار، نوقف الحذف
        // إزالة النقاط المتكررة
        // التحقق من التوازي وإزالة النقاط إذا لزم الأمر
        // إذا كانت النقاط متوازية أو على نفس القطعة، نقوم بحذف النقطة السابقة
        // ترتيب النقاط المتبقية مرة أخرى 


        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outputPoints, ref List<Line> outputLines, ref List<Polygon> outputPolygons)
        {
            // If only one point, return as the Convex Hull
            if (points.Count == 1)
            {
                outputPoints = points;
                return;
            }

            int lowestPointIndex = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < points[lowestPointIndex].Y)
                    lowestPointIndex = i;
            }

             // Swap the lowest point to the beginning
            List<Point> swappedPoints = SwapPoints(points[0], points[lowestPointIndex]);
            points[0] = swappedPoints[0];
            points[lowestPointIndex] = swappedPoints[1];

            // Sort points by Y and then by angle from the base point
            points = points.OrderBy(point => point.Y).ToList();
            points = points.OrderBy(point => Math.Atan2(point.Y - points[0].Y, point.X - points[0].X)).ToList();

            List<Point> remainingPoints = new List<Point>();

            //  which points remain for the Convex Hull
            for (int i = 0; i < points.Count; i++)
            {
                if (i == 0 || i == 1 || i == 2)
                    remainingPoints.Add(points[i]);
                else
                {
                    remainingPoints.Add(points[i]);
                    Point p1, p2, p3;

                    // Check turns 
                    while (true)
                    {
                        p1 = remainingPoints[remainingPoints.Count - 1];
                        p2 = remainingPoints[remainingPoints.Count - 2];
                        p3 = remainingPoints[remainingPoints.Count - 3];

                        Line tempLine = new Line(p3, p2);

                        // If turn is left, stop removing points
                        if (HelperMethods.CheckTurn(tempLine, p1) == Enums.TurnType.Left)
                            break;

                        remainingPoints.Remove(p2);
                    }
                }
            }

            // Remove duplicate points
            removeDuplicatePoints(ref remainingPoints);

            // Check for collinearity and remove points 
            Point firstPoint = remainingPoints[0];
            for (int i = 1; i < remainingPoints.Count - 1; i++)
            {
                Point secondPoint = remainingPoints[i];
                bool isOnSegment = HelperMethods.PointOnSegment(firstPoint, secondPoint, remainingPoints[i + 1]);
                Enums.TurnType turn = HelperMethods.CheckTurn(secondPoint, remainingPoints[i + 1]);

                // If points are collinear or on the same segment, remove the previous point
                if (turn == Enums.TurnType.Colinear || isOnSegment)
                {
                    remainingPoints.Remove(remainingPoints[i - 1]);
                }
            }

            // Sort remaining points again by Y
            remainingPoints = remainingPoints.OrderBy(point => point.Y).ToList();
            outputPoints = remainingPoints;

        }

     
        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }



        // fun swap two points
        public List<Point> SwapPoints(Point firstPoint, Point secondPoint)
        {
            Point temp = firstPoint;
            firstPoint = secondPoint;
            secondPoint = temp;

            List<Point> swappedPoints = new List<Point>();
            swappedPoints.Add(firstPoint);
            swappedPoints.Add(secondPoint);
            return swappedPoints;
        }




        // remove duplicate points from the list
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
