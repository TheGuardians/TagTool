using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.ShaderDecompiler.UcodeDisassembler;
using static TagTool.ShaderDecompiler.Decompiler;

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
		public static void Get(Instruction instruction)
		{
			PreFixups.Apply(ref instruction.alu_instr);

			if (instruction.alu_instr.Has_vector_op)
				Vector(instruction);
			if (instruction.alu_instr.Has_vector_op)
				Scalar(instruction);
		}

		// Translates the Vector portions of an ALU instruction into HLSL fragments.
		private static void Vector(Instruction instruction)
		{
			var dest = instruction.alu_instr.GetVectorDest_Operand();
			var src1 = instruction.alu_instr.GetSrc1_Operand();
			var src2 = instruction.alu_instr.GetSrc2_Operand();
			var src3 = instruction.alu_instr.GetSrc3_Operand();
			var function = "";

			switch (instruction.alu_instr.vector_opc)
			{
				case VectorOpcode.add:
					Main +=
						$"{dest} = {src1} + {src2};";
					break;
				case VectorOpcode.cndeq:
					Main +=
						$"{dest} = ({src1} == 0.0f) ? {src2} : {src3};";
					break;
				case VectorOpcode.cndge:
					Main +=
						$"{dest} = ({src1} ?= 0.0f) ? {src2} : {src3};";
					break;
				case VectorOpcode.cndgt:
					Main +=
						$"{dest} = ({src1} > 0.0f) ? {src2} : {src3};";
					break;
				case VectorOpcode.cube:
					Main +=
						$"// {instruction.alu_instr.GetVectorAsmString()}\n";
					break;
				case VectorOpcode.dp2add:
					Main +=
						$"{dest} = dot({src1}, {src2}) + {src3};";
					break;
				case VectorOpcode.dp3:
					Main +=
						$"{dest} = dot({src1}, {src2});";
					break;
				case VectorOpcode.dp4:
					Main +=
						$"{dest} = dot({src1}, {src2});";
					break;
				case VectorOpcode.dst:
					Main +=
						$"{dest} = dst({src1}, {src2});";
					break;
				case VectorOpcode.floor:
					Main +=
						$"{dest} = floor({src1});";
					break;
				case VectorOpcode.frc:
					Main +=
						$"{dest} = frac({src1});";
					break;

					// TODO: these kill instructions need to be moved to a per-component function.
				case VectorOpcode.kill_eq:
					Main +=
					   $" if ( {src1} == {src2}) {{\n"+
						" 	clip(-1);		\n" +
						" }					\n" +
						$" {dest} = 0.0f;";
					break;
				case VectorOpcode.kill_ge:
					Main +=
						$" if ( {src1} >= {src2}) {{\n" +
						" 	clip(-1);		\n" +
						" }					\n" +
						$" {dest} = 0.0f;";
					break;
				case VectorOpcode.kill_gt:
					Main +=
						$" if ( {src1} > {src2}) {{	\n" +
						" 	clip(-1);		\n" +
						" }					\n" +
						$" {dest} = 0.0f;";
					break;
				case VectorOpcode.kill_ne:
					Main +=
						$" if ( {src1} != {src2}) {{\n" +
						" 	clip(-1);		\n" +
						" }					\n" +
						$" {dest} = 0.0f;";
					break;
				case VectorOpcode.mad:
					Main +=
						$"{dest} = ({src1} * {src2}) + {src3};";
					break;
				case VectorOpcode.max:
					Main +=
						$"{dest} = max({src1}, {src2});";
					break;
				case VectorOpcode.max4:
					function =
						"float4 max4_func(float4 src1) {\n" +
						"	return max(src1.x, (max(src1.y, max(src1.z, src1.w)));\n" +
						"}";
					Main +=
						$"{dest} = max4_func({src1});";
					break;
				case VectorOpcode.maxa:
					function =
						"float4 maxa_func(float4 src1, float4 src2) {\n" +
						"	a0 = clamp(int(floor(src1.w + 0.5)), -256, 255);\n" +
						"	return max(src1, src2);\n" +
						"}";
					Main +=
						$"{dest} = maxa_func({src1}, {src2});";
					break;
				case VectorOpcode.min:
					Main +=
						$"{dest} = min({src1}, {src2});";
					break;
				case VectorOpcode.mul:
					Main +=
						$"{dest} = {src1} * {src2};";
					break;
				case VectorOpcode.seq:
					Main +=
						$"{dest} = ({src1} == {src1}) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.setp_eq_push:
					function =
						"float4 setp_eq_push_func(float4 src1, float4 src2) {\n" +
						"	p0 = src1.w == 0.0 && src2.w == 0.0 ? true : false;\n" +
						"	return float4(src1.x == 0.0 && src2.x == 0.0 ? 0.0 : src1.x + 1.0);\n" +
						"}";
					Main +=
						$"{dest} = setp_eq_push_func({src1}, {src2});";
					break;
				case VectorOpcode.setp_ge_push:
					function =
						"float4 setp_ge_push_func(float4 src1, float4 src2) {\n" +
						"	p0 = src1.w == 0.0 && src2.w >= 0.0 ? true : false;\n" +
						"	return float4(src1.x == 0.0 && src2.x >= 0.0 ? 0.0 : src1.x + 1.0);\n" +
						"}";
					Main +=
						$"{dest} = setp_ge_push_func({src1}, {src2});";
					break;
				case VectorOpcode.setp_gt_push:
					function =
						"float4 setp_gt_push_func(float4 src1, float4 src2) {\n" +
						"	p0 = src1.w == 0.0 && src2.w > 0.0 ? true : false;\n" +
						"	return float4(src1.x == 0.0 && src2.x > 0.0 ? 0.0 : src1.x + 1.0);\n" +
						"}";
					Main +=
						$"{dest} = setp_gt_push_func({src1}, {src2});";
					break;
				case VectorOpcode.setp_ne_push:
					function =
						"float4 setp_ne_push_func(float4 src1, float4 src2) {\n" +
						"	p0 = src1.w == 0.0 && src2.w != 0.0 ? true : false;\n" +
						"	return float4(src1.x == 0.0 && src2.x != 0.0 ? 0.0 : src1.x + 1.0);\n" +
						"}";
					Main +=
						$"{dest} = setp_ne_push_func({src1}, {src2});";
					break;
				case VectorOpcode.sge:
					Main +=
						$"{dest} = ({src1} >= {src1}) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.sgt:
					Main +=
						$"{dest} = ({src1} > {src1}) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.sne:
					Main +=
						$"{dest} = ({src1} != {src1}) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.trunc:
					Main += 
						$"{dest} = trunc({src1});";
					break;
				case VectorOpcode.opcode_30:
				case VectorOpcode.opcode_31:
					Main += $"// {instruction.alu_instr.GetVectorAsmString()}\n";
					break;
				default:
					Main += $"// *DEFAULTED* {instruction.alu_instr.GetVectorAsmString()}\n";
					break;
			}

			if (!Functions.Contains(function))
				Functions += $"{function}\n";
		}

		// Translates the Scalar portions of an ALU instruction into HLSL fragments
		private static void Scalar(Instruction instruction)
		{
			var dest = instruction.alu_instr.GetScalarDest_Operand();
			var src1 = instruction.alu_instr.GetSrc1_Operand();
			var src2 = instruction.alu_instr.GetSrc2_Operand();
			var src3 = instruction.alu_instr.GetSrc3_Operand();
			var function = "";

			switch (instruction.alu_instr.scalar_opc)
			{
				case ScalarOpcode.adds:
					Functions +=
						"float4 adds_func(float4 src1) {\n" +
						"	return ps = src1.x + src1.y;\n" +
						"}";
					Main +=
						$"{dest} = adds_func({src1});";
					break;
				case ScalarOpcode.addsc0:
				case ScalarOpcode.addsc1:
					Functions +=
						"float4 addsc_func(float4 src1, float4 src2) {\n" +
						"	return ps = src1.x + src2.x;\n" +
						"}";
					Main +=
						$"{dest} = addsc_func({src1}, {src2});";
					break;
				case ScalarOpcode.adds_prev:
					Functions +=
						"float4 adds_prev_func(float4 src1) {\n" +
						"	return ps = src1.x + ps;\n" +
						"}";
					Main +=
						$"{dest} = adds_prev_func({src1});";
					break;
				case ScalarOpcode.cos:
					Functions +=
						"float4 cos_func(float4 src1) {\n" +
						"	return ps = cos(src1.x);\n" +
						"}";
					Main +=
						$"{dest} = cos_func({src1});";
					break;
				case ScalarOpcode.exp:
					Functions +=
						"float4 exp_func(float4 src1) {\n" +
						"	return ps = pow(2, src1.x);\n" +
						"}";
					Main +=
						$"{dest} = exp_func({src1});";
					break;
				case ScalarOpcode.floors:
					Functions +=
						"float4 floors_func(float4 src1) {\n" +
						"	return ps = floor(src1.x);\n" +
						"}";
					Main +=
						$"{dest} = floors_func({src1});";
					break;
				case ScalarOpcode.frcs:
					Functions +=
						"float4 frcs_func(float4 src1) {\n" +
						"	return ps = src1.x - floor(src1.x);\n" +
						"}";
					Main +=
						$"{dest} = frcs_func({src1});";
					break;
				case ScalarOpcode.killseq:
					Main +=
						" ps = dest = 0.0f;" +
						" if ( src0.x == 0 )\n" +
						" {					\n" +
						" 	ps = dest= 1.0f;\n" +
						" }					\n" +
						" clip(-pv);		  ";
					break;
				case ScalarOpcode.killsge:
					Main +=
						" ps = dest = 0.0f;" +
						" if ( src0.x >= 0 )\n" +
						" {					\n" +
						" 	ps = dest = 1.0f;\n" +
						" }					\n" +
						" clip(-pv);		  "; break;
				case ScalarOpcode.killsgt:
					Main +=
						" ps = dest = 0.0f;" +
						" if ( src0.x > 0 )\n" +
						" {					\n" +
						" 	ps = dest = 1.0f;\n" +
						" }					\n" +
						" clip(-pv);		  "; break;
				case ScalarOpcode.killsne:
					Main +=
						" ps = dest = 0.0f;" +
						" if ( src0.x != 0 )\n" +
						" {					\n" +
						" 	ps = dest = 1.0f;\n" +
						" }					\n" +
						" clip(-pv);		  "; break;
				case ScalarOpcode.killsone:
					Main +=
						" ps = dest = 0.0f;" +
						" if ( src0.x == 1 )\n" +
						" {					\n" +
						"	ps = dest = 1.0f;\n" +
						" }					\n" +
						" clip(-pv);		  "; break;
				case ScalarOpcode.log:
					Main +=
						"ps = dest = log( src0.x ) / log( 2 );";
					break;
				case ScalarOpcode.logc:
					Main +=
						$"{dest} = log( src0.x ) / log( 2 );	\n"+
						"if (dest == -INFINITY)				\n" +
						"{									\n" +
						"	dest = -MAX_FLOAT;				\n" +
						"}									  " +
						"ps = dest;";
					break;
				case ScalarOpcode.maxas:
					Main +=
						"a0 = src0.x; \n" +
						"ps = dest = ( src0.x >= src0.y ) ? src0.x : src0.y;";
					break;
				case ScalarOpcode.maxasf:
					Main +=
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
					Main +=
						"ps = dest = max(src0.x, src0.y);";
					break;
				case ScalarOpcode.mins:
					Main +=
						"ps = dest = min(src0.x, src0.y);";
					break;
				case ScalarOpcode.muls:
					Main +=
						"ps = dest = src0.x * src0.y;";
					break;
				case ScalarOpcode.mulsc0:
				case ScalarOpcode.mulsc1:
					Main +=
						"ps = dest = src0.x * {src1}.y;";
					break;
				case ScalarOpcode.muls_prev:
					Main +=
						"ps = dest = src0.x * ps;";
					break;
				case ScalarOpcode.muls_prev2:
					Main +=
						"ps = dest = ps == -MAX_FLOAT || isinf(ps) || isnan(ps) || isnan(src0.y) || " +
						"src0.y <= 0.0 ? -MAX_FLOAT : src0.x * ps;";
					break;
				case ScalarOpcode.rcp:
					Main +=
						"ps = dest = 1.0f / src0.x;";
					break;
				case ScalarOpcode.rcpc:
					Main +=
						$"{dest} = 1.0f / src0.x;				\n" +
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
					Main +=
						$"{dest} = 1.0f / src0.x;				\n" +
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
					Main += 
						"ps = dest = ps;";
					break;
				case ScalarOpcode.rsq:
					Main +=
						"ps = dest = 1.0f / sqrt ( src0.x );";
					break;
				case ScalarOpcode.rsqc:
					Main +=
						$"{dest} = 1.0f / sqrt ( src0.x );			\n" +
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
					Main +=
						$"{dest} = 1.0f / sqrt ( src0.x );			\n" +
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
					Main +=
						"ps = dest = ( src0.x == 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.setpclr:
					Main +=
						"ps = dest = +MAX_FLOAT; \n" +
						"p0 = false; ";
					break;
				case ScalarOpcode.setpeq:
					Main +=
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
					Main +=
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
					Main +=
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
					Main +=
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
					Main +=
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
					Main +=
						$"{dest} = src0.x - 1.0f;	\n" +
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
					Main +=
						$"{dest} = src0.x;			\n" +
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
					Main +=
						"ps = dest = ( src0.x >= 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.sgts:
					Main +=
						"ps = dest = ( src0.x > 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.sin:
					Main += 
						"ps = dest = sin(src0.x);";
					break;
				case ScalarOpcode.snes:
					Main +=
						"ps = dest = ( src0.x != 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.sqrt:
					Main +=
						"ps = dest = sqrt(src0.x);";
					break;
				case ScalarOpcode.subs:
					Main +=
						"ps = dest = src0.x - src0.y;";
					break;
				case ScalarOpcode.subsc0:
				case ScalarOpcode.subsc1:
					Main +=
						"ps = dest = src0.x - {src1}.y;";
					break;
				case ScalarOpcode.subs_prev:
					Main +=
						"ps = dest = src0.x - ps;";
					break;
				case ScalarOpcode.truncs:
					Main +=
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
					Main += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
				default:
					Main += $"// {instruction.alu_instr.GetScalarAsmString()}\n";
					break;
			}

			if (!Functions.Contains(function))
				Functions += $"{function}\n";
		}
	}
}
