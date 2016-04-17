using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;
using Pudge;
using Pudge.Player;

namespace PudgeClient
{
    public class Decider
    {
        public const int maxCoord = 170;
        public const int minCoord = -maxCoord;
        const int step = 5;
        private Graph graph;
        public PudgeSensorsData data;
        public SeenNetwork network;
        public Location Location => new Location(data.SelfLocation);

        public Decider(Graph graph, PudgeSensorsData data, SeenNetwork network)
        {
            this.data = data;
            this.graph = graph;
            this.network = network;
        }

        public Rune GetBest(List<Rune> runes)
        {
            var minimum = double.PositiveInfinity;
            Rune best = null;
            foreach (var rune in runes)
            {
                var now = rune.GetDistance(Location) / rune.prior;
                if (minimum > now)
                {
                    minimum = now;
                    best = rune;
                } 
            }
            return best;
        }

        public Node Decide()
        {
            Location pos = new Location(data.SelfLocation);
            var edges = graph[pos];
            if (edges == null)
                return graph.GetClosestNode(pos);
            var runes = graph.runes
                .Where(x => !x.visited)
                .ToList();
            if (runes.Count != 0)
                return GetBest(runes);
            else
            {
                var center = new Point(0, 0);
                if (pos == center)
                    return null;
                return graph.TryGetRune(new Point(0, 0));
            }
            /*var to = edges
                .Select(x => x.To)
                .OrderByDescending(x => network.GetUnseenNeighboursCount(x))
                .ThenBy(x => Math.Abs(pos.GetTurnAngle(x)))
                .ToList();
            return to[0];*/
        }
    }
}
