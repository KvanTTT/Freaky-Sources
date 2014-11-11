using System.CodeDom.Compiler;
using System.Diagnostics;
namespace SelfCompiliedProgram
{
	class P
	{
		static void Main()
		{
			var s = @"using System.CodeDom.Compiler;using System.Diagnostics;class P{static void Main(){var s = {0}{1}{0};var t = ;using (var p = new Microsoft.CSharp.CSharpCodeProvider()){var o = new CompilerParameters(new string[] { ""System.dll"" }) { GenerateExecutable = true };t = p.CompileAssemblyFromSource(o, s).PathToAssembly;}var i = new ProcessStartInfo(t) { UseShellExecute = false, CreateNoWindow = true };System.IO.File.Delete(t);}}";
			s = string.Format(s, s, '"');

			var t = "";
			using (var p = new Microsoft.CSharp.CSharpCodeProvider())
			{
				var o = new CompilerParameters(new string[] { "System.dll" }) { GenerateExecutable = true };
				var r = p.CompileAssemblyFromSource(o, s);
				t = r.PathToAssembly;
			}

			var i = new ProcessStartInfo(t) { UseShellExecute = false, CreateNoWindow = true };
			Process.Start(i).WaitForExit();
			System.IO.File.Delete(t);
		}
	}
}