using System.Runtime.InteropServices;

/* WARNING: DO NOT TOUCH THIS FILE UNLESS YOU KNOW WHAT YOU'RE DOING, AND ALSO MAKE THE APPROPRIATE
 CHANGES TO THE MATCHING FILE IN THE `TagToolUtilities` PROJECT */

namespace TagTool.ShaderDecompiler.UcodeDisassembler
{
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
