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
	public class ContrailTemplateShaderGenerator : TemplateShaderGenerator
    {
        protected override string ShaderGeneratorType => "contrail_template";
        protected override List<DirectX.MacroDefine> TemplateDefinitions => new List<DirectX.MacroDefine>
        {
            new DirectX.MacroDefine {Name = "_debug_color", Definition = "float4(1, 0, 0, 0)" }
        };

        public ContrailTemplateShaderGenerator(GameCacheContext cacheContext, TemplateShaderGenerator.Drawmode drawmode, Int32[] args, Int32 arg_pos = 0) : base(
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

		};

		#endregion

		#region Uniforms/Registers

		protected override MultiValueDictionary<object, TemplateParameter> Uniforms => new MultiValueDictionary<object, TemplateParameter>
		{

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
