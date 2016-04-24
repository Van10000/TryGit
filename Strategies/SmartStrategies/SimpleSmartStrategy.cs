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
                //yield return new MoveCommand(new Point(-40, -40));
                //yield return new HookAroundCommand(10);
                //yield return new MoveCommand(new Point(0, 0));
                while (true)
                {
                    var closestRune = GetClosestRune();
                    if (closestRune != null)
                        yield return new LongKillMoveCommand(GetClosestRune());
                    else if (Location != new Point(0, 0))
                        yield return new LongKillMoveCommand(new Point(0, 0));
                    else
                        yield return new WaitCommand(0.1);
                }
            }
        }

        public SimpleSmartStrategy(PudgeSensorsData data, Graph graph) : base(data, graph) { }

    }
}
