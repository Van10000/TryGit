using System;
using Geometry;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PudgeClient
{
    [JsonObject]
    public class Graph
    {
        const double maxGo = 51;
        const int go = 20;
        const double pudgeSize = 7.5; // TODO pudge is a square - not a circle
        const double treeSize = 8;

        public SortedDictionary<Node, List<Edge>> edges = new SortedDictionary<Node, List<Edge>>();
        readonly List<Point> Trees = GetTreesCoordinates().Select(x => new Point(x[0], x[1])).ToList();
        public SortedSet<Rune> runes = new SortedSet<Rune>();

        [JsonConstructor]
        private Graph()
        {
        }

        public Graph(double minCoord, double maxCoord)
        {
            for (var x = minCoord; x < maxCoord; x += go)
                for (var y = minCoord; y < maxCoord; y += go)
                {
                    var newNode = new Node(x, y);
                    edges.Add(newNode, newNode.Edges);
                }
            foreach (var node in edges.Keys)
                AddEdges(node, false);
        }

        public void AddEdges(Node node, bool doubleConnect)
        {
            foreach (var curNode in edges.Keys.Where(curNode => curNode != node)
                                         .Where(curNode => node.GetDistance(curNode) < maxGo))                                        
            {
                var curSegment = new Segment(curNode, node);
                if (!IntersectsWithTrees(curSegment))
                    if (doubleConnect)
                        DoubleConnect(node, curNode);
                    else
                        Connect(node, curNode);
            }
        }

        public void Connect(Node a, Node b)
        {
            this[a].Add(new Edge(a, b));
        }

        public void DoubleConnect(Node a, Node b)
        {
            this[a].Add(new Edge(a, b));
            this[b].Add(new Edge(b, a));
        }

        public void ClearNodes(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
                node.Clear();
        }

        public List<Edge> this[Point node] => TryGetEdges(node);

        public List<Edge> TryGetEdges(Point point)
        {
            var node = new Node(point);
            if (edges.ContainsKey(node))
                return edges[node];
            return null;
        }

        public bool IntersectsWithTrees(Segment segment)
        {
            var intersects = false;
            var treeSegments = Trees.Select(tree => new Rectangle(tree, treeSize))
                                    .SelectMany(treeRect => treeRect.GetSegments());
            foreach (var treeSegment in treeSegments)
                if (segment.GetDistance(treeSegment) < pudgeSize)
                {
                    intersects = true;
                    break;
                }
            return intersects;
        }

        public Rune TryGetRune(Point point)
        {
            var rune = new Rune(point.x, point.y);
            foreach (var curRune in runes)
                if (curRune == rune) return curRune;
            return null;
        }

        public Node GetClosestNode(Point curNode)
        {
            var nodes = edges.Keys.Where(node => node != curNode)
                                  .ToList();
            nodes.Sort(Comparer<Node>.Create((n1, n2) => n1.GetDistance(curNode).CompareTo(n2.GetDistance(curNode))));
            foreach (var node in nodes)
                if (!IntersectsWithTrees(new Segment(node, curNode)))
                    return node;
            return null;
        }

        public static List<double[]> GetTreesCoordinates()
        {
            return JsonConvert.DeserializeObject<List<double[]>>(string.Join("", File.ReadAllLines("trees.json")));
        }

    }
}
