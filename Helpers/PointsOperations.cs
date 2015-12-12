using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Math;

namespace ColdBilancer.Helpers
{
    public static class PointsOperations
    {
        /// <summary>
        /// Returns value in radians 
        /// </summary>
      
        public static double AngleBetweenPoints(Point p1, Point p2)
        {
            return Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }

        public static double DistanceBetweenPoints(Point p1, Point p2)
        {
            return Sqrt(Pow(p2.X - p1.X, 2.0) + Pow(p1.Y - p2.Y, 2.0));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static double? DistanceBetweenPointsInRadius(Point p1, Point p2, double radius)
        {
            double? d = DistanceBetweenPoints(p1, p2);
            return d <= radius ? d : null;
        }

        public static bool IsPointInsideRadius(Point p1, Point p2, double radius)
        {
            return DistanceBetweenPoints(p1, p2) <= radius;
        }
    }
}
