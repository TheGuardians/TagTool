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
    public class BeamTemplateShaderGenerator : TemplateShaderGenerator
    {
        protected override string ShaderGeneratorType => "beam_template";
        protected override List<DirectX.MacroDefine> TemplateDefinitions => new List<DirectX.MacroDefine>
        {
            new DirectX.MacroDefine {Name = "_debug_color", Definition = "float4(1, 0, 0, 0)" }
        };

        public BeamTemplateShaderGenerator(GameCacheContext cacheContext, TemplateShaderGenerator.Drawmode drawmode, Int32[] args, Int32 arg_pos = 0) : base(
                drawmode,
                (Albedo)GetNextTemplateArg(args, ref arg_pos),
                (Blend_Mode)GetNextTemplateArg(args, ref arg_pos),
                (Black_Point)GetNextTemplateArg(args, ref arg_pos),
                (Fog)GetNextTemplateArg(args, ref arg_pos))
        {
            this.CacheContext = cacheContext;
        }

        #region Implemented Features Check

        protected override MultiValueDictionary<Type, object> ImplementedEnums => new MultiValueDictionary<Type, object>
        {
            {typeof(Albedo), Albedo.DiffuseOnly },
            {typeof(Albedo), Albedo.Palettized_Plus_Alpha },
            {typeof(Black_Point), Black_Point.Off },
            {typeof(Blend_Mode), Blend_Mode.Opaque },
            {typeof(Blend_Mode), Blend_Mode.Add_Src_Times_DstAlpha },
            {typeof(Blend_Mode), Blend_Mode.Add_Src_Times_SrcAlpha },
            {typeof(Blend_Mode), Blend_Mode.Pre_Multiplied_Alpha },
        };

        #endregion

        #region Uniforms/Registers

        protected override MultiValueDictionary<object, TemplateParameter> Uniforms => new MultiValueDictionary<object, TemplateParameter>
        {
            {0, new TemplateParameter(typeof(Int32), "g_exposure", ShaderParameter.RType.Vector) { SpecificOffset = 0 } },

            {Albedo.DiffuseOnly, new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },

            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "palette", ShaderParameter.RType.Sampler) },
            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "alpha_map", ShaderParameter.RType.Sampler) },
        };

        #endregion

        #region Enums

        public Albedo albedo => (Albedo)EnumValues[0];
        public Blend_Mode blend_mode => (Blend_Mode)EnumValues[1];
        public Black_Point black_point => (Black_Point)EnumValues[2];
        public Fog fog => (Fog)EnumValues[3];

        public enum Albedo
        {
            DiffuseOnly,
            Palettized,
            Palettized_Plus_Alpha
        }

        public enum Blend_Mode
        {
            Opaque,
            Additive,
            Multiply,
            Alpha_Mlend,
            Double_Multiply,
            Maximum,
            Multiply_Add,
            Add_Src_Times_DstAlpha,
            Add_Src_Times_SrcAlpha,
            Inv_Alpha_Blend,
            Pre_Multiplied_Alpha
        }

        public enum Black_Point
        {
            Off,
            On
        }

        public enum Fog
        {
            Off,
            On
        }

        #endregion
    }
}
