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

        public GraphUpdater(Graph graph)
        {
            this.graph = graph;
            var timer = new Timer(PudgeRules.Current.RuneRespawnTime * 1000);
            timer.AutoReset = true;
            timer.Start();
            timer.Elapsed += ((sender, args) => UpdateRunes());
        }

        public void UpdateRunes()
        {
            foreach (var rune in graph.runes)
                rune.visited = false;
        }

        public void Update(PudgeSensorsData data)
        {
            var Location = new Location(data.SelfLocation);
            /*foreach (var rune in data.Map.Runes)
            {
                var curRune = new Rune(rune.Location.X, rune.Location.Y);
                if (!graph.edges.ContainsKey(curRune))
                {
                    graph.edges.Add(curRune, curRune.Edges);
                    graph.runes.Add(curRune);
                    graph.AddEdges(curRune, true);
                }
            }*/
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
