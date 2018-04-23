using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderDecompiler.UcodeDisassembler
{
	// A container for different instruction types, so we can put them into a list.
	// (inheritence not good practice because we are using structs for C++/C# interop)
	public class Instruction
	{
		public ControlFlowInstruction[] cf_instrs;
		public ALUInstruction alu_instr;
		public FetchInstruction fetch_instr;

		public Instruction(ControlFlowInstruction[] cf_instrs, ALUInstruction alu_instr, FetchInstruction fetch_instr)
		{
			this.cf_instrs = cf_instrs;
			this.alu_instr = alu_instr;
			this.fetch_instr = fetch_instr;
		}

		public Instruction() { }
	}
}
