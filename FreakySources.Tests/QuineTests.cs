using NUnit.Framework;
using SourceChecker;
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
		[Test]
		public void SimpleProgram()
		{
			string simpleProgram = "class P{static void Main(){}}";
			Assert.IsFalse(Checker.Compile(simpleProgram).HasError);
		}

		[Test]
		public void SimpleQuine()
		{
			string simpleQuine = "class P{static void Main(){var s=\"class P{{static void Main(){{var s={1}{0}{1};System.Console.Write(s,s,'{1}');}}}}\";System.Console.Write(s,s,'\"');}}";
			Assert.IsFalse(Checker.CheckQuineProgram(simpleQuine).HasError);
		}

		[Test]
		public void SingleLineCommentPalindrome()
		{
			string singleLineCommentsPalindrome = "//}}{)(niaM diov citats{P ssalc\r\n\rclass P{static void Main(){}}//";
			Assert.IsFalse(Checker.CheckPalindromeProgram(singleLineCommentsPalindrome).HasError);
		}

		[Test]
		public void MultiLineCommentPalindrome()
		{
			string multiLineCommentsPalindrome = "/**/class P{static void Main(){}};/*/;}}{)(niaM diov citats{P ssalc/**/";
			Assert.IsFalse(Checker.CheckPalindromeProgram(multiLineCommentsPalindrome).HasError);
		}

		[Test]
		public void SingleLineCommentPalindromeQuine()
		{
			string singleLineCommentsPalindromeQuine = Checker.RemoveSpacesInSource(File.ReadAllText(@"..\..\..\SingleCommentsPalindromeQuine\Program.cs"));
			singleLineCommentsPalindromeQuine = Checker.PrepareSingleLineCommentsPalindrome(singleLineCommentsPalindromeQuine);
			Assert.IsFalse(Checker.CheckPalindromeQuineProgram(singleLineCommentsPalindromeQuine).HasError);
		}

		[Test]
		public void MultiLineCommentPalindromeQuine()
		{
			string multiLineCommentsPalindromeQuine = Checker.RemoveSpacesInSource(File.ReadAllText("..\\..\\..\\MultiCommentsPalindromeQuine\\Program.cs"));
			multiLineCommentsPalindromeQuine = Checker.PrepareMultiLineCommentsPalindrome(multiLineCommentsPalindromeQuine);
			Assert.IsFalse(Checker.CheckPalindromeQuineProgram(multiLineCommentsPalindromeQuine).HasError);
		}
	}
}