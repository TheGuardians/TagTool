using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderDecompiler
{
	// Class for managing fixups that may need to be done after generating HLSL.
	// Dany's research on Registers/rmt2 https://gist.github.com/dany5639/15b8692fa81cd65d05d07196bd24b225
	public class PostFixups
	{
		public static string Apply(string hlsl)
		{
			return hlsl;
		}
	}
}
