using System;
using Geometry;

namespace PudgeClient
{
    public class HookCommand : Command
    {
        public Point Target { get; private set; }
        public const string TypeName = "HookCommand";

        public HookCommand(Point target)
        {
            Target = target;
        }
    }
}
