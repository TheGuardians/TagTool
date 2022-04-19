using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.Collections.Generic;
using System.IO;
using TagTool.Shaders;
using TagTool.Shaders.ShaderMatching;
using System;
using System.Linq;
using TagTool.Shaders.ShaderFunctions;
using static TagTool.Tags.Definitions.RenderMethod;
using static TagTool.Tags.Definitions.RenderMethod.RenderMethodPostprocessBlock;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public ShaderMatcherNew Matcher = new ShaderMatcherNew();

        private List<string> emblemTagNames = new List<string>
        {
            @"objects\characters\odst\shaders\mc_emblem",
            @"objects\characters\odst_cine\shaders\mc_emblem",
            @"objects\characters\odst_oni_op\shaders\mc_emblem",
            @"objects\characters\elite\shaders\elite_emblem",
            @"objects\characters\masterchief\shaders\mc_emblem"
        };

        private readonly List<string> RealConstantToBoolConstant = new List<string>
        {
            "use_material_texture",
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
                rasg.MotionBlurParametersLegacy.NumberOfTaps = 6;
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
                case ShaderGlass rmgl:
                case ShaderScreen rmss:
                case ShaderZonly rmzo:
                case ShaderCortana rmct:
                    return ConvertShaderInternal(cacheStream, blamCacheStream, (RenderMethod)definition, blamTag, (RenderMethod)blamDefinition);

                case ContrailSystem cntl:
                    var blamCntl = (ContrailSystem)blamDefinition;
                    for (int i = 0; i < cntl.Contrails.Count; i++)
                    {
                        cntl.Contrails[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, cntl.Contrails[i].RenderMethod, blamTag, blamCntl.Contrails[i].RenderMethod);
                        if (cntl.Contrails[i].RenderMethod == null) return null;
                    }
                    return cntl;

                case Particle prt3:
                    var blamPrt3 = (Particle)blamDefinition;
                    prt3.RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, prt3.RenderMethod, blamTag, blamPrt3.RenderMethod);
                    if (prt3.RenderMethod == null) return null;
                    return prt3;

                case LightVolumeSystem ltvl:
                    var blamLtvl = (LightVolumeSystem)blamDefinition;
                    for (int i = 0; i < ltvl.LightVolumes.Count; i++)
                    {
                        ltvl.LightVolumes[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, ltvl.LightVolumes[i].RenderMethod, blamTag, blamLtvl.LightVolumes[i].RenderMethod);
                        if (ltvl.LightVolumes[i].RenderMethod == null) return null;
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
                    for (int i = 0; i < beamSystem.Beams.Count; i++)
                    {
                        beamSystem.Beams[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, beamSystem.Beams[i].RenderMethod, blamTag, blamBeam.Beams[i].RenderMethod);
                        if (beamSystem.Beams[i].RenderMethod == null) return null;
                    }
                    return beamSystem;
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

            return ConvertRenderMethod(cacheStream, blamCacheStream, definition, blamDefinition, blamShaderProperty.Template, blamTag);
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
                case "rmgl" when CacheContext.TagCache.TryGetTag(@"levels\dlc\sidewinder\shaders\side_hall_glass03", out shaderTag):
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

            return Matcher.FindClosestTemplate(blamRmt2, BlamCache.Deserialize<RenderMethodTemplate>(blamCacheStream, blamRmt2), FlagIsSet(PortingFlags.GenerateShaders));
        }

        private RenderMethod ConvertRenderMethod(Stream cacheStream, Stream blamCacheStream, RenderMethod finalRm, RenderMethod blamRm, CachedTag blamRmt2, CachedTag blamTag)
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
            var newShaderProperty = new RenderMethod.RenderMethodPostprocessBlock
            {
                TextureConstants = new List<TextureConstant>(),
                RealConstants = new List<RealConstant>(),
                IntegerConstants = new List<uint>(),
                BooleanConstants = 0
            };

            RenderMethod originalRm = finalRm;

            // convert filter mode
            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                foreach (var textureConstant in finalRm.ShaderProperties[0].TextureConstants)
                    textureConstant.FilterMode = textureConstant.FilterModeReach.FilterMode;
            }
            else if (BlamCache.Version == CacheVersion.Halo3ODST)
            {
                foreach (var textureConstant in finalRm.ShaderProperties[0].TextureConstants)
                    textureConstant.FilterMode = textureConstant.FilterModeODST.FilterMode;
            }
            else if (BlamCache.Version <= CacheVersion.Halo3Retail)
            {
                foreach (var textureConstant in finalRm.ShaderProperties[0].TextureConstants)
                    textureConstant.FilterMode = textureConstant.FilterModeH3;
            }

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

            // get relevant rmdf
            CachedTag rmdfInstance = Matcher.FindRmdf(edRmt2Instance);
            if (rmdfInstance == null) // shader matching will fail without an rmdf -- throw an exception
                throw new Exception($"Unable to find valid \"{edRmt2Descriptor.Type}\" rmdf for rmt2");

            finalRm.BaseRenderMethod = rmdfInstance;

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

            RenderMethodDefinition renderMethodDefinition = CacheContext.Deserialize<RenderMethodDefinition>(cacheStream, rmdfInstance);

            // dictionaries for fast lookup
            //var optionParameters = Matcher.GetOptionParameters(rmt2Descriptor.Options.ToList(), renderMethodDefinition);
            var optionBlocks = Matcher.GetOptionBlocks(edRmt2Descriptor.Options.ToList(), renderMethodDefinition);
            var optionBitmaps = Matcher.GetOptionBitmaps(edRmt2Descriptor.Options.ToList(), renderMethodDefinition);

            List<string> methodNames = new List<string>();
            foreach (var method in renderMethodDefinition.Categories)
                methodNames.Add(CacheContext.StringTable.GetString(method.Name));

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
            {
                if (RealConstantToBoolConstant.Contains(eA) && bmRealConstants.Contains(eA))
                {
                    if (finalRm.ShaderProperties[0].RealConstants[bmRealConstants.IndexOf(eA)].Arg0 == 0.0f)
                        newShaderProperty.BooleanConstants &= ~(1u << edBoolConstants.IndexOf(eA));
                }
                else
                {
                    foreach (var bA in bmBoolConstants)
                        if (eA == bA)
                        {
                            if ((finalRm.ShaderProperties[0].BooleanConstants & (1u << bmBoolConstants.IndexOf(bA))) == 0)
                                newShaderProperty.BooleanConstants &= ~(1u << edBoolConstants.IndexOf(eA));
                        }
                }
            }

            newShaderProperty.BlendMode = finalRm.ShaderProperties[0].BlendMode;
            newShaderProperty.Flags = finalRm.ShaderProperties[0].Flags;

            // in these shaders alphatesting acts differently than in h3. disabling\enabling SW alpha testing works for now
            // TODO: fix properly
            if (blamTag.Name == @"objects\levels\dlc\lockout\shaders\celltower_lights" ||
                blamTag.Name == @"objects\levels\dlc\lockout\shaders\celltower_lights_blue" ||
                blamTag.Name == @"levels\dlc\sidewinder\shaders\side_tree_branch_snow")
                newShaderProperty.Flags &= ~RenderMethodPostprocessFlags.EnableAlphaTest;
            if (blamTag.Name == @"levels\atlas\sc110\shaders\tree_leaves_acacia" ||
                (blamTag.Name == @"levels\solo\030_outskirts\shaders\outtree_leaf" && BlamCache.Version == CacheVersion.Halo3ODST)) // might be in h3 too, remove version if so
                newShaderProperty.Flags |= RenderMethodPostprocessFlags.EnableAlphaTest;

            // apply post option->options fixups
            ApplyPostOptionFixups(newShaderProperty, originalRm.ShaderProperties[0], blamRmt2Descriptor, edRmt2Descriptor, edRmt2, bmRmt2, renderMethodDefinition);

            finalRm.Parameters = new List<RenderMethodParameterBlock>(); // most likely not used
            finalRm.ShaderProperties[0].Template = edRmt2Instance;
            finalRm.ShaderProperties[0].TextureConstants = newShaderProperty.TextureConstants;
            finalRm.ShaderProperties[0].RealConstants = newShaderProperty.RealConstants;
            finalRm.ShaderProperties[0].IntegerConstants = newShaderProperty.IntegerConstants;
            finalRm.ShaderProperties[0].BooleanConstants = newShaderProperty.BooleanConstants;
            finalRm.ShaderProperties[0].BlendMode = newShaderProperty.BlendMode;
            finalRm.ShaderProperties[0].Flags = newShaderProperty.Flags;

            // fixup runtime queryable properties
            if (BlamCache.Version < CacheVersion.HaloReach)
            {
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
            }
            else if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                finalRm.ShaderProperties[0].QueryableProperties = new short[8];

                for (int i = 0; i < finalRm.ShaderProperties[0].QueryableProperties.Length; i++)
                {
                    if (finalRm.ShaderProperties[0].QueryablePropertiesReach[i] == -1)
                    {
                        finalRm.ShaderProperties[0].QueryableProperties[i] = -1;
                        continue;
                    }

                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 5:
                            finalRm.ShaderProperties[0].QueryableProperties[i] = (short)edMaps.IndexOf(bmMaps[finalRm.ShaderProperties[0].QueryablePropertiesReach[i]]);
                            break;
                        case 4:
                            finalRm.ShaderProperties[0].QueryableProperties[i] = (short)edRealConstants.IndexOf(bmRealConstants[finalRm.ShaderProperties[0].QueryablePropertiesReach[i]]);
                            break;
                        default:
                            finalRm.ShaderProperties[0].QueryableProperties[i] = -1;
                            break;
                    }
                }
            }

            // fixup xform arguments;
            foreach (var tex in finalRm.ShaderProperties[0].TextureConstants)
            {
                if (tex.TextureTransformConstantIndex != -1)
                    tex.TextureTransformConstantIndex = (sbyte)edRealConstants.IndexOf(bmRealConstants[tex.TextureTransformConstantIndex]);
            }

            // fixup rm animations
            if (finalRm.ShaderProperties[0].Functions.Count > 0)
            {
                RebuildRenderMethodAnimationsFromRmt2(finalRm, blamRm, edRmt2, bmRmt2);
            }

            // build new rm option indices
            finalRm.Options = BuildRenderMethodOptionIndices(edRmt2Descriptor);

            return finalRm;
        }

        private List<Dictionary<RegisterID, int>> MatchPixelRegisters(PixelShader externalPixl, PixelShader basePixl, GlobalPixelShader externalGlps, GlobalPixelShader baseGlps, byte[] bmOptions, byte[] edOptions)
        {
            List<Dictionary<RegisterID, int>> pixelRegisters = new List<Dictionary<RegisterID, int>>();

            for (int entryPoint = 0; entryPoint < externalPixl.EntryPointShaders.Count; entryPoint++)
            {
                Dictionary<RegisterID, int> entryPointRegisters = new Dictionary<RegisterID, int>();

                if (basePixl.EntryPointShaders.Count <= entryPoint || baseGlps.EntryPoints.Count <= entryPoint)
                {
                    new TagToolWarning($"Pixel entrypoint does not match up with external ({((EntryPoint)entryPoint).ToString()})");
                    break;
                }

                // glps first
                int bmGlpsIndex = -1;
                int edGlpsIndex = -1;

                if (externalGlps.EntryPoints[entryPoint].CategoryDependency.Count > 0 && baseGlps.EntryPoints[entryPoint].CategoryDependency.Count > 0)
                {
                    bmGlpsIndex = externalGlps.EntryPoints[entryPoint].CategoryDependency[0].OptionDependency[bmOptions[externalGlps.EntryPoints[entryPoint].CategoryDependency[0].DefinitionCategoryIndex]].CompiledShaderIndex;
                    edGlpsIndex = baseGlps.EntryPoints[entryPoint].CategoryDependency[0].OptionDependency[edOptions[baseGlps.EntryPoints[entryPoint].CategoryDependency[0].DefinitionCategoryIndex]].CompiledShaderIndex;
                }
                else if (externalGlps.EntryPoints[entryPoint].DefaultCompiledShaderIndex > -1 && baseGlps.EntryPoints[entryPoint].DefaultCompiledShaderIndex > -1)
                {
                    bmGlpsIndex = externalGlps.EntryPoints[entryPoint].DefaultCompiledShaderIndex;
                    edGlpsIndex = baseGlps.EntryPoints[entryPoint].DefaultCompiledShaderIndex;
                }

                if (bmGlpsIndex != -1 && edGlpsIndex != -1)
                {
                    foreach (var xboxParameter in externalGlps.Shaders[bmGlpsIndex].GetConstantTable(BlamCache.Version, BlamCache.Platform).Constants)
                    {
                        RegisterID registerID = new RegisterID(xboxParameter.RegisterIndex, xboxParameter.RegisterType);

                        if (entryPointRegisters.ContainsKey(registerID))
                            continue;

                        string xboxParameterName = BlamCache.StringTable.GetString(xboxParameter.ParameterName);

                        foreach (var pcParameter in baseGlps.Shaders[edGlpsIndex].GetConstantTable(CacheContext.Version, CacheContext.Platform).Constants)
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
                    foreach (var xboxParameter in externalPixl.Shaders[i].GetConstantTable(BlamCache.Version, BlamCache.Platform).Constants)
                    {
                        RegisterID registerID = new RegisterID(xboxParameter.RegisterIndex, xboxParameter.RegisterType);

                        if (entryPointRegisters.ContainsKey(registerID))
                            continue;

                        string xboxParameterName = BlamCache.StringTable.GetString(xboxParameter.ParameterName);

                        for (int j = basePixl.EntryPointShaders[entryPoint].Offset; j < basePixl.EntryPointShaders[entryPoint].Offset + basePixl.EntryPointShaders[entryPoint].Count; j++)
                        {
                            bool parameterFound = false;

                            foreach (var pcParameter in basePixl.Shaders[j].GetConstantTable(CacheContext.Version, CacheContext.Platform).Constants)
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

            int entryPointCount = externalGlvs.VertexTypes[0].EntryPoints.Count;

            while (vertexRegisters.Count != entryPointCount)
                vertexRegisters.Add(new Dictionary<RegisterID, int>());

            foreach (var validVertexType in validVertexTypes)
            {
                for (int i = 0; i < entryPointCount; i++)
                {
                    if (externalGlvs.VertexTypes[validVertexType].EntryPoints[i].ShaderIndex == -1)
                        continue;
                    if (baseGlvs.VertexTypes[validVertexType].EntryPoints[i].ShaderIndex == -1)
                    {
                        new TagToolWarning($"Invalid vertex shader index \"{((TagTool.Geometry.VertexType)validVertexType).ToString()}, {((EntryPoint)i).ToString()}\"");
                        continue;
                    }

                    foreach (var xboxParameter in externalGlvs.Shaders[externalGlvs.VertexTypes[validVertexType].EntryPoints[i].ShaderIndex].GetConstantTable(BlamCache.Version, BlamCache.Platform).Constants)
                    {
                        RegisterID registerID = new RegisterID(xboxParameter.RegisterIndex, xboxParameter.RegisterType);

                        if (vertexRegisters[i].ContainsKey(registerID))
                            continue;

                        string xboxParameterName = BlamCache.StringTable.GetString(xboxParameter.ParameterName);

                        foreach (var pcParameter in baseGlvs.Shaders[baseGlvs.VertexTypes[validVertexType].EntryPoints[i].ShaderIndex].GetConstantTable(CacheContext.Version, CacheContext.Platform).Constants)
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

        private bool TableParameterAlreadyExists(RenderMethodTemplate.TagBlockIndex tableInteger, List<RenderMethodRoutingInfoBlock> tableParameters, RenderMethodRoutingInfoBlock parameter)
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
            foreach (var vertex in blamRmdf.VertexTypes)
                if (!validVertexTypes.Contains((int)vertex.VertexType))
                    validVertexTypes.Add((int)vertex.VertexType);

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
            List<List<RenderMethodRoutingInfoBlock>> tableUsedSamplerParameters = new List<List<RenderMethodRoutingInfoBlock>>();
            List<List<RenderMethodRoutingInfoBlock>> tableUsedVertexParameters = new List<List<RenderMethodRoutingInfoBlock>>();
            List<List<RenderMethodRoutingInfoBlock>> tableUsedPixelParameters = new List<List<RenderMethodRoutingInfoBlock>>();

            foreach (var entryPoint in finalRm.ShaderProperties[0].EntryPoints)
            {
                List<RenderMethodRoutingInfoBlock> usedSamplerParameters = new List<RenderMethodRoutingInfoBlock>();
                List<RenderMethodRoutingInfoBlock> usedVertexParameters = new List<RenderMethodRoutingInfoBlock>();
                List<RenderMethodRoutingInfoBlock> usedPixelParameters = new List<RenderMethodRoutingInfoBlock>();

                for (int tableIndex = entryPoint.Offset; tableIndex < entryPoint.Offset + entryPoint.Count; tableIndex++)
                {
                    var table = finalRm.ShaderProperties[0].Passes[tableIndex];

                    for (int i = table.Texture.Offset; i < table.Texture.Offset + table.Texture.Count; i++)
                        usedSamplerParameters.Add(finalRm.ShaderProperties[0].RoutingInfo[i]);

                    for (int i = table.RealVertex.Offset; i < table.RealVertex.Offset + table.RealVertex.Count; i++)
                        usedVertexParameters.Add(finalRm.ShaderProperties[0].RoutingInfo[i]);

                    for (int i = table.RealPixel.Offset; i < table.RealPixel.Offset + table.RealPixel.Count; i++)
                        usedPixelParameters.Add(finalRm.ShaderProperties[0].RoutingInfo[i]);
                }

                tableUsedSamplerParameters.Add(usedSamplerParameters);
                tableUsedVertexParameters.Add(usedVertexParameters);
                tableUsedPixelParameters.Add(usedPixelParameters);
            }

            //
            // Build parameters
            //

            // TODO: fix this code with new knowledge of ms30 entry points

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
            finalRm.ShaderProperties[0].Passes.Clear();
            finalRm.ShaderProperties[0].RoutingInfo.Clear();

            while (finalRm.ShaderProperties[0].EntryPoints.Count != edRmt2.EntryPoints.Count)
                finalRm.ShaderProperties[0].EntryPoints.Add(new RenderMethodTemplate.TagBlockIndex());
            while (finalRm.ShaderProperties[0].Passes.Count != edRmt2.Passes.Count)
                finalRm.ShaderProperties[0].Passes.Add(new RenderMethodPostprocessPassBlock());

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
                        finalRm.ShaderProperties[0].Passes[tableIndex].Texture.Offset = (ushort)finalRm.ShaderProperties[0].RoutingInfo.Count;

                        foreach (var samplerParameter in samplerParameters)
                        {
                            // adjust sampler (pixl) register and source index (texture constant)

                            RegisterID registerID = new RegisterID(samplerParameter.RegisterIndex, ShaderParameter.RType.Sampler);
                            if (!pixelRegisters[epIndex].ContainsKey(registerID) || !newTextureConstantIndices.ContainsKey(samplerParameter.SourceIndex))
                                continue;

                            samplerParameter.RegisterIndex = (short)pixelRegisters[epIndex][registerID];
                            samplerParameter.SourceIndex = (byte)newTextureConstantIndices[samplerParameter.SourceIndex];

                            if (!TableParameterAlreadyExists(finalRm.ShaderProperties[0].Passes[tableIndex].Texture, finalRm.ShaderProperties[0].RoutingInfo, samplerParameter))
                            {
                                finalRm.ShaderProperties[0].RoutingInfo.Add(samplerParameter);
                                finalRm.ShaderProperties[0].Passes[tableIndex].Texture.Count++;
                            }
                        }
                    }

                    if (vertexParameters.Count > 0)
                    {
                        finalRm.ShaderProperties[0].Passes[tableIndex].RealVertex.Offset = (ushort)finalRm.ShaderProperties[0].RoutingInfo.Count;

                        foreach (var vertexParameter in vertexParameters)
                        {
                            // adjust vertex register and source index (real constant)

                            RegisterID registerID = new RegisterID(vertexParameter.RegisterIndex, ShaderParameter.RType.Vector);
                            if (!vertexRegisters[epIndex].ContainsKey(registerID) || !newRealConstantIndices.ContainsKey(vertexParameter.SourceIndex))
                                continue;

                            vertexParameter.RegisterIndex = (short)vertexRegisters[epIndex][registerID];
                            vertexParameter.SourceIndex = (byte)newRealConstantIndices[vertexParameter.SourceIndex];

                            if (!TableParameterAlreadyExists(finalRm.ShaderProperties[0].Passes[tableIndex].RealVertex, finalRm.ShaderProperties[0].RoutingInfo, vertexParameter))
                            {
                                finalRm.ShaderProperties[0].RoutingInfo.Add(vertexParameter);
                                finalRm.ShaderProperties[0].Passes[tableIndex].RealVertex.Count++;
                            }
                        }
                    }

                    if (pixelParameters.Count > 0)
                    {
                        finalRm.ShaderProperties[0].Passes[tableIndex].RealPixel.Offset = (ushort)finalRm.ShaderProperties[0].RoutingInfo.Count;

                        foreach (var pixelParameter in pixelParameters)
                        {
                            // adjust pixel register and source index (real constant)

                            RegisterID registerID = new RegisterID(pixelParameter.RegisterIndex, ShaderParameter.RType.Vector);
                            if (!pixelRegisters[epIndex].ContainsKey(registerID) || !newRealConstantIndices.ContainsKey(pixelParameter.SourceIndex))
                                continue;

                            pixelParameter.RegisterIndex = (short)pixelRegisters[epIndex][registerID];
                            pixelParameter.SourceIndex = (byte)newRealConstantIndices[pixelParameter.SourceIndex];

                            if (!TableParameterAlreadyExists(finalRm.ShaderProperties[0].Passes[tableIndex].RealPixel, finalRm.ShaderProperties[0].RoutingInfo, pixelParameter))
                            {
                                finalRm.ShaderProperties[0].RoutingInfo.Add(pixelParameter);
                                finalRm.ShaderProperties[0].Passes[tableIndex].RealPixel.Count++;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Same as RebuildRenderMethodAnimations(), however matches parameters via rmt2 instead of register info.
        /// </summary>
        private void RebuildRenderMethodAnimationsFromRmt2(RenderMethod finalRm, RenderMethod blamRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2)
        {
            var animatedParameters = ShaderFunctionHelper.GetAnimatedParameters(BlamCache, blamRm, bmRmt2);

            ShaderFunctionHelper.BuildAnimatedParameters(CacheContext, finalRm, edRmt2, animatedParameters);
        }

        private TextureConstant GetDefaultTextureConstant(string parameter, ShaderMatcherNew.Rmt2Descriptor rmt2Descriptor, Dictionary<StringId, CachedTag> optionBitmaps)
        {
            TextureConstant textureConstant = new TextureConstant { TextureTransformConstantIndex = -1 };

            if (rmt2Descriptor.Type == "particle")
            {
                textureConstant.SamplerAddressMode = new TextureConstant.PackedSamplerAddressMode
                {
                    AddressU = TextureConstant.SamplerAddressModeEnum.Clamp,
                    AddressV = TextureConstant.SamplerAddressModeEnum.Clamp
                };
            }

            // get default bitmap from dictionary
            if (!optionBitmaps.TryGetValue(CacheContext.StringTable.GetStringId(parameter), out textureConstant.Bitmap) || textureConstant.Bitmap == null)
            {
                // null bitmap causes bad rendering, use default_detail in these cases
                if (rmt2Descriptor.IsMs30)
                    textureConstant.Bitmap = CacheContext.TagCache.GetTag(@"ms30\shaders\default_bitmaps\bitmaps\default_detail.bitm");
                else
                    textureConstant.Bitmap = CacheContext.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\default_detail.bitm");
            }

            return textureConstant;
        }

        private uint GetDefaultValue(string parameter, string type, List<string> methodNames, Dictionary<StringId, RenderMethodOption.ParameterBlock> optionBlocks)
        {
            if (!optionBlocks.TryGetValue(CacheContext.StringTable.GetStringId(parameter), out RenderMethodOption.ParameterBlock optionBlock))
            {
                if (!methodNames.Contains(parameter))
                {
                    new TagToolWarning($"No type found for {type} parameter \"{parameter}\"");

                    optionBlock = new RenderMethodOption.ParameterBlock();
                }
            }

            return optionBlock.DefaultIntBoolArgument;
        }

        private RealConstant GetDefaultRealConstant(string parameter, string type, List<string> methodNames, Dictionary<StringId, RenderMethodOption.ParameterBlock> optionBlocks)
        {
            if (!optionBlocks.TryGetValue(CacheContext.StringTable.GetStringId(parameter), out RenderMethodOption.ParameterBlock optionBlock))
            {
                // TODO: verify, very rarely some arg names show up
                if (!methodNames.Contains(parameter))
                {
                    new TagToolWarning($"No type found for {type} parameter \"{parameter}\"");

                    optionBlock = new RenderMethodOption.ParameterBlock
                    {
                        Type = RenderMethodOption.ParameterBlock.OptionDataType.Real,
                        DefaultFloatArgument = 0.0f
                    };
                }

                else // particles, contrails, ltvl
                    optionBlock = new RenderMethodOption.ParameterBlock
                    {
                        Type = RenderMethodOption.ParameterBlock.OptionDataType.Bitmap,
                        DefaultBitmapScale = 1.0f
                    };
            }

            if (type == "terrain" && parameter.StartsWith("specular_tint_m_")) // prevent purple terrain
                return new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };

            // use color if one is set
            if (optionBlock.DefaultColor.GetValue() != 0)
                optionBlock.Type = RenderMethodOption.ParameterBlock.OptionDataType.ArgbColor;

            // uses 1.0 as default bitmap scale for effect RMs
            if (EffectRenderMethodTypes.Contains(type) && optionBlock.Type == RenderMethodOption.ParameterBlock.OptionDataType.Bitmap && optionBlock.DefaultBitmapScale == 0.0f)
                optionBlock.DefaultBitmapScale = 1.0f;

            switch (optionBlock.Type)
            {
                case RenderMethodOption.ParameterBlock.OptionDataType.Bitmap:
                    return new RealConstant { Arg0 = optionBlock.DefaultBitmapScale, Arg1 = optionBlock.DefaultBitmapScale, Arg2 = 0.0f, Arg3 = 0.0f };

                case RenderMethodOption.ParameterBlock.OptionDataType.Real:
                case RenderMethodOption.ParameterBlock.OptionDataType.Color:
                    return new RealConstant { Arg0 = optionBlock.DefaultFloatArgument, Arg1 = optionBlock.DefaultFloatArgument, Arg2 = optionBlock.DefaultFloatArgument, Arg3 = optionBlock.DefaultFloatArgument };

                // convert ARGB to RealRGBA
                case RenderMethodOption.ParameterBlock.OptionDataType.ArgbColor:
                    return new RealConstant { Arg0 = optionBlock.DefaultColor.Red / 255.0f, Arg1 = optionBlock.DefaultColor.Blue / 255.0f, Arg2 = optionBlock.DefaultColor.Green / 255.0f, Arg3 = optionBlock.DefaultColor.Alpha / 255.0f };

                default:
                    return new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
            }
        }

        private List<RenderMethodOptionIndex> BuildRenderMethodOptionIndices(ShaderMatcherNew.Rmt2Descriptor rmt2Descriptor)
        {
            List<RenderMethodOptionIndex> newRmIndices = new List<RenderMethodOptionIndex>();

            foreach (var option in rmt2Descriptor.Options)
            {
                RenderMethodOptionIndex optionIndex = new RenderMethodOptionIndex();
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
                for (int i = 0; i < rmdf.Categories.Count; i++)
                {
                    // find name, and compare the options at that index. maybe need to loop blam rmdf too?
                    if (CacheContext.StringTable.GetString(rmdf.Categories[i].Name) == methodName && blamRmt2Descriptor.Options[i] != edRmt2Descriptor.Options[i])
                    {
                        edOptionIndex = i;
                        return true;
                    }
                }
            }

            return false;
        }

        private void ApplyDefaultOptionFixups(RenderMethodPostprocessBlock edShaderProperty, RenderMethodPostprocessBlock bmShaderProperty, ShaderMatcherNew.Rmt2Descriptor blamRmt2Descriptor, ShaderMatcherNew.Rmt2Descriptor edRmt2Descriptor, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, RenderMethodDefinition rmdf)
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
            if (OptionChanged("terrain", "material_3", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf) && BlamCache.Version < CacheVersion.HaloReach)
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
            if (OptionChanged("shader", "parallax", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                if (blamRmt2Descriptor.Options[optionIndex] == 0 && edRmt2Descriptor.Options[optionIndex] != 0)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                    {
                        // prevent "smoothing"
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "height_map")
                            realConstants[i] = new RealConstant { Arg0 = 1.0f, Arg1 = 1.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "height_scale")
                            realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                    }
                }
            }
        }

        private void ApplyPostOptionFixups(RenderMethodPostprocessBlock edShaderProperty, RenderMethodPostprocessBlock bmShaderProperty, ShaderMatcherNew.Rmt2Descriptor blamRmt2Descriptor, ShaderMatcherNew.Rmt2Descriptor edRmt2Descriptor, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, RenderMethodDefinition rmdf)
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