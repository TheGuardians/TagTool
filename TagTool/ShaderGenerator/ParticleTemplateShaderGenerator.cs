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
		static string ShaderFile { get; } = "ShaderGenerator/shader_code/particle_templates/particle_template.hlsl";

		public ParticleTemplateShaderGenerator(GameCacheContext cacheContext, Int32[] args, Int32 arg_pos = 0) : base(
				(Albedo)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Blend_Mode)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Specialized_Rendering)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Lighting)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Render_Targets)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Depth_Fade)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Black_Point)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Fog)(args.Length == arg_pos ? 0 : args[arg_pos++]))
		{
			this.CacheContext = cacheContext;
		}

		#region Implemented Features Check

		protected override MultiValueDictionary<Type, object> ImplementedEnums { get; set; } = new MultiValueDictionary<Type, object>
		{

		};

		#endregion

		#region TemplateShaderGenerator

		public override ShaderGeneratorResult Generate()
		{
#if DEBUG
			CheckImplementedParameters();
#endif

			var shader_parameters = GenerateShaderParameters(58, 0, 0);
			Dictionary<string, string> file_overrides = new Dictionary<string, string>()
			{
				{ "parameters.hlsl", GenerateUniformsFile(shader_parameters)}
			};

			List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();
			definitions.AddRange(GenerateFunctionDefinition());
			definitions.AddRange(GenerateCompilationFlagDefinitions());

			var compiler = new Util.DirectX();
			compiler.SetCompilerFileOverrides(file_overrides);
			var result = compiler.CompilePCShaderFromFile(
				ShaderFile,
				definitions.ToArray(),
				"main",
				"ps_3_0",
				0,
				0,
				out byte[] ShaderBytecode,
				out string ErrorMsgs
			);
			if (!result) throw new Exception(ErrorMsgs);

			new Disassemble(ShaderBytecode, out string disassembly);

			Console.WriteLine();
			Console.WriteLine(disassembly);
			Console.WriteLine();

			return new ShaderGeneratorResult { ByteCode = ShaderBytecode, Parameters = shader_parameters };
		}

		#endregion

		#region Uniforms/Registers

		protected override MultiValueDictionary<object, TemplateParameter> Uniforms { get; set; } = new MultiValueDictionary<object, TemplateParameter>
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
