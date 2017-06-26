using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FreakySources
{
	public static class Helpers
	{
		public static bool HasNotErrors(this List<CheckingResult> results)
		{
			return results.Count == 1 && !results[0].IsError;
		}

		public static string GetCSharpCompilerPath()
		{
			return Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(), "csc.exe");
		}

		public static string GetJavaExePath(string exeName)
		{
			string result = CheckFile(Environment.SpecialFolder.ProgramFiles, true, exeName) ??
							CheckFile(Environment.SpecialFolder.ProgramFiles, false, exeName) ??
							CheckFile(Environment.SpecialFolder.ProgramFilesX86, true, exeName) ??
							CheckFile(Environment.SpecialFolder.ProgramFilesX86, false, exeName);

			return result;
		}

		private static string CheckFile(Environment.SpecialFolder specialFolder, bool jdk, string exeName)
		{
			var javaFilesDir = Path.Combine(Environment.GetFolderPath(specialFolder), "java");
			if (Directory.Exists(javaFilesDir))
			{
				var dirs = Directory.GetDirectories(javaFilesDir);

				dirs = dirs.Where(dir => Path.GetFileName(dir).StartsWith(jdk ? "jdk" : "jre")).ToArray();
				foreach (var dir in dirs)
				{
					var result = Path.Combine(dir, exeName);
					if (File.Exists(result))
					{
						return result;
					}
				}
			}

			return null;
		}
	}
}
