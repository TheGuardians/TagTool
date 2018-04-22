using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderDisassembler
{
	public class Disassembler
	{
		public static List<Instruction> Disassemble(byte[] shader_data)
		{
			var instructions = new List<Instruction> { };

			for (var i = 0; i < shader_data.Length; i += 12)
			{
				var dword0 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(shader_data, i+0)).Reverse().ToArray(), 0);
				var dword1 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(shader_data, i+4)).Reverse().ToArray(), 0);
				var dword2 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(shader_data, i+8)).Reverse().ToArray(), 0);

				var cfs = InstructionDecoding.DecodeCF(dword0, dword1, dword2);

				foreach (var cf in cfs)
				{
					instructions.Add(new Instruction(cf));

					if (cf.Executes)
					{
						var instrs_data = new byte[cf.exec.count * 12];
						Array.Copy(shader_data, cf.exec.address * 12, instrs_data, 0, cf.exec.count * 12);

						for (var j = 0; j < instrs_data.Length; j += 12)
						{
							dword0 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(instrs_data, j + 0)).Reverse().ToArray(), 0);
							dword1 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(instrs_data, j + 4)).Reverse().ToArray(), 0);
							dword2 = BitConverter.ToUInt32(BitConverter.GetBytes(BitConverter.ToUInt32(instrs_data, j + 8)).Reverse().ToArray(), 0);

							if (!cf.ExecuteIndexIsFetch(j / 12))
							{
								var alu = InstructionDecoding.DecodeALU(dword0, dword1, dword2);
								instructions.Add(new Instruction(alu));
							}
							else if (cf.ExecuteIndexIsFetch(j / 12))
							{
								var fetch = InstructionDecoding.DecodeFetch(dword0, dword1, dword2);
								instructions.Add(new Instruction(fetch));
							}
						}
					}

					if (cf.EndsShader)
						return instructions;
				}
			}
			return instructions;
		}
	}
}
