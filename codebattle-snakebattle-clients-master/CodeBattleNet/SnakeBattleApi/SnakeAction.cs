namespace SnakeBattle.Api
{
    public class SnakeAction
    {
        public static SnakeAction Down = new SnakeAction(false, Direction.Down);
        public static SnakeAction Left = new SnakeAction(false, Direction.Left);
        public static SnakeAction Right = new SnakeAction(false, Direction.Right);
        public static SnakeAction Up = new SnakeAction(false, Direction.Up);
        public static SnakeAction Stop = new SnakeAction(false, Direction.Stop);

        private const string ACT_COMMAND_PREFIX = "ACT,";

        private readonly bool _act;
        private readonly Direction _direction;

        public SnakeAction(bool act, Direction direction)
        {
            _act = act;
            _direction = direction;
        }

        public override string ToString()
        {
            var cmd = _act ? ACT_COMMAND_PREFIX : string.Empty;
            return cmd + _direction;
        }
    }
}
