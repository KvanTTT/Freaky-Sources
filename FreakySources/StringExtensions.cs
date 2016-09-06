using CSharpMinifier;
using System;

namespace FreakySources
{
	public static class StringExtensions
	{
		public static CheckingResult CompareStrings(string s1, string s2)
		{
			int firstErrorLine = 0;
			int firstErrorColumn = 0;
			if (s1 == null || s2 == null)
			{
				firstErrorColumn = 0;
			}
			else
			{
				if (s1.Length != s2.Length)
				{
					firstErrorColumn = Math.Min(s1.Length, s2.Length);
				}
				else
				{
					for (int i = 0; i < s1.Length; i++)
						if (s1[i] != s2[i])
						{
							firstErrorColumn = i;
							goto exit;
						}
					firstErrorLine = -1;
					firstErrorColumn = -1;
				}
			}

			exit:
			return new CheckingResult
			{
				FirstErrorLine = firstErrorLine,
				FirstErrorColumn = firstErrorColumn,
				Output = firstErrorLine == -1 ? s1 : "",
				Description = firstErrorLine != -1 ? "Strings are not equal" : ""
			};
		}

		public static string ReverseString(string s)
		{
			char[] charArray = s.ToCharArray();
			Array.Reverse(charArray);
			return new string(charArray);
		}

		public static string RemoveSpacesInSource(string csharpCode)
		{
			var minifierOptions = new MinifierOptions(false);
			minifierOptions.SpacesRemoving = true;
			minifierOptions.NamespacesRemoving = true;
			var minifier = new Minifier(minifierOptions);
			return minifier.MinifyFromString(csharpCode);
		}

		public static string PrepareSingleLineCommentsPalindrome(string s)
		{
			return ReverseString(s) + "\r\n\r" + s;
		}

		public static string PrepareMultiLineCommentsPalindrome(string s)
		{
			return s + "/*/" + ReverseString(s);
		}

		public static CheckingResult CheckPalindrome(string s)
		{
			int firstErrorLine;
			int firstErrorColumn;
			for (int i = 0; i < s.Length / 2; i++)
			{
				if (s[i] != s[s.Length - 1 - i])
				{
					firstErrorLine = 0;
					firstErrorColumn = i;
					goto exit;
				}
			}
			firstErrorLine = -1;
			firstErrorColumn = -1;

			exit:
			return new CheckingResult
			{
				FirstErrorLine = firstErrorLine,
				FirstErrorColumn = firstErrorColumn,
				Output = firstErrorLine == -1 ? s : "",
				Description = firstErrorLine != -1 ? "String is not palindrome" : ""
			};
		}
	}
}
