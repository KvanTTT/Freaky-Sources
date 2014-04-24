using System;
using System.Text;
using System.Collections.Generic;

namespace Asciimation_1_2
{
	class Program
	{
		/*$ByteCounts*//*ByteCounts$*/

		/*$HuffmanTree*//*HuffmanTree$*/

		/*$DecodeBase64*//*DecodeBase64$*/

		/*$HuffmanRleDecode*//*HuffmanRleDecode$*/

		const int FrameHeight = 13;
		const int FrameWidth = 67;
		static string HuffmanTable = /*$HuffmanRleTable*/""/*HuffmanRleTable$*/;
		static string[] Frames = {
			/*$HuffmanRleFrames*/""/*HuffmanRleFrames$*/
		};
		static int CurrentFrame = /*$currentFrame*/0/*currentFrame$*/;

		static void Main()
		{
			var decodedTree = new HuffmanTree(DeserializeByteCount(DecodeBase64(HuffmanTable)));
			var line = Encoding.UTF8.GetString(Decode(decodedTree, DecodeBase64(Frames[CurrentFrame])));

			var output = new StringBuilder("//" + Environment.NewLine);
			for (int i = 0; i < FrameHeight; i++)
				output.AppendLine("//	" + line.Substring(i * FrameWidth, FrameWidth));

			CurrentFrame = (CurrentFrame + 1) % Frames.Length;

			/*$print$*/
		}
	}
}
/*$output$*/