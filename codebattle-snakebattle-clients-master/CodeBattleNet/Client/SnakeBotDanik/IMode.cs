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
    interface IMode
    {
        SensValue Update(GameBoard game, GameMap map);
    }
}
