using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FreakySources
{
	public class CSharpChecker : CodeChecker
	{
		public override List<CheckingResult> CompileAndRun(string program)
		{
			CompilerResults compilerResults;
			var result = Compile(program, out compilerResults);

			if (!compilerResults.Errors.HasErrors)
			{
				Process process = null;
				try
				{
					process = SetupHiddenProcessAndRun(compilerResults.PathToAssembly);
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
				finally
				{
					if (process != null)
					{
						process.Dispose();
					}
				}
			}

			return result;
		}

		public override List<CheckingResult> Compile(string program)
		{
			CompilerResults compilerResults;
			return Compile(program, out compilerResults);
		}

		public List<CheckingResult> Compile(string program, out CompilerResults compilerResults)
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
	}
}