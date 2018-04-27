using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.ShaderDecompiler.UcodeDisassembler;
using static TagTool.ShaderDecompiler.Decompiler;

namespace TagTool.ShaderDecompiler.Translations
{
	// Class for providing translation methods from Vector/Scalar ALU instructions into HLSL.
	public static class ALU
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

			// ALU instruction predication opening.... this doesn't seem to be used, or I'm doing it wrong...
			if (instruction.alu_instr.Is_predicated)
			{
				Main += $"{INDENT}if (p0 == {instruction.alu_instr.Pred_condition}) {{\n";
				INDENT += "	";
			}

			if (instruction.alu_instr.Has_vector_op)
			{
				Main += $"{INDENT}// {instruction.alu_instr.vector_opc}\n";
				Vector(instruction);
			}
			if (instruction.alu_instr.Has_scalar_op)
			{
				Main += $"{INDENT}// {instruction.alu_instr.scalar_opc}\n";
				Scalar(instruction);
			}

			// ALU instruction predication closing.... this doesn't seem to be used, or I'm doing it wrong...
			if (instruction.alu_instr.Is_predicated)
			{
				INDENT.Remove(INDENT.Length - 1);
				Main += $"{INDENT}}}\n";
			}
		}

		// Translates the Vector portions of an ALU instruction into HLSL fragments.
		private static void Vector(Instruction instruction)
		{
			var dest = instruction.alu_instr.GetVectorDest_Operand();
			var src1 = instruction.alu_instr.GetSrc1_Operand();
			var src2 = instruction.alu_instr.GetSrc2_Operand();
			var src3 = instruction.alu_instr.GetSrc3_Operand();
			var function = "";
			var main = "";
			switch (instruction.alu_instr.vector_opc)
			{
				case VectorOpcode.add:
					main +=
						$"{dest}{src1} + {src2};";
					break;
				case VectorOpcode.cndeq:
					main +=
						$"{dest}({src1} == 0.0f) ? {src2} : {src3};";
					break;
				case VectorOpcode.cndge:
					main +=
						$"{dest}({src1} ?= 0.0f) ? {src2} : {src3};";
					break;
				case VectorOpcode.cndgt:
					main +=
						$"{dest}({src1} > 0.0f) ? {src2} : {src3};";
					break;
				case VectorOpcode.cube:
					main +=
						$"{instruction.alu_instr.GetVectorAsmString()}\n";
					break;
				case VectorOpcode.dp2add:
					main +=
						$"{dest}dot({src1}, {src2}) + {src3};";
					break;
				case VectorOpcode.dp3:
					main +=
						$"{dest}dot({src1}, {src2});";
					break;
				case VectorOpcode.dp4:
					main +=
						$"{dest}dot({src1}, {src2});";
					break;
				case VectorOpcode.dst:
					main +=
						$"{dest}dst({src1}, {src2});";
					break;
				case VectorOpcode.floor:
					main +=
						$"{dest}floor({src1});";
					break;
				case VectorOpcode.frc:
					main +=
						$"{dest}frac({src1});";
					break;

					// TODO: these kill instructions need to be moved to a per-component function.
				case VectorOpcode.kill_eq:
					main +=
					   $" if ( {src1} == {src2}) {{\n"+
						" 	clip(-1);		\n" +
						" }					\n" +
						$"{dest}0.0f;";
					break;
				case VectorOpcode.kill_ge:
					main +=
						$" if ( {src1} >= {src2}) {{\n" +
						" 	clip(-1);		\n" +
						" }					\n" +
						$"{dest}0.0f;";
					break;
				case VectorOpcode.kill_gt:
					main +=
						$" if ( {src1} > {src2}) {{	\n" +
						" 	clip(-1);		\n" +
						" }					\n" +
						$"{dest}0.0f;";
					break;
				case VectorOpcode.kill_ne:
					main +=
						$" if ( {src1} != {src2}) {{\n" +
						" 	clip(-1);		\n" +
						" }					\n" +
						$"{dest}0.0f;";
					break;
				case VectorOpcode.mad:
					main +=
						$"{dest}({src1} * {src2}) + {src3};";
					break;
				case VectorOpcode.max:
					main +=
						$"{dest}max({src1}, {src2});";
					break;
				case VectorOpcode.max4:
					function =
						"float4 max4_func(float4 src1) {\n" +
						"	return max(src1.x, (max(src1.y, max(src1.z, src1.w)));\n" +
						"}";
					main +=
						$"{dest}max4_func({src1});";
					break;
				case VectorOpcode.maxa:
					function =
						"float4 maxa_func(float4 src1, float4 src2) {\n" +
						"	a0 = clamp(int(floor(src1.w + 0.5)), -256, 255);\n" +
						"	return max(src1, src2);\n" +
						"}";
					main +=
						$"{dest}maxa_func({src1}, {src2});";
					break;
				case VectorOpcode.min:
					main +=
						$"{dest}min({src1}, {src2});";
					break;
				case VectorOpcode.mul:
					main +=
						$"{dest}{src1} * {src2};";
					break;
				case VectorOpcode.seq:
					main +=
						$"{dest}({src1} == {src1}) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.setp_eq_push:
					function =
						"float4 setp_eq_push_func(float4 src1, float4 src2) {\n" +
						"	p0 = src1.w == 0.0 && src2.w == 0.0 ? true : false;\n" +
						"	return float4(src1.x == 0.0 && src2.x == 0.0 ? 0.0 : src1.x + 1.0);\n" +
						"}";
					main +=
						$"{dest}setp_eq_push_func({src1}, {src2});";
					break;
				case VectorOpcode.setp_ge_push:
					function =
						"float4 setp_ge_push_func(float4 src1, float4 src2) {\n" +
						"	p0 = src1.w == 0.0 && src2.w >= 0.0 ? true : false;\n" +
						"	return float4(src1.x == 0.0 && src2.x >= 0.0 ? 0.0 : src1.x + 1.0);\n" +
						"}";
					main +=
						$"{dest}setp_ge_push_func({src1}, {src2});";
					break;
				case VectorOpcode.setp_gt_push:
					function =
						"float4 setp_gt_push_func(float4 src1, float4 src2) {\n" +
						"	p0 = src1.w == 0.0 && src2.w > 0.0 ? true : false;\n" +
						"	return float4(src1.x == 0.0 && src2.x > 0.0 ? 0.0 : src1.x + 1.0);\n" +
						"}";
					main +=
						$"{dest}setp_gt_push_func({src1}, {src2});";
					break;
				case VectorOpcode.setp_ne_push:
					function =
						"float4 setp_ne_push_func(float4 src1, float4 src2) {\n" +
						"	p0 = src1.w == 0.0 && src2.w != 0.0 ? true : false;\n" +
						"	return float4(src1.x == 0.0 && src2.x != 0.0 ? 0.0 : src1.x + 1.0);\n" +
						"}";
					main +=
						$"{dest}setp_ne_push_func({src1}, {src2});";
					break;
				case VectorOpcode.sge:
					main +=
						$"{dest}({src1} >= {src1}) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.sgt:
					main +=
						$"{dest}({src1} > {src1}) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.sne:
					main +=
						$"{dest}({src1} != {src1}) ? 1.0f : 0.0f;";
					break;
				case VectorOpcode.trunc:
					main += 
						$"{dest}trunc({src1});";
					break;
				default:
					main += $"{instruction.alu_instr.GetVectorAsmString()}\n";
					break;
			}

			if (!Functions.Contains(function))
				Functions += $"{function}\n";

			// indent all our lines and add to "main"
			using (var sr = new StringReader(main))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
					Main += $"{INDENT}{line}\n";
			}
		}

		// Translates the Scalar portions of an ALU instruction into HLSL fragments
		private static void Scalar(Instruction instruction)
		{
			var dest = instruction.alu_instr.GetScalarDest_Operand();
			var src1 = instruction.alu_instr.GetSrc1_Operand();
			var src2 = instruction.alu_instr.GetSrc2_Operand();
			var src3 = instruction.alu_instr.GetSrc3_Operand();
			var function = "";
			var main = "";

			switch (instruction.alu_instr.scalar_opc)
			{
				case ScalarOpcode.adds:
					function =
						"float4 adds_func(float4 src1) {\n" +
						"	return ps = src1.x + src1.y;\n" +
						"}";
					main +=
						$"{dest}adds_func({src1});";
					break;
				case ScalarOpcode.addsc0:
				case ScalarOpcode.addsc1:
					function =
						"float4 addsc_func(float4 src1, float4 src2) {\n" +
						"	return ps = src1.x + src2.x;\n" +
						"}";
					main +=
						$"{dest}addsc_func({src1}, {src2});";
					break;
				case ScalarOpcode.adds_prev:
					function =
						"float4 adds_prev_func(float4 src1) {\n" +
						"	return ps = src1.x + ps;\n" +
						"}";
					main +=
						$"{dest}adds_prev_func({src1});";
					break;
				case ScalarOpcode.cos:
					function =
						"float4 cos_func(float4 src1) {\n" +
						"	return ps = cos(src1.x);\n" +
						"}";
					main +=
						$"{dest}cos_func({src1});";
					break;
				case ScalarOpcode.exp:
					function =
						"float4 exp_func(float4 src1) {\n" +
						"	return ps = pow(2, src1.x);\n" +
						"}";
					main +=
						$"{dest}exp_func({src1});";
					break;
				case ScalarOpcode.floors:
					function =
						"float4 floors_func(float4 src1) {\n" +
						"	return ps = floor(src1.x);\n" +
						"}";
					main +=
						$"{dest}floors_func({src1});";
					break;
				case ScalarOpcode.frcs:
					function =
						"float4 frcs_func(float4 src1) {\n" +
						"	return ps = src1.x - floor(src1.x);\n" +
						"}";
					main +=
						$"{dest}frcs_func({src1});";
					break;
				case ScalarOpcode.killseq:
					function =
						"float4 killseq_func(float4 src1) {\n" +
						"	if (src1.x == 0) {" +
						"		clip(-1);" +
						"	}\n" +
						$"	return ps = 0.0f;" +
						"}";
					main +=
						$"{dest}killseq_func({src1});";
					break;
				case ScalarOpcode.killsge:
					function =
						"float4 killsge_func(float4 src1) {\n" +
						"	if (src1.x >= 0) {" +
						"		clip(-1);" +
						"	}\n" +
						$"	return ps = 0.0f;" +
						"}";
					main +=
						$"{dest}killsge_func({src1});";
					break;
				case ScalarOpcode.killsgt:
					function =
						"float4 killsgt_func(float4 src1) {\n" +
						"	if (src1.x > 0) {" +
						"		clip(-1);" +
						"	}\n" +
						$"	return ps = 0.0f;" +
						"}";
					main +=
						$"{dest}killsgt_func({src1});";
					break;
				case ScalarOpcode.killsne:
					function =
						"float4 killsne_func(float4 src1) {\n" +
						"	if (src1.x != 0) {" +
						"		clip(-1);" +
						"	}\n" +
						$"	return ps = 0.0f;" +
						"}";
					main +=
						$"{dest}killsne_func({src1});";
					break;
				case ScalarOpcode.killsone:
					function =
						"float4 killsone_func(float4 src1) {\n" +
						"	if (src1.x == 1) {" +
						"		clip(-1);" +
						"	}\n" +
						$"	return ps = 0.0f;" +
						"}";
					main +=
						$"{dest}killsone_func({src1});";
					break;
				case ScalarOpcode.log:
					function =
						"float4 log_func(float4 src1) {\n" +
						$"	return ps = log(src1.x) / log(2);\n" +
						"}";
					main +=
						$"{dest}log_func({src1});";
					break;
				case ScalarOpcode.logc:
					function =
						"float4 logc_func(float4 src1) {\n" +
						"	ps = log(src1.x) / log(2);  \n" +
						"	ps = ps == -INFINITY ? -MAX_FLOAT : ps;" +
						"}";
					main +=
						$"{dest}logc_func({src1});";
					break;
				case ScalarOpcode.maxas:
					function =
						"float4 maxas_func(float4 src1) {\n" +
						"	a0 = src1.x;" +
						"	ps = max(src1.x, src1.y);\n" +
						"}";
					main +=
						$"{dest}maxas_func({src1});";
					break;
				case ScalarOpcode.maxasf:
					main +=
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
					function =
						"float4 maxs_func(float4 src1) {\n" +
						"	return ps = max(src1.x, src1.y);\n" +
						"}";
					main +=
						$"{dest}maxs_func({src1});";
					break;
				case ScalarOpcode.mins:
					main +=
						"ps = dest = min(src0.x, src0.y);";
					break;
				case ScalarOpcode.muls:
					main +=
						"ps = dest = src0.x * src0.y;";
					break;
				case ScalarOpcode.mulsc0:
				case ScalarOpcode.mulsc1:
					function =
						"float4 mulsc_func(float4 src1, float4 src2) {\n" +
						"	return ps = src1.x * src2.y;\n" +
						"}";
					main +=
						$"{dest}mulsc_func({src1}, {src2});";
					break;
				case ScalarOpcode.muls_prev:
					function =
						"float4 muls_prev_func(float4 src1) {\n" +
						"	return ps = src1.x * ps;\n" +
						"}";
					main +=
						$"{dest}muls_prev_func({src1});";
					break;
				case ScalarOpcode.muls_prev2:
					main +=
						"ps = dest = ps == -MAX_FLOAT || isinf(ps) || isnan(ps) || isnan(src0.y) || " +
						"src0.y <= 0.0 ? -MAX_FLOAT : src0.x * ps;";
					break;
				case ScalarOpcode.rcp:
					function =
						"float4 rcp_func(float4 src1) {\n" +
						"	return ps = 1.0f / src1.x;\n" +
						"}";
					main +=
						$"{dest}rcp_func({src1});";
					break;
				case ScalarOpcode.rcpc:
					main +=
						$"{dest}1.0f / src0.x;				\n" +
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
					main +=
						$"{dest}1.0f / src0.x;				\n" +
						"if (dest == -INFINITY)				\n" +
						"{									\n" +
						"	dest = -ZERO;					\n" +
						"}									\n" +
						"else if (dest == INFINITY)			\n" +
						"{									\n" +
						"	dest = ZERO;					\n" +
						"}									\n" +
						"ps = dest;";
					break;
				case ScalarOpcode.retain_prev:
					main += 
						"ps = dest = ps;";
					break;
				case ScalarOpcode.rsq:
					main +=
						"ps = dest = 1.0f / sqrt ( src0.x );";
					break;
				case ScalarOpcode.rsqc:
					main +=
						$"{dest}1.0f / sqrt ( src0.x );			\n" +
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
					main +=
						$"{dest}1.0f / sqrt ( src0.x );			\n" +
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
					main +=
						"ps = dest = ( src0.x == 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.setpclr:
					main +=
						"ps = dest = +MAX_FLOAT; \n" +
						"p0 = false; ";
					break;
				case ScalarOpcode.setpeq:
					main +=
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
					main +=
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
					main +=
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
					main +=
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
					main +=
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
					main +=
						$"{dest}src0.x - 1.0f;	\n" +
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
					main +=
						$"{dest}src0.x;			\n" +
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
					main +=
						"ps = dest = ( src0.x >= 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.sgts:
					main +=
						"ps = dest = ( src0.x > 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.sin:
					main += 
						"ps = dest = sin(src0.x);";
					break;
				case ScalarOpcode.snes:
					main +=
						"ps = dest = ( src0.x != 0.0f ) ? 1.0f : 0.0f;";
					break;
				case ScalarOpcode.sqrt:
					function =
						"float4 sqrt_func(float4 src1) {\n" +
						"	return ps = sqrt(src1.x);\n" +
						"}";
					main +=
						$"{dest}sqrt_func({src1});";
					break;
				case ScalarOpcode.subs:
					function =
						"float4 subs_func(float4 src1) {\n" +
						"	return ps = src1.x - src1.y;\n" +
						"}";
					main +=
						$"{dest}subs_func({src1});";
					break;
				case ScalarOpcode.subsc0:
				case ScalarOpcode.subsc1:
					main +=
						"ps = dest = src0.x - {src1}.y;";
					break;
				case ScalarOpcode.subs_prev:
					main +=
						"ps = dest = src0.x - ps;";
					break;
				case ScalarOpcode.truncs:
					main +=
						"ps = dest = trunc( src0.x );";
					break;
				default:
					main += $"{instruction.alu_instr.GetScalarAsmString()}\n";
					break;
			}

			if (!Functions.Contains(function))
				Functions += $"{function}\n";

			// indent all our lines and add to "main"
			using (var sr = new StringReader(main))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
					Main += $"{INDENT}{line}\n";
			}
		}
	}
}
