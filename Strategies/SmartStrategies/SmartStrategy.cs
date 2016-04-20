using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pudge;
using Pudge.Player;

namespace PudgeClient
{
    public abstract class SmartStrategy : Strategy
    {
        public PudgeSensorsData data;
        private Graph graph;
        private Location Location => new Location(data.SelfLocation);

        public Rune GetClosestRune()
        {
            var minimum = double.PositiveInfinity;
            Rune best = null;
            //not linq just to make it faster
            foreach (var rune in graph.runes)
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

        public SmartStrategy(PudgeSensorsData data, Graph graph)
        {
            this.data = data;
            this.graph = graph;
        }
    }
}
