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
	public static class Decompiler
	{
			public static string Constants = "";
			public static string Parameters = "";
			public static string Inputs = "";
			public static string Outputs = "";
			public static string Functions = "";
			public static string Main = "";
			public static string INDENT = "	";


		public static string Decompile(byte[] shader_data)
		{
			var instructions = Disassembler.Disassemble(shader_data);
			Constants = "";
			Parameters = "";
			Inputs = "";
			Outputs = "";
			Functions = "";
			Main = "";

			// HLSL generation here. Any code generation should be appended to the above stringvariables. If none of the
			// variables logically fit the generated code, add a new one, along with matching string interpolation
			// into the 'hlsl' string variable below. Rarely, some things may need be hardcoded into the 'hlsl' string 
			// variable, however this should be avoided unless it makes either the C# or HLSL output hard to read.

			Constants +=
				"#define MAX_FLOAT 3.402823466e+38F		\n" +
				"#define INFINITY (1 / 0)				\n" +
				"#define NaN (0 / 0)					\n" +
				"#define ZERO 0							\n";

			// Much more work is needed here.
			// TODO: handle all ControlFlowInstruction types.
			// Distinguish between ALU and Fetch instructions.
			foreach (var instr in instructions)
			{
				foreach (var cf_instr in instr.cf_instrs)
				{
					Main += $"{INDENT}// {cf_instr.opcode}\n";

					if (cf_instr.Executes)
					{
						var alus = instructions.Skip((int)cf_instr.exec.address).Take((int)cf_instr.exec.count).ToArray();

						foreach (var alu in alus)
						{
							ALU.Get(alu);
							// Main += $"{INDENT}{alu.alu_instr.GetVectorAsmString()}\n";
							// Main += $"{INDENT}{alu.alu_instr.GetScalarAsmString()}\n";
						}
					}

					if (cf_instr.EndsShader)
						goto EndOfShader;
				}
			}
			EndOfShader:

			string hlsl =
				"// Decompiled with Jabukufo's ucode Decompiler \n" +
				"/*---------------------------------------*/\n" +
				" // Constants:								\n" +
				$"{Constants}								\n" +
				"/*---------------------------------------*/\n" +
				" // Parameters:							\n" +
				$"{Parameters}								\n" +
				"/*---------------------------------------*/\n" +
				" // Inputs:								\n" +
				"struct INPUT								\n" +
				"{											\n" +
				$"{Inputs}									\n" +
				"};											\n" +
				"/*---------------------------------------*/\n" +
				" // Outputs:								\n" +
				"struct OUTPUT								\n" +
				"{											\n" +
				$"{Outputs}									\n" +
				"};											\n" +
				"/*---------------------------------------*/\n" +
				" // Variables:								\n" +
				"bool   p0 = false; // predicate 'register'.\n" +
				"int    a0 = 0; // address 'register'.		\n" +
				"float4 aL = 0; // loop 'register'.			\n" +
				"float4 lc = 0; // loop count.				\n" +
				"float4 ps = 0; // previous scalar result,	\n" +
				"float4 pv = 0; // previous vector result.	\n" +
				"/*---------------------------------------*/\n" +
				" // Functions:								\n" +
				$"{Functions}								\n" +
				"/*---------------------------------------*/\n" +
				" // Main:									\n" +
				"OUTPUT main ( INPUT In )					\n" +
				"{											\n" +
				"	OUTPUT Out;								\n" +
				"											\n" +
				$"{Main}									\n" +
				"	return Out;								\n" +
				"}											";

			return PostFixups.Apply(hlsl);
		}
	}
}
