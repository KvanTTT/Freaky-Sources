using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C = System.Console;

namespace FreakySources.Code
{
    public class QuineSnakeGenerator
    {
        /*$ Input */
        public static int
            FieldWidth,
            FieldHeight,
            FieldWidth2,
            FieldHeight2,
            HeadRow,
            HeadColumn,
            FoodRow,
            FoodColumn,
            GameState;
        static int NewDir;
        static int[] Dirs;
        static string R;

        public static string NewDirString
        {
            get { return "" + NewDir.ToString(); }
        }
        public static string DirsString
        {
            get { return "{" + string.Join(",", Dirs.Select(d => "" + d)) + "}"; }
        }
        /* Input $*/

        static QuineSnakeGenerator()
        {
            FieldWidth = 20;
            FieldHeight = 10;
            FieldWidth2 = FieldWidth / 2;
            FieldHeight2 = FieldHeight / 2;
            var FieldState = new int[FieldHeight, FieldWidth];
            HeadRow = FieldHeight / 2 - 1;
            HeadColumn = FieldWidth / 2;
            NewDir = Bottom;
            Dirs = new[] { Bottom, Bottom };

            int i, y = HeadRow, x = HeadColumn;
            int dir = Left;
            FieldState[y, x] = Head;
            for (i = 0; i < Dirs.Length - 1; i++)
            {
                dir = Dirs[i];
                if (dir == Left)
                    x++;
                if (dir == Right)
                    x--;
                if (dir == Top)
                    y++;
                if (dir == Bottom)
                    y--;
                FieldState[y, x] = dir == Left || dir == Right ? Horizontal : Vertical;
            }
            FieldState[y, x] = dir == Left || dir == Right ? HorizontalTail : VerticalTail;

            var rand = new Random();
            for (; ; )
            {
                FoodRow = rand.Next(FieldHeight);
                FoodColumn = rand.Next(FieldWidth);
                if (FieldState[FoodRow, FoodColumn] == Empty)
                {
                    FieldState[FoodRow, FoodColumn] = Food;
                    break;
                }
            }
        }

        /*#Snake*/

        const int Empty = 0;
        const int Vertical = 1;
        const int Horizontal = 2;
        const int TopLeftBottomRightBend = 3;
        const int TopRightBottomLeftBend = 4;
        const int Head = 5;
        const int VerticalTail = 6;
        const int HorizontalTail = 7;
        const int Food = 8;

        const int Left = 0;
        const int Top = 1;
        const int Right = 2;
        const int Bottom = 3;

        const int Init = 0;
        const int Playing = 1;
        const int GameOver = 2;
        const int Win = 3;

        static string MakeStepAndPrint()
        {
            int y = HeadRow,
                x = HeadColumn,
                dir = Left,
                headDir = Bottom,
                offset = 0,
                I1, I2, S, A1, A2, B1, B2,
                l = Dirs.Length;
            bool expand = false;

            var FieldState = new int[FieldHeight, FieldWidth];
            FieldState[FoodRow, FoodColumn] = Food;
            if (GameState != Playing)
                goto X;

            for (int i = 0; i < l; i++)
            {
                dir = Dirs[i];

                var d = i == 0 ? NewDir : Dirs[i - 1];
                if (d == Left || d == Right)
                {
                    A1 = Left;
                    A2 = Right;
                    B1 = Top;
                    B2 = Bottom;
                    S = Horizontal;
                    I1 = 0;
                    I2 = 1;
                }
                else
                {
                    A1 = Top;
                    A2 = Bottom;
                    B1 = Left;
                    B2 = Right;
                    S = Vertical;
                    I1 = 1;
                    I2 = 0;
                }
                if (i == 0)
                {
                    if ((NewDir != A1 || dir != A2) && (NewDir != A2 || dir != A1))
                        headDir = NewDir;
                    else
                        headDir = dir;

                    offset = headDir == A1 ? -1 : 1;
                    HeadRow = HeadRow + offset * I1;
                    HeadColumn = HeadColumn + offset * I2;

                    if (HeadRow < 0 || HeadRow >= FieldHeight || HeadColumn < 0 || HeadColumn >= FieldWidth)
                    {
                        GameState = GameOver;
                        goto X;
                    }
                    if (FieldState[HeadRow, HeadColumn] == Food)
                        expand = true;
                    FieldState[HeadRow, HeadColumn] = Head;
                }

                if (i != l - 1 || expand)
                    FieldState[y, x] = dir == B1 || dir == B2 ? dir == B1 ^ d != A1 ? TopLeftBottomRightBend : TopRightBottomLeftBend : S;
                else
                    FieldState[y, x] = d == Left || d == Right ? HorizontalTail : VerticalTail;

                if (dir == Left)
                    x++;
                if (dir == Right)
                    x--;
                if (dir == Top)
                    y++;
                if (dir == Bottom)
                    y--;
            }

            if (FieldState[HeadRow, HeadColumn] != Head)
            {
                GameState = GameOver;
                goto X;
            }

            if (expand)
            {
                FieldState[y, x] = dir == Left || dir == Right ? HorizontalTail : VerticalTail;

                GameState = Win;
                for (I1 = 0; I1 < FieldHeight; I1++)
                    for (I2 = 0; I2 < FieldWidth; I2++)
                        if (FieldState[I1, I2] == Empty)
                            GameState = Playing;
                if (GameState == Win)
                    goto X;

                var rand = new Random();
                for (; ; )
                {
                    FoodRow = rand.Next(FieldHeight);
                    FoodColumn = rand.Next(FieldWidth);
                    if (FieldState[FoodRow, FoodColumn] == Empty)
                    {
                        FieldState[FoodRow, FoodColumn] = Food;
                        break;
                    }
                }

                var ar = new int[l + 1];
                for (offset = 0; offset < l; offset++)
                    ar[offset + 1] = Dirs[offset];
                ar[0] = headDir;
                Dirs = ar;
            }
            else
            {
                for (offset = l - 2; offset >= 0; offset--)
                    Dirs[offset + 1] = Dirs[offset];
                Dirs[0] = headDir;
            }

            X:
            var R = "";
            var rn = "\r\n";
            var s = new string('/', FieldWidth + 4);
            R += rn + s + rn;

            var stateChars = " |-\\/O\"=*";
            for (I1 = 0; I1 < FieldHeight; I1++)
            {
                R += "//";
                if (GameState == Playing)
                    for (I2 = 0; I2 < FieldWidth; I2++)
                        R += stateChars[FieldState[I1, I2]].ToString();
                else
                {
                    var m = GameState == GameOver ? "Game Over!" : "  Win!!!  ";
                    int fw2 = FieldWidth / 2;
                    R += new string(' ', fw2 - 5) + m + new string(' ', FieldWidth - fw2 - 5);
                }
                R += "//"; R += rn;
            }

            R += s + "   QuineSnake by KvanTTT for GoStash" + rn;

            return R;
        }

        /*Snake#*/
    }
}