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

			// All control-flow instructions are at the beginning of the shader, and are 6 bytes long.
			// We're decoding them in pairs, so the increment is 12.
			for (var i = 0; i < shader_data.Length; i += 12)
			{
				// read three uint32, and flip endianness. (3 * 4 = 12)
				var dword0 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(shader_data, i+0)).Reverse().ToArray(), 0);
				var dword1 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(shader_data, i+4)).Reverse().ToArray(), 0);
				var dword2 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(shader_data, i+8)).Reverse().ToArray(), 0);

				// decode two control-flow instructions
				var cfs = InstructionDecoding.DecodeCF(dword0, dword1, dword2);

				// foreach of those two control-flow instructions...
				foreach (var cf in cfs)
				{
					// add the control-flow instruction to the instructions list.
					instructions.Add(new Instruction(cf));

					// if the control flow instruction executes ALU or Fetch type instructions...
					if (cf.Executes)
					{
						// each ALU and Fetch instruction is 12-bytes, so we need a byte array that is 12 * amount of instructions.
						var instrs_data = new byte[cf.exec.count * 12];
						// copy the instructions to instrs_data.
						// sourceIndex = address * 12 bytes; length = count * 12; (12 bytes per instruction)
						Array.Copy(shader_data, cf.exec.address * 12, instrs_data, 0, cf.exec.count * 12);

						// Foreach 12-byte instruction in instrs_data...
						for (var j = 0; j < instrs_data.Length; j += 12)
						{
							// read three uint32, and flip endianness. (3 * 4 = 12)
							dword0 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(instrs_data, j + 0)).Reverse().ToArray(), 0);
							dword1 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(instrs_data, j + 4)).Reverse().ToArray(), 0);
							dword2 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(instrs_data, j + 8)).Reverse().ToArray(), 0);

							// If the instruction at the current index is marked as ALU...
							if (!cf.ExecuteIndexIsFetch(j / 12))
							{
								// decode as an ALU instruction and add to the instructions list.
								var alu = InstructionDecoding.DecodeALU(dword0, dword1, dword2);
								instructions.Add(new Instruction(alu));
							}
							// If the instruction at the current index is marked as Fetch...
							else if (cf.ExecuteIndexIsFetch(j / 12))
							{
								// decode as a Fetch instruction and add to the instructions list.
								var fetch = InstructionDecoding.DecodeFetch(dword0, dword1, dword2);
								instructions.Add(new Instruction(fetch));
							}
						}
					}

					// If the control-flow instruction is a type that marks the end of shader, then return
					// our instruction list.
					if (cf.EndsShader)
						return instructions;
				}
			}
			// if somehow the above return didn't happen, go ahead and return the instructions here
			// (all shaders should have an instruction that ends execution)
			return instructions;
		}
	}
}
