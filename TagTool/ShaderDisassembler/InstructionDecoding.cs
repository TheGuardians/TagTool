using System.Runtime.InteropServices;

/* WARNING: DO NOT TOUCH THIS FILE UNLESS YOU KNOW WHAT YOU'RE DOING, AND ALSO MAKE THE APPROPRIATE
CHANGES TO THE MATCHING FILE IN THE `TagTool` PROJECT */

namespace TagTool.ShaderDisassembler
{
	public class InstructionDecoding
	{
		// Decodes two control-flow instructions from three uints.
		// (the uints must have endianness swapped so when ToString("X8") is used, they appear in
		// same order as raw hex)
		public static ControlFlowInstruction[] DecodeCF(uint dword0, uint dword1, uint dword2)
		{
			var cfi0 = new ControlFlowInstruction();
			var cfi1 = new ControlFlowInstruction();

			// necessary shifting/masking copied from xenia
			decodeCF(dword0, dword1 & 0xFFFF, ref cfi0);
			decodeCF((dword1 >> 16) | (dword2 << 16), dword2 >> 16, ref cfi1);

			return new ControlFlowInstruction[] { cfi0, cfi1 };
		}

		// Decodes an ALU instruction from three uints.
		// (the uints must have endianness swapped so when ToString("X8") is used, they appear in
		// same order as raw hex)
		public static ALUInstruction DecodeALU(uint dword0, uint dword1, uint dword2)
		{
			var alu = new ALUInstruction();
			decodeALU(dword0, dword1, dword2, ref alu);
			return alu;
		}

		// Decodes a Fetch instruction from three uints.
		// (the uints must have endianness swapped so when ToString("X8") is used, they appear in
		// same order as raw hex)
		public static FetchInstruction DecodeFetch(uint dword0, uint dword1, uint dword2)
		{
			var fetch = new FetchInstruction();
			decodeFetch(dword0, dword1, dword2, ref fetch);
			return fetch;
		}

		// Function imports (these are written in C++ to take advantage of the language's built in bitfields,
		// and also allows some amount of copy-paste from xenia)
		[DllImport("TagToolUtilities.dll")]
		private static extern void decodeCF(uint dword0, uint dword1, ref ControlFlowInstruction cfi);

		[DllImport("TagToolUtilities.dll")]
		private static extern void decodeALU(uint dword0, uint dword1, uint dword2, ref ALUInstruction alui);

		[DllImport("TagToolUtilities.dll")]
		private static extern void decodeFetch(uint dword0, uint dword1, uint dword2, ref FetchInstruction fi);
	}
}
