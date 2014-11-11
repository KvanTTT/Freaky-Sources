using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreakySources.Code
{
	public class HuffmanRle
	{
		public static byte[] Encode(HuffmanTree tree, byte[] bytes)
		{
			var result = new byte[bytes.Length * 2];
			int curBit = 0;
			Encode(tree, bytes, ref curBit, result);
			result = result.Take((curBit + 7) / 8).ToArray();
			return result;
		}

		public static void Encode(HuffmanTree tree, byte[] bytes, ref int curBit, byte[] result, int bitsCountPerRepLength = 8, int bitsCountPerNotRepLength = 8)
		{
			var maxRepeatCount = (1 << (bitsCountPerRepLength - 1)) + 1;
			var maxNotRepeatCount = 1 << (bitsCountPerNotRepLength - 1);

			var compressedBytes = tree.CompressedBytes;
			int i = 0;
			while (i < bytes.Length)
			{
				int j = i;
				do
					j++;
				while (j != bytes.Length && bytes[j] == bytes[i]);

				int repeatCount = j - i;
				if (repeatCount >= 2)
				{
					int segmentCount = repeatCount / maxRepeatCount;
					int rest = repeatCount % maxRepeatCount;

					for (int k = 0; k < segmentCount; k++)
					{
						Utils.AddInt(result, ref curBit, maxRepeatCount - 2, bitsCountPerRepLength);
						Utils.AddIntReversed(result, ref curBit, compressedBytes[bytes[i]]);
					}
					if (rest >= 2)
					{
						Utils.AddInt(result, ref curBit, (rest - 2), bitsCountPerRepLength);
						Utils.AddIntReversed(result, ref curBit, compressedBytes[bytes[i]]);
						i = j;
					}
					else
						i = j - rest;
				}
				else
				{
					while (j != bytes.Length && bytes[j] != bytes[j - 1])
						j++;

					int nonrepeatCount = j - i;
					if (j != bytes.Length)
						nonrepeatCount--;
					int segmentCount = nonrepeatCount / maxNotRepeatCount;
					int rest = nonrepeatCount % maxNotRepeatCount;

					for (int k = 0; k < segmentCount; k++)
					{
						Utils.AddInt(result, ref curBit, (1 << bitsCountPerNotRepLength) - 1, bitsCountPerNotRepLength);
						for (int l = 0; l < maxNotRepeatCount; l++)
							Utils.AddIntReversed(result, ref curBit, compressedBytes[bytes[i + k * maxNotRepeatCount + l]]);
					}

					if (rest >= 1)
					{
						Utils.AddInt(result, ref curBit, (maxNotRepeatCount | (rest - 1)), bitsCountPerNotRepLength);
						for (int l = 0; l < rest; l++)
							Utils.AddIntReversed(result, ref curBit, compressedBytes[bytes[i + segmentCount * maxNotRepeatCount + l]]);
					}
					i = j;
					if (j != bytes.Length)
						i--;
				}
			}
		}

		/*#HuffmanRleDecode*/

		public static byte[] Decode(HuffmanTree tree, byte[] bytes)
		{
			var root = tree.Root;
			var result = new List<byte>(bytes.Length * 2);
			int bitPos = 0;

			while (bytes.Length * 8 - bitPos > 8)
			{
				var length = Utils.GetInt(bytes, ref bitPos, 8);

				if ((length & 128) == 0)
				{
					var repeatCount = length + 2;
					byte value = Utils.GetValue(root, bytes, ref bitPos);
					for (int j = 0; j < repeatCount; j++)
						result.Add(value);
				}
				else
				{
					var notRepeatCount = (127 & length) + 1;
					for (int j = 0; j < notRepeatCount; j++)
					{
						byte value = Utils.GetValue(root, bytes, ref bitPos);
						result.Add(value);
					}
				}
			}

			return result.ToArray();
		}

		/*HuffmanRleDecode#*/
	}

	/*#HuffmanRleDecode2*/

	public class HuffmanRle2
	{
		public static byte[] Decode(HuffmanTree tree, byte[] bytes, ref int curBit, int bytesCount, int bitsCountPerRepLength = 8, int bitsCountPerNotRepLength = 8)
		{
			int minLength = Math.Min(bitsCountPerRepLength, bitsCountPerNotRepLength);
			var maxCount = 1 << (minLength - 1);

			var root = tree.Root;
			var result = new List<byte>(bytes.Length * 2);

			int curBytesCount = 0;
			int i = 0;
			while (curBytesCount < bytesCount)
			{
				var length = Utils.GetInt(bytes, ref curBit, minLength);

				if ((length & maxCount) == 0)
				{
					curBit -= minLength;
					length = Utils.GetInt(bytes, ref curBit, bitsCountPerRepLength);
					var repeatCount = length + 2;
					byte value = Utils.GetValue(root, bytes, ref curBit);
					for (int j = 0; j < repeatCount; j++)
					{
						result.Add(value);
						curBytesCount++;
					}
				}
				else
				{
					curBit -= minLength;
					length = Utils.GetInt(bytes, ref curBit, bitsCountPerNotRepLength);
					var notRepeatCount = (((1 << (bitsCountPerNotRepLength - 1)) - 1) & length) + 1;
					for (int j = 0; j < notRepeatCount; j++)
					{
						byte value = Utils.GetValue(root, bytes, ref curBit);
						result.Add(value);
						curBytesCount++;
					}
				}
				i++;
			}

			return result.ToArray();
		}
	}

	/*HuffmanRleDecode2#*/
}
