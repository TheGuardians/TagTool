using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.ShaderDecompiler.UcodeDisassembler;

namespace TagTool.ShaderDecompiler
{
	// Class for generating HLSL code from a List<Instruction>
	public class Decompiler
	{
		public static string Decompile(List<Instruction> instructions)
		{
			PreFixups.Apply(ref instructions);

			string parameters = "";
			string inputs = "";
			string outputs = "";
			string functions = "";
			string constants = "";
			string logic = "";
			// HLSL generation here. Any code generation should be appended to the above stringvariables. If none of the
			// variables logically fit the generated code, add a new one, along with matching string interpolation
			// into the 'hlsl' string variable below. Rarely, some things may need be hardcoded into the 'hlsl' string 
			// variable, however this should be avoided unless it makes either the C# or HLSL output hard to read.


			string hlsl = 
			$"{parameters}          " +
			"struct INPUT           " +
			"{                      " +
			$"{inputs}              " +
			"};                     " +
			"struct OUTPUT          " +
			"{                      " +
			$"{outputs}             " +
			"};                     " +
			$"{functions}           " +
			"OUTPUT main(INPUT In ) " +
			"{                      " +
			$"{constants}           " +
			"OUTPUT Out;            " +
			$"{logic}               " +
			"return Out;            " +
			"}                      ";

			return PostFixups.Apply(hlsl);
		}
	}
}
