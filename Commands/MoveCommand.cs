using System;
using System.Collections.Generic;
using System.Linq;
using Geometry;

namespace PudgeClient
{
    public class MoveCommand : Command
    {
        public Point Destination { get; private set; }
        public const string TypeName = "MoveCommand";

        public MoveCommand(Point destination)
        {
            Destination = destination;
        }
    }
}
