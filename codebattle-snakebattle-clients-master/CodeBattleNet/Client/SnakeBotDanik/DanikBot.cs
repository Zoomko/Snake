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
    class DanikBot : IBot
    {
        BoardPoint head;
        TargetWindow window;
        SnakeAction prevAction = SnakeAction.Right;
        TiksTaskMaker taskMaker = new TiksTaskMaker();
        GameMap map = new GameMap();

        public static GameBoard Game;

        public DanikBot()
        {
            window = new TargetWindow(5);

            var barierSensor = new SensorParemetrizer(() => Game.GetBarriers(), (len) =>
            {
                if (len == 1)
                    return -100;
                return 0;
            });
            window.AddParametrizedSensor(barierSensor);

            window.AddSensor(BoardElement.Apple, (len) =>
            {
                if (len != 0)
                    return 20 / len;
                return 0;
            });

            window.AddSensor(BoardElement.Gold, (len) =>
            {
                if (len != 0)
                    return 50 / len;
                return 0;
            });

            window.AddSensor(BoardElement.FuryPill, (len) =>
            {
                if (len < 3)
                    return 50;
                return 0;
            });

            window.AddSensor(BoardElement.FlyingPill, (len) =>
            {
                if (len < 3)
                {
                    barierSensor.active = false;
                    taskMaker.AddTask(() => barierSensor.active = true, 10);
                    return 50;
                }
                return 0;
            });
        }

        public SnakeAction DoRun(GameBoard game)
        {
            Game = game;
            if (game.GetMyHead().HasValue)
                head = game.GetMyHead().Value;
            TargetWindow.Init(game, head);
            map.Update(game);
            var enemys = map.Enemys;
            
            Console.WriteLine(string.Join(" ", enemys.Select(v => v.Length)));
            MakeTaskLogick();
            var headCell = window.Update();
            Console.WriteLine(headCell);

            return CreateSnakeAktionForSensValue(headCell);
        }

        private void MakeTaskLogick()
        {
            taskMaker.Update();
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

    public class TiksTaskMaker
    {
        public long Tick { get; private set; }
        private List<TickTask> tasks = new List<TickTask>();

        public void AddTask(Action task, int time)
        {
            tasks.Add(new TickTask(task, Tick + time));
        }

        public void Update()
        {
            foreach(var task in tasks.ToList())
            {
                if(task.IsShouldActivate(Tick))
                {
                    tasks.Remove(task);
                    task.Action();
                }
            }
            Tick++;
        }

        public class TickTask
        {
            public Action Action;
            public long TimeActivate;

            public TickTask(Action action, long timeActivate)
            {
                Action = action;
                TimeActivate = timeActivate;
            }

            public bool IsShouldActivate(long curTime) => TimeActivate == curTime;
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

        List<SensorParemetrizer> sensors = new List<SensorParemetrizer>();
        SensValue head;
        SensValue[,] pole;

        public TargetWindow(int size)
        {
            this.size = size;
            pole = new SensValue[size, size];
            pole.ForEach(cell => cell = new SensValue());
            head = new SensValue();
        }

        public void AddSensor(BoardElement element, Func<int, int> activate)
        {
            AddParametrizedSensor(new SensorParemetrizer(() => Game.FindAllElements(element), activate));
        }

        public void AddSensor(Func<IEnumerable<BoardPoint>> getActivePoints, Func<int, int> activate)
        {
            AddParametrizedSensor(new SensorParemetrizer(getActivePoints, activate));
        }

        public void AddParametrizedSensor(SensorParemetrizer sensor)
        {
            sensor.Sensor = new Sensor(pole, sensor.Activate);
            sensors.Add(sensor);
        }

        public SensValue Update()
        {
            head = SensValue.Zero;
            foreach(var sensor in sensors)
            {
                if (sensor.active)
                {
                    var elements = sensor.GetActivePoints();
                    var sum = SensValue.Zero;
                    foreach (var el in elements.Select(ap => sensor.Sensor.ActivateAndGetValue(сenter, ap)))
                    {
                        sum += el;
                    }
                    head += sum;
                }
            }
 
            return head;
        }

        public class SensorParemetrizer
        {
            public bool active = true;
            public Func<IEnumerable<BoardPoint>> GetActivePoints;
            public Func<int, int> Activate;
            public Sensor Sensor;

            public SensorParemetrizer(
                Func<IEnumerable<BoardPoint>> getActivePoints,
                Func<int, int> activate
                )
            {
                GetActivePoints = getActivePoints;
                Activate = activate;
            }
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
                    return ActivateAndGetValue(center, point);
                }     
                return SensValue.Zero;
            }

            public SensValue ActivateAndGetValue(BoardPoint center, BoardPoint point)
            {
                var Weight = new SensValue();
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
