using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using HaloShaderGenerator.Shader;

namespace TagTool.Commands.Shaders
{
    public class GenerateShaderCommand : Command
    {
        GameCache Cache;

        public GenerateShaderCommand(GameCache cache) :
            base(true,

                "GenerateShader",
                "Generates a shader template and a relevant \'rm  \' tag if specified",

                "GenerateShader <shader type> <options>",

                "Generates a shader template\n" +
                "<shader type> - Specify shader type, EX. \"shader\" for \'rmsh\'\n" +
                "<options> - Specify the template\'s options as either integers or by names")
        {
            Cache = cache;
        }

        static readonly List<string> SupportedShaderTypes = new List<string> { "shader", "particle", /*"black"*/ };

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
                return new TagToolError(CommandError.ArgCount);

            string shaderType = args[0].ToLower();
            if (!SupportedShaderTypes.Contains(shaderType))
                return new TagToolError(CommandError.CustomMessage, $"Shader type \"{shaderType}\" is unsupported");

            args.RemoveAt(0); // we should only have options from this point

            // get relevant rmdf
            if (!Cache.TagCache.TryGetTag($"shaders\\{shaderType}.rmdf", out var rmdfTag))
                return new TagToolError(CommandError.TagInvalid, $"Could not find rmdf tag for \"{shaderType}\"");

            using (var stream = Cache.OpenCacheReadWrite())
            {
                var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

                if (rmdf.GlobalVertexShader == null || rmdf.GlobalPixelShader == null)
                    return new TagToolError(CommandError.TagInvalid, "A global shader was missing from rmdf");

                List<int> options = new List<int>();
                for (int i = 0; i < args.Count; i++)
                {
                    // parse options as int, if fails try finding in rmdf string
                    if (!int.TryParse(args[i].ToLower(), out int optionInteger))
                    {
                        bool found = false;

                        for (int j = 0; j < rmdf.Methods[i].ShaderOptions.Count; j++)
                        {
                            if (Cache.StringTable.GetString(rmdf.Methods[i].ShaderOptions[j].Type) == args[i].ToLower())
                            {
                                found = true;
                                options.Add(j);
                            }
                        }

                        if (!found)
                            return new TagToolError(CommandError.ArgInvalid, $"Shader option \"{args[i]}\" not found");
                    }

                    else
                    {
                        options.Add(optionInteger);
                    }
                }

                // make up options count, may not work very well
                while (options.Count != rmdf.Methods.Count)
                    options.Add(0);

                // build tagname
                string rmt2TagName = $"shaders\\{shaderType}_templates\\_{string.Join("_", options.ToArray())}";

                // generate shader
                switch (shaderType)
                {
                    case "shader": GenerateShader(stream, options, rmt2TagName, rmdf); break;
                    case "particle": GenerateParticle(stream, options, rmt2TagName, rmdf); break;
                        //case "black": GenerateShaderBlack(stream, rmt2TagName, rmdf); break;
                }

                Console.WriteLine($"Generated shader template \"{rmt2TagName}\"");
            }

            return true;
        }

        private void GenerateShader(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            Albedo albedo = (Albedo)options[0];
            Bump_Mapping bumpMapping = (Bump_Mapping)options[1];
            Alpha_Test alphaTest = (Alpha_Test)options[2];
            Specular_Mask specularMask = (Specular_Mask)options[3];
            Material_Model materialModel = (Material_Model)options[4];
            Environment_Mapping environmentMapping = (Environment_Mapping)options[5];
            Self_Illumination selfIllumination = (Self_Illumination)options[6];
            Blend_Mode blendMode = (Blend_Mode)options[7];
            Parallax parallax = (Parallax)options[8];
            Misc misc = (Misc)options[9];
            Distortion distortion = (Distortion)options[10];

            var generator = new ShaderGenerator(albedo, bumpMapping, alphaTest, specularMask, materialModel, environmentMapping, selfIllumination, blendMode, parallax, misc, distortion);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateParticle(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            HaloShaderGenerator.Particle.Albedo albedo = (HaloShaderGenerator.Particle.Albedo)options[0];
            HaloShaderGenerator.Particle.Blend_Mode blend_mode = (HaloShaderGenerator.Particle.Blend_Mode)options[1];
            HaloShaderGenerator.Particle.Specialized_Rendering specialized_rendering = (HaloShaderGenerator.Particle.Specialized_Rendering)options[2];
            HaloShaderGenerator.Particle.Lighting lighting = (HaloShaderGenerator.Particle.Lighting)options[3];
            HaloShaderGenerator.Particle.Render_Targets render_targets = (HaloShaderGenerator.Particle.Render_Targets)options[4];
            HaloShaderGenerator.Particle.Depth_Fade depth_fade = (HaloShaderGenerator.Particle.Depth_Fade)options[5];
            HaloShaderGenerator.Particle.Black_Point black_point = (HaloShaderGenerator.Particle.Black_Point)options[6];
            HaloShaderGenerator.Particle.Fog fog = (HaloShaderGenerator.Particle.Fog)options[7];
            HaloShaderGenerator.Particle.Frame_Blend frame_blend = (HaloShaderGenerator.Particle.Frame_Blend)options[8];
            HaloShaderGenerator.Particle.Self_Illumination self_Illumination = (HaloShaderGenerator.Particle.Self_Illumination)options[9];

            var generator = new HaloShaderGenerator.Particle.ParticleGenerator(albedo, blend_mode, specialized_rendering, lighting, render_targets, depth_fade, black_point, fog, frame_blend, self_Illumination, true);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, Cache.TagCache.GetTag("shaders\\particle_shared_pixel_shaders.glps"));
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, Cache.TagCache.GetTag("shaders\\particle_shared_vertex_shaders.glvs"));

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }
    }
}
