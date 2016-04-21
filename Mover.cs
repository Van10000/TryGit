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
        public const int pathSplitPiecesCount = 4;
        const int step = 5;
        public bool hooked = false;
        private int counter = 0;
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

        public void UpdateData(PudgeSensorsData data)
        {
            if (data.IsDead)
                UpdateDataUnsafe(client.Wait(PudgeRules.Current.PudgeRespawnTime));
            else
                UpdateDataUnsafe(data);
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

        void ExecuteWait(double time)
        {
            UpdateData(client.Wait(time));
        }

        void ExecuteMove(Point to)
        {
            client.Rotate(Location.GetTurnAngle(to));
            UpdateData((client.Move(Location.GetDistance(to))));
            var rune = graph.TryGetRune(to);
            if (!Object.ReferenceEquals(rune, null))
                rune.visited = true;
        }

        void ExecuteHook(Point to)
        {
            var angle = Location.GetTurnAngle(to);
            if (Math.Abs(angle) > 0.001)
                UpdateData(client.Rotate(angle));
            UpdateData(client.Hook());
            ExecuteWaitHook();
        }

        void ExecuteHookAround()
        {
            while (true)
            {
                while (data.Events.Select(x => x.Event).Contains(PudgeEvent.HookCooldown))
                    ExecuteWait(defaultWait);
                foreach (var hero in data.Map.Heroes)
                {
                    ExecuteHook(new Point(hero.Location.X, hero.Location.Y));
                    return;
                }
                ExecuteWait(defaultWait);
            }
        }

        void ExecuteWaitHook()
        {
            while (data.Events.Select(x => x.Event).Contains(PudgeEvent.HookThrown))
                ExecuteWait(defaultWait);
        }

        void ExecuteLongMove(Point to)
        {
            while (to != Location)
            {
                ExecuteMove(pathFinder.GetNextPoint(Location, to));
            }
        }

        void ExecuteLongKillMove(Point to, out bool hooked)
        {
            hooked = this.hooked;
            while (to != Location)
            {
                ExecuteMove(pathFinder.GetNextPoint(Location, to));
                var target = data.Map.Heroes.GroupBy(hero => hero.Type.ToString())
                                            .Select(group => new { Type = group.Key, Location = group
                                                                           .Select(hero => new Point(hero.Location.X, hero.Location.Y))
                                                                           .ToList()
                                                                           .First()})
                                            .ToList();
                if (target.Any())
                {
                    foreach (var pudge in target.Where(hero => hero.Type == "Pudge"))
                        ExecuteHook(pudge.Location);
                    foreach (var slardar in target.Where(hero => hero.Type == "Slardar"))
                        ExecuteHook(slardar.Location);
                    hooked = true;
                }
            }
        }

        void ExecuteMeetSlardar(Point meeting)
        {
            ExecuteLongKillMove(meeting, out hooked);
            if (hooked)
                return;
            while (!data.Map.Heroes.Any())
                ExecuteWait(defaultWait);
            ExecuteHookAround();
        }

        void ExecuteMoveAndReturn(Point to)
        {
            var current = Location;
            ExecuteLongMove(to);
            ExecuteLongMove(current);
        }
    }
}
