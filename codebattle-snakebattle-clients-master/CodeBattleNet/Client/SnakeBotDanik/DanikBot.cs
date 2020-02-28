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

        public DanikBot()
        {
            window = new TargetWindow(5);
            window.AddSensor(BoardElement.Apple, (len) =>
            {
                if(len != 0)
                    return 20 / len;
                return 0;
            });
            window.AddSensor(BoardElement.Wall, (len) =>
                {
                    if (len == 1)
                        return -100;
                    else
                        return 0;
                });
        }

        public SnakeAction DoRun(GameBoard game)
        {
            if(game.GetMyHead().HasValue)
                head = game.GetMyHead().Value;
            TargetWindow.Init(game, head);
            var headCell = window.Update();
            Console.WriteLine(headCell);
            return CreateSnakeAktionForSensValue(headCell);
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
            var action = SelectActionWithSensValue(sensValue, list[0]);
            if (action.IsOpposite(prevAction))
                action = SelectActionWithSensValue(sensValue, list[1]);
            prevAction = action;
            return action;
        }

        public SnakeAction SelectActionWithSensValue(SensValue sensValue, int value)
        {
            if (value == sensValue.up) return SnakeAction.Up;
            if (value == sensValue.right) return SnakeAction.Right;
            if (value == sensValue.left) return SnakeAction.Left;
            if (value == sensValue.down) return SnakeAction.Down;
            return SnakeAction.Stop;
        }
    }

    public class TargetWindow
    {
        public static GameBoard Game;

        public static void Init(GameBoard game, BoardPoint head)
        {
            Game = game;
            сenter = head;
        }

        private int size;
        private static BoardPoint сenter;

        Dictionary<BoardElement, Sensor> sensors = new Dictionary<BoardElement, Sensor>();
        SensValue head;
        SensValue[,] pole;

        public TargetWindow(int size)
        {
            this.size = size;
            pole = new SensValue[size, size];
            pole.ForEach(cell => cell = new SensValue());
            head = new SensValue();
        }

        public void AddSensor(BoardElement boardElement, Func<int, int> activate)
        {
            sensors[boardElement] = new Sensor(pole, activate) { Element = boardElement };
        }

        public SensValue Update()
        {
            head = SensValue.Zero;
            foreach(var sensor in sensors)
            {
                var apples = Game.FindAllElements(sensor.Key);
                var sum = SensValue.Zero;
                foreach(var el in apples.Select(ap => sensor.Value.GetValue(сenter, ap)))
                {
                    sum += el;
                }
                head += sum;
            }
 
            return head;
        }
        public class Sensor
        { 
            public Func<int, int> Activate;
            public BoardElement Element;
            SensValue[,] pole;

            public Sensor(SensValue[,] pole, Func<int, int> activate)
            {
                this.pole = pole;
                Activate = activate;
            }

            public bool IsActiv(BoardPoint point)
            {
                return Game.HasElementAt(point, Element);
            }  
            
            public SensValue GetValue(BoardPoint center, BoardPoint point)
            {
                var Weight = new SensValue();
                if (IsActiv(point))
                {
                    var shift = center - point;
                    var len = Math.Abs(shift.X) + Math.Abs(shift.Y);
                    if (shift.X > 0)
                        Weight.left = Activate(len);
                    else if (shift.X < 0)
                        Weight.right = Activate(len);
                    if (shift.Y > 0)
                        Weight.up = Activate(len);
                    else if (shift.Y < 0)
                        Weight.down = Activate(len);

                    return Weight;
                }     
                return SensValue.Zero;
            }
        }

        public class SensValue
        {
            public static SensValue Zero = new SensValue();

            public int left;
            public int up;
            public int right;
            public int down;

            public SensValue() { }
            public SensValue(int left, int up, int right, int down)
            {
                this.down = down;
                this.up = up;
                this.left = left;
                this.right = right;
            }

            public static SensValue operator +(SensValue a, SensValue b)
            {
                return new SensValue(a.left + b.left, a.up + b.up, a.right + b.right, a.down + b.down);
            }

            public override string ToString()
            {
                return $"{left} {up} {right} {down}";
            }
        }
    }
}
