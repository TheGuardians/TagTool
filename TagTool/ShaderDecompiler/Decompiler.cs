using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.ShaderDecompiler.ConstantData;
using TagTool.ShaderDecompiler.Translations;
using TagTool.ShaderDecompiler.UcodeDisassembler;
using TagTool.ShaderDecompiler.UPDB;

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

		public static Dictionary<int, int> AllocTemps = new Dictionary<int, int> { };
		public static Dictionary<int, int> AllocConsts = new Dictionary<int, int> { };
		public static Dictionary<int, int> AllocBools = new Dictionary<int, int> { };
		public static Dictionary<int, int> AllocInts = new Dictionary<int, int> { };

		public static string Decompile(byte[] debugData, byte[] constantData, byte[] shaderData)
		{
			var shaderPdb = Generator.GetShaderpdb(debugData, constantData, shaderData);
			var instructions = Disassembler.Disassemble(shaderData);

			Constants = "";
			Parameters = "";
			Inputs = "";
			Outputs = "";
			Functions = "";
			Main = "";

			// Constants that are commonly used by instructions.
			Constants +=
				"#define MAX_FLOAT 3.402823466e+38F		\n" +
				"#define INFINITY (1 / 0)				\n" +
				"#define NaN (0 / 0)					\n" +
				"#define ZERO 0							\n";

			// declaring these as arrays lets us not even worry about parameters on the HLSL side
			// and they will point to the correct register index that the game stores values in.
			// Example: if we disassemble an instruction 'adds r0, c19.xy', we can replace that in HLSL with
			// r0 = c[19].x + c[19].y - VERY SIMPLE!
			Parameters +=
				"float4 c[224];\n" +
				"int i[16];	   \n" +
				"bool b[16];   \n" +
				"sampler s[16];\n";

			// Add fields from our generated UPDB into HLSL
			if (shaderPdb.Shaders != null)
			{
				var shader = shaderPdb.Shaders.Shader;
				foreach (var intrp in shader.InterpolatorInfo.Interpolator)
				{
					var index = GetTempIndex(intrp.Register);
					Inputs += $"{INDENT}float4 r{index} : {(Semantic)Convert.ToInt32(intrp.Semantic, 16)};\n";
					Main += $"{INDENT}r[{index}] = In.r{index};\n";
				}
				foreach (var Float in shader.LiteralFloats.Float)
				{
					var index = GetConstIndex(Float.Register);
					Main += $"{INDENT}c[{index}] = float4({Float.Value0}, {Float.Value1}, {Float.Value2}, {Float.Value3});\n";
				}
				foreach (var Bool in shader.LiteralBools.Bool)
				{
					var index = GetBoolIndex(Bool.Register);
					Main += $"{INDENT}b[{index}] = {Bool.Value};\n";
				}
				foreach (var Int in shader.LiteralInts.Int)
				{
					var index = GetIntIndex(Int.Register);
					Main += $"{INDENT}i[{index}] = int3({Int.Count}, {Int.Start}, {Int.Increment});\n";
				}
			}

			// Much more work is needed here.
			// TODO: handle all ControlFlowInstruction types.
			// Distinguish between ALU and Fetch instructions.
			foreach (var instr in instructions)
			{
				foreach (var cf_instr in instr.cf_instrs)
				{
					Main += $"{INDENT}// {cf_instr.opcode}\n";

					// Handle CF instructions which execute ALU/Fetch instructions
					if (cf_instr.Executes)
					{
						var executes = instructions.Skip((int)cf_instr.exec.address).Take((int)cf_instr.exec.count).ToArray();

						for (var i = 0; i < executes.Length; i++)
						{
							var execute = executes[i];

							// Use 'i' with bit operations on 'cf_instr.exec.serialize' to determine ALU/Fetch.
							// TODO: work on fetch-type output.
							if ((cf_instr.exec.serialize & (1 << (i * 2))) != 0)
								Main += Fetch.Get(execute);
							else
								ALU.Get(execute);
						}
					}

					if (cf_instr.EndsShader)
						goto EndOfShader;
				}
			}
			EndOfShader:

			string hlsl =
				"// Decompiled with Jabukufo's ucode Decompiler \n" +
				"\n" +
				" // Constants:								\n" +
				$"{Constants}								\n" +
				"\n" +
				" // Parameters:							\n" +
				$"{Parameters}								\n" +
				"\n" +
				" // Inputs:								\n" +
				"struct INPUT								\n" +
				"{											\n" +
				$"{Inputs}									\n" +
				"};											\n" +
				"\n" +
				" // Outputs:								\n" +
				"struct OUTPUT								\n" +
				"{											\n" +
				$"{Outputs}									\n" +
				"};											\n" +
				"\n" +
				" // Variables:								\n" +
				"bool   p0 = false; // predicate 'register'.\n" +
				"int    a0 = 0; // address 'register'.		\n" +
				"float4 aL = 0; // loop 'register'.			\n" +
				"float4 lc = 0; // loop count.				\n" +
				"float4 ps = 0; // previous scalar result,	\n" +
				"float4 pv = 0; // previous vector result.	\n" +
				"float4 r[32];  // temp 'registers'			\n" +
				"\n" +
				" // Functions:								\n" +
				$"{Functions}								\n" +
				"\n" +
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


		// Parameter registers from tags should be allocated using the below functions BEFORE
		// calling Decompile(). These ensure legal register indices are always used. It will
		// try to allocate registers at their original index from xbox first. If that index
		// falls out of the legal range for dx9, the first available/unused register will be
		// allocated. If a register already has an allocated index, this will not change.
		//	After allocation is done, returns the allocated index for the register.

		// allocates/gets a r# register.
		public static int GetTempIndex(int index)
		{
			if (!AllocTemps.ContainsKey(index))
				if (index < 32)
					AllocTemps[index] = index;
				else
					for (var i = 0; i < 32; i++)
					if (!AllocTemps.ContainsValue(i))
					{
						AllocTemps[index] = i;
						break;
					}
			return AllocTemps[index];
		}

		// allocates/gets a c# register.
		public static int GetConstIndex(int index)
		{
			if (!AllocConsts.ContainsKey(index))
				if (index < 224)
					AllocConsts[index] = index;
				else
					for (var i = 0; i < 224; i++)
						if (!AllocConsts.ContainsValue(i))
						{
							AllocConsts[index] = i;
							break;
						}
			return AllocConsts[index];
		}

		// allocates/gets a b# register.
		public static int GetBoolIndex(int index)
		{
			if (!AllocBools.ContainsKey(index))
				if (index < 16)
					AllocBools[index] = index;
				else
					for (var i = 0; i < 16; i++)
					if (!AllocBools.ContainsValue(i))
					{
						AllocBools[index] = i;
						break;
					}
			return AllocBools[index];
		}

		// allocates/gets a i# register.
		public static int GetIntIndex(int index)
		{
			if (!AllocInts.ContainsKey(index))
				if (index < 16)
					AllocInts[index] = index;
				else
					for (var i = 0; i < 16; i++)
					if (!AllocInts.ContainsValue(i))
					{
						AllocInts[index] = i;
						break;
					}
			return AllocInts[index];
		}
	}
}
