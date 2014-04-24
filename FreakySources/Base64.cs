using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreakySources
{
	public class Base64
	{
		/*$DecodeBase64*/
		const string Alphabet64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		public static byte[] DecodeBase64(string str)
		{
			int lastSpecialInd = str.Length;
			while (str[lastSpecialInd - 1] == '=')
				lastSpecialInd--;
			int tailLength = str.Length - lastSpecialInd;

			int resultLength = (str.Length + 3) / 4 * 3 - tailLength;
			byte[] result = new byte[resultLength];

			int length4 = (str.Length - tailLength) / 4;
			int ind, x1, x2, x3, x4;
			int srcInd, dstInd;
			for (ind = 0; ind < length4; ind++)
			{
				srcInd = ind * 4;
				dstInd = ind * 3;
				x1 = Alphabet64.IndexOf(str[srcInd]);
				x2 = Alphabet64.IndexOf(str[srcInd + 1]);
				x3 = Alphabet64.IndexOf(str[srcInd + 2]);
				x4 = Alphabet64.IndexOf(str[srcInd + 3]);
				result[dstInd] = (byte)((x1 << 2) | ((x2 >> 4) & 0x3));
				result[dstInd + 1] = (byte)((x2 << 4) | ((x3 >> 2) & 0xF));
				result[dstInd + 2] = (byte)((x3 << 6) | (x4 & 0x3F));
			}

			switch (tailLength)
			{
				case 2:
					ind = length4;
					srcInd = ind * 4;
					dstInd = ind * 3;
					x1 = Alphabet64.IndexOf(str[srcInd]);
					x2 = Alphabet64.IndexOf(str[srcInd + 1]);
					result[dstInd] = (byte)((x1 << 2) | ((x2 >> 4) & 0x3));
					break;
				case 1:
					ind = length4;
					srcInd = ind * 4;
					dstInd = ind * 3;
					x1 = Alphabet64.IndexOf(str[srcInd]);
					x2 = Alphabet64.IndexOf(str[srcInd + 1]);
					x3 = Alphabet64.IndexOf(str[srcInd + 2]);
					result[dstInd] = (byte)((x1 << 2) | ((x2 >> 4) & 0x3));
					result[dstInd + 1] = (byte)((x2 << 4) | ((x3 >> 2) & 0xF));
					break;
			}

			return result;
		}

		/*DecodeBase64$*/
	}
}
