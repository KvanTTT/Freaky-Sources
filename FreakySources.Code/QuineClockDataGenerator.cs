using System;
using System.Collections.Generic;
using System.Linq;
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

        static string TimeToString(string[] digits, DateTime dateTime)
        {
            var newLine = Environment.NewLine;
            var result = new StringBuilder(newLine + newLine);

            Append(result, "//" + newLine);
            for (int i = 0; i < DigitHeight; i++)
            {
                Append(result, "//  ");
                AppendNumberString(digits, result, dateTime.Hour, i);
                AppendDots(result, i);
                AppendNumberString(digits, result, dateTime.Minute, i);
                AppendDots(result, i);
                AppendNumberString(digits, result, dateTime.Second, i);
                Append(result, newLine);
            }
            Append(result, "//" + newLine + "//" + string.Format("{0,62}", "Quine Clock by KvanTTT, 2014") + newLine + newLine);

            return result.ToString();
        }

        static void AppendNumberString(string[] digits, StringBuilder sb, int number, int line)
        {
            Append(sb, digits[number / 10].Substring(line * DigitWidth, DigitWidth).Replace('!', '\\'));
            Append(sb, DigitSpace);
            Append(sb, digits[number % 10].Substring(line * DigitWidth, DigitWidth).Replace('!', '\\'));
        }

        static void AppendDots(StringBuilder sb, int line)
        {
            Append(sb, DotSpace);
            Append(sb, line == 1 || line == 2 || line == 4 || line == 5 ? "++" : "  ");
            Append(sb, DotSpace);
        }

        static void Append(StringBuilder sb, string s)
        {
            sb.Append(s);
        }

        /*QuineClock3#*/
    }
}
