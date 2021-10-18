using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Shaders;
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
                "<shader type> - Specify shader type, EX. \"shader\" for \'rmsh\'.\n" +
                "Use \"explicit\" for explicit shaders, and \"glvs\" or \"glps\" for global shaders.\n" +
                "<options> - Specify the template\'s options as either integers or by names.\n" +
                "For explicit shaders, you should specify the name or the rasg shader index.")
        {
            Cache = cache;
        }

        static readonly List<string> UnsupportedShaderTypes = new List<string> { "glass", "cortana" };

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
                return new TagToolError(CommandError.ArgCount);

            string shaderType = args[0].ToLower();

            if (shaderType == "explicit")
                return GenerateExplicitShader(args[1].ToLower(), args.Count > 2 ? args[2].ToLower() : "default", args.Count > 3 ? args[3].ToLower() : "");
            else if (shaderType == "chud")
                return GenerateChudShader(args[1].ToLower());
            else if (shaderType == "glvs" || shaderType == "glps")
                return GenerateGlobalShader(args[1].ToLower(), shaderType == "glps");

            if (UnsupportedShaderTypes.Contains(shaderType))
                return new TagToolError(CommandError.CustomMessage, $"Shader type \"{shaderType}\" is unsupported");

            args.RemoveAt(0); // we should only have options from this point

            using (var stream = Cache.OpenCacheReadWrite())
            {
                // get relevant rmdf
                if (!Cache.TagCache.TryGetTag($"shaders\\{shaderType}.rmdf", out CachedTag rmdfTag))
                {
                    // don't need actual options yet - we just need to initialize the generator (input option indices are verified using rmdf, which we don't have yet)
                    List<byte> fakeOptions = new List<byte>();
                    for (int i = 0; i < args.Count; i++)
                        fakeOptions.Add(0);

                    rmdfTag = GenerateRmdf(stream, shaderType, fakeOptions.ToArray());
                    if (rmdfTag == null)
                        return new TagToolError(CommandError.TagInvalid, $"Could not find or generate rmdf tag for \"{shaderType}\"");
                }

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

                        for (int j = 0; j < rmdf.Categories[i].ShaderOptions.Count; j++)
                        {
                            if (Cache.StringTable.GetString(rmdf.Categories[i].ShaderOptions[j].Name) == args[i].ToLower())
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
                while (options.Count != rmdf.Categories.Count)
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
                    case "custom":          GenerateCustom(stream, options, rmt2TagName, rmdf); break;
                    case "screen":          GenerateScreen(stream, options, rmt2TagName, rmdf); break;
                    case "zonly":           GenerateZOnly(stream, options, rmt2TagName, rmdf); break;
                    case "water":           GenerateWater(stream, options, rmt2TagName, rmdf); break;
                    case "terrain":         GenerateTerrain(stream, options, rmt2TagName, rmdf); break;
                    case "foliage":         GenerateFoliage(stream, options, rmt2TagName, rmdf); break;
                }

                Console.WriteLine($"Generated shader template \"{rmt2TagName}\"");
            }

            return true;
        }

        HaloShaderGenerator.Generator.IShaderGenerator GetShaderGenerator(string shaderType, byte[] options, bool applyFixes = false)
        {
            switch (shaderType)
            {
                case "beam":            return new HaloShaderGenerator.Beam.BeamGenerator(options, applyFixes);
                case "black":           return new HaloShaderGenerator.Black.ShaderBlackGenerator();
                case "contrail":        return new HaloShaderGenerator.Contrail.ContrailGenerator(options, applyFixes);
                //case "cortana":         return new HaloShaderGenerator.Cortana.CortanaGenerator(options, applyFixes);
                case "custom":          return new HaloShaderGenerator.Custom.CustomGenerator(options, applyFixes);
                case "decal":           return new HaloShaderGenerator.Decal.DecalGenerator(options, applyFixes);
                case "foliage":         return new HaloShaderGenerator.Foliage.FoliageGenerator(options, applyFixes);
                //case "glass":           return new HaloShaderGenerator.Glass.GlassGenerator(options, applyFixes);
                case "halogram":        return new HaloShaderGenerator.Halogram.HalogramGenerator(options, applyFixes);
                case "light_volume":    return new HaloShaderGenerator.LightVolume.LightVolumeGenerator(options, applyFixes);
                case "particle":        return new HaloShaderGenerator.Particle.ParticleGenerator(options, applyFixes);
                case "screen":          return new HaloShaderGenerator.Screen.ScreenGenerator(options, applyFixes);
                case "shader":          return new HaloShaderGenerator.Shader.ShaderGenerator(options, applyFixes);
                case "terrain":         return new HaloShaderGenerator.Terrain.TerrainGenerator(options, applyFixes);
                case "water":           return new HaloShaderGenerator.Water.WaterGenerator(options, applyFixes);
                case "zonly":           return new HaloShaderGenerator.ZOnly.ZOnlyGenerator(options, applyFixes);
            }
            return null;
        }

        HaloShaderGenerator.Generator.IShaderGenerator GetGlobalShaderGenerator(string shaderType, bool applyFixes = false)
        {
            switch (shaderType)
            {
                case "beam":            return new HaloShaderGenerator.Beam.BeamGenerator(applyFixes);
                case "black":           return new HaloShaderGenerator.Black.ShaderBlackGenerator();
                case "contrail":        return new HaloShaderGenerator.Contrail.ContrailGenerator(applyFixes);
                //case "cortana":         return new HaloShaderGenerator.Cortana.CortanaGenerator(applyFixes);
                case "custom":          return new HaloShaderGenerator.Custom.CustomGenerator(applyFixes);
                case "decal":           return new HaloShaderGenerator.Decal.DecalGenerator(applyFixes);
                case "foliage":         return new HaloShaderGenerator.Foliage.FoliageGenerator(applyFixes);
                //case "glass":           return new HaloShaderGenerator.Glass.GlassGenerator(applyFixes);
                case "halogram":        return new HaloShaderGenerator.Halogram.HalogramGenerator(applyFixes);
                case "light_volume":    return new HaloShaderGenerator.LightVolume.LightVolumeGenerator(applyFixes);
                case "particle":        return new HaloShaderGenerator.Particle.ParticleGenerator(applyFixes);
                case "screen":          return new HaloShaderGenerator.Screen.ScreenGenerator(applyFixes);
                case "shader":          return new HaloShaderGenerator.Shader.ShaderGenerator(applyFixes);
                case "terrain":         return new HaloShaderGenerator.Terrain.TerrainGenerator(applyFixes);
                case "water":           return new HaloShaderGenerator.Water.WaterGenerator(applyFixes);
                case "zonly":           return new HaloShaderGenerator.ZOnly.ZOnlyGenerator(applyFixes);
            }
            return null;
        }

        private CachedTag GenerateRmdf(Stream stream, string shaderType, byte[] options)
        {
            var generator = GetShaderGenerator(shaderType, options, true);
            if (generator == null)
                return null;

            //var rmdf = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodDefinition(Cache, stream, generator, shaderType, out _, out _);
            var rmdf = TagTool.Shaders.ShaderGenerator.RenderMethodDefinitionGenerator.GenerateRenderMethodDefinition(Cache, stream, generator, shaderType, out _, out _);
            CachedTag rmdfTag = Cache.TagCache.AllocateTag<RenderMethodDefinition>($"shaders\\{shaderType}");
            Cache.Serialize(stream, rmdfTag, rmdf);
            if (Cache is GameCacheHaloOnlineBase)
                (Cache as GameCacheHaloOnlineBase).SaveTagNames();
            return rmdfTag;
        }

        public ShaderConstantTable BuildConstantTable(HaloShaderGenerator.ShaderGeneratorResult generatorResult, GameCache cache, bool pixelShader)
        {
            ShaderConstantTable result = new ShaderConstantTable
            {
                ShaderType = pixelShader ? ShaderType.PixelShader : ShaderType.VertexShader,
                Constants = new List<ShaderParameter>()
            };

            foreach (var register in generatorResult.Registers)
            {
                var nameId = cache.StringTable.GetStringId(register.Name);
                if (nameId == TagTool.Common.StringId.Invalid)
                    nameId = cache.StringTable.AddString(register.Name);

                ShaderParameter.RType rType = (ShaderParameter.RType)Enum.Parse(typeof(ShaderParameter.RType), register.registerType.ToString());

                var parameterBlock = new ShaderParameter { ParameterName = nameId, RegisterCount = (byte)register.Size, RegisterIndex = (ushort)register.Register, RegisterType = rType };
                result.Constants.Add(parameterBlock);
            }

            return result;
        }

        private object GenerateExplicitShader(string shader, string entry, string vertexType)
        {
            if (!Enum.TryParse(shader, out ExplicitShader value))
            {
                if (!int.TryParse(shader, out int intValue))
                    return new TagToolError(CommandError.ArgInvalid);
                value = (ExplicitShader)intValue;
            }

            using (var stream = Cache.OpenCacheReadWrite())
            {
                CachedTag pixlTag = Cache.TagCache.GetTag($@"rasterizer\shaders\{value}.pixl");
                var pixl = Cache.Deserialize<PixelShader>(stream, pixlTag);
                CachedTag vtshTag = Cache.TagCache.GetTag($@"rasterizer\shaders\{value}.vtsh");
                var vtsh = Cache.Deserialize<VertexShader>(stream, vtshTag);

                List<string> shaderStages = Enum.GetNames(typeof(HaloShaderGenerator.Globals.ShaderStage)).Select(s => s.ToLower()).ToList();
                int entryIndex = shaderStages.IndexOf(entry);
                if (entryIndex == -1)
                    return new TagToolError(CommandError.ArgInvalid, "Entry point not found.");

                while (entryIndex >= pixl.EntryPointShaders.Count)
                    pixl.EntryPointShaders.Add(new ShortOffsetCountBlock());

                if (pixl.EntryPointShaders[entryIndex].Count <= 0)
                {
                    pixl.EntryPointShaders[entryIndex].Offset = (byte)pixl.Shaders.Count;
                    pixl.EntryPointShaders[entryIndex].Count = 1;
                    pixl.Shaders.Add(new PixelShaderBlock());
                }

                int pixelShaderIndex = pixl.EntryPointShaders[entryIndex].Offset;

                var pixelResult = GenericPixelShaderGenerator.GeneratePixelShader(value.ToString(), entry);
                pixl.Shaders[pixelShaderIndex].PCShaderBytecode = pixelResult.Bytecode;
                pixl.Shaders[pixelShaderIndex].PCConstantTable = BuildConstantTable(pixelResult, Cache, true);

                List<string> vertexTypes = Enum.GetNames(typeof(TagTool.Geometry.VertexType)).Select(s => s.ToLower()).ToList();
                int vertexIndex = vertexTypes.IndexOf(vertexType.Replace("_", ""));

                if (vertexIndex != -1)
                {
                    while (entryIndex >= vtsh.EntryPoints.Count)
                        vtsh.EntryPoints.Add(new VertexShader.VertexShaderEntryPoint { SupportedVertexTypes = new List<ShortOffsetCountBlock>() });
                    while (vertexIndex >= vtsh.EntryPoints[entryIndex].SupportedVertexTypes.Count)
                        vtsh.EntryPoints[entryIndex].SupportedVertexTypes.Add(new ShortOffsetCountBlock());

                    if (vtsh.EntryPoints[entryIndex].SupportedVertexTypes[vertexIndex].Count <= 0)
                    {
                        vtsh.EntryPoints[entryIndex].SupportedVertexTypes[vertexIndex].Offset = (byte)vtsh.Shaders.Count;
                        vtsh.EntryPoints[entryIndex].SupportedVertexTypes[vertexIndex].Count = 1;
                        vtsh.Shaders.Add(new VertexShaderBlock());
                    }

                    int vertexShaderIndex = vtsh.EntryPoints[entryIndex].SupportedVertexTypes[vertexIndex].Offset;

                    var vertexResult = GenericVertexShaderGenerator.GenerateVertexShader(value.ToString(), entry);
                    vtsh.Shaders[vertexShaderIndex].PCShaderBytecode = vertexResult.Bytecode;
                    vtsh.Shaders[vertexShaderIndex].PCConstantTable = BuildConstantTable(vertexResult, Cache, false);

                    Cache.Serialize(stream, vtshTag, vtsh);
                }

                Cache.Serialize(stream, pixlTag, pixl);
            }

            Console.WriteLine($"Generated explicit shader for {value}");
            return true;
        }

        private object GenerateChudShader(string shader)
        {
            if (!Enum.TryParse(shader, out HaloShaderGenerator.Globals.ChudShader value))
            {
                if (!int.TryParse(shader, out int intValue))
                    return new TagToolError(CommandError.ArgInvalid);
                value = (HaloShaderGenerator.Globals.ChudShader)intValue;
            }

            // TODO: write register info to tag
            // TODO: vtsh support
            // TODO: entry point support
            // TODO: failsafes

            using (var stream = Cache.OpenCacheReadWrite())
            {
                var result = GenericPixelShaderGenerator.GeneratePixelShader(value.ToString(), HaloShaderGenerator.Globals.ShaderStage.Default.ToString().ToLower(), true);

                int pixelShaderIndex = 0; // TODO

                CachedTag pixlTag = Cache.TagCache.GetTag($@"rasterizer\shaders\{value}.pixl");
                var pixl = Cache.Deserialize<PixelShader>(stream, pixlTag);

                pixl.Shaders[pixelShaderIndex].PCShaderBytecode = result.Bytecode;

                Cache.Serialize(stream, pixlTag, pixl);
            }

            Console.WriteLine($"Generated chud shader for {value}");
            return true;
        }

        private object GenerateGlobalShader(string shaderType, bool pixel)
        {
            var generator = GetGlobalShaderGenerator(shaderType, true);
            if (generator == null)
                return new TagToolError(CommandError.ArgInvalid, $"\"{shaderType}\"");

            using (var stream = Cache.OpenCacheReadWrite())
            {
                CachedTag rmdfTag = Cache.TagCache.GetTag($"shaders\\{shaderType}.rmdf");
                RenderMethodDefinition rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

                if (pixel)
                {
                    GlobalPixelShader glps = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateSharedPixelShader(Cache, generator);
                    Cache.Serialize(stream, rmdf.GlobalPixelShader, glps);
                }
                else
                {
                    GlobalVertexShader glvs = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateSharedVertexShader(Cache, generator);
                    Cache.Serialize(stream, rmdf.GlobalVertexShader, glvs);
                }
            }

            Console.WriteLine($"Generated global {(pixel ? "pixel" : "vertex")} shader for {shaderType}");
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
            HaloShaderGenerator.Shared.Distortion distortion = (HaloShaderGenerator.Shared.Distortion)options[10];

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

        private void GenerateCustom(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            HaloShaderGenerator.Custom.Albedo albedo = (HaloShaderGenerator.Custom.Albedo)options[0];
            HaloShaderGenerator.Custom.Bump_Mapping bumpMapping = (HaloShaderGenerator.Custom.Bump_Mapping)options[1];
            HaloShaderGenerator.Custom.Alpha_Test alphaTest = (HaloShaderGenerator.Custom.Alpha_Test)options[2];
            HaloShaderGenerator.Custom.Specular_Mask specularMask = (HaloShaderGenerator.Custom.Specular_Mask)options[3];
            HaloShaderGenerator.Custom.Material_Model materialModel = (HaloShaderGenerator.Custom.Material_Model)options[4];
            HaloShaderGenerator.Custom.Environment_Mapping environmentMapping = (HaloShaderGenerator.Custom.Environment_Mapping)options[5];
            HaloShaderGenerator.Custom.Self_Illumination selfIllumination = (HaloShaderGenerator.Custom.Self_Illumination)options[6];
            HaloShaderGenerator.Custom.Blend_Mode blendMode = (HaloShaderGenerator.Custom.Blend_Mode)options[7];
            HaloShaderGenerator.Custom.Parallax parallax = (HaloShaderGenerator.Custom.Parallax)options[8];
            HaloShaderGenerator.Custom.Misc misc = (HaloShaderGenerator.Custom.Misc)options[9];

            var generator = new HaloShaderGenerator.Custom.CustomGenerator(albedo, bumpMapping, alphaTest, specularMask, materialModel, environmentMapping, selfIllumination, blendMode, parallax, misc);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateScreen(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            HaloShaderGenerator.Screen.Warp warp = (HaloShaderGenerator.Screen.Warp)options[0];
            HaloShaderGenerator.Screen.Base _base = (HaloShaderGenerator.Screen.Base)options[1];
            HaloShaderGenerator.Screen.Overlay_A overlay_a = (HaloShaderGenerator.Screen.Overlay_A)options[2];
            HaloShaderGenerator.Screen.Overlay_B overlay_b = (HaloShaderGenerator.Screen.Overlay_B)options[3];
            HaloShaderGenerator.Screen.Blend_Mode blend_mode = (HaloShaderGenerator.Screen.Blend_Mode)options[4];

            var generator = new HaloShaderGenerator.Screen.ScreenGenerator(warp, _base, overlay_a, overlay_b, blend_mode);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateZOnly(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            HaloShaderGenerator.ZOnly.Test test = (HaloShaderGenerator.ZOnly.Test)options[0];

            var generator = new HaloShaderGenerator.ZOnly.ZOnlyGenerator(test);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateWater(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            var generator = new HaloShaderGenerator.Water.WaterGenerator(options.Select(x => (byte)x).ToArray(), true);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateFoliage(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            var generator = new HaloShaderGenerator.Foliage.FoliageGenerator(options.Select(x => (byte)x).ToArray(), true);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnlineBase).SaveTagNames();
        }

        private void GenerateTerrain(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            var generator = new HaloShaderGenerator.Terrain.TerrainGenerator(options.Select(x => (byte)x).ToArray(), true);

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
