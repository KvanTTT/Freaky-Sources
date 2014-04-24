using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit;

namespace FreakySources.Tests
{
	[TestFixture]
	public class CompressionTests
	{
		[Test]
		public void RleEncodeDecode()
		{
			byte[] bytes = new byte[] { 
				0, 0, 0, 0, 0, 0, 4, 2, 0, 4, 4, 4, 4, 4, 4, 4,
				80, 80, 80, 80, 0, 2, 2, 2, 2, 255, 255, 255, 255, 255, 0, 0
			};

			byte[] encoded = Rle.Encode(bytes);
			byte[] decoded = Rle.Decode(encoded);

			Assert.Less(encoded.Length, bytes.Length);
			Assert.AreEqual(bytes, decoded);
		}

		[Test]
		public void RleEncodeDecodeLongSequence()
		{
			byte[] bytes = new byte[] {
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
				
				255, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			};

			byte[] encoded = Rle.Encode(bytes);
			byte[] decoded = Rle.Decode(encoded);

			Assert.Less(encoded.Length, bytes.Length);
			Assert.AreEqual(bytes, decoded);
		}

		[Test]
		public void RleHuffmanEncodeDecode()
		{
			var generator = new AsciimationDataGenerator(File.ReadAllText(@"..\..\..\Sources\Asciimation.txt"));
			var bytesFreqs = generator.GetBytesFreqs();
			var tree = new HuffmanTree(bytesFreqs);

			for (int i = 0; i < generator.Frames.Length; i++)
			{
				var frame = generator.Frames[i];
				var orig = frame.Bytes;
				var encoded = HuffmanRle.Encode(tree, orig);
				var decoded = HuffmanRle.Decode(tree, encoded);
				CollectionAssert.AreEqual(orig, decoded);
			}
		}

		[Test]
		public void ConvertBase64()
		{
			var randomAr = new byte[100];
			var rand = new Random();
			rand.NextBytes(randomAr);

			var converted = Convert.ToBase64String(randomAr);
			CollectionAssert.AreEqual(randomAr, Base64.DecodeBase64(converted));
		}

		[Test]
		public void HuffmanRleFull()
		{
			var generator = new AsciimationDataGenerator(File.ReadAllText(@"..\..\..\Sources\Asciimation.txt"));
			var bytesFreqs = generator.GetBytesFreqs(true);
			var bytes = AsciimationDataGenerator.SerializeByteCount(bytesFreqs);
			var huffmanTable = Convert.ToBase64String(bytes);
			var tree = new HuffmanTree(bytesFreqs);

			var encodedTable = Convert.ToBase64String(bytes);
			var encodedFrames = new List<string>();
			for (int i = 0; i < generator.Frames.Length; i++)
			{
				var frame = generator.Frames[i];
				encodedFrames.Add(Convert.ToBase64String(HuffmanRle.Encode(tree, frame.Bytes)));
			}

			var decodedTree = new HuffmanTree(AsciimationDataGenerator.DeserializeByteCount(Base64.DecodeBase64(encodedTable)));
			var decodedFrames = new List<string>();
			for (int i = 0; i < encodedFrames.Count; i++)
			{
				var frame = HuffmanRle.Decode(decodedTree, Base64.DecodeBase64(encodedFrames[i]));
				CollectionAssert.AreEqual(generator.Frames[i].Bytes, frame);
			}
		}
	}
}
