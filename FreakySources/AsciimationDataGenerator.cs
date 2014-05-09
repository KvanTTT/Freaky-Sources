using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FreakySources
{
	public class Frame
	{
		public int RepeatCount = 1;
		public string[] Lines;
		public string[] ReducedLines;
		public string Line;
		public string ReducedLine;
		public byte[] Bytes;
		public byte[] ReducedBytes;
	}

	public class CompressedFrame
	{
		public FrameType FrameType;
		public List<FrameChange> FrameChanges;
		public byte[] CompressedBytes;
	}

	public class AsciimationDataGenerator
	{
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

			int frameNumber = 0;
			for (int i = 1; i < lines.Length - 1; i += FrameHeight + 1)
			{
				string[] frameLines = new string[FrameHeight];
				string[] reducedLines = new string[FrameHeight];

				for (int j = i; j < i + FrameHeight; j++)
				{
					var linej = lines[j].Replace("\\\\", "\\").Replace("\\'", "'");
					frameLines[j - i] = linej.PadRight(FrameWidth, ' ').Substring(0, FrameWidth);
					reducedLines[j - i] = linej.Length >= FrameWidth ? linej.Substring(0, FrameWidth) : linej + '\n';
				}

				var line = string.Join("", frameLines);
				var tmp1 = string.Join(Environment.NewLine, frameLines);
				var reducedLine = string.Join("", reducedLines);
				result.Add(new Frame
				{
					RepeatCount = int.Parse(lines[i - 1]),
					Lines = frameLines,
					ReducedLines = reducedLines,
					Line = line,
					ReducedLine = reducedLine,
					Bytes = line.Select(c => (byte)c).ToArray(),
					ReducedBytes = reducedLine.Select(c => (byte)c).ToArray()
				});
				frameNumber++;
			}

			Frames = result.ToArray();
		}

		#region version 1.0

		public string GetRleFrames()
		{
			var rleEncoded = new StringBuilder();
			foreach (var frame in Frames)
			{
				double compressRatio;
				var compressed = Compress(frame, out compressRatio);
				rleEncoded.AppendLine("\"" + Convert.ToBase64String(compressed) + "\",");
			}

			return rleEncoded.ToString();
		}

		private static byte[] Compress(Frame frame, out double compressRatio)
		{
			StringBuilder str = new StringBuilder(FrameWidth * FrameHeight);
			foreach (var line in frame.Lines)
				str.Append(line);

			var result = Rle.Encode(Encoding.UTF8.GetBytes(str.ToString()));
			compressRatio = (double)result.Length / str.Length;

			return result;
		}

		#endregion

		#region version 1.1
		
		public string GetGZipCompressedFrames()
		{
			return '"' + StringCompressor.CompressString(Input) + '"';
		}

		#endregion

		#region version 1.2

		public string GetHuffmanRleTable()
		{
			var bytesFreqs = GetBytesFreqs(false);
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

		public ByteCount[] GetBytesFreqs(bool reducedLines = false, int lengthBitsCount = 8)
		{
			int maxRepeatCount = (1 << (lengthBitsCount - 1)) + 1;
			var result = new List<ByteCount>();
			for (int i = 0; i < 256; i++)
				result.Add(new ByteCount { Byte = (byte)i, Count = 0 });

			int length = 0;
			foreach (var frame in Frames)
			{
				string line;
				if (!reducedLines)
					line = frame.Line;
				else
					line = frame.ReducedLine;

				int i = 0;
				while (i < line.Length)
				{
					result[(int)line[i]].Count++;
					length++;

					var beginChar = line[i];
					int j;
					for (j = i + 1; j < line.Length; j++)
						if (line[j] != beginChar || j - i >= maxRepeatCount)
							break;
					i = j;
				}
			}

			result = result.Where(bc => bc.Count != 0).OrderByDescending(bc => bc.Count).ToList();
			return result.ToArray();
		}

		public ByteCount[] GetBytesFreqs(bool reducedLines, int lengthBitsCount, List<CompressedFrame> compressedFrames)
		{
			int maxRepeatCount = (1 << (lengthBitsCount - 1)) + 1;
			var result = new List<ByteCount>();
			for (int i = 0; i < 256; i++)
				result.Add(new ByteCount { Byte = (byte)i, Count = 0 });

			int length = 0;
			for (int k = 0; k < compressedFrames.Count; k++)
			{
				if (compressedFrames[k].FrameType == FrameType.Basic)
				{
					string line;
					if (!reducedLines)
						line = Frames[k].Line;
					else
						line = Frames[k].ReducedLine;

					int i = 0;
					while (i < line.Length)
					{
						result[(int)line[i]].Count++;
						length++;

						var beginChar = line[i];
						int j;
						for (j = i + 1; j < line.Length; j++)
							if (line[j] != beginChar || j - i >= maxRepeatCount)
								break;
						i = j;
					}
				}
				else
				{
					foreach (var change in compressedFrames[k].FrameChanges)
						foreach (var c in change.Chars)
							result[(int)c].Count++;
				}
			}

			result = result.Where(bc => bc.Count != 0).OrderByDescending(bc => bc.Count).ToList();
			return result.ToArray();
		}

		public static byte[] SerializeByteCount(ByteCount[] byteCounts)
		{
			var result = new byte[byteCounts.Length * 4];
			int bitPos = 0;
			foreach (var byteCount in byteCounts)
			{
				Utils.AddInt(result, ref bitPos, byteCount.Byte, 8);
				Utils.AddInt(result, ref bitPos, byteCount.Count, 24);
			}
			return result;
		}

		/*#ByteCounts*/

		public static ByteCount[] DeserializeByteCount(byte[] ar)
		{
			var result = new ByteCount[ar.Length / 4];
			int ind = 0;
			int bitPos = 0;
			for (int i = 0; i < ar.Length; i += 4)
			{
				result[ind++] = new ByteCount
				{
					Byte = (byte)Utils.GetInt(ar, ref bitPos, 8),
					Count = Utils.GetInt(ar, ref bitPos, 24)
				};
			}
			return result;
		}

		/*ByteCounts#*/

		#endregion

		#region version 1.3

		public string Compress_v_1_3(out List<CompressedFrame> compressedFrames, bool reducedLines = true)
		{
			var bytesFreqs = GetBytesFreqs(reducedLines, HuffmanRleRepeatedBits);
			var tree = new HuffmanTree(bytesFreqs);
			List<int> frameDiffs;

			var framesChanges = CalculateFrameChanges();
			compressedFrames = GetCompressedFrames(tree, reducedLines, framesChanges, out frameDiffs);

			bytesFreqs = GetBytesFreqs(reducedLines, HuffmanRleRepeatedBits, compressedFrames);
			tree = new HuffmanTree(bytesFreqs);
			compressedFrames = GetCompressedFrames(tree, reducedLines, framesChanges, out frameDiffs);

			var result = new byte[1000000];

			int bitPos = 0;
			Utils.AddInt(result, ref bitPos, bytesFreqs.Length, 8);
			foreach (var byteCount in bytesFreqs)
			{
				Utils.AddInt(result, ref bitPos, byteCount.Byte, 8);
				Utils.AddInt(result, ref bitPos, byteCount.Count, 24);
			}
			
			Utils.AddInt(result, ref bitPos, frameDiffs.Count, 24);
			foreach (var diff in frameDiffs)
				Utils.AddInt(result, ref bitPos, diff, 9);

			foreach (var frame in compressedFrames)
				Utils.AddBytes(result, ref bitPos, frame.CompressedBytes);

			result = result.Take((bitPos + 7) / 8).ToArray();

			return Convert.ToBase64String(result);
		}

		private List<CompressedFrame> GetCompressedFrames(HuffmanTree tree, bool reducedLines, List<Dictionary<FrameType, CompressedFrame>> framesChanges,
			out List<int> frameDiffs)
		{
			var compressedFrames = new List<CompressedFrame>();
			int currentFrame = 0;
			frameDiffs = new List<int>();
			for (int i = 0; i < Frames.Length; i++)
			{
				var frame = Frames[i];
				var frameChanges = framesChanges[i];

				int basisBitPos = 0;
				byte[] basisBytes = null;
				byte[] transBytes, transLeftBytes, transRightBytes, transTopBytes, transBottomBytes;
				transBytes = transLeftBytes = transRightBytes = transTopBytes = transBottomBytes = null;

				int frameTypeBit;
				basisBytes = new byte[frame.Bytes.Length * 2];
				Utils.AddInt(basisBytes, ref basisBitPos, frame.RepeatCount, 7);
				frameTypeBit = basisBitPos;
				Utils.AddInt(basisBytes, ref basisBitPos, (int)FrameType.Basic, 3);
				Utils.AddInt(basisBytes, ref basisBitPos, frame.ReducedBytes.Length, BasicBytesLengthBits);
				HuffmanRle.Encode(tree, reducedLines ? frame.ReducedBytes : frame.Bytes, ref basisBitPos, basisBytes, HuffmanRleRepeatedBits, HuffmanRleNotRepeatedBits);
				basisBytes = basisBytes.Take((basisBitPos + 7) / 8).ToArray();

				if (i != 0)
				{
					transBytes = GetCompressedFrameBytes(tree, frame, frameChanges[FrameType.Transitional]);
					transLeftBytes = GetCompressedFrameBytes(tree, frame, frameChanges[FrameType.TransitionalLeft]);
					transRightBytes = GetCompressedFrameBytes(tree, frame, frameChanges[FrameType.TransitionalRight]);
					transTopBytes = GetCompressedFrameBytes(tree, frame, frameChanges[FrameType.TransitionalTop]);
					transBottomBytes = GetCompressedFrameBytes(tree, frame, frameChanges[FrameType.TransitionalBottom]);
				}

				var typesBytes = new Dictionary<FrameType, byte[]>()
				{
					{ FrameType.Basic, basisBytes },
					{ FrameType.Transitional, transBytes },
					{ FrameType.TransitionalLeft, transLeftBytes },
					{ FrameType.TransitionalRight, transRightBytes },
					{ FrameType.TransitionalTop, transTopBytes },
					{ FrameType.TransitionalBottom, transBottomBytes },
				};

				var minLengthBytesKey = typesBytes.Where(tb => tb.Value != null)
					.Aggregate((a, b) => a.Value.Length < b.Value.Length ? a : b).Key;

				Utils.AddInt(typesBytes[minLengthBytesKey], ref frameTypeBit, (int)minLengthBytesKey, 3);

				compressedFrames.Add(new CompressedFrame
				{
					CompressedBytes = typesBytes[minLengthBytesKey],
					FrameType = minLengthBytesKey,
					FrameChanges = !frameChanges.ContainsKey(minLengthBytesKey) ? null : frameChanges[minLengthBytesKey].FrameChanges
				});
				currentFrame += typesBytes[minLengthBytesKey].Length;
				frameDiffs.Add(typesBytes[minLengthBytesKey].Length);
			}

			return compressedFrames;
		}

		private byte[] GetCompressedFrameBytes(HuffmanTree tree, Frame frame, CompressedFrame compressedFrame)
		{
			int transBitPos = 0;
			var transZipBytes = new byte[frame.Bytes.Length * 10];
			Utils.AddInt(transZipBytes, ref transBitPos, frame.RepeatCount, 7);
			Utils.AddInt(transZipBytes, ref transBitPos, (int)compressedFrame.FrameType, 3);
			Utils.AddInt(transZipBytes, ref transBitPos, compressedFrame.FrameChanges.Count, 7);
			for (int i = 0; i < compressedFrame.FrameChanges.Count; i++)
			{
				var change = compressedFrame.FrameChanges[i];
				Utils.AddInt(transZipBytes, ref transBitPos, (int)change.Type, 2);
				Utils.AddInt(transZipBytes, ref transBitPos, GetPos(change.X, change.Y), 10);

				if (change.Type == FrameChangeType.One)
					Utils.AddIntReversed(transZipBytes, ref transBitPos, tree.CompressedBytes[change.Chars[0]]);
				else
				{
					Utils.AddInt(transZipBytes, ref transBitPos, change.Length,
						change.Type == FrameChangeType.Horizontal ? 7 : 4);
					//HuffmanRle.Encode(tree, change.Chars.Select(c => (byte)c).ToArray(), ref transBitPos, transZipBytes, 5, 4);
					for (int j = 0; j < change.Chars.Count; j++)
						Utils.AddIntReversed(transZipBytes, ref transBitPos, tree.CompressedBytes[change.Chars[j]]);
					/*if (i == 0 && compressedFrame.FrameType != FrameType.Basic && compressedFrame.FrameType != FrameType.Transitional)
						HuffmanRle.Encode(tree, change.Chars.Select(c => (byte)c).ToArray(), ref transBitPos, transZipBytes, 5, 4);
					else
						for (int j = 0; j < change.Chars.Count; j++)
							Utils.AddIntReversed(transZipBytes, ref transBitPos, tree.CompressedBytes[change.Chars[j]]);*/
				}
			}
			return transZipBytes.Take((transBitPos + 7) / 8).ToArray();
		}

		private List<Dictionary<FrameType, CompressedFrame>> CalculateFrameChanges()
		{
			var result = new List<Dictionary<FrameType, CompressedFrame>>();
			result.Add(new Dictionary<FrameType, CompressedFrame>());
			for (int i = 1; i < Frames.Length; i++)
				result.Add(CalculateChangesForDifferentChangeTypes(Frames, i));
			return result;
		}

		private Dictionary<FrameType, CompressedFrame> CalculateChangesForDifferentChangeTypes(Frame[] frames, int frameNumber)
		{
			var changes = new Dictionary<FrameType, CompressedFrame>();
			changes.Add(FrameType.Transitional, CalculateFrameChanges(Frames, FrameType.Transitional, frameNumber));
			changes.Add(FrameType.TransitionalLeft, CalculateFrameChanges(Frames, FrameType.TransitionalLeft, frameNumber));
			changes.Add(FrameType.TransitionalRight, CalculateFrameChanges(Frames, FrameType.TransitionalRight, frameNumber));
			changes.Add(FrameType.TransitionalTop, CalculateFrameChanges(Frames, FrameType.TransitionalTop, frameNumber));
			changes.Add(FrameType.TransitionalBottom, CalculateFrameChanges(Frames, FrameType.TransitionalBottom, frameNumber));
			return changes;
		}

		private CompressedFrame CalculateFrameChanges(Frame[] frames, FrameType type, int frameNumber)
		{
			int x_inc = 0;
			int y_inc = 0;
			switch (type)
			{
				case FrameType.TransitionalLeft:
					x_inc = -1;
					break;
				case FrameType.TransitionalRight:
					x_inc = 1;
					break;
				case FrameType.TransitionalTop:
					y_inc = -1;
					break;
				case FrameType.TransitionalBottom:
					y_inc = 1;
					break;
			}

			var currentFrame = frames[frameNumber];
			var prevFrame = frames[frameNumber - 1];
			var changesPos = new List<Tuple<int, int>>();

			for (int y = 0; y < FrameHeight; y++)
			{
				if (y + y_inc >= 0 && y + y_inc < FrameHeight)
				{
					var line = currentFrame.Lines[y + y_inc];
					var prevLine = prevFrame.Lines[y];
					for (int x = 0; x < FrameWidth; x++)
					{
						if (x + x_inc >= 0 && x + x_inc < FrameWidth)
						{
							if (line[x + x_inc] != prevLine[x])
								changesPos.Add(new Tuple<int, int>(x + x_inc, y + y_inc));
						}
					}
				}
			}

			var frameChanges = new List<FrameChange>();

			var markedChanges = new bool[FrameHeight, FrameWidth];
			if (type == FrameType.TransitionalLeft || type == FrameType.TransitionalRight)
			{
				int x = type == FrameType.TransitionalLeft ? FrameWidth - 1 : 0;
				var chars = new List<char>();
				for (int y = 0; y < FrameHeight; y++)
				{
					markedChanges[y, x] = true;
					chars.Add(currentFrame.Lines[y][x]);
				}
				frameChanges.Add(new FrameChange { Type = FrameChangeType.Vertical, X = x, Y = 0, Chars = chars, Length = FrameHeight });
			}
			else if (type == FrameType.TransitionalTop || type == FrameType.TransitionalBottom)
			{
				int y = type == FrameType.TransitionalTop ? FrameHeight - 1 : 0;
				var chars = new List<char>();
				for (int x = 0; x < FrameWidth; x++)
				{
					markedChanges[y, x] = true;
					chars.Add(currentFrame.Lines[y][x]);
				}
				frameChanges.Add(new FrameChange { Type = FrameChangeType.Horizontal, X = 0, Y = y, Chars = chars, Length = FrameWidth });
			}

			foreach (var changePos in changesPos)
			{
				if (!markedChanges[changePos.Item2, changePos.Item1])
				{
					int lineXInd = FrameWidth;
					for (int x = changePos.Item1 + 1; x < FrameWidth; x++)
						if (changesPos.FirstOrDefault(pos => pos.Item1 == x && pos.Item2 == changePos.Item2) == null)
						{
							lineXInd = x;
							break;
						}

					int lineYInd = FrameHeight;
					for (int y = changePos.Item2 + 1; y < FrameHeight; y++)
						if (changesPos.FirstOrDefault(pos => pos.Item1 == changePos.Item1 && pos.Item2 == y) == null)
						{
							lineYInd = y;
							break;
						}

					var frameChange = new FrameChange
					{
						X = changePos.Item1,
						Y = changePos.Item2
					};
					if (lineXInd - changePos.Item1 >= lineYInd - changePos.Item2)
					{
						if (lineXInd - changePos.Item1 == 1)
						{
							frameChange.Type = FrameChangeType.One;
							frameChange.Length = 1;
							frameChange.Chars = new List<char>()
								{
									currentFrame.Lines[changePos.Item2][changePos.Item1],
								};
							markedChanges[changePos.Item2, changePos.Item1] = true;
						}
						else
						{
							frameChange.Type = FrameChangeType.Horizontal;
							frameChange.Length = lineXInd - changePos.Item1;
							frameChange.Chars = new List<char>();
							for (int x = 0; x < frameChange.Length; x++)
							{
								frameChange.Chars.Add(currentFrame.Lines[changePos.Item2][changePos.Item1 + x]);
								markedChanges[changePos.Item2, changePos.Item1 + x] = true;
							}
						}
					}
					else
					{
						frameChange.Type = FrameChangeType.Vertical;
						frameChange.Length = lineYInd - changePos.Item2;
						frameChange.Chars = new List<char>();
						for (int y = 0; y < frameChange.Length; y++)
						{
							frameChange.Chars.Add(currentFrame.Lines[changePos.Item2 + y][changePos.Item1]);
							markedChanges[changePos.Item2 + y, changePos.Item1] = true;
						}
					}

					frameChanges.Add(frameChange);
				}
			}

			return new CompressedFrame { FrameChanges = frameChanges, FrameType = type };
		}

		public int GetPos(int x, int y)
		{
			return (y * FrameWidth + x);
		}

		/*#Asciimation_1_3*/

		const int HuffmanRleRepeatedBits = 5;
		const int HuffmanRleNotRepeatedBits = 3;
		const int BasicBytesLengthBits = 10;
		const int FrameHeight = 13;
		const int FrameWidth = 67;

		public static string Decompress_v_1_3(string str, int currentFrame)
		{
			byte[] bytes = Convert.FromBase64String(str);

			int bitPos = 0;
			byte tableLength = (byte)Utils.GetInt(bytes, ref bitPos, 8);

			var bytesFreqs = new ByteCount[tableLength];
			for (int i = 0; i < tableLength; i++)
			{
				bytesFreqs[i] = new ByteCount
				{
					Byte = (byte)Utils.GetInt(bytes, ref bitPos, 8),
					Count = Utils.GetInt(bytes, ref bitPos, 24)
				};
			}
			var tree = new HuffmanTree(bytesFreqs);

			int frameDiffsCount = Utils.GetInt(bytes, ref bitPos, 24);
			var frameNumbers = new List<int>();
			int sum = 0;
			frameNumbers.Add(sum);
			for (int i = 0; i < frameDiffsCount; i++)
			{
				sum += Utils.GetInt(bytes, ref bitPos, 9);
				frameNumbers.Add(sum);
			}

			int beginBitPos = bitPos;
			bitPos = frameNumbers[currentFrame] * 8 + beginBitPos;
			int repeatCount = Utils.GetInt(bytes, ref bitPos, 7);
			var frameType = (FrameType)Utils.GetInt(bytes, ref bitPos, 3);
			if (frameType == FrameType.Basic)
			{
				var frameLength = Utils.GetInt(bytes, ref bitPos, BasicBytesLengthBits);
				var frameBytes = HuffmanRle2.Decode(tree, bytes, ref bitPos, frameLength, HuffmanRleRepeatedBits, HuffmanRleNotRepeatedBits);
				return CharsToLine(BytesToFrame(frameBytes));
			}
			else
			{
				int prevFrame = currentFrame;
				int prevRepeatCount;
				FrameType prevFrameType;
				do
				{
					prevFrame--;
					bitPos = frameNumbers[prevFrame] * 8 + beginBitPos;
					prevRepeatCount = Utils.GetInt(bytes, ref bitPos, 7);
					prevFrameType = (FrameType)Utils.GetInt(bytes, ref bitPos, 3);
				}
				while (prevFrameType != FrameType.Basic);

				var frameLength = Utils.GetInt(bytes, ref bitPos, BasicBytesLengthBits);
				var frame = BytesToFrame(HuffmanRle2.Decode(tree, bytes, ref bitPos, frameLength, HuffmanRleRepeatedBits, HuffmanRleNotRepeatedBits));

				do
				{
					prevFrame++;
					bitPos = frameNumbers[prevFrame] * 8 + beginBitPos;
					prevRepeatCount = Utils.GetInt(bytes, ref bitPos, 7);
					prevFrameType = (FrameType)Utils.GetInt(bytes, ref bitPos, 3);

					switch (prevFrameType)
					{
						case FrameType.Transitional:
							break;
						case FrameType.TransitionalLeft:
							for (int y = 0; y < FrameHeight; y++)
								for (int x = 0; x < FrameWidth - 1; x++)
									frame[y, x] = frame[y, x + 1];
							break;
						case FrameType.TransitionalRight:
							for (int y = 0; y < FrameHeight; y++)
								for (int x = FrameWidth - 1; x >= 1; x--)
									frame[y, x] = frame[y, x - 1];
							break;
						case FrameType.TransitionalTop:
							for (int y = 0; y < FrameHeight - 1; y++)
								for (int x = 0; x < FrameWidth; x++)
									frame[y, x] = frame[y + 1, x];
							break;
						case FrameType.TransitionalBottom:
							for (int y = FrameHeight - 1; y >= 1; y--)
								for (int x = 0; x < FrameWidth; x++)
									frame[y, x] = frame[y - 1, x];
							break;
					}

					var frameChangesCount = Utils.GetInt(bytes, ref bitPos, 7);
					for (int i = 0; i < frameChangesCount; i++)
					{
						var frameChangeType = (FrameChangeType)Utils.GetInt(bytes, ref bitPos, 2);
						var position = Utils.GetInt(bytes, ref bitPos, 10);

						int length;
						if (frameChangeType == FrameChangeType.One)
							length = 1;
						else
							length = Utils.GetInt(bytes, ref bitPos, frameChangeType == FrameChangeType.Horizontal ? 7 : 4);
						for (int j = 0; j < length; j++)
						{
							frame[GetY(position), GetX(position)] = (char)Utils.GetValue(tree.Root, bytes, ref bitPos);
							position += frameChangeType == FrameChangeType.Horizontal ? 1 : FrameWidth;
						}
					}
				}
				while (prevFrame != currentFrame);

				return CharsToLine(frame).ToString();
			}
		}

		static int GetX(int pos)
		{
			return pos % FrameWidth;
		}

		static int GetY(int pos)
		{
			return pos / FrameWidth;
		}

		static string CharsToLine(char[,] chars)
		{
			var result = new StringBuilder("\r\n\r\n//  ");
			for (int i = 0; i < FrameHeight; i++)
			{
				for (int j = 0; j < FrameWidth; j++)
					result.Append(chars[i, j]);
				result.Append("\r\n");
				if (i != FrameHeight - 1)
					result.Append("//  ");
			}
			return result.ToString();
		}

		static char[,] BytesToFrame(byte[] frame)
		{
			var result = new char[FrameHeight, FrameWidth];
			int x = 0, y = 0, i = 0, tail;
			while (i < frame.Length)
			{
				var c = (char)frame[i];
				if (c != '\n')
					result[y, x++] = c;
				if (c == '\n' || x == FrameWidth)
				{
					tail = FrameWidth - x;
					while (tail-- > 0)
						result[y, x++] = ' ';
					y++;
					x = 0;
				}
				i++;
			}
			return result;
		}

		/*Asciimation_1_3#*/

		#endregion
	}
}
