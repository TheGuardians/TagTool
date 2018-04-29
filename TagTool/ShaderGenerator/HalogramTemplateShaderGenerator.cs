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
		static string ShaderFile { get; } = "ShaderGenerator/shader_code/beam_template.hlsl";

		public HalogramTemplateShaderGenerator(GameCacheContext cacheContext, Int32[] args, Int32 arg_pos = 0) : base(
				(Albedo)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Self_Illumination)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Blend_Mode)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Misc)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Warp)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Overlay)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Edge_Fade)(args.Length == arg_pos ? 0 : args[arg_pos++]))
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

		protected override MultiValueDictionary<object, object> Uniforms { get; set; } = new MultiValueDictionary<object, object>
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
