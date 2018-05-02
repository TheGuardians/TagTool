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
    public class ParticleTemplateShaderGenerator : TemplateShaderGenerator
    {
        protected override string ShaderGeneratorType => "particle_template";
        protected override List<DirectX.MacroDefine> TemplateDefinitions => new List<DirectX.MacroDefine>
        {
            new DirectX.MacroDefine {Name = "_debug_color", Definition = "float4(1, 0, 0, 0)" }
        };

        public ParticleTemplateShaderGenerator(GameCacheContext cacheContext, TemplateShaderGenerator.Drawmode drawmode, Int32[] args, Int32 arg_pos = 0) : base(
                drawmode,
                (Albedo)GetNextTemplateArg(args, ref arg_pos),
				(Blend_Mode)GetNextTemplateArg(args, ref arg_pos),
				(Specialized_Rendering)GetNextTemplateArg(args, ref arg_pos),
				(Lighting)GetNextTemplateArg(args, ref arg_pos),
				(Render_Targets)GetNextTemplateArg(args, ref arg_pos),
				(Depth_Fade)GetNextTemplateArg(args, ref arg_pos),
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

		public enum Albedo
        {
            Diffuse_Only,
            Diffuse_Plus_Billboard_Alpha,
            Palettized,
            Palettized_Plus_Billboard_Alpha,
            Diffuse_Plus_Sprite_Alpha,
            Palettized_Plus_Sprite_Alpha
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

        public enum Specialized_Rendering
        {
            None,
            Distortion,
            Distortion_Expensive,
            Distortion_Diffuse,
            Distortion_Expensive_Diffuse
        }

        public enum Lighting
        {
            None,
            Per_Vertex_Ravi_Order_3,
            Per_Vertex_Ravi_Order_0
        }

        public enum Render_Targets
        {
            LDR_and_HDR,
            LDR_Only
        }

        public enum Depth_Fade
        {
            Off,
            On
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

        public enum Frame_Blend
        {
            Off,
            On
        }

        public enum Self_Illumination
        {
            None,
            Constant_Color
        }

        #endregion
    }
}
