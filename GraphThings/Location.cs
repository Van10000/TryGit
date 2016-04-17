using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace PudgeClient
{
    public class Location : Point
    {
        public double angle;

        public Location(double x, double y, double angle) : base(x, y)
        {
            this.angle = angle;
        }

        public Location(Point a, double angle) : base(a.x, a.y)
        {
            this.angle = angle;
        }

        public Location(CVARC.V2.LocatorItem location) : base(location.X, location.Y)
        {
            this.angle = location.Angle;
        }

        private static double NormalizeAngle(double angle)
        {
            return ((angle + Math.PI) % (2 * Math.PI) + (2 * Math.PI)) % (2 * Math.PI) - Math.PI;
        }

        private static double ToDegrees(double angle)
        {
            return angle / Math.PI * 180;
        }

        private static double ToRadians(double angle)
        {
            return angle / 180 * Math.PI;
        }

        public double GetTurnAngle(Point descination)
        {
            var direction = new Vector(this, descination);
            var nextAngle = direction.GetPolarAngle();
            var angle = NormalizeAngle(nextAngle - ToRadians(this.angle));
            return ToDegrees(angle);
        }
    }
}
