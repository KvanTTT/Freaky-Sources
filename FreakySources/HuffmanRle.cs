using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreakySources
{
	public class HuffmanRle
	{
		public static byte[] Encode(HuffmanTree tree, byte[] bytes)
		{
			var compressedBytes = tree.CompressedBytes;
			int curBit = 0;
			var result = new byte[bytes.Length];

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
					int segmentCount = repeatCount / 129;
					int rest = repeatCount % 129;

					for (int k = 0; k < segmentCount; k++)
					{
						AddInt(result, ref curBit, 127, 8);
						AddIntReversed(result, ref curBit, compressedBytes[bytes[i]]);
					}
					if (rest >= 2)
					{
						AddInt(result, ref curBit, (uint)(rest - 2), 8);
						AddIntReversed(result, ref curBit, compressedBytes[bytes[i]]);
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
					int segmentCount = nonrepeatCount / 128;
					int rest = nonrepeatCount % 128;

					for (int k = 0; k < segmentCount; k++)
					{
						AddInt(result, ref curBit, 0xFF, 8);
						for (int l = 0; l < 128; l++)
							AddIntReversed(result, ref curBit, compressedBytes[bytes[i + k * 128 + l]]);
					}
					if (rest >= 1)
					{
						AddInt(result, ref curBit, (uint)(0x80 | (rest - 1)), 8);
						for (int l = 0; l < rest; l++)
							AddIntReversed(result, ref curBit, compressedBytes[bytes[i + segmentCount * 128 + l]]);
					}
					i = j;
					if (j != bytes.Length)
						i--;
				}
			}

			return result.Take((curBit + 7) / 8).ToArray();
		}

		private static void AddIntReversed(byte[] array, ref int bitPos, CompressedByte compressed)
		{
			AddInt(array, ref bitPos, new CompressedByte
			{
				Value = ReverseBits(compressed.Value, compressed.Length),
				Length = compressed.Length
			});
		}

		private static void AddInt(byte[] array, ref int bitPos, CompressedByte compressed)
		{
			uint value = compressed.Value;
			int bitsCount = compressed.Length;
			AddInt(array, ref bitPos, compressed.Value, compressed.Length);
		}

		private static void AddInt(byte[] array, ref int bitPos, uint value, int bitsCount)
		{
			int curBytePos = bitPos / 8;
			int curBitInBytePos = bitPos % 8;

			int xLength = Math.Min(bitsCount, 8 - curBitInBytePos);
			if (xLength != 0)
			{
				byte x1 = (byte)(value << 32 - bitsCount >> 24 + curBitInBytePos);
				array[curBytePos] |= x1;

				curBytePos += (curBitInBytePos + xLength) / 8;
				curBitInBytePos = (curBitInBytePos + xLength) % 8;

				int x2Length = bitsCount - xLength;
				if (x2Length > 8)
					x2Length = 8;

				while (x2Length > 0)
				{
					xLength += x2Length;
					byte x2 = (byte)(value >> bitsCount - xLength << 8 - x2Length);
					array[curBytePos] |= x2;

					curBytePos += (curBitInBytePos + x2Length) / 8;
					curBitInBytePos = (curBitInBytePos + x2Length) % 8;

					x2Length = bitsCount - xLength;
					if (x2Length > 8)
						x2Length = 8;
				}
			}

			bitPos += bitsCount;
		}

		/*$HuffmanRleDecode*/

		public static byte[] Decode(HuffmanTree tree, byte[] bytes)
		{
			var root = tree.Root;
			var result = new List<byte>(bytes.Length * 2);
			int bitPos = 0;

			while (bytes.Length * 8 - bitPos > 8)
			{
				uint length = GetInt(bytes, ref bitPos, 8);

				if ((length & 0x80) == 0)
				{
					uint repeatCount = length + 2;
					byte value = GetValue(root, bytes, ref bitPos);
					for (int j = 0; j < repeatCount; j++)
						result.Add(value);
				}
				else
				{
					uint repeatCount = (0x7F & length) + 1;
					for (int j = 0; j < repeatCount; j++)
					{
						byte value = GetValue(root, bytes, ref bitPos);
						result.Add(value);
					}
				}
			}

			return result.ToArray();
		}

		private static uint GetInt(byte[] array, ref int bitPos, int bitsCount)
		{
			uint result = 0;

			int curBytePos = bitPos / 8;
			int curBitInBytePos = bitPos % 8;
			int xLength = Math.Min(bitsCount, 8 - curBitInBytePos);
			if (xLength != 0)
			{
				result = ((uint)array[curBytePos] << 56 + curBitInBytePos) >> 64 - xLength << bitsCount - xLength;

				curBytePos += (curBitInBytePos + xLength) / 8;
				curBitInBytePos = (curBitInBytePos + xLength) % 8;

				int x2Length = bitsCount - xLength;
				if (x2Length > 8)
					x2Length = 8;

				while (x2Length > 0)
				{
					xLength += x2Length;
					result |= (uint)array[curBytePos] >> 8 - x2Length << bitsCount - xLength;

					curBytePos += (curBitInBytePos + x2Length) / 8;
					curBitInBytePos = (curBitInBytePos + x2Length) % 8;

					x2Length = bitsCount - xLength;
					if (x2Length > 8)
						x2Length = 8;
				}
			}

			bitPos += bitsCount;
			return result;
		}

		private static byte GetValue(HuffmanTreeNode root, byte[] bytes, ref int bitPos)
		{
			int curBytePos = bitPos / 8;
			int curBitInBytePos = bitPos % 8;
			int bit;
			var curNode = root;
			while (curNode.Left != null)
			{
				bit = bytes[curBytePos] & (128 >> curBitInBytePos);
				if (bit == 0)
					curNode = curNode.Left;
				else
					curNode = curNode.Right;
				curBytePos += (curBitInBytePos + 1) / 8;
				curBitInBytePos = (curBitInBytePos + 1) % 8;
			}
			bitPos = curBytePos * 8 + curBitInBytePos;
			return curNode.ByteCount.Byte;
		}

		private static uint ReverseBits(uint n, int bitsCount)
		{
			n = ((n >> 1) & 0x55555555) | ((n & 0x55555555) << 1);
			n = ((n >> 2) & 0x33333333) | ((n & 0x33333333) << 2);
			n = ((n >> 4) & 0x0F0F0F0F) | ((n & 0x0F0F0F0F) << 4);
			n = ((n >> 8) & 0x00FF00FF) | ((n & 0x00FF00FF) << 8);
			n = (n >> 16) | (n << 16);
			n = n >> (32 - bitsCount);
			return n;
		}

		/*HuffmanRleDecode$*/
	}
}
