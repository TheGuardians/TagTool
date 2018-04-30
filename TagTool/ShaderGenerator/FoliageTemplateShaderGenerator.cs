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
    public class FoliageTemplateShaderGenerator : TemplateShaderGenerator
    {
		static string ShaderFile { get; } = "ShaderGenerator/shader_code/foliage_templates/foliage_template.hlsl";

		public FoliageTemplateShaderGenerator(GameCacheContext cacheContext, Int32[] args, Int32 arg_pos = 0) : base(
				(Albedo)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Alpha_Test)(args.Length == arg_pos ? 0 : args[arg_pos++]),
				(Material_Model)(args.Length == arg_pos ? 0 : args[arg_pos++]))
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
            Default
        }

        public enum Alpha_Test
        {
            None,
            Simple
        }

        public enum Material_Model
        {
            Default
        }

        #endregion
    }
}
