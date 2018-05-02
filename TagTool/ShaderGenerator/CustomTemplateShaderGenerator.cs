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
	public class CustomTemplateShaderGenerator : TemplateShaderGenerator
    {
        protected override string ShaderGeneratorType => "custom_template";
        protected override List<DirectX.MacroDefine> TemplateDefinitions => new List<DirectX.MacroDefine>
        {
            new DirectX.MacroDefine {Name = "_debug_color", Definition = "float4(1, 0, 0, 0)" }
        };

        public CustomTemplateShaderGenerator(GameCacheContext cacheContext, TemplateShaderGenerator.Drawmode drawmode, Int32[] args, Int32 arg_pos = 0) : base(
                drawmode,
                (Albedo)GetNextTemplateArg(args, ref arg_pos),
				(Bump_Mapping)GetNextTemplateArg(args, ref arg_pos),
				(Alpha_Test)GetNextTemplateArg(args, ref arg_pos),
				(Specular_Mask)GetNextTemplateArg(args, ref arg_pos),
				(Material_Model)GetNextTemplateArg(args, ref arg_pos),
				(Environment_Mapping)GetNextTemplateArg(args, ref arg_pos),
				(Self_Illumination)GetNextTemplateArg(args, ref arg_pos),
				(Blend_Mode)GetNextTemplateArg(args, ref arg_pos),
				(Parallax)GetNextTemplateArg(args, ref arg_pos),
				(Misc)GetNextTemplateArg(args, ref arg_pos))
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
		public Bump_Mapping bump_mapping => (Bump_Mapping)EnumValues[1];
		public Alpha_Test alpha_test => (Alpha_Test)EnumValues[2];
		public Specular_Mask specular_mask => (Specular_Mask)EnumValues[3];
		public Material_Model material_model => (Material_Model)EnumValues[4];
		public Environment_Mapping environment_mapping => (Environment_Mapping)EnumValues[5];
		public Self_Illumination self_illumination => (Self_Illumination)EnumValues[6];
		public Blend_Mode blend_mode => (Blend_Mode)EnumValues[7];
		public Parallax parallax => (Parallax)EnumValues[8];
		public Misc misc => (Misc)EnumValues[9];

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
			Two_Detail_Black_Point,
			Waterfall,
			Multiply_Map
		}

		public enum Bump_Mapping
		{
			Off,
			Standard,
			Detail
		}

		public enum Alpha_Test
		{
			None,
			Simple,
			Multiply_Map
		}

		public enum Specular_Mask
		{
			No_Specular_Mask,
			Specular_Mask_From_Diffuse,
			Specular_Mask_From_Texture
		}

		public enum Material_Model
		{
			Diffuse_Only,
			Two_Lobe_Phong,
			Foliage,
			None
		}

		public enum Environment_Mapping
		{
			None,
			Per_Pixel,
			Dynamic,
			From_Flat_Texture
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
			Self_Illum_Times_Diffuse
		}

		public enum Blend_Mode
		{
			Opaque,
			Additive,
			Multiply,
			Alpha_Blend,
			Double_Multiply
		}

		public enum Parallax
		{
			Off,
			Simple,
			Interpolated,
			Simple_Detail
		}

		public enum Misc
		{
			First_Person_Never,
			First_Person_Sometimes,
			First_Person_Always,
			First_Person_Never_W_Rotating_Bitmaps
		}

		#endregion
	}
}
