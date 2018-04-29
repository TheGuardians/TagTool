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
    public class TerrainTemplateShaderGenerator : TemplateShaderGenerator
    {
        static string ShaderFile { get; } = "ShaderGenerator/shader_code/decals_template.hlsl";

        public TerrainTemplateShaderGenerator( GameCacheContext cacheContext, List<string> args, int arg_pos = 0) : base(
                (Blending)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]),
                (Environment_Map)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]),
                (Material_0)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]),
                (Material_1)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]),
                (Material_2)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]),
                (Material_3)Int32.Parse(args.Count == arg_pos ? "0" : args[arg_pos++]))
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

        public Blending blending => (Blending)EnumValues[0];
        public Environment_Map environment_map => (Environment_Map)EnumValues[1];
        public Material_0 material_0 => (Material_0)EnumValues[2];
        public Material_1 material_1 => (Material_1)EnumValues[3];
        public Material_2 material_2 => (Material_2)EnumValues[4];
        public Material_3 material_3 => (Material_3)EnumValues[5];

        public enum Blending
        {
            Morph,
            Dynamic_Morph
        }

        public enum Environment_Map
        {
            None,
            Per_Pixel,
            Dynamic
        }

        public enum Material_0
        {
            Diffuse_Only,
            Diffuse_Plus_Specular,
            Off
        }

        public enum Material_1
        {
            Diffuse_Only,
            Diffuse_Plus_Specular,
            Off
        }

        public enum Material_2
        {
            Diffuse_Only,
            Diffuse_Plus_Specular,
            Off
        }

        public enum Material_3
        {
            Off,
            Diffuse_Only, // four_material_shaders_disable_detail_bump
            Diffuse_Plus_Specular, //(four_material_shaders_disable_detail_bump),
        }

        #endregion
    }
}
