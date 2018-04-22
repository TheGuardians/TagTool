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
			if (!instruction.aluInstr.Has_vector_op)
				return translation;

			switch (instruction.aluInstr.vector_opc)
			{
				case VectorOpcode.add:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.cndeq:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.cndge:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.cndgt:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.cube:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.dp2add:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.dp3:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.dp4:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.dst:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.floor:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.frc:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.kill_eq:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.kill_ge:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.kill_gt:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.kill_ne:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.mad:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.max:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.max4:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.maxa:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.min:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.mul:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.opcode_30:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.opcode_31:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.seq:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.setp_eq_push:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.setp_ge_push:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.setp_gt_push:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.setp_ne_push:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.sge:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.sgt:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.sne:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				case VectorOpcode.trunc:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
					break;
				default:
					translation += $"// *DEFAULTED* {instruction.aluInstr.vector_opc.ToString()}";
					break;
			}

			return translation += "\n";
		}

		// Translates the Scalar portions of an ALU instruction into HLSL fragments
		private string Scalar(Instruction instruction)
		{
			string translation = "";
			if (!instruction.aluInstr.Has_scalar_op)
				return translation;

			switch (instruction.aluInstr.scalar_opc)
			{
				case ScalarOpcode.adds:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.addsc0:
				case ScalarOpcode.addsc1:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.adds_prev:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.cos:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.exp:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.floors:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.frcs:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.killseq:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.killsge:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.killsgt:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.killsne:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.killsone:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.log:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.logc:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.maxas:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.maxasf:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.maxs:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.mins:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.muls:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.mulsc0:
				case ScalarOpcode.mulsc1:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.muls_prev:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.muls_prev2:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.rcp:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.rcpc:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.rcpf:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.retain_prev:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.rsq:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.rsqc:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.rsqf:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.seqs:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.setpclr:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.setpeq:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.setpge:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.setpgt:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.setpinv:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.setpne:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.setppop:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.setprstr:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.sges:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.sgts:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.sin:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.snes:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.sqrt:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.subs:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.subsc0:
				case ScalarOpcode.subsc1:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.subs_prev:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.truncs:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_41:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_51:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_52:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_53:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_54:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_55:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_56:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_57:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_58:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_59:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_60:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_61:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_62:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				case ScalarOpcode.opcode_63:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
				default:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
			}

			return translation += "\n";
		}
	}
}
