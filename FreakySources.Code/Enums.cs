using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreakySources.Code
{
	/*#Enums*/

	public enum FrameType
	{
		Basic,
		Transitional,
		TransitionalLeft,
		TransitionalRight,
		TransitionalTop,
		TransitionalBottom
	}

	public enum FrameChangeType
	{
		One = 0,
		Horizontal = 1,
		Vertical = 2
	}

	/*Enums#*/

	public class FrameChange
	{
		public FrameChangeType Type;
		public int X;
		public int Y;
		public int Length;
		public List<char> Chars;
	}
}
