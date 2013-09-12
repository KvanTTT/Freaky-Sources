/**/
using System;
class P
{
	static void Main()
	{
        var s = "{2}**/using System;class P{{static void Main(){{var s={1}{0}{1};s=string.Format(s,s,'{1}','{2}');var c = s.ToCharArray();Array.Reverse(c);Console.Write(s+'*'+new string(c));}}}}/";
		s = string.Format(s, s, '"', '/');
		var c = s.ToCharArray();
		Array.Reverse(c);
		Console.Write(s + '*' + new string(c));
	}
}