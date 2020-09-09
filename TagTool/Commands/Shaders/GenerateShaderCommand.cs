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
                "Generates a shader template",

                "GenerateShader <shader type> <options>",

                "Generates a shader template\n" +
                "<shader type> - Specify shader type, EX. \"shader\" for \'rmsh\'\n" +
                "<options> - Specify the template\'s options as either integers or by names")
        {
            Cache = cache;
        }

        static readonly List<string> SupportedShaderTypes = new List<string> { "shader", "particle", "contrail", "beam", "light_volume", "decal", "black", "halogram" };

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
                    case "shader":          GenerateShader(stream, options, rmt2TagName, rmdf); break;
                    case "particle":        GenerateParticle(stream, options, rmt2TagName, rmdf); break;
                    case "contrail":        GenerateContrail(stream, options, rmt2TagName, rmdf); break;
                    case "beam":            GenerateBeam(stream, options, rmt2TagName, rmdf); break;
                    case "light_volume":    GenerateLightVolume(stream, options, rmt2TagName, rmdf); break;
                    case "decal":           GenerateDecal(stream, options, rmt2TagName, rmdf); break;
                    case "black":           GenerateShaderBlack(stream, rmt2TagName, rmdf); break;
                    case "halogram":        GenerateHalogram(stream, options, rmt2TagName, rmdf); break;

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

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateContrail(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            HaloShaderGenerator.Contrail.Albedo albedo = (HaloShaderGenerator.Contrail.Albedo)options[0];
            HaloShaderGenerator.Contrail.Blend_Mode blend_mode = (HaloShaderGenerator.Contrail.Blend_Mode)options[1];
            HaloShaderGenerator.Contrail.Black_Point black_point = (HaloShaderGenerator.Contrail.Black_Point)options[2];
            HaloShaderGenerator.Contrail.Fog fog = (HaloShaderGenerator.Contrail.Fog)options[3];

            var generator = new HaloShaderGenerator.Contrail.ContrailGenerator(albedo, blend_mode, black_point, fog, true);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateBeam(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            HaloShaderGenerator.Beam.Albedo albedo = (HaloShaderGenerator.Beam.Albedo)options[0];
            HaloShaderGenerator.Beam.Blend_Mode blend_mode = (HaloShaderGenerator.Beam.Blend_Mode)options[1];
            HaloShaderGenerator.Beam.Black_Point black_point = (HaloShaderGenerator.Beam.Black_Point)options[2];
            HaloShaderGenerator.Beam.Fog fog = (HaloShaderGenerator.Beam.Fog)options[3];

            var generator = new HaloShaderGenerator.Beam.BeamGenerator(albedo, blend_mode, black_point, fog, true);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateLightVolume(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            HaloShaderGenerator.LightVolume.Albedo albedo = (HaloShaderGenerator.LightVolume.Albedo)options[0];
            HaloShaderGenerator.LightVolume.Blend_Mode blend_mode = (HaloShaderGenerator.LightVolume.Blend_Mode)options[1];
            HaloShaderGenerator.LightVolume.Fog fog = (HaloShaderGenerator.LightVolume.Fog)options[2];

            var generator = new HaloShaderGenerator.LightVolume.LightVolumeGenerator(albedo, blend_mode, fog, true);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateDecal(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            HaloShaderGenerator.Decal.Albedo albedo = (HaloShaderGenerator.Decal.Albedo)options[0];
            HaloShaderGenerator.Decal.Blend_Mode blend_mode = (HaloShaderGenerator.Decal.Blend_Mode)options[1];
            HaloShaderGenerator.Decal.Render_Pass render_pass = (HaloShaderGenerator.Decal.Render_Pass)options[2];
            HaloShaderGenerator.Decal.Specular specular = (HaloShaderGenerator.Decal.Specular)options[3];
            HaloShaderGenerator.Decal.Bump_Mapping bump_mapping = (HaloShaderGenerator.Decal.Bump_Mapping)options[4];
            HaloShaderGenerator.Decal.Tinting tinting = (HaloShaderGenerator.Decal.Tinting)options[5];

            var generator = new HaloShaderGenerator.Decal.DecalGenerator(albedo, blend_mode, render_pass, specular, bump_mapping, tinting, true);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateShaderBlack(Stream stream, string rmt2Name, RenderMethodDefinition rmdf)
        {
            var generator = new HaloShaderGenerator.Black.ShaderBlackGenerator();

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateHalogram(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            HaloShaderGenerator.Halogram.Albedo albedo = (HaloShaderGenerator.Halogram.Albedo)options[0];
            HaloShaderGenerator.Halogram.Self_Illumination self_illumination = (HaloShaderGenerator.Halogram.Self_Illumination)options[1];
            HaloShaderGenerator.Halogram.Blend_Mode blend_mode = (HaloShaderGenerator.Halogram.Blend_Mode)options[2];
            HaloShaderGenerator.Halogram.Misc misc = (HaloShaderGenerator.Halogram.Misc)options[3];
            HaloShaderGenerator.Halogram.Warp warp = (HaloShaderGenerator.Halogram.Warp)options[4];
            HaloShaderGenerator.Halogram.Overlay overlay = (HaloShaderGenerator.Halogram.Overlay)options[5];
            HaloShaderGenerator.Halogram.Edge_Fade edge_fade = (HaloShaderGenerator.Halogram.Edge_Fade)options[6];

            var generator = new HaloShaderGenerator.Halogram.HalogramGenerator(albedo, self_illumination, blend_mode, misc, warp, overlay, edge_fade, true);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }
    }
}
