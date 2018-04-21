using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderDisassembler
{
	public class InstructionDecoding
	{

		public static ControlFlowInstruction[] DecodeCF(uint dword0, uint dword1, uint dword2)
		{
			var cfi0 = new ControlFlowInstruction();
			var cfi1 = new ControlFlowInstruction();

			decodeCF(dword0, dword1 & 0xFFFF, ref cfi0);
			decodeCF((dword1 >> 16) | (dword2 << 16), dword2 >> 16, ref cfi1);

			return new ControlFlowInstruction[] { cfi0, cfi1 };
		}

		public static ALUInstruction DecodeALU(uint dword0, uint dword1, uint dword2)
		{
			var alu = new ALUInstruction();
			decodeALU(dword0, dword1, dword2, ref alu);
			return alu;
		}

		public static FetchInstruction DecodeFetch(uint dword0, uint dword1, uint dword2)
		{
			var fetch = new FetchInstruction();
			decodeFetch(dword0, dword1, dword2, ref fetch);
			return fetch;
		}

	
		[DllImport("Interop.dll")]
		public static extern void decodeCF(uint dword0, uint dword1, ref ControlFlowInstruction cfi);

		[DllImport("Interop.dll")]
		private static extern void decodeALU(uint dword0, uint dword1, uint dword2, ref ALUInstruction alui);

		[DllImport("Interop.dll")]
		private static extern void decodeFetch(uint dword0, uint dword1, uint dword2, ref FetchInstruction fi);
	}
}
