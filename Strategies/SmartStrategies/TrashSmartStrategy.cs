using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;
using Pudge.Player;

namespace PudgeClient
{
    class TrashSmartStrategy : SmartStrategy
    {
        public override IEnumerable<Command> Commands
        {
            get
            {
                while (true)
                {
                    if (Location != new Point(0, 0))
                        yield return new LongKillMoveCommand(new Point(0, 0));
                    else
                        yield return new HookAroundCommand();
                }
            }
        }

        public TrashSmartStrategy(PudgeSensorsData data, Graph graph) : base(data, graph) { }
    }
}
