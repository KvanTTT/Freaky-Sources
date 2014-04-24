using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FreakySources
{
	public class CodeDataGeneratorParam
	{
		public string KeyBegin;
		public string KeyEnd;
		public string Value;

		public CodeDataGeneratorParam()
		{
		}

		public CodeDataGeneratorParam(string key)
		{
			KeyBegin = key;
			KeyEnd = key;
		}

		public CodeDataGeneratorParam(string keyBegin, string keyEnd)
		{
			KeyBegin = keyBegin;
			KeyEnd = keyEnd;
		}

		public CodeDataGeneratorParam(string keyBegin, string keyEnd, string value)
		{
			KeyBegin = keyBegin;
			KeyEnd = keyEnd;
			Value = value;
		}
	}

	public class CodeDataGenerator
	{
		private const int SpacesInTab = 4;
		private static string TabSpaces = new string(' ', SpacesInTab);
		private List<string> Codes;

		public bool SaveKeys
		{
			get;
			set;
		}

		public bool IgnoreIndents
		{
			get;
			set;
		}

		public string KeyBeginPattern
		{
			get;
			set;
		}

		public string KeyEndPattern
		{
			get;
			set;
		}

		public CodeDataGenerator(string codeFilesDir)
		{
			Codes = new List<string>();
			var files = Directory.GetFiles(codeFilesDir, "*.cs");
			foreach (var file in files)
				Codes.Add(File.ReadAllText(file));

			KeyBeginPattern = @"\/\*\$(\w+)\*\/";
			KeyEndPattern = @"\/\*(\w+)\$\*\/";
		}

		public string SubstituteCode(string source)
		{
			var codeDataGeneratorParams = new List<CodeDataGeneratorParam>();

			var matches = Regex.Matches(source, KeyBeginPattern);
			foreach (Match match in matches)
			{
				var endMatch = Regex.Unescape(KeyEndPattern.Replace(@"(\w+)", match.Groups[1].Value));
				codeDataGeneratorParams.Add(new CodeDataGeneratorParam(match.Value, endMatch));
			}

			var result = new StringBuilder(source);
			foreach (var p in codeDataGeneratorParams)
			{
				var code = SearchCode(p);
				SubstituteParam(result, new CodeDataGeneratorParam(p.KeyBegin, p.KeyEnd, code));
			}
			return result.ToString();
		}

		public string SubstituteData(string source, List<CodeDataGeneratorParam> codeDataGeneratorParams)
		{
			var result = new StringBuilder(source);
			foreach (var p in codeDataGeneratorParams)
			{
				SubstituteParam(result, p);
			}
			return result.ToString();
		}

		private void SubstituteParam(StringBuilder source, CodeDataGeneratorParam p)
		{
			if (p.Value != null)
			{
				int beginInd = source.IndexOf(p.KeyBegin);
				if (beginInd != -1)
				{
					int endInd = source.IndexOf(p.KeyEnd, beginInd);

					int ind, length;
					if (!SaveKeys)
					{
						ind = beginInd;
						length = endInd + p.KeyEnd.Length - ind;
					}
					else
					{
						ind = beginInd + p.KeyBegin.Length;
						length = endInd - ind;
					}
					source = source.Remove(ind, length);

					if (!IgnoreIndents)
						source = InsertWithIndents(source, ind, p.Value);
					else
						source = source.Insert(ind, p.Value);
				}
			}
		}

		private static StringBuilder InsertWithIndents(StringBuilder source, int ind, string code)
		{
			string sourceIndents;
			if (ind > 0)
			{
				int i = ind - 1;
				bool b = char.IsWhiteSpace('\n');
				while (i >= 0 && char.IsWhiteSpace(source[i]) && source[i] != '\n' && source[i] != '\r')
					i--;
				sourceIndents = source.GetSubstring(i + 1, ind - (i + 1)).ToString();
			}
			else
				sourceIndents = "";
			bool tabs = sourceIndents.Contains('\t');

			string[] codeLines = code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			
			int removedIndentsInd = 0;
			while (char.IsWhiteSpace(codeLines[0][removedIndentsInd]))
				removedIndentsInd++;
			
			for (int j = 0; j < codeLines.Length; j++)
			{
				if (codeLines[j] != "")
				{
					codeLines[j] = codeLines[j].Substring(removedIndentsInd);
					var codeLine = codeLines[j];
					int i = 0;
					while (i < codeLine.Length && char.IsWhiteSpace(codeLine[i]))
						i++;
					if (i > 0)
					{
						string codeIndents = codeLine.Remove(i);
						if (tabs)
							codeIndents = codeIndents.Replace(TabSpaces, "\t");
						else
							codeIndents = codeIndents.Replace("\t", TabSpaces);

						codeLines[j] = codeIndents + codeLine.Substring(i);
					}
				}
			}
			string indentedCode = string.Join(Environment.NewLine + sourceIndents, codeLines);

			return source.Insert(ind, indentedCode);
		}

		private string SearchCode(CodeDataGeneratorParam param)
		{
			foreach (var code in Codes)
			{
				int beginInd = code.IndexOf(param.KeyBegin);
				if (beginInd != -1)
				{
					int i = beginInd + param.KeyBegin.Length;
					while (i < code.Length && char.IsWhiteSpace(code[i]))
						i++;
					i--;
					while (i >= 0 && char.IsWhiteSpace(code[i]) && code[i] != '\r' && code[i] != '\n')
						i--;
					beginInd = i + 1;
					int endInd = code.IndexOf(param.KeyEnd, beginInd);
					return code.Substring(beginInd, endInd - beginInd).TrimEnd();
				}
			}
			return null;
		}
	}
}
