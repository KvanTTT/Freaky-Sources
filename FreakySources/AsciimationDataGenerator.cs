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
		public int RepeatCount = 1;
		public FrameType FrameType;
	}

	public class AsciimationDataGenerator
	{
		public const int FrameHeight = 13;
		public const int FrameWidth = 67;

		private string _input;
		private Frame[] _frames;

		public AsciimationDataGenerator(string frames)
		{
			_input = frames;
			string[] lines = frames.Split(new string[] { "\\n" }, StringSplitOptions.None);
			List<Frame> result = new List<Frame>();

			for (int i = 1; i < lines.Length - 1; i += FrameHeight + 1)
			{
				string[] frameLines = new string[FrameHeight];

				for (int j = i; j < i + FrameHeight; j++)
					frameLines[j - i] = lines[j].PadRight(FrameWidth, ' ');

				result.Add(new Frame()
				{
					RepeatCount = int.Parse(lines[i - 1]),
					Lines = frameLines
				});
			}

			_frames = result.ToArray();
		}

		public string InsertRleFrames(string code, string keyBegin, string keyEnd)
		{
			int beginInd = code.IndexOf(keyBegin);
			if (beginInd != -1)
			{
				int endInd = code.IndexOf(keyEnd, beginInd);

				var rleEncoded = new StringBuilder();
				foreach (var frame in _frames)
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
					(insert ? StringCompressor.CompressString(_input) : "") + "\"" + code.Substring(endInd);
			}
			return code;
		}

		public static byte[] Compress(Frame frame, out double compressRatio)
		{
			StringBuilder str = new StringBuilder(FrameWidth * FrameHeight);
			foreach (var line in frame.Lines)
				str.Append(line);

			var result = RLE.Encode(Encoding.UTF8.GetBytes(str.ToString()));
			compressRatio = (double)result.Length / str.Length;

			return result;
		}
	}
}
