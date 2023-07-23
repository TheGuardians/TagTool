using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Shaders;
using TagTool.Shaders.ShaderFunctions;
using HaloShaderGenerator.Shader;
using static TagTool.Tags.Definitions.RenderMethod.RenderMethodPostprocessBlock;
using TagTool.Shaders.ShaderGenerator;

namespace TagTool.Commands.Shaders
{
    public class GenerateShaderCommand : Command
    {
        public struct SDependentRenderMethodData
        {
            public CachedTag Tag;
            public object Definition;
            public ShaderFunctionHelper.AnimatedParameter[] AnimatedParameters;
            public List<string> OrderedRealParameters;
            public List<string> OrderedIntParameters;
            public List<string> OrderedBoolParameters;
            public List<string> OrderedTextures;
            public int EffectIndex;

            public static void AddDependant(List<SDependentRenderMethodData> dependentRenderMethods, GameCache cache, CachedTag dependent, RenderMethod renderMethod, RenderMethodTemplate origRmt2, int effectIndex = -1)
            {
                var animatedParams = ShaderFunctionHelper.GetAnimatedParameters(cache, renderMethod, origRmt2);

                SDependentRenderMethodData dpData = new SDependentRenderMethodData
                {
                    Tag = dependent,
                    Definition = renderMethod,
                    AnimatedParameters = animatedParams.ToArray(),
                    OrderedRealParameters = new List<string>(),
                    OrderedIntParameters = new List<string>(),
                    OrderedBoolParameters = new List<string>(),
                    OrderedTextures = new List<string>(),
                    EffectIndex = effectIndex,
                };

                foreach (var realP in origRmt2.RealParameterNames)
                    dpData.OrderedRealParameters.Add(cache.StringTable.GetString(realP.Name));
                foreach (var intP in origRmt2.IntegerParameterNames)
                    dpData.OrderedIntParameters.Add(cache.StringTable.GetString(intP.Name));
                foreach (var boolP in origRmt2.BooleanParameterNames)
                    dpData.OrderedBoolParameters.Add(cache.StringTable.GetString(boolP.Name));
                foreach (var textureP in origRmt2.TextureParameterNames)
                    dpData.OrderedTextures.Add(cache.StringTable.GetString(textureP.Name));

                dependentRenderMethods.Add(dpData);
            }
        }

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

        static readonly List<string> UnsupportedShaderTypes = new List<string> { "glass" };

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
                    return new TagToolError(CommandError.TagInvalid, $"Could not find rmdf tag for \"{shaderType}\"");
                }

                var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

                if (rmdf.GlobalVertexShader == null || rmdf.GlobalPixelShader == null)
                    return new TagToolError(CommandError.TagInvalid, "A global shader was missing from rmdf");

                List<byte> options = new List<byte>();
                for (int i = 0; i < args.Count; i++)
                {
                    // parse options as int, if fails try finding in rmdf string
                    if (!byte.TryParse(args[i].ToLower(), out byte optionInteger))
                    {
                        bool found = false;

                        for (byte j = 0; j < rmdf.Categories[i].ShaderOptions.Count; j++)
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

                GenerateRenderMethodTemplate(Cache, stream, shaderType, options.ToArray(), rmdf);
            }

            return true;
        }

        public static HaloShaderGenerator.Generator.IShaderGenerator GetShaderGenerator(string shaderType, byte[] options, bool applyFixes = false)
        {
            switch (shaderType)
            {
                case "beam":            return new HaloShaderGenerator.Beam.BeamGenerator(options, applyFixes);
                case "black":           return new HaloShaderGenerator.Black.ShaderBlackGenerator();
                case "contrail":        return new HaloShaderGenerator.Contrail.ContrailGenerator(options, applyFixes);
                case "cortana":         return new HaloShaderGenerator.Cortana.CortanaGenerator(options, applyFixes);
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

        public static HaloShaderGenerator.Generator.IShaderGenerator GetGlobalShaderGenerator(string shaderType, bool applyFixes = false)
        {
            switch (shaderType)
            {
                case "beam":            return new HaloShaderGenerator.Beam.BeamGenerator(applyFixes);
                case "black":           return new HaloShaderGenerator.Black.ShaderBlackGenerator();
                case "contrail":        return new HaloShaderGenerator.Contrail.ContrailGenerator(applyFixes);
                case "cortana":         return new HaloShaderGenerator.Cortana.CortanaGenerator(applyFixes);
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
                var rasg = Cache.Deserialize<RasterizerGlobals>(stream, Cache.TagCache.GetTag("globals\\rasterizer_globals.rasterizer_globals"));

                CachedTag pixlTag = rasg.DefaultShaders[(int)value].PixelShader ?? Cache.TagCache.AllocateTag<PixelShader>($"rasterizer\\shaders\\{value}");
                CachedTag vtshTag = rasg.DefaultShaders[(int)value].VertexShader ?? Cache.TagCache.AllocateTag<VertexShader>($"rasterizer\\shaders\\{value}");

                ShaderGeneratorNew.GenerateExplicitShader(Cache, stream, value.ToString(), out PixelShader pixl, out VertexShader vtsh);

                Cache.Serialize(stream, vtshTag, vtsh);
                Cache.Serialize(stream, pixlTag, pixl);
            }

            Console.WriteLine($"Generated explicit shaders for \"{value}\"");
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

            var type = (HaloShaderGenerator.Globals.ShaderType)Enum.Parse(typeof(HaloShaderGenerator.Globals.ShaderType), shaderType, true);

            using (var stream = Cache.OpenCacheReadWrite())
            {
                CachedTag rmdfTag = Cache.TagCache.GetTag($"shaders\\{shaderType}.rmdf");
                RenderMethodDefinition rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

                if (pixel)
                {
                    GlobalPixelShader glps = TagTool.Shaders.ShaderGenerator.ShaderGeneratorNew.GenerateSharedPixelShaders(Cache, rmdf, type);
                    Cache.Serialize(stream, rmdf.GlobalPixelShader, glps);
                }
                else
                {
                    GlobalVertexShader glvs = TagTool.Shaders.ShaderGenerator.ShaderGeneratorNew.GenerateSharedVertexShaders(Cache, rmdf, type);
                    Cache.Serialize(stream, rmdf.GlobalVertexShader, glvs);
                }
            }

            Console.WriteLine($"Generated global {(pixel ? "pixel" : "vertex")} shader for {shaderType}");
            return true;
        }

        public static void GenerateRenderMethodTemplate(GameCache cache, Stream stream, string shaderType, byte[] options, RenderMethodDefinition rmdf, bool suppressCli = false)
        {
            string rmt2Name = $"shaders\\{shaderType}_templates\\_{string.Join("_", options)}";

            // Collect dependent render methods, store arguments

            List<SDependentRenderMethodData> dependentRenderMethods = new List<SDependentRenderMethodData>();

            if (cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
            {
                var origRmt2 = cache.Deserialize<RenderMethodTemplate>(stream, rmt2Tag);
                var dependents = (cache as GameCacheHaloOnlineBase).TagCacheGenHO.NonNull().Where(t => ((Cache.HaloOnline.CachedTagHaloOnline)t).Dependencies.Contains(rmt2Tag.Index));

                foreach (var dependent in dependents)
                {
                    object definition = null;
                    
                    if (dependent.IsInGroup("rm  "))
                    { 
                        definition = cache.Deserialize(stream, dependent);
                    }
                    else
                    {
                        switch (dependent.Group.Tag.ToString())
                        {
                            case "prt3":
                                var prt3 = cache.Deserialize<Particle>(stream, dependent);
                                definition = prt3.RenderMethod;
                                break;
                            case "decs":
                                var decs = cache.Deserialize<DecalSystem>(stream, dependent);
                                for (int i = 0; i < decs.Decal.Count; i++)
                                    if (decs.Decal[i].RenderMethod.ShaderProperties[0].Template.Name == rmt2Name)
                                        SDependentRenderMethodData.AddDependant(dependentRenderMethods, cache, dependent, decs.Decal[i].RenderMethod, origRmt2, i);
                                continue;
                            case "beam":
                                var beam = cache.Deserialize<BeamSystem>(stream, dependent);
                                for (int i = 0; i < beam.Beams.Count; i++)
                                    if (beam.Beams[i].RenderMethod.ShaderProperties[0].Template.Name == rmt2Name)
                                        SDependentRenderMethodData.AddDependant(dependentRenderMethods, cache, dependent, beam.Beams[i].RenderMethod, origRmt2, i);
                                continue;
                            case "ltvl":
                                var ltvl = cache.Deserialize<LightVolumeSystem>(stream, dependent);
                                for (int i = 0; i < ltvl.LightVolumes.Count; i++)
                                    if (ltvl.LightVolumes[i].RenderMethod.ShaderProperties[0].Template.Name == rmt2Name)
                                        SDependentRenderMethodData.AddDependant(dependentRenderMethods, cache, dependent, ltvl.LightVolumes[i].RenderMethod, origRmt2, i);
                                continue;
                            case "cntl":
                                var cntl = cache.Deserialize<ContrailSystem>(stream, dependent);
                                for (int i = 0; i < cntl.Contrails.Count; i++)
                                    if (cntl.Contrails[i].RenderMethod.ShaderProperties[0].Template.Name == rmt2Name)
                                        SDependentRenderMethodData.AddDependant(dependentRenderMethods, cache, dependent, cntl.Contrails[i].RenderMethod, origRmt2, i);
                                continue;
                        }
                    }

                    SDependentRenderMethodData.AddDependant(dependentRenderMethods, cache, dependent, (RenderMethod)definition, origRmt2);
                }
            }

            // Generate template

            var generator = GetShaderGenerator(shaderType, options, true);

            var glps = cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            //var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGeneratorNew.GenerateTemplateSafe(cache, stream, rmdf, rmt2Name, out _, out _);
            //TagTool.Shaders.ShaderGenerator.ShaderGeneratorNew.VerifyRmt2Routing(cache, stream, rmt2, rmdf, options.ToList());

            if (rmt2Tag == null)
                rmt2Tag = cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            cache.Serialize(stream, rmt2Tag, rmt2);
            cache.SaveStrings();
            (cache as GameCacheHaloOnlineBase).SaveTagNames();

            if (!suppressCli)
                Console.WriteLine($"Generated shader template \"{rmt2Name}\"");

            // Fixup render method parameters

            foreach (var dependent in dependentRenderMethods)
            {
                var postprocess = (dependent.Definition as RenderMethod).ShaderProperties[0];

                List<TextureConstant> reorderedTextureConstants = new List<TextureConstant>();
                foreach (var textureName in rmt2.TextureParameterNames)
                {
                    int origIndex = dependent.OrderedTextures.IndexOf(cache.StringTable.GetString(textureName.Name));
                    if (origIndex != -1)
                        reorderedTextureConstants.Add(postprocess.TextureConstants[origIndex]);
                    else
                        reorderedTextureConstants.Add(new TextureConstant());
                }
                postprocess.TextureConstants = reorderedTextureConstants;

                List<RealConstant> reorderedRealConstants = new List<RealConstant>();
                foreach (var realName in rmt2.RealParameterNames)
                {
                    int origIndex = dependent.OrderedRealParameters.IndexOf(cache.StringTable.GetString(realName.Name));
                    if (origIndex != -1)
                        reorderedRealConstants.Add(postprocess.RealConstants[origIndex]);
                    else
                        reorderedRealConstants.Add(new RealConstant());
                }
                postprocess.RealConstants = reorderedRealConstants;

                List<uint> reorderedIntConstants = new List<uint>();
                foreach (var intName in rmt2.IntegerParameterNames)
                {
                    int origIndex = dependent.OrderedIntParameters.IndexOf(cache.StringTable.GetString(intName.Name));
                    if (origIndex != -1)
                        reorderedIntConstants.Add(postprocess.IntegerConstants[origIndex]);
                    else
                        reorderedIntConstants.Add(new uint());
                }
                postprocess.IntegerConstants = reorderedIntConstants;

                uint reorderedBoolConstants = 0;
                for (int i = 0; i < rmt2.BooleanParameterNames.Count; i++)
                {
                    int origIndex = dependent.OrderedBoolParameters.IndexOf(cache.StringTable.GetString(rmt2.BooleanParameterNames[i].Name));
                    if (origIndex != -1)
                        reorderedBoolConstants |= ((postprocess.BooleanConstants >> origIndex) & 1) == 1 ? 1u << i : 0;
                }
                postprocess.BooleanConstants = reorderedBoolConstants;

                var textureAnimatedParams = dependent.AnimatedParameters.Where(x => x.Type == ShaderFunctionHelper.ParameterType.Texture);
                var realAnimatedParams = dependent.AnimatedParameters.Where(x => x.Type == ShaderFunctionHelper.ParameterType.Real);
                var intAnimatedParams = dependent.AnimatedParameters.Where(x => x.Type == ShaderFunctionHelper.ParameterType.Int);
                var boolAnimatedParams = dependent.AnimatedParameters.Where(x => x.Type == ShaderFunctionHelper.ParameterType.Bool);

                postprocess.RoutingInfo.Clear();
                var routingInfo = postprocess.RoutingInfo;

                foreach (var pass in postprocess.Passes)
                {
                    pass.RealPixel.Integer = 0;
                    pass.RealVertex.Integer = 0;
                    pass.Texture.Integer = 0;
                }

                for (int k = 0; k < postprocess.EntryPoints.Count; k++)
                {
                    var entry = postprocess.EntryPoints[k];
                    var rmt2Entry = rmt2.EntryPoints[k];

                    for (int i = entry.Offset; i < entry.Offset + entry.Count; i++)
                    {
                        var pass = postprocess.Passes[i];
                        var rmt2Pass = rmt2.Passes[rmt2Entry.Offset + (i - entry.Offset)];

                        // Texture

                        int usageOffset = rmt2Pass.Values[(int)ParameterUsage.Texture].Offset;
                        int usageCount = rmt2Pass.Values[(int)ParameterUsage.Texture].Count;

                        pass.Texture.Offset = (ushort)routingInfo.Count;
                        for (int j = usageOffset; j < usageOffset + usageCount; j++)
                        {
                            var rmt2RoutingInfo = rmt2.RoutingInfo[j];

                            string paramName = cache.StringTable.GetString(rmt2.TextureParameterNames[rmt2RoutingInfo.SourceIndex].Name);

                            foreach (var animatedParam in textureAnimatedParams)
                            {
                                if (animatedParam.Name == paramName)
                                {
                                    var newBlock = new RenderMethodRoutingInfoBlock 
                                    { 
                                        SourceIndex = rmt2RoutingInfo.SourceIndex, 
                                        FunctionIndex = (byte)animatedParam.FunctionIndex,
                                        RegisterIndex = (short)rmt2RoutingInfo.DestinationIndex
                                    };

                                    routingInfo.Add(newBlock);
                                    pass.Texture.Count++;
                                    break;
                                }
                            }
                        }
                        pass.Texture.Offset = pass.Texture.Count == 0 ? (ushort)0 : pass.Texture.Offset;

                        // Real PS

                        usageOffset = rmt2Pass.Values[(int)ParameterUsage.PS_Real].Offset;
                        usageCount = rmt2Pass.Values[(int)ParameterUsage.PS_Real].Count;

                        pass.RealPixel.Offset = (ushort)routingInfo.Count;
                        for (int j = usageOffset; j < usageOffset + usageCount; j++)
                        {
                            var rmt2RoutingInfo = rmt2.RoutingInfo[j];

                            string paramName = cache.StringTable.GetString(rmt2.RealParameterNames[rmt2RoutingInfo.SourceIndex].Name);

                            foreach (var animatedParam in realAnimatedParams)
                            {
                                if (animatedParam.Name == paramName)
                                {
                                    var newBlock = new RenderMethodRoutingInfoBlock
                                    {
                                        SourceIndex = rmt2RoutingInfo.SourceIndex,
                                        FunctionIndex = (byte)animatedParam.FunctionIndex,
                                        RegisterIndex = (short)rmt2RoutingInfo.DestinationIndex
                                    };

                                    routingInfo.Add(newBlock);
                                    pass.RealPixel.Count++;
                                    break;
                                }
                            }
                        }
                        pass.RealPixel.Offset = pass.RealPixel.Count == 0 ? (ushort)0 : pass.RealPixel.Offset;

                        // Real VS

                        usageOffset = rmt2Pass.Values[(int)ParameterUsage.VS_Real].Offset;
                        usageCount = rmt2Pass.Values[(int)ParameterUsage.VS_Real].Count;

                        pass.RealVertex.Offset = (ushort)routingInfo.Count;
                        for (int j = usageOffset; j < usageOffset + usageCount; j++)
                        {
                            var rmt2RoutingInfo = rmt2.RoutingInfo[j];

                            string paramName = cache.StringTable.GetString(rmt2.RealParameterNames[rmt2RoutingInfo.SourceIndex].Name);

                            foreach (var animatedParam in realAnimatedParams)
                            {
                                if (animatedParam.Name == paramName)
                                {
                                    var newBlock = new RenderMethodRoutingInfoBlock
                                    {
                                        SourceIndex = rmt2RoutingInfo.SourceIndex,
                                        FunctionIndex = (byte)animatedParam.FunctionIndex,
                                        RegisterIndex = (short)rmt2RoutingInfo.DestinationIndex
                                    };

                                    routingInfo.Add(newBlock);
                                    pass.RealVertex.Count++;
                                    break;
                                }
                            }
                        }
                        pass.RealVertex.Offset = pass.RealVertex.Count == 0 ? (ushort)0 : pass.RealVertex.Offset;
                    }
                }

                if (dependent.Tag.IsInGroup("rm  "))
                {
                    cache.Serialize(stream, dependent.Tag, dependent.Definition);
                }
                else
                {
                    switch (dependent.Tag.Group.Tag.ToString())
                    {
                        case "prt3":
                            var prt3 = cache.Deserialize<Particle>(stream, dependent.Tag);
                            prt3.RenderMethod = (RenderMethod)dependent.Definition;
                            cache.Serialize(stream, dependent.Tag, prt3);
                            break;
                        case "decs":
                            var decs = cache.Deserialize<DecalSystem>(stream, dependent.Tag);
                            decs.Decal[dependent.EffectIndex].RenderMethod = (RenderMethod)dependent.Definition;
                            cache.Serialize(stream, dependent.Tag, decs);
                            break;
                        case "beam":
                            var beam = cache.Deserialize<BeamSystem>(stream, dependent.Tag);
                            beam.Beams[dependent.EffectIndex].RenderMethod = (RenderMethod)dependent.Definition;
                            cache.Serialize(stream, dependent.Tag, beam);
                            break;
                        case "ltvl":
                            var ltvl = cache.Deserialize<LightVolumeSystem>(stream, dependent.Tag);
                            ltvl.LightVolumes[dependent.EffectIndex].RenderMethod = (RenderMethod)dependent.Definition;
                            cache.Serialize(stream, dependent.Tag, ltvl);
                            break;
                        case "cntl":
                            var cntl = cache.Deserialize<ContrailSystem>(stream, dependent.Tag);
                            cntl.Contrails[dependent.EffectIndex].RenderMethod = (RenderMethod)dependent.Definition;
                            cache.Serialize(stream, dependent.Tag, cntl);
                            break;
                    }
                }
            }

            if (dependentRenderMethods.Count > 0 && !suppressCli)
                Console.WriteLine($"Corrected {dependentRenderMethods.Count} render method{(dependentRenderMethods.Count > 1 ? "s" : "")}");
        }

        // ASYNC -------------------------------

        public struct STemplateRecompileInfo
        {
            // init
            public string Name;
            public string ShaderType;
            public byte[] Options;
            public CachedTag Tag;
            public List<SDependentRenderMethodData> Dependants;
            public List<RenderMethodOption.ParameterBlock> AllRmopParameters;
            // post
            public PixelShader PixelShader;
            public VertexShader VertexShader;
            public RenderMethodTemplate Template;
        }

        /// <summary>
        /// For async recompile
        /// </summary>
        public static List<SDependentRenderMethodData> GetDependantsAsync(GameCache cache,
            Stream stream,
            string shaderType,
            byte[] options)
        {
            string rmt2Name = $"shaders\\{shaderType}_templates\\_{string.Join("_", options)}";

            // Collect dependent render methods, store arguments

            List<SDependentRenderMethodData> dependentRenderMethods = new List<SDependentRenderMethodData>();

            if (cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
            {
                RenderMethodTemplate originalRmt2 = cache.Deserialize<RenderMethodTemplate>(stream, rmt2Tag);
                var dependents = (cache as GameCacheHaloOnlineBase).TagCacheGenHO.NonNull().Where(t => ((Cache.HaloOnline.CachedTagHaloOnline)t).Dependencies.Contains(rmt2Tag.Index));

                foreach (var dependent in dependents)
                {
                    object definition = null;

                    if (dependent.IsInGroup("rm  "))
                    {
                        definition = cache.Deserialize(stream, dependent);
                    }
                    else
                    {
                        switch (dependent.Group.Tag.ToString())
                        {
                            case "prt3":
                                var prt3 = cache.Deserialize<Particle>(stream, dependent);
                                definition = prt3.RenderMethod;
                                break;
                            case "decs":
                                var decs = cache.Deserialize<DecalSystem>(stream, dependent);
                                for (int i = 0; i < decs.Decal.Count; i++)
                                    if (decs.Decal[i].RenderMethod.ShaderProperties[0].Template.Name == rmt2Name)
                                        SDependentRenderMethodData.AddDependant(dependentRenderMethods, cache, dependent, decs.Decal[i].RenderMethod, originalRmt2, i);
                                continue;
                            case "beam":
                                var beam = cache.Deserialize<BeamSystem>(stream, dependent);
                                for (int i = 0; i < beam.Beams.Count; i++)
                                    if (beam.Beams[i].RenderMethod.ShaderProperties[0].Template.Name == rmt2Name)
                                        SDependentRenderMethodData.AddDependant(dependentRenderMethods, cache, dependent, beam.Beams[i].RenderMethod, originalRmt2, i);
                                continue;
                            case "ltvl":
                                var ltvl = cache.Deserialize<LightVolumeSystem>(stream, dependent);
                                for (int i = 0; i < ltvl.LightVolumes.Count; i++)
                                    if (ltvl.LightVolumes[i].RenderMethod.ShaderProperties[0].Template.Name == rmt2Name)
                                        SDependentRenderMethodData.AddDependant(dependentRenderMethods, cache, dependent, ltvl.LightVolumes[i].RenderMethod, originalRmt2, i);
                                continue;
                            case "cntl":
                                var cntl = cache.Deserialize<ContrailSystem>(stream, dependent);
                                for (int i = 0; i < cntl.Contrails.Count; i++)
                                    if (cntl.Contrails[i].RenderMethod.ShaderProperties[0].Template.Name == rmt2Name)
                                        SDependentRenderMethodData.AddDependant(dependentRenderMethods, cache, dependent, cntl.Contrails[i].RenderMethod, originalRmt2, i);
                                continue;
                        }
                    }

                    SDependentRenderMethodData.AddDependant(dependentRenderMethods, cache, dependent, (RenderMethod)definition, originalRmt2);
                }
            }

            return dependentRenderMethods;
        }

        /// <summary>
        /// For async recompile
        /// </summary>
        public static void ReserializeDependantsAsync(GameCache cache,
            Stream stream,
            RenderMethodTemplate rmt2,
            List<SDependentRenderMethodData> dependentRenderMethods)
        {
            // Fixup render method parameters

            foreach (var dependent in dependentRenderMethods)
            {
                var postprocess = (dependent.Definition as RenderMethod).ShaderProperties[0];

                List<TextureConstant> reorderedTextureConstants = new List<TextureConstant>();
                foreach (var textureName in rmt2.TextureParameterNames)
                {
                    int origIndex = dependent.OrderedTextures.IndexOf(cache.StringTable.GetString(textureName.Name));
                    if (origIndex != -1)
                        reorderedTextureConstants.Add(postprocess.TextureConstants[origIndex]);
                    else
                        reorderedTextureConstants.Add(new TextureConstant());
                }
                postprocess.TextureConstants = reorderedTextureConstants;

                List<RealConstant> reorderedRealConstants = new List<RealConstant>();
                foreach (var realName in rmt2.RealParameterNames)
                {
                    int origIndex = dependent.OrderedRealParameters.IndexOf(cache.StringTable.GetString(realName.Name));
                    if (origIndex != -1)
                        reorderedRealConstants.Add(postprocess.RealConstants[origIndex]);
                    else
                        reorderedRealConstants.Add(new RealConstant());
                }
                postprocess.RealConstants = reorderedRealConstants;

                List<uint> reorderedIntConstants = new List<uint>();
                foreach (var intName in rmt2.IntegerParameterNames)
                {
                    int origIndex = dependent.OrderedIntParameters.IndexOf(cache.StringTable.GetString(intName.Name));
                    if (origIndex != -1)
                        reorderedIntConstants.Add(postprocess.IntegerConstants[origIndex]);
                    else
                        reorderedIntConstants.Add(new uint());
                }
                postprocess.IntegerConstants = reorderedIntConstants;

                uint reorderedBoolConstants = 0;
                for (int i = 0; i < rmt2.BooleanParameterNames.Count; i++)
                {
                    int origIndex = dependent.OrderedBoolParameters.IndexOf(cache.StringTable.GetString(rmt2.BooleanParameterNames[i].Name));
                    if (origIndex != -1)
                        reorderedBoolConstants |= ((postprocess.BooleanConstants >> origIndex) & 1) == 1 ? 1u << i : 0;
                }
                postprocess.BooleanConstants = reorderedBoolConstants;

                var textureAnimatedParams = dependent.AnimatedParameters.Where(x => x.Type == ShaderFunctionHelper.ParameterType.Texture);
                var realAnimatedParams = dependent.AnimatedParameters.Where(x => x.Type == ShaderFunctionHelper.ParameterType.Real);
                var intAnimatedParams = dependent.AnimatedParameters.Where(x => x.Type == ShaderFunctionHelper.ParameterType.Int);
                var boolAnimatedParams = dependent.AnimatedParameters.Where(x => x.Type == ShaderFunctionHelper.ParameterType.Bool);

                postprocess.RoutingInfo.Clear();
                var routingInfo = postprocess.RoutingInfo;

                foreach (var pass in postprocess.Passes)
                {
                    pass.RealPixel.Integer = 0;
                    pass.RealVertex.Integer = 0;
                    pass.Texture.Integer = 0;
                }

                for (int k = 0; k < postprocess.EntryPoints.Count; k++)
                {
                    var entry = postprocess.EntryPoints[k];
                    var rmt2Entry = rmt2.EntryPoints[k];

                    for (int i = entry.Offset; i < entry.Offset + entry.Count; i++)
                    {
                        var pass = postprocess.Passes[i];
                        var rmt2Pass = rmt2.Passes[rmt2Entry.Offset + (i - entry.Offset)];

                        // Texture

                        int usageOffset = rmt2Pass.Values[(int)ParameterUsage.Texture].Offset;
                        int usageCount = rmt2Pass.Values[(int)ParameterUsage.Texture].Count;

                        pass.Texture.Offset = (ushort)routingInfo.Count;
                        for (int j = usageOffset; j < usageOffset + usageCount; j++)
                        {
                            var rmt2RoutingInfo = rmt2.RoutingInfo[j];

                            string paramName = cache.StringTable.GetString(rmt2.TextureParameterNames[rmt2RoutingInfo.SourceIndex].Name);

                            foreach (var animatedParam in textureAnimatedParams)
                            {
                                if (animatedParam.Name == paramName)
                                {
                                    var newBlock = new RenderMethodRoutingInfoBlock
                                    {
                                        SourceIndex = rmt2RoutingInfo.SourceIndex,
                                        FunctionIndex = (byte)animatedParam.FunctionIndex,
                                        RegisterIndex = (short)rmt2RoutingInfo.DestinationIndex
                                    };

                                    routingInfo.Add(newBlock);
                                    pass.Texture.Count++;
                                    break;
                                }
                            }
                        }
                        pass.Texture.Offset = pass.Texture.Count == 0 ? (ushort)0 : pass.Texture.Offset;

                        // Real PS

                        usageOffset = rmt2Pass.Values[(int)ParameterUsage.PS_Real].Offset;
                        usageCount = rmt2Pass.Values[(int)ParameterUsage.PS_Real].Count;

                        pass.RealPixel.Offset = (ushort)routingInfo.Count;
                        for (int j = usageOffset; j < usageOffset + usageCount; j++)
                        {
                            var rmt2RoutingInfo = rmt2.RoutingInfo[j];

                            string paramName = cache.StringTable.GetString(rmt2.RealParameterNames[rmt2RoutingInfo.SourceIndex].Name);

                            foreach (var animatedParam in realAnimatedParams)
                            {
                                if (animatedParam.Name == paramName)
                                {
                                    var newBlock = new RenderMethodRoutingInfoBlock
                                    {
                                        SourceIndex = rmt2RoutingInfo.SourceIndex,
                                        FunctionIndex = (byte)animatedParam.FunctionIndex,
                                        RegisterIndex = (short)rmt2RoutingInfo.DestinationIndex
                                    };

                                    routingInfo.Add(newBlock);
                                    pass.RealPixel.Count++;
                                    break;
                                }
                            }
                        }
                        pass.RealPixel.Offset = pass.RealPixel.Count == 0 ? (ushort)0 : pass.RealPixel.Offset;

                        // Real VS

                        usageOffset = rmt2Pass.Values[(int)ParameterUsage.VS_Real].Offset;
                        usageCount = rmt2Pass.Values[(int)ParameterUsage.VS_Real].Count;

                        pass.RealVertex.Offset = (ushort)routingInfo.Count;
                        for (int j = usageOffset; j < usageOffset + usageCount; j++)
                        {
                            var rmt2RoutingInfo = rmt2.RoutingInfo[j];

                            string paramName = cache.StringTable.GetString(rmt2.RealParameterNames[rmt2RoutingInfo.SourceIndex].Name);

                            foreach (var animatedParam in realAnimatedParams)
                            {
                                if (animatedParam.Name == paramName)
                                {
                                    var newBlock = new RenderMethodRoutingInfoBlock
                                    {
                                        SourceIndex = rmt2RoutingInfo.SourceIndex,
                                        FunctionIndex = (byte)animatedParam.FunctionIndex,
                                        RegisterIndex = (short)rmt2RoutingInfo.DestinationIndex
                                    };

                                    routingInfo.Add(newBlock);
                                    pass.RealVertex.Count++;
                                    break;
                                }
                            }
                        }
                        pass.RealVertex.Offset = pass.RealVertex.Count == 0 ? (ushort)0 : pass.RealVertex.Offset;
                    }
                }

                if (dependent.Tag.IsInGroup("rm  "))
                {
                    cache.Serialize(stream, dependent.Tag, dependent.Definition);
                }
                else
                {
                    switch (dependent.Tag.Group.Tag.ToString())
                    {
                        case "prt3":
                            var prt3 = cache.Deserialize<Particle>(stream, dependent.Tag);
                            prt3.RenderMethod = (RenderMethod)dependent.Definition;
                            cache.Serialize(stream, dependent.Tag, prt3);
                            break;
                        case "decs":
                            var decs = cache.Deserialize<DecalSystem>(stream, dependent.Tag);
                            decs.Decal[dependent.EffectIndex].RenderMethod = (RenderMethod)dependent.Definition;
                            cache.Serialize(stream, dependent.Tag, decs);
                            break;
                        case "beam":
                            var beam = cache.Deserialize<BeamSystem>(stream, dependent.Tag);
                            beam.Beams[dependent.EffectIndex].RenderMethod = (RenderMethod)dependent.Definition;
                            cache.Serialize(stream, dependent.Tag, beam);
                            break;
                        case "ltvl":
                            var ltvl = cache.Deserialize<LightVolumeSystem>(stream, dependent.Tag);
                            ltvl.LightVolumes[dependent.EffectIndex].RenderMethod = (RenderMethod)dependent.Definition;
                            cache.Serialize(stream, dependent.Tag, ltvl);
                            break;
                        case "cntl":
                            var cntl = cache.Deserialize<ContrailSystem>(stream, dependent.Tag);
                            cntl.Contrails[dependent.EffectIndex].RenderMethod = (RenderMethod)dependent.Definition;
                            cache.Serialize(stream, dependent.Tag, cntl);
                            break;
                    }
                }
            }

            //if (dependentRenderMethods.Count > 0)
            //    Console.WriteLine($"Corrected {dependentRenderMethods.Count} render method{(dependentRenderMethods.Count > 1 ? "s" : "")}");
        }

        /// <summary>
        /// For async recompile.
        /// 
        /// Usage:
        ///     -> Collect all dependants for all targetted templates
        ///     -> Perform recompile on all templates and store results (this is the critical part that utilizes as much cpu as possible)
        ///     -> Serialization
        ///     -> Apply fixups for dependants
        /// </summary>
        public static STemplateRecompileInfo GenerateRenderMethodTemplateAsync(GameCache cache,
            STemplateRecompileInfo recompileInfo,
            RenderMethodDefinition rmdf,
            GlobalVertexShader glvs,
            GlobalPixelShader glps)
        {
            recompileInfo.Template = ShaderGeneratorNew.GenerateTemplate(cache, rmdf, glvs, glps, 
                recompileInfo.AllRmopParameters, recompileInfo.Name, out recompileInfo.PixelShader, out recompileInfo.VertexShader);

            return recompileInfo;
        }
    }
}
