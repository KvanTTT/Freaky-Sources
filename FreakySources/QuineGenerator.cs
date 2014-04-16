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
		public const string Quotes1 = "\"";
		public const string Quotes2 = "'";
		public const string Backslash = "\\";
		public const string Newline = "\r\n";

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

		public bool Minified
		{
			get;
			set;
		}

		public QuineGenerator(string strName = "s",
			string printMethod = "System.Console.Write",
			string kernelPattern = "/*$print$*/",
			bool minified = true)
		{
			StrName = strName;
			PrintMethod = printMethod;
			KernelPattern = kernelPattern;
			Minified = minified;
		}

		public string Generate(string csharpCode, bool formatOutput = false, params QuineParam[] extraParams)
		{
			bool newlineEscaping = csharpCode.Contains("\r\n");
			bool backslashEscaping = newlineEscaping || csharpCode.Contains('\\');
			bool minified = Minified;
			string printMethod = PrintMethod;
			if (csharpCode.Contains("using System;"))
				printMethod = printMethod.Replace("System.", "");

			int number = 0;
			string strNumberString = "{" + number++ + "}";
			string quotes1NumberString = "{" + number++ + "}";
			string backslashNumberString = backslashEscaping ? "{" + number++ + "}" : "";
			string newlineNumberString = (!minified || newlineEscaping) ? "{" + number++ + "}" : "";
			string space = minified ? "" : " ";
			string indent = "";
			if (!minified)
			{
				int ind = csharpCode.IndexOf(KernelPattern);
				int newlineInd = csharpCode.LastIndexOf(Newline, ind);
				indent = new string('	', ind - newlineInd - Newline.Length);
			}
			var existedExtraParams = extraParams.Where(p => csharpCode.IndexOf(p.KeyBegin) != -1);

			var kernel = new StringBuilder();
			kernel.AppendFormat("var {0}{5}={5}{4}{3}{4};{6}{1}({0},{5}{0},{5}{2}{4}{2}",
				StrName, printMethod, Quotes2, strNumberString, quotes1NumberString, space, minified ? "" : newlineNumberString + indent);
			if (backslashEscaping)
				kernel.AppendFormat(",{2}{0}{1}{1}{0}", Quotes2, backslashNumberString, space);
			if (newlineEscaping)
				kernel.AppendFormat(",{2}{0}{1}r{1}n{0}", quotes1NumberString, backslashNumberString, space);
			foreach (var p in existedExtraParams)
			{
				string value = p.Value.Replace(Quotes1, quotes1NumberString);
				if (backslashEscaping)
					value = value.Replace(Backslash, backslashNumberString);
				if (newlineEscaping)
					value = value.Replace(Newline, newlineNumberString);
				kernel.AppendFormat(",{1}{0}", value, space);
			}
			kernel.Append(");");

			var str = new StringBuilder(csharpCode);
			str = str.Replace("{", "{{").Replace("}", "}}").Replace(Quotes1, quotes1NumberString);
			if (backslashEscaping)
				str = str.Replace(Backslash, backslashNumberString);
			if (newlineEscaping)
				str = str.Replace(Newline, newlineNumberString);
			str = str.Replace(KernelPattern, kernel.ToString());
			foreach (var p in existedExtraParams)
			{
				int beginInd = str.IndexOf(p.KeyBegin);
				int endInd = str.IndexOf(p.KeyEnd, beginInd);
				if (beginInd == endInd)
					str = str.Replace(p.KeyBegin, "{" + number++ + "}");
				else
				{
					str = str.Remove(beginInd, endInd + p.KeyEnd.Length - beginInd);
					str = str.Insert(beginInd, "{" + number++ + "}");
				}
			}

			var insertToResult = new StringBuilder();
			insertToResult.AppendFormat("var {0}{5}={5}{1}{2}{1};{6}{3}({0},{5}{0},{5}{4}{1}{4}",
				StrName, Quotes1, str, printMethod, Quotes2, space, minified ? "" : Newline + indent);
			if (backslashEscaping)
				insertToResult.AppendFormat(",{2}{0}{1}{1}{0}", Quotes2, Backslash, space);
			if (!minified || newlineEscaping)
				insertToResult.AppendFormat(",{2}{0}{1}r{1}n{0}", Quotes1, Backslash, space);
			foreach (var p in existedExtraParams)
				insertToResult.AppendFormat(",{1}{0}", p.Value, space);
			insertToResult.Append(");");

			var result = new StringBuilder(csharpCode);
			result = result.Replace(KernelPattern, insertToResult.ToString());
			foreach (var p in existedExtraParams)
			{
				int beginInd = result.IndexOf(p.KeyBegin);
				int endInd = result.IndexOf(p.KeyEnd, beginInd);
				if (beginInd == endInd)
					result = result.Replace(p.KeyBegin, p.KeySubstitute == "$key$" ? "" : p.KeySubstitute);
				else
				{
					if (p.KeySubstitute == "$key$")
						result = result.Replace(p.KeyBegin, "").Replace(p.KeyEnd, "");
					else
					{
						result = result.Remove(beginInd, endInd + p.KeyEnd.Length - beginInd);
						result = result.Insert(beginInd, p.KeySubstitute);
					}
				}
			}

			if (formatOutput)
				return new CSharpParser().Parse(result.ToString()).GetText();
			else
				return result.ToString();
		}
	}
}
