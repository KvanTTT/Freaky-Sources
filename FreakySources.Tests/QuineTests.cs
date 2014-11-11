using NUnit.Framework;
using FreakySources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FreakySources.Tests
{
	[TestFixture]
	public class QuineTests
	{
        public const string PatternsFolder = @"..\..\..\Patterns\";

		[Test]
		public void SimpleProgram()
		{
			string simpleProgram = "class P{static void Main(){}}";
			var checkingResult = Checker.CompileAndRun(simpleProgram);
			Assert.IsTrue(checkingResult.Count == 1 && !checkingResult.First().IsError);
		}

		[Test]
		public void SimpleQuine()
		{
			string simpleQuine = "class P{static void Main(){var s=\"class P{{static void Main(){{var s={1}{0}{1};System.Console.Write(s,s,'{1}');}}}}\";System.Console.Write(s,s,'\"');}}";
			var checkingResult = Checker.CheckQuineProgram(simpleQuine);
			Assert.IsTrue(checkingResult.Count == 1 && !checkingResult.First().IsError);
		}

		[Test]
		public void SingleLineCommentPalindrome()
		{
			string singleLineCommentsPalindrome = "//}}{)(niaM diov citats{P ssalc\r\n\rclass P{static void Main(){}}//";
			var checkingResult = Checker.CheckPalindromeProgram(singleLineCommentsPalindrome);
			Assert.IsTrue(checkingResult.Count == 1 && !checkingResult.First().IsError);
		}

		[Test]
		public void MultiLineCommentPalindrome()
		{
			string multiLineCommentsPalindrome = "/**/class P{static void Main(){}};/*/;}}{)(niaM diov citats{P ssalc/**/";
			var checkingResult = Checker.CheckPalindromeProgram(multiLineCommentsPalindrome);
			Assert.IsTrue(checkingResult.Count == 1 && !checkingResult.First().IsError);
		}

		[Test]
		public void SingleLineCommentPalindromeQuine()
		{
			string singleLineCommentsPalindromeQuine = Checker.RemoveSpacesInSource(File.ReadAllText(Path.Combine(PatternsFolder, "SingleCommentsPalindromeQuine.cs")));
			singleLineCommentsPalindromeQuine = Checker.PrepareSingleLineCommentsPalindrome(singleLineCommentsPalindromeQuine);
			var checkingResult = Checker.CheckPalindromeQuineProgram(singleLineCommentsPalindromeQuine);
			Assert.IsTrue(checkingResult.Count == 1 && !checkingResult.First().IsError);
		}

		[Test]
		public void MultiLineCommentPalindromeQuine()
		{
			string multiLineCommentsPalindromeQuine = Checker.RemoveSpacesInSource(File.ReadAllText(Path.Combine(PatternsFolder, "MultiCommentsPalindromeQuine.cs")));
			multiLineCommentsPalindromeQuine = Checker.PrepareMultiLineCommentsPalindrome(multiLineCommentsPalindromeQuine);
			var checkingResult = Checker.CheckPalindromeQuineProgram(multiLineCommentsPalindromeQuine);
			Assert.IsTrue(checkingResult.Count == 1 && !checkingResult.First().IsError);
		}
	}
}