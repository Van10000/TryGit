using System;
using Pudge;
using Pudge.Player;
using System.IO;
using Newtonsoft.Json;
using System.Timers;

namespace PudgeClient
{
    class Program
    {
        const string CvarcTag = "69a2c91f-a99f-46b7-b5de-f439becd3190";

        // Пример визуального отображения данных с сенсоров при отладке.
        // Если какая-то информация кажется вам лишней, можете закомментировать что-нибудь.
        static void Print(PudgeSensorsData data)
        {
            Console.WriteLine("---------------------------------");
            if (data.IsDead)
            {
                // Правильное обращение со смертью.
                Console.WriteLine("Ooops, i'm dead :(");
                return;
            }
            Console.WriteLine("I'm here: " + data.SelfLocation);
            Console.WriteLine("My score now: {0}", data.SelfScores);
            Console.WriteLine("Current time: {0:F}", data.WorldTime);
            foreach (var rune in data.Map.Runes)
                Console.WriteLine("Rune! Type: {0}, Size = {1}, Location: {2}", rune.Type, rune.Size, rune.Location);
            foreach (var heroData in data.Map.Heroes)
                Console.WriteLine("Enemy! Type: {0}, Location: {1}, Angle: {2:F}", heroData.Type, heroData.Location, heroData.Angle); 
            foreach (var eventData in data.Events)
                Console.WriteLine("I'm under effect: {0}, Duration: {1}", eventData.Event,
                    eventData.Duration - (data.WorldTime - eventData.Start));
            Console.WriteLine("---------------------------------");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
                //args = new[] { "87.224.245.130", "14001" };
                args = new[] {"127.0.0.1", "14000"};
            var ip = args[0];
            var port = int.Parse(args[1]);

            // Каждую неделю клиент будет новый. Соотетственно Level1, Level2 и Level3.
            var client = new PudgeClientLevel2();

            // У этого метода так же есть необязательные аргументы:
            // timeLimit -- время в секундах, сколько будет идти матч (по умолчанию 90)
            // operationalTimeLimit -- время в секундах, отображающее ваш лимит на операции в сумме за всю игру
            // По умолчанию -- 1000. На турнире будет использоваться значение 5. Подробнее про это можно прочитать в правилах.
            // isOnLeftSide -- предпочитаемая сторона. Принимается во внимание во время отладки. По умолчанию true.
            // seed -- источник энтропии для случайного появления рун. По умолчанию -- 0. 
            // При изменении руны будут появляться в другом порядке
            // speedUp -- ускорение отладки в два раза. Может вызывать снижение FPS на слабых машинах

            var graph = JsonConvert.DeserializeObject<Graph>(string.Join("", File.ReadAllLines("graph.json")));
            var data = client.Configurate(ip, port, CvarcTag);
            //client.SensorDataReceived += Print;
            var mover = new Mover(graph, data, client);
            mover.Run(new SimpleSmartStrategy(data, graph));
            client.Exit();
        }
    }
}
