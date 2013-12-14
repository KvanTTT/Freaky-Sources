using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpMinifier;

namespace FreakySources
{
	public class QuineGenerator
	{
		public string StrName
		{
			get;
			set;
		}
		
		public string PrintMethod
		{
			get;
			set;
		}
		
		public string KernelPattern
		{
			get;
			set;
		}
		
		public string Quotes1
		{
			get;
			protected set;
		}

		public string Quotes2
		{
			get;
			protected set;
		}

		public QuineGenerator(string strName = "s",
			string printMethod = "System.Console.Write",
			string kernelPattern = "/*$$$*/")
		{
			StrName = strName;
			PrintMethod = printMethod;
			KernelPattern = kernelPattern;
			Quotes1 = "\"";
			Quotes2 = "'";
		}

		public string Generate(string csharpCode, bool formatOutput = false, params Tuple<string, string>[] extraParams)
		{
			var minified = Checker.RemoveSpacesInSource(csharpCode);

			var kernel = new StringBuilder();
			kernel.AppendFormat("var {0}={{1}}{{0}}{{1}};{1}({0},{0},{2}{{1}}{2}", StrName, PrintMethod, Quotes2);
			foreach (var p in extraParams)
				kernel.Append("," + p.Item2);
			kernel.Append(");");

			var str = minified.Replace("{", "{{").Replace("}", "}}").Replace("\"", "{1}");
			str = str.Replace(KernelPattern, kernel.ToString());
			int number = 2;
			foreach (var p in extraParams)
				str = str.Replace(p.Item1, "{" + number++ + "}");

			var insertToResult = new StringBuilder();
			insertToResult.AppendFormat("var {0}={1}{2}{1};{3}({0},{0},{4}{1}{4}", StrName, Quotes1, str, PrintMethod, Quotes2);
			foreach (var p in extraParams)
				insertToResult.Append("," + p.Item2);
			insertToResult.Append(");");

			var result = minified.Replace(KernelPattern, insertToResult.ToString());

			if (formatOutput)
				result = new CSharpParser().Parse(result, "temp.cs").GetText();

			return result;
		}
	}
}
