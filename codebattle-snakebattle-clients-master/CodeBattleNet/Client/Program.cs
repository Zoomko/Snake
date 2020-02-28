using System;
using Client.SnakeBotDanik;
using SnakeBattle.Api;

namespace Client
{
    class Program
    {
        //const string SERVER_ADDRESS = "http://epruizhsa0001t2:8080/codenjoy-contest/board/player/4ol1pue9vqijd518yjlb?code=1180389975885916399&gameName=snakebattle";
        const string SERVER_ADDRESS = "http://codingdojo2020.westeurope.cloudapp.azure.com/codenjoy-contest/board/player/lc35j4dwfll6a8i9jcww?code=6398271465899362279&gameName=snakebattle";

        static void Main(string[] args)
        {
            var client = new SnakeBattleClient(SERVER_ADDRESS);
            client.Run(DoRun);

            Console.ReadKey();
            client.InitiateExit();
        }

        private static IBot bot = new DanikBot();

        private static SnakeAction DoRun(GameBoard gameBoard)
        {
            return bot.DoRun(gameBoard);
        }
    }
}