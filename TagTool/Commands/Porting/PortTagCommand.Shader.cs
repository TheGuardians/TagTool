using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.Collections.Generic;
using System.IO;
using TagTool.Shaders;
using TagTool.Shaders.ShaderMatching;
using System;
using System.Linq;
using static TagTool.Tags.Definitions.RenderMethod;
using static TagTool.Tags.Definitions.RenderMethod.ShaderProperty;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public ShaderMatcherNew Matcher = new ShaderMatcherNew();

        private List<string> emblemTagNames = new List<string> // needs proper fix
        {
            @"objects\characters\odst\shaders\mc_emblem",
            @"objects\characters\odst_cine\shaders\mc_emblem",
            @"objects\characters\odst_oni_op\shaders\mc_emblem",
            @"objects\characters\elite\shaders\elite_emblem",
            @"objects\characters\masterchief\shaders\mc_emblem"
        };

        private readonly List<string> EffectRenderMethodTypes = new List<string>
        {
            "decal",
            "contrail",
            "particle",
            "beam",
            "light_volume",
        };

        private RasterizerGlobals ConvertRasterizerGlobals(RasterizerGlobals rasg)
        {
            if (BlamCache.Version == CacheVersion.Halo3ODST)
            {
                rasg.Unknown6HO = 6;
            }
            else
            {
                rasg.Unknown6HO = rasg.Unknown6;
            }
            return rasg;
        }

        private object ConvertShader(Stream cacheStream, Stream blamCacheStream, object definition, CachedTag blamTag, object blamDefinition)
        {
            switch (definition)
            {
                case ShaderFoliage rmfl:
                case ShaderTerrain rmtr:
                case ShaderCustom rmcs:
                case ShaderDecal rmd:
                case ShaderHalogram rmhg:
                case Shader rmsh:
                case ShaderWater rmw:
                case ShaderBlack rmbk:
                    return ConvertShaderInternal(cacheStream, blamCacheStream, (RenderMethod)definition, blamTag, (RenderMethod)blamDefinition);

                case ContrailSystem cntl:
                    var blamCntl = (ContrailSystem)blamDefinition;
                    for (int i = 0; i < cntl.Contrail.Count; i++)
                    {
                        cntl.Contrail[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, cntl.Contrail[i].RenderMethod, blamTag, blamCntl.Contrail[i].RenderMethod);
                        if (cntl.Contrail[i].RenderMethod == null) return null;
                    }
                    return cntl;

                case Particle prt3:
                    var blamPrt3 = (Particle)blamDefinition;
                    prt3.RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, prt3.RenderMethod, blamTag, blamPrt3.RenderMethod);
                    if (prt3.RenderMethod == null) return null;
                    return prt3;

                case LightVolumeSystem ltvl:
                    var blamLtvl = (LightVolumeSystem)blamDefinition;
                    for (int i = 0; i < ltvl.LightVolume.Count; i++)
                    {
                        ltvl.LightVolume[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, ltvl.LightVolume[i].RenderMethod, blamTag, blamLtvl.LightVolume[i].RenderMethod);
                        if (ltvl.LightVolume[i].RenderMethod == null) return null;
                    }
                    return ltvl;
                case DecalSystem decs:
                    var blamDecs = (DecalSystem)blamDefinition;
                    for (int i = 0; i < decs.Decal.Count; i++)
                    {
                        decs.Decal[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, decs.Decal[i].RenderMethod, blamTag, blamDecs.Decal[i].RenderMethod);
                        if (decs.Decal[i].RenderMethod == null) return null;
                    }
                    return decs;
                case BeamSystem beamSystem:
                    var blamBeam = (BeamSystem)blamDefinition;
                    for (int i = 0; i < beamSystem.Beam.Count; i++)
                    {
                        beamSystem.Beam[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, beamSystem.Beam[i].RenderMethod, blamTag, blamBeam.Beam[i].RenderMethod);
                        if (beamSystem.Beam[i].RenderMethod == null) return null;
                    }
                    return beamSystem;

                case ShaderScreen rmss:
                case ShaderZonly rmzo:
                case ShaderCortana rmct:
                    return null;
            }
            return null;
        }

        private RenderMethod ConvertShaderInternal(Stream cacheStream, Stream blamCacheStream, RenderMethod definition, CachedTag blamTag, RenderMethod blamDefinition)
        {
            var shaderProperty = definition.ShaderProperties[0];
            var blamShaderProperty = blamDefinition.ShaderProperties[0];

            // in case of failed match
            if (shaderProperty.Template == null)
                return null;

            return ConvertRenderMethod(cacheStream, blamCacheStream, definition, blamShaderProperty.Template, blamTag.Name);
        }

        private CachedTag GetDefaultShader(Tag groupTag, CachedTag edTag)
        {
            CachedTag shaderTag;

            switch (groupTag.ToString())
            {
                case "beam" when CacheContext.TagCache.TryGetTag(@"objects\weapons\support_high\spartan_laser\fx\firing_3p.beam", out shaderTag):
                    return shaderTag;

                case "cntl" when CacheContext.TagCache.TryGetTag(@"objects\weapons\pistol\needler\fx\projectile.cntl", out shaderTag):
                    return shaderTag;

                case "decs" when CacheContext.TagCache.TryGetTag(@"fx\decals\impact_plasma\impact_plasma_medium\hard.decs", out shaderTag):
                    return shaderTag;

                case "ltvl" when CacheContext.TagCache.TryGetTag(@"objects\weapons\pistol\plasma_pistol\fx\charged\projectile.ltvl", out shaderTag):
                    return shaderTag;

                case "prt3" when CacheContext.TagCache.TryGetTag(@"fx\particles\energy\sparks\impact_spark_orange.prt3", out shaderTag):
                    return shaderTag;

                case "rmd " when CacheContext.TagCache.TryGetTag(@"objects\gear\human\military\shaders\human_military_decals.rmd", out shaderTag):
                    return shaderTag;

                case "rmfl" when CacheContext.TagCache.TryGetTag(@"levels\multi\riverworld\shaders\riverworld_tree_leafa.rmfl", out shaderTag):
                    return shaderTag;

                case "rmtr" when CacheContext.TagCache.TryGetTag(@"levels\multi\riverworld\shaders\riverworld_ground.rmtr", out shaderTag):
                    return shaderTag;

                case "rmw " when CacheContext.TagCache.TryGetTag(@"levels\multi\riverworld\shaders\riverworld_water_rough.rmw", out shaderTag):
                    return shaderTag;

                case "rmhg" when CacheContext.TagCache.TryGetTag(@"objects\multi\shaders\koth_shield.rmhg", out shaderTag):
                    return shaderTag;

                case "rmbk" when CacheContext.TagCache.TryGetTag(@"levels\dlc\bunkerworld\shaders\z_black.rmsh", out shaderTag):
                    return shaderTag;

                case "rmrd":
                case "rmsh":
                case "rmss":
                case "rmcs":
                case "rmzo":
                case "rmct":
                    return CacheContext.TagCache.GetTag<Shader>(@"shaders\invalid");
            }

            Console.WriteLine($"No default shader found for \"{groupTag.ToString()}\", using \"shaders\\invalid.rmsh\"");
            return CacheContext.TagCache.GetTag<Shader>(@"shaders\invalid");
        }

        private CachedTag FindClosestRmt2(Stream cacheStream, Stream blamCacheStream, CachedTag blamRmt2)
        {
            // Verify that the ShaderMatcher is ready to use
            if (!Matcher.IsInitialized)
                Matcher.Init(CacheContext, BlamCache, cacheStream, blamCacheStream, FlagIsSet(PortingFlags.Ms30), FlagIsSet(PortingFlags.PefectShaderMatchOnly));

            return Matcher.FindClosestTemplate(blamRmt2, BlamCache.Deserialize<RenderMethodTemplate>(blamCacheStream, blamRmt2));
        }

        private RenderMethod ConvertRenderMethod(Stream cacheStream, Stream blamCacheStream, RenderMethod finalRm, CachedTag blamRmt2, string blamTagName)
        {
            var bmMaps = new List<string>();
            var bmRealConstants = new List<string>();
            var bmIntConstants = new List<string>();
            var bmBoolConstants = new List<string>();
            var edMaps = new List<string>();
            var edRealConstants = new List<string>();
            var edIntConstants = new List<string>();
            var edBoolConstants = new List<string>();

            // Make a template of ShaderProperty, with the correct bitmaps and arguments counts. 
            var newShaderProperty = new RenderMethod.ShaderProperty
            {
                TextureConstants = new List<TextureConstant>(),
                RealConstants = new List<RealConstant>(),
                IntegerConstants = new List<uint>(),
                BooleanConstants = 0
            };

            RenderMethod originalRm = finalRm;

            // Get a simple list of bitmaps and arguments names
            var bmRmt2Instance = blamRmt2;
            var bmRmt2 = BlamCache.Deserialize<RenderMethodTemplate>(blamCacheStream, bmRmt2Instance);

            // Get a simple list of H3 bitmaps and arguments names
            foreach (var a in bmRmt2.TextureParameterNames)
                bmMaps.Add(BlamCache.StringTable.GetString(a.Name));
            foreach (var a in bmRmt2.RealParameterNames)
                bmRealConstants.Add(BlamCache.StringTable.GetString(a.Name));
            foreach (var a in bmRmt2.IntegerParameterNames)
                bmIntConstants.Add(BlamCache.StringTable.GetString(a.Name));
            foreach (var a in bmRmt2.BooleanParameterNames)
                bmBoolConstants.Add(BlamCache.StringTable.GetString(a.Name));

            // get template previously matched from porttag rmt2
            CachedTag edRmt2Instance = finalRm.ShaderProperties[0].Template;

            if (edRmt2Instance == null)
            {
                throw new Exception($"Failed to find HO rmt2 for this RenderMethod instance");
            }

            // create blam rmt2 descriptor
            ShaderMatcherNew.Rmt2Descriptor.TryParse(bmRmt2Instance.Name, out ShaderMatcherNew.Rmt2Descriptor blamRmt2Descriptor);
            // create ed rmt2 descriptor
            ShaderMatcherNew.Rmt2Descriptor.TryParse(edRmt2Instance.Name, out ShaderMatcherNew.Rmt2Descriptor edRmt2Descriptor);

            // black has no options, skip conversion
            if (edRmt2Descriptor.Type == "black")
                return finalRm;

            var edRmt2 = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, edRmt2Instance);

            foreach (var a in edRmt2.TextureParameterNames)
                edMaps.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.RealParameterNames)
                edRealConstants.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.IntegerParameterNames)
                edIntConstants.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.BooleanParameterNames)
                edBoolConstants.Add(CacheContext.StringTable.GetString(a.Name));

            // get relevant rmdf
            CachedTag rmdfInstance = Matcher.FindRmdf(edRmt2Instance);
            if (rmdfInstance == null) // shader matching will fail without an rmdf -- throw an exception
                throw new Exception($"Unable to find valid \"{edRmt2Descriptor.Type}\" rmdf for rmt2");
            RenderMethodDefinition renderMethodDefinition = CacheContext.Deserialize<RenderMethodDefinition>(cacheStream, rmdfInstance);

            // dictionaries for fast lookup
            //var optionParameters = Matcher.GetOptionParameters(rmt2Descriptor.Options.ToList(), renderMethodDefinition);
            var optionBlocks = Matcher.GetOptionBlocks(edRmt2Descriptor.Options.ToList(), renderMethodDefinition);
            var optionBitmaps = Matcher.GetOptionBitmaps(edRmt2Descriptor.Options.ToList(), renderMethodDefinition);

            List<string> methodNames = new List<string>();
            foreach (var method in renderMethodDefinition.Methods)
                methodNames.Add(CacheContext.StringTable.GetString(method.Type));

            foreach (var a in edRealConstants)
                newShaderProperty.RealConstants.Add(GetDefaultRealConstant(a, edRmt2Descriptor.Type, methodNames, optionBlocks));

            foreach (var a in edIntConstants)
                newShaderProperty.IntegerConstants.Add(GetDefaultValue(a, edRmt2Descriptor.Type, methodNames, optionBlocks));

            foreach (var a in edMaps)
                newShaderProperty.TextureConstants.Add(GetDefaultTextureConstant(a, edRmt2Descriptor, optionBitmaps));

            // if we have bits enabled by default, this actually disables boolean args. fixes a few visual issues
            for (int a = 0; a < edRmt2.BooleanParameterNames.Count; a++)
                newShaderProperty.BooleanConstants |= (1u << a); 
            // newShaderProperty.BooleanConstants |= (GetDefaultValue(CacheContext.StringTable.GetString(edRmt2.BooleanParameterNames[a].Name), edRmt2Descriptor.Type, methodNames, optionBlocks) << a);

            // apply option->option conversion where applicable
            ApplyDefaultOptionFixups(newShaderProperty, originalRm.ShaderProperties[0], blamRmt2Descriptor, edRmt2Descriptor, edRmt2, bmRmt2, renderMethodDefinition);

            // Reorder blam bitmaps to match the HO rmt2 order
            // Reorder blam real constants to match the HO rmt2 order
            // Reorder blam int constants to match the HO rmt2 order
            // Reorder blam bool constants to match the HO rmt2 order
            foreach (var eM in edMaps)
                foreach (var bM in bmMaps)
                    if (eM == bM)
                        newShaderProperty.TextureConstants[edMaps.IndexOf(eM)] = finalRm.ShaderProperties[0].TextureConstants[bmMaps.IndexOf(bM)];

            foreach (var eA in edRealConstants)
                foreach (var bA in bmRealConstants)
                    if (eA == bA)
                        newShaderProperty.RealConstants[edRealConstants.IndexOf(eA)] = finalRm.ShaderProperties[0].RealConstants[bmRealConstants.IndexOf(bA)];

            foreach (var eA in edIntConstants)
                foreach (var bA in bmIntConstants)
                    if (eA == bA)
                        newShaderProperty.IntegerConstants[edIntConstants.IndexOf(eA)] = finalRm.ShaderProperties[0].IntegerConstants[bmIntConstants.IndexOf(bA)];

            foreach (var eA in edBoolConstants)
                foreach (var bA in bmBoolConstants)
                    if (eA == bA)
                    {
                        if ((finalRm.ShaderProperties[0].BooleanConstants & (1u << bmBoolConstants.IndexOf(bA))) == 0)
                            newShaderProperty.BooleanConstants &= ~(1u << edBoolConstants.IndexOf(eA));
                    }

            newShaderProperty.AlphaBlendMode = finalRm.ShaderProperties[0].AlphaBlendMode;
            newShaderProperty.BlendFlags = finalRm.ShaderProperties[0].BlendFlags;

            // in these 3 shaders, alphatesting acts differently than in h3. disabling SW alpha testing works for now (less accurate testing with it off)
            // TODO: fix properly
            if (blamTagName == @"objects\levels\dlc\lockout\shaders\celltower_lights" ||
                blamTagName == @"objects\levels\dlc\lockout\shaders\celltower_lights_blue" ||
                blamTagName == @"levels\dlc\sidewinder\shaders\side_tree_branch_snow")
                newShaderProperty.BlendFlags &= ~BlendModeFlags.EnableAlphaTest;

            // apply post option->options fixups
            ApplyPostOptionFixups(newShaderProperty, originalRm.ShaderProperties[0], blamRmt2Descriptor, edRmt2Descriptor, edRmt2, bmRmt2, renderMethodDefinition);

            finalRm.ImportData = new List<ImportDatum>(); // most likely not used
            finalRm.ShaderProperties[0].Template = edRmt2Instance;
            finalRm.ShaderProperties[0].TextureConstants = newShaderProperty.TextureConstants;
            finalRm.ShaderProperties[0].RealConstants = newShaderProperty.RealConstants;
            finalRm.ShaderProperties[0].IntegerConstants = newShaderProperty.IntegerConstants;
            finalRm.ShaderProperties[0].BooleanConstants = newShaderProperty.BooleanConstants;
            finalRm.ShaderProperties[0].AlphaBlendMode = newShaderProperty.AlphaBlendMode;
            finalRm.ShaderProperties[0].BlendFlags = newShaderProperty.BlendFlags;

            // fixup runtime queryable properties
            for (int i = 0; i < finalRm.ShaderProperties[0].QueryableProperties.Length; i++)
            {
                if (finalRm.ShaderProperties[0].QueryableProperties[i] == -1)
                    continue;

                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 5:
                        finalRm.ShaderProperties[0].QueryableProperties[i] = (short)edMaps.IndexOf(bmMaps[finalRm.ShaderProperties[0].QueryableProperties[i]]);
                        break;
                    case 4:
                        finalRm.ShaderProperties[0].QueryableProperties[i] = (short)edRealConstants.IndexOf(bmRealConstants[finalRm.ShaderProperties[0].QueryableProperties[i]]);
                        break;
                    default:
                        finalRm.ShaderProperties[0].QueryableProperties[i] = -1;
                        break;
                }
            }

            // fixup xform arguments;
            foreach (var tex in finalRm.ShaderProperties[0].TextureConstants)
            {
                if (tex.XFormArgumentIndex != -1)
                    tex.XFormArgumentIndex = (sbyte)edRealConstants.IndexOf(bmRealConstants[tex.XFormArgumentIndex]);
            }

            finalRm.BaseRenderMethod = rmdfInstance;

            // remove emblem animations (prevents crash)
            if (edRmt2Descriptor.Type == "decal" && emblemTagNames.Contains(blamTagName))
            {
                finalRm.ShaderProperties[0].EntryPoints.Clear();
                finalRm.ShaderProperties[0].ParameterTables.Clear();
                finalRm.ShaderProperties[0].Parameters.Clear();
                finalRm.ShaderProperties[0].Functions.Clear();
            }

            // fixup rm animations
            if (finalRm.ShaderProperties[0].Functions.Count > 0)
                RebuildRenderMethodAnimations(cacheStream, blamCacheStream, finalRm, edRmt2, bmRmt2, rmdfInstance, blamRmt2Descriptor.Options, edRmt2Descriptor.Options);

            // build new rm option indices
            finalRm.RenderMethodDefinitionOptionIndices = BuildRenderMethodOptionIndices(edRmt2Descriptor);

            return finalRm;
        }

        private List<Dictionary<RegisterID, int>> MatchPixelRegisters(PixelShader externalPixl, PixelShader basePixl, GlobalPixelShader externalGlps, GlobalPixelShader baseGlps, byte[] bmOptions, byte[] edOptions)
        {
            List<Dictionary<RegisterID, int>> pixelRegisters = new List<Dictionary<RegisterID, int>>();

            for (int entryPoint = 0; entryPoint < externalPixl.EntryPointShaders.Count; entryPoint++)
            {
                Dictionary<RegisterID, int> entryPointRegisters = new Dictionary<RegisterID, int>();

                // glps first
                int bmGlpsIndex = -1;
                int edGlpsIndex = -1;

                if (externalGlps.EntryPoints[entryPoint].Option.Count > 0 && baseGlps.EntryPoints[entryPoint].Option.Count > 0)
                {
                    bmGlpsIndex = externalGlps.EntryPoints[entryPoint].Option[0].OptionMethodShaderIndices[bmOptions[externalGlps.EntryPoints[entryPoint].Option[0].RenderMethodOptionIndex]];
                    edGlpsIndex = baseGlps.EntryPoints[entryPoint].Option[0].OptionMethodShaderIndices[edOptions[baseGlps.EntryPoints[entryPoint].Option[0].RenderMethodOptionIndex]];
                }
                else if (externalGlps.EntryPoints[entryPoint].ShaderIndex > -1 && baseGlps.EntryPoints[entryPoint].ShaderIndex > -1)
                {
                    bmGlpsIndex = externalGlps.EntryPoints[entryPoint].ShaderIndex;
                    edGlpsIndex = baseGlps.EntryPoints[entryPoint].ShaderIndex;
                }

                if (bmGlpsIndex != -1 && edGlpsIndex != -1)
                {
                    foreach (var xboxParameter in externalGlps.Shaders[bmGlpsIndex].XboxParameters)
                    {
                        RegisterID registerID = new RegisterID(xboxParameter.RegisterIndex, xboxParameter.RegisterType);

                        if (entryPointRegisters.ContainsKey(registerID))
                            continue;

                        string xboxParameterName = BlamCache.StringTable.GetString(xboxParameter.ParameterName);

                        foreach (var pcParameter in baseGlps.Shaders[edGlpsIndex].PCParameters)
                        {
                            string pcParameterName = CacheContext.StringTable.GetString(pcParameter.ParameterName);

                            if (xboxParameterName == pcParameterName && xboxParameter.RegisterType == pcParameter.RegisterType)
                            {
                                entryPointRegisters.Add(registerID, pcParameter.RegisterIndex);
                                break;
                            }
                        }
                    }
                }

                // pixl
                for (int i = externalPixl.EntryPointShaders[entryPoint].Offset; i < externalPixl.EntryPointShaders[entryPoint].Offset + externalPixl.EntryPointShaders[entryPoint].Count; i++)
                {
                    foreach (var xboxParameter in externalPixl.Shaders[i].XboxParameters)
                    {
                        RegisterID registerID = new RegisterID(xboxParameter.RegisterIndex, xboxParameter.RegisterType);

                        if (entryPointRegisters.ContainsKey(registerID))
                            continue;

                        string xboxParameterName = BlamCache.StringTable.GetString(xboxParameter.ParameterName);

                        for (int j = basePixl.EntryPointShaders[entryPoint].Offset; j < basePixl.EntryPointShaders[entryPoint].Offset + basePixl.EntryPointShaders[entryPoint].Count; j++)
                        {
                            bool parameterFound = false;

                            foreach (var pcParameter in basePixl.Shaders[j].PCParameters)
                            {
                                string pcParameterName = CacheContext.StringTable.GetString(pcParameter.ParameterName);

                                if (xboxParameterName == pcParameterName && xboxParameter.RegisterType == pcParameter.RegisterType)
                                {
                                    parameterFound = true;
                                    entryPointRegisters.Add(registerID, pcParameter.RegisterIndex);
                                    break;
                                }
                            }

                            if (parameterFound)
                                break;
                        }
                    }
                }

                pixelRegisters.Add(entryPointRegisters);
            }

            return pixelRegisters;
        }

        private List<Dictionary<RegisterID, int>> MatchVertexRegisters(GlobalVertexShader externalGlvs, GlobalVertexShader baseGlvs, List<int> validVertexTypes)
        {
            List<Dictionary<RegisterID, int>> vertexRegisters = new List<Dictionary<RegisterID, int>>();

            int entryPointCount = externalGlvs.VertexTypes[0].DrawModes.Count;

            while (vertexRegisters.Count != entryPointCount)
                vertexRegisters.Add(new Dictionary<RegisterID, int>());

            foreach (var validVertexType in validVertexTypes)
            {
                for (int i = 0; i < entryPointCount; i++)
                {
                    if (externalGlvs.VertexTypes[validVertexType].DrawModes[i].ShaderIndex == -1)
                        continue;

                    foreach (var xboxParameter in externalGlvs.Shaders[externalGlvs.VertexTypes[validVertexType].DrawModes[i].ShaderIndex].XboxParameters)
                    {
                        RegisterID registerID = new RegisterID(xboxParameter.RegisterIndex, xboxParameter.RegisterType);

                        if (vertexRegisters[i].ContainsKey(registerID))
                            continue;

                        string xboxParameterName = BlamCache.StringTable.GetString(xboxParameter.ParameterName);

                        foreach (var pcParameter in baseGlvs.Shaders[baseGlvs.VertexTypes[validVertexType].DrawModes[i].ShaderIndex].PCParameters)
                        {
                            string pcParameterName = CacheContext.StringTable.GetString(pcParameter.ParameterName);

                            if (xboxParameterName == pcParameterName && xboxParameter.RegisterType == pcParameter.RegisterType)
                            {
                                vertexRegisters[i].Add(registerID, pcParameter.RegisterIndex);
                                break;
                            }
                        }
                    }
                }
            }

            return vertexRegisters;
        }

        private bool TableParameterAlreadyExists(RenderMethodTemplate.PackedInteger_10_6 tableInteger, List<ParameterMapping> tableParameters, ParameterMapping parameter)
        {
            for (int i = tableInteger.Offset; i < tableInteger.Offset + tableInteger.Count; i++)
                if (tableParameters[i].RegisterIndex == parameter.RegisterIndex &&
                    tableParameters[i].FunctionIndex == parameter.FunctionIndex &&
                    tableParameters[i].SourceIndex == parameter.SourceIndex)
                    return true;

            return false;
        }

        private void RebuildRenderMethodAnimations(Stream cacheStream, Stream blamCacheStream, RenderMethod finalRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, CachedTag rmdfTag, byte[] bmOptions, byte[] edOptions)
        {
            //
            // Store shader definitions
            //

            CachedTag blamRmdfTag = BlamCache.TagCache.GetTag(rmdfTag.Name.Replace("ms30\\", "") + ".rmdf");

            RenderMethodDefinition blamRmdf = BlamCache.Deserialize<RenderMethodDefinition>(blamCacheStream, blamRmdfTag);
            GlobalVertexShader blamGlvs = BlamCache.Deserialize<GlobalVertexShader>(blamCacheStream, blamRmdf.GlobalVertexShader);
            GlobalPixelShader blamGlps = BlamCache.Deserialize<GlobalPixelShader>(blamCacheStream, blamRmdf.GlobalPixelShader);
            PixelShader blamPixl = BlamCache.Deserialize<PixelShader>(blamCacheStream, bmRmt2.PixelShader);

            RenderMethodDefinition rmdf = CacheContext.Deserialize<RenderMethodDefinition>(cacheStream, rmdfTag);
            GlobalVertexShader glvs = CacheContext.Deserialize<GlobalVertexShader>(cacheStream, rmdf.GlobalVertexShader);
            GlobalPixelShader glps = CacheContext.Deserialize<GlobalPixelShader>(cacheStream, rmdf.GlobalPixelShader);
            PixelShader pixl = CacheContext.Deserialize<PixelShader>(cacheStream, edRmt2.PixelShader);

            // get valid vertex types
            List<int> validVertexTypes = new List<int>();
            foreach (var vertex in blamRmdf.Vertices)
                if (!validVertexTypes.Contains(vertex.VertexType))
                    validVertexTypes.Add(vertex.VertexType);

            // EntryPoints<<external ID, new register>>
            List<Dictionary<RegisterID, int>> vertexRegisters = MatchVertexRegisters(blamGlvs, glvs, validVertexTypes);
            List<Dictionary<RegisterID, int>> pixelRegisters = MatchPixelRegisters(blamPixl, pixl, blamGlps, glps, bmOptions, edOptions);

            // <external index, new index>
            Dictionary<int, int> newRealConstantIndices = new Dictionary<int, int>();
            Dictionary<int, int> newTextureConstantIndices = new Dictionary<int, int>();

            for (int i = 0; i < bmRmt2.RealParameterNames.Count; i++)
                for (int j = 0; j < edRmt2.RealParameterNames.Count; j++)
                    if (BlamCache.StringTable.GetString(bmRmt2.RealParameterNames[i].Name) == CacheContext.StringTable.GetString(edRmt2.RealParameterNames[j].Name))
                    {
                        newRealConstantIndices.Add(i, j);
                        break;
                    }

            for (int i = 0; i < bmRmt2.TextureParameterNames.Count; i++)
                for (int j = 0; j < edRmt2.TextureParameterNames.Count; j++)
                    if (BlamCache.StringTable.GetString(bmRmt2.TextureParameterNames[i].Name) == CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[j].Name))
                    {
                        newTextureConstantIndices.Add(i, j);
                        break;
                    }

            //
            // Collect parameters per entry point
            //

            // EntryPoints<Parameters<RegisterID, FunctionIndex>>
            List<List<ParameterMapping>> tableUsedSamplerParameters = new List<List<ParameterMapping>>();
            List<List<ParameterMapping>> tableUsedVertexParameters = new List<List<ParameterMapping>>();
            List<List<ParameterMapping>> tableUsedPixelParameters = new List<List<ParameterMapping>>();

            foreach (var entryPoint in finalRm.ShaderProperties[0].EntryPoints)
            {
                List<ParameterMapping> usedSamplerParameters = new List<ParameterMapping>();
                List<ParameterMapping> usedVertexParameters = new List<ParameterMapping>();
                List<ParameterMapping> usedPixelParameters = new List<ParameterMapping>();

                for (int tableIndex = entryPoint.Offset; tableIndex < entryPoint.Offset + entryPoint.Count; tableIndex++)
                {
                    var table = finalRm.ShaderProperties[0].ParameterTables[tableIndex];

                    for (int i = table.Texture.Offset; i < table.Texture.Offset + table.Texture.Count; i++)
                        usedSamplerParameters.Add(finalRm.ShaderProperties[0].Parameters[i]);

                    for (int i = table.RealVertex.Offset; i < table.RealVertex.Offset + table.RealVertex.Count; i++)
                        usedVertexParameters.Add(finalRm.ShaderProperties[0].Parameters[i]);

                    for (int i = table.RealPixel.Offset; i < table.RealPixel.Offset + table.RealPixel.Count; i++)
                        usedPixelParameters.Add(finalRm.ShaderProperties[0].Parameters[i]);
                }

                tableUsedSamplerParameters.Add(usedSamplerParameters);
                tableUsedVertexParameters.Add(usedVertexParameters);
                tableUsedPixelParameters.Add(usedPixelParameters);
            }

            //
            // Build parameters
            //

            // collect new entry point indices (saber crap)
            List<int> newEntryPoints = new List<int>();
            switch (rmdfTag.Name)
            {
                // ms30 custom should be here too but their templates werent converted properly...
                case @"shaders\shader":
                case @"ms30\shaders\shader":
                case @"ms30\shaders\halogram":
                    newEntryPoints = new List<int> { 18, 19 }; 
                    break;
                case @"ms30\shaders\water":
                    newEntryPoints = new List<int> { 16, 17, 18 }; 
                    break;
            }

            finalRm.ShaderProperties[0].EntryPoints.Clear();
            finalRm.ShaderProperties[0].ParameterTables.Clear();
            finalRm.ShaderProperties[0].Parameters.Clear();

            while (finalRm.ShaderProperties[0].EntryPoints.Count != edRmt2.EntryPoints.Count)
                finalRm.ShaderProperties[0].EntryPoints.Add(new RenderMethodTemplate.PackedInteger_10_6());
            while (finalRm.ShaderProperties[0].ParameterTables.Count != edRmt2.ParameterTables.Count)
                finalRm.ShaderProperties[0].ParameterTables.Add(new ParameterTable());

            int skipInt = 0;

            for (int epIndex = 0; epIndex < edRmt2.EntryPoints.Count; epIndex++)
            {
                finalRm.ShaderProperties[0].EntryPoints[epIndex] = edRmt2.EntryPoints[epIndex];

                if (newEntryPoints.Contains(epIndex))
                {
                    skipInt++;
                    continue;
                }

                // the loops ran before dont account for new entry points, this corrects the index
                int listIndex = epIndex - skipInt;

                var samplerParameters = tableUsedSamplerParameters[listIndex];
                var vertexParameters = tableUsedVertexParameters[listIndex];
                var pixelParameters = tableUsedPixelParameters[listIndex];

                int tableCount = finalRm.ShaderProperties[0].EntryPoints[epIndex].Offset + finalRm.ShaderProperties[0].EntryPoints[epIndex].Count;

                for (int tableIndex = finalRm.ShaderProperties[0].EntryPoints[epIndex].Offset; tableIndex < tableCount; tableIndex++)
                {
                    if (samplerParameters.Count > 0)
                    {
                        finalRm.ShaderProperties[0].ParameterTables[tableIndex].Texture.Offset = (ushort)finalRm.ShaderProperties[0].Parameters.Count;

                        foreach (var samplerParameter in samplerParameters)
                        {
                            // adjust sampler (pixl) register and source index (texture constant)

                            RegisterID registerID = new RegisterID(samplerParameter.RegisterIndex, ShaderParameter.RType.Sampler);
                            if (!pixelRegisters[epIndex].ContainsKey(registerID) || !newTextureConstantIndices.ContainsKey(samplerParameter.SourceIndex))
                                continue;

                            samplerParameter.RegisterIndex = (short)pixelRegisters[epIndex][registerID];
                            samplerParameter.SourceIndex = (byte)newTextureConstantIndices[samplerParameter.SourceIndex];

                            if (!TableParameterAlreadyExists(finalRm.ShaderProperties[0].ParameterTables[tableIndex].Texture, finalRm.ShaderProperties[0].Parameters, samplerParameter))
                            {
                                finalRm.ShaderProperties[0].Parameters.Add(samplerParameter);
                                finalRm.ShaderProperties[0].ParameterTables[tableIndex].Texture.Count++;
                            }
                        }
                    }

                    if (vertexParameters.Count > 0)
                    {
                        finalRm.ShaderProperties[0].ParameterTables[tableIndex].RealVertex.Offset = (ushort)finalRm.ShaderProperties[0].Parameters.Count;

                        foreach (var vertexParameter in vertexParameters)
                        {
                            // adjust vertex register and source index (real constant)

                            RegisterID registerID = new RegisterID(vertexParameter.RegisterIndex, ShaderParameter.RType.Vector);
                            if (!vertexRegisters[epIndex].ContainsKey(registerID) || !newRealConstantIndices.ContainsKey(vertexParameter.SourceIndex))
                                continue;

                            vertexParameter.RegisterIndex = (short)vertexRegisters[epIndex][registerID];
                            vertexParameter.SourceIndex = (byte)newRealConstantIndices[vertexParameter.SourceIndex];

                            if (!TableParameterAlreadyExists(finalRm.ShaderProperties[0].ParameterTables[tableIndex].RealVertex, finalRm.ShaderProperties[0].Parameters, vertexParameter))
                            {
                                finalRm.ShaderProperties[0].Parameters.Add(vertexParameter);
                                finalRm.ShaderProperties[0].ParameterTables[tableIndex].RealVertex.Count++;
                            }
                        }
                    }

                    if (pixelParameters.Count > 0)
                    {
                        finalRm.ShaderProperties[0].ParameterTables[tableIndex].RealPixel.Offset = (ushort)finalRm.ShaderProperties[0].Parameters.Count;

                        foreach (var pixelParameter in pixelParameters)
                        {
                            // adjust pixel register and source index (real constant)

                            RegisterID registerID = new RegisterID(pixelParameter.RegisterIndex, ShaderParameter.RType.Vector);
                            if (!pixelRegisters[epIndex].ContainsKey(registerID) || !newRealConstantIndices.ContainsKey(pixelParameter.SourceIndex))
                                continue;

                            pixelParameter.RegisterIndex = (short)pixelRegisters[epIndex][registerID];
                            pixelParameter.SourceIndex = (byte)newRealConstantIndices[pixelParameter.SourceIndex];

                            if (!TableParameterAlreadyExists(finalRm.ShaderProperties[0].ParameterTables[tableIndex].RealPixel, finalRm.ShaderProperties[0].Parameters, pixelParameter))
                            {
                                finalRm.ShaderProperties[0].Parameters.Add(pixelParameter);
                                finalRm.ShaderProperties[0].ParameterTables[tableIndex].RealPixel.Count++;
                            }
                        }
                    }
                }
            }
        }

        private TextureConstant GetDefaultTextureConstant(string parameter, ShaderMatcherNew.Rmt2Descriptor rmt2Descriptor, Dictionary<StringId, CachedTag> optionBitmaps)
        {
            TextureConstant textureConstant = new TextureConstant { XFormArgumentIndex = -1 };

            if (rmt2Descriptor.Type == "particle") // not sure what this is but all prt3 have it
                textureConstant.SamplerFlags = 17;

            // hardcoded because the default bitmaps for these parameters produces bad results
            switch (parameter)
            {
                case "alpha_test_map": // the default bitmap for this puts a transparent "ALPHA" all over the shader
                    if (rmt2Descriptor.IsMs30)
                        textureConstant.Bitmap = CacheContext.TagCache.GetTag(@"ms30\shaders\default_bitmaps\bitmaps\default_detail.bitm");
                    else
                        textureConstant.Bitmap = CacheContext.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\default_detail.bitm");
                    return textureConstant;
            }

            // get default bitmap from dictionary
            if (!optionBitmaps.TryGetValue(CacheContext.StringTable.GetStringId(parameter), out textureConstant.Bitmap) || textureConstant.Bitmap == null)
            {
                // fallback
                Console.WriteLine($"WARNING: Sampler parameter \"{parameter}\" has no default bitmap");

                // null bitmap causes bad rendering, use default_detail in these cases
                if (rmt2Descriptor.IsMs30)
                    textureConstant.Bitmap = CacheContext.TagCache.GetTag(@"ms30\shaders\default_bitmaps\bitmaps\default_detail.bitm");
                else
                    textureConstant.Bitmap = CacheContext.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\default_detail.bitm");
            }

            return textureConstant;
        }

        private uint GetDefaultValue(string parameter, string type, List<string> methodNames, Dictionary<StringId, RenderMethodOption.OptionBlock> optionBlocks)
        {
            if (!optionBlocks.TryGetValue(CacheContext.StringTable.GetStringId(parameter), out RenderMethodOption.OptionBlock optionBlock))
            {
                if (!methodNames.Contains(parameter))
                {
                    Console.WriteLine($"WARNING: No type found for {type} parameter \"{parameter}\"");

                    optionBlock = new RenderMethodOption.OptionBlock();
                }
            }

            return optionBlock.DefaultIntBoolArgument;
        }

        private RealConstant GetDefaultRealConstant(string parameter, string type, List<string> methodNames, Dictionary<StringId, RenderMethodOption.OptionBlock> optionBlocks)
        {
            if (!optionBlocks.TryGetValue(CacheContext.StringTable.GetStringId(parameter), out RenderMethodOption.OptionBlock optionBlock))
            {
                // TODO: verify, very rarely some arg names show up
                if (!methodNames.Contains(parameter))
                {
                    Console.WriteLine($"WARNING: No type found for {type} parameter \"{parameter}\"");

                    optionBlock = new RenderMethodOption.OptionBlock
                    {
                        Type = RenderMethodOption.OptionBlock.OptionDataType.Float,
                        DefaultFloatArgument = 0.0f
                    };
                }

                else // particles, contrails, ltvl
                    optionBlock = new RenderMethodOption.OptionBlock
                    {
                        Type = RenderMethodOption.OptionBlock.OptionDataType.Sampler,
                        DefaultBitmapScale = 1.0f
                    };
            }

            if (type == "terrain" && parameter.StartsWith("specular_tint_m_")) // prevent purple terrain
                return new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };

            // use color if one is set
            if (optionBlock.DefaultColor.GetValue() != 0)
                optionBlock.Type = RenderMethodOption.OptionBlock.OptionDataType.IntegerColor;

            // uses 1.0 as default bitmap scale for effect RMs
            if (EffectRenderMethodTypes.Contains(type) && optionBlock.Type == RenderMethodOption.OptionBlock.OptionDataType.Sampler && optionBlock.DefaultBitmapScale == 0.0f)
                optionBlock.DefaultBitmapScale = 1.0f;

            switch (optionBlock.Type)
            {
                case RenderMethodOption.OptionBlock.OptionDataType.Sampler:
                    return new RealConstant { Arg0 = optionBlock.DefaultBitmapScale, Arg1 = optionBlock.DefaultBitmapScale, Arg2 = 0.0f, Arg3 = 0.0f };

                case RenderMethodOption.OptionBlock.OptionDataType.Float:
                case RenderMethodOption.OptionBlock.OptionDataType.Float4:
                    return new RealConstant { Arg0 = optionBlock.DefaultFloatArgument, Arg1 = optionBlock.DefaultFloatArgument, Arg2 = optionBlock.DefaultFloatArgument, Arg3 = optionBlock.DefaultFloatArgument };

                // convert ARGB to RealRGBA
                case RenderMethodOption.OptionBlock.OptionDataType.IntegerColor:
                    return new RealConstant { Arg0 = optionBlock.DefaultColor.Red / 255.0f, Arg1 = optionBlock.DefaultColor.Blue / 255.0f, Arg2 = optionBlock.DefaultColor.Green / 255.0f, Arg3 = optionBlock.DefaultColor.Alpha / 255.0f };

                default:
                    return new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
            }
        }

        private List<RenderMethodDefinitionOptionIndex> BuildRenderMethodOptionIndices(ShaderMatcherNew.Rmt2Descriptor rmt2Descriptor)
        {
            List<RenderMethodDefinitionOptionIndex> newRmIndices = new List<RenderMethodDefinitionOptionIndex>();

            foreach (var option in rmt2Descriptor.Options)
            {
                RenderMethodDefinitionOptionIndex optionIndex = new RenderMethodDefinitionOptionIndex();
                optionIndex.OptionIndex = option;
                newRmIndices.Add(optionIndex);
            }

            return newRmIndices;
        }

        private bool OptionChanged(string rmt2Type, string methodName, out int edOptionIndex, ShaderMatcherNew.Rmt2Descriptor blamRmt2Descriptor, ShaderMatcherNew.Rmt2Descriptor edRmt2Descriptor, RenderMethodDefinition rmdf)
        {
            //
            // This compares the original rmt2 with the matched rmt2 from the matching process and checks if the specified option has changed.
            //

            edOptionIndex = -1;

            if (edRmt2Descriptor.Type == rmt2Type)
            {
                for (int i = 0; i < rmdf.Methods.Count; i++)
                {
                    // find name, and compare the options at that index. maybe need to loop blam rmdf too?
                    if (CacheContext.StringTable.GetString(rmdf.Methods[i].Type) == methodName && blamRmt2Descriptor.Options[i] != edRmt2Descriptor.Options[i])
                    {
                        edOptionIndex = i;
                        return true;
                    }
                }
            }

            return false;
        }

        private void ApplyDefaultOptionFixups(ShaderProperty edShaderProperty, ShaderProperty bmShaderProperty, ShaderMatcherNew.Rmt2Descriptor blamRmt2Descriptor, ShaderMatcherNew.Rmt2Descriptor edRmt2Descriptor, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, RenderMethodDefinition rmdf)
        {
            //
            // This applies manual option->option fixups for matching. This concept should not be used for rm creation/generation.
            //

            // special case, has no options.
            if (blamRmt2Descriptor.Type == "black" || edRmt2Descriptor.Type == "black")
                return;

            // TODO: cleaner way of doing this

            var realConstants = edShaderProperty.RealConstants;
            var textureConstants = edShaderProperty.TextureConstants;
            var bmRealConstants = bmShaderProperty.RealConstants;
            var bmTextureConstants = bmShaderProperty.TextureConstants;

            int optionIndex = -1;

            if (OptionChanged("halogram", "overlay", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if overlay is new to the shader, set its intensity to 0
                if (blamRmt2Descriptor.Options[optionIndex] == 0 && edRmt2Descriptor.Options[optionIndex] != 0)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "overlay_intensity")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                            break;
                        }
                }
                // overlay_detail_map to 0
                for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                    if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "overlay_detail_map")
                    {
                        realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                        break;
                    }
            }
            if (OptionChanged("halogram", "edge_fade", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // prevent purple edge_fade
                if (blamRmt2Descriptor.Options[optionIndex] == 0 && edRmt2Descriptor.Options[optionIndex] == 1)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "edge_fade_center_tint")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 1.0f, Arg1 = 1.0f, Arg2 = 1.0f, Arg3 = 1.0f };
                            break;
                        }
                }
            }
            if (OptionChanged("water", "bankalpha", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if the water shader uses bankalpha.depth when it didnt previously, set the depth to 0 to prevent transparent puddles
                if (blamRmt2Descriptor.Options[optionIndex] != 1 && edRmt2Descriptor.Options[optionIndex] == 1)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "bankalpha_infuence_depth")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                            break;
                        }
                }
            }
            if (OptionChanged("shader", "self_illumination", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if self_illumination is new to the shader, set self_illum_intensity to 0
                if (blamRmt2Descriptor.Options[optionIndex] == 0 && edRmt2Descriptor.Options[optionIndex] != 0)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "self_illum_intensity")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                            break;
                        }
                }
            }
            if (OptionChanged("terrain", "material_3", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if the fourth material is new, set its bitmaps to be that of the first
                if (blamRmt2Descriptor.Options[optionIndex] == 0 && edRmt2Descriptor.Options[optionIndex] != 0)
                {
                    bool baseIsSet = false, detailIsSet = false, bumpIsSet = false, detailBumpIsSet = false;

                    for (int i = 0; i < edRmt2.TextureParameterNames.Count; i++)
                    {
                        if (!baseIsSet && CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name) == "base_map_m_3")
                            for (int j = 0; j < bmRmt2.TextureParameterNames.Count; j++)
                                if (BlamCache.StringTable.GetString(bmRmt2.TextureParameterNames[j].Name) == "base_map_m_0")
                                {
                                    baseIsSet = true;
                                    textureConstants[i].Bitmap = CacheContext.TagCache.GetTag(bmTextureConstants[j].Bitmap.Name + ".bitm");
                                    textureConstants[i].BitmapIndex = bmTextureConstants[j].BitmapIndex;
                                    break;
                                }
                        if (!detailIsSet && CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name) == "detail_map_m_3")
                            for (int j = 0; j < bmRmt2.TextureParameterNames.Count; j++)
                                if (BlamCache.StringTable.GetString(bmRmt2.TextureParameterNames[j].Name) == "detail_map_m_0")
                                {
                                    detailIsSet = true;
                                    textureConstants[i].Bitmap = CacheContext.TagCache.GetTag(bmTextureConstants[j].Bitmap.Name + ".bitm");
                                    textureConstants[i].BitmapIndex = bmTextureConstants[j].BitmapIndex;
                                    break;
                                }
                        if (!bumpIsSet && CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name) == "bump_map_m_3")
                            for (int j = 0; j < bmRmt2.TextureParameterNames.Count; j++)
                                if (BlamCache.StringTable.GetString(bmRmt2.TextureParameterNames[j].Name) == "bump_map_m_0")
                                {
                                    bumpIsSet = true;
                                    textureConstants[i].Bitmap = CacheContext.TagCache.GetTag(bmTextureConstants[j].Bitmap.Name + ".bitm");
                                    textureConstants[i].BitmapIndex = bmTextureConstants[j].BitmapIndex;
                                    break;
                                }
                        if (!detailBumpIsSet && CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name) == "detail_bump_m_3")
                            for (int j = 0; j < bmRmt2.TextureParameterNames.Count; j++)
                                if (BlamCache.StringTable.GetString(bmRmt2.TextureParameterNames[j].Name) == "detail_bump_m_0")
                                {
                                    detailBumpIsSet = true;
                                    textureConstants[i].Bitmap = CacheContext.TagCache.GetTag(bmTextureConstants[j].Bitmap.Name + ".bitm");
                                    textureConstants[i].BitmapIndex = bmTextureConstants[j].BitmapIndex;
                                    break;
                                }
                    }

                    baseIsSet = false; detailIsSet = false; bumpIsSet = false; detailBumpIsSet = false;

                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                    {
                        if (!baseIsSet && CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "base_map_m_3")
                            for (int j = 0; j < bmRmt2.RealParameterNames.Count; j++)
                                if (BlamCache.StringTable.GetString(bmRmt2.RealParameterNames[j].Name) == "base_map_m_0")
                                {
                                    baseIsSet = true;
                                    realConstants[i] = bmRealConstants[j];
                                    break;
                                }
                        if (!detailIsSet && CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "detail_map_m_3")
                            for (int j = 0; j < bmRmt2.RealParameterNames.Count; j++)
                                if (BlamCache.StringTable.GetString(bmRmt2.RealParameterNames[j].Name) == "detail_map_m_0")
                                {
                                    detailIsSet = true;
                                    realConstants[i] = bmRealConstants[j];
                                    break;
                                }
                        if (!bumpIsSet && CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "bump_map_m_3")
                            for (int j = 0; j < bmRmt2.RealParameterNames.Count; j++)
                                if (BlamCache.StringTable.GetString(bmRmt2.RealParameterNames[j].Name) == "bump_map_m_0")
                                {
                                    bumpIsSet = true;
                                    realConstants[i] = bmRealConstants[j];
                                    break;
                                }
                        if (!detailBumpIsSet && CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "detail_bump_m_3")
                            for (int j = 0; j < bmRmt2.RealParameterNames.Count; j++)
                                if (BlamCache.StringTable.GetString(bmRmt2.RealParameterNames[j].Name) == "detail_bump_m_0")
                                {
                                    detailBumpIsSet = true;
                                    realConstants[i] = bmRealConstants[j];
                                    break;
                                }
                    }
                }
            }
            if (OptionChanged("decal", "tinting", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if decal tinting is new, ensure its colour is white
                if (blamRmt2Descriptor.Options[optionIndex] == 0 && edRmt2Descriptor.Options[optionIndex] != 0)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "tint_color")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 1.0f, Arg1 = 1.0f, Arg2 = 1.0f, Arg3 = 1.0f };
                            break;
                        }
                }
            }
        }

        private void ApplyPostOptionFixups(ShaderProperty edShaderProperty, ShaderProperty bmShaderProperty, ShaderMatcherNew.Rmt2Descriptor blamRmt2Descriptor, ShaderMatcherNew.Rmt2Descriptor edRmt2Descriptor, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, RenderMethodDefinition rmdf)
        {
            //
            // This applies manual option->option fixups for matching. This concept should not be used for rm creation/generation.
            //

            // special case, has no options.
            if (blamRmt2Descriptor.Type == "black" || edRmt2Descriptor.Type == "black")
                return;

            // TODO: cleaner way of doing this

            var realConstants = edShaderProperty.RealConstants;
            var textureConstants = edShaderProperty.TextureConstants;
            var bmRealConstants = bmShaderProperty.RealConstants;
            var bmTextureConstants = bmShaderProperty.TextureConstants;

            int optionIndex = -1;

            if (OptionChanged("shader", "specular_mask", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if specular mask/texture is no longer used, 0 the specular coefficients (prevents blinding/bad env reflections)
                if (blamRmt2Descriptor.Options[optionIndex] > 1 && edRmt2Descriptor.Options[optionIndex] == 0)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "specular_coefficient")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                            break;
                        }
                }
            }
        }
    }
}