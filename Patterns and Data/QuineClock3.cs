using System;
using System.Text;

namespace QuineClock3
{
    class Program
    {
        static string[] Digits = new [] { /*%Digits*/null/*Digits%*/ };

        /*#QuineClock3*//*QuineClock3#*/

        static void Main()
        {
            var ticksPerSecond = TimeSpan.TicksPerSecond;
            var n = DateTime.Now;
            var nextDate = n.AddTicks(ticksPerSecond - (n.Ticks % ticksPerSecond));
            var sleepMs = (nextDate.Ticks - n.Ticks) * 1000 / ticksPerSecond - 50;
            if (sleepMs < 0)
                sleepMs = 0;
            System.Threading.Thread.Sleep((int)sleepMs);
            var output = TimeToString(nextDate);
            /*@*/
        }
    }
}
/*$QuineClockOutput$*/