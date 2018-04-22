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
		public static string Decompile(byte[] shader_data)
		{
			var instructions = Disassembler.Disassemble(shader_data);
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
			$"{parameters}								\n" +
			"struct INPUT								\n" +
			"{											\n" +
			$"{inputs}									\n" +
			"};											\n" +
			"											\n" +
			"struct OUTPUT								\n" +
			"{											\n" +
			$"{outputs}									\n" +
			"};											\n" +
			$"{functions}								\n" +
			"OUTPUT main ( INPUT In )					\n" +
			"{											\n" +
			$"{constants}								\n" +
			"	bool p0 = false;						\n" +
			"	int a0 = 0;								\n" +
			"	float4 aL = float4(0, 0, 0, 0);			\n" +
			"	float4 loop_count = float4(0, 0, 0, 0);	\n" +
			"	float4 ps = 0.0f;						\n" +
			"	OUTPUT Out;								\n" +
			$"{logic}									\n" +
			"	return Out;								\n" +
			"}											";

			return PostFixups.Apply(hlsl);
		}
	}
}
