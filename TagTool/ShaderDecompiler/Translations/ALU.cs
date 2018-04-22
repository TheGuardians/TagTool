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
				default:
					translation += $"// {instruction.aluInstr.vector_opc.ToString()}";
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
				default:
					translation += $"// {instruction.aluInstr.scalar_opc.ToString()}";
					break;
			}

			return translation += "\n";
		}
	}
}
