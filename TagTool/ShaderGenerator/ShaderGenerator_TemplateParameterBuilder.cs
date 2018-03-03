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
    public partial class ShaderGenerator
    {

        static List<TemplateParameter> GetShaderParametersList(object key, Type type)
        {
            if (ShaderTemplateShaderGenerator.Uniforms.ContainsKey((object)key))
            {
                var list = ShaderTemplateShaderGenerator.Uniforms[(object)key].Cast<TemplateParameter>().ToList();
                list = list.Where(param => param.Target_Type == type).ToList();
                return list;
            }
            return new List<TemplateParameter>();
        }

        static List<ShaderParameter> GenerateShaderParameters(GameCacheContext cacheContext, ShaderGeneratorParameters template_parameters)
        {
            var params_Albedo = GetShaderParametersList(template_parameters.albedo, typeof(ShaderTemplateShaderGenerator.Albedo));
            var params_Bump_Mapping = GetShaderParametersList(template_parameters.bump_mapping, typeof(ShaderTemplateShaderGenerator.Bump_Mapping));
            var params_Alpha_Test = GetShaderParametersList(template_parameters.alpha_test, typeof(ShaderTemplateShaderGenerator.Alpha_Test));
            var params_Specular_Mask = GetShaderParametersList(template_parameters.specular_mask, typeof(ShaderTemplateShaderGenerator.Specular_Mask));
            var params_Material_Model = GetShaderParametersList(template_parameters.material_model, typeof(ShaderTemplateShaderGenerator.Material_Model));
            var params_Environment_Mapping = GetShaderParametersList(template_parameters.environment_mapping, typeof(ShaderTemplateShaderGenerator.Environment_Mapping));
            var params_Self_Illumination = GetShaderParametersList(template_parameters.self_illumination, typeof(ShaderTemplateShaderGenerator.Self_Illumination));
            var params_Blend_Mode = GetShaderParametersList(template_parameters.blend_mode, typeof(ShaderTemplateShaderGenerator.Blend_Mode));
            var params_Parallax = GetShaderParametersList(template_parameters.parallax, typeof(ShaderTemplateShaderGenerator.Parallax));
            var params_Misc = GetShaderParametersList(template_parameters.misc, typeof(ShaderTemplateShaderGenerator.Misc));
            var params_Distortion = GetShaderParametersList(template_parameters.distortion, typeof(ShaderTemplateShaderGenerator.Distortion));
            var params_Soft_fade = GetShaderParametersList(template_parameters.soft_fade, typeof(ShaderTemplateShaderGenerator.Soft_Fade));

            List<List<TemplateParameter>> parameter_lists = new List<List<TemplateParameter>>
            {
                params_Albedo,
                params_Bump_Mapping,
                params_Alpha_Test,
                params_Specular_Mask,
                params_Material_Model,
                params_Environment_Mapping,
                params_Self_Illumination,
                params_Blend_Mode,
                params_Parallax,
                params_Misc,
                params_Distortion,
                params_Soft_fade
            };

            IndicesManager indices = new IndicesManager();
            List<ShaderParameter> parameters = new List<ShaderParameter>();


            foreach (var _params in parameter_lists)
            {
                var vector_params = TemplateShaderGenerator.GenerateShaderParameters(ShaderParameter.RType.Vector, cacheContext, _params, indices);
                var sampler_params = TemplateShaderGenerator.GenerateShaderParameters(ShaderParameter.RType.Sampler, cacheContext, _params, indices);
                var boolean_params = TemplateShaderGenerator.GenerateShaderParameters(ShaderParameter.RType.Boolean, cacheContext, _params, indices);

                parameters.AddRange(vector_params);
                parameters.AddRange(sampler_params);
                parameters.AddRange(boolean_params);
            }

            return parameters;
        }

    };
}
