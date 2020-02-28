using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.SnakeBotDanik
{
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

        public void AddSensor(BoardElement element, Func<SensData, int> activate)
        {
            AddParametrizedSensor(new SensorParemetrizer(() => Game.FindAllElements(element), activate));
        }

        public void AddSensor(Func<IEnumerable<BoardPoint>> getActivePoints, Func<SensData, int> activate)
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
            foreach (var sensor in sensors)
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
            public Func<SensData, int> Activate;
            public Sensor Sensor;

            public SensorParemetrizer(
                Func<IEnumerable<BoardPoint>> getActivePoints,
                Func<SensData, int> activate
                )
            {
                GetActivePoints = getActivePoints;
                Activate = activate;
            }
        }

        public class Sensor
        {
            public Func<SensData, int> Activate;
            public BoardElement Element;
            SensValue[,] pole;

            public Sensor(SensValue[,] pole, Func<SensData, int> activate)
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
                var sensData = new SensData();
                var shift = sensData.Shift = center - point;
                sensData.Point = point;
                sensData.Length = Math.Abs(sensData.Shift.X) + Math.Abs(sensData.Shift.Y);
                if (shift.X > 0)
                {
                    sensData.Direction = Direction.Left;
                    Weight.left = Activate(sensData);
                }
                else if (shift.X < 0) 
                {
                    sensData.Direction = Direction.Right;
                    Weight.right = Activate(sensData);
                }  
                if (shift.Y > 0)
                {
                    sensData.Direction = Direction.Up;
                    Weight.up = Activate(sensData);
                }
                else if (shift.Y < 0)
                {
                    sensData.Direction = Direction.Down;
                    Weight.down = Activate(sensData);
                }
                   

                return Weight;
            }
        }

        public class SensData
        {
            public BoardPoint Point;
            public BoardPoint Shift;
            public Direction Direction;
            public int Length;

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
            public Direction DirectionOfValue(int value)
            {
                if (value == up) return Direction.Up;
                if (value == right) return Direction.Right;
                if (value == left) return Direction.Left;
                if (value == down) return Direction.Down;
                return Direction.Stop;
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
