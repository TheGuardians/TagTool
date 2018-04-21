using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderDisassembler
{
	public enum FetchOpCode
	{
		vfetch = 0x00,
		tfetch = 0x01,
		getBCF = 0x10,
		getCompTexLOD = 0x11,
		getGradients = 0x12,
		getWeights = 0x13,
		setTexLOD = 0x18,
		setGradientH = 0x19,
		setGradientV = 0x1A,
		kUnknownTextureOp = 0x1B,
	}
	public enum TextureFilter
	{
		Point = 0x00,
		Linear = 0x01,
		BaseMap = 0x02,  // Only applicable for mip-filter.
		UseFetchConst = 0x03,
	}
	public enum AnisoFilter
	{
		Disabled = 0x00,
		Max_1_1 = 0x01,
		Max_2_1 = 0x02,
		Max_4_1 = 0x03,
		Max_8_1 = 0x04,
		Max_16_1 = 0x05,
		UseFetchConst = 0x07,
	}
	public enum ArbitraryFilter
	{
		ARBITRARY_FILTER_2X4_SYM = 0x00,
		ARBITRARY_FILTER_2X4_ASYM = 0x01,
		ARBITRARY_FILTER_4X2_SYM = 0x02,
		ARBITRARY_FILTER_4X2_ASYM = 0x03,
		ARBITRARY_FILTER_4X4_SYM = 0x04,
		ARBITRARY_FILTER_4X4_ASYM = 0x05,
		ARBITRARY_FILTER_USE_FETCH_CONST = 0x07,
	}
	public enum SampleLocation
	{
		Centroid = 0x00,
		Center = 0x01,
	}
	public enum SurfaceFormat
	{
		FMT_1_REVERSE = 0x00,
		FMT_1 = 0x01,
		FMT_8 = 0x02,
		FMT_1_5_5_5 = 0x03,
		FMT_5_6_5 = 0x04,
		FMT_6_5_5 = 0x05,
		FMT_8_8_8_8 = 0x06,
		FMT_2_10_10_10 = 0x07,
		FMT_8_A = 0x08,
		FMT_8_B = 0x09,
		FMT_8_8 = 0x0A,
		FMT_Cr_Y1_Cb_Y0 = 0x0B,
		FMT_Y1_Cr_Y0_Cb = 0x0C,
		FMT_5_5_5_1 = 0x0D,
		FMT_8_8_8_8_A = 0x0E,
		FMT_4_4_4_4 = 0x0F,
		FMT_10_11_11 = 0x10,
		FMT_11_11_10 = 0x11,
		FMT_DXT1 = 0x12,
		FMT_DXT2_3 = 0x13,
		FMT_DXT4_5 = 0x14,
		FMT_24_8 = 0x16,
		FMT_24_8_FLOAT = 0x17,
		FMT_16 = 0x18,
		FMT_16_16 = 0x19,
		FMT_16_16_16_16 = 0x1A,
		FMT_16_EXPAND = 0x1B,
		FMT_16_16_EXPAND = 0x1C,
		FMT_16_16_16_16_EXPAND = 0x1D,
		FMT_16_FLOAT = 0x1E,
		FMT_16_16_FLOAT = 0x1F,
		FMT_16_16_16_16_FLOAT = 0x20,
		FMT_32 = 0x21,
		FMT_32_32 = 0x22,
		FMT_32_32_32_32 = 0x23,
		FMT_32_FLOAT = 0x24,
		FMT_32_32_FLOAT = 0x25,
		FMT_32_32_32_32_FLOAT = 0x26,
		FMT_32_AS_8 = 0x27,
		FMT_32_AS_8_8 = 0x28,
		FMT_16_MPEG = 0x29,
		FMT_16_16_MPEG = 0x2A,
		FMT_8_INTERLACED = 0x2B,
		FMT_32_AS_8_INTERLACED = 0x2C,
		FMT_32_AS_8_8_INTERLACED = 0x2D,
		FMT_16_INTERLACED = 0x2E,
		FMT_16_MPEG_INTERLACED = 0x2F,
		FMT_16_16_MPEG_INTERLACED = 0x30,
		FMT_DXN = 0x31,
		FMT_8_8_8_8_AS_16_16_16_16 = 0x32,
		FMT_DXT1_AS_16_16_16_16 = 0x33,
		FMT_DXT2_3_AS_16_16_16_16 = 0x34,
		FMT_DXT4_5_AS_16_16_16_16 = 0x35,
		FMT_2_10_10_10_AS_16_16_16_16 = 0x36,
		FMT_10_11_11_AS_16_16_16_16 = 0x37,
		FMT_11_11_10_AS_16_16_16_16 = 0x38,
		FMT_32_32_32_FLOAT = 0x39,
		FMT_DXT3A = 0x3A,
		FMT_DXT5A = 0x3B,
		FMT_CTX1 = 0x3C,
		FMT_DXT3A_AS_1_1_1_1 = 0x3D,
	}
	public enum VertexFormat
	{
		kUndefined = 0x00,
		k_8_8_8_8 = 0x06,
		k_2_10_10_10 = 0x07,
		k_10_11_11 = 0x10,
		k_11_11_10 = 0x11,
		k_16_16 = 0x19,
		k_16_16_16_16 = 0x1A,
		k_16_16_FLOAT = 0x1F,
		k_16_16_16_16_FLOAT = 0x20,
		k_32 = 0x21,
		k_32_32 = 0x22,
		k_32_32_32_32 = 0x23,
		k_32_FLOAT = 0x24,
		k_32_32_FLOAT = 0x25,
		k_32_32_32_32_FLOAT = 0x26,
		k_32_32_32_FLOAT = 0x39,
	}
	public enum TextureDimension : byte
	{
		One = 0x00,
		Two = 0x01,
		Three = 0x02,
		Cube = 0x03,
	}

	public struct VFetch
	{
		public FetchOpCode opcode_value;
		public uint src_reg;
		public uint src_reg_am;
		public uint dst_reg;
		public uint dst_reg_am;
		public uint must_be_one;
		public uint const_index;
		public uint const_index_sel;
		public uint prefetch_count;
		public uint src_swiz;

		public uint dst_swiz;
		public uint fomat_comp_all;
		public uint num_format_all;
		public uint signed_rf_mode_all;
		public uint is_index_rounded;
		public uint format;
		public uint reserved2;
		public uint exp_adjust;
		public uint is_mini_fetch;
		public uint is_predicated;

		public uint stride;
		public uint offset;
		public uint pred_condition;
	}

	public struct TFetch
	{
		public FetchOpCode opcode_value;
		public uint src_reg;
		public uint src_reg_am;
		public uint dst_reg;
		public uint dst_reg_am;
		public uint fetch_valid_only;
		public uint const_index;
		public uint tx_coord_denorm;
		public uint src_swiz;  // xyz

		public uint dst_swiz;         // xyzw
		public TextureFilter mag_filter;        // instr_tex_filter_t
		public TextureFilter min_filter;        // instr_tex_filter_t
		public TextureFilter mip_filter;        // instr_tex_filter_t
		public AnisoFilter aniso_filter;      // instr_aniso_filter_t
		public ArbitraryFilter arbitrary_filter;  // instr_arbitrary_filter_t
		public TextureFilter vol_mag_filter;    // instr_tex_filter_t
		public TextureFilter vol_min_filter;    // instr_tex_filter_t
		public uint use_comp_lod;
		public uint use_reg_lod;
		public uint unk;
		public uint is_predicated;

		public uint use_reg_gradients;
		public SampleLocation sample_location;
		public uint lod_bias;
		public uint unused;
		public TextureDimension dimension;
		public uint offset_x;
		public uint offset_y;
		public uint offset_z;
		public uint pred_condition;
	};

	public struct FetchInstruction
	{
		VFetch vFetch;
		TFetch tfetch;
		FetchOpCode opcode;
	}
}
