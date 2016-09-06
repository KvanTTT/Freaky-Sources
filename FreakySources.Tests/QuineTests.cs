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
		private CSharpChecker _cSharpChecker;
		private JavaChecker _javaChecker;
		public const string PatternsFolder = @"..\..\..\Patterns and Data\";

		[SetUp]
		public void Init()
		{
			_cSharpChecker = new CSharpChecker();
			_javaChecker = new JavaChecker
			{
				JavaPath = Helpers.GetJavaExePath(@"bin\java.exe"),
				JavaCompilerPath = Helpers.GetJavaExePath(@"bin\javac.exe")
			};
		}

		[Test]
		public void SimpleProgram()
		{
			string simpleProgram = "class P{static void Main(){}}";
			var checkingResult = _cSharpChecker.CompileAndRun(simpleProgram);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void SimpleQuine()
		{
			string simpleQuine = "class P{static void Main(){var s=\"class P{{static void Main(){{var s={1}{0}{1};System.Console.Write(s,s,'{1}');}}}}\";System.Console.Write(s,s,'\"');}}";
			var checkingResult = _cSharpChecker.CheckQuineProgram(simpleQuine);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void SingleLineCommentPalindrome()
		{
			string singleLineCommentsPalindrome = "//}}{)(niaM diov citats{P ssalc\r\n\rclass P{static void Main(){}}//";
			var checkingResult = _cSharpChecker.CheckPalindromeProgram(singleLineCommentsPalindrome);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void MultiLineCommentPalindrome()
		{
			string multiLineCommentsPalindrome = "/**/class P{static void Main(){}};/*/;}}{)(niaM diov citats{P ssalc/**/";
			var checkingResult = _cSharpChecker.CheckPalindromeProgram(multiLineCommentsPalindrome);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void SingleLineCommentPalindromeQuine()
		{
			string singleLineCommentsPalindromeQuine = StringExtensions.RemoveSpacesInSource(File.ReadAllText(Path.Combine(PatternsFolder, "SingleCommentsPalindromeQuine.cs")));
			singleLineCommentsPalindromeQuine = StringExtensions.PrepareSingleLineCommentsPalindrome(singleLineCommentsPalindromeQuine);
			var checkingResult = _cSharpChecker.CheckPalindromeQuineProgram(singleLineCommentsPalindromeQuine);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void MultiLineCommentPalindromeQuine()
		{
			string multiLineCommentsPalindromeQuine = StringExtensions.RemoveSpacesInSource(File.ReadAllText(Path.Combine(PatternsFolder, "MultiCommentsPalindromeQuine.cs")));
			multiLineCommentsPalindromeQuine = StringExtensions.PrepareMultiLineCommentsPalindrome(multiLineCommentsPalindromeQuine);
			var checkingResult = _cSharpChecker.CheckPalindromeQuineProgram(multiLineCommentsPalindromeQuine);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void CSharpJavaPolyglotQuine()
		{
			string quine = File.ReadAllText(Path.Combine(PatternsFolder, "CSharpJavaPolyglotQuine.cs"));
			var cSharpCheckingResult = _cSharpChecker.CheckQuineProgram(quine);
			Assert.IsTrue(cSharpCheckingResult.HasNotErrors());

			_javaChecker.ClassName = "Program";
			var javaCheckingResult = _javaChecker.CheckQuineProgram(quine);
			Assert.IsTrue(javaCheckingResult.HasNotErrors());
		}
	}
}