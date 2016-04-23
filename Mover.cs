using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Pudge;
using Pudge.Player;
using Geometry;
using Pudge.World;

namespace PudgeClient
{
    public class Mover
    {
        public const int maxCoord = 170;
        public const int minCoord = -maxCoord;
        public const double defaultWait = 0.1;
        public const double criticalAttackDistance = 20;
        const int step = 5;
        public bool hooked = false;
        private Graph graph;
        private PudgeSensorsData data;
        private PudgeClientLevel3 client;
        private Location Location => new Location(data.SelfLocation);
        private SeenNetwork seenNetwork = new SeenNetwork(minCoord, maxCoord, step, PudgeRules.Current.VisibilityRadius);
        private PathFinder pathFinder;
        private GraphUpdater graphUpdater;
        private Dictionary<string, Action<Command>> executeCommand;
        private SmartStrategy smartStrategy = null;

        public Mover(Graph graph, PudgeSensorsData data, PudgeClientLevel3 client)
        {
            pathFinder = new PathFinder(graph);
            graphUpdater = new GraphUpdater(graph);
            graphUpdater.Update(data);
            this.graph = graph;
            this.data = data;
            this.client = client;
            executeCommand = new Dictionary<string, Action<Command>>
            {
                { HookCommand.TypeName, x => ExecuteHook((x as HookCommand).Target) },
                { MoveCommand.TypeName, x => ExecuteMove((x as MoveCommand).Destination) },
                { WaitCommand.TypeName, x => ExecuteWait((x as WaitCommand).Time)},
                { LongMoveCommand.TypeName, x => ExecuteLongMove((x as LongMoveCommand).Destination)},
                { LongKillMoveCommand.TypeName, x => ExecuteLongKillMove((x as LongKillMoveCommand).Destination, out hooked)},
                { MoveAndReturnCommand.TypeName, x => ExecuteMoveAndReturn((x as MoveAndReturnCommand).Destination)},
                { HookAroundCommand.TypeName, x =>  ExecuteHookAround()},
                { MeetSlardarCommand.TypeName, x => ExecuteMeetSlardar((x as MeetSlardarCommand).Destination) }
            };
        }

        private void UpdateDataUnsafe(PudgeSensorsData data)
        {
            this.data = data;
            if (smartStrategy != null)
                smartStrategy.data = data;
            graphUpdater.Update(data);
        }

        public bool UpdateData(PudgeSensorsData data)
        {
            var ans = data.IsDead;
            if (data.IsDead)
                UpdateDataUnsafe(client.Wait(PudgeRules.Current.PudgeRespawnTime));
            else
                UpdateDataUnsafe(data);
            return ans;
        }

        public void Run(Strategy strategy)
        {
            if (strategy is SmartStrategy)
                smartStrategy = strategy as SmartStrategy;
            foreach (var command in strategy.Commands)
                ExecuteCommand(command);
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        void ExecuteCommand(Command command)
        {
            executeCommand[command.GetType().Name](command);
        }

        bool ExecuteWait(double time)
        {
            return UpdateData(client.Wait(time));
        }

        bool ExecuteMove(Point to)
        {
            if (UpdateData(client.Rotate(Location.GetTurnAngle(to))))
                return true;
            if (UpdateData(client.Move(Location.GetDistance(to))))
                return true;
            var rune = graph.TryGetRune(to);
            if (rune != null)
                rune.visited = true;
            return false;
        }

        bool UnderEffect(PudgeEvent e)
        {
            return data.Events.Select(x => x.Event).Contains(e);
        }

        bool ExecuteWaitEvent(PudgeEvent e)
        {
            while (UnderEffect(e))
                if (ExecuteWait(defaultWait))
                    return true;
            return false;
        }

        bool ExecuteHook(Point to)
        {
            if (UnderEffect(PudgeEvent.HookCooldown))
                return false;
            var angle = Location.GetTurnAngle(to);
            if (Math.Abs(angle) > 0.001)
                if (UpdateData(client.Rotate(angle)))
                    return true;
            if (UpdateData(client.Hook()))
                return true;
            return ExecuteWaitHook();
        }

        bool ExecuteHookAround()
        {
            while (true)
            {
                foreach (var hero in data.Map.Heroes)
                    return ExecuteHook(new Point(hero.Location.X, hero.Location.Y));
                if (ExecuteWait(defaultWait))
                    return true;
            }
        }

        bool ExecuteWaitHook()
        {
            return ExecuteWaitEvent(PudgeEvent.HookThrown);
        }

        bool ExecuteLongMove(Point to)
        {
            while (to != Location)
            {
                if (ExecuteMove(pathFinder.GetNextPoint(Location, to)))
                    return true;
            }
            return false;
        }

        bool ExecuteLongKillMove(Point to, out bool hooked)
        {
            hooked = this.hooked;
            while (to != Location)
            {
                if (ExecuteMove(pathFinder.GetNextPoint(Location, to)))
                    return true;
                if (UnderEffect(PudgeEvent.HookCooldown))
                    continue;
                var target = data.Map.Heroes
                    .Select(hero => new {Type = hero.Type.ToString(), Location = new Point(hero.Location.X, hero.Location.Y)} )
                    .ToList();
                hooked = target.Any();
                foreach (var pudge in target.Where(hero => hero.Type == "Pudge"))
                    if (ExecuteHook(pudge.Location))
                        return true;
                bool visible = !data.Events.Select(e => e.Event).Contains(PudgeEvent.Invisible);
                foreach (var slardar in target.Where(hero => hero.Type == "Slardar"))
                    if (visible || Location.GetDistance(slardar.Location) < criticalAttackDistance)
                        if (ExecuteHook(slardar.Location))
                            return true;
            }
            return false;
        }

        bool ExecuteMeetSlardar(Point meeting)
        {
            ExecuteLongKillMove(meeting, out hooked);
            if (hooked)
                return false;
            while (!data.Map.Heroes.Any())
                if (ExecuteWait(defaultWait))
                    return true;
            return ExecuteHookAround();
        }

        bool ExecuteMoveAndReturn(Point to)
        {
            var current = Location;
            if (ExecuteLongMove(to))
                return true;
            return ExecuteLongMove(current);
        }
    }
}
