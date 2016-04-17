using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace PudgeClient
{
    public class MoveAndReturnCommand : MoveCommand
    {
        public new const string TypeName = "MoveAndReturnCommand";

        public MoveAndReturnCommand(Point destination) : base(destination) { }
    }
}
