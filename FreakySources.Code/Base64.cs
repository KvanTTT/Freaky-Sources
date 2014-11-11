using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreakySources.Code
{
	/*#DecodeBase64*/

	public class Base64
	{
		public static byte[] DecodeBase64(string str)
		{
			var alphabet = new StringBuilder(64);
			int i = 65;
			while (i <= 90)
				alphabet.Append((char)i++);
			i = 97;
			while (i <= 122)
				alphabet.Append((char)i++);
			i = 48;
			while (i <= 57)
				alphabet.Append((char)i++);
			alphabet.Append('+');
			alphabet.Append('/');
			var alp = alphabet.ToString();

			int lastSpecialInd = str.Length;
			while (str[lastSpecialInd - 1] == '=')
				lastSpecialInd--;

			int resultLength = (str.Length + 3) / 4 * 3 - str.Length + lastSpecialInd;
			byte[] result = new byte[resultLength];

			int srcInd = 0, dstInd = 0;
			while (dstInd < resultLength)
			{
				int x1 = IndexOf(alp, str[srcInd++]);
				int x2 = IndexOf(alp, str[srcInd++]);
				result[dstInd++] = (byte)((x1 << 2) | ((x2 >> 4) & 3));
				if (dstInd < resultLength)
				{
					x1 = IndexOf(alp, str[srcInd++]);
					result[dstInd++] = (byte)((x2 << 4) | ((x1 >> 2) & 15));
					if (dstInd < resultLength)
					{
						x2 = IndexOf(alp, str[srcInd++]);
						result[dstInd++] = (byte)((x1 << 6) | (x2 & 63));
					}
				}
			}

			return result;
		}

		private static int IndexOf(string alphabet, char c)
		{
			return alphabet.IndexOf(c);
		}
	}

	/*DecodeBase64#*/
}
