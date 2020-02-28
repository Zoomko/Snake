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

        public static HashSet<BoardElement> EnemyElements = new HashSet<BoardElement>
        {
            BoardElement.EnemyHeadDown,
            BoardElement.EnemyHeadLeft,
            BoardElement.EnemyHeadUp,
            BoardElement.EnemyHeadRight,
            BoardElement.EnemyHeadDead,
            BoardElement.EnemyHeadEvil,
            BoardElement.EnemyHeadFly,
            BoardElement.EnemyBodyHorizontal,
            BoardElement.EnemyBodyLeftDown,
            BoardElement.EnemyBodyLeftUp,
            BoardElement.EnemyBodyRightDown,
            BoardElement.EnemyBodyRightUp,
            BoardElement.EnemyBodyVertical,
            BoardElement.EnemyTailEndDown,
            BoardElement.EnemyTailEndLeft,
            BoardElement.EnemyTailEndRight,
            BoardElement.EnemyTailEndUp,
            BoardElement.EnemyTailInactive
        };

        public static HashSet<BoardElement> EnemyHead = new HashSet<BoardElement>
        {
            BoardElement.EnemyHeadDown,
            BoardElement.EnemyHeadLeft,
            BoardElement.EnemyHeadUp,
            BoardElement.EnemyHeadRight,
            BoardElement.EnemyHeadDead,
            BoardElement.EnemyHeadEvil,
            BoardElement.EnemyHeadFly
        };

        public static HashSet<BoardElement> EnemyBody = new HashSet<BoardElement>
        {
            BoardElement.EnemyBodyHorizontal,
            BoardElement.EnemyBodyLeftDown,
            BoardElement.EnemyBodyLeftUp,
            BoardElement.EnemyBodyRightDown,
            BoardElement.EnemyBodyRightUp,
            BoardElement.EnemyBodyVertical
        };

        public static HashSet<BoardElement> EnemyTail = new HashSet<BoardElement>
        {
            BoardElement.EnemyTailEndDown,
            BoardElement.EnemyTailEndLeft,
            BoardElement.EnemyTailEndRight,
            BoardElement.EnemyTailEndUp,
            BoardElement.EnemyTailInactive
        };

        public static HashSet<BoardElement> HeroElements = new HashSet<BoardElement>
        {
            BoardElement.BodyHorizontal,
            BoardElement.BodyLeftDown,
            BoardElement.BodyLeftUp,
            BoardElement.BodyRightDown,
            BoardElement.BodyRightUp,
            BoardElement.BodyVertical,
            BoardElement.TailEndDown,
            BoardElement.TailEndLeft,
            BoardElement.TailEndRight,
            BoardElement.TailEndUp,
            BoardElement.TailInactive,
            BoardElement.HeadDead,
            BoardElement.HeadDown,
            BoardElement.HeadEvil,
            BoardElement.HeadFly,
            BoardElement.HeadLeft,
            BoardElement.HeadRight,
            BoardElement.HeadSleep,
            BoardElement.HeadUp
        };

        public static HashSet<BoardElement> HeroHead = new HashSet<BoardElement>
        {
            BoardElement.HeadDead,
            BoardElement.HeadDown,
            BoardElement.HeadEvil,
            BoardElement.HeadFly,
            BoardElement.HeadLeft,
            BoardElement.HeadRight,
            BoardElement.HeadSleep,
            BoardElement.HeadUp
        };

        public static HashSet<BoardElement> HeroBody = new HashSet<BoardElement>
        {
            BoardElement.BodyHorizontal,
            BoardElement.BodyLeftDown,
            BoardElement.BodyLeftUp,
            BoardElement.BodyRightDown,
            BoardElement.BodyRightUp,
            BoardElement.BodyVertical
        };

        public static HashSet<BoardElement> HeroTail = new HashSet<BoardElement>
        { 
            BoardElement.TailEndDown,
            BoardElement.TailEndLeft,
            BoardElement.TailEndRight,
            BoardElement.TailEndUp,
            BoardElement.TailInactive
        };


        public HashSet<BoardPoint> bodyPoints = new HashSet<BoardPoint>();
        public int Length => bodyPoints.Count();
        public Direction Direction;
        public BoardPoint Head;
        public BoardPoint Tail;

        public SnakeInfo(GameBoard gameBoard, BoardPoint head)
        {
            SnakeInit(gameBoard, head);
        }

        public void SnakeInit(GameBoard gameBoard, BoardPoint head)
        {
            Head = head;
            var body = head;
            if (gameBoard.HasElementAt(Tail, EnemyTail))
                body = Tail;
            else
                bodyPoints.Clear();
            while (true)
            {
                bodyPoints.Add(body);
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
