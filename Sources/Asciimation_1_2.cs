using System;
using System.Text;
using System.Collections.Generic;

namespace Asciimation_1_2
{
    class Program
    {
        /*#ByteCounts*//*ByteCounts#*/

        /*#HuffmanTree*//*HuffmanTree#*/

        /*#DecodeBase64*//*DecodeBase64#*/

        /*#HuffmanRleDecode*//*HuffmanRleDecode#*/

        /*#Utils*//*Utils#*/

        const int FrameHeight = 13;
        const int FrameWidth = 67;
        static string HuffmanTable = /*%HuffmanRleTable*/""/*HuffmanRleTable%*/;
        static string[] Frames = {
            /*%HuffmanRleFrames*/""/*HuffmanRleFrames%*/
        };
        static int CurrentFrame = /*$CurrentFrame*/0/*CurrentFrame$*/;

        static void Main()
        {
            var decodedTree = new HuffmanTree(DeserializeByteCount(Base64.DecodeBase64(HuffmanTable)));
            var bytes = Decode(decodedTree, Base64.DecodeBase64(Frames[CurrentFrame]));
            var chars = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                chars[i] = (char)bytes[i];
            var line = new string(chars);

            var output = new StringBuilder("//\r\n");
            for (int i = 0; i < FrameHeight; i++)
                output.AppendLine("//   " + line.Substring(i * FrameWidth, FrameWidth));

            CurrentFrame = (CurrentFrame + 1) % Frames.Length;

            /*@*/
        }
    }
}
/*$Output$*/