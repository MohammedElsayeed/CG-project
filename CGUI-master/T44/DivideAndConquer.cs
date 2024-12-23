using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        
        /*
         check edit -> yousef
        check editing mo
        check edit mo
        
         */
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // Sort points x and then y
            points = points.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            outPoints = new List<Point>();

            // Compute the convex -> divide and conquer
            List<Point> new_po = SplitPoints(points);

            // Add unique points
            for (int i = 0; i < new_po.Count; ++i)
                if (!outPoints.Contains(new_po[i]))
                {
                    outPoints.Add(new_po[i]);

                }
        }

  
        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }
        /*
         
         
         
         */

        //******** Recursive division ************
        public List<Point> SplitPoints(List<Point> inputPoints)
        {
            if (inputPoints.Count == 1)
            {
                return inputPoints; // Base case
            }

            // Split points into left and right halves
            List<Point> leftPoints = new List<Point>();
            List<Point> rightPoints = new List<Point>();
            for (int i = 0; i < inputPoints.Count / 2; i++)
                leftPoints.Add(inputPoints[i]);
            for (int i = inputPoints.Count / 2; i < inputPoints.Count; i++)
                rightPoints.Add(inputPoints[i]);

            // Recursively compute convex hulls for left and right halves
            List<Point> leftHull = SplitPoints(leftPoints);
            List<Point> rightHull = SplitPoints(rightPoints);

            // Merge the two halves
            return CombineHulls(leftHull, rightHull);
        }

        // ************** Merges two convex hulls into a single hull **************
        public List<Point> CombineHulls(List<Point> firstHull, List<Point> secondHull)
        {
            // Create unique point lists for both hulls
            List<Point> uniqueFirstHull = new List<Point>();
            List<Point> uniqueSecondHull = new List<Point>();

            for (int i = 0; i < firstHull.Count; ++i)
            {
                if (!uniqueFirstHull.Contains(firstHull[i]))
                    uniqueFirstHull.Add(firstHull[i]);
            }

            for (int i = 0; i < secondHull.Count; ++i)
            {
                if (!uniqueSecondHull.Contains(secondHull[i]))
                    uniqueSecondHull.Add(secondHull[i]);
            }

            // Find rightmost point of 'uniqueFirstHull' and leftmost point of 'uniqueSecondHull'
            int firstHullSize = uniqueFirstHull.Count;
            int secondHullSize = uniqueSecondHull.Count;
            int rightmostIndex = 0;
            int leftmostIndex = 0;

            for (int i = 1; i < firstHullSize; i++)
            {
                if (uniqueFirstHull[i].X > uniqueFirstHull[rightmostIndex].X)
                    rightmostIndex = i;
                else if (uniqueFirstHull[i].X == uniqueFirstHull[rightmostIndex].X)
                {
                    if (uniqueFirstHull[i].Y > uniqueFirstHull[rightmostIndex].Y)
                        rightmostIndex = i;
                }
            }

            for (int i = 1; i < secondHullSize; i++)
            {
                if (uniqueSecondHull[i].X < uniqueSecondHull[leftmostIndex].X)
                    leftmostIndex = i;
                else if (uniqueSecondHull[i].X == uniqueSecondHull[leftmostIndex].X)
                {
                    if (uniqueSecondHull[i].Y < uniqueSecondHull[leftmostIndex].Y)
                        leftmostIndex = i;
                }
            }

            // Find the upper tangent
            int upperFirst = rightmostIndex;
            int upperSecond = leftmostIndex;
            bool tangentFound = false;

            while (!tangentFound)
            {
                tangentFound = true;

                // Move 'uniqueFirstHull' clockwise
                while (CGUtilities.HelperMethods.CheckTurn(
                           new Line(uniqueSecondHull[upperSecond].X, uniqueSecondHull[upperSecond].Y, uniqueFirstHull[upperFirst].X, uniqueFirstHull[upperFirst].Y),
                           uniqueFirstHull[(upperFirst + 1) % firstHullSize]) == Enums.TurnType.Right)
                {
                    upperFirst = (upperFirst + 1) % firstHullSize;
                    tangentFound = false;
                }

                // Check for collinear case
                if (tangentFound == true &&
                    (CGUtilities.HelperMethods.CheckTurn(
                         new Line(uniqueSecondHull[upperSecond].X, uniqueSecondHull[upperSecond].Y, uniqueFirstHull[upperFirst].X, uniqueFirstHull[upperFirst].Y),
                         uniqueFirstHull[(upperFirst + 1) % firstHullSize]) == Enums.TurnType.Colinear))
                    upperFirst = (upperFirst + 1) % firstHullSize;

                // Move 'uniqueSecondHull' counterclockwise
                while (CGUtilities.HelperMethods.CheckTurn(
                           new Line(uniqueFirstHull[upperFirst].X, uniqueFirstHull[upperFirst].Y, uniqueSecondHull[upperSecond].X, uniqueSecondHull[upperSecond].Y),
                           uniqueSecondHull[(secondHullSize + upperSecond - 1) % secondHullSize]) == Enums.TurnType.Left)
                {
                    upperSecond = (secondHullSize + upperSecond - 1) % secondHullSize;
                    tangentFound = false;
                }

                // Check for collinear case
                if (tangentFound == true &&
                    (CGUtilities.HelperMethods.CheckTurn(
                         new Line(uniqueFirstHull[upperFirst].X, uniqueFirstHull[upperFirst].Y, uniqueSecondHull[upperSecond].X, uniqueSecondHull[upperSecond].Y),
                         uniqueSecondHull[(upperSecond + secondHullSize - 1) % secondHullSize]) == Enums.TurnType.Colinear))
                    upperSecond = (upperSecond + secondHullSize - 1) % secondHullSize;
            }

            // Find the lower tangent
            int lowerFirst = rightmostIndex;
            int lowerSecond = leftmostIndex;
            tangentFound = false;

            while (!tangentFound)
            {
                tangentFound = true;

                // Move 'uniqueFirstHull' counterclockwise
                while (CGUtilities.HelperMethods.CheckTurn(
                           new Line(uniqueSecondHull[lowerSecond].X, uniqueSecondHull[lowerSecond].Y, uniqueFirstHull[lowerFirst].X, uniqueFirstHull[lowerFirst].Y),
                           uniqueFirstHull[(lowerFirst + firstHullSize - 1) % firstHullSize]) == Enums.TurnType.Left)
                {
                    lowerFirst = (lowerFirst + firstHullSize - 1) % firstHullSize;
                    tangentFound = false;
                }

                // Check for collinear case
                if (tangentFound == true &&
                    (CGUtilities.HelperMethods.CheckTurn(
                         new Line(uniqueSecondHull[lowerSecond].X, uniqueSecondHull[lowerSecond].Y, uniqueFirstHull[lowerFirst].X, uniqueFirstHull[lowerFirst].Y),
                         uniqueFirstHull[(lowerFirst + firstHullSize - 1) % firstHullSize]) == Enums.TurnType.Colinear))
                    lowerFirst = (lowerFirst + firstHullSize - 1) % firstHullSize;

                // Move 'uniqueSecondHull' clockwise
                while (CGUtilities.HelperMethods.CheckTurn(
                           new Line(uniqueFirstHull[lowerFirst].X, uniqueFirstHull[lowerFirst].Y, uniqueSecondHull[lowerSecond].X, uniqueSecondHull[lowerSecond].Y),
                           uniqueSecondHull[(lowerSecond + 1) % secondHullSize]) == Enums.TurnType.Right)
                {
                    lowerSecond = (lowerSecond + 1) % secondHullSize;
                    tangentFound = false;
                }

                // Check for collinear case
                if (tangentFound == true &&
                    (CGUtilities.HelperMethods.CheckTurn(
                         new Line(uniqueFirstHull[lowerFirst].X, uniqueFirstHull[lowerFirst].Y, uniqueSecondHull[lowerSecond].X, uniqueSecondHull[lowerSecond].Y),
                         uniqueSecondHull[(lowerSecond + 1) % secondHullSize]) == Enums.TurnType.Colinear))
                    lowerSecond = (lowerSecond + 1) % secondHullSize;
            }

            // Collect points of the merged hull
            List<Point> finalHullPoints = new List<Point>();

            // Add points from 'uniqueFirstHull'
            int currentIndex = upperFirst;
            if (!finalHullPoints.Contains(uniqueFirstHull[upperFirst]))
                finalHullPoints.Add(uniqueFirstHull[upperFirst]);

            while (currentIndex != lowerFirst)
            {
                currentIndex = (currentIndex + 1) % firstHullSize;
                if (!finalHullPoints.Contains(uniqueFirstHull[currentIndex]))
                    finalHullPoints.Add(uniqueFirstHull[currentIndex]);
            }

            // Add points from 'uniqueSecondHull'
            currentIndex = lowerSecond;
            if (!finalHullPoints.Contains(uniqueSecondHull[lowerSecond]))
                finalHullPoints.Add(uniqueSecondHull[lowerSecond]);

            while (currentIndex != upperSecond)
            {
                currentIndex = (currentIndex + 1) % secondHullSize;
                if (!finalHullPoints.Contains(uniqueSecondHull[currentIndex]))
                    finalHullPoints.Add(uniqueSecondHull[currentIndex]);
            }

            return finalHullPoints;
        }


    }


}
