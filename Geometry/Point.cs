using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Point : Geom, IComparable<Point>
    {
        public double x, y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double GetDistance(Point point)
        {
            var vector = new Vector(this, point);
            return vector.GetLength();
        }

        public bool IsOnSegment(Segment segment)
        {
            return segment.ContainsPoint(this);
        }

        /// <summary>
        /// Вращает исходную точку относительно принимаемой методом точки на угол, заданный синусом
        /// и косинусом.
        /// </summary>
        public void Rotate(Point point, double sin, double cos)
        {
            if (this == point)
                return;
            Vector toPointVector = new Vector(point, this);
            toPointVector.Rotate(sin, cos);
            x = point.x + toPointVector.x;
            y = point.y + toPointVector.y;
        }

        /// <summary>
        /// Возвращает исходную точку, повёрнутую относительно принимаемой методом точки на угол,
        ///  заданный в синусом и косинусом.
        /// </summary>
        public Point GetRotated(Point point, double sin, double cos)
        {
            if (this == point)
                return new Point(x, y);
            Vector toPointVector = new Vector(point, this);
            toPointVector.Rotate(sin, cos);
            return point + toPointVector;
        }

        /// <summary>
        /// Вращает исходную точку относительно принимаемой методом точки на угол, заданный в радианах.
        /// </summary>
        public void Rotate(Point point, double angle)
        {
            Rotate(point, Math.Sin(angle), Math.Cos(angle));
        }

        /// <summary>
        /// Возвращает исходную точку, повёрнутую относительно принимаемой методом точки на угол,
        ///  заданный в радианах.
        /// </summary>
        public Point GetRotated(Point point, double angle)
        {
            return GetRotated(point, Math.Sin(angle), Math.Cos(angle));
        }

        public int CompareTo(Point other)
        {
            if (this == other)
                return 0;
            return this < other ? -1 : 1;
        }

        public static Point operator +(Point point, Vector vector)
        {
            return new Point(point.x + vector.x, point.y + vector.y);
        }

        public static Point operator -(Point point, Vector vector)
        {
            return new Point(point.x - vector.x, point.y - vector.y);
        }

        public static bool operator ==(Point A, Point B)
        {
            if (Object.ReferenceEquals(A, null) && Object.ReferenceEquals(B, null))
                return true;
            if (Object.ReferenceEquals(A, null) || Object.ReferenceEquals(B, null))
                return false;
            return ((Math.Abs(A.x - B.x) < GetPrecision()) && (Math.Abs(A.y - B.y) < GetPrecision()));
        }

        public static bool operator !=(Point A, Point B)
        {
            return !(A == B);
        }

        public static bool operator < (Point A, Point B)
        {
            return ((A.x < B.x - GetPrecision()) || 
                ((Math.Abs(A.x - B.x) < GetPrecision()) && (A.y < B.y - GetPrecision())));
        }

        public static bool operator > (Point A, Point B)
        {
            return B < A;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;
            return ((Point)obj) == this;
        }
    }
}
