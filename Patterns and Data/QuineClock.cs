using System;
namespace QuineClock
{
    class Program
    {
        static long PrevDateTicks = /*$PrevDateTicks*/DateTime.Now.Ticks/*PrevDateTicks$*/;
        static int CompileTicksLength = 10;
        static int SleepMs = 500;
        static int CurrentCompileNumber = /*$CurrentCompileNumber*/0/*CurrentCompileNumber$*/;
        static  long[] CompileTicks = /*$CompileTicks*/new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }/*CompileTicks$*/;        

        static void Main()
        {
            //var ticksPerSecond = TimeSpan.TicksPerSecond;
            var n = DateTime.Now;
            var compileTimeTicks = n.Ticks - PrevDateTicks;// - new TimeSpan(0,0,0,0,SleepMs).Ticks;
            CompileTicks[CurrentCompileNumber++] = compileTimeTicks;
            CurrentCompileNumber %= CompileTicks.Length;
            long avgCompileTime = 0;
            for (int i = 0; i < CompileTicks.Length; i++)
                  avgCompileTime += CompileTicks[i];
            var output = "\r\n\r\n//    " + new TimeSpan(avgCompileTime / CompileTicks.Length) + "\r\n\r\n";
            
            System.Threading.Thread.Sleep(SleepMs);
            /*@*/
        }
    }
}
/*$QuineClockOutput$*/