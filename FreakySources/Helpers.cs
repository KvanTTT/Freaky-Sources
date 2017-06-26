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
			if (IsRunningOnLinux)
			{
				return Path.GetFileNameWithoutExtension(exeName);
			}

			string dir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			string programFilesDir, programFilesX86Dir;
			string x86postfix = " (x86)";
			if (dir.EndsWith(x86postfix))
			{
				programFilesDir = dir.Remove(dir.Length - x86postfix.Length);
				programFilesX86Dir = dir;
			}
			else
			{
				programFilesDir = dir;
				programFilesX86Dir = dir + x86postfix;
			}
			string result = CheckFile(programFilesDir, true, exeName) ??
							CheckFile(programFilesDir, false, exeName) ??
							CheckFile(programFilesX86Dir, true, exeName) ??
							CheckFile(programFilesX86Dir, false, exeName);

			return result;
		}

		private static string CheckFile(string specialFolder, bool jdk, string exeName)
		{
			var javaFilesDir = Path.Combine(specialFolder, "java");
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

		public static bool IsRunningOnLinux
		{
			get
			{
				int p = (int)Environment.OSVersion.Platform;
				return (p == 4) || (p == 6) || (p == 128);
			}
		}
	}
}
