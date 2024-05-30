using System.Collections.Generic;

namespace TagTool.Commands.Porting.Gen2
{
    public static class Gen2ShaderTemplateMappings
    {
        public static readonly Dictionary<string, Gen2ShaderTemplateMapBase> Templates;

        static Gen2ShaderTemplateMappings()
        {
            Templates = new Dictionary<string, Gen2ShaderTemplateMapBase>
            {
                {
                    "tex_bump_alpha_test", new Gen2ShaderTemplateMapBase("shaders\\shader_templates\\opaque\\tex_bump_alpha_test")
                    {
                        Type = ShaderType.Shader,
                        Bitmaps =
                        {
                            (Platform.Vista, 1, "base_map"),
                            (Platform.Vista, 2, "detail_map"),

                            (Platform.Xbox, 1, "xbox_base_map"),
                            (Platform.Xbox, 2, "xbox_detail_map")
                        },
                        PixelConstants = 
                        {
                            (Platform.All, 0, "normal_specular_tint"),
                            (Platform.All, 1, "glancing_specular_tint")
                        },
                        VertexConstants =
                        {
                            (Platform.Vista, 0, "bump_map"),
                            (Platform.Vista, 1, "base_map"),
                            (Platform.Vista, 2, "detail_map"),
                            (Platform.Vista, 4, "alpha_test_map"),

                            (Platform.Xbox, 0, "detail_map"),
                            (Platform.Xbox, 1, "base_map"),
                            (Platform.Xbox, 2, "bump_map"),
                            (Platform.Xbox, 4, "alpha_test_map")
                        },
                    }
                },
                {
                    "tex_bump_detail", new Gen2ShaderTemplateMapBase("shaders\\shader_templates\\transparent\\tex_bump_detail")
                    {
                        // Define properties for this template...
                    }
                }
            };
        }

        public static Gen2ShaderTemplateMapBase GetTemplate(string name)
        {
            if (Templates.TryGetValue(name, out var template))
            {
                return template;
            }
            else
            {
                throw new KeyNotFoundException($"No shader template found with name: {name}");
            }
        }
    }
}