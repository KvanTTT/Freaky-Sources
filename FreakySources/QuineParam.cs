namespace FreakySources
{
	public class QuineParam
	{
		public string KeyBegin { get; set; } = "";
		public string KeyEnd { get; set; } = "";
		public string Value { get; set; } = "";
		public string KeySubstitute { get; set; } = "";

		public QuineParam(string keyBegin, string keyEnd, string value, string keySubstitute = "")
		{
			KeyBegin = keyBegin ?? "";
			KeyEnd = keyEnd ?? "";
			Value = value ?? "";
			KeySubstitute = keySubstitute ?? "";
		}

		public QuineParam()
		{
		}

		public override string ToString()
		{
			return "Key:" + (KeyBegin != KeyEnd ? $"{KeyBegin}...{KeyEnd}" : KeyBegin) + $"; Value:{Value}; KeySubstitute:{KeySubstitute}";
		}
	}
}
