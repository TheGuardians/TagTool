using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.ShaderGenerator.Types;
using TagTool.Shaders;

namespace TagTool.ShaderGenerator
{
    public class TemplateShaderGenerator
    {
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
