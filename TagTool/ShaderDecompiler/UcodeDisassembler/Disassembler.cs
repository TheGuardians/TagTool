using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderDecompiler.UcodeDisassembler
{
	public class Disassembler
	{
		// Disassembles a raw shader into an orderly List of Instructions.
		public static List<Instruction> Disassemble(byte[] shader_data)
		{
			var instructions = new List<Instruction> { };

			// All instructions are 12 bytes long. Except for CF instructions which are 6 bytes long, and that we're decoding
			// in pairs. Increment our loop by 12.
			for (var i = 0; i < shader_data.Length; i += 12)
			{
				// read three uint32, and flip endianness. (3 * 4 = 12)
				var dword0 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(shader_data, i+0)).Reverse().ToArray(), 0);
				var dword1 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(shader_data, i+4)).Reverse().ToArray(), 0);
				var dword2 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(shader_data, i+8)).Reverse().ToArray(), 0);

				// decode two control-flow instructions
				var cfs = InstructionDecoding.DecodeCF(dword0, dword1, dword2);
				var alu = InstructionDecoding.DecodeALU(dword0, dword1, dword2);
				var fetch = InstructionDecoding.DecodeFetch(dword0, dword1, dword2);
			}

			// return our List of Instruction
			return instructions;
		}
	}
}
