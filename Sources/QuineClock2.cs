using System;
using System.Threading;
namespace QuineClock
{
    class Program
    {
        static void Main()
        {
            var ticksPerSecond = TimeSpan.TicksPerSecond;
            var n = DateTime.Now;
            var nextDate = n.AddTicks(ticksPerSecond - (n.Ticks % ticksPerSecond));
            var sleepTicks = nextDate.Ticks - n.Ticks;
            Thread.Sleep((int)(sleepTicks * 1000 / ticksPerSecond));
            var output = "\r\n\r\n//    " + nextDate.ToString("HH:mm:ss") + ":" + nextDate.Millisecond + " " + n.ToString("HH:mm:ss") + "\r\n\r\n";
            /*@*/
        }
    }
}
/*$QuineClockOutput$*/