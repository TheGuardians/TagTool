using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Direct3D.Functions;
using TagTool.ShaderGenerator.Types;
using TagTool.Util;

namespace TagTool.ShaderGenerator
{
    public class HalogramTemplateShaderGenerator : TemplateShaderGenerator
    {
        protected override string ShaderGeneratorType => "halogram_template";
        protected override List<DirectX.MacroDefine> TemplateDefinitions => new List<DirectX.MacroDefine>
        {
            new DirectX.MacroDefine {Name = "_debug_color", Definition = "float4(1, 0, 0, 0)" }
        };

        public HalogramTemplateShaderGenerator(GameCacheContext cacheContext, TemplateShaderGenerator.Drawmode drawmode, Int32[] args, Int32 arg_pos = 0) : base(
                drawmode,
                (Albedo)GetNextTemplateArg(args, ref arg_pos),
				(Self_Illumination)GetNextTemplateArg(args, ref arg_pos),
				(Blend_Mode)GetNextTemplateArg(args, ref arg_pos),
				(Misc)GetNextTemplateArg(args, ref arg_pos),
				(Warp)GetNextTemplateArg(args, ref arg_pos),
				(Overlay)GetNextTemplateArg(args, ref arg_pos),
				(Edge_Fade)GetNextTemplateArg(args, ref arg_pos))
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

		public enum Albedo
        {
            Default,
            Detail_Blend,
            Constant_Color,
            Two_Change_Color,
            Four_Change_Color,
            Three_Detail_Blend,
            Two_Detail_Overlay,
            Two_Detail,
            Color_Mask,
            Two_Detail_Black_Point
        }

        public enum Self_Illumination
        {
            Off,
            Simple,
            _3_Channel_Self_Illum,
            Plasma,
            From_Diffuse,
            Illum_Detail,
            Meter,
            Self_Illum_Times_Diffuse,
            Multilayer_Additive,
            ML_Add_Four_Change_Color,
            ML_Add_Five_Change_Color,
            Scope_Blur
        }

        public enum Blend_Mode
        {
            Opaque,
            Additive,
            Multiply,
            Alpha_Blend,
            Double_Multiply
        }

        public enum Misc
        {
            First_Person_Never,
            First_Person_Sometimes,
            First_Person_Always,
            First_Person_Never_With_rotating_Bitmaps
        }

        public enum Warp
        {
            None,
            From_Texture,
            Parallax_Simple
        }

        public enum Overlay
        {
            None,
            Additive,
            Additive_Detail,
            Multiply,
            Multiply_And_Additive_Detail
        }

        public enum Edge_Fade
        {
            None,
            Simple
        }

        #endregion
    }
}
