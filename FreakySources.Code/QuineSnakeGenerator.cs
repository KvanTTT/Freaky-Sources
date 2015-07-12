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
            HeadRow,
            HeadColumn,
            FoodRow,
            FoodColumn;
        static Direction NewDir;
        static Direction[] Dirs;
        static string R;

        public static string NewDirString
        {
            get { return "Direction." + NewDir.ToString(); }
        }
        public static string DirsString
        {
            get { return "{" + string.Join(",", Dirs.Select(d => "Direction." + d)) + "}"; }
        }
        /* Input $*/

        static QuineSnakeGenerator()
        {
            FieldWidth = 20;
            FieldHeight = 15;
            var FieldState = new ItemState[FieldHeight, FieldWidth];
            HeadRow = FieldHeight / 2;
            HeadColumn = FieldWidth / 2;
            NewDir = Direction.Bottom;
            Dirs = new[] { Direction.Bottom, Direction.Bottom };

            int i, y = HeadRow, x = HeadColumn;
            Direction dir = Direction.Left;
            FieldState[y, x] = ItemState.Head;
            for (i = 0; i < Dirs.Length - 1; i++)
            {
                dir = Dirs[i];
                if (dir == Direction.Left)
                    x++;
                if (dir == Direction.Right)
                    x--;
                if (dir == Direction.Top)
                    y++;
                if (dir == Direction.Bottom)
                    y--;
                FieldState[y, x] = dir == Direction.Left || dir == Direction.Right ? ItemState.Horizontal : ItemState.Vertical;
            }
            FieldState[y, x] = dir == Direction.Left || dir == Direction.Right ? ItemState.HorizontalTail : ItemState.VerticalTail;

            var rand = new Random();
            for (; ; )
            {
                FoodRow = rand.Next(FieldHeight);
                FoodColumn = rand.Next(FieldWidth);
                if (FieldState[FoodRow, FoodColumn] == ItemState.Empty)
                {
                    FieldState[FoodRow, FoodColumn] = ItemState.Food;
                    break;
                }
            }
        }

        /*#Snake*/

        enum ItemState
        {
            Empty = 0,
            Vertical = 1,
            Horizontal = 2,
            TopLeftBottomRightBend = 3,
            TopRightBottomLeftBend = 4,
            Head = 5,
            VerticalTail = 6,
            HorizontalTail = 7,
            Food = 8
        }

        enum Direction
        {
            Left = 0,
            Top = 1,
            Right = 2,
            Bottom = 3
        }

        static void MakeStepAndPrint()
        {
            int y = HeadRow, x = HeadColumn;
            int prevY = y, prevX = x;
            Direction dir = Direction.Left;
            var FieldState = new ItemState[FieldHeight, FieldWidth];
            Direction headDir = Direction.Bottom;
            int offset = 0;
            FieldState[FoodRow, FoodColumn] = ItemState.Food;
            bool expand = false;

            int I1, I2;
            ItemState S;
            Direction A1, A2, B1, B2;

            for (int i = 0; i < Dirs.Length; i++)
            {
                dir = Dirs[i];

                var d = i == 0 ? NewDir : Dirs[i - 1];
                if (d == Direction.Left || d == Direction.Right)
                {
                    A1 = Direction.Left;
                    A2 = Direction.Right;
                    B1 = Direction.Top;
                    B2 = Direction.Bottom;
                    S = ItemState.Horizontal;
                    I1 = 0;
                    I2 = 1;
                }
                else
                {
                    A1 = Direction.Top;
                    A2 = Direction.Bottom;
                    B1 = Direction.Left;
                    B2 = Direction.Right;
                    S = ItemState.Vertical;
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
                        //GameOver = true;
                        return;
                    }
                    if (FieldState[HeadRow, HeadColumn] == ItemState.Food)
                        expand = true;
                    FieldState[HeadRow, HeadColumn] = ItemState.Head;
                }

                if (i != Dirs.Length - 1 || expand)
                    FieldState[y, x] = dir == B1 || dir == B2 ? dir == B1 ^ d != A1 ? ItemState.TopLeftBottomRightBend : ItemState.TopRightBottomLeftBend : S;
                else
                    FieldState[y, x] = d == Direction.Left || d == Direction.Right ? ItemState.HorizontalTail : ItemState.VerticalTail;

                prevY = y;
                prevX = x;
                if (dir == Direction.Left)
                    x++;
                if (dir == Direction.Right)
                    x--;
                if (dir == Direction.Top)
                    y++;
                if (dir == Direction.Bottom)
                    y--;
            }

            if (FieldState[HeadRow, HeadColumn] != ItemState.Head)
            {
                //GameOver = true;
                return;
            }

            var l = Dirs.Length;
            if (expand)
            {
                FieldState[y, x] = dir == Direction.Left || dir == Direction.Right ? ItemState.HorizontalTail : ItemState.VerticalTail;

                var rand = new Random();
                for (; ; )
                {
                    FoodRow = rand.Next(FieldHeight);
                    FoodColumn = rand.Next(FieldWidth);
                    if (FieldState[FoodRow, FoodColumn] == ItemState.Empty)
                    {
                        FieldState[FoodRow, FoodColumn] = ItemState.Food;
                        break;
                    }
                }

                var ar = new Direction[l + 1];
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



            R = "";
            var rn = "\r\n";
            var s = new string('/', FieldWidth + 4);
            R += rn + s + rn;

            var stateChars = " |-\\/O\"=*";

            for (I1 = 0; I1 < FieldHeight; I1++)
            {
                R += "//";
                for (I2 = 0; I2 < FieldWidth; I2++)
                    R += stateChars[(int)FieldState[I1, I2]].ToString();
                R += "//" + rn;
            }

            R += s + rn;
        }

        /*Snake#*/
    }
}