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
	public struct QuineParam
	{
		public string KeyBegin;
		public string KeyEnd;
		public string Value;
		public string KeySubstitute;

		public QuineParam(string key, string value, string keySubstitute)
		{
			KeyBegin = key;
			KeyEnd = key;
			Value = value;
			KeySubstitute = keySubstitute;
		}

		public QuineParam(string keyBegin, string keyEnd, string value, string keySubstitute)
		{
			KeyBegin = keyBegin;
			KeyEnd = keyEnd;
			Value = value;
			KeySubstitute = keySubstitute;
		}
	}

	public class QuineGenerator
	{
		private const string Quotes1 = "\"";
		private const string Quotes2 = "'";
		private const string Backslash = "\\";

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

		public QuineGenerator(string strName = "s",
			string printMethod = "System.Console.Write",
			string kernelPattern = "/*$$$*/")
		{
			StrName = strName;
			PrintMethod = printMethod;
			KernelPattern = kernelPattern;
		}

		public string Generate(string csharpCode, bool formatOutput = false, params QuineParam[] extraParams)
		{
			var minified = Checker.RemoveSpacesInSource(csharpCode);

			var kernel = new StringBuilder();
			kernel.AppendFormat("var {0}={{1}}{{0}}{{1}};{1}({0},{0},{2}{{1}}{2}", StrName, PrintMethod, Quotes2);
			foreach (var p in extraParams)
				kernel.Append("," + p.Value);
			kernel.Append(");");

			var str = minified.Replace("{", "{{").Replace("}", "}}").Replace("\"", "{1}");
			str = str.Replace(KernelPattern, kernel.ToString());
			int number = 2;
			foreach (var p in extraParams)
			{
				int beginInd = str.IndexOf(p.KeyBegin);
				int endInd = str.IndexOf(p.KeyEnd, beginInd);
				if (beginInd == endInd)
					str = str.Replace(p.KeyBegin, "{" + number++ + "}");
				else
					str = string.Format("{0}{{{1}}}{2}", str.Remove(beginInd), number++, str.Substring(endInd + p.KeyEnd.Length));
			}

			var insertToResult = new StringBuilder();
			insertToResult.AppendFormat("var {0}={1}{2}{1};{3}({0},{0},{4}{1}{4}", StrName, Quotes1, str, PrintMethod, Quotes2);
			foreach (var p in extraParams)
				insertToResult.Append("," + p.Value);
			insertToResult.Append(");");

			var result = minified.Replace(KernelPattern, insertToResult.ToString());
			foreach (var p in extraParams)
			{
				int beginInd = result.IndexOf(p.KeyBegin);
				int endInd = result.IndexOf(p.KeyEnd, beginInd);
				if (beginInd == endInd)
					result = result.Replace(p.KeyBegin, p.KeySubstitute);
				else
				{
					if (p.KeySubstitute == "$key$")
						result = result.Replace(p.KeyBegin, "").Replace(p.KeyEnd, "");
					else
						result = string.Format("{0}{1}{2}",
							result.Remove(beginInd), p.KeySubstitute, result.Substring(endInd + p.KeyEnd.Length));
				}
			}

			if (formatOutput)
				result = new CSharpParser().Parse(result, "temp.cs").GetText();

			return result;
		}
	}
}
