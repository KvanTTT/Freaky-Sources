using System;

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
            FoodColumn = /*$FoodColumn*/0/*FoodColumn$*/,
            NewDir = /*$NewDir*/3/*NewDir$*/,
            GameState = /*$GameState*/0/*GameState$*/;
        static int[] Dirs = /*$Dirs*/ { 3, 3 } /*Dirs$*/ ;

        /*#Snake*//*Snake#*/

        static void Main()
        {
            if (GameState != 0)
            {
                int k = (int)Console.ReadKey(true).Key;
                NewDir = k < 37 || k > 40 ? Dirs[0] : k - 37;
            }
            if (GameState == 0)
                GameState = 1;
            var R = MakeStepAndPrint();
            /*@*/
        }
    }
}

/*$SnakeOutput$*/