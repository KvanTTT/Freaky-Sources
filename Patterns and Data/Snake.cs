using System;
using System.Text;
using C = System.Console;

namespace QuineClock3
{
    class Program
    {
        const int
            FieldHeight = /*$FieldHeight*/FieldHeight/*FieldHeight$*/,
            FieldWidth = /*$FieldWidth*/FieldWidth/*FieldWidth$*/;
        static int
            HeadRow = /*$HeadRow*/FieldHeight / 2/*HeadRow$*/,
            HeadColumn = /*$HeadColumn*/FieldWidth / 2/*HeadColumn$*/,
            FoodRow = /*$FoodRow*/0/*FoodRow$*/,
            FoodColumn = /*$FoodColumn*/0/*FoodColumn$*/;
        static Direction NewDir = /*$NewDir*/Direction.Bottom/*NewDir$*/;
        static Direction[] Dirs = /*$Dirs*/ { Direction.Bottom, Direction.Bottom } /*Dirs$*/ ;
        static string R;

        /*#Snake*//*Snake#*/

        static void Main()
        {
            int key = (int)C.ReadKey(true).Key;
            NewDir = key < 37 || key > 40 ? Dirs[0] : key - 37;
            MakeStepAndPrint();
            /*@*/
        }
    }
}

/*$SnakeOutput$*/