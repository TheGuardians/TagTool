using System.Runtime.InteropServices;

/* WARNING: DO NOT TOUCH THIS FILE UNLESS YOU KNOW WHAT YOU'RE DOING, AND ALSO MAKE THE APPROPRIATE
 CHANGES TO THE MATCHING FILE IN THE `TagToolUtilities` PROJECT */

namespace TagTool.ShaderDecompiler.UcodeDisassembler
{
	// ALU instruction data
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ALUInstruction
	{
		public uint vector_dest;
		public uint vector_dest_rel;
		public uint abs_constants;
		public uint scalar_dest;
		public uint scalar_dest_rel;
		public uint export_data;
		public Mask vector_write_mask;
		public Mask scalar_write_mask;
		public uint vector_clamp;
		public uint scalar_clamp;
		public ScalarOpcode scalar_opc;  // instr_scalar_opc_t

		public Swizzle src3_swiz;
		public Swizzle src2_swiz;
		public Swizzle src1_swiz;
		public uint src3_reg_negate;
		public uint src2_reg_negate;
		public uint src1_reg_negate;
		public uint pred_condition;
		public uint is_predicated;
		public uint address_absolute;
		public uint const_1_rel_abs;
		public uint const_0_rel_abs;

		public uint src3_reg;
		public uint src2_reg;
		public uint src1_reg;
		public VectorOpcode vector_opc;  // instr_vector_opc_t
		public uint src3_sel;
		public uint src2_sel;
		public uint src1_sel;

		// Whether data is being exported (or written to local registers).
		public bool Is_export { get => export_data != 0; }

		// Whether the instruction is predicated (or conditional).
		public bool Is_predicated { get => is_predicated != 0; }
		// Required condition value of the comparision (true or false).
		public bool Pred_condition { get => pred_condition != 0; }

		public bool Abs_constants { get => abs_constants == 1; }
		public bool Is_const_0_addressed { get => const_0_rel_abs != 0; }
		public bool Is_const_1_addressed { get => const_1_rel_abs != 0; }
		public bool Is_address_relative { get => address_absolute != 0; }

		// Whether the instruction operates on the vector ALU
		public bool Has_vector_op { get => vector_write_mask != 0 || Is_export; }
		public bool Is_vector_dest_relative { get => vector_dest_rel != 0; }
		public bool Vector_clamp { get => vector_clamp != 0; }

		// Whether the instruction operates on the scalar ALU
		public bool Has_scalar_op { get => scalar_opc != ScalarOpcode.retain_prev || (!Is_export && scalar_write_mask != 0); }
		public bool Is_scalar_dest_relative { get => scalar_dest_rel != 0; }
		public bool Scalar_clamp { get => scalar_clamp != 0; }

		// Gets the string representation of the Vector portion of the ALU instruction.
		public string GetVectorAsmString()
		{
			string asmString = "";
			asmString += $"{vector_opc} {GetVectorDest_Operand()}, {GetSrc1_Operand()}, {GetSrc2_Operand()}, {GetSrc2_Operand()}";
			return asmString;
		}

		// Gets the string representation of the Scalar portion of the ALU instruction.
		public string GetScalarAsmString()
		{
			string asmString = "";
			asmString += $"{scalar_opc}";
			return asmString;
		}

		// gets the string representation of the full dest operand
		public string GetVectorDest_Operand()
		{
			var rtype = "r"; // TODO: find the proper way to check dest register type
			var index = vector_dest;
			var mask = vector_write_mask.ToString().Replace(",", "").Replace(" ", "");
			if (mask == "_")
				return "";

			return $"{rtype}[{index}].{mask} = ";
		}
		public string GetScalarDest_Operand()
		{
			var rtype = "r"; // TODO: find the proper way to check dest register type
			var index = scalar_dest;
			var mask = scalar_write_mask.ToString().Replace(",", "").Replace(" ", "");

			if (mask == "_")
				return "";

			return $"{rtype}[{index}].{mask} = ";
		}

		// gets the string representation of the full src0 operand
		public string GetSrc1_Operand()
		{
			var oprnd = "";
			if (src1_reg_negate != 0)
				oprnd += '-';
			if (src1_sel != 0)
				oprnd += 'r';
			else
			{
				src1_reg = (uint)Decompiler.GetConstIndex((int)src1_reg);
				oprnd += 'c';
			}
			oprnd += $"[{src1_reg}]";
			return $"{oprnd}.{src1_swiz}";
		}

		// gets the string representation of the full src1 operand
		public string GetSrc2_Operand()
		{
			var oprnd = "";
			if (src2_reg_negate != 0)
				oprnd += '-';
			if (src2_sel != 0)
				oprnd += 'r';
			else
			{
				src2_reg = (uint)Decompiler.GetConstIndex((int)src2_reg);
				oprnd += 'c';
			}
			oprnd += $"[{src2_reg}]";
			return $"{oprnd}.{src2_swiz}";
		}

		// gets the string representation of the full src2 operand
		public string GetSrc3_Operand()
		{
			var oprnd = "";
			if (src3_reg_negate != 0)
				oprnd += '-';
			if (src3_sel != 0)
				oprnd += 'r';
			else
			{
				src3_reg = (uint)Decompiler.GetConstIndex((int)src3_reg);
				oprnd += 'c';
			}
			oprnd += $"[{src3_reg}]";
			return $"{oprnd}.{src3_swiz}";
		}
	}
}
