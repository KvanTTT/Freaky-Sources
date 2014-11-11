using FreakySources.Code;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FreakySources.Tests
{
	[TestFixture]
	public class Asciimation_v_1_3_Tests
	{
		[Test]
		public void CompDecomp_v_1_3()
		{
			var generator = new AsciimationDataGenerator(File.ReadAllText(Path.Combine(QuineTests.PatternsFolder, "Asciimation.txt")));

			List<CompressedFrame> compressedFrames;
			var str = generator.Compress_v_1_3(out compressedFrames);
			File.WriteAllText("CompDecomp_v_1_3.txt", str);

			for (int i = 0; i < generator.Frames.Length; i++)
			{
				var expectedStr = "\r\n\r\n" + string.Join("\r\n", generator.Frames[i].Lines.Select(l => "//  " + l)) + "\r\n";
				var acturalStr = AsciimationDataGenerator.Decompress_v_1_3(str, i);
				Assert.AreEqual(expectedStr, acturalStr);
			}
		}
	}
}
