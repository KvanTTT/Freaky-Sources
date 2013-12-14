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
	public class QuineGeneratorTests
	{
		[Test]
		public void SimpleQuineGenerate()
		{
			var generator = new QuineGenerator();
			var generated = generator.Generate(File.ReadAllText(@"..\..\..\CustomQuine\Program.cs"));
			Assert.IsFalse(Checker.CheckQuineProgram(generated).HasError);
		}
	}
}
