using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FreakySources
{
	public class PhpChecker : CodeChecker
	{
		public string PhpPath { get; set; }

		public override List<CheckingResult> CompileAndRun(string program)
		{
			return Compile(program);
		}

		public override List<CheckingResult> Compile(string program)
		{
			Process process = null;
			string fileName = "";
			string phpFileName = "";
			var result = new List<CheckingResult>();
			if (result.Count == 0)
			{
				try
				{
					fileName = Path.GetTempFileName();
					phpFileName = fileName + ".php";
					File.WriteAllText(phpFileName, program);

					process = SetupHiddenProcessAndRun(PhpPath, $"-f {phpFileName}", Path.GetTempPath());
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
					if (File.Exists(phpFileName))
					{
						File.Delete(phpFileName);
					}

					if (process != null)
					{
						process.Dispose();
					}
				}
			}

			return result;
		}
	}
}
