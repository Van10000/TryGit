using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace PudgeClient
{
    public class SeenNetwork
    {
        private SortedSet<Point> seen = new SortedSet<Point>();
        private List<Point> pointsNetwork = new List<Point>();
        private double visibilityRadius;

        public SeenNetwork(int minCoord, int maxCoord, int step, double visibilityRadius)
        {
            for (int i = minCoord + step; i < maxCoord; i += step)
                for (int j = minCoord + step; j < maxCoord; j += step)
                    pointsNetwork.Add(new Point(i, j));
            this.visibilityRadius = visibilityRadius;
        }

        public int GetUnseenNeighboursCount(Point a)
        {
            int count = 0;
            foreach (var point in GetPointsAround(a))
                if (!(seen.Contains(point)))
                    count++;
            return count;
        }

        public IEnumerable<Point> GetPointsAround(Point a)
        {
            foreach (var point in pointsNetwork)
                if (point.GetDistance(a) < visibilityRadius)
                    yield return point;
        }

        public void MarkSeenNeighbours(Point a)
        {
            foreach (var point in GetPointsAround(a))
                seen.Add(point);
        }
    }
}
