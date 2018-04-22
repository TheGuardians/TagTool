using System.Runtime.InteropServices;

/* WARNING: DO NOT TOUCH THIS FILE UNLESS YOU KNOW WHAT YOU'RE DOING, AND ALSO MAKE THE APPROPRIATE
 CHANGES TO THE MATCHING FILE IN THE `TagToolUtilities` PROJECT */

namespace TagTool.ShaderDecompiler.UcodeDisassembler
{
	public enum Mask : uint
	{
		A_Vector = 0x01, // Vector destination mask
		B_Vector = 0x02, // Vector destination mask
		C_Vector = 0x04, // Vector destination mask
		D_Vector = 0x08, // Vector destination mask
	}
	public enum ScalarOpcode : uint
	{
		adds = 0x00,
		adds_prev = 0x01,
		muls = 0x02,
		muls_prev = 0x03,
		muls_prev2 = 0x4,
		maxs = 0x5,
		mins = 0x6,
		seqs = 0x7,
		sgts = 0x8,
		sges = 0x9,
		snes = 0xA,
		frcs = 0xB,
		truncs = 0xC,
		floors = 0xD,
		exp = 0xE,
		logc = 0xF,
		log = 0x10,
		rcpc = 0x11,
		rcpf = 0x12,
		rcp = 0x13,
		rsqc = 0x14,
		rsqf = 0x15,
		rsq = 0x16,
		maxas = 0x17,
		maxasf = 0x18,
		subs = 0x19,
		subs_prev = 0x1A,
		setpeq = 0x1B,
		setpne = 0x1C,
		setpgt = 0x1D,
		setpge = 0x1E,
		setpinv = 0x1F,
		setppop = 0x20,
		setpclr = 0x21,
		setprstr = 0x22,
		killseq = 0x23,
		killsgt = 0x24,
		killsge = 0x25,
		killsne = 0x26,
		killsone = 0x27,
		sqrt = 0x28,
		opcode_41 = 0x29, // disassembler calls it this, probably unused garbage.
		mulsc0 = 0x2A, // same as mulsc.
		mulsc1 = 0x2B, // same as mulsc.
		addsc0 = 0x2C, // same as addsc.
		addsc1 = 0x2D, // same as addsc.
		subsc0 = 0x2E, // same as subsc.
		subsc1 = 0x2F, // same as subsc.
		sin = 0x30,
		cos = 0x31,
		retain_prev = 0x32,

		opcode_51 = 0x33,   // disassembler calls it this, probably unused garbage.
		opcode_52 = 0x34,   // disassembler calls it this, probably unused garbage.
		opcode_53 = 0x35,   // disassembler calls it this, probably unused garbage.
		opcode_54 = 0x36,   // disassembler calls it this, probably unused garbage.
		opcode_55 = 0x37,   // disassembler calls it this, probably unused garbage.
		opcode_56 = 0x38,   // disassembler calls it this, probably unused garbage.
		opcode_57 = 0x39,   // disassembler calls it this, probably unused garbage.
		opcode_58 = 0x3A,   // disassembler calls it this, probably unused garbage.
		opcode_59 = 0x3B,   // disassembler calls it this, probably unused garbage.
		opcode_60 = 0x3C,   // disassembler calls it this, probably unused garbage.
		opcode_61 = 0x3D,   // disassembler calls it this, probably unused garbage.
		opcode_62 = 0x3E,   // disassembler calls it this, probably unused garbage.
		opcode_63 = 0x3F,
	}
	public enum VectorOpcode : uint
	{
		add = 0x00,
		mul = 0x01,
		max = 0x02,
		min = 0x03,
		seq = 0x04,
		sgt = 0x05,
		sge = 0x06,
		sne = 0x07,
		frc = 0x08,
		trunc = 0x09,
		floor = 0x0A,
		mad = 0x0B,
		cndeq = 0x0C,
		cndge = 0x0D,
		cndgt = 0x0E,
		dp4 = 0x0F,
		dp3 = 0x10,
		dp2add = 0x11,
		cube = 0x12,
		max4 = 0x13,
		setp_eq_push = 0x14,
		setp_ne_push = 0x15,
		setp_gt_push = 0x16,
		setp_ge_push = 0x17,
		kill_eq = 0x18,
		kill_gt = 0x19,
		kill_ge = 0x1A,
		kill_ne = 0x1B,
		dst = 0x1C,
		maxa = 0x1D,
		opcode_30 = 0x1E, // disassembler calls it this, probably unused garbage.
		opcode_31 = 0x1F,
	}

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

		public uint src3_swiz;
		public uint src2_swiz;
		public uint src1_swiz;
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
		public bool is_export { get => export_data != 0; }

		// Whether the instruction is predicated (or conditional).
		public bool Is_predicated { get => is_predicated != 0; }
		// Required condition value of the comparision (true or false).
		public bool Pred_condition { get => pred_condition != 0; }

		public bool Abs_constants { get => abs_constants == 1; }
		public bool Is_const_0_addressed { get => const_0_rel_abs != 0; }
		public bool Is_const_1_addressed { get => const_1_rel_abs != 0; }
		public bool Is_address_relative { get => address_absolute != 0; }

		// Whether the instruction operates on the vector ALU
		public bool Has_vector_op { get => vector_write_mask != 0 || is_export; }
		public bool Is_vector_dest_relative { get => vector_dest_rel != 0; }
		public bool Vector_clamp { get => vector_clamp != 0; }

		// Whether the instruction operates on the scalar ALU
		public bool Has_scalar_op { get => scalar_opc != ScalarOpcode.retain_prev || (!is_export && scalar_write_mask != 0); }
		public bool Is_scalar_dest_relative { get => scalar_dest_rel != 0; }
		public bool Scalar_clamp { get => scalar_clamp != 0; }
	}
}
