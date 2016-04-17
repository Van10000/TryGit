using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace PudgeClient
{
    class ThreeSlardarsStrategy : Strategy
    {
        public override IEnumerable<Command> Commands
        {
            get
            {
                yield return new MoveCommand(new Point(-130, -10));
                yield return new HookCommand(new Point(-130, 50));
                while (true)
                {
                    yield return new MoveCommand(new Point(40, -40));
                    yield return new HookAroundCommand();
                    yield return new MoveCommand(new Point(-40, 40));
                    yield return new HookAroundCommand();
                }
            }
        }
    }
}
