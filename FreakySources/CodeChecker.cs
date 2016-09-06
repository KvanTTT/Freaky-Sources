using System.Collections.Generic;
using System.Diagnostics;

namespace FreakySources
{
	public abstract class CodeChecker
	{
		public List<CheckingResult> CheckPalindromeQuineProgram(string program)
		{
			var checkPalindromeResult = StringExtensions.CheckPalindrome(program);
			if (!checkPalindromeResult.IsError)
				return CheckQuineProgram(program);
			else
				return new List<CheckingResult>() { checkPalindromeResult };
		}

		public List<CheckingResult> CheckQuineProgram(string program)
		{
			var compileResult = CompileAndRun(program);
			if (compileResult.Count == 1 && compileResult[0].Output != null)
				return new List<CheckingResult>() {
					StringExtensions.CompareStrings(program, compileResult[0].Output)
				};
			else
				return compileResult;
		}

		public List<CheckingResult> CheckPalindromeProgram(string program)
		{
			var checkPalindromeResult = StringExtensions.CheckPalindrome(program);
			if (!checkPalindromeResult.IsError)
				return CompileAndRun(program);
			else
				return new List<CheckingResult>() { checkPalindromeResult };
		}

		public abstract List<CheckingResult> CompileAndRun(string program);

		public abstract List<CheckingResult> Compile(string program);

		protected Process SetupHiddenProcessAndRun(string fileName, string arguments = "", string workingDirectory = "")
		{
			var process = new Process();
			var startInfo = process.StartInfo;
			startInfo.FileName = fileName;
			startInfo.Arguments = arguments;
			if (workingDirectory != null)
			{
				startInfo.WorkingDirectory = workingDirectory;
			}
			startInfo.UseShellExecute = false;
			startInfo.CreateNoWindow = true;
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardOutput = true;
			process.Start();
			process.WaitForExit();
			return process;
		}
	}
}
