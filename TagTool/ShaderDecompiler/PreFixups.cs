using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.ShaderDecompiler.UcodeDisassembler;

namespace TagTool.ShaderDecompiler
{
	// Class for managing fixups that may need to be done before generating HLSL (such as register fixups)
	// Dany's research on Registers/rmt2 https://gist.github.com/dany5639/15b8692fa81cd65d05d07196bd24b225
	public class PreFixups
	{
		public static void Apply(ref ControlFlowInstruction instruction)
		{

		}

		public static void Apply(ref ALUInstruction instruction)
		{

		}

		public static void Apply(ref FetchInstruction instruction)
		{

		}
	}
}
