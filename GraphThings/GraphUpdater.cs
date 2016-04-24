using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pudge;
using Pudge.Player;
using System.Timers;

namespace PudgeClient
{
    public class GraphUpdater
    {
        private Graph graph;
        private int count = 0;

        public GraphUpdater(Graph graph)
        {
            this.graph = graph;
        }

        public void UpdateRunes(PudgeSensorsData data)
        {
            int nowCount = (int)Math.Floor(data.WorldTime / PudgeRules.Current.RuneRespawnTime);
            if (nowCount > count)
            {
                count++;
                foreach (var rune in graph.runes)
                    rune.visited = false;
            }

        }

        public void Update(PudgeSensorsData data)
        {
            var Location = new Location(data.SelfLocation);
            var runesAround = graph.runes
                .Where(x => x.GetDistance(Location) <= PudgeRules.Current.VisibilityRadius - 1);
            var seeRunes = data.Map.Runes.Select(x => new Rune(x.Location.X, x.Location.Y));
            var runesVisited = runesAround
                .Where(x => !seeRunes.Contains(x));
            foreach (var rune in runesVisited)
                rune.visited = true;
        }
    }
}
