using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    static class Helper
    {
        public static T Max<T>(params T[] compare)
        {
            return compare.Max();
        }

        public static void ForEach<T>(this T[,] ts, Action<T> action)
        {
            foreach (var t in ts)
            {
                action(t);
            }
        }
    }
}
