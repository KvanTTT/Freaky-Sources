using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FreakySources
{
	public class JavaChecker : CodeChecker
	{
		public string JavaCompilerPath { get; set; }

		public string JavaPath { get; set; }

		public string ClassName { get; set; }

		public override List<CheckingResult> CompileAndRun(string program)
		{
			var result = Compile(program, false);

			Process process = null;
			if (result.Count == 0)
			{
				try
				{
					process = SetupHiddenProcessAndRun(JavaPath, ClassName, Path.GetTempPath());
					var output = process.StandardOutput.ReadToEnd();
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
					string classFileName = Path.Combine(Path.GetTempPath(), ClassName + ".class");
					if (File.Exists(classFileName))
					{
						File.Delete(classFileName);
					}

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
			return Compile(program, true);
		}

		private List<CheckingResult> Compile(string program, bool removeClassFile)
		{
			Process process = null;
			string fileName = "";
			string javaFileName = "";
			var result = new List<CheckingResult>();
			try
			{
				fileName = Path.GetTempFileName();
				javaFileName = fileName + ".java";
				File.WriteAllText(javaFileName, program);

				process = SetupHiddenProcessAndRun(JavaCompilerPath, "\"" + javaFileName + "\"", Path.GetTempPath());
				var errorsLines = process.StandardError.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var line in errorsLines)
				{
					if (line.Contains(": error:"))
					{
						int errorStrIndex = line.IndexOf(": error:");
						int firstNotFileColonIndex = line.LastIndexOf(':', errorStrIndex - 1);
						int codeLine = int.Parse(line.Substring(firstNotFileColonIndex + 1, errorStrIndex - firstNotFileColonIndex - 1));
						result.Add(new CheckingResult
						{
							FirstErrorLine = codeLine,
							FirstErrorColumn = 0,
							Output = null,
							Description = line
						});
					}
				}
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
				if (File.Exists(javaFileName))
				{
					File.Delete(javaFileName);
				}

				string classFileName = Path.Combine(Path.GetTempPath(), ClassName + ".class");
				if (removeClassFile && File.Exists(classFileName))
				{
					File.Delete(classFileName);
				}

				if (process != null)
				{
					process.Dispose();
				}
			}

			return result;
		}
	}
}
