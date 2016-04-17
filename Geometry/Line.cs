using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Line : Geom
    {
        public double a, b, c;

        public Line(Point firstPoint, Point secondPoint)
        {
            var directionVector = new Vector(firstPoint, secondPoint);
            a = directionVector.y;
            b = -directionVector.x;
            c = -a * firstPoint.x - b * firstPoint.y;
        }

        public double GetDistance(Point point)
        {
            return Math.Abs((a * point.x + b * point.y + c) / (Math.Sqrt(a * a + b * b)));
        }

        public Vector GetDirectionVector()
        {
            return new Vector(b, -a);
        }

        public Vector GetNormalVector()
        {
            return new Vector(a, b);
        }

        /// <summary>
        /// Возвращает значение выражения, получаемого подстановкой координат точки в уравнение прямой.
        /// </summary>
        public double GetValueOfLineEquation(Point point)
        {
            return a * point.x + b * point.y + c;
        }

        public bool ContainsPoint(Point point)
        {
            return Math.Abs(GetValueOfLineEquation(point)) < GetPrecision();
        }

        public bool IsParallel(Line line)
        {
            Vector firstLineDirectionVector = GetDirectionVector();
            Vector secondLineDirectionVector = line.GetDirectionVector();
            firstLineDirectionVector.Normalize(secondLineDirectionVector.GetLength());
            if (!(firstLineDirectionVector - secondLineDirectionVector).IsZeroVector())
                firstLineDirectionVector = firstLineDirectionVector * (-1.0);
            Vector difference = firstLineDirectionVector - secondLineDirectionVector;
            return difference.IsZeroVector();
        }

        public Point GetPoint()
        {
            return (Math.Abs(b) < GetPrecision()) ? new Point(-c / a, 0) : new Point(0, -c / b);
        }

        public Point Intersect(Line line)
        {
            if (IsParallel(line))
                return new Point(double.NaN, double.NaN);
            Vector a = new Vector(line.a, this.a);
            Vector b = new Vector(line.b, this.b);
            Vector c = new Vector(line.c, this.c);
            return new Point(-c.GetDotProduct(b) / a.GetDotProduct(b), -a.GetDotProduct(c) / a.GetDotProduct(b));
        }

        public static bool operator ==(Line a, Line b)
        {
            return a.IsParallel(b) && b.ContainsPoint(a.GetPoint());
        }

        public static bool operator !=(Line a, Line b)
        {
            return !(a == b);
        }
    }
}
