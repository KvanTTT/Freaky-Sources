using SourceChecker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class Program
    {
        private static void Main(string[] args)
        {
            string simpleProgram = "class P{static void Main(){}}";
            string simpleQuine = "class P{static void Main(){var s=\"class P{{static void Main(){{var s={1}{0}{1};System.Console.Write(s,s,'{1}');}}}}\";System.Console.Write(s,s,'\"');}}";
            string singleLineCommentsPalindrome = "//}}{)(niaM diov citats{P ssalc\r\n\rclass P{static void Main(){}}//";
            string multiLineCommentsPalindrome = "/**/class P{static void Main(){}};/*/;}}{)(niaM diov citats{P ssalc/**/";
            string singleLineCommentsPalindromeQuine = Checker.MinimizeString(File.ReadAllText("..\\..\\..\\SingleCommentsPalindromeQuine\\Program.cs"));
            singleLineCommentsPalindromeQuine = Checker.PrepareSingleLineCommentsPalindrome(singleLineCommentsPalindromeQuine);
            string multiLineCommentsPalindromeQuine = Checker.MinimizeString(File.ReadAllText("..\\..\\..\\MultiCommentsPalindromeQuine\\Program.cs"));
            multiLineCommentsPalindromeQuine = Checker.PrepareMultiLineCommentsPalindrome(multiLineCommentsPalindromeQuine);
            Program.WriteOutput(simpleProgram, "Simple Program", new Func<string, CheckingResult>(Checker.CompileAndGetOutput));
            Program.WriteOutput(simpleQuine, "Simple Quine", new Func<string, CheckingResult>(Checker.CheckQuineProgram));
            Program.WriteOutput(singleLineCommentsPalindrome, "Single Line Comments Palindrome", new Func<string, CheckingResult>(Checker.CheckPalindromeProgram));
            Program.WriteOutput(multiLineCommentsPalindrome, "Multi Line Comments Palindrome", new Func<string, CheckingResult>(Checker.CheckPalindromeProgram));
            Program.WriteOutput(singleLineCommentsPalindromeQuine, "Single Line Comments Palindrome Quine", new Func<string, CheckingResult>(Checker.CheckPalindromeQuineProgram));
            Program.WriteOutput(multiLineCommentsPalindromeQuine, "Multi Line Comments Palindrome Quine", new Func<string, CheckingResult>(Checker.CheckPalindromeQuineProgram));
            Console.ReadLine();
        }

        private static void WriteOutput(string program, string title, Func<string, CheckingResult> checker)
        {
            Console.WriteLine("{0}:{1}", title, Environment.NewLine);
            Console.WriteLine(program);
            CheckingResult checkingResult = checker(program);
            Console.WriteLine();
            Console.WriteLine("Program length: " + program.Length);
            if (checkingResult.HasError)
            {
                Console.WriteLine("{0}: Fail. Error at line: {1}, column: {2}", checker.Method.Name, checkingResult.FirstErrorLine, checkingResult.FirstErrorColumn);
            }
            else
            {
                Console.WriteLine("{0}: Success.", checker.Method.Name);
            }
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
