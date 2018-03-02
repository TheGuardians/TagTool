using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Shaders;
using TagTool.Utilities;

namespace TagTool.ShaderGenerator
{
    public partial class ShaderGenerator
    {
        private static Dictionary<Type, List<object>> ImplementedEnums = new Dictionary<Type, List<object>>
        {
            {typeof(Albedo), new List<object> { Albedo.Default } },
            {typeof(Bump_Mapping), new List<object>{ Bump_Mapping.Standard} },
            {typeof(Bump_Mapping), new List<object>{ Bump_Mapping.Detail} },
            {typeof(Blend_Mode), new List<object>{ Blend_Mode.Opaque} }
        };

        public class ShaderGenerator_Result
        {
            public byte[] ByteCode;
            public List<ShaderParameter> Parameters;
        }

        private static StringId GetOrCreateStringID(GameCacheContext cacheContext, string str_id)
        {
            var value = cacheContext.GetStringId(str_id);
            if (value == null || value == StringId.Null) value = cacheContext.StringIdCache.AddString(str_id);
            return value;
        }

        public static string ShaderParameter_ToString(ShaderParameter param, GameCacheContext cacheContext)
        {
            if(param.RegisterCount == 1)
            {
                switch (param.RegisterType)
                {
                    case ShaderParameter.RType.Boolean:
                        return $"uniform bool {cacheContext.GetString(param.ParameterName)} : register(b{param.RegisterIndex});";
                    case ShaderParameter.RType.Integer:
                        return $"uniform int {cacheContext.GetString(param.ParameterName)} : register(i{param.RegisterIndex});";
                    case ShaderParameter.RType.Vector:
                        return $"uniform float4 {cacheContext.GetString(param.ParameterName)} : register(c{param.RegisterIndex});";
                    case ShaderParameter.RType.Sampler:
                        return $"uniform sampler {cacheContext.GetString(param.ParameterName)} : register(s{param.RegisterIndex});";
                    default:
                        throw new NotImplementedException();
                }
            } else
            {
                throw new NotImplementedException();
            }
        }

        public static string GenerateUniformsFile(List<ShaderParameter> parameters, GameCacheContext cacheContext)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("#ifndef __UNIFORMS");
            sb.AppendLine("#define __UNIFORMS");
            sb.AppendLine();

            foreach(var param in parameters)
            {
                sb.AppendLine(ShaderParameter_ToString(param, cacheContext));
            }

            sb.AppendLine();
            sb.AppendLine("#endif");

            return sb.ToString();
        }

        public static ShaderGenerator_Result GenerateSource(ShaderType type, ShaderGeneratorParameters template_parameters, GameCacheContext cacheContext)
        {
#if DEBUG
            CheckImplementedParameters(template_parameters);
#endif
            var shader_parameters = GenerateShaderParameters(cacheContext, template_parameters);

            var uniforms_file = GenerateUniformsFile(shader_parameters, cacheContext);
            Dictionary<string, string> file_overrides = new Dictionary<string, string>();
            file_overrides["parameters.hlsl"] = uniforms_file;

            //TODO: Think about the easiest way to do this
            //var type_defs = GenerateEnumsDefinitions();
            var func_defs = GenerateFunctionDefinition(template_parameters);
            var flag_defs = GenerateCompilationFlagDefinitions(template_parameters);

            List<DirectXUtilities.MacroDefine> definitions = new List<DirectXUtilities.MacroDefine>();
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
                string shader_file = "";
                switch (type)
                {
                    case ShaderType.DecalsTemplate:
                        shader_file = "ShaderGenerator/shader_code/decals_template.hlsl";
                        break;
                    default:
                        shader_file = "ShaderGenerator/shader_code/shader_template.hlsl";
                        break;
                }


                var entry_point = "main";
                //Include include = null;
                var profile = "ps_3_0";
                // ShaderFlags flags = ShaderFlags.Debug;
                //using (var stream = ShaderLoader.CompileShaderFromFile(shader_file, entry_point, include, profile, flags))
                //{

                //    stream.Seek(0, System.IO.SeekOrigin.Begin);
                //    compiled_shader = new byte[stream.Length];
                //    stream.Read(compiled_shader, 0, (int)stream.Length);
                //}

                var result = Utilities.DirectXUtilities.CompilePCShaderFromFile(
                    shader_file,
                    definitions.ToArray(),
                    entry_point,
                    profile,
                    0,
                    0,
                    out byte[] Shader,
                    out string ErrorMsgs,
                    file_overrides);

                if (!result) throw new Exception(ErrorMsgs);
                compiled_shader = Shader;
            }

            var disassembly = ShaderCompiler.Disassemble(compiled_shader);

            Console.WriteLine();
            Console.WriteLine(disassembly);
            Console.WriteLine();

            //var shader_parameters = ReadShaderParamsFromDisassembly(disassembly, cacheContext);

            return new ShaderGenerator_Result { ByteCode = compiled_shader, Parameters = shader_parameters };
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

        private static IEnumerable<DirectXUtilities.MacroDefine> GenerateEnumDefinitions(Type _enum)
        {
            List<DirectXUtilities.MacroDefine> definitions = new List<DirectXUtilities.MacroDefine>();

            var values = Enum.GetValues(_enum);

            foreach (var value in values)
            {
                definitions.Add(new DirectXUtilities.MacroDefine
                {
                    Name = $"{_enum.Name}_{value}",
                    Definition = Convert.ChangeType(value, Enum.GetUnderlyingType(_enum)).ToString()
                });
            }

            return definitions;
        }

        private static DirectXUtilities.MacroDefine GenerateEnumFuncDefinition(object value, string prefix = "")
        {
            Type _enum = value.GetType();
            var values = Enum.GetValues(_enum);

            return new DirectXUtilities.MacroDefine
            {
                Name = $"{_enum.Name}",
                Definition = $"{_enum.Name}_{value}".ToLower()
            };
        }

        private static DirectXUtilities.MacroDefine GenerateEnumFlagDefinition(object value, string prefix = "")
        {
            Type _enum = value.GetType();
            var values = Enum.GetValues(_enum);

            return new DirectXUtilities.MacroDefine
            {
                Name = $"flag_{ _enum.Name }_{ value }".ToLower(),
                Definition = "1"
            };
        }

        private static List<DirectXUtilities.MacroDefine> GenerateFunctionDefinition(ShaderGeneratorParameters _params)
        {
            List<DirectXUtilities.MacroDefine> definitions = new List<DirectXUtilities.MacroDefine>();

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

        private static List<DirectXUtilities.MacroDefine> GenerateCompilationFlagDefinitions(ShaderGeneratorParameters _params)
        {
            List<DirectXUtilities.MacroDefine> definitions = new List<DirectXUtilities.MacroDefine>();

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

        private static void CheckImplementedParameters(object value)
        {
            if (ImplementedEnums.ContainsKey(value.GetType()))
            {
                var list = ImplementedEnums[value.GetType()];
                if (list.Contains(value)) return;
            }
            var message = $"{value.GetType().Name} has not implemented {value}";
            //throw new NotImplementedException(message);]
            Console.WriteLine(message);
        }

        private static void CheckImplementedParameters(ShaderGeneratorParameters _params)
        {
            CheckImplementedParameters(_params.albedo);
            CheckImplementedParameters(_params.bump_mapping);
            CheckImplementedParameters(_params.alpha_test);
            CheckImplementedParameters(_params.specular_mask);
            CheckImplementedParameters(_params.material_model);
            CheckImplementedParameters(_params.environment_mapping);
            CheckImplementedParameters(_params.self_illumination);
            CheckImplementedParameters(_params.blend_mode);
            CheckImplementedParameters(_params.parallax);
            CheckImplementedParameters(_params.misc);
            CheckImplementedParameters(_params.distortion);
            CheckImplementedParameters(_params.soft_fade);
        }

        public static IEnumerable<DirectXUtilities.MacroDefine> GenerateEnumsDefinitions()
        {
            var defs_Albedo = GenerateEnumDefinitions(typeof(Albedo));
            var defs_Bump_Mapping = GenerateEnumDefinitions(typeof(Bump_Mapping));
            var defs_Alpha_Test = GenerateEnumDefinitions(typeof(Alpha_Test));
            var defs_Specular_Mask = GenerateEnumDefinitions(typeof(Specular_Mask));
            var defs_Material_Model = GenerateEnumDefinitions(typeof(Material_Model));
            var defs_Environment_Mapping = GenerateEnumDefinitions(typeof(Environment_Mapping));
            var defs_Self_Illumination = GenerateEnumDefinitions(typeof(Self_Illumination));
            var defs_Blend_Mode = GenerateEnumDefinitions(typeof(Blend_Mode));
            var defs_Parallax = GenerateEnumDefinitions(typeof(Parallax));
            var defs_Misc = GenerateEnumDefinitions(typeof(Misc));
            var defs_Distortion = GenerateEnumDefinitions(typeof(Distortion));
            var defs_Soft_fade = GenerateEnumDefinitions(typeof(Soft_Fade));
            var defs = (new List<DirectXUtilities.MacroDefine> { })
                .Concat(defs_Albedo)
                .Concat(defs_Bump_Mapping)
                .Concat(defs_Alpha_Test)
                .Concat(defs_Specular_Mask)
                .Concat(defs_Material_Model)
                .Concat(defs_Environment_Mapping)
                .Concat(defs_Self_Illumination)
                .Concat(defs_Blend_Mode)
                .Concat(defs_Parallax)
                .Concat(defs_Misc)
                .Concat(defs_Distortion)
                .Concat(defs_Soft_fade);
            return defs;
        }

        public class ShaderGeneratorParameters
        {
            public Albedo albedo;
            public Bump_Mapping bump_mapping;
            public Alpha_Test alpha_test;
            public Specular_Mask specular_mask;
            public Material_Model material_model;
            public Environment_Mapping environment_mapping;
            public Self_Illumination self_illumination;
            public Blend_Mode blend_mode;
            public Parallax parallax;
            public Misc misc;
            public Distortion distortion;
            public Soft_Fade soft_fade;
        }

    }
}
