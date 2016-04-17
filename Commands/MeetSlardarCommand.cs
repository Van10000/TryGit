using Geometry;

namespace PudgeClient
{
    class MeetSlardarCommand : MoveCommand
    {
        public new const string TypeName = "MeetSlardarCommand";

        public MeetSlardarCommand(Point destination) : base(destination) { }
    }
}
