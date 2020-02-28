using Client.Algorithm;
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
        SnakeAction action;
        BoardPoint head;
        public SnakeAction DoRun(GameBoard game)
        {
            new StoreOfPieces(game.Size, game);
            var store = StoreOfPieces.store;
            head =  game.GetMyHead().Value;

            var left = store[head.X-1, head.Y].Weight;
            var right = store[head.X + 1, head.Y].Weight;
            var up = store[head.X, head.Y - 1].Weight;
            var down = store[head.X, head.Y + 1].Weight;

            var max = Max(left, right, up, down);

            Console.Write($"{right} {left} {up} {down}  max: {max}");

            if (down == max) action = SnakeActionHelper.Down;
            else if (right == max) action = SnakeActionHelper.Right;
            else if (up == max) action = SnakeActionHelper.Up;
            else if (left == max) action = SnakeActionHelper.Left;

            return action;
        }

        double Max(params double[] compare)
        {
            return compare.Max();
        }
    }
}
