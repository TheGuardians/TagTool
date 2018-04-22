using System.Runtime.InteropServices;

/* WARNING: DO NOT TOUCH THIS FILE UNLESS YOU KNOW WHAT YOU'RE DOING, AND ALSO MAKE THE APPROPRIATE
 CHANGES TO THE MATCHING FILE IN THE `TagToolUtilities` PROJECT */

namespace TagTool.ShaderDecompiler.UcodeDisassembler
{
	public enum FetchOpCode
	{
		// Fetches data from a vertex buffer using a semantic. 
		vfetch = 0x00,

		// Fetches sample data from a texture. 
		tfetch = 0x01,

		//Gets the fraction of border color that would be blended into the texture data 
		// (retrieved using a texture-fetch) at the specified coordinates. 
		getBCF = 0x10,

		// For textures, gets the LOD for all of the pixels in the quad at the specified coordinates. 
		getCompTexLOD = 0x11,

		// Gets the gradients of the first source, relative to the screen horizontal and screen vertical. 
		getGradients = 0x12,

		// Gets the weights used in a bilinear fetch from textures. 
		getWeights = 0x13,

		// Sets the level of detail. 
		setTexLOD = 0x18,

		// Sets the horizontal gradients. 
		setGradientH = 0x19,

		// Sets the vertical gradients. 
		setGradientV = 0x1A,

		// Converts xbox360 shaders to PC format! ecks dee.
		UnknownTextureOp = 0x1B,
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
	public enum TextureDimension
	{
		One = 0x00,
		Two = 0x01,
		Three = 0x02,
		Cube = 0x03,
	}

	// Instruction data for FetchOpCode.vfetch
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct VFetch
	{
		public FetchOpCode opcode_value;
		public uint src_reg;
		private uint src_reg_am;
		public uint dst_reg;
		private uint dst_reg_am;
		public uint must_be_one;
		public uint const_index;
		public uint const_index_sel;
		public uint prefetch_count;
		public uint src_swiz;

		public uint dst_swiz;
		private uint fomat_comp_all;
		private uint num_format_all;
		public uint signed_rf_mode_all;
		private uint is_index_rounded;
		public VertexFormat format;
		public uint reserved2;
		public uint exp_adjust;
		private uint is_mini_fetch;
		private uint is_predicated;

		public uint stride;
		public uint offset;
		private uint pred_condition;

		// Whether the instruction is predicated (or conditional).
		public bool Is_predicated { get => is_predicated != 0; }
		// Required condition value of the comparision (true or false).
		public bool Predicate_condition { get => pred_condition != 0; }

		public bool Is_dest_relative { get => dst_reg_am != 0; }
		public bool Is_src_relative { get => src_reg_am != 0; }

		public bool Is_mini_fetch { get => is_mini_fetch != 0; }

		public bool Is_signed { get => fomat_comp_all != 0; }
		public bool Is_unnormalized { get => num_format_all != 0; }
		public bool Is_index_rounded { get => is_index_rounded != 0; }

		// Returns true if the fetch actually fetches data. This may be false if it's used only to populate constants.
		public bool Fetches_any_data
		{
			get
			{
				var _dst_swiz = dst_swiz;
				bool fetches_any_data = false;
				for (int i = 0; i < 4; i++)
				{
					if ((_dst_swiz & 0x7) == 4)
					{
						// 0.0
					}
					else if ((_dst_swiz & 0x7) == 5)
					{
						// 1.0
					}
					else if ((_dst_swiz & 0x7) == 6)
					{
						// ?
					}
					else if ((_dst_swiz & 0x7) == 7)
					{
						// Previous register value.
					}
					else
					{
						fetches_any_data = true;
						break;
					}
					_dst_swiz >>= 3;
				}
				return fetches_any_data;
			}
		}
	}

	// Instruction data for FetchOpCode.tfetch, getBCF, getCompTexLod, getGradients, getWeights, setTexLOD,
	// setGradientH, setGradientV, and "kUnknownTextureOp"
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct TFetch
	{
		public FetchOpCode opcode_value;
		public uint src_reg;
		private uint src_reg_am;
		public uint dst_reg;
		private uint dst_reg_am;
		private uint fetch_valid_only;
		public uint const_index;
		private uint tx_coord_denorm;
		public uint src_swiz;  // xyz

		public uint dst_swiz;         // xyzw
		private TextureFilter mag_filter;        // instr_tex_filter_t
		private TextureFilter min_filter;        // instr_tex_filter_t
		private TextureFilter mip_filter;        // instr_tex_filter_t
		private AnisoFilter aniso_filter;		// instr_aniso_filter_t
		public ArbitraryFilter arbitrary_filter;// instr_arbitrary_filter_t
		public TextureFilter vol_mag_filter;    // instr_tex_filter_t
		public TextureFilter vol_min_filter;    // instr_tex_filter_t
		private uint use_comp_lod;
		private uint use_reg_lod;
		public uint unk;
		private uint is_predicated;

		private uint use_reg_gradients;
		public SampleLocation sample_location;
		public uint lod_bias;
		public uint unused;
		public TextureDimension dimension;

		// Xenia does some weird conversion of these instead of just casting to float. https://github.com/benvanik/xenia/blob/55d2c03943b475fbbfe150b1ab9d8374ac15a132/src/xenia/gpu/ucode.h#L643-L651
		public uint offset_x;
		public uint offset_y;
		public uint offset_z;

		private uint pred_condition;

		// Whether the instruction is predicated (or conditional).
		public bool Is_predicated { get => is_predicated != 0; }
		// Required condition value of the comparision (true or false).
		public bool Predicate_condition { get => pred_condition != 0; }

		public bool Is_dest_relative { get => dst_reg_am != 0; }
		public bool Is_src_relative { get => src_reg_am != 0; }

		public bool Fetch_valid_only{ get => fetch_valid_only != 0; }
		public bool Unnormalized_coordinates{ get => tx_coord_denorm != 0; }
		public bool Has_mag_filter { get => mag_filter != TextureFilter.UseFetchConst; }
		public bool Has_min_filter { get => min_filter != TextureFilter.UseFetchConst; }
		public bool Has_mip_filter { get => mip_filter != TextureFilter.UseFetchConst; }
		public bool Has_aniso_filter { get => aniso_filter != AnisoFilter.UseFetchConst; }

		public bool Use_computed_lod { get => use_comp_lod != 0; }
		public bool Ese_register_lod { get => use_reg_lod != 0; }
		public bool Use_register_gradients { get => use_reg_gradients != 0; }
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct FetchInstruction
	{
		public VFetch vFetch;
		public TFetch tfetch;
		public FetchOpCode opcode;
	}
}
