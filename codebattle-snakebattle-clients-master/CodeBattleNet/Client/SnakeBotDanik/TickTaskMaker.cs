using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.SnakeBotDanik
{
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
            foreach (var task in tasks.ToList())
            {
                if (task.IsShouldActivate(Tick))
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
}
