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
		[Test]
		public void SimpleQuineGenerate()
		{
			var generator = new QuineGenerator();
			var generated = generator.Generate(File.ReadAllText(@"..\..\..\Sources\CustomQuine.cs"));
			var checkingResult = Checker.CheckQuineProgram(generated);
			Assert.IsTrue(checkingResult.Count == 1 && !checkingResult.First().IsError);
		}

		[Test]
		public void SimpleQuineGenerateNotMinified()
		{
			var generator = new QuineGenerator() { Minified = false };
			var generated = generator.Generate(File.ReadAllText(@"..\..\..\Sources\CustomQuine.cs"));
			var checkingResult = Checker.CheckQuineProgram(generated);
			Assert.IsTrue(checkingResult.Count == 1 && !checkingResult.First().IsError);
		}

		[Test]
		public void SimpleQuineGeneratedMinifiedInput()
		{
			var generator = new QuineGenerator() { };
			var minifier = new Minifier(new MinifierOptions(true) { CommentsRemoving = false, ConsoleApp = true });
			var minified = minifier.MinifyFromString(File.ReadAllText(@"..\..\..\Sources\CustomQuine.cs"));
			var generated = generator.Generate(minified);
			var checkResult = Checker.CheckQuineProgram(generated);
			Assert.IsTrue(checkResult.Count == 1 && !checkResult.First().Output.Contains(QuineGenerator.Newline) && !checkResult.First().IsError);
		}
	}
}
