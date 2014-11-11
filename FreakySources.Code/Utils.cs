using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreakySources.Code
{
	/*#Utils*/

	public class Utils
	{
		/*#Ignore*/

		public static void AddIntReversed(byte[] array, ref int bitPos, CompressedByte compressed)
		{
			AddInt(array, ref bitPos, new CompressedByte
			{
				Value = ReverseBits(compressed.Value, compressed.Length),
				Length = compressed.Length
			});
		}

		public static void AddInt(byte[] array, ref int bitPos, CompressedByte compressed)
		{
			var value = compressed.Value;
			int bitsCount = compressed.Length;
			AddInt(array, ref bitPos, compressed.Value, compressed.Length);
		}

		public static void AddBytes(byte[] array, ref int bitPos, byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
				AddInt(array, ref bitPos, bytes[i], 8);
		}

		public static void AddInt(byte[] array, ref int bitPos, int value, int bitsCount)
		{
			int curBytePos = bitPos / 8;
			int curBitInBytePos = bitPos % 8;

			int xLength = Math.Min(bitsCount, 8 - curBitInBytePos);
			if (xLength != 0)
			{
				array[curBytePos] |= (byte)((uint)value << 32 - bitsCount >> 24 + curBitInBytePos);

				curBytePos += (curBitInBytePos + xLength) / 8;
				curBitInBytePos = (curBitInBytePos + xLength) % 8;

				int x2Length = bitsCount - xLength;
				if (x2Length > 8)
					x2Length = 8;

				while (x2Length > 0)
				{
					xLength += x2Length;
					array[curBytePos] |= (byte)((uint)value >> bitsCount - xLength << 8 - x2Length);

					curBytePos += (curBitInBytePos + x2Length) / 8;
					curBitInBytePos = (curBitInBytePos + x2Length) % 8;

					x2Length = bitsCount - xLength;
					if (x2Length > 8)
						x2Length = 8;
				}
			}

			bitPos += bitsCount;
		}

		/*Ignore#*/

		public static int GetInt(byte[] array, ref int bitPos, int bitsCount)
		{
			int result = 0;

			int curBytePos = bitPos / 8;
			int curBitInBytePos = bitPos % 8;
			int xLength = Math.Min(bitsCount, 8 - curBitInBytePos);
			if (xLength != 0)
			{
				result = (array[curBytePos] << 24 + curBitInBytePos) >> 32 - xLength << bitsCount - xLength;

				curBytePos += (curBitInBytePos + xLength) / 8;
				curBitInBytePos = (curBitInBytePos + xLength) % 8;

				int x2Length = bitsCount - xLength;
				if (x2Length > 8)
					x2Length = 8;

				while (x2Length > 0)
				{
					xLength += x2Length;
					result |= array[curBytePos] >> 8 - x2Length << bitsCount - xLength;

					curBytePos += (curBitInBytePos + x2Length) / 8;
					curBitInBytePos = (curBitInBytePos + x2Length) % 8;

					x2Length = bitsCount - xLength;
					if (x2Length > 8)
						x2Length = 8;
				}
			}

			bitPos += bitsCount;
			return result & ((1 << bitsCount) - 1);
		}

		public static byte GetValue(HuffmanTreeNode root, byte[] bytes, ref int bitPos)
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

		private static int ReverseBits(int n, int bitsCount)
		{
			uint k = (uint)n;
			uint r = k;
			int s = 31;
			for (k >>= 1; k != 0; k >>= 1)
			{
				r <<= 1;
				r |= k & 1;
				s--;
			}
			r = r << s >> 32 - bitsCount;
			return (int)r;
		}
	}

	/*Utils#*/
}
