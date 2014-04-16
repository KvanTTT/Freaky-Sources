using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreakySources
{
	public static class ExtensionsHelper
	{
		public static int IndexOf(this StringBuilder sb, string value, int startIndex = 0, bool ignoreCase = false)
		{
			int index;
			int length = value.Length;
			int maxSearchLength = (sb.Length - length) + 1;

			if (ignoreCase)
			{
				for (int i = startIndex; i < maxSearchLength; ++i)
				{
					if (char.ToLower(sb[i]) == char.ToLower(value[0]))
					{
						index = 1;
						while (index < length && char.ToLower(sb[i + index]) == char.ToLower(value[index]))
							++index;

						if (index == length)
							return i;
					}
				}

				return -1;
			}

			for (int i = startIndex; i < maxSearchLength; ++i)
			{
				if (sb[i] == value[0])
				{
					index = 1;
					while (index < length && sb[i + index] == value[index])
						++index;

					if (index == length)
						return i;
				}
			}

			return -1;
		}

		public static StringBuilder Remove(this StringBuilder sb, int ind)
		{
			return sb.Remove(ind, sb.Length - ind);
		}

		public static StringBuilder Substring(this StringBuilder sb, int ind)
		{
			return sb.Remove(0, ind);
		}

		public static StringBuilder Substring(this StringBuilder sb, int ind, int length)
		{
			return sb.Remove(ind + length).Remove(0, ind);
		}
	}
}
