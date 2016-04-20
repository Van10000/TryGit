using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace PudgeClient
{
    public class DistanceComparer : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            if (Math.Abs(x.Distance - y.Distance) < Geom.GetPrecision() && x == y)
                return 0;
            int res = x.Distance.CompareTo(y.Distance);
            if (res == 0)
                return x.CompareTo(y);
            return res;
        }
    }

    public class PathFinder
    {
        Graph graph;
        List<Node> touched = new List<Node>();
        private List<Point> currentPath;
        private int currentPointNumber;

        public PathFinder(Graph graph)
        {
            this.graph = graph;
        }

        public Point GetNextPoint(Location location, Point destination)
        {
            if (currentPath == null || currentPath.Count <= currentPointNumber ||
                currentPath[currentPointNumber] != location || currentPath.Last() != destination)
            {
                var currentDestination = ProcessDijkstra(location, destination);
                currentPath = GetPath(location, currentDestination);
                currentPointNumber = 0;
            }
            if (graph[location] == null)
                return destination;
            currentPointNumber++;
            return currentPath[currentPointNumber - 1];
        }

        public List<Point> GetPath(Point start, Node destination)
        {
            var current = destination;
            var way = new List<Point>();
            while (current != start)
            {
                way.Add(current);
                current = current.Ancestor;
            }
            way.Reverse();
            return way;
        }

        public Node ProcessDijkstra(Location location, Point destination)
        {
            graph.ClearNodes(touched);
            touched.Clear();
            var heap = new SortedSet<Node>(new DistanceComparer());
            var firstNode = new Node(location, 0);
            heap.Add(firstNode);
            while (heap.Count != 0)
            {
                var current = heap.Min;
                heap.Remove(current);
                foreach (var edge in graph[current])
                {
                    var to = edge.To;
                    touched.Add(to);
                    var previousDist = to.Distance;
                    var newDist = current.Distance + edge.Length;
                    if (previousDist > newDist)
                    {
                        heap.Remove(to);
                        to.Distance = newDist;
                        to.Ancestor = current;
                        heap.Add(new Node(to, newDist));
                    }
                    if (to == destination)
                        return new Node(to, to.Distance);
                }
            }
            return null;
        }
    }
}
