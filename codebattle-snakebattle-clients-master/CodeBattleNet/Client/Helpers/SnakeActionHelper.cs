using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class SnakeActionHelper
    {
        public static SnakeAction Down = new SnakeAction(false, Direction.Down);
        public static SnakeAction Left = new SnakeAction(false, Direction.Left);
        public static SnakeAction Right = new SnakeAction(false, Direction.Right);
        public static SnakeAction Up = new SnakeAction(false, Direction.Up);
        public static SnakeAction Stop = new SnakeAction(false, Direction.Stop);

    }
}
