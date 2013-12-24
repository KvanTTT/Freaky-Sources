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
			var generated = generator.Generate(File.ReadAllText(@"..\..\..\CustomQuine\Program.cs"));
			Assert.IsFalse(Checker.CheckQuineProgram(generated).HasError);
		}

		[Test]
		public void SimpleQuineGenerateNotMinified()
		{
			var generator = new QuineGenerator() { Minified = false };
			var generated = generator.Generate(File.ReadAllText(@"..\..\..\CustomQuine\Program.cs"));
			Assert.IsFalse(Checker.CheckQuineProgram(generated).HasError);
		}

		[Test]
		public void SimpleQuineGeneratedMinifiedInput()
		{
			var generator = new QuineGenerator() { };
			var minifier = new Minifier(new MinifierOptions(true) { CommentsRemoving = false });
			var minified = minifier.MinifyFromString(File.ReadAllText(@"..\..\..\CustomQuine\Program.cs"));
			var generated = generator.Generate(minified);
			var checkResult = Checker.CheckQuineProgram(generated);
			Assert.IsFalse(checkResult.Output.Contains(QuineGenerator.Newline));
			Assert.IsFalse(checkResult.HasError);
		}
	}
}
