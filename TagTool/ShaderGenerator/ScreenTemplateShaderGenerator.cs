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
		static string ShaderFile { get; } = "ShaderGenerator/shader_code/screen_templates/screen_template.hlsl";

		public ScreenTemplateShaderGenerator(GameCacheContext cacheContext, Int32[] args, Int32 arg_pos = 0) : base(
			(Warp)(args.Length == arg_pos ? 0 : args[arg_pos++]),
			(Base)(args.Length == arg_pos ? 0 : args[arg_pos++]),
			(Overlay_A)(args.Length == arg_pos ? 0 : args[arg_pos++]),
			(Overlay_B)(args.Length == arg_pos ? 0 : args[arg_pos++]),
			(Blend_Mode)(args.Length == arg_pos ? 0 : args[arg_pos++]))
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

		public Warp warp => (Warp)EnumValues[0];
		public Base _base => (Base)EnumValues[1];
		public Overlay_A overlay_a => (Overlay_A)EnumValues[2];
		public Overlay_B overlay_b => (Overlay_B)EnumValues[3];
		public Blend_Mode blend_mode => (Blend_Mode)EnumValues[4];

		public enum Warp
		{
			None,
			Pixel_Space,
			Screen_Space
		}

		public enum Base
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
