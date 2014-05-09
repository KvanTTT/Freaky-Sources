using FreakySources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AsciimationStatistics
{
	class Program
	{
		static void Main(string[] args)
		{
			var generator = new AsciimationDataGenerator(File.ReadAllText(@"..\..\..\Sources\Asciimation.txt"));
			
			List<CompressedFrame> compressedFrames;
			var compressedStr = generator.Compress_v_1_3(out compressedFrames, true);
			File.WriteAllText(DateTime.Now.Ticks.ToString() + ".txt", compressedStr);

			Console.WriteLine("Global frames count: " + generator.Frames.Length);
			Console.WriteLine("Compressed frames count: " + compressedFrames.Where(f => f.FrameType != FrameType.Basic).Count());
			
			var repeatedLengths = new List<int>();
			var notrepeatedLengths = new List<int>();
			var changeLengths = new List<int>();
			var changeCounts = new List<int>();
			int maxReducedLineLength = 0;

			var frameTypesCount = new Dictionary<FrameType, int>()
			{
				{ FrameType.Basic, 0 },
				{ FrameType.Transitional, 0 },
				{ FrameType.TransitionalLeft, 0 },
				{ FrameType.TransitionalRight, 0 },
				{ FrameType.TransitionalTop, 0 },
				{ FrameType.TransitionalBottom, 0 }
			};

			for (int i = 0; i < compressedFrames.Count; i++)
			{
				var compressedFrame = compressedFrames[i];
				switch (compressedFrame.FrameType)
				{
					case FrameType.Basic:
						GetRepeatedCount(generator.Frames[i].ReducedLine, repeatedLengths, notrepeatedLengths);
						if (generator.Frames[i].ReducedLine.Length > maxReducedLineLength)
							maxReducedLineLength = generator.Frames[i].ReducedLine.Length;
						break;
					case FrameType.Transitional:
					case FrameType.TransitionalLeft:
					case FrameType.TransitionalRight:
					case FrameType.TransitionalTop:
					case FrameType.TransitionalBottom:
						foreach (var change in compressedFrame.FrameChanges)
						{
							//GetRepeatedCount(new string(change.Chars.ToArray()), repeatedLengths, notrepeatedLengths);
							changeLengths.Add(change.Chars.Count);
						}
						changeCounts.Add(compressedFrame.FrameChanges.Count);
						break;
				}
				frameTypesCount[compressedFrame.FrameType]++;
			}

			Console.WriteLine("Basic count: " + frameTypesCount[FrameType.Basic]);
			Console.WriteLine("Trans count: " + frameTypesCount[FrameType.Transitional]);
			Console.WriteLine("Trans left count: " + frameTypesCount[FrameType.TransitionalLeft]);
			Console.WriteLine("Trans right count: " + frameTypesCount[FrameType.TransitionalRight]);
			Console.WriteLine("Trans top count: " + frameTypesCount[FrameType.TransitionalTop]);
			Console.WriteLine("Trans bottom count: " + frameTypesCount[FrameType.TransitionalBottom]);

			Console.WriteLine("Avg repeated chars length: " + repeatedLengths.Average());
			Console.WriteLine("Avg not repeated chars length: " + notrepeatedLengths.Average());
			Console.WriteLine("Avg change length: " + changeLengths.Average());
			Console.WriteLine("Max change length: " + changeLengths.Max());
			Console.WriteLine("Max change count: " + changeCounts.Max());
			Console.WriteLine("Max reduced line length: " + maxReducedLineLength);

			Console.ReadLine();
		}

		static void GetRepeatedCount(string str, List<int> repeatedLengths, List<int> notrepeatedLengths)
		{
			int i = 0;
			while (i < str.Length)
			{
				int j = i;
				do
					j++;
				while (j != str.Length && str[j] == str[i]);

				int repeatCount = j - i;
				if (repeatCount >= 2)
				{
					repeatedLengths.Add(repeatCount);

					i = j;
				}
				else
				{
					while (j != str.Length && str[j] != str[j - 1])
						j++;

					int nonrepeatCount = j - i;
					if (j != str.Length)
						nonrepeatCount--;

					notrepeatedLengths.Add(nonrepeatCount);

					i = j;
					if (j != str.Length)
						i--;
				}
			}
		}
	}
}
