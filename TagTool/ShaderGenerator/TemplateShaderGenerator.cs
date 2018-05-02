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
    public abstract class TemplateShaderGenerator : IShaderGenerator
    {
        public enum Drawmode
        {
            Default,
            Albedo,
            Static_Default,
            Static_Per_Pixel,
            Static_Per_Vertex,
            Static_Sh,
            Static_Prt_Ambient,
            Static_Prt_Linear,
            Static_Prt_Quadratic,
            Dynamic_Light,
            Shadow_Generate,
            Shadow_Apply,
            Active_Camo,
            Lightmap_Debug_Mode,
            Static_Per_Vertex_Color,
            Water_Tessellation,
            Water_Shading,
            Dynamic_Light_Cinematic,
            Z_Only,
            Sfx_Distort
        }
        protected Drawmode drawMode { get; set; }

        protected TemplateShaderGenerator(Drawmode drawmode, params object[] enums)
        {
            this.drawMode = drawmode;
            this.EnumValues = enums;
            this.EnumTypes = new Type[enums.Length];
            for (var i = 0; i < enums.Length; i++)
            {
                EnumTypes[i] = enums[i].GetType();
            }
        }

        protected static int GetNextTemplateArg(Int32[] args, ref Int32 arg_pos)
        {
            return arg_pos >= args.Length ? 0 : args[arg_pos++];
        }

        public GameCacheContext CacheContext { get; internal set; }

        protected virtual string ShaderFile { get; } = "ShaderGenerator/shader_code/entry_points.hlsl";
        abstract protected string ShaderGeneratorType { get; }

        protected object[] EnumValues { get; }
        protected Type[] EnumTypes { get; }

        abstract protected List<DirectX.MacroDefine> TemplateDefinitions { get; }
        abstract protected MultiValueDictionary<Type, object> ImplementedEnums { get; }
        abstract protected MultiValueDictionary<object, TemplateParameter> Uniforms { get; }

        public ShaderGeneratorResult Generate()
        {

#if DEBUG
            CheckImplementedParameters();
#endif

            var shader_parameters = GenerateShaderParameters(58, 0, 0);
            var parameters_hlsl = GenerateUniformsFile(shader_parameters);
            Dictionary<string, string> file_overrides = new Dictionary<string, string>()
            {
                { "parameters.hlsl", parameters_hlsl}
            };

            List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();
            definitions.AddRange(GenerateFunctionDefinition());
            definitions.AddRange(GenerateCompilationFlagDefinitions());
            definitions.AddRange(GenerateParameterDefinitions(shader_parameters));
            definitions.Add(new DirectX.MacroDefine { Name = ShaderGeneratorType, Definition="1" });
            definitions.AddRange(TemplateDefinitions);

            var entrypoint = $"{Enum.GetName(drawMode.GetType(), drawMode).ToLower()}_ps";
            definitions.Add(new DirectX.MacroDefine { Name = $"entry_{entrypoint}", Definition = "1" });

            var compiler = new Util.DirectX();
            compiler.SetCompilerFileOverrides(file_overrides);
            var result = compiler.CompilePCShaderFromFile(
                ShaderFile,
                definitions.ToArray(),
                entrypoint,
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

        public List<TemplateParameter> GetShaderParametersList(object key)
        {
            Type type = key.GetType();
            if (Uniforms.ContainsKey((object)key))
            {
                var list = Uniforms[(object)key].Cast<TemplateParameter>().ToList();
                list = list.Where(param => param.Target_Type == type).ToList();
                return list;
            }
            return new List<TemplateParameter>();
        }

        public List<TemplateParameter> GetGenericShaderParametersList()
        {
            List<TemplateParameter> list = new List<TemplateParameter>();
            if (Uniforms.ContainsKey(0))
            {
                var null_list = Uniforms[0].Cast<TemplateParameter>().ToList();
                null_list = null_list.Where(param => param.Target_Type == typeof(Int32)).ToList();
                list.AddRange(null_list);
            }
            return list;
        }

        private static IEnumerable<DirectX.MacroDefine> GenerateEnumDefinitions(Type _enum)
        {
            List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();

            var values = Enum.GetValues(_enum);

            foreach (var value in values)
            {
                definitions.Add(new DirectX.MacroDefine
                {
                    Name = $"{_enum.Name}_{value}",
                    Definition = Convert.ChangeType(value, Enum.GetUnderlyingType(_enum)).ToString()
                });
            }

            return definitions;
        }

        public static string ShaderParameter_ToString(ShaderParameter param, GameCacheContext cacheContext)
        {
            if (param.RegisterCount == 1)
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
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public string GenerateUniformsFile(List<ShaderParameter> parameters)
        {
            return GenerateUniformsFile(parameters, CacheContext);
        }

        protected static DirectX.MacroDefine GenerateEnumFuncDefinition(object value)
        {
            Type _enum = value.GetType();
            var values = Enum.GetValues(_enum);

            return new DirectX.MacroDefine
            {
                Name = $"{_enum.Name}",
                Definition = $"{_enum.Name}_{value}".ToLower()
            };
        }

        protected static DirectX.MacroDefine GenerateEnumFlagDefinition(object value)
        {
            Type _enum = value.GetType();
            var values = Enum.GetValues(_enum);

            return new DirectX.MacroDefine
            {
                Name = $"flag_{ _enum.Name }_{ value }".ToLower(),
                Definition = "1"
            };
        }

        public static string GenerateUniformsFile(List<ShaderParameter> parameters, GameCacheContext cacheContext)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("#ifndef __UNIFORMS");
            sb.AppendLine("#define __UNIFORMS");
            sb.AppendLine();

            foreach (var param in parameters)
            {
                sb.AppendLine(ShaderParameter_ToString(param, cacheContext));
            }

            sb.AppendLine();
            sb.AppendLine("#endif");

            return sb.ToString();
        }

        protected static IEnumerable<DirectX.MacroDefine> GenerateHLSLEnumDefinitions(params Type[] types)
        {
            var defs = new List<DirectX.MacroDefine>();
            foreach (var type in types)
                defs.AddRange(GenerateEnumDefinitions(type));
            return defs;
        }

        protected List<ShaderParameter> GenerateShaderParameters(int vector_start, int sampler_start, int boolean_start, params Type[] types)
        {
            List<IEnumerable<TemplateParameter>> parameter_lists = new List<IEnumerable<TemplateParameter>>();

            var generic_params = GetGenericShaderParametersList();
            parameter_lists.Add(generic_params);

            foreach (var type in types)
            {
                var this_type = this.GetType();
                var value = this_type.GetProperty(type.Name.ToLower()).GetValue(this);
                if (value == null) throw new Exception("Value should never be null");
                var _params = GetShaderParametersList(value);
                parameter_lists.Add(_params);
            }

            IndicesManager indices = new IndicesManager { vector_index = vector_start, sampler_index = sampler_start, boolean_index = boolean_start };
            List<ShaderParameter> parameters = new List<ShaderParameter>();

            foreach (var _params in parameter_lists)
            {
                var vector_params = GenerateShaderParameters(ShaderParameter.RType.Vector, CacheContext, _params, indices);
                var sampler_params = GenerateShaderParameters(ShaderParameter.RType.Sampler, CacheContext, _params, indices);
                var boolean_params = GenerateShaderParameters(ShaderParameter.RType.Boolean, CacheContext, _params, indices);

                parameters.AddRange(vector_params);
                parameters.AddRange(sampler_params);
                parameters.AddRange(boolean_params);
            }

            return parameters;
        }

        public static List<ShaderParameter> GenerateShaderParameters(
            ShaderParameter.RType target_type,
            GameCacheContext cacheContext,
            IEnumerable<TemplateParameter> _params,
            IndicesManager indices)
        {
            List<ShaderParameter> parameters = new List<ShaderParameter>();

            foreach (var param in _params)
            {
                if (param.Type != target_type)
                    continue;

                switch (param.Type)
                {
                    case ShaderParameter.RType.Vector:
                        var vector_index = param.SpecificOffset == -1 ? indices.vector_index++ : param.SpecificOffset;
                        if (param.Enabled)
                            parameters.Add(new ShaderParameter()
                            {
                                ParameterName = cacheContext.GetStringId(param.Name),
                                RegisterCount = 1,
                                RegisterType = ShaderParameter.RType.Vector,
                                RegisterIndex = (ushort)vector_index
                            });
                        break;

                    case ShaderParameter.RType.Sampler:
                        var sampler_index = param.SpecificOffset == -1 ? indices.sampler_index++ : param.SpecificOffset;
                        if (param.Enabled)
                            parameters.Add(new ShaderParameter()
                            {
                                ParameterName = cacheContext.GetStringId(param.Name),
                                RegisterCount = 1,
                                RegisterType = ShaderParameter.RType.Sampler,
                                RegisterIndex = (ushort)sampler_index
                            });
                        break;
                    case ShaderParameter.RType.Boolean:
                        var boolean_index = param.SpecificOffset == -1 ? indices.boolean_index++ : param.SpecificOffset;
                        if (param.Enabled)
                            parameters.Add(new ShaderParameter()
                            {
                                ParameterName = cacheContext.GetStringId(param.Name),
                                RegisterCount = 1,
                                RegisterType = ShaderParameter.RType.Boolean,
                                RegisterIndex = (ushort)boolean_index
                            });
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
            return parameters;
        }

        #region Implemented Features Check

        protected bool CheckImplementedParameters(params object[] values)
        {
            bool supported = true;

            foreach (var value in values)
            {
                if (ImplementedEnums.ContainsKey(value.GetType()))
                    if (ImplementedEnums[value.GetType()].Contains(value)) continue;
                Console.WriteLine($"{value.GetType().Name} has not implemented {value}");
                supported = false;
            }

            return supported;
        }

        public bool CheckImplementedParameters()
        {
            return CheckImplementedParameters(EnumValues);
        }

        #endregion

        #region HLSL Generation

        protected List<DirectX.MacroDefine> GenerateFunctionDefinition()
        {
            List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();

            foreach (var _enum in EnumValues)
            {
                definitions.Add(GenerateEnumFuncDefinition(_enum));
            }

            return definitions;
        }

        protected List<DirectX.MacroDefine> GenerateCompilationFlagDefinitions()
        {
            List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();

            foreach (var _enum in EnumValues)
            {
                definitions.Add(GenerateEnumFlagDefinition(_enum));
            }

            return definitions;
        }

        protected List<DirectX.MacroDefine> GenerateParameterDefinitions(List<ShaderParameter> parameters)
        {
            var definitions = new List<DirectX.MacroDefine>();

            foreach (var param in parameters)
            {
                definitions.Add(new DirectX.MacroDefine { Name = $"param_{CacheContext.GetString(param.ParameterName).ToLower()}", Definition="1" });
            }

            return definitions;
        }

        protected List<ShaderParameter> GenerateShaderParameters(int vector_start, int sampler_start, int boolean_start)
        {
            return GenerateShaderParameters(vector_start, sampler_start, boolean_start, EnumTypes);
        }

        protected IEnumerable<DirectX.MacroDefine> GenerateHLSLEnumDefinitions()
        {
            return GenerateHLSLEnumDefinitions(EnumTypes);
        }

        #endregion
    }
}
