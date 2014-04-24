using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreakySources
{
	public enum FrameType
	{
		Basic,
		Transitional,
		TransitionalLeft,
		TransitionalRight,
		TransitionalTop,
		TransitionalBottom
	}

	public class Frame
	{
		public string[] Lines;
		public string[] ReducedLines;
		public int RepeatCount = 1;
		public FrameType FrameType;
		public string Line;
		public byte[] Bytes;
	}

	public class AsciimationDataGenerator
	{
		public const int FrameHeight = 13;
		public const int FrameWidth = 67;

		public string Input
		{
			get;
			private set;
		}

		public Frame[] Frames
		{
			get;
			private set;
		}

		public AsciimationDataGenerator(string frames)
		{
			Input = frames;
			string[] lines = frames.Split(new string[] { "\\n" }, StringSplitOptions.None);
			List<Frame> result = new List<Frame>();

			for (int i = 1; i < lines.Length - 1; i += FrameHeight + 1)
			{
				string[] frameLines = new string[FrameHeight];
				string[] reducedLines = new string[FrameHeight];

				for (int j = i; j < i + FrameHeight; j++)
				{
					frameLines[j - i] = lines[j].PadRight(FrameWidth, ' ');
					reducedLines[j - i] = lines[j];
				}

				var line = string.Join("", frameLines);
				result.Add(new Frame()
				{
					RepeatCount = int.Parse(lines[i - 1]),
					Lines = frameLines,
					ReducedLines = reducedLines,
					Line = line,
					Bytes = Encoding.UTF8.GetBytes(line)
				});
			}

			Frames = result.ToArray();
		}

		public string InsertRleFrames(string code, string keyBegin, string keyEnd)
		{
			int beginInd = code.IndexOf(keyBegin);
			if (beginInd != -1)
			{
				int endInd = code.IndexOf(keyEnd, beginInd);

				var rleEncoded = new StringBuilder();
				foreach (var frame in Frames)
				{
					double compressRatio;
					var compressed = Compress(frame, out compressRatio);
					rleEncoded.AppendLine("\"" + Convert.ToBase64String(compressed) + "\",");
				}

				return code.Remove(beginInd + keyBegin.Length) + rleEncoded.ToString() + code.Substring(endInd);
			}
			return code;
		}

		public string ChangeGZipCompressedFrames(string code, string keyBegin, string keyEnd, bool insert = true)
		{
			int beginInd = code.IndexOf(keyBegin);
			if (beginInd != -1)
			{
				int endInd = code.IndexOf(keyEnd, beginInd);
				return code.Remove(beginInd + keyBegin.Length) + "\"" +
					(insert ? StringCompressor.CompressString(Input) : "") + "\"" + code.Substring(endInd);
			}
			return code;
		}

		public static byte[] Compress(Frame frame, out double compressRatio)
		{
			StringBuilder str = new StringBuilder(FrameWidth * FrameHeight);
			foreach (var line in frame.Lines)
				str.Append(line);

			var result = Rle.Encode(Encoding.UTF8.GetBytes(str.ToString()));
			compressRatio = (double)result.Length / str.Length;

			return result;
		}

		public ByteCount[] GetBytesFreqs(bool reducedLines = false)
		{
			var result = new List<ByteCount>();
			for (int i = 0; i < 256; i++)
				result.Add(new ByteCount { Byte = (byte)i, Count = 0 });

			int length = 0;
			foreach (var frame in Frames)
			{
				if (!reducedLines)
				{
					int i = 0;
					while (i < frame.Line.Length)
					{
						result[(int)frame.Line[i]].Count++;
						length++;

						var beginChar = frame.Line[i];
						int j;
						for (j = i + 1; j < frame.Line.Length; j++)
							if (frame.Line[j] != beginChar || j - i >= 129)
								break;
						i = j;
					}
				}
				else
				{
					for (int k = 0; k < frame.ReducedLines.Length; k++)
					{
						int i = 0;
						var reducedLine = frame.ReducedLines[k];
						while (i < reducedLine.Length)
						{
							result[(int)reducedLine[i]].Count++;
							length++;

							var beginChar = reducedLine[i];
							int j;
							for (j = i + 1; j < reducedLine.Length; j++)
								if (reducedLine[j] != beginChar || j - i >= 129)
									break;
							i = j;
						}
					}
				}
			}

			result = result.Where(bc => bc.Count != 0).OrderByDescending(bc => bc.Count).ToList();
			return result.ToArray();
		}

		public static byte[] SerializeByteCount(ByteCount[] byteCounts)
		{
			var result = new byte[byteCounts.Length * 4];
			int ind = 0;
			foreach (var byteCount in byteCounts)
			{
				result[ind++] = byteCount.Byte;
				result[ind++] = (byte)((byteCount.Count & 0x00FF0000) >> 16);
				result[ind++] = (byte)((byteCount.Count & 0x0000FF00) >> 8);
				result[ind++] = (byte)(byteCount.Count & 0x000000FF);
			}
			return result;
		}

		public string GetHuffmanRleTable()
		{
			var bytesFreqs = GetBytesFreqs(true);
			var bytes = SerializeByteCount(bytesFreqs);
			return '"' + Convert.ToBase64String(bytes) + '"';
		}

		public string GetHuffmanRleFrames()
		{
			var bytesFreqs = GetBytesFreqs(false);
			var tree = new HuffmanTree(bytesFreqs);
			var result = new StringBuilder();
			foreach (var frame in Frames)
			{
				result.Append('"' + Convert.ToBase64String(HuffmanRle.Encode(tree, frame.Bytes)) + "\"," + Environment.NewLine);
			}
			return result.ToString();
		}

		/*$ByteCounts*/

		public static ByteCount[] DeserializeByteCount(byte[] ar)
		{
			var result = new ByteCount[ar.Length / 4];
			int ind = 0;
			for (int i = 0; i < ar.Length; i += 4)
			{
				result[ind++] = new ByteCount
				{
					Byte = ar[i],
					Count = ((int)ar[i + 1] << 16) + ((int)ar[i + 2] << 8) + (int)ar[i + 3]
				};
			}
			return result;
		}

		/*ByteCounts$*/
	}
}
