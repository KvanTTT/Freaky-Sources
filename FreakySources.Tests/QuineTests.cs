using NUnit.Framework;
using System;
using System.IO;

namespace FreakySources.Tests
{
    [TestFixture]
	public class QuineTests
	{
		private Lazy<CSharpChecker> _cSharpChecker = new Lazy<CSharpChecker>(() => new CSharpChecker());
		private Lazy<JavaChecker> _javaChecker = new Lazy<JavaChecker>(() => new JavaChecker
		{
			JavaPath = Helpers.GetJavaExePath(@"bin\java.exe"),
			JavaCompilerPath = Helpers.GetJavaExePath(@"bin\javac.exe"),
			ClassName = "Program"
		});
		private Lazy<PhpChecker> _phpChecker = new Lazy<PhpChecker>(() => new PhpChecker
		{
			PhpPath = @"C:\xampp\php\php.exe",
		});
		public const string PatternsFolder = @"..\..\..\Patterns and Data\";

		[Test]
		public void SimpleProgram()
		{
			string simpleProgram = "class P{static void Main(){}}";
			var checkingResult = _cSharpChecker.Value.CompileAndRun(simpleProgram);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void SimpleQuine()
		{
			string simpleQuine = "class P{static void Main(){var s=\"class P{{static void Main(){{var s={1}{0}{1};System.Console.Write(s,s,'{1}');}}}}\";System.Console.Write(s,s,'\"');}}";
			var checkingResult = _cSharpChecker.Value.CheckQuineProgram(simpleQuine);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void ShortestQuine()
		{
			string shortestQuine = File.ReadAllText(Path.Combine(PatternsFolder, "ShortestQuine.cs"));
			var checkingResult = _cSharpChecker.Value.CheckQuineProgram(shortestQuine);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void SingleLineCommentPalindrome()
		{
			string singleLineCommentsPalindrome = "//}}{)(niaM diov citats{P ssalc\r\n\rclass P{static void Main(){}}//";
			var checkingResult = _cSharpChecker.Value.CheckPalindromeProgram(singleLineCommentsPalindrome);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void MultiLineCommentPalindrome()
		{
			string multiLineCommentsPalindrome = "/**/class P{static void Main(){}};/*/;}}{)(niaM diov citats{P ssalc/**/";
			var checkingResult = _cSharpChecker.Value.CheckPalindromeProgram(multiLineCommentsPalindrome);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void SingleLineCommentPalindromeQuine()
		{
			string singleLineCommentsPalindromeQuine = StringExtensions.RemoveSpacesInSource(File.ReadAllText(Path.Combine(PatternsFolder, "SingleCommentsPalindromeQuine.cs")));
			singleLineCommentsPalindromeQuine = StringExtensions.PrepareSingleLineCommentsPalindrome(singleLineCommentsPalindromeQuine);
			var checkingResult = _cSharpChecker.Value.CheckPalindromeQuineProgram(singleLineCommentsPalindromeQuine);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void MultiLineCommentPalindromeQuine()
		{
			string multiLineCommentsPalindromeQuine = StringExtensions.RemoveSpacesInSource(File.ReadAllText(Path.Combine(PatternsFolder, "MultiCommentsPalindromeQuine.cs")));
			multiLineCommentsPalindromeQuine = StringExtensions.PrepareMultiLineCommentsPalindrome(multiLineCommentsPalindromeQuine);
			var checkingResult = _cSharpChecker.Value.CheckPalindromeQuineProgram(multiLineCommentsPalindromeQuine);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void JavaPhpPolyglot()
		{
			string polyglot = File.ReadAllText(Path.Combine(PatternsFolder, "Polyglot.java.php"));
			var javaCheckingResult = _javaChecker.Value.CompileAndRun(polyglot);
			var phpCheckingResult = _phpChecker.Value.CompileAndRun(polyglot);
			Assert.AreEqual("/*Hello World!", javaCheckingResult[0].Output);
			Assert.AreEqual("/*Hello World!", phpCheckingResult[0].Output);
		}

		[Test]
		public void CSharpJavaPhpPolyglot()
		{
			string polyglot = File.ReadAllText(Path.Combine(PatternsFolder, "Polyglot.cs.java.php"));
			var csCheckingResult = _cSharpChecker.Value.CompileAndRun(polyglot);
			var javaCheckingResult = _javaChecker.Value.CompileAndRun(polyglot);
			var phpCheckingResult = _phpChecker.Value.CompileAndRun(polyglot);
			Assert.AreEqual("//Hello World!", csCheckingResult[0].Output);
			Assert.AreEqual(csCheckingResult[0].Output, javaCheckingResult[0].Output);
			Assert.AreEqual(csCheckingResult[0].Output, phpCheckingResult[0].Output);
		}

		[Test]
		public void CSharpJavaPolyglotQuine()
		{
			string polyglotQuine = File.ReadAllText(Path.Combine(PatternsFolder, "PolyglotQuine.cs.java"));

			var cSharpCheckingResult = _cSharpChecker.Value.CheckQuineProgram(polyglotQuine);
			Assert.IsTrue(cSharpCheckingResult.HasNotErrors());
			
			var javaCheckingResult = _javaChecker.Value.CheckQuineProgram(polyglotQuine);
			Assert.IsTrue(javaCheckingResult.HasNotErrors());
		}

		[Test]
		public void PalindromeCSharpJavaPolyglotQuine()
		{
			string palindromePolyglotQuine = File.ReadAllText(Path.Combine(PatternsFolder, "PalindromePolyglotQuine.cs.java"));

			var cSharpCheckingResult = _cSharpChecker.Value.CheckPalindromeQuineProgram(palindromePolyglotQuine);
			Assert.IsTrue(cSharpCheckingResult.HasNotErrors());

			var javaCheckingResult = _javaChecker.Value.CheckPalindromeQuineProgram(palindromePolyglotQuine);
			Assert.IsTrue(javaCheckingResult.HasNotErrors());
		}
	}
}