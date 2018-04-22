using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderDecompiler
{
	// Class for managing fixups that may need to be done after generating HLSL.
	public class PostFixups
	{
		public static string Apply(string hlsl)
		{
			return hlsl;
		}
	}
}
