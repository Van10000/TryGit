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
        public const int pathSplitPiecesCount = 5;
        Graph graph;
        List<Node> touched = new List<Node>();
        private List<Point> currentPath;
        private int currentPointNumber;

        public PathFinder(Graph graph)
        {
            this.graph = graph;
        }

        public List<Point> SplitPath(List<Point> path, int piecesCount)
        {
            List<Point> newPath = new List<Point>();
            for (int i = 0; i < path.Count - 1; ++i)
            {
                var seg = new Segment(path[i], path[i + 1]);
                for (double ratio = 0.0; ratio < 0.99; ratio += 1.0/pathSplitPiecesCount)
                    newPath.Add(seg.GetDividingPointByRatio(ratio));
            }
            newPath.Add(path.Last());
            return newPath;
        }

        public Point GetNextPoint(Location location, Point destination)
        {
            if (destination == null)
                return graph.GetClosestNode(location);
            if (currentPath == null)
            {
                if (graph[location] != null)
                {
                    var currentDestination = ProcessDijkstra(location, destination);
                    currentPath = SplitPath(GetPath(location, currentDestination), pathSplitPiecesCount);
                    return currentPath[1];
                }
                else
                {
                    return graph.GetClosestNode(location);
                }
            }
            int index = currentPath.FindIndex(x => x == location);
            if (index != -1 && currentPath.Count > index + 1)
                return currentPath[index + 1];
            else
            {
                if (graph[location] != null)
                {
                    var currentDestination = ProcessDijkstra(location, destination);
                    currentPath = SplitPath(GetPath(location, currentDestination), pathSplitPiecesCount);
                    return currentPath[1];
                }
                else
                {
                    currentPath = null;
                    return graph.GetClosestNode(location);
                }
            }
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
            way.Add(start);
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
