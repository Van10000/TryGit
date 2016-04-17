using System;
using System.Collections.Generic;

namespace PudgeClient
{
    public class Edge : IComparable<Edge>
    {
        public readonly Node From;
        public readonly Node To;
        public readonly double Length;
        
        public Edge(Node from, Node to)
        {
            From = from;
            To = to;
            Length = To.GetDistance(From);
        }

        public override string ToString()
        {
            return From.ToString() + " " + To.ToString();
        }

        public int CompareTo(Edge other)
        {
            int res = To.CompareTo(other.To);
            if (res == 0)
                return From.CompareTo(other.From);
            return res;
        }

        public void Clear()
        {
            To.Clear();
        }
    }
}
