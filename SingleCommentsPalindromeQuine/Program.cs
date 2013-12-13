using System;
class P
{
	static void Main()
	{
		var s = "using System;class P{{static void Main(){{var s={1}{0}{1};s=string.Format(s,s,'{1}','{2}{2}');var c=s.ToCharArray();Array.Reverse(c);Console.Write(new string(c)+{1}{2}r{2}n{2}r{1}+s);}}}}//";
		s = string.Format(s, s, '"', '\\');
		var c = s.ToCharArray();
		Array.Reverse(c);
		Console.Write(new string(c) + "\r\n\r" + s);
	}
}
//