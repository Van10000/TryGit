using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PudgeClient
{
    public abstract class Strategy
    {
        public abstract IEnumerable<Command> Commands { get; }
    }
}
