using Geometry;

namespace PudgeClient
{ 
    public class LongKillMoveCommand : MoveCommand
    {
        public new const string TypeName = "LongKillMoveCommand";

        public LongKillMoveCommand(Point destination) : base(destination) { }
    }
}
