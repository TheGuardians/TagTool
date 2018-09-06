using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_template", Tag = "rmt2", Size = 0x84)]
    public class RenderMethodTemplate
    {
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

        public enum ShaderMode : sbyte
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

        public enum ShaderModeBitmask : uint
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

        public CachedTagInstance VertexShader;
        public CachedTagInstance PixelShader;
        public ShaderModeBitmask DrawModeBitmask;
        public List<DrawMode> DrawModes; // Entries in here correspond to an enum in the EXE
        public List<DrawModeRegisterOffsetBlock> RegisterOffsets;
        public List<ArgumentMapping> ArgumentMappings;
        public List<ShaderArgument> VectorArguments;
        public List<ShaderArgument> IntegerArguments;
        public List<ShaderArgument> BooleanArguments;
        public List<ShaderArgument> SamplerArguments;

        [TagField(Padding = true, Length = 12)]
        public byte[] Unused;

        [TagStructure(Size = 0x2)]
        public class PackedInteger_10_6
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

        // getters and setters put in here as this value is endian dependant
        [TagStructure(Size = 0x0)]
        public class DrawMode : PackedInteger_10_6
        {

        }

        [TagStructure(Size = 0x1C)]
        public class DrawModeRegisterOffsetBlock
        {
            public enum DrawModeRegisterOffsetType
            {
                SamplerArguments,
                WaterVectorArguments,
                Unknown1,
                Unknown2,
                VectorArguments,
                IntegerArguments,
                GlobalArguments,
                RenderMethodExternArguments,
                Unknown4,
                Unknown5,
                DebugVectorRegisters,
                Unknown6,
                Unknown7,
                Unknown8,
                DrawModeRegisterOffsetType_Count
            }

            public enum DrawModeRegisterOffsetTypeBits
            {
                ShaderMapSamplerRegisters = 1 << 0,
                UnknownVectorRegisters = 1 << 1,
                Unknown1 = 1 << 2,
                Unknown2 = 1 << 3,
                ArgumentsVectorRegisters = 1 << 4,
                Unknown3 = 1 << 5,
                GlobalArgumentsVectorRegisters = 1 << 6,
                RenderBufferSamplerRegisters = 1 << 7,
                Unknown4 = 1 << 8,
                Unknown5 = 1 << 9,
                DebugVectorRegisters = 1 << 10,
                Unknown6 = 1 << 11,
                Unknown7 = 1 << 12,
                Unknown8 = 1 << 13
            }

            public ushort SamplerArguments_Offset { get => GetOffset(DrawModeRegisterOffsetType.SamplerArguments); set => SetOffset(DrawModeRegisterOffsetType.SamplerArguments, value); }
            public ushort WaterVectorArguments_Offset { get => GetOffset(DrawModeRegisterOffsetType.WaterVectorArguments); set => SetOffset(DrawModeRegisterOffsetType.WaterVectorArguments, value); }
            public ushort Unknown1_Offset { get => GetOffset(DrawModeRegisterOffsetType.Unknown1); set => SetOffset(DrawModeRegisterOffsetType.Unknown1, value); }
            public ushort Unknown2_Offset { get => GetOffset(DrawModeRegisterOffsetType.Unknown2); set => SetOffset(DrawModeRegisterOffsetType.Unknown2, value); }
            public ushort VectorArguments_Offset { get => GetOffset(DrawModeRegisterOffsetType.VectorArguments); set => SetOffset(DrawModeRegisterOffsetType.VectorArguments, value); }
            public ushort IntegerArguments_Offset { get => GetOffset(DrawModeRegisterOffsetType.IntegerArguments); set => SetOffset(DrawModeRegisterOffsetType.IntegerArguments, value); }
            public ushort GlobalArguments_Offset { get => GetOffset(DrawModeRegisterOffsetType.GlobalArguments); set => SetOffset(DrawModeRegisterOffsetType.GlobalArguments, value); }
            public ushort RenderMethodExternArguments_Offset { get => GetOffset(DrawModeRegisterOffsetType.RenderMethodExternArguments); set => SetOffset(DrawModeRegisterOffsetType.RenderMethodExternArguments, value); }
            public ushort Unknown4_Offset { get => GetOffset(DrawModeRegisterOffsetType.Unknown4); set => SetOffset(DrawModeRegisterOffsetType.Unknown4, value); }
            public ushort Unknown5_Offset { get => GetOffset(DrawModeRegisterOffsetType.Unknown5); set => SetOffset(DrawModeRegisterOffsetType.Unknown5, value); }
            public ushort DebugVectorRegisters_Offset { get => GetOffset(DrawModeRegisterOffsetType.DebugVectorRegisters); set => SetOffset(DrawModeRegisterOffsetType.DebugVectorRegisters, value); }
            public ushort Unknown6_Offset { get => GetOffset(DrawModeRegisterOffsetType.Unknown6); set => SetOffset(DrawModeRegisterOffsetType.Unknown6, value); }
            public ushort Unknown7_Offset { get => GetOffset(DrawModeRegisterOffsetType.Unknown7); set => SetOffset(DrawModeRegisterOffsetType.Unknown7, value); }
            public ushort Unknown8_Offset { get => GetOffset(DrawModeRegisterOffsetType.Unknown8); set => SetOffset(DrawModeRegisterOffsetType.Unknown8, value); }
            public ushort SamplerArguments_Count { get => GetCount(DrawModeRegisterOffsetType.SamplerArguments); set => SetCount(DrawModeRegisterOffsetType.SamplerArguments, value); }
            public ushort WaterVectorArguments_Count { get => GetCount(DrawModeRegisterOffsetType.WaterVectorArguments); set => SetCount(DrawModeRegisterOffsetType.WaterVectorArguments, value); }
            public ushort Unknown1_Count { get => GetCount(DrawModeRegisterOffsetType.Unknown1); set => SetCount(DrawModeRegisterOffsetType.Unknown1, value); }
            public ushort Unknown2_Count { get => GetCount(DrawModeRegisterOffsetType.Unknown2); set => SetCount(DrawModeRegisterOffsetType.Unknown2, value); }
            public ushort VectorArguments_Count { get => GetCount(DrawModeRegisterOffsetType.VectorArguments); set => SetCount(DrawModeRegisterOffsetType.VectorArguments, value); }
            public ushort IntegerArguments_Count { get => GetCount(DrawModeRegisterOffsetType.IntegerArguments); set => SetCount(DrawModeRegisterOffsetType.IntegerArguments, value); }
            public ushort GlobalArguments_Count { get => GetCount(DrawModeRegisterOffsetType.GlobalArguments); set => SetCount(DrawModeRegisterOffsetType.GlobalArguments, value); }
            public ushort RenderMethodExternArguments_Count { get => GetCount(DrawModeRegisterOffsetType.RenderMethodExternArguments); set => SetCount(DrawModeRegisterOffsetType.RenderMethodExternArguments, value); }
            public ushort Unknown4_Count { get => GetCount(DrawModeRegisterOffsetType.Unknown4); set => SetCount(DrawModeRegisterOffsetType.Unknown4, value); }
            public ushort Unknown5_Count { get => GetCount(DrawModeRegisterOffsetType.Unknown5); set => SetCount(DrawModeRegisterOffsetType.Unknown5, value); }
            public ushort DebugVectorRegisters_Count { get => GetCount(DrawModeRegisterOffsetType.DebugVectorRegisters); set => SetCount(DrawModeRegisterOffsetType.DebugVectorRegisters, value); }
            public ushort Unknown6_Count { get => GetCount(DrawModeRegisterOffsetType.Unknown6); set => SetCount(DrawModeRegisterOffsetType.Unknown6, value); }
            public ushort Unknown7_Count { get => GetCount(DrawModeRegisterOffsetType.Unknown7); set => SetCount(DrawModeRegisterOffsetType.Unknown7, value); }
            public ushort Unknown8_Count { get => GetCount(DrawModeRegisterOffsetType.Unknown8); set => SetCount(DrawModeRegisterOffsetType.Unknown8, value); }

            private ushort GetValue(DrawModeRegisterOffsetType offset) => (ushort)this.GetType().GetField($"RegisterMapping{(int)offset}").GetValue(this);
            private void SetValue(DrawModeRegisterOffsetType offset, ushort value) => this.GetType().GetField($"RegisterMapping{(int)offset}").SetValue(this, value);
            public ushort GetCount(DrawModeRegisterOffsetType offset) => (ushort)(GetValue(offset) >> 10);
            public ushort GetOffset(DrawModeRegisterOffsetType offset) => (ushort)(GetValue(offset) & 0x3FFu);
            public void SetCount(DrawModeRegisterOffsetType offset, ushort count)
            {
                if (count > 0x3Fu) throw new System.Exception("Out of range");
                var a = GetOffset(offset);
                var b = (count & 0x3F) << 10;
                var value = (ushort)(a | b);
                SetValue(offset, value);
            }
            public void SetOffset(DrawModeRegisterOffsetType offset, ushort _offset)
            {
                if (_offset > 0x3FFu) throw new System.Exception("Out of range");
                var a = (_offset & 0x3FF);
                var b = (GetCount(offset) & 0x3F) << 10;
                var value = (ushort)(a | b);
                SetValue(offset, value);
            }

            public ushort RegisterMapping0;
            public ushort RegisterMapping1;
            public ushort RegisterMapping2;
            public ushort RegisterMapping3;
            public ushort RegisterMapping4;
            public ushort RegisterMapping5;
            public ushort RegisterMapping6;
            public ushort RegisterMapping7;
            public ushort RegisterMapping8;
            public ushort RegisterMapping9;
            public ushort RegisterMapping10;
            public ushort RegisterMapping11;
            public ushort RegisterMapping12;
            public ushort RegisterMapping13;
        }

        /// <summary>
        /// Binds an argument in the render method tag to a pixel shader constant.
        /// </summary>
        [TagStructure(Size = 0x4)]
        public class ArgumentMapping
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
        public class ShaderArgument
        {
            public StringId Name;
        }
    }
}