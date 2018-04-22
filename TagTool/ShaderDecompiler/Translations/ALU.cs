using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.ShaderDecompiler.UcodeDisassembler;

namespace TagTool.ShaderDecompiler.Translations
{
	// Class for providing translation methods from Vector/Scalar ALU instructions into HLSL.
	public class ALU
	{
		// Conventions:
		// - All temporary registers are float4s.
		// - Scalar ops swizzle out a single component of their source registers denoted
		//   by 'a' or 'b'. src0.a means 'the first component specified for src0' and
		//   src0.ab means 'two components specified for src0, in order'.
		// - Scalar ops write the result to the entire destination register.
		// - pv and ps are the previous results of a vector or scalar ALU operation.
		//   Both are valid only within the current ALU clause. They are not modified
		//   when write masks are disabled or the instruction that would write them
		//   fails its predication check.

		// Translates an ALU Vector/Scalar pair into HLSL fragments (Vector and Scalar ALU instructions
		// are ALWAYS executed in pairs, and are part of the same instruction definition.
		public string Get(Instruction instruction)
		{
			string translation = "";

			translation += $"{Vector(instruction)}";
			translation += $"{Scalar(instruction)}";

			return translation;
		}

		// Translates the Vector portions of an ALU instruction into HLSL fragments.
		private string Vector(Instruction instruction)
		{
			string translation = "";
			if (!instruction.alu_instr.Has_vector_op)
				return translation;

			string asmInstruction = "// {instruction.aluInstr.GetVectorAsmString()}\n";

			switch (instruction.alu_instr.vector_opc)
			{
				case VectorOpcode.add:
					translation += 
						"pv = src0 + src1;";
					break;
				case VectorOpcode.cndeq:
					translation +=
						" pv.x = (src0.x == 0.0f) ? src1.x : src2.x;\n" +
						" pv.y = (src0.y == 0.0f) ? src1.y : src2.y;\n" +
						" pv.z = (src0.z == 0.0f) ? src1.z : src2.z;\n" +
						" pv.w = (src0.w == 0.0f) ? src1.w : src2.w;";
					break;
				case VectorOpcode.cndge:
					translation +=
						" pv.x = (src0.x >= 0.0f) ? src1.x : src2.x;\n" +
						" pv.y = (src0.y >= 0.0f) ? src1.y : src2.y;\n" +
						" pv.z = (src0.z >= 0.0f) ? src1.z : src2.z;\n" +
						" pv.w = (src0.w >= 0.0f) ? src1.w : src2.w;";
					break;
				case VectorOpcode.cndgt:
					translation +=
						" pv.x = (src0.x > 0.0f) ? src1.x : src2.x;\n" +
						" pv.y = (src0.y > 0.0f) ? src1.y : src2.y;\n" +
						" pv.z = (src0.z > 0.0f) ? src1.z : src2.z;\n" +
						" pv.w = (src0.w > 0.0f) ? src1.w : src2.w;";
					break;
				case VectorOpcode.cube:
					translation += $"// {instruction.alu_instr.GetVectorAsmString()}\n";
					break;
				case VectorOpcode.dp2add:
					translation += 
						"pv = src0.x * src1.x + src0.y * src1.y + src2.x;";
					break;
				case VectorOpcode.dp3:
					translation += 
						"pv = src0.x * src1.x + src0.y * src1.y + src0.z * src1.z;";
					break;
				case VectorOpcode.dp4:
					translation += 
						"pv = src0.x * src1.x + src0.y * src1.y + src0.z * src1.z + src0.w * src1.w;";
					break;
				case VectorOpcode.dst:
					translation +=
						"pv = dst(src0, src1);";
					break;
				case VectorOpcode.floor:
					translation += 
						"pv = floor(src0);";
					break;
				case VectorOpcode.frc:
					translation += 
						"pv = fract(src0);";
					break;
				case VectorOpcode.kill_eq:
					translation +=
						" pv = 0.0f;" +
						" if ( src1.x == src2.x || src1.y == src2.y || src1.z == src2.z || src1.w == src2.w )\n" +
						" {					\n" +
						" 	pv = 1.0f;		\n" +
						" }					\n" +
						" clip(-pv);		  ";
					break;
				case VectorOpcode.kill_ge:
					translation +=
						" pv = 0.0f;" +
						" if ( src1.x >= src2.x || src1.y >= src2.y || src1.z >= src2.z || src1.w >= src2.w )\n" +
						" {					\n" +
						" 	pv = 1.0f;		\n" +
						" }					\n" +
						" clip(-pv);		  ";
					break;
				case VectorOpcode.kill_gt:
					translation +=
						" pv = 0.0f;" +
						" if ( src1.x > src2.x || src1.y > src2.y || src1.z > src2.z || src1.w > src2.w )\n" +
						" {					\n" +
						" 	pv = 1.0f;		\n" +
						" }					\n" +
						" clip(-pv);		  ";
					break;
				case VectorOpcode.kill_ne:
					translation +=
						" pv = 0.0f;" +
						" if ( src1.x != src2.x || src1.y != src2.y || src1.z != src2.z || src1.w != src2.w )\n" +
						" {					\n" +
						" 	pv = 1.0f;		\n" +
						" }					\n" +
						" clip(-pv);		  ";
					break;
				case VectorOpcode.mad:
					translation += 
						"pv = (src0 * src1) + src2;";
					break;
				case VectorOpcode.max:
					translation += 
						"pv = max(src0, src1);";
					break;
				case VectorOpcode.max4:
					translation += 
						"pv = max(src0.x, max(src0.y, max(src0.z, src0.w)));";
					break;
				case VectorOpcode.maxa:
					translation +=
						"a0 = clamp(int(floor(src0.w + 0.5)), -256, 255);" +
						"pv = max(src0, src1);";
					break;
				case VectorOpcode.min:
					translation += 
						"pv = min(src0, src1);";
					break;
				case VectorOpcode.mul:
					translation += 
						"pv = src0 * src1;";
					break;
				case VectorOpcode.seq:
					translation +=
						"pv.x = (src0.x == src1.x) ? 1.0f : 0.0f;\n" +
						"pv.y = (src0.y == src1.y) ? 1.0f : 0.0f;\n" +
						"pv.z = (src0.z == src1.z) ? 1.0f : 0.0f;\n" +
						"pv.w = (src0.w == src1.w) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.setp_eq_push:
					translation +=
						"p0 = src0.w == 0.0 && src1.w == 0.0 ? true : false;\n" +
						"pv = float4(src0.x == 0.0 && src1.x == 0.0 ? 0.0 : src0.x + 1.0);";
					break;
				case VectorOpcode.setp_ge_push:
					translation +=
						"p0 = src0.w == 0.0 && src1.w >= 0.0 ? true : false;\n" +
						"pv = float4(src0.x == 0.0 && src1.x >= 0.0 ? 0.0 : src0.x + 1.0);";
					break;
				case VectorOpcode.setp_gt_push:
					translation +=
						"p0 = src0.w == 0.0 && src1.w > 0.0 ? true : false;\n" +
						"pv = float4(src0.x == 0.0 && src1.x > 0.0 ? 0.0 : src0.x + 1.0);";
					break;
				case VectorOpcode.setp_ne_push:
					translation +=
						"p0 = src0.w == 0.0 && src1.w != 0.0 ? true : false;\n" +
						"pv = float4(src0.x == 0.0 && src1.x != 0.0 ? 0.0 : src0.x + 1.0);";
					break;
				case VectorOpcode.sge:
					translation +=
						"pv.x = (src0.x >= src1.x) ? 1.0f : 0.0f;\n" +
						"pv.y = (src0.y >= src1.y) ? 1.0f : 0.0f;\n" +
						"pv.z = (src0.z >= src1.z) ? 1.0f : 0.0f;\n" +
						"pv.w = (src0.w >= src1.w) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.sgt:
					translation +=
						"pv.x = (src0.x > src1.x) ? 1.0f : 0.0f;\n" +
						"pv.y = (src0.y > src1.y) ? 1.0f : 0.0f;\n" +
						"pv.z = (src0.z > src1.z) ? 1.0f : 0.0f;\n" +
						"pv.w = (src0.w > src1.w) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.sne:
					translation +=
						"pv.x = (src0.x != src1.x) ? 1.0f : 0.0f;\n" +
						"pv.y = (src0.y != src1.y) ? 1.0f : 0.0f;\n" +
						"pv.z = (src0.z != src1.z) ? 1.0f : 0.0f;\n" +
						"pv.w = (src0.w != src1.w) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.trunc:
					translation += 
						"pv = trunc(src0);";
					break;
				case VectorOpcode.opcode_30:
				case VectorOpcode.opcode_31:
					translation += $"// {instruction.alu_instr.GetVectorAsmString()}\n";
					break;
				default:
					translation += $"// *DEFAULTED* {instruction.alu_instr.GetVectorAsmString()}\n";
					break;
			}

			translation.Replace("pv.", instruction.alu_instr.GetDest_Register());
			translation.Replace("pv", instruction.alu_instr.GetDest_Operand());

			translation.Replace("src0.", instruction.alu_instr.GetSrc0_Register());
			translation.Replace("src0", instruction.alu_instr.GetSrc0_Operand());

			translation.Replace("src1.", instruction.alu_instr.GetSrc1_Register());
			translation.Replace("src1", instruction.alu_instr.GetSrc1_Operand());

			translation.Replace("src2.", instruction.alu_instr.GetSrc2_Register());
			translation.Replace("src2", instruction.alu_instr.GetSrc2_Operand());

			return asmInstruction + translation + "\n";
		}

		// Translates the Scalar portions of an ALU instruction into HLSL fragments
		private string Scalar(Instruction instruction)
		{
			string translation = "";
			if (!instruction.alu_instr.Has_scalar_op)
				return translation;

			switch (instruction.alu_instr.scalar_opc)
			{
				case ScalarOpcode.adds:
					translation +=
						"ps = dest = src0 + src0;";
					break;
				case ScalarOpcode.addsc0:
				case ScalarOpcode.addsc1:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.adds_prev:
					translation +=
						"ps = dest = src0.x + ps;";
					break;
				case ScalarOpcode.cos:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.exp:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.floors:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.frcs:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.killseq:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.killsge:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.killsgt:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.killsne:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.killsone:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.log:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.logc:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.maxas:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.maxasf:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.maxs:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.mins:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.muls:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.mulsc0:
				case ScalarOpcode.mulsc1:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.muls_prev:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.muls_prev2:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.rcp:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.rcpc:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.rcpf:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.retain_prev:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.rsq:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.rsqc:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.rsqf:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.seqs:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.setpclr:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.setpeq:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.setpge:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.setpgt:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.setpinv:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.setpne:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.setppop:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.setprstr:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.sges:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.sgts:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.sin:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.snes:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.sqrt:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.subs:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.subsc0:
				case ScalarOpcode.subsc1:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.subs_prev:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.truncs:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_41:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_51:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_52:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_53:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_54:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_55:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_56:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_57:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_58:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_59:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_60:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_61:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_62:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				case ScalarOpcode.opcode_63:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				default:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
			}

			return translation += "\n";
		}
	}
}
