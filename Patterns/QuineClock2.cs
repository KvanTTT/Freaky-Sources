using System;
namespace QuineClock
{
    class Program
    {
        static void Main()
        {
            var ticksPerSecond = TimeSpan.TicksPerSecond;
            var n = DateTime.Now;
            var nextDate = n.AddTicks(ticksPerSecond - (n.Ticks % ticksPerSecond));
            var sleepMs = (nextDate.Ticks - n.Ticks) * 1000 / ticksPerSecond - 50;
            if (sleepMs < 0) sleepMs = 0;
            System.Threading.Thread.Sleep((int)sleepMs);
            var output = "\r\n\r\n//    " + nextDate.ToString("HH:mm:ss") + "\r\n\r\n";
            /*@*/
        }
    }
}
/*$QuineClockOutput$*/