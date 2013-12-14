using CSharpMinifier;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FreakySources
{
	public class Checker
	{
		#region Palindrome Utils

		public static string PrepareSingleLineCommentsPalindrome(string s)
		{
			return ReverseString(s) + "\r\n\r" + s;
		}

		public static string PrepareMultiLineCommentsPalindrome(string s)
		{
			return s + "/*/" + ReverseString(s);
		}

		#endregion

		#region Checking Utils

		public static CheckingResult CheckPalindromeQuineProgram(string program)
		{
			CheckingResult result = new CheckingResult();
			CheckPalindrome(program, out result.FirstErrorLine, out result.FirstErrorColumn);
			if (!result.HasError)
			{
				result = CheckQuineProgram(program);
			}
			return result;
		}

		public static CheckingResult CheckQuineProgram(string program)
		{
			CheckingResult result = Compile(program);
			if (result.Output != null)
			{
				CompareStrings(program, result.Output, out result.FirstErrorLine, out result.FirstErrorColumn);
			}
			return result;
		}

		public static CheckingResult CheckPalindromeProgram(string program)
		{
			CheckingResult result = new CheckingResult();
			CheckPalindrome(program, out result.FirstErrorLine, out result.FirstErrorColumn);
			if (!result.HasError)
			{
				result = Compile(program);
			}
			return result;
		}

		public static CheckingResult Compile(string program)
		{
			CompilerResults compilerResults = null;
			using (CSharpCodeProvider provider = new CSharpCodeProvider())
			{
				compilerResults = provider.CompileAssemblyFromSource(new CompilerParameters(new string[]
				{
					"System.dll"
				})
				{
					GenerateExecutable = true
				}, new string[]
				{
					program
				});
			}
			CheckingResult result;
			if (compilerResults.Errors.HasErrors)
			{
				result.FirstErrorLine = compilerResults.Errors[0].Line;
				result.FirstErrorColumn = compilerResults.Errors[0].Column;
				result.Output = null;
			}
			else
			{
				try
				{
					Process process = Process.Start(new ProcessStartInfo(compilerResults.PathToAssembly)
					{
						UseShellExecute = false,
						CreateNoWindow = true,
						RedirectStandardInput = true,
						RedirectStandardOutput = true
					});
					process.StandardInput.WriteLine(Environment.NewLine);
					process.WaitForExit();
					string output = process.StandardOutput.ReadToEnd();
					try
					{
						File.Delete(compilerResults.PathToAssembly);
					}
					catch
					{
					}
					result.FirstErrorLine = -1;
					result.FirstErrorColumn = -1;
					result.Output = output;
				}
				catch
				{
					result.FirstErrorLine = 0;
					result.FirstErrorColumn = 0;
					result.Output = null;
				}
			}
			return result;
		}

		public static bool CheckPalindrome(string s, out int firstErrorLine, out int firstErrorColumn)
		{
			bool result;
			for (int i = 0; i < s.Length / 2; i++)
			{
				if (s[i] != s[s.Length - 1 - i])
				{
					firstErrorLine = 0;
					firstErrorColumn = i;
					result = false;
					return result;
				}
			}
			firstErrorLine = -1;
			firstErrorColumn = -1;
			result = true;
			return result;
		}

		#endregion

		#region String Utils

		public static bool CompareStrings(string s1, string s2, out int firstErrorLine, out int firstErrorColumn)
		{
			firstErrorLine = 0;
			bool result;
			if (s1 == null || s2 == null)
			{
				firstErrorColumn = 0;
				result = false;
			}
			else
			{
				if (s1.Length != s2.Length)
				{
					firstErrorColumn = Math.Min(s1.Length, s2.Length);
					result = false;
				}
				else
				{
					for (int i = 0; i < s1.Length; i++)
					{
						if (s1[i] != s2[i])
						{
							firstErrorColumn = i;
							result = false;
							return result;
						}
					}
					firstErrorLine = -1;
					firstErrorColumn = -1;
					result = true;
				}
			}
			return result;
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
			var minifier = new Minifier(minifierOptions);
			return minifier.MinifyFromString(csharpCode);
		}

		#endregion
	}
}