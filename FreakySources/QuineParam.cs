using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreakySources
{
	public class QuineParam
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

		public override string ToString()
		{
			return "Key:" + (KeyBegin != KeyEnd ? $"{KeyBegin}...{KeyEnd}" : KeyBegin) + $"; Value:{Value}; KeySubstitute:{KeySubstitute}";
		}
	}
}
