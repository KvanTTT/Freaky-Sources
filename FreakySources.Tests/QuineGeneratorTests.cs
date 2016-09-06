using NUnit.Framework;
using FreakySources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSharpMinifier;

namespace FreakySources.Tests
{
	[TestFixture]
	public class QuineGeneratorTests
	{
		private CSharpChecker _cSharpChecker;

		[SetUp]
		public void Init()
		{
			_cSharpChecker = new CSharpChecker();
		}

		[Test]
		public void SimpleQuineGenerate()
		{
			var generator = new QuineGenerator();
			var generated = generator.Generate(File.ReadAllText(Path.Combine(QuineTests.PatternsFolder, "CustomQuine.cs")));
			var checkingResult = _cSharpChecker.CheckQuineProgram(generated);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void SimpleQuineGenerateNotMinified()
		{
			var generator = new QuineGenerator() { Minified = false };
			var generated = generator.Generate(File.ReadAllText(Path.Combine(QuineTests.PatternsFolder, "CustomQuine.cs")));
			var checkingResult = _cSharpChecker.CheckQuineProgram(generated);
			Assert.IsTrue(checkingResult.HasNotErrors());
		}

		[Test]
		public void SimpleQuineGeneratedMinifiedInput()
		{
			var generator = new QuineGenerator() { };
			var minifier = new Minifier(new MinifierOptions(true) { CommentsRemoving = false, ConsoleApp = true });
			var minified = minifier.MinifyFromString(File.ReadAllText(Path.Combine(QuineTests.PatternsFolder, "CustomQuine.cs")));
			var generated = generator.Generate(minified);
			var checkingResult = _cSharpChecker.CheckQuineProgram(generated);
			Assert.IsTrue(checkingResult.HasNotErrors());
			Assert.IsTrue(!checkingResult.First().Output.Contains(QuineGenerator.Newline));
		}
	}
}
