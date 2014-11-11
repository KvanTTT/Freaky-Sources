using System;
namespace QuineClock
{
    class Program
    {
        static long Delay = 0;
        static long PrevSleepTicks = /*$PrevSleepTicks*/0/*PrevSleepTicks$*/;
        static long PrevDateTicks = /*$PrevDateTicks*/0/*PrevDateTicks$*/;
        
        static void Main()
        {
            var ticksPerSecond = TimeSpan.TicksPerSecond;
            var n = DateTime.Now;
            var compileTimeTicks = n.Ticks - PrevDateTicks - PrevSleepTicks;
            var nextDate = n.AddTicks(ticksPerSecond - (n.Ticks % ticksPerSecond)).AddMilliseconds(Delay);
            var sleepTicks = Math.Max(0, nextDate.Ticks - n.Ticks - compileTimeTicks);
            var output = "\r\n\r\n//    " + n.ToString("HH:mm:ss") + " " + new TimeSpan(compileTimeTicks) + "\r\n\r\n";
            /*@*/
            System.Threading.Thread.Sleep((int)((sleepTicks - Delay) * 1000 / ticksPerSecond));
        }
    }
}
/*$QuineClockOutput$*/