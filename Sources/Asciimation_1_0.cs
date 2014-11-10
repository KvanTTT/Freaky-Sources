using System;
using System.Collections.Generic;
using System.Text;

namespace Asciimation_1_0
{
    class Program
    {
        static string[] Frames =  {
            /*%Frames*/"fyB/IH8gfyB/IH8gXyA=",
            "fyB/IB8gAVeDLkFTQwBJi01BVElPTi5DTy5OWn8gNyCHcHJlc2VudHN/IH8gaSA=",
            "fyB/IH8gfyB/IH8gXyA=",/*Frames%*/
        };
        static int CurrentFrame = /*$CurrentFrame*/0/*CurrentFrame$*/;
        static byte[] RleDecode(byte[] bytes)
        {
            var result = new List<byte>();
            int i = 0;
            while (i < bytes.Length)
            {
                if ((bytes[i] & 0x80) == 0)
                {
                    int repeatCount = bytes[i] + 2;
                    for (int j = 0; j < repeatCount; j++)
                        result.Add(bytes[i + 1]);
                    i = i + 2;
                }
                else
                {
                    int repeatCount = (0x7F & bytes[i]) + 1;
                    for (int j = 0; j < repeatCount; j++)
                        result.Add(bytes[i + 1 + j]);
                    i = i + 1 + repeatCount;
                }
            }
            return result.ToArray();
        }
        static void Main()
        {
            var frame = Encoding.UTF8.GetString(RleDecode(Convert.FromBase64String(Frames[CurrentFrame])));
            CurrentFrame = (CurrentFrame + 1) % Frames.Length;
            var frames = new StringBuilder();
            foreach (var k in Frames)
                frames.AppendFormat("{1}{0}{1},", f, '"');
            var output = new StringBuilder("//" + Environment.NewLine);
            for (int i = 0; i < 13; i++)
                output.AppendLine("//   " + frame.Substring(i * 67, 67));
            /*@*/
        }
    }
}
/*$Output$*/