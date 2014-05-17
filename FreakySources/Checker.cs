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
		#region Checking Utils

		public static List<CheckingResult> CheckPalindromeQuineProgram(string program)
		{
			var checkPalindromeResult = CheckPalindrome(program);
			if (!checkPalindromeResult.IsError)
				return CheckQuineProgram(program);
			else
				return new List<CheckingResult>() { checkPalindromeResult };
		}

		public static List<CheckingResult> CheckQuineProgram(string program)
		{
			var compileResult = CompileAndRun(program);
			if (compileResult.Count == 1 && compileResult[0].Output != null)
				return new List<CheckingResult>() {
					CompareStrings(program, compileResult[0].Output)
				};
			else
				return compileResult;
		}

		public static List<CheckingResult> CheckPalindromeProgram(string program)
		{
			var checkPalindromeResult = CheckPalindrome(program);
			if (!checkPalindromeResult.IsError)
				return CompileAndRun(program);
			else
				return new List<CheckingResult>() { checkPalindromeResult };
		}

		public static List<CheckingResult> CompileAndRun(string program)
		{
			CompilerResults compilerResults;
			var result = Compile(program, out compilerResults);

			if (!compilerResults.Errors.HasErrors)
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
					process.WaitForExit(400);
					string output = process.StandardOutput.ReadToEnd();
					try
					{
						File.Delete(compilerResults.PathToAssembly);
					}
					catch
					{
					}
					result.Add(new CheckingResult
					{
						FirstErrorLine = -1,
						FirstErrorColumn = -1,
						Output = output,
						Description = null
					});
				}
				catch (Exception ex)
				{
					result.Add(new CheckingResult
					{
						FirstErrorLine = 0,
						FirstErrorColumn = 0,
						Output = null,
						Description = ex.Message
					});
				}
			}

			return result;
		}

		public static List<CheckingResult> Compile(string program)
		{
			CompilerResults compilerResults;
			return Compile(program, out compilerResults);
		}

		public static List<CheckingResult> Compile(string program, out CompilerResults compilerResults)
		{
			compilerResults = null;
			using (var provider = new CSharpCodeProvider())
			{
				compilerResults = provider.CompileAssemblyFromSource(new CompilerParameters(new string[]
				{
					"System.dll"
				})
				{
					GenerateExecutable = true
				},
				new string[]
				{
					program
				});
			}
			var result = new List<CheckingResult>();
			if (compilerResults.Errors.HasErrors)
			{
				for (int i = 0; i < compilerResults.Errors.Count; i++)
					result.Add(new CheckingResult
					{
						FirstErrorLine = compilerResults.Errors[i].Line,
						FirstErrorColumn = compilerResults.Errors[i].Column,
						Output = null,
						Description = compilerResults.Errors[i].ErrorText
					});
			}
			return result;
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

		#endregion

		#region String Utils

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

		#endregion
	}
}