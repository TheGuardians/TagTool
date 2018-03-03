using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.ShaderGenerator.Types;
using TagTool.Shaders;
using TagTool.Util;

namespace TagTool.ShaderGenerator
{
    public abstract class TemplateShaderGenerator : IShaderGenerator
    {
        public GameCacheContext CacheContext { get; internal set; }
        
        public abstract ShaderGeneratorResult Generate();
        public abstract List<TemplateParameter> GetShaderParametersList(object key);

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

        protected List<ShaderParameter> GenerateShaderParameters(params Type[] types)
        {
            List<IEnumerable<TemplateParameter>> parameter_lists = new List<IEnumerable<TemplateParameter>>();
            foreach (var type in types)
            {
                var value = this.GetType().GetField(type.Name.ToLower()).GetValue(this);
                if (value == null) throw new Exception("Value should never be null");
                var _params = GetShaderParametersList(value);
                parameter_lists.Add(_params);
            }

            IndicesManager indices = new IndicesManager();
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
                if (param.Type != target_type) continue;
                switch (param.Type)
                {
                    case ShaderParameter.RType.Vector:
                        if (param.enabled)
                            parameters.Add(new ShaderParameter()
                            {
                                ParameterName = cacheContext.GetStringId(param.Name),
                                RegisterCount = 1,
                                RegisterType = ShaderParameter.RType.Vector,
                                RegisterIndex = (ushort)indices.vector_index
                            });
                        indices.vector_index++;
                        break;
                    case ShaderParameter.RType.Sampler:
                        if (param.enabled)
                            parameters.Add(new ShaderParameter()
                            {
                                ParameterName = cacheContext.GetStringId(param.Name),
                                RegisterCount = 1,
                                RegisterType = ShaderParameter.RType.Sampler,
                                RegisterIndex = (ushort)indices.sampler_index
                            });
                        indices.sampler_index++;

                        break;
                    case ShaderParameter.RType.Boolean:
                        if (param.enabled)
                            parameters.Add(new ShaderParameter()
                            {
                                ParameterName = cacheContext.GetStringId(param.Name),
                                RegisterCount = 1,
                                RegisterType = ShaderParameter.RType.Boolean,
                                RegisterIndex = (ushort)indices.boolean_index
                            });
                        indices.boolean_index++;
                        break;
                    case ShaderParameter.RType.Integer:
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return parameters;
        }
    }
}
