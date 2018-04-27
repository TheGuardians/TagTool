#include <cstdint>

/* WARNING: DO NOT TOUCH THIS FILE UNLESS YOU KNOW WHAT YOU'RE DOING, AND ALSO MAKE THE APPROPRIATE
CHANGES TO THE MATCHING FILE IN THE `TagTool` PROJECT */

__declspec(align(1)) struct PackedALU {
	uint32_t vector_dest : 6;
	uint32_t vector_dest_rel : 1;
	uint32_t abs_constants : 1;
	uint32_t scalar_dest : 6;
	uint32_t scalar_dest_rel : 1;
	uint32_t export_data : 1;
	uint32_t vector_write_mask : 4;
	uint32_t scalar_write_mask : 4;
	uint32_t vector_clamp : 1;
	uint32_t scalar_clamp : 1;
	uint32_t scalar_opc : 6;  // instr_scalar_opc_t

	uint32_t src3_swiz : 8;
	uint32_t src2_swiz : 8;
	uint32_t src1_swiz : 8;
	uint32_t src3_reg_negate : 1;
	uint32_t src2_reg_negate : 1;
	uint32_t src1_reg_negate : 1;
	uint32_t pred_condition : 1;
	uint32_t is_predicated : 1;
	uint32_t address_absolute : 1;
	uint32_t const_1_rel_abs : 1;
	uint32_t const_0_rel_abs : 1;

	uint32_t src3_reg : 8;
	uint32_t src2_reg : 8;
	uint32_t src1_reg : 8;
	uint32_t vector_opc : 5;  // instr_vector_opc_t
	uint32_t src3_sel : 1;
	uint32_t src2_sel : 1;
	uint32_t src1_sel : 1;
};
__declspec(align(1)) struct ALUInstruction {
	uint32_t vector_dest;
	uint32_t vector_dest_rel;
	uint32_t abs_constants;
	uint32_t scalar_dest;
	uint32_t scalar_dest_rel;
	uint32_t export_data;
	uint32_t vector_write_mask;
	uint32_t scalar_write_mask;
	uint32_t vector_clamp;
	uint32_t scalar_clamp;
	uint32_t scalar_opc;  // instr_scalar_opc_t

	uint32_t src3_swiz;
	uint32_t src2_swiz;
	uint32_t src1_swiz;
	uint32_t src3_reg_negate;
	uint32_t src2_reg_negate;
	uint32_t src1_reg_negate;
	uint32_t pred_condition;
	uint32_t is_predicated;
	uint32_t address_absolute;
	uint32_t const_1_rel_abs;
	uint32_t const_0_rel_abs;

	uint32_t src3_reg;
	uint32_t src2_reg;
	uint32_t src1_reg;
	uint32_t vector_opc;  // instr_vector_opc_t
	uint32_t src3_sel;
	uint32_t src2_sel;
	uint32_t src1_sel;
};

__declspec(align(1)) union ALUUnion {
	PackedALU alu;

	struct {
		uint32_t dword0;
		uint32_t dword1;
		uint32_t dword2;
	};
};

extern "C"
{
	__declspec(dllexport) void decodeALU(uint32_t dword0, uint32_t dword1, uint32_t dword2, ALUInstruction *alui)
	{
		ALUUnion aluu;
		aluu.dword0 = dword0;
		aluu.dword1 = dword1;
		aluu.dword2 = dword2;

		alui->abs_constants = aluu.alu.abs_constants;
		alui->address_absolute = aluu.alu.address_absolute;
		alui->const_0_rel_abs = aluu.alu.const_0_rel_abs;
		alui->const_1_rel_abs = aluu.alu.const_1_rel_abs;
		alui->export_data = aluu.alu.export_data;
		alui->is_predicated = aluu.alu.is_predicated;
		alui->pred_condition = aluu.alu.pred_condition;
		alui->scalar_clamp = aluu.alu.scalar_clamp;
		alui->scalar_dest = aluu.alu.scalar_dest;
		alui->scalar_dest_rel = aluu.alu.scalar_dest_rel;
		alui->scalar_opc = aluu.alu.scalar_opc;
		alui->scalar_write_mask = aluu.alu.scalar_write_mask;
		alui->src1_reg = aluu.alu.src1_reg;
		alui->src1_reg_negate = aluu.alu.src1_reg_negate;
		alui->src1_sel = aluu.alu.src1_sel;
		alui->src1_swiz = aluu.alu.src1_swiz;
		alui->src2_reg = aluu.alu.src2_reg;
		alui->src2_reg_negate = aluu.alu.src2_reg_negate;
		alui->src2_sel = aluu.alu.src2_sel;
		alui->src2_swiz = aluu.alu.src2_swiz;
		alui->src3_reg = aluu.alu.src3_reg;
		alui->src3_reg_negate = aluu.alu.src3_reg_negate;
		alui->src3_sel = aluu.alu.src3_sel;
		alui->src3_swiz = aluu.alu.src3_swiz;
		alui->vector_clamp = aluu.alu.vector_clamp;
		alui->vector_dest = aluu.alu.vector_dest;
		alui->vector_dest_rel = aluu.alu.vector_dest_rel;
		alui->vector_opc = aluu.alu.vector_opc;
		alui->vector_write_mask = aluu.alu.vector_write_mask;
	}
}