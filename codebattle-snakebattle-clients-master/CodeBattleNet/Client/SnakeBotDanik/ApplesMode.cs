using Client.Helpers;
using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Client.SnakeBotDanik.TargetWindow;

namespace Client.SnakeBotDanik
{
    class ApplesMode : IMode
    {
        TargetWindow window = new TargetWindow(5);
        TiksTaskMaker taskMaker = new TiksTaskMaker();
        GameMap map = new GameMap();
        GameBoard game;

        public ApplesMode()
        {
            window = new TargetWindow(5);

            window.AddSensor(() => Game.GetBarriers(), (data) =>
            {
                if (data.Length == 1)
                    return -100;
                return 0;
            });

            window.AddSensor(BoardElement.Apple, (data) =>
            {
                if (data.Length != 0)
                    return 20 / data.Length;
                return 0;
            });

            window.AddSensor(BoardElement.Gold, (data) =>
            {
                if (data.Length != 0)
                    return 50 / data.Length;
                return 0;
            });

            //window.AddSensor(BoardElement.FuryPill, (data) =>
            //{
            //    if (data.Length < 3)
            //        return 50;
            //    return 0;
            //});

            //window.AddSensor(BoardElement.FlyingPill, (data) =>
            //{
            //    if (data.Length < 3)
            //    {
            //        barierSensor.active = false;
            //        taskMaker.AddTask(() => barierSensor.active = true, 10);
            //        return 50;
            //    }
            //    return 0;
            //});

            //window.AddSensor(() => game.FindAllElements(SnakeInfo.enemyBody), (data) =>
            //{
            //    if(data.Length < 2)
            //    {
            //        return -50;
            //    }
            //    return 0;
            //});
        }

        BoardPoint head = new BoardPoint();

        public SensValue Update(GameBoard game, GameMap map)
        {
            this.game = game;
            if (game.GetMyHead().HasValue)
                head = game.GetMyHead().Value;
            TargetWindow.Init(game, head);

            //map.Update(game);
            //var enemys = map.Enemys;
            //Console.WriteLine(string.Join(" ", enemys.Select(v => v.Length)));

            taskMaker.Update();
            
            var headCell = window.Update();
            Console.WriteLine(headCell);

            return window.Update();
        }
    }
}
