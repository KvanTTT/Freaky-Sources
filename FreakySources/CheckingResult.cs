using System.Collections.Generic;

namespace FreakySources
{
	public class CheckingResult
	{
		public int FirstErrorLine
		{
			get;
			set;
		}

		public int FirstErrorColumn
		{
			get;
			set;
		}

		public string Output
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public bool IsError
		{
			get
			{
				return FirstErrorColumn != -1 && FirstErrorLine != -1;
			}
		}

		public CheckingResult()
		{
		}

		public CheckingResult(CheckingResult result)
		{
			FirstErrorLine = result.FirstErrorLine;
			FirstErrorColumn = result.FirstErrorColumn;
			Output = result.Output;
			Description = result.Description;
		}
	}
}