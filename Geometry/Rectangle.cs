using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Rectangle : Geom
    {
        public Point A, B, C, D; // Points are ordered clockwise

        public Rectangle(Point A, Point B, Point C, Point D)
        {
            var orderedPoints = Geometry.OrderClockwise(A, B, C, D);
            this.A = orderedPoints[0];
            this.B = orderedPoints[1];
            this.C = orderedPoints[2];
            this.D = orderedPoints[3];
            if (!Geometry.IsRectangle(this))
                throw new Exception("Incorrect data inputted in Rectangle constructor");
        }

        public Rectangle(Point center, double halfSideSize)
        {
            var dx = new[] { -1, -1, 1, 1 };
            var dy = new[] { 1, -1, 1, -1 };
            List<Point> points = new List<Point>();
            for (int i = 0; i < dx.Length; ++i)
                points.Add(new Point(center.x + dx[i] * halfSideSize, center.y + dy[i] * halfSideSize));
            points = Geometry.OrderClockwise(points.ToArray()).ToList();
            A = points[0];
            B = points[1];
            C = points[2];
            D = points[3];
        }

        public Rectangle(Point A, Point B, Point C)
        {
            Rectangle rect = null;
            try
            {
                rect = new Rectangle(A, B, C, C - new Vector(A, B));
            }
            catch { }

            try
            {
                rect = new Rectangle(A, B, C, C + new Vector(A, B));
            }
            catch { }

            try
            {
                rect = new Rectangle(A, C, B, B - new Vector(A, C));
            }
            catch { }

            try
            {
                rect = new Rectangle(A, C, B, B + new Vector(A, C));
            }
            catch { }
            try
            {
                rect.GetSegments(); // if it's null it will throw Exception
                this.A = rect.A;
                this.B = rect.B;
                this.C = rect.C;
                this.D = rect.D;
            }
            catch (Exception)
            {
                throw new Exception("Incorrect data inputted in Rectangle constructor");
            }
        }

        public void Rotate(Point point, double sin, double cos)
        {
            A.Rotate(point, sin, cos);
            B.Rotate(point, sin, cos);
            C.Rotate(point, sin, cos);
            D.Rotate(point, sin, cos);
        }

        public Point[] GetPoints()
        {
            var points = new Point[4];
            points[0] = A;
            points[1] = B;
            points[2] = C;
            points[3] = D;
            return points;
        }

        public Segment[] GetSegments()
        {
            var segments = new Segment[4];
            segments[0] = new Segment(A, B);
            segments[1] = new Segment(B, C);
            segments[2] = new Segment(C, D);
            segments[3] = new Segment(D, A);
            return segments;
        }

        public Rectangle GetRotated(Point point, double sin, double cos)
        {
            return new Rectangle(A.GetRotated(point, sin, cos), B.GetRotated(point, sin, cos), 
                C.GetRotated(point, sin, cos), D.GetRotated(point, sin, cos));
        }

        public void Rotate(Point point, double angle)
        {
            Rotate(point, Math.Sin(angle), Math.Cos(angle));
        }

        public Rectangle GetRotated(Point point, double angle)
        {
            return GetRotated(point, Math.Sin(angle), Math.Cos(angle));
        }

        public double GetDistance(Point point)
        {
            return GetSegments().Min(x => x.GetDistance(point));
        }

        public void CyclicalShiftOfPoints()
        {
            Point tmpPoint = A;
            A = B;
            B = C;
            C = D;
            D = tmpPoint;
        }

        private bool IsAbsolutelyEqual(Rectangle rect)
        {
            return ((rect.A == A) && (rect.B == B) && (rect.C == C) && (rect.D == D));
        }

        public static bool operator == (Rectangle rect1, Rectangle rect2)
        {
            for (int i = 0; i < 4; ++i)
            {
                if (rect1.IsAbsolutelyEqual(rect2))
                    return true;
                rect1.CyclicalShiftOfPoints();
            }
            return false;
        }

        public static bool operator != (Rectangle rect1, Rectangle rect2)
        {
            return !(rect1 == rect2);
        }
    }
}
