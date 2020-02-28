using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    class SnakeInfo
    {
        public static HashSet<BoardElement> enemyHead = new HashSet<BoardElement>
        {
            BoardElement.EnemyHeadDown,
            BoardElement.EnemyHeadLeft,
            BoardElement.EnemyHeadUp,
            BoardElement.EnemyHeadRight,
            BoardElement.EnemyHeadDead,
            BoardElement.EnemyHeadEvil,
            BoardElement.EnemyHeadFly
        };

        public static HashSet<BoardElement> enemyBody = new HashSet<BoardElement>
        {
            BoardElement.EnemyBodyHorizontal,
            BoardElement.EnemyBodyLeftDown,
            BoardElement.EnemyBodyLeftUp,
            BoardElement.EnemyBodyRightDown,
            BoardElement.EnemyBodyRightUp,
            BoardElement.EnemyBodyVertical
        };


        public HashSet<BoardPoint> bodyPoints = new HashSet<BoardPoint>();
        public int Length;
        public Direction Direction;
        public BoardPoint Head;

        public SnakeInfo(GameBoard gameBoard, BoardPoint head)
        {
            SnakeInit(gameBoard, head);
        }

        public void SnakeInit(GameBoard gameBoard, BoardPoint head)
        {
            Head = head;
            var body = Head;
            Length = 1;
            while (true)
            {
                bodyPoints.Add(body);
                Length++;
                var down = body.ShiftBottom();
                var up = body.ShiftTop();
                var left = body.ShiftLeft();
                var right = body.ShiftRight();
                if (gameBoard.HasElementAt(down,
                    BoardElement.EnemyBodyVertical,
                    BoardElement.EnemyBodyLeftUp,
                    BoardElement.EnemyBodyRightUp))
                    body = down;
                else if (gameBoard.HasElementAt(up,
                    BoardElement.EnemyBodyVertical,
                    BoardElement.EnemyBodyRightDown,
                    BoardElement.EnemyBodyLeftDown))
                    body = up;
                else if (gameBoard.HasElementAt(left,
                    BoardElement.EnemyBodyHorizontal,
                    BoardElement.EnemyBodyLeftDown,
                    BoardElement.EnemyBodyLeftUp))
                    body = left;
                else if (gameBoard.HasElementAt(right,
                    BoardElement.EnemyBodyHorizontal,
                    BoardElement.EnemyBodyRightDown,
                    BoardElement.EnemyBodyRightUp))
                    body = left;
                else
                    break;
            }
        }
    }
}
