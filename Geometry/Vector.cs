using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Vector : Geom
    {
        public double x, y;

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        
        public Vector(Point A, Point B)
        {
            x = B.x - A.x;
            y = B.y - A.y;
        }

        public void Normalize(double newLength)
        {
            double coefficient = newLength / GetLength();
            x *= coefficient;
            y *= coefficient;
        }

        public bool Collinear(Vector vector)
        {
            return Math.Abs(GetDotProduct(vector)) < GetPrecision();
        }

        public bool SameDirected(Vector vector) // TODO: rename
        {
            return (Collinear(vector) && (GetScalarProduct(vector) > 0));
        }

        public bool DifferentDirected(Vector vector) // TODO: rename
        {
            return (Collinear(vector) && (GetScalarProduct(vector) < 0));
        }

        /// <summary>
        /// Вращает вектор против часовой стрелки на угол, заданный синусом и косинусом.
        /// </summary>
        public void Rotate(double sin, double cos)
        {
            double oldX = x;
            double oldY = y;
            x = oldX * cos - oldY * sin;
            y = oldX * sin + oldY * cos;
        }

        /// <summary>
        /// Возвращает исходный вектор, повёрнутый против 
        /// часовой стрелки на угол, заданный синусом и косинусом.
        /// </summary>
        public Vector GetRotated(double sin, double cos)
        {
            return new Vector(x * cos - y * sin, x * sin + y * cos);
        }

        /// <summary>
        /// Вращает вектор против часовой стрелки на угол, заданный в радианах.
        /// </summary>
        public void Rotate(double angle)
        {
            Rotate(Math.Sin(angle), Math.Cos(angle));
        }

        /// <summary>
        /// Возвращает исходный вектор, повёрнутый против часовой стрелки на угол, заданный в радианах.
        /// </summary>
        public Vector GetRotated(double angle)
        {
            return GetRotated(Math.Sin(angle), Math.Cos(angle));
        }

        public double GetScalarProduct(Vector vector)
        {
            return x * vector.x + y * vector.y;
        }

        public double GetDotProduct(Vector vector)
        {
            return x * vector.y - y * vector.x;
        }

        public double GetPolarAngle()
        {
            return Math.Atan2(y, x);
        }

        public double GetAngle(Vector vector)
        {
            return Math.Atan2(GetDotProduct(vector), GetScalarProduct(vector));
        }

        public double GetLength()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public void Reverse()
        {
            x = -x;
            y = -y;
        }

        public bool IsZeroVector()
        {
            return this == new Vector(0, 0);
        }

        public Vector GetReversed()
        {
            return new Vector(-x, -y);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y);
        }

        public static Vector operator *(Vector a, double t)
        {
            return new Vector(a.x * t, a.y * t);
        }

        public static Vector operator /(Vector a, double t)
        {
            return a * (1 / t);
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return ((Math.Abs(a.x - b.x) < GetPrecision()) && (Math.Abs(a.y - b.y) < GetPrecision()));
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }
    }
}
