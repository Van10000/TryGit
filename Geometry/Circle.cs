using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Circle : Geom
    {
        public double x, y, r;

        public Circle(double x, double y, double radius)
        {
            this.x = x;
            this.y = y;
            this.r = radius;
        }

        public Circle(Point centre, double radius)
        {
            x = centre.x;
            y = centre.y;
            r = radius;
        }

        public Point GetCentre()
        {
            return new Point(x, y);
        }

        public static Circle operator +(Circle circle, Vector vector)
        {
            return new Circle(circle.GetCentre() + vector, circle.r);
        }

        public static Circle operator -(Circle circle, Vector vector)
        {
            return new Circle(circle.GetCentre() - vector, circle.r);
        }
    }
}
