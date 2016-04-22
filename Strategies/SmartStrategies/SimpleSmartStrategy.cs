using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pudge;
using Pudge.Player;
using Geometry;

namespace PudgeClient
{
    public class SimpleSmartStrategy : SmartStrategy
    {
        public override IEnumerable<Command> Commands
        {
            get
            {
                while (true)
                {
                    yield return new LongKillMoveCommand(GetClosestRune());
                }
            }
        }

        public SimpleSmartStrategy(PudgeSensorsData data, Graph graph) : base(data, graph) { }

    }
}
