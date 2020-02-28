using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    interface IBot
    {
        SnakeAction DoRun(GameBoard game);
    }
}
