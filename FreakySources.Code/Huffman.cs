using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreakySources.Code
{
	/*#HuffmanTree*/

	public class ByteCount
	{
		public byte Byte;
		public int Count;

		public override string ToString()
		{
			return "Byte: " + Byte + "; Count: " + Count;
		}
	}

	public class CompressedByte
	{
		public int Value;
		public int Length;

		public CompressedByte()
		{
		}

		public CompressedByte(CompressedByte CompressByte)
		{
			Value = CompressByte.Value;
			Length = CompressByte.Length;
		}

		public override string ToString()
		{
			return "Value: " + Value + "; Length: " + Length;
		}
	}

	public class HuffmanTreeNode
	{
		public HuffmanTreeNode Left, Right, Parent;
		public ByteCount ByteCount;

		public HuffmanTreeNode()
		{
		}

		public HuffmanTreeNode(HuffmanTreeNode left, HuffmanTreeNode right)
		{
			Left = left;
			Right = right;
			Left.Parent = Right.Parent = this;
			if (ByteCount == null)
				ByteCount = new ByteCount();
			ByteCount.Count = Left.ByteCount.Count + Right.ByteCount.Count;
		}

		public override string ToString()
		{
			return (Left == null && Right == null) ? ByteCount.ToString() : ("Count: " + ByteCount.Count.ToString());
		}
	}

	public class HuffmanTree
	{
		public HuffmanTreeNode Root;
		public Dictionary<int, CompressedByte> CompressedBytes;

		public HuffmanTree(ByteCount[] bytesFreqs)
		{
			var nodes = new List<HuffmanTreeNode>();
			for (int i = 0; i < bytesFreqs.Length; i++)
				nodes.Add(new HuffmanTreeNode
				{
					ByteCount = new ByteCount
					{
						Byte = bytesFreqs[i].Byte,
						Count = bytesFreqs[i].Count
					}
				});

			HuffmanTreeNode left, right;
			while (nodes.Count > 1)
			{
				int min = nodes[0].ByteCount.Count;
				int minInd = 0;
				for (int j = 1; j < nodes.Count; j++)
					if (min > nodes[j].ByteCount.Count)
					{
						min = nodes[j].ByteCount.Count;
						minInd = j;
					}
				left = nodes[minInd];
				nodes.RemoveAt(minInd);

				min = nodes[0].ByteCount.Count;
				minInd = 0;
				for (int j = 1; j < nodes.Count; j++)
					if (min > nodes[j].ByteCount.Count)
					{
						min = nodes[j].ByteCount.Count;
						minInd = j;
					}
				right = nodes[minInd];
				nodes.RemoveAt(minInd);

				nodes.Add(new HuffmanTreeNode(left, right));
			}

			Root = nodes[0];
			CalculateBytes();
		}

		private void CalculateBytes()
		{
			CompressedBytes = new Dictionary<int, CompressedByte>();
			TraverseTree(Root, 0, 0);
		}

		private void TraverseTree(HuffmanTreeNode node, int length, int value)
		{
			if (node.Left != null)
			{
				length++;
				TraverseTree(node.Left, length, value);
				value |= (1 << (length - 1));
				TraverseTree(node.Right, length, value);
			}
			else
				CompressedBytes[node.ByteCount.Byte] = new CompressedByte { Length = length, Value = value };
		}
	}

	/*HuffmanTree#*/
}
