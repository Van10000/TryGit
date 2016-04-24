using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PudgeClient
{
    public class HookAroundCommand : Command
    {
        public int IterCount;
        public const string TypeName = "HookAroundCommand";

        public HookAroundCommand(int IterCount = int.MaxValue)
        {
            this.IterCount = IterCount;
        }
    }
}
