using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_template", Tag = "rmt2", Size = 0x84)]
    public class RenderMethodTemplate : TagStructure
	{
        public CachedTag VertexShader;
        public CachedTag PixelShader;
        public EntryPointBitMask ValidEntryPoints;
        public List<PackedInteger_10_6> EntryPoints; // Ranges of ParameterTables by usage
        public List<ParameterTable> ParameterTables; // Ranges of Parameters
        public List<ParameterMapping> Parameters; 
        public List<ShaderArgument> RealParameterNames;
        public List<ShaderArgument> IntegerParameterNames;
        public List<ShaderArgument> BooleanParameterNames;
        public List<ShaderArgument> TextureParameterNames;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] Unused;

        public enum RenderMethodExtern : byte
        {
            none,
            texture_global_target_texaccum,
            texture_global_target_normal,
            texture_global_target_z,
            texture_global_target_shadow_buffer1,
            texture_global_target_shadow_buffer2,
            texture_global_target_shadow_buffer3,
            texture_global_target_shadow_buffer4,
            texture_global_target_texture_camera,
            texture_global_target_reflection,
            texture_global_target_refraction,
            texture_lightprobe_texture,
            texture_dominant_light_intensity_map,
            texture_unused1,
            texture_unused2,
            object_change_color_primary,
            object_change_color_secondary,
            object_change_color_tertiary,
            object_change_color_quaternary,
            object_change_color_quinary,
            object_change_color_primary_anim,
            object_change_color_secondary_anim,
            texture_dynamic_environment_map_0,
            texture_dynamic_environment_map_1,
            texture_cook_torrance_cc0236,
            texture_cook_torrance_dd0236,
            texture_cook_torrance_c78d78,
            light_dir_0,
            light_color_0,
            light_dir_1,
            light_color_1,
            light_dir_2,
            light_color_2,
            light_dir_3,
            light_color_3,
            texture_unused_3,
            texture_unused_4,
            texture_unused_5,
            texture_dynamic_light_gel_0,
            flat_envmap_matrix_x,
            flat_envmap_matrix_y,
            flat_envmap_matrix_z,
            debug_tint,
            screen_constants,
            active_camo_distortion_texture,
            scene_ldr_texture,
            scene_hdr_texture,
            water_memory_export_address,
            tree_animation_timer,
            emblem_player_shoulder_texture,
            emblem_clan_chest_texture,
        }

        public enum EntryPoint : sbyte
        {
            Default,
            Albedo,
            Static_Default,
            Static_Per_Pixel,
            Static_Per_Vertex,
            Static_Sh,
            Static_Prt_Ambient,
            Static_Prt_Linear,
            Static_Prt_Quadratic,
            Dynamic_Light,
            Shadow_Generate,
            Shadow_Apply,
            Active_Camo,
            Lightmap_Debug_Mode,
            Static_Per_Vertex_Color,
            Water_Tessellation,
            Water_Shading,
            Dynamic_Light_Cinematic,
            Z_Only,
            Sfx_Distort
        }

        public enum EntryPointBitMask : uint
        {
            Default = 1 << 0,
            Albedo = 1 << 1,
            Static_Default = 1 << 2,
            Static_Per_Pixel = 1 << 3,
            Static_Per_Vertex = 1 << 4,
            Static_Sh = 1 << 5,
            Static_Prt_Ambient = 1 << 6,
            Static_Prt_Linear = 1 << 7,
            Static_Prt_Quadratic = 1 << 8,
            Dynamic_Light = 1 << 9,
            Shadow_Generate = 1 << 10,
            Shadow_Apply = 1 << 11,
            Active_Camo = 1 << 12,
            Lightmap_Debug_Mode = 1 << 13,
            Static_Per_Vertex_Color = 1 << 14,
            Water_Tessellation = 1 << 15,
            Water_Shading = 1 << 16,
            Dynamic_Light_Cinematic = 1 << 17,
            Z_Only = 1 << 18,
            Sfx_Distort = 1 << 19,
        }

        [TagStructure(Size = 0x2)]
        public class PackedInteger_10_6 : TagStructure
		{
            public ushort Offset { get => GetOffset(); set => SetOffset(value); }
            public ushort Count { get => GetCount(); set => SetCount(value); }

            private ushort GetCount() => (ushort)(Integer >> 10);
            private ushort GetOffset() => (ushort)(Integer & 0x3FFu);
            private void SetCount(ushort count)
            {
                if (count > 0x3Fu) throw new System.Exception("Out of range");
                var a = GetOffset();
                var b = (count & 0x3F) << 10;
                var value = (ushort)(a | b);
                Integer = value;
            }
            private void SetOffset(ushort _offset)
            {
                if (_offset > 0x3FFu) throw new System.Exception("Out of range");
                var a = (_offset & 0x3FF);
                var b = (GetCount() & 0x3F) << 10;
                var value = (ushort)(a | b);
                Integer = value;
            }

            public ushort Integer;
        }

        public enum ParameterUsage
        {
            Texture,
            VS_Real,
            VS_Integer,
            VS_Boolean,
            PS_Real,
            PS_Integer,
            PS_Boolean,
            TextureExtern,
            VS_RealExtern,
            VS_IntegerExtern,
            PS_RealExtern,
            PS_IntegerExtern,
            Unused1,
            Unused2,
            Count
        }

        [TagStructure(Size = 0x1C)]
        public class ParameterTable : TagStructure
		{
            [TagField(Length = (int)ParameterUsage.Count)]
            public PackedInteger_10_6[] Values = new PackedInteger_10_6[(int)ParameterUsage.Count];

            public PackedInteger_10_6 this[ParameterUsage usage]
            {
                get { return Values[(int)usage]; }
                set { Values[(int)usage] = value; }
            }
        }

        /// <summary>
        /// Binds an argument in the render method tag to a pixel shader constant.
        /// </summary>
        [TagStructure(Size = 0x4)]
        public class ParameterMapping : TagStructure
		{
            /// <summary>
            /// The GPU register to bind the argument to.
            /// </summary>
            public ushort RegisterIndex;

            /// <summary>
            /// The index of the argument in one of the blocks in the render method tag.
            /// The block used depends on the argument type.
            /// </summary>
            public byte ArgumentIndex;

            public byte Unknown;
        }

        [TagStructure(Size = 0x4)]
        public class ShaderArgument : TagStructure
		{
            public StringId Name;
        }
    }
}