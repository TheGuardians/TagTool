using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.ShaderGenerator.Types;
using TagTool.Shaders;
using TagTool.Util;

namespace TagTool.ShaderGenerator
{
    public partial class ShaderGenerator
    {

        private static StringId GetOrCreateStringID(GameCacheContext cacheContext, string str_id)
        {
            var value = cacheContext.GetStringId(str_id);
            if (value == null || value == StringId.Null) value = cacheContext.StringIdCache.AddString(str_id);
            return value;
        }

        public static ShaderGeneratorResult GenerateSource(ShaderGeneratorParameters template_parameters, GameCacheContext cacheContext)
        {
#if DEBUG
            CheckImplementedParameters(template_parameters);
#endif


            var shader_parameters = GenerateShaderParameters(cacheContext, template_parameters);

            var uniforms_file = TemplateShaderGenerator.GenerateUniformsFile(shader_parameters, cacheContext);


            Dictionary<string, string> file_overrides = new Dictionary<string, string>();
            file_overrides["parameters.hlsl"] = uniforms_file;

            //TODO: Think about the easiest way to do this
            //var type_defs = GenerateEnumsDefinitions();
            var func_defs = GenerateFunctionDefinition(template_parameters);
            var flag_defs = GenerateCompilationFlagDefinitions(template_parameters);

            List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();
            definitions.AddRange(func_defs);
            definitions.AddRange(flag_defs);

            //var result = DirectXUtilities.CompilePCShaderFromFile(
            //    "ShaderGenerator/shader_code/shader_template.hlsl",
            //    value_defs.ToArray(),
            //    "main",
            //    "ps_3_0",
            //    0,
            //    out byte[] ShaderBytecode,
            //    out string ErrorMsgs,
            //    out string ConstantTable
            //    );
            //if(!result)
            //{
            //    throw new Exception(ErrorMsgs);
            //}

            byte[] compiled_shader;
            {
                string shader_file = "ShaderGenerator/shader_code/shader_template.hlsl";
                var entry_point = "main";
                var profile = "ps_3_0";

                //Include include = null;

                // ShaderFlags flags = ShaderFlags.Debug;
                //using (var stream = ShaderLoader.CompileShaderFromFile(shader_file, entry_point, include, profile, flags))
                //{

                //    stream.Seek(0, System.IO.SeekOrigin.Begin);
                //    compiled_shader = new byte[stream.Length];
                //    stream.Read(compiled_shader, 0, (int)stream.Length);
                //}

                var compiler = new Util.DirectX();

                var result = compiler.CompilePCShaderFromFile(
                    shader_file,
                    definitions.ToArray(),
                    entry_point,
                    profile,
                    0,
                    0,
                    out byte[] Shader,
                    out string ErrorMsgs);

                if (!result) throw new Exception(ErrorMsgs);
                compiled_shader = Shader;
            }

            var disassembly = ShaderCompiler.Disassemble(compiled_shader);

            Console.WriteLine();
            Console.WriteLine(disassembly);
            Console.WriteLine();

            //var shader_parameters = ReadShaderParamsFromDisassembly(disassembly, cacheContext);

            return new ShaderGeneratorResult { ByteCode = compiled_shader, Parameters = shader_parameters };
        }

        private static List<ShaderParameter> ReadShaderParamsFromDisassembly(string disassembly, GameCacheContext cacheContext)
        {
            List<ShaderParameter> shader_parameters = new List<ShaderParameter>();
            using (StringReader reader = new StringReader(disassembly))
            {
                bool found_registers_output = false;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim() == "ps_3_0" || line.Trim() == "vs_3_0") break;

                    if (!found_registers_output && line.Contains("Registers:"))
                    {
                        found_registers_output = true;
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        continue;
                    }
                    if (!found_registers_output) continue;

                    var args = line.Trim().Split(new string[] { "//", " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (args.Length != 3) break;

                    var name = args[0];
                    var register = args[1];
                    var register_index = ushort.Parse(register.Substring(1));
                    var size = byte.Parse(args[2]);

                    ShaderParameter parameter = new ShaderParameter
                    {
                        ParameterName = GetOrCreateStringID(cacheContext, name),
                        RegisterCount = size,
                        RegisterIndex = register_index,
                    };

                    switch (register[0])
                    {
                        case 'c':
                            parameter.RegisterType = ShaderParameter.RType.Vector;
                            break;
                        case 's':
                            parameter.RegisterType = ShaderParameter.RType.Sampler;
                            break;
                        case 'i':
                            parameter.RegisterType = ShaderParameter.RType.Integer;
                            break;
                        case 'b':
                            parameter.RegisterType = ShaderParameter.RType.Boolean;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    shader_parameters.Add(parameter);

                    Console.WriteLine($"{name}[{size}] {register}");
                }
            }
            return shader_parameters;
        }

        private static DirectX.MacroDefine GenerateEnumFuncDefinition(object value, string prefix = "")
        {
            Type _enum = value.GetType();
            var values = Enum.GetValues(_enum);

            return new DirectX.MacroDefine
            {
                Name = $"{_enum.Name}",
                Definition = $"{_enum.Name}_{value}".ToLower()
            };
        }

        private static DirectX.MacroDefine GenerateEnumFlagDefinition(object value, string prefix = "")
        {
            Type _enum = value.GetType();
            var values = Enum.GetValues(_enum);

            return new DirectX.MacroDefine
            {
                Name = $"flag_{ _enum.Name }_{ value }".ToLower(),
                Definition = "1"
            };
        }

        private static List<DirectX.MacroDefine> GenerateFunctionDefinition(ShaderGeneratorParameters _params)
        {
            List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();

            definitions.Add(GenerateEnumFuncDefinition(_params.albedo, "albedo"));
            definitions.Add(GenerateEnumFuncDefinition(_params.bump_mapping, "bump_mapping"));
            definitions.Add(GenerateEnumFuncDefinition(_params.alpha_test, "alpha_test"));
            definitions.Add(GenerateEnumFuncDefinition(_params.specular_mask, "specular_mask"));
            definitions.Add(GenerateEnumFuncDefinition(_params.material_model, "material_model"));
            definitions.Add(GenerateEnumFuncDefinition(_params.environment_mapping, "environment_mapping"));
            definitions.Add(GenerateEnumFuncDefinition(_params.self_illumination, "self_illumination"));
            definitions.Add(GenerateEnumFuncDefinition(_params.blend_mode, "blend_mode"));
            definitions.Add(GenerateEnumFuncDefinition(_params.parallax, "parallax"));
            definitions.Add(GenerateEnumFuncDefinition(_params.misc, "misc"));
            definitions.Add(GenerateEnumFuncDefinition(_params.distortion, "distortion"));
            definitions.Add(GenerateEnumFuncDefinition(_params.soft_fade, "soft_fade"));

            return definitions;
        }

        private static List<DirectX.MacroDefine> GenerateCompilationFlagDefinitions(ShaderGeneratorParameters _params)
        {
            List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();

            definitions.Add(GenerateEnumFlagDefinition(_params.albedo, "albedo"));
            definitions.Add(GenerateEnumFlagDefinition(_params.bump_mapping, "bump_mapping"));
            definitions.Add(GenerateEnumFlagDefinition(_params.alpha_test, "alpha_test"));
            definitions.Add(GenerateEnumFlagDefinition(_params.specular_mask, "specular_mask"));
            definitions.Add(GenerateEnumFlagDefinition(_params.material_model, "material_model"));
            definitions.Add(GenerateEnumFlagDefinition(_params.environment_mapping, "environment_mapping"));
            definitions.Add(GenerateEnumFlagDefinition(_params.self_illumination, "self_illumination"));
            definitions.Add(GenerateEnumFlagDefinition(_params.blend_mode, "blend_mode"));
            definitions.Add(GenerateEnumFlagDefinition(_params.parallax, "parallax"));
            definitions.Add(GenerateEnumFlagDefinition(_params.misc, "misc"));
            definitions.Add(GenerateEnumFlagDefinition(_params.distortion, "distortion"));
            definitions.Add(GenerateEnumFlagDefinition(_params.soft_fade, "soft_fade"));

            return definitions;
        }

        private static MultiValueDictionary<Type, object> ImplementedEnums = new MultiValueDictionary<Type, object>
        {
            {typeof(ShaderTemplateShaderGenerator.Albedo), ShaderTemplateShaderGenerator.Albedo.Default },
            {typeof(ShaderTemplateShaderGenerator.Albedo), ShaderTemplateShaderGenerator.Albedo.Detail_Blend },
            {typeof(ShaderTemplateShaderGenerator.Albedo), ShaderTemplateShaderGenerator.Albedo.Constant_Color },
            {typeof(ShaderTemplateShaderGenerator.Albedo), ShaderTemplateShaderGenerator.Albedo.Two_Change_Color },
            {typeof(ShaderTemplateShaderGenerator.Albedo), ShaderTemplateShaderGenerator.Albedo.Four_Change_Color },
            {typeof(ShaderTemplateShaderGenerator.Albedo), ShaderTemplateShaderGenerator.Albedo.Two_Detail_Overlay },
            {typeof(ShaderTemplateShaderGenerator.Albedo), ShaderTemplateShaderGenerator.Albedo.Three_Detail_Blend },
            {typeof(ShaderTemplateShaderGenerator.Bump_Mapping), ShaderTemplateShaderGenerator.Bump_Mapping.Standard },
            {typeof(ShaderTemplateShaderGenerator.Bump_Mapping), ShaderTemplateShaderGenerator.Bump_Mapping.Detail },
            {typeof(ShaderTemplateShaderGenerator.Bump_Mapping), ShaderTemplateShaderGenerator.Bump_Mapping.Off },
            {typeof(ShaderTemplateShaderGenerator.Blend_Mode), ShaderTemplateShaderGenerator.Blend_Mode.Opaque },
        };

        private static void CheckImplementedParameters(params object[] values)
        {
            foreach (var value in values)
            {
                if (ImplementedEnums.ContainsKey(value.GetType()))
                    if (ImplementedEnums[value.GetType()].Contains(value)) return;
                Console.WriteLine($"{value.GetType().Name} has not implemented {value}");
            }
        }

        private static void CheckImplementedParameters(ShaderGeneratorParameters _params)
        {
            CheckImplementedParameters(
            _params.albedo,
            _params.bump_mapping,
            _params.alpha_test,
            _params.specular_mask,
            _params.material_model,
            _params.environment_mapping,
            _params.self_illumination,
            _params.blend_mode,
            _params.parallax,
            _params.misc,
            _params.distortion,
            _params.soft_fade);
        }





        public class ShaderGeneratorParameters
        {
            public ShaderTemplateShaderGenerator.Albedo albedo;
            public ShaderTemplateShaderGenerator.Bump_Mapping bump_mapping;
            public ShaderTemplateShaderGenerator.Alpha_Test alpha_test;
            public ShaderTemplateShaderGenerator.Specular_Mask specular_mask;
            public ShaderTemplateShaderGenerator.Material_Model material_model;
            public ShaderTemplateShaderGenerator.Environment_Mapping environment_mapping;
            public ShaderTemplateShaderGenerator.Self_Illumination self_illumination;
            public ShaderTemplateShaderGenerator.Blend_Mode blend_mode;
            public ShaderTemplateShaderGenerator.Parallax parallax;
            public ShaderTemplateShaderGenerator.Misc misc;
            public ShaderTemplateShaderGenerator.Distortion distortion;
            public ShaderTemplateShaderGenerator.Soft_Fade soft_fade;
        }

    }
}
