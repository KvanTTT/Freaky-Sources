using System;
namespace SelfPrintingGameOfLife
{
	class G  /* GAME OF LIFE by Igor Ostrovsky */  {
		static string[] S ={
"############################################################################",
"#                                                               * *        #",
"#  ***                                                         *           #",
"#                                       *                      *           #",
"#                                     * *                      *  *        #",
"#                           **      **            **           ***         #",
"#                          *   *    **            **                       #",
"#               **        *     *   **                                     #",
"#               **        *   * **    * *                                  #",
"#                         *     *       *                                  #",
"#                          *   *                                           #",
"#                           **                                             #",
"#   **     **                                                              #",
"#    **   **                                        *  *                   #",
"# *  * * * *  *                                         *                  #",
"# *** ** ** ***                                     *   *                  #",
"#  * * * * * *                                       ****                  #",
"#   ***   ***                                                              #",
"#                                                                          #",
"#   ***   ***                                                              #",
"#  * * * * * *            *                                                #",
"# *** ** ** ***           *           *  *                             *** #",
"# *  * * * *  *                           *                           *  * #",
"#    **   **             ***          *   *                **            * #",
"#   **     **                          ****                              * #",
"#                                                                     * *  #",
"############################################################################",    
};
		static void Main()
		{
			string T = "\",r=\"using System;class G  /* GAME OF LIFE b" +
				"y Igor Ostrovsky \"+\"*/  {static string[]S={\\n\";int p=31,i,j,b,d;for(i=0;" +
				"i<27;i++){r+='\"'; for(j=0;j<76;j++){if(S[i][j]!='#'){b=0;for(d=0;d<9;d++)if" +
				"(S[i-1+d/3][j-1+d%3]=='*')b++;r+=b==3 ||(S[i][j]=='*'&&b==4)?'*':' ';} else " +
				"r+='#';}r+=\"\\\",\\n\";}r+=\"};static\"+\" void Main(){string T=\\\"\";fore" +
				"ach(var c in T){if(c=='\\\\'||c=='\"'){r+='\\\\';p++;} r+=c; if(++p>=77){r+=" +
				"\"\\\"+\\n\\\"\";p=1;}} foreach(var c in T){r+=c;if(++p%79==0)r+='\\n';}Cons" +
				"ole.Write(r);}}", r = "using System;class G  /* GAME OF LIFE by Igor Ostrovsky " +
				"*/  {static string[]S={\n"; int p = 31, i, j, b, d; for (i = 0; i < 27; i++)
			{
				r += '"'; for (j = 0;
	j < 76; j++)
				{
					if (S[i][j] != '#')
					{
						b = 0; for (d = 0; d < 9; d++) if (S[i - 1 + d / 3][j - 1 + d % 3] == '*') b++;
						r += b == 3 || (S[i][j] == '*' && b == 4) ? '*' : ' ';
					}
					else r += '#';
				} r += "\",\n";
			} r += "};static"
	+ " void Main(){string T=\""; foreach (var c in T)
			{
				if (c == '\\' || c == '"')
				{
					r += '\\'; p++
	;
				} r += c; if (++p >= 77) { r += "\"+\n\""; p = 1; }
			} foreach (var c in T)
			{
				r += c; if (++p % 79 == 0)
					r += '\n';
			} Console.Write(r);
		}
	}
}