using System;
using System.Collections.Generic;
using System.Linq;
using Geometry;

namespace PudgeClient
{
    class TwoSlardarsSrategy : Strategy
    {
        public override IEnumerable<Command> Commands
        {
            get
            {
                yield return new MoveCommand(new Point(-130, -10));
                yield return new HookCommand(new Point(-130, 50));
                yield return new LongMoveCommand(new Point(-130, 130));
                //var counter = 0;
                //Point[] runes = {new Point(-130, 130), new Point(0, 0), new Point(130, -130), new Point(0, 0)};
                while (true)
                {
                    yield return new MeetSlardarCommand(new Point(-130, 10));
                    yield return new LongMoveCommand(new Point(0, 0));
                    yield return new LongKillMoveCommand(new Point(130, -130));
                    yield return new MeetSlardarCommand(new Point(110, -50));
                    yield return new LongMoveCommand(new Point(120, 70));
                    yield return new LongMoveCommand(new Point(70, 120));
                    yield return new LongKillMoveCommand(new Point(-130, 130));
                    //counter++;
                }
            }
        }
    }
}
