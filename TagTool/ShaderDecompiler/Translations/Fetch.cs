using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.ShaderDecompiler.UcodeDisassembler;

namespace TagTool.ShaderDecompiler.Translations
{
	// Class for providing translations from Fetch instructions into HLSL.
	public static class Fetch
	{
		// Translates a VFetch or TFetch type instruction into an HLSL fragment.
		public static string Get(Instruction instruction)
		{
			PreFixups.Apply(ref instruction.fetch_instr);

			string translation = "";

			if (instruction.fetch_instr.opcode == FetchOpCode.vfetch)
				translation += Vfetch(instruction);
			else
				translation += Tfetch(instruction);

			return translation;
		}

		// Translates a VFetch type instruction into an HLSL fragment.
		private static string Vfetch(Instruction instruction)
		{
			string translation = "";

			translation += $"// {instruction.fetch_instr.opcode.ToString()}";

			return translation += "\n";
		}

		// Translates a TFetch type instruction into an HLSL fragment.
		private static string Tfetch(Instruction instruction)
		{
			string translation = "";

			switch (instruction.fetch_instr.opcode)
			{
				case FetchOpCode.getBCF:
					translation += $"// {instruction.fetch_instr.opcode.ToString()}";
					break;
				case FetchOpCode.getCompTexLOD:
					translation += $"// {instruction.fetch_instr.opcode.ToString()}";
					break;
				case FetchOpCode.getGradients:
					translation += $"// {instruction.fetch_instr.opcode.ToString()}";
					break;
				case FetchOpCode.getWeights:
					translation += $"// {instruction.fetch_instr.opcode.ToString()}";
					break;
				case FetchOpCode.setGradientH:
					translation += $"// {instruction.fetch_instr.opcode.ToString()}";
					break;
				case FetchOpCode.setGradientV:
					translation += $"// {instruction.fetch_instr.opcode.ToString()}";
					break;
				case FetchOpCode.setTexLOD:
					translation += $"// {instruction.fetch_instr.opcode.ToString()}";
					break;
				case FetchOpCode.tfetch:
					translation += $"// {instruction.fetch_instr.opcode.ToString()}";
					break;
				case FetchOpCode.UnknownTextureOp:
					translation += $"// {instruction.fetch_instr.opcode.ToString()}";
					break;
				default:
					translation += $"// {instruction.fetch_instr.opcode.ToString()}";
					break;
			}

			return translation += "\n";
		}
	}
}
