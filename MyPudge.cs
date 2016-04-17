using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pudge;
using Pudge.Player;

namespace PudgeClient
{
    class MyPudge
    {
        double hasteStart = -1;
        double hasteDuration;
        bool Haste => hasteStart != -1;
        double inviseStart = -1;
        double inviseDuration;
        bool Invise => inviseStart != -1;
        MyPudge(PudgeSensorsData data)
        {
            foreach (var effect in data.Events)
            {
                if (effect.ToString() == "Hasted")
                {
                    hasteStart = effect.Start;
                    hasteDuration = effect.Duration;
                }
                if (effect.ToString() == "Invisible")
                {
                    inviseStart = effect.Start;
                    inviseDuration = effect.Duration;
                }
            }
        }
    }
}
