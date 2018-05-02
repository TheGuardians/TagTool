using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Direct3D.Functions;
using TagTool.ShaderGenerator.Types;
using TagTool.Shaders;
using TagTool.Util;

namespace TagTool.ShaderGenerator
{
    public class DecalTemplateShaderGenerator : TemplateShaderGenerator
    {
        protected override string ShaderGeneratorType => "decal_template";
        protected override List<DirectX.MacroDefine> TemplateDefinitions => new List<DirectX.MacroDefine>
        {
            new DirectX.MacroDefine {Name = "_debug_color", Definition = "float4(1, 0, 0, 0)" }
        };

        public DecalTemplateShaderGenerator(GameCacheContext cacheContext, TemplateShaderGenerator.Drawmode drawmode, Int32[] args, Int32 arg_pos = 0) : base(
                drawmode,
         (Albedo)GetNextTemplateArg(args, ref arg_pos),
		 (Blend_Mode)GetNextTemplateArg(args, ref arg_pos),
		 (Render_Pass)GetNextTemplateArg(args, ref arg_pos),
		 (Specular)GetNextTemplateArg(args, ref arg_pos),
		 (Bump_Mapping)GetNextTemplateArg(args, ref arg_pos),
		 (Tinting)GetNextTemplateArg(args, ref arg_pos))
		{
			this.CacheContext = cacheContext;
		}

        #region Implemented Features Check

        protected override MultiValueDictionary<Type, object> ImplementedEnums => new MultiValueDictionary<Type, object>
        {
            {typeof(Albedo), Albedo.DiffuseOnly },
            {typeof(Albedo), Albedo.Palettized_Plus_Alpha },
            {typeof(Albedo), Albedo.Diffuse_Plus_Alpha },
            {typeof(Albedo), Albedo.Change_Color },

            {typeof(Bump_Mapping), Bump_Mapping.Leave },
            {typeof(Bump_Mapping), Bump_Mapping.Standard },

            //{typeof(Blend_Mode), Blend_Mode.Opaque },
            //{typeof(Blend_Mode), Blend_Mode.Additive },
            //{typeof(Blend_Mode), Blend_Mode.Pre_Multiplied_Alpha },
            
            {typeof(Blend_Mode), Blend_Mode.Multiply },
            {typeof(Blend_Mode), Blend_Mode.Alpha_Blend },
            {typeof(Blend_Mode), Blend_Mode.Double_Multiply },

            {typeof(Render_Pass), Render_Pass.Pre_Lighting},

            {typeof(Tinting), Tinting.None },
            {typeof(Tinting), Tinting.Unmodulated },
            {typeof(Tinting), Tinting.Fully_Modulated },

            {typeof(Specular), Specular.Leave },
            {typeof(Specular), Specular.Modulate },
        };

        #endregion
        
        #region Uniforms/Registers

        protected override MultiValueDictionary<object, TemplateParameter> Uniforms => new MultiValueDictionary<object, TemplateParameter>
        {
            // These appear to be apart of some kind of global structure
            {0, new TemplateParameter(typeof(Int32), "g_exposure", ShaderParameter.RType.Vector) {SpecificOffset = 0 } },
            {0, new TemplateParameter(typeof(Int32), "g_alt_exposure", ShaderParameter.RType.Vector) {SpecificOffset = 12 } },
            {0, new TemplateParameter(typeof(Int32), "fade", ShaderParameter.RType.Vector) {SpecificOffset = 32 } },

            {Albedo.DiffuseOnly, new TemplateParameter(typeof(Albedo), "unknown_sampler0", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.DiffuseOnly, new TemplateParameter(typeof(Albedo), "unknown_sampler1", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.DiffuseOnly, new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },

            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "unknown_sampler0", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "unknown_sampler1", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "alpha_map", ShaderParameter.RType.Sampler) },
            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "palette", ShaderParameter.RType.Sampler) },

            {Albedo.Diffuse_Plus_Alpha, new TemplateParameter(typeof(Albedo), "unknown_sampler0", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Diffuse_Plus_Alpha, new TemplateParameter(typeof(Albedo), "unknown_sampler1", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Diffuse_Plus_Alpha, new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Diffuse_Plus_Alpha, new TemplateParameter(typeof(Albedo), "alpha_map", ShaderParameter.RType.Sampler) },

            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "unknown_sampler0", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "unknown_sampler1", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "change_color_map", ShaderParameter.RType.Sampler) },
            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "primary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "secondary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "tertiary_change_color", ShaderParameter.RType.Vector) },

            {Bump_Mapping.Standard, new TemplateParameter(typeof(Bump_Mapping), "bump_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Standard, new TemplateParameter(typeof(Bump_Mapping), "bump_map_xform", ShaderParameter.RType.Vector) },

            {Tinting.Unmodulated, new TemplateParameter(typeof(Tinting), "tint_color", ShaderParameter.RType.Vector) },
            {Tinting.Unmodulated, new TemplateParameter(typeof(Tinting), "intensity", ShaderParameter.RType.Vector) },

            {Tinting.Fully_Modulated, new TemplateParameter(typeof(Tinting), "tint_color", ShaderParameter.RType.Vector) },
            {Tinting.Fully_Modulated, new TemplateParameter(typeof(Tinting), "intensity", ShaderParameter.RType.Vector) },
        };

        #endregion

        #region Enums

        public Albedo albedo => (Albedo)EnumValues[0];
        public Blend_Mode blend_mode => (Blend_Mode)EnumValues[1];
        public Render_Pass render_pass => (Render_Pass)EnumValues[2];
        public Specular specular => (Specular)EnumValues[3];
        public Bump_Mapping bump_mapping => (Bump_Mapping)EnumValues[4];
        public Tinting tinting => (Tinting)EnumValues[5];

        public enum Albedo
        {
            DiffuseOnly,
            Palettized,
            Palettized_Plus_Alpha,
            Diffuse_Plus_Alpha,
            Emblem_Change_Color,
            Change_Color,
            Diffuse_Plus_Alpha_Mask,
            Palettized_Plus_Alpha_Mask,
            Vector_Alpha,
            Vector_Alpha_Drop_Shadow
        }

        public enum Blend_Mode
        {
            Opaque,
            Additive,
            Multiply,
            Alpha_Blend,
            Double_Multiply,
            Maximum,
            Multiply_Add,
            Add_Src_Times_DstAlpha,
            Add_Src_Times_SrcAlpha,
            Inv_Alpha_Blend,
            Pre_Multiplied_Alpha
        }

        public enum Render_Pass
        {
            Pre_Lighting,
            Post_Lighting
        }

        public enum Specular
        {
            Leave,
            Modulate
        }

        public enum Bump_Mapping
        {
            Leave,
            Standard,
            Standard_Mask
        }

        public enum Tinting
        {
            None,
            Unmodulated,
            Partially_Modulated,
            Fully_Modulated
        }

        #endregion
    }
}
