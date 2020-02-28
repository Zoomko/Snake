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
    class DanikBot : IBot
    {
        BoardPoint head;
        TargetWindow window;
        SnakeAction prevAction = SnakeAction.Right;
        TiksTaskMaker taskMaker = new TiksTaskMaker();
        GameMap map = new GameMap();

        ApplesMode ApplesMode = new ApplesMode();

        public static GameBoard Game;

        public DanikBot()
        {
            
        }

        public SnakeAction DoRun(GameBoard game)
        {
            var sensValue = ApplesMode.Update(game, map);
            return CreateSnakeAktionForSensValue(sensValue);
        }

        public SnakeAction CreateSnakeAktionForSensValue(TargetWindow.SensValue sensValue)
        {
            var list = new List<int>()
            {
                sensValue.left,
                sensValue.down,
                sensValue.right,
                sensValue.up
            };

            list.Sort();
            list.Reverse();
            var action = new SnakeAction(false, sensValue.DirectionOfValue(list[0]));
            if (action.IsOpposite(prevAction))
            {
                prevAction = action;
                action = new SnakeAction(false, sensValue.DirectionOfValue(list[1]));
            }
            if (action.IsOpposite(prevAction))
            {
                prevAction = action;
                action = new SnakeAction(false, sensValue.DirectionOfValue(list[2]));
            }
            if (action.IsOpposite(prevAction))
            {
                prevAction = action;
                action = new SnakeAction(false, sensValue.DirectionOfValue(list[3]));
            }
            prevAction = action;
            return action;
        }
    }
}
