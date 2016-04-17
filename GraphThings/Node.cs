using System;
using Pudge;
using Geometry;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;

namespace PudgeClient
{
    [TypeConverter(typeof(NodeConverter))]
    public class Node: Point
    {
        public List<Edge> Edges { get; private set; }
        public Node Ancestor { get; set; }
        public double Distance { get; set; }

        public Node(double x, double y, List<Edge> edges, double distance = double.PositiveInfinity) : base(x, y)
        {
            Edges = new List<Edge>();
            Distance = distance;
            Edges = edges;
        }

        public Node(double x, double y, double distance = double.PositiveInfinity): base(x, y)
        {
            Edges = new List<Edge>();
            Distance = distance;
        }

        public Node(Point point, double distance = double.PositiveInfinity) : base(point.x, point.y)
        {
            Edges = new List<Edge>();
            Distance = distance;
        }

        public Node(Node node, double distance) : base(node.x, node.y)
        {
            Ancestor = node.Ancestor;
            Distance = distance;
        }

        public void Connect(Node node)
        {
            var edge = new Edge(this, node);
            Edges.Add(edge);
        }

        public void DoubleConnect(Node node)
        {
            node.Connect(this);
            Connect(node);
        }

        public void Clear()
        {
            Ancestor = null;
            Distance = double.PositiveInfinity;
        }

        public override string ToString()
        {
            return x + " " + y;
        }
    }
}
