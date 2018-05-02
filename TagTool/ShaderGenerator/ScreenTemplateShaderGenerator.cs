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
	public class ScreenTemplateShaderGenerator : TemplateShaderGenerator
    {
        protected override string ShaderGeneratorType => "screen_template";
        protected override List<DirectX.MacroDefine> TemplateDefinitions => new List<DirectX.MacroDefine>
        {
            new DirectX.MacroDefine {Name = "_debug_color", Definition = "float4(1, 0, 0, 0)" }
        };

        public ScreenTemplateShaderGenerator(GameCacheContext cacheContext, TemplateShaderGenerator.Drawmode drawmode, Int32[] args, Int32 arg_pos = 0) : base(
                drawmode,
            (Warp)GetNextTemplateArg(args, ref arg_pos),
			(_Base)GetNextTemplateArg(args, ref arg_pos),
			(Overlay_A)GetNextTemplateArg(args, ref arg_pos),
			(Overlay_B)GetNextTemplateArg(args, ref arg_pos),
			(Blend_Mode)GetNextTemplateArg(args, ref arg_pos))
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

		public Warp warp => (Warp)EnumValues[0];
		public _Base _base => (_Base)EnumValues[1];
		public Overlay_A overlay_a => (Overlay_A)EnumValues[2];
		public Overlay_B overlay_b => (Overlay_B)EnumValues[3];
		public Blend_Mode blend_mode => (Blend_Mode)EnumValues[4];

		public enum Warp
		{
			None,
			Pixel_Space,
			Screen_Space
		}

		public enum _Base
		{
			Single_Screen_Space,
			Single_Pixel_Space
		}

		public enum Overlay_A
		{
			None,
			Tint_Add_Color,
			Detail_Screen_Space,
			Detail_Pixel_Space,
			Detail_Masked_Screen_Space
		}

		public enum Overlay_B
		{
			None,
			Tint_Add_Color
		}

		public enum Blend_Mode
		{
			Opaque,
			Additive,
			Multiply,
			Alpha_Blend,
			Double_Multiply,
			Pre_Multiplied_Alpha
		}

		#endregion
	}
}
