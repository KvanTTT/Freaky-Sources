namespace SourceChecker
{
	public struct CheckingResult
	{
		public int FirstErrorLine;
		public int FirstErrorColumn;
		public string Output;
		public bool HasError
		{
			get
			{
				return this.FirstErrorColumn != -1;
			}
		}
	}
}