using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class VectorFieldBot : IBot
    {
        public SnakeAction DoRun(GameBoard game)
        {
            return new SnakeAction(true, Direction.Down);
        }
    }
}
