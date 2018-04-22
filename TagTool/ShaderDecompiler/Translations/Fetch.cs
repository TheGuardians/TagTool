using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.ShaderDecompiler.UcodeDisassembler;

namespace TagTool.ShaderDecompiler.Translations
{
	// Class for providing translations from Fetch instructions into HLSL.
	public class Fetch
	{
		// Translates a VFetch or TFetch type instruction into an HLSL fragment.
		public string Get(Instruction instruction)
		{
			string translation = "";

			if (instruction.fetchInstr.opcode == FetchOpCode.vfetch)
				translation += Vfetch(instruction);
			else
				translation += Tfetch(instruction);

			return translation;
		}

		// Translates a VFetch type instruction into an HLSL fragment.
		private string Vfetch(Instruction instruction)
		{
			string translation = "";

			switch (instruction.fetchInstr.opcode)
			{
				default:
					translation += $"// {instruction.fetchInstr.opcode.ToString()}";
					break;
			}

			return translation += "\n";
		}

		// Translates a TFetch type instruction into an HLSL fragment.
		private string Tfetch(Instruction instruction)
		{
			string translation = "";

			switch (instruction.fetchInstr.opcode)
			{
				default:
					translation += $"// {instruction.fetchInstr.opcode.ToString()}";
					break;
			}

			return translation += "\n";
		}
	}
}
