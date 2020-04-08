using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.Collections.Generic;
using System.IO;
using TagTool.Serialization;
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

                case ShaderBlack rmbk:
                    return CreateShaderBlack(cacheStream, blamCacheStream, rmbk, blamTag, (RenderMethod)blamDefinition);

                case ShaderScreen rmss:
                case ShaderZonly rmzo:
                case ShaderCortana rmct:
                    return null;
            }
            return null;
        }

        // temp until generation is possible
        private RenderMethod CreateShaderBlack(Stream cacheStream, Stream blamCacheStream, ShaderBlack rmbk, CachedTag blamTag, RenderMethod blamDefinition)
        {
            // use default template - we only need albedo_color for this to work
            CachedTag defaultTemplateInstance = CacheContext.GetTag(ShaderMatcherNew.DefaultTemplate);
            rmbk.BaseRenderMethod = Matcher.FindRmdf(defaultTemplateInstance);

            rmbk.ShaderProperties[0].Template = defaultTemplateInstance;

            // reset rm options
            rmbk.RenderMethodDefinitionOptionIndices.Clear();
            for (int i = 0; i < 11; i++)
                rmbk.RenderMethodDefinitionOptionIndices.Add(new RenderMethodDefinitionOptionIndex());

            // setup default values
            RenderMethod result = ConvertShaderInternal(cacheStream, blamCacheStream, rmbk, blamTag, blamDefinition);

            // set albedo_color to black
            result.ShaderProperties[0].RealConstants[2].Arg0 = 0.0f;
            result.ShaderProperties[0].RealConstants[2].Arg1 = 0.0f;
            result.ShaderProperties[0].RealConstants[2].Arg2 = 0.0f;
            // enable dynamic lights
            result.ShaderProperties[0].BooleanConstants = 0;

            return result;
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

        private CachedTag GetDefaultShader(Tag groupTag)
        {
            switch (groupTag.ToString())
            {
                case "beam":
                    return CacheContext.GetTag<BeamSystem>(@"objects\weapons\support_high\spartan_laser\fx\firing_3p");

                case "cntl":
                    return CacheContext.GetTag<ContrailSystem>(@"objects\weapons\pistol\needler\fx\projectile");

                case "decs":
                    return CacheContext.GetTag<DecalSystem>(@"fx\decals\impact_plasma\impact_plasma_medium\hard");

                case "ltvl":
                    return CacheContext.GetTag<LightVolumeSystem>(@"objects\weapons\pistol\plasma_pistol\fx\charged\projectile");

                case "prt3":
                    return CacheContext.GetTag<Particle>(@"fx\particles\energy\sparks\impact_spark_orange");

                case "rmd ":
                    return CacheContext.GetTag<ShaderDecal>(@"objects\gear\human\military\shaders\human_military_decals");

                case "rmfl":
                    return CacheContext.GetTag<ShaderFoliage>(@"levels\multi\riverworld\shaders\riverworld_tree_leafa");

                case "rmtr":
                    return CacheContext.GetTag<ShaderTerrain>(@"levels\multi\riverworld\shaders\riverworld_ground");

                case "rmw ":
                    return CacheContext.GetTag<ShaderWater>(@"levels\multi\riverworld\shaders\riverworld_water_rough");

                case "rmhg":
                    return CacheContext.GetTag<ShaderHalogram>(@"objects\multi\shaders\koth_shield");

                case "rmbk": // hackfix
                    return CacheContext.GetTag<Shader>(@"levels\dlc\bunkerworld\shaders\z_black");

                case "rmrd":
                case "rmsh":
                case "rmss":
                case "rmcs":
                case "rmzo":
                case "rmct":
                    return CacheContext.GetTag<Shader>(@"shaders\invalid");
            }
            return CacheContext.GetTag<Shader>(@"shaders\invalid");
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

            // apply option->option conversion where applicable
            ApplyOptionFixups(newShaderProperty.RealConstants, blamRmt2Descriptor, edRmt2Descriptor, edRmt2, renderMethodDefinition);

            foreach (var a in edIntConstants)
                newShaderProperty.IntegerConstants.Add(0);

            foreach (var a in edMaps)
                newShaderProperty.TextureConstants.Add(GetDefaultTextureConstant(a, edRmt2Descriptor, optionBitmaps));

            // if we have bits enabled by default, this actually disables boolean args. fixes a few visual issues
            for (int a = 0; a < edRmt2.BooleanParameterNames.Count; a++)
                newShaderProperty.BooleanConstants |= (1u << a);

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


            finalRm.ImportData = new List<ImportDatum>(); // most likely not used
            finalRm.ShaderProperties[0].Template = edRmt2Instance;
            finalRm.ShaderProperties[0].TextureConstants = newShaderProperty.TextureConstants;
            finalRm.ShaderProperties[0].RealConstants = newShaderProperty.RealConstants;
            finalRm.ShaderProperties[0].IntegerConstants = newShaderProperty.IntegerConstants;
            finalRm.ShaderProperties[0].BooleanConstants = newShaderProperty.BooleanConstants;

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

            FixAnimationProperties(cacheStream, blamCacheStream, CacheContext, finalRm, edRmt2, bmRmt2, blamTagName, edRmt2Descriptor.IsMs30);

            // build new rm option indices
            finalRm.RenderMethodDefinitionOptionIndices = BuildRenderMethodOptionIndices(edRmt2Descriptor);

            return finalRm;
        }

        private RenderMethod FixAnimationProperties(Stream cacheStream, Stream blamCacheStream, GameCacheHaloOnlineBase CacheContext, RenderMethod finalRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, string blamTagName, bool isMs30)
        {
            // finalRm is a H3 rendermethod with ported bitmaps, 
            if (finalRm.ShaderProperties[0].Functions.Count == 0)
                return finalRm;

            var pixlTag = CacheContext.Deserialize(cacheStream, edRmt2.PixelShader);
            var edPixl = (PixelShader)pixlTag;
            var bmPixl = BlamCache.Deserialize<PixelShader>(blamCacheStream, bmRmt2.PixelShader);

            // Make a collection of drawmodes and their DrawModeItem's
            // DrawModeItem are has info about all registers modified by functions for each drawmode.

            var bmPixlParameters = new Dictionary<int, List<ArgumentMapping>>(); // key is shader index

            // pixl side
            // For each drawmode, find its shader, and get all that shader's parameter.
            // Each parameter has a registerIndex, a registerType, and a registerName.
            // We'll use this to know which function acts on what shader and which registers

            var RegistersList = new Dictionary<int, int>();

            // match pixl registers
            foreach (var xboxShader in bmPixl.Shaders)
                foreach (var xboxParameter in xboxShader.XboxParameters)
                {
                    if (RegistersList.Keys.Contains(xboxParameter.RegisterIndex))
                        continue;

                    var xboxParameterName = BlamCache.StringTable.GetString(xboxParameter.ParameterName);

                    // loop pc params, find and match register
                    foreach (var pcShader in edPixl.Shaders)
                        foreach (var pcParameter in pcShader.PCParameters)
                        {
                            var pcParameterName = CacheContext.StringTable.GetString(pcParameter.ParameterName);

                            if (pcParameterName == xboxParameterName && xboxParameter.RegisterType == pcParameter.RegisterType && !RegistersList.ContainsKey(xboxParameter.RegisterIndex))
                                RegistersList.Add(xboxParameter.RegisterIndex, pcParameter.RegisterIndex);
                        }
                }

            // match glvs registers
            CachedTag blamRmdfTag = null;
            CachedTag edRmdfTag = null;

            string blamRmdfName = finalRm.BaseRenderMethod.Name;
            if (isMs30) // remove "ms30\\" from the name
                blamRmdfName = finalRm.BaseRenderMethod.Name.Remove(0, 5);

            if (BlamCache.TryGetTag(blamRmdfName + ".rmdf", out blamRmdfTag) && CacheContext.TryGetTag(finalRm.BaseRenderMethod.Name + ".rmdf", out edRmdfTag))
            {
                var bmRmdf = BlamCache.Deserialize<RenderMethodDefinition>(blamCacheStream, blamRmdfTag);
                var edRmdf = CacheContext.Deserialize<RenderMethodDefinition>(cacheStream, edRmdfTag);

                if (bmRmdf.GlobalVertexShader != null && edRmdf.GlobalVertexShader != null)
                {
                    var bmGlvs = BlamCache.Deserialize<GlobalVertexShader>(blamCacheStream, bmRmdf.GlobalVertexShader);
                    var edGlvs = CacheContext.Deserialize<GlobalVertexShader>(cacheStream, edRmdf.GlobalVertexShader);

                    foreach (var xboxShader in bmGlvs.Shaders)
                        foreach (var xboxParameter in xboxShader.XboxParameters)
                        {
                            if (RegistersList.Keys.Contains(xboxParameter.RegisterIndex))
                                continue;

                            var xboxParameterName = BlamCache.StringTable.GetString(xboxParameter.ParameterName);

                            // loop pc params, find and match register
                            foreach (var pcShader in edGlvs.Shaders)
                                foreach (var pcParameter in pcShader.PCParameters)
                                {
                                    var pcParameterName = CacheContext.StringTable.GetString(pcParameter.ParameterName);

                                    if (pcParameterName == xboxParameterName && xboxParameter.RegisterType == pcParameter.RegisterType && !RegistersList.ContainsKey(xboxParameter.RegisterIndex))
                                        RegistersList.Add(xboxParameter.RegisterIndex, pcParameter.RegisterIndex);
                                }
                        }
                }
            }

            var DrawModeIndex = -1;
            foreach (var a in bmPixl.DrawModes)
            {
                DrawModeIndex++;

                bmPixlParameters.Add(DrawModeIndex, new List<ArgumentMapping>());

                if (a.Count == 0)
                    continue;

                foreach (var b in bmPixl.Shaders[a.Offset].XboxParameters)
                {
                    var ParameterName = BlamCache.StringTable.GetString(b.ParameterName);

                    bmPixlParameters[DrawModeIndex].Add(new ArgumentMapping
                    {
                        ShaderIndex = a.Offset,
                        ParameterName = ParameterName,
                        RegisterIndex = b.RegisterIndex,
                        RegisterType = b.RegisterType
                    });
                }
            }

            // rm side
            var bmDrawmodesFunctions = new Dictionary<int, Unknown3Tagblock>(); // key is shader index
            DrawModeIndex = -1;
            foreach (var a in finalRm.ShaderProperties[0].EntryPoints)
            {
                DrawModeIndex++;

                // These are not modes. This is an indireciton table of packed 10_6 shorts
                // from RMT2 ShaderDrawmodes to RegisterOffsets
                // register_offset = ShaderDrawmodes[current_drawmode].Offset
                var drawmodeRegisterOffset = (int)a.Offset;
                var drawmodeRegisterCount = (int)a.Count;


                var ArgumentMappingsIndexSampler = (byte)finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].Texture.Offset;
                var ArgumentMappingsCountSampler = finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].Texture.Count;
                var ArgumentMappingsIndexUnknown = (byte)finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].RealVertex.Offset;
                var ArgumentMappingsCountUnknown = finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].RealVertex.Count;
                var ArgumentMappingsIndexVector = (byte)finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].RealPixel.Offset;
                var ArgumentMappingsCountVector = finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].RealPixel.Count;
                var ArgumentMappings = new List<ArgumentMapping>();

                for (int j = 0; j < ArgumentMappingsCountSampler; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexSampler + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexSampler + j].SourceIndex, // i don't think i can use it to match stuf
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexSampler + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Sampler,
                        ShaderIndex = drawmodeRegisterOffset,
                        // WARNING i think drawmodes in rm aren't the same as in pixl, because rm drawmodes can point to a global shader .
                        // say rm.drawmodes[17]'s value is 13, pixl.drawmodes[17] would typically be 12
                    });
                }

                for (int j = 0; j < ArgumentMappingsCountUnknown; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexUnknown + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexUnknown + j].SourceIndex,
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexUnknown + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Vector,
                        ShaderIndex = drawmodeRegisterOffset,
                        // it's something else, uses a global shader or some shit, one water shader pointed to a vtsh in rasg, but not in H3, maybe coincidence
                        // yeah guaranteed rmdf's glvs or rasg shaders
                    });
                }

                for (int j = 0; j < ArgumentMappingsCountVector; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexVector + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexVector + j].SourceIndex,
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexVector + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Vector,
                        ShaderIndex = drawmodeRegisterOffset,
                    });
                }

                bmDrawmodesFunctions.Add(DrawModeIndex, new Unknown3Tagblock
                {
                    Unknown3Index = drawmodeRegisterOffset, // not shader index for rm and rmt2
                    Unknown3Count = drawmodeRegisterCount, // should always be 4 for enabled drawmodes
                    ArgumentMappingsIndexSampler = ArgumentMappingsIndexSampler,
                    ArgumentMappingsCountSampler = ArgumentMappingsCountSampler,
                    ArgumentMappingsIndexUnknown = ArgumentMappingsIndexUnknown, // no clue what it's used for, global shaders? i know one of the drawmodes will use one or more shaders from glvs, no idea if always or based on something
                    ArgumentMappingsCountUnknown = ArgumentMappingsCountUnknown,
                    ArgumentMappingsIndexVector = ArgumentMappingsIndexVector,
                    ArgumentMappingsCountVector = ArgumentMappingsCountVector,
                    ArgumentMappings = ArgumentMappings
                });
            }

            DrawModeIndex = -1;
            foreach (var a in bmDrawmodesFunctions)
            {
                DrawModeIndex++;
                if (a.Value.Unknown3Count == 0)
                    continue;

                foreach (var b in a.Value.ArgumentMappings)
                {
                    foreach (var c in bmPixlParameters[a.Key])
                    {
                        if (b.RegisterIndex == c.RegisterIndex && b.RegisterType == c.RegisterType)
                        {
                            b.ParameterName = c.ParameterName;
                            break;
                        }
                    }
                }
            }

            // Now that we know which register is what for each drawmode, find its halo online equivalent register indexes based on register name.
            // This is where it gets tricky because drawmodes count changed in HO. 
            foreach (var a in bmDrawmodesFunctions)
            {
                if (a.Value.Unknown3Count == 0)
                    continue;

                foreach (var b in a.Value.ArgumentMappings)
                {
                    foreach (var c in edPixl.Shaders[edPixl.DrawModes[a.Key].Offset].PCParameters)
                    {
                        var ParameterName = CacheContext.StringTable.GetString(c.ParameterName);

                        if (ParameterName == b.ParameterName && b.RegisterType == c.RegisterType)
                        {
                            if (!RegistersList.ContainsKey(b.RegisterIndex))
                                RegistersList.Add(b.RegisterIndex, c.RegisterIndex);

                            b.EDRegisterIndex = c.RegisterIndex;
                        }
                    }
                }
            }

            // Store registers, in case they are set to -1 and are lost
            List<short> priorRegisters = new List<short>();
            foreach (var parameter in finalRm.ShaderProperties[0].Parameters)
                priorRegisters.Add(parameter.RegisterIndex);

            // Set new registers
            foreach (var a in bmDrawmodesFunctions)
            {
                if (a.Value.Unknown3Count == 0)
                    continue;

                foreach (var b in a.Value.ArgumentMappings)
                {
                    finalRm.ShaderProperties[0].Parameters[b.ArgumentMappingsTagblockIndex].RegisterIndex = (short)b.EDRegisterIndex;
                }
            }

            // replace bm registers with ed registers if the above failed
            // TODO: properly fix the above code so this isnt needed (this will work fine for now tho)
            for (int i = 0; i < finalRm.ShaderProperties[0].Parameters.Count; i++)
            {
                if (finalRm.ShaderProperties[0].Parameters[i].RegisterIndex == -1)
                {
                    int bmRegister = (int)priorRegisters[i];

                    if (!RegistersList.TryGetValue(bmRegister, out int newRegister))
                    {
                        Console.WriteLine($"WARNING: Could not match blam register \"{bmRegister}\"");
                    }
                    else
                        finalRm.ShaderProperties[0].Parameters[i].RegisterIndex = (short)newRegister;
                }
            }

            //
            // Gather all register indices from relevant pixl and glvs
            //

            var validEdRegisters = new List<int>();

            foreach (var a in edPixl.Shaders)
                foreach (var b in a.PCParameters)
                    if (!validEdRegisters.Contains(b.RegisterIndex))
                        validEdRegisters.Add(b.RegisterIndex);

            if (edRmdfTag != null)
            {
                var edRmdf = CacheContext.Deserialize<RenderMethodDefinition>(cacheStream, edRmdfTag);
                if (edRmdf.GlobalVertexShader != null)
                {
                    var edGlvs = CacheContext.Deserialize<GlobalVertexShader>(cacheStream, edRmdf.GlobalVertexShader);

                    foreach (var pcShader in edGlvs.Shaders)
                        foreach (var pcParameter in pcShader.PCParameters)
                            if (!validEdRegisters.Contains(pcParameter.RegisterIndex))
                                validEdRegisters.Add(pcParameter.RegisterIndex);
                }
            }

            //
            // Check that all register indices are valid 
            //

            foreach (var a in finalRm.ShaderProperties[0].Parameters)
            {
                if (!validEdRegisters.Contains(a.RegisterIndex))
                {
                    //Console.WriteLine($"INVALID REGISTER \"{a.RegisterIndex}\" IN TAG {blamTagName}!");

                    // this code is only reached if an invalid register was found - this throws off the indices for other animations
                    // TODO: add system to preserve other animations

                    finalRm.ShaderProperties[0].EntryPoints = new List<RenderMethodTemplate.PackedInteger_10_6>();
                    finalRm.ShaderProperties[0].ParameterTables = new List<ParameterTable>();
                    finalRm.ShaderProperties[0].Parameters = new List<ParameterMapping>();
                    finalRm.ShaderProperties[0].Functions = new List<ShaderFunction>();
                    foreach (var textureConstant in finalRm.ShaderProperties[0].TextureConstants)
                        textureConstant.Functions.Integer = 0;
                    break;
                }
            }

            return finalRm;
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
                        textureConstant.Bitmap = CacheContext.GetTag(@"ms30\shaders\default_bitmaps\bitmaps\default_detail.bitm");
                    else
                        textureConstant.Bitmap = CacheContext.GetTag(@"shaders\default_bitmaps\bitmaps\default_detail.bitm");
                    return textureConstant;
            }

            // get default bitmap from dictionary
            if (!optionBitmaps.TryGetValue(CacheContext.StringTable.GetStringId(parameter), out textureConstant.Bitmap) || textureConstant.Bitmap == null)
            {
                // fallback
                Console.WriteLine($"WARNING: Sampler parameter \"{parameter}\" has no default bitmap");

                // null bitmap causes bad rendering, use default_detail in these cases
                if (rmt2Descriptor.IsMs30)
                    textureConstant.Bitmap = CacheContext.GetTag(@"ms30\shaders\default_bitmaps\bitmaps\default_detail.bitm");
                else
                    textureConstant.Bitmap = CacheContext.GetTag(@"shaders\default_bitmaps\bitmaps\default_detail.bitm");
            }

            return textureConstant;
        }

        private RealConstant GetDefaultRealConstant(string parameter, string type, List<string> methodNames, Dictionary<StringId, RenderMethodOption.OptionBlock> optionBlocks)
        {
            if (!optionBlocks.TryGetValue(CacheContext.StringTable.GetStringId(parameter), out var optionBlock))
            {
                // TODO: verify these -- some parameters are method names???
                if (!methodNames.Contains(parameter))
                    Console.WriteLine($"WARNING: No type found for {type} parameter \"{parameter}\"");

                // just 1.0f for now (see above)
                optionBlock = new RenderMethodOption.OptionBlock
                {
                    Type = RenderMethodOption.OptionBlock.OptionDataType.Float,
                    DefaultFloatArgument = 1.0f
                };
            }

            switch (optionBlock.Type)
            {
                case RenderMethodOption.OptionBlock.OptionDataType.Sampler:
                    return new RealConstant { Arg0 = 1.0f, Arg1 = 1.0f, Arg2 = 0.0f, Arg3 = 0.0f };

                case RenderMethodOption.OptionBlock.OptionDataType.Float:
                    return new RealConstant { Arg0 = optionBlock.DefaultFloatArgument, Arg1 = optionBlock.DefaultFloatArgument, Arg2 = optionBlock.DefaultFloatArgument, Arg3 = optionBlock.DefaultFloatArgument };

                // convert ARGB to RealRGBA
                case RenderMethodOption.OptionBlock.OptionDataType.Float4:
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

        private void ApplyOptionFixups(List<RealConstant> realConstants, ShaderMatcherNew.Rmt2Descriptor blamRmt2Descriptor, ShaderMatcherNew.Rmt2Descriptor edRmt2Descriptor, RenderMethodTemplate edRmt2, RenderMethodDefinition rmdf)
        {
            //
            // This applies manual option->option fixups for matching. This concept should not be used for rm creation/generation.
            //

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
            if (OptionChanged("shader", "parallax", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if parallax is new to the shader, set height map scale to 0
                if (blamRmt2Descriptor.Options[optionIndex] == 0 && edRmt2Descriptor.Options[optionIndex] != 0)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "height_scale")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                            break;
                        }
                }
            }
        }
    }
}