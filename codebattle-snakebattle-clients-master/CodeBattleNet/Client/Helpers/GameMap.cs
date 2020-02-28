using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    class GameMap
    {
        Dictionary<BoardPoint, SnakeInfo> Enemy = new Dictionary<BoardPoint, SnakeInfo>();
        public List<SnakeInfo> Enemys = new List<SnakeInfo>();
        GameBoard GameBoard;
        public void Update(GameBoard gameBoard)
        {
            GameBoard = gameBoard;
            var headPoints = gameBoard.FindAllElements(SnakeInfo.EnemyHead.ToArray());

            Enemys = headPoints.Select(point =>
            {
                if (Enemy.TryGetValue(point, out SnakeInfo snake))
                {
                    snake.SnakeInit(GameBoard, point);
                    return snake;
                }
                return new SnakeInfo(GameBoard, point);
            }).ToList();

            Enemys = Enemys.OrderBy(v => v.Length).ToList();

            foreach(var snake in Enemys)
                foreach (var pointBody in snake.bodyPoints)
                    Enemy[pointBody] = snake;
            
        }

        public SnakeInfo GetEnemySnakeInfo(BoardPoint point)
        {
            if(!GameBoard.HasElementAt(point, SnakeInfo.EnemyBody) 
                && !GameBoard.HasElementAt(point, SnakeInfo.EnemyHead))
            {
                Enemy.Remove(point);
                return null;
            }
            if (Enemy.TryGetValue(point, out SnakeInfo snake))
            {
                return snake;
            }
            return null;
        }
    }
}
