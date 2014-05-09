using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Asciimation_1_1
{
    class P
    {
        const int FrameHeight = 13;
        const int FrameWidth = 67;
        static string Frames = /*%CompressedFramesGZipStream*/null/*CompressedFramesGZipStream%*/;
        static int CurrentFrame = /*$CurrentFrame*/0/*CurrentFrame$*/;

        static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    gZipStream.Read(buffer, 0, buffer.Length);

                return Encoding.UTF8.GetString(buffer);
            }
        }

        static void Main()
        {
            string[] lines = DecompressString(Frames).Split(new string[] { "\\n" }, StringSplitOptions.None);

            string[] frame = new string[FrameHeight];
            int ind = CurrentFrame * (FrameHeight + 1) + 1;
            for (int j = ind; j < ind + FrameHeight; j++)
                frame[j - ind] = lines[j].PadRight(FrameWidth, ' ');

            var output = new StringBuilder("//" + Environment.NewLine);
            for (int i = 0; i < FrameHeight; i++)
                output.AppendLine("//   " + frame[i]);

            CurrentFrame = (CurrentFrame + 1) % Frames.Length;

            /*@*/
        }
    }
}
/*$Output$*/