using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace PudgeClient
{
    class OneSlardarStrategy : Strategy
    {
        public override IEnumerable<Command> Commands
        {
            get
            {
                yield return new MoveCommand(new Point(-130, -10));
                yield return new HookCommand(new Point(-130, 50));
                yield return new MoveCommand(new Point(-130, 30));
                var hooked = true;
                var counter = 1;
                Point[] runes = { new Point(-120, -70), new Point(0, 0), new Point(-130, 130) };
                while (true)
                {
                    if (hooked)
                    {
                        yield return new MoveAndReturnCommand(runes[counter % 3]);
                        hooked = false;
                    }
                    yield return new HookAroundCommand();
                    counter++;
                    hooked = true;
                }
            }
        }
    }
}
