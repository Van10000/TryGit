using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace PudgeClient
{
    public class LongMoveCommand : MoveCommand
    {
        public new const string TypeName = "LongMoveCommand";

        public LongMoveCommand(Point destination) : base(destination) { }
    }
}
