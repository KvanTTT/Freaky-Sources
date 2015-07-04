using System;
using System.Collections.Generic;
using System.Text;

namespace FreakySources.Code
{
    public class QuineClockDataGenerator
    {
        static string[] Digits;

        public QuineClockDataGenerator(string digitsData)
        {
            var lines = digitsData.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Digits = new string[lines.Length];
            var digit = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                var lines2 = lines[i].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                digit.Clear();
                foreach (var line2 in lines2)
                    digit.Append(line2 + new string(' ', DigitWidth - line2.Length));
                Digits[i] = digit.ToString().Replace('\\', '!');
            }
        }

        public string[] GetDigits()
        {
            return Digits;
        }

        /*#QuineClock3*/

        const int DigitWidth = 7;
        const int DigitHeight = 8;
        const string DigitSpace = "  ";
        const string DotSpace = "  ";
        static StringBuilder Result;

        static string TimeToString(DateTime dateTime)
        {
            var newLine = Environment.NewLine;
            Result = new StringBuilder(newLine + newLine);

            Append("//" + newLine);
            for (int i = 0; i < DigitHeight; i++)
            {
                Append("//  ");
                AppendNumberString(dateTime.Hour, i);
                AppendDots(i);
                AppendNumberString(dateTime.Minute, i);
                AppendDots(i);
                AppendNumberString(dateTime.Second, i);
                Append(newLine);
            }
            Append("//" + newLine + "//" + string.Format("{0,62}", "Quine Clock by KvanTTT, 2014") + newLine + newLine);

            return Result.ToString();
        }

        static void AppendNumberString(int number, int line)
        {
            Append(Digits[number / 10].Substring(line * DigitWidth, DigitWidth).Replace('!', '\\'));
            Append(DigitSpace);
            Append(Digits[number % 10].Substring(line * DigitWidth, DigitWidth).Replace('!', '\\'));
        }

        static void AppendDots(int line)
        {
            Append(DotSpace);
            Append(line == 1 | line == 2 | line == 4 | line == 5 ? "++" : "  ");
            Append(DotSpace);
        }

        static void Append(string s)
        {
            Result.Append(s);
        }

        /*QuineClock3#*/
    }
}
