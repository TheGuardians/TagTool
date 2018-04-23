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
		public static string Get(Instruction instruction, string indent)
		{
			PreFixups.Apply(ref instruction.alu_instr);

			string translation = "";

			translation += $"{Vector(instruction, indent)}";
			translation += $"{Scalar(instruction, indent)}";

			return translation;
		}

		// Translates the Vector portions of an ALU instruction into HLSL fragments.
		private static string Vector(Instruction instruction, string indent)
		{
			string translation = "";
			if (!instruction.alu_instr.Has_vector_op)
				return translation;

			switch (instruction.alu_instr.vector_opc)
			{
				case VectorOpcode.add:
					translation +=
						"dest = src0 + src1;";
					break;
				case VectorOpcode.cndeq:
					translation +=
						" dest.x = (src0.x == 0.0f) ? src1.x : src2.x;\n" +
						" dest.y = (src0.y == 0.0f) ? src1.y : src2.y;\n" +
						" dest.z = (src0.z == 0.0f) ? src1.z : src2.z;\n" +
						" dest.w = (src0.w == 0.0f) ? src1.w : src2.w;";
					break;
				case VectorOpcode.cndge:
					translation +=
						" dest.x = (src0.x >= 0.0f) ? src1.x : src2.x;\n" +
						" dest.y = (src0.y >= 0.0f) ? src1.y : src2.y;\n" +
						" dest.z = (src0.z >= 0.0f) ? src1.z : src2.z;\n" +
						" dest.w = (src0.w >= 0.0f) ? src1.w : src2.w;";
					break;
				case VectorOpcode.cndgt:
					translation +=
						" dest.x = (src0.x > 0.0f) ? src1.x : src2.x;\n" +
						" dest.y = (src0.y > 0.0f) ? src1.y : src2.y;\n" +
						" dest.z = (src0.z > 0.0f) ? src1.z : src2.z;\n" +
						" dest.w = (src0.w > 0.0f) ? src1.w : src2.w;";
					break;
				case VectorOpcode.cube:
					translation += 
						$"// {instruction.alu_instr.GetVectorAsmString()}\n";
					break;
				case VectorOpcode.dp2add:
					translation +=
						"dest = src0.x * src1.x + src0.y * src1.y + src2.x;";
					break;
				case VectorOpcode.dp3:
					translation +=
						"dest = src0.x * src1.x + src0.y * src1.y + src0.z * src1.z;";
					break;
				case VectorOpcode.dp4:
					translation +=
						"dest = src0.x * src1.x + src0.y * src1.y + src0.z * src1.z + src0.w * src1.w;";
					break;
				case VectorOpcode.dst:
					translation +=
						"dest = dst(src0, src1);";
					break;
				case VectorOpcode.floor:
					translation +=
						"dest = floor(src0);";
					break;
				case VectorOpcode.frc:
					translation +=
						"dest = fract(src0);";
					break;
				case VectorOpcode.kill_eq:
					translation +=
						" dest = 0.0f;" +
						" if ( src0.x == src1.x || src0.y == src1.y || src0.z == src1.z || src0.w == src1.w )\n" +
						" {					\n" +
						" 	dest = 1.0f;		\n" +
						" }					\n" +
						" clip(-dest);		  ";
					break;
				case VectorOpcode.kill_ge:
					translation +=
						" dest = 0.0f;" +
						" if ( src0.x >= src1.x || src0.y >= src1.y || src0.z >= src1.z || src0.w >= src1.w )\n" +
						" {					\n" +
						" 	dest = 1.0f;		\n" +
						" }					\n" +
						" clip(-dest);		  ";
					break;
				case VectorOpcode.kill_gt:
					translation +=
						" dest = 0.0f;" +
						" if ( src0.x > src1.x || src0.y > src1.y || src0.z > src1.z || src0.w > src1.w )\n" +
						" {					\n" +
						" 	dest = 1.0f;		\n" +
						" }					\n" +
						" clip(-dest);		  ";
					break;
				case VectorOpcode.kill_ne:
					translation +=
						" dest = 0.0f;" +
						" if ( src0.x != src1.x || src0.y != src1.y || src0.z != src1.z || src0.w != src1.w )\n" +
						" {					\n" +
						" 	dest = 1.0f;		\n" +
						" }					\n" +
						" clip(-dest);		  ";
					break;
				case VectorOpcode.mad:
					translation +=
						"dest = (src0 * src1) + src2;";
					break;
				case VectorOpcode.max:
					translation +=
						"dest = max(src0, src1);";
					break;
				case VectorOpcode.max4:
					translation +=
						"dest = max(src0.x, max(src0.y, max(src0.z, src0.w)));";
					break;
				case VectorOpcode.maxa:
					translation +=
						"a0 = clamp(int(floor(src0.w + 0.5)), -256, 255);" +
						"dest = max(src0, src1);";
					break;
				case VectorOpcode.min:
					translation +=
						"dest = min(src0, src1);";
					break;
				case VectorOpcode.mul:
					translation +=
						"dest = src0 * src1;";
					break;
				case VectorOpcode.seq:
					translation +=
						"dest.x = (src0.x == src1.x) ? 1.0f : 0.0f;\n" +
						"dest.y = (src0.y == src1.y) ? 1.0f : 0.0f;\n" +
						"dest.z = (src0.z == src1.z) ? 1.0f : 0.0f;\n" +
						"dest.w = (src0.w == src1.w) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.setp_eq_push:
					translation +=
						"p0 = src0.w == 0.0 && src1.w == 0.0 ? true : false;\n" +
						"dest = float4(src0.x == 0.0 && src1.x == 0.0 ? 0.0 : src0.x + 1.0);";
					break;
				case VectorOpcode.setp_ge_push:
					translation +=
						"p0 = src0.w == 0.0 && src1.w >= 0.0 ? true : false;\n" +
						"dest = float4(src0.x == 0.0 && src1.x >= 0.0 ? 0.0 : src0.x + 1.0);";
					break;
				case VectorOpcode.setp_gt_push:
					translation +=
						"p0 = src0.w == 0.0 && src1.w > 0.0 ? true : false;\n" +
						"dest = float4(src0.x == 0.0 && src1.x > 0.0 ? 0.0 : src0.x + 1.0);";
					break;
				case VectorOpcode.setp_ne_push:
					translation +=
						"p0 = src0.w == 0.0 && src1.w != 0.0 ? true : false;\n" +
						"dest = float4(src0.x == 0.0 && src1.x != 0.0 ? 0.0 : src0.x + 1.0);";
					break;
				case VectorOpcode.sge:
					translation +=
						"dest.x = (src0.x >= src1.x) ? 1.0f : 0.0f;\n" +
						"dest.y = (src0.y >= src1.y) ? 1.0f : 0.0f;\n" +
						"dest.z = (src0.z >= src1.z) ? 1.0f : 0.0f;\n" +
						"dest.w = (src0.w >= src1.w) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.sgt:
					translation +=
						"dest.x = (src0.x > src1.x) ? 1.0f : 0.0f;\n" +
						"dest.y = (src0.y > src1.y) ? 1.0f : 0.0f;\n" +
						"dest.z = (src0.z > src1.z) ? 1.0f : 0.0f;\n" +
						"dest.w = (src0.w > src1.w) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.sne:
					translation +=
						"dest.x = (src0.x != src1.x) ? 1.0f : 0.0f;\n" +
						"dest.y = (src0.y != src1.y) ? 1.0f : 0.0f;\n" +
						"dest.z = (src0.z != src1.z) ? 1.0f : 0.0f;\n" +
						"dest.w = (src0.w != src1.w) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.trunc:
					translation += 
						"dest = trunc(src0);";
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

			return $"{indent}{translation}\n";
		}

		// Translates the Scalar portions of an ALU instruction into HLSL fragments
		private static string Scalar(Instruction instruction, string indent)
		{
			string translation = "";
			if (!instruction.alu_instr.Has_scalar_op)
				return translation;

			switch (instruction.alu_instr.scalar_opc)
			{
				case ScalarOpcode.adds:
					translation +=
						"ps = dest = src0.x + src0.y;";
					break;
				case ScalarOpcode.addsc0:
				case ScalarOpcode.addsc1:
					translation +=
						"ps = dest = src0.x + src0.y;";
					break;
				case ScalarOpcode.adds_prev:
					translation +=
						"ps = dest = src0.x + ps;";
					break;
				case ScalarOpcode.cos:
					translation +=
						"ps = dest = cos( src0.x );";
					break;
				case ScalarOpcode.exp:
					translation +=
						"ps = dest = pow( 2, src0.x );";
					break;
				case ScalarOpcode.floors:
					translation +=
						"ps = dest = floor( src0.x );";
					break;
				case ScalarOpcode.frcs:
					translation +=
						"ps = dest = src0.x − floor( src0.x );";
					break;
				case ScalarOpcode.killseq:
					translation +=
						" ps = dest = 0.0f;" +
						" if ( src0.x == 0 )\n" +
						" {					\n" +
						" 	ps = dest= 1.0f;\n" +
						" }					\n" +
						" clip(-pv);		  ";
					break;
				case ScalarOpcode.killsge:
					translation +=
						" ps = dest = 0.0f;" +
						" if ( src0.x >= 0 )\n" +
						" {					\n" +
						" 	ps = dest = 1.0f;\n" +
						" }					\n" +
						" clip(-pv);		  "; break;
				case ScalarOpcode.killsgt:
					translation +=
						" ps = dest = 0.0f;" +
						" if ( src0.x > 0 )\n" +
						" {					\n" +
						" 	ps = dest = 1.0f;\n" +
						" }					\n" +
						" clip(-pv);		  "; break;
				case ScalarOpcode.killsne:
					translation +=
						" ps = dest = 0.0f;" +
						" if ( src0.x != 0 )\n" +
						" {					\n" +
						" 	ps = dest = 1.0f;\n" +
						" }					\n" +
						" clip(-pv);		  "; break;
				case ScalarOpcode.killsone:
					translation +=
						" ps = dest = 0.0f;" +
						" if ( src0.x == 1 )\n" +
						" {					\n" +
						"	ps = dest = 1.0f;\n" +
						" }					\n" +
						" clip(-pv);		  "; break;
				case ScalarOpcode.log:
					translation +=
						"ps = dest = log( src0.x ) / log( 2 );";
					break;
				case ScalarOpcode.logc:
					translation +=
						"dest = log( src0.x ) / log( 2 );	\n"+
						"if (dest == -INFINITY)				\n" +
						"{									\n" +
						"	dest = -MAX_FLOAT;				\n" +
						"}									  " +
						"ps = dest;";
					break;
				case ScalarOpcode.maxas:
					translation +=
						"a0 = src0.x; \n" +
						"ps = dest = ( src0.x >= src0.y ) ? src0.x : src0.y;";
					break;
				case ScalarOpcode.maxasf:
					translation +=
						"int temp = floor( src0.x);\n" +
						"if (temp < −256 )				\n" +
						"{								\n" +
						"	temp = −256;				\n" +
						"}								\n" +
						"if (temp > 255)				\n" +
						"{								\n" +
						"	temp = 255;					\n" +
						"}								\n" +
						"a0 = temp;						\n" +
						"ps = dest = ( src0.x >= src0.y ) ? src0.x : src0.y;";
					break;
				case ScalarOpcode.maxs:
					translation +=
						"ps = dest = max(src0.x, src0.y);";
					break;
				case ScalarOpcode.mins:
					translation +=
						"ps = dest = min(src0.x, src0.y);";
					break;
				case ScalarOpcode.muls:
					translation +=
						"ps = dest = src0.x * src0.y;";
					break;
				case ScalarOpcode.mulsc0:
				case ScalarOpcode.mulsc1:
					translation +=
						"ps = dest = src0.x * src1.y;";
					break;
				case ScalarOpcode.muls_prev:
					translation +=
						"ps = dest = src0.x * ps;";
					break;
				case ScalarOpcode.muls_prev2:
					translation +=
						"ps = dest = ps == -MAX_FLOAT || isinf(ps) || isnan(ps) || isnan(src0.y) || " +
						"src0.y <= 0.0 ? -MAX_FLOAT : src0.x * ps;";
					break;
				case ScalarOpcode.rcp:
					translation +=
						"ps = dest = 1.0f / src0.x;";
					break;
				case ScalarOpcode.rcpc:
					translation +=
						"dest = 1.0f / src0.x;				\n" +
						"if (dest == -INFINITY)				\n" +
						"{									\n" +
						"	dest = -MAX_FLOAT;				\n" +
						"}									\n" +
						"else if (dest == INFINITY)			\n" +
						"{									\n" +
						"	dest = MAX_FLOAT;				\n" +
						"}									\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.rcpf:
					translation +=
						"dest = 1.0f / src0.x;				\n" +
						"if (dest == -INFINITY)				\n" +
						"{									\n" +
						"	dest = -ZERO;					\n" +
						"}									\n" +
						"else if (dest == INFINITY)			\n" +
						"{									\n" +
						"	dest = ZERO;					\n" +
						"}									\n" +
						"ps = dest;"; break;
				case ScalarOpcode.retain_prev:
					translation += 
						"ps = dest = ps;";
					break;
				case ScalarOpcode.rsq:
					translation +=
						"ps = dest = 1.0f / sqrt ( src0.x );";
					break;
				case ScalarOpcode.rsqc:
					translation +=
						"dest = 1.0f / sqrt ( src0.x );			\n" +
						"if (dest == -INFINITY)			\n" +
						"{										\n" +
						"	dest = -MAX_FLOAT;			\n" +
						"}										\n" +
						"else if (dest == INFINITY)		\n" +
						"{										\n" +
						"	dest = MAX_FLOAT;			\n" +
						"}										\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.rsqf:
					translation +=
						"dest = 1.0f / sqrt ( src0.x );			\n" +
						"if (dest == -INFINITY)					\n" +
						"{										\n" +
						"	dest = -ZERO;						\n" +
						"}										\n" +
						"else if (dest == +INFINITY)			\n" +
						"{										\n" +
						"	dest = +ZERO;						\n" +
						"}										\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.seqs:
					translation +=
						"ps = dest = ( src0.x == 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.setpclr:
					translation +=
						"ps = dest = +MAX_FLOAT; \n" +
						"p0 = false; ";
					break;
				case ScalarOpcode.setpeq:
					translation +=
						"if (src0.x == 0.0f)	\n" +
						"{						\n" +
						"	dest = 0.0f;		\n" +
						"	p0 = true;			\n" +
						"}						\n" +
						"else					\n" +
						"{						\n" +
						"	dest = 1.0f;		\n" +
						"	p0 = false;			\n" +
						"}						\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.setpge:
					translation +=
						"if (src0.x >= 0.0f)	\n" +
						"{						\n" +
						"	dest = 0.0f;		\n" +
						"	p0 = true;			\n" +
						"}						\n" +
						"else					\n" +
						"{						\n" +
						"	dest = 1.0f;		\n" +
						"	p0 = false;			\n" +
						"}						\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.setpgt:
					translation +=
						"if (src0.x > 0.0f)	\n" +
						"{						\n" +
						"	dest = 0.0f;		\n" +
						"	p0 = true;			\n" +
						"}						\n" +
						"else					\n" +
						"{						\n" +
						"	dest = 1.0f;		\n" +
						"	p0 = false;			\n" +
						"}						\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.setpinv:
					translation +=
						"if (src0.x == 1.0f)	\n" +
						"{						\n" +
						"	dest = 0.0f;		\n" +
						"	p0 = true;			\n" +
						"}						\n" +
						"else					\n" +
						"{						\n" +
						"	if (src0.x == 0.0f)	\n" +
						"	{					\n" +
						"		dest = 1.0f;	\n" +
						"	}					\n" +
						"	else				\n" +
						"	{					\n" +
						"		dest = src0.x;	\n" +
						"	}					\n" +
						"	p0 = false;			\n" +
						"}						\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.setpne:
					translation +=
						"if (src0.x != 0.0f)	\n" +
						"{						\n" +
						"	dest = 0.0f;		\n" +
						"	p0 = true;			\n" +
						"}						\n" +
						"else					\n" +
						"{						\n" +
						"	dest = 1.0f;		\n" +
						"	p0 = false;			\n" +
						"}						\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.setppop:
					translation +=
						"dest = src0.x - 1.0f;	\n" +
						"if (dest <= 0.0f)		\n" +
						"{						\n" +
						"	dest = 0.0f;		\n" +
						"	p0 = true;			\n" +
						"}						\n" +
						"else					\n" +
						"{						\n" +
						"	p0 = false;			\n" +
						"}						\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.setprstr:
					translation +=
						"dest = src0.x;			\n" +
						"if (dest == 0.0f)		\n" +
						"{						\n" +
						"	p0 = true;			\n" +
						"}						\n" +
						"else					\n" +
						"{						\n" +
						"	p0 = false;			\n" +
						"}						\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.sges:
					translation +=
						"ps = dest = ( src0.x >= 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.sgts:
					translation +=
						"ps = dest = ( src0.x > 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.sin:
					translation += 
						"ps = dest = sin(src0.x);";
					break;
				case ScalarOpcode.snes:
					translation +=
						"ps = dest = ( src0.x != 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.sqrt:
					translation +=
						"ps = dest = sqrt(src0.x);";
					break;
				case ScalarOpcode.subs:
					translation +=
						"ps = dest = src0.x - src0.y;";
					break;
				case ScalarOpcode.subsc0:
				case ScalarOpcode.subsc1:
					translation +=
						"ps = dest = src0.x - src1.y;";
					break;
				case ScalarOpcode.subs_prev:
					translation +=
						"ps = dest = src0.x - ps;";
					break;
				case ScalarOpcode.truncs:
					translation +=
						"ps = dest = trunc( src0.x );";
					break;
				case ScalarOpcode.opcode_41:
				case ScalarOpcode.opcode_51:
				case ScalarOpcode.opcode_52:
				case ScalarOpcode.opcode_53:
				case ScalarOpcode.opcode_54:
				case ScalarOpcode.opcode_55:
				case ScalarOpcode.opcode_56:
				case ScalarOpcode.opcode_57:
				case ScalarOpcode.opcode_58:
				case ScalarOpcode.opcode_59:
				case ScalarOpcode.opcode_60:
				case ScalarOpcode.opcode_61:
				case ScalarOpcode.opcode_62:
				case ScalarOpcode.opcode_63:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				default:
					translation += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
			}

			return $"{indent}{translation}\n";
		}
	}
}
