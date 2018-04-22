#include <cstdint>

struct PackedVFetch {
	uint32_t opcode_value : 5;
	uint32_t src_reg : 6;
	uint32_t src_reg_am : 1;
	uint32_t dst_reg : 6;
	uint32_t dst_reg_am : 1;
	uint32_t must_be_one : 1;
	uint32_t const_index : 5;
	uint32_t const_index_sel : 2;
	uint32_t prefetch_count : 3;
	uint32_t src_swiz : 2;

	uint32_t dst_swiz : 12;
	uint32_t fomat_comp_all : 1;
	uint32_t num_format_all : 1;
	uint32_t signed_rf_mode_all : 1;
	uint32_t is_index_rounded : 1;
	uint32_t format : 6;
	uint32_t reserved2 : 2;
	uint32_t exp_adjust : 6;
	uint32_t is_mini_fetch : 1;
	uint32_t is_predicated : 1;

	uint32_t stride : 8;
	uint32_t offset : 23;
	uint32_t pred_condition : 1;
};
struct VFetch {
	uint32_t opcode_value;
	uint32_t src_reg;
	uint32_t src_reg_am;
	uint32_t dst_reg;
	uint32_t dst_reg_am;
	uint32_t must_be_one;
	uint32_t const_index;
	uint32_t const_index_sel;
	uint32_t prefetch_count;
	uint32_t src_swiz;

	uint32_t dst_swiz;
	uint32_t fomat_comp_all;
	uint32_t num_format_all;
	uint32_t signed_rf_mode_all;
	uint32_t is_index_rounded;
	uint32_t format;
	uint32_t reserved2;
	uint32_t exp_adjust;
	uint32_t is_mini_fetch;
	uint32_t is_predicated;

	uint32_t stride;
	uint32_t offset;
	uint32_t pred_condition;
};

struct PackedTFetch {
	uint32_t opcode_value : 5;
	uint32_t src_reg : 6;
	uint32_t src_reg_am : 1;
	uint32_t dst_reg : 6;
	uint32_t dst_reg_am : 1;
	uint32_t fetch_valid_only : 1;
	uint32_t const_index : 5;
	uint32_t tx_coord_denorm : 1;
	uint32_t src_swiz : 6;  // xyz

	uint32_t dst_swiz : 12;         // xyzw
	uint32_t mag_filter : 2;        // instr_tex_filter_t
	uint32_t min_filter : 2;        // instr_tex_filter_t
	uint32_t mip_filter : 2;        // instr_tex_filter_t
	uint32_t aniso_filter : 3;      // instr_aniso_filter_t
	uint32_t arbitrary_filter : 3;  // instr_arbitrary_filter_t
	uint32_t vol_mag_filter : 2;    // instr_tex_filter_t
	uint32_t vol_min_filter : 2;    // instr_tex_filter_t
	uint32_t use_comp_lod : 1;
	uint32_t use_reg_lod : 1;
	uint32_t unk : 1;
	uint32_t is_predicated : 1;

	uint32_t use_reg_gradients : 1;
	uint32_t sample_location : 1;
	uint32_t lod_bias : 7;
	uint32_t unused : 5;
	uint32_t dimension : 2;
	uint32_t offset_x : 5;
	uint32_t offset_y : 5;
	uint32_t offset_z : 5;
	uint32_t pred_condition : 1;
};
struct TFetch {
	uint32_t opcode_value;
	uint32_t src_reg;
	uint32_t src_reg_am;
	uint32_t dst_reg;
	uint32_t dst_reg_am;
	uint32_t fetch_valid_only;
	uint32_t const_index;
	uint32_t tx_coord_denorm;
	uint32_t src_swiz;  // xyz

	uint32_t dst_swiz;         // xyzw
	uint32_t mag_filter;        // instr_tex_filter_t
	uint32_t min_filter;        // instr_tex_filter_t
	uint32_t mip_filter;        // instr_tex_filter_t
	uint32_t aniso_filter;      // instr_aniso_filter_t
	uint32_t arbitrary_filter;  // instr_arbitrary_filter_t
	uint32_t vol_mag_filter;    // instr_tex_filter_t
	uint32_t vol_min_filter;    // instr_tex_filter_t
	uint32_t use_comp_lod;
	uint32_t use_reg_lod;
	uint32_t unk;
	uint32_t is_predicated;

	uint32_t use_reg_gradients;
	uint32_t sample_location;
	uint32_t lod_bias;
	uint32_t unused;
	uint32_t dimension;
	uint32_t offset_x;
	uint32_t offset_y;
	uint32_t offset_z;
	uint32_t pred_condition;
};

union FetchUnion {
	PackedVFetch vfetch;
	PackedTFetch tfetch;

	struct {
		uint32_t dword0;
		uint32_t dword1;
		uint32_t dword2;
	};
};

struct FetchInstruction {
	VFetch vfetch;
	TFetch tfetch;
	uint32_t opcode;
};

extern "C"
{
	__declspec(dllexport) void decodeFetch(uint32_t dword0, uint32_t dword1, uint32_t dword2, FetchInstruction *fi)
	{
		FetchUnion fu;
		fu.dword0 = dword0;
		fu.dword1 = dword1;
		fu.dword2 = dword2;

		fi->opcode = fu.vfetch.opcode_value;

		fi->vfetch.const_index = fu.vfetch.const_index;
		fi->vfetch.const_index_sel = fu.vfetch.const_index_sel;
		fi->vfetch.dst_reg = fu.vfetch.dst_reg;
		fi->vfetch.dst_reg_am = fu.vfetch.dst_reg_am;
		fi->vfetch.dst_swiz = fu.vfetch.dst_swiz;
		fi->vfetch.exp_adjust = fu.vfetch.exp_adjust;
		fi->vfetch.fomat_comp_all = fu.vfetch.fomat_comp_all;
		fi->vfetch.format = fu.vfetch.format;
		fi->vfetch.is_index_rounded = fu.vfetch.is_index_rounded;
		fi->vfetch.is_mini_fetch = fu.vfetch.is_mini_fetch;
		fi->vfetch.is_predicated = fu.vfetch.is_predicated;
		fi->vfetch.must_be_one = fu.vfetch.must_be_one;
		fi->vfetch.num_format_all = fu.vfetch.num_format_all;
		fi->vfetch.offset = fu.vfetch.offset;
		fi->vfetch.opcode_value = fu.vfetch.opcode_value;
		fi->vfetch.pred_condition = fu.vfetch.pred_condition;
		fi->vfetch.prefetch_count = fu.vfetch.prefetch_count;
		fi->vfetch.reserved2 = fu.vfetch.reserved2;
		fi->vfetch.signed_rf_mode_all = fu.vfetch.signed_rf_mode_all;
		fi->vfetch.src_reg = fu.vfetch.src_reg;
		fi->vfetch.src_reg_am = fu.vfetch.src_reg_am;
		fi->vfetch.src_swiz = fu.vfetch.src_swiz;
		fi->vfetch.stride = fu.vfetch.stride;

		fi->tfetch.aniso_filter = fu.tfetch.aniso_filter;
		fi->tfetch.arbitrary_filter = fu.tfetch.arbitrary_filter;
		fi->tfetch.const_index = fu.tfetch.const_index;
		fi->tfetch.dimension = fu.tfetch.dimension;
		fi->tfetch.dst_reg = fu.tfetch.dst_reg;
		fi->tfetch.dst_reg_am = fu.tfetch.dst_reg_am;
		fi->tfetch.dst_swiz = fu.tfetch.dst_swiz;
		fi->tfetch.fetch_valid_only = fu.tfetch.fetch_valid_only;
		fi->tfetch.is_predicated = fu.tfetch.is_predicated;
		fi->tfetch.lod_bias = fu.tfetch.lod_bias;
		fi->tfetch.mag_filter = fu.tfetch.mag_filter;
		fi->tfetch.min_filter = fu.tfetch.min_filter;
		fi->tfetch.mip_filter = fu.tfetch.mip_filter;
		fi->tfetch.offset_x = fu.tfetch.offset_x;
		fi->tfetch.offset_y = fu.tfetch.offset_y;
		fi->tfetch.offset_z = fu.tfetch.offset_z;
		fi->tfetch.opcode_value = fu.tfetch.opcode_value;
		fi->tfetch.pred_condition = fu.tfetch.pred_condition;
		fi->tfetch.sample_location = fu.tfetch.sample_location;
		fi->tfetch.src_reg = fu.tfetch.src_reg;
		fi->tfetch.src_reg_am = fu.tfetch.src_reg_am;
		fi->tfetch.src_swiz = fu.tfetch.src_swiz;
		fi->tfetch.tx_coord_denorm = fu.tfetch.tx_coord_denorm;
		fi->tfetch.unk = fu.tfetch.unk;
		fi->tfetch.unused = fu.tfetch.unused;
		fi->tfetch.use_comp_lod = fu.tfetch.use_comp_lod;
		fi->tfetch.use_reg_gradients = fu.tfetch.use_reg_gradients;
		fi->tfetch.use_reg_lod = fu.tfetch.use_reg_lod;
		fi->tfetch.vol_mag_filter = fu.tfetch.vol_mag_filter;
		fi->tfetch.vol_min_filter = fu.tfetch.vol_min_filter;
	}
}