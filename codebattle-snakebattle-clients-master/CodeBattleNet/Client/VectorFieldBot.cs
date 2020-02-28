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

            var left = 0.0;
            var right = 0.0;
            var up = 0.0;
            var down = 0.0;

            try
            {
                left = store[head.X - 1, head.Y].Weight;
                right = store[head.X + 1, head.Y].Weight;
                up = store[head.X, head.Y - 1].Weight;
                down = store[head.X, head.Y + 1].Weight;
            }
            catch { };
            

            var max = Max(left, right, up, down);

            Console.WriteLine($"{right} {left} {up} {down}  max: {max}");

            if (down == max) action = SnakeAction.Down;
            else if (right == max) action = SnakeAction.Right;
            else if (up == max) action = SnakeAction.Up;
            else if (left == max) action = SnakeAction.Left;

            return action;
        }

        double Max(params double[] compare)
        {
            return compare.Max();
        }
    }
}
