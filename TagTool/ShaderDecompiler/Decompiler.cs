using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.ShaderDecompiler.Translations;
using TagTool.ShaderDecompiler.UcodeDisassembler;

namespace TagTool.ShaderDecompiler
{
	// Class for generating HLSL code from a List<Instruction>
	public class Decompiler
	{
		public static string Decompile(byte[] shader_data)
		{
			var instructions = Disassembler.Disassemble(shader_data);

			string parameters = "";
			string inputs = "";
			string outputs = "";
			string functions = "";
			string constants = "";
			string main = "";

			// HLSL generation here. Any code generation should be appended to the above stringvariables. If none of the
			// variables logically fit the generated code, add a new one, along with matching string interpolation
			// into the 'hlsl' string variable below. Rarely, some things may need be hardcoded into the 'hlsl' string 
			// variable, however this should be avoided unless it makes either the C# or HLSL output hard to read.

			constants +=
				"#define MAX_FLOAT 3.402823466e+38F		\n" +
				"#define INFINITY (1 / 0)				\n" +
				"#define NaN (0 / 0)					\n" +
				"#define ZERO 0							\n";

			string indent = "	";

			// Much more work is needed here.
			// TODO: handle all ControlFlowInstruction types.
			// Distinguish between ALU and Fetch instructions.
			foreach (var instr in instructions)
			{
				foreach (var cf_instr in instr.cf_instrs)
				{
					main += $"{indent}// {cf_instr.opcode}\n";

					if (cf_instr.Executes)
					{
						var alus = instructions.Skip((int)cf_instr.exec.address).Take((int)cf_instr.exec.count).ToArray();

						foreach (var alu in alus)
							main += $"{ALU.Get(alu, indent)}\n";
					}

					if (cf_instr.EndsShader)
						goto EndOfShader;
				}
			}
			EndOfShader:

			string hlsl =
				"// Decompiled with Jabukufo's ucode Decompiler \n" +
				$"{constants}								\n" +
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
				"	bool p0 = false;						\n" +
				"	int a0 = 0;								\n" +
				"	float4 aL = float4(0, 0, 0, 0);			\n" +
				"	float4 loop_count = float4(0, 0, 0, 0);	\n" +
				"	float4 ps = 0.0f;						\n" +
				"	OUTPUT Out;								\n" +
				"											\n" +
				$"{main}									\n" +
				"	return Out;								\n" +
				"}											";

			return PostFixups.Apply(hlsl);
		}
	}
}
