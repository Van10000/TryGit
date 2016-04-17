using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PudgeClient
{
    public class WaitCommand : Command
    {
        public double Time { get; private set; }
        public const string TypeName = "WaitCommand";

        public WaitCommand(double time)
        {
            Time = time;
        }
    }
}
