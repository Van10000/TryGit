using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public static class Geometry
    {
        public static void Transfer(Vector transferVector, Point[] points)
        {
            for (int i = 0; i < points.Length; ++i)
                points[i] = points[i] + transferVector;
        }

        public static void Transfer(Vector transferVector, Circle[] circles)
        {
            for (int i = 0; i < circles.Length; ++i)
                circles[i] = circles[i] + transferVector;
        }

        public static void Scale(double scale, Point[] points)
        {
            for (int i = 0; i < points.Length; ++i)
            {
                points[i].x *= scale;
                points[i].y *= scale;
            }
        }

        public static void Scale(double scale, Circle[] circles)
        {
            for (int i = 0; i < circles.Length; ++i)
            {
                circles[i].x *= scale;
                circles[i].y *= scale;
                circles[i].r *= scale;
            }
        }

        public static Segment CreateSegment(Point A, Point B)
        {
            return new Segment(A, B);
        }

        public static Rectangle CreateRectangle(Point A, Point B, Point C, Point D)
        {
            return new Rectangle(A, B, C, D);
        }

        public static bool IsRectangle(Rectangle rect)
        {
            return IsRectangle(rect.A, rect.B, rect.C, rect.D);
        }

        public static bool IsRectangle(Point A, Point B, Point C, Point D)
        {
            var points = OrderClockwise(A, B, C, D);
            return IsRectangle(points);
        }

        public static bool IsRectangle(Point[] points)
        {
            return ((new Vector(points[0], points[1]) == new Vector(points[3], points[2])) &&
                (new Vector(points[1], points[2]) == new Vector(points[0], points[3])) &&
                (Math.Abs(new Vector(points[0], points[1]).GetScalarProduct
                (new Vector(points[0], points[3]))) < Geom.GetPrecision()));
        }

        public static bool PointInInsideSegment(Point point, Segment segment)
        {
            return segment.ContainsPoint(point);
        }

        public static Point[] OrderClockwise(params Point[] points)
        {
            for (int i = 0; i < points.Length; ++i)
                if (points[0] > points[i])
                {
                    Point forSwap = points[0];
                    points[0] = points[i];
                    points[i] = forSwap;
                }
            for (int i = 1; i < points.Length; ++i)
                for (int j = i + 1; j < points.Length; ++j)
                {
                    Vector toI = new Vector(points[0], points[i]);
                    Vector toJ = new Vector(points[0], points[j]);
                    if (toI.GetDotProduct(toJ) > 0)
                    {
                        Point forSwap = points[i];
                        points[i] = points[j];
                        points[j] = forSwap;
                    }
                }
            return points;
        } 
    }
}