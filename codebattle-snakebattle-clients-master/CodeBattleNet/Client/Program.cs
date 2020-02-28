using System;
using System.Diagnostics;
using Client.SnakeBotDanik;
using SnakeBattle.Api;

namespace Client
{
    class Program
    {
        //const string SERVER_ADDRESS = "http://epruizhsa0001t2:8080/codenjoy-contest/board/player/4ol1pue9vqijd518yjlb?code=1180389975885916399&gameName=snakebattle";
        const string SERVER_ADDRESS = "http://codingdojo2020.westeurope.cloudapp.azure.com/codenjoy-contest/board/player/lc35j4dwfll6a8i9jcww?code=6398271465899362279&gameName=snakebattle";
        //const string SERVER_ADDRESS = "ws://codenjoy.com:80/codenjoy-contest/ws?user=3edq63tw0bq4w4iem7nb&code=12345678901234567890";

        static void Main(string[] args)
        {
            var client = new SnakeBattleClient(SERVER_ADDRESS);
            client.Run(DoRun);

            Console.ReadKey();
            client.InitiateExit();
        }

        private static IBot bot = new DanikBot();
        private static Stopwatch stopwatch = new Stopwatch();

        private static SnakeAction DoRun(GameBoard gameBoard)
        {
            stopwatch.Restart();
            var act=  bot.DoRun(gameBoard);
            stopwatch.Stop();
            Console.WriteLine( $"timetick:{stopwatch.Elapsed}");
            return act;
        }
    }
}