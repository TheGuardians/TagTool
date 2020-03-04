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
                    }
                    return cntl;

                case Particle prt3:
                    var blamPrt3 = (Particle)blamDefinition;
                    prt3.RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, prt3.RenderMethod, blamTag, blamPrt3.RenderMethod);
                    return prt3;

                case LightVolumeSystem ltvl:
                    var blamLtvl = (LightVolumeSystem)blamDefinition;
                    for (int i = 0; i < ltvl.LightVolume.Count; i++)
                    {
                        ltvl.LightVolume[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, ltvl.LightVolume[i].RenderMethod, blamTag, blamLtvl.LightVolume[i].RenderMethod);
                    }
                    return ltvl;
                case DecalSystem decs:
                    var blamDecs = (DecalSystem)blamDefinition;
                    for (int i = 0; i < decs.Decal.Count; i++)
                    {
                        decs.Decal[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, decs.Decal[i].RenderMethod, blamTag, blamDecs.Decal[i].RenderMethod);
                    }
                    return decs;
                case BeamSystem beamSystem:
                    var blamBeam = (BeamSystem)blamDefinition;
                    for (int i = 0; i < beamSystem.Beam.Count; i++)
                    {
                        beamSystem.Beam[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, beamSystem.Beam[i].RenderMethod, blamTag, blamBeam.Beam[i].RenderMethod);
                    }
                    return beamSystem;

                case ShaderScreen rmss:
                case ShaderZonly rmzo:
                case ShaderBlack rmbk:
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
                    return CacheContext.GetTag<ShaderWater>(@"objects\multi\shaders\green_plasma_receiver");

                case "rmbk":
                    return CacheContext.GetTag<ShaderWater>(@"levels\dlc\bunkerworld\shaders\z_black");

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
            if (!Matcher.IsInitialized)
                Matcher.Init(CacheContext, BlamCache, cacheStream, blamCacheStream, FlagIsSet(PortingFlags.Ms30));

            return Matcher.FindClosestTemplate(blamRmt2, BlamCache.Deserialize<RenderMethodTemplate>(blamCacheStream, blamRmt2));
        }

        private RenderMethod ConvertRenderMethod(Stream cacheStream, Stream blamCacheStream, RenderMethod finalRm, CachedTag sourceRmt2, string blamTagName)
        {
            // Verify that the ShaderMatcher is ready to use
            if (!Matcher.IsInitialized)
                Matcher.Init(CacheContext, BlamCache, cacheStream, blamCacheStream, FlagIsSet(PortingFlags.Ms30));

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
                IntegerConstants = new List<uint>()
            };

            // Get a simple list of bitmaps and arguments names
            var bmRmt2Instance = sourceRmt2; 
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

            // Find a HO equivalent rmt2
            CachedTag edRmt2Instance = finalRm.ShaderProperties[0].Template;

            if (edRmt2Instance == null)
            {
                throw new Exception($"Failed to find HO rmt2 for this RenderMethod instance");
            }

            var edRmt2 = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, edRmt2Instance);

            foreach (var a in edRmt2.TextureParameterNames)
                edMaps.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.RealParameterNames)
                edRealConstants.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.IntegerParameterNames)
                edIntConstants.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.BooleanParameterNames)
                edBoolConstants.Add(CacheContext.StringTable.GetString(a.Name));

            foreach (var a in edRealConstants)
                newShaderProperty.RealConstants.Add(new RealConstant());

            foreach (var a in edIntConstants)
                newShaderProperty.IntegerConstants.Add(0);

            foreach (var a in edMaps)
                newShaderProperty.TextureConstants.Add(new TextureConstant());

            newShaderProperty.BooleanConstants = 0;

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
                        if ((newShaderProperty.BooleanConstants & (1u << bmBoolConstants.IndexOf(bA))) != 0)
                        {
                            newShaderProperty.BooleanConstants &= ~(1u << bmBoolConstants.IndexOf(bA));
                            newShaderProperty.BooleanConstants |= (1u << edBoolConstants.IndexOf(eA));
                        }
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

                switch(i)
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
            foreach(var tex in finalRm.ShaderProperties[0].TextureConstants)
            {
                if(tex.XFormArgumentIndex != -1)
                    tex.XFormArgumentIndex = (sbyte)edRealConstants.IndexOf(bmRealConstants[tex.XFormArgumentIndex]);
            }

            finalRm.BaseRenderMethod = Matcher.FindRmdf(edRmt2Instance);

            if (finalRm.BaseRenderMethod == null)
                throw new Exception("Unable to find valid rmdf for rmt2");

            FixAnimationProperties(cacheStream, blamCacheStream, CacheContext, finalRm, edRmt2, bmRmt2, blamTagName);

            return finalRm;
        }

        private RenderMethod FixAnimationProperties(Stream cacheStream, Stream blamCacheStream, GameCacheHaloOnlineBase CacheContext, RenderMethod finalRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, string blamTagName)
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

            var RegistersList = new Dictionary<int, string>();

            foreach (var a in finalRm.ShaderProperties[0].Parameters)
                if (!RegistersList.ContainsKey(a.RegisterIndex))
                    RegistersList.Add(a.RegisterIndex, "");

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

            // // Now that we know which register is what for each drawmode, find its halo online equivalent register indexes based on register name.
            // // This is where it gets tricky because drawmodes count changed in HO. 
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
                            if (RegistersList[b.RegisterIndex] == "")
                                RegistersList[b.RegisterIndex] = $"{c.RegisterIndex}";
                            else
                                RegistersList[b.RegisterIndex] = $"{RegistersList[b.RegisterIndex]},{c.RegisterIndex}";

                            b.EDRegisterIndex = c.RegisterIndex;
                        }
                    }
                }
            }

            // DEBUG draw registers
            // DEBUG check for invalid registers
            foreach (var a in bmDrawmodesFunctions)
            {
                if (a.Value.Unknown3Count == 0)
                    continue;

                foreach (var b in a.Value.ArgumentMappings)
                {
                    finalRm.ShaderProperties[0].Parameters[b.ArgumentMappingsTagblockIndex].RegisterIndex = (short)b.EDRegisterIndex;
                }
            }

            // one final check
            // Gather all register indexes from pixl tag. Then check against all the converted register indexes. 
            // It should detect registers that are invalid and would crash, but it does not verify if the register is valid.
            var validEDRegisters = new List<int>();

            foreach (var a in edPixl.Shaders)
                foreach (var b in a.PCParameters)
                    if (!validEDRegisters.Contains(b.RegisterIndex))
                        validEDRegisters.Add(b.RegisterIndex);

            foreach (var a in finalRm.ShaderProperties[0].Parameters)
            {
                if (!validEDRegisters.Contains((a.RegisterIndex)))
                {
                    // Display a warning
                    // Console.WriteLine($"INVALID REGISTERS IN TAG {blamTagName}!");

                    finalRm.ShaderProperties[0].EntryPoints = new List<RenderMethodTemplate.PackedInteger_10_6>();
                    finalRm.ShaderProperties[0].ParameterTables = new List<ParameterTable>();
                    finalRm.ShaderProperties[0].Parameters = new List<ParameterMapping>();
                    finalRm.ShaderProperties[0].Functions = new List<ShaderFunction>();
                    foreach (var map in finalRm.ShaderProperties[0].TextureConstants)
                        map.Functions.Integer = 0;
                    return finalRm;
                }
            }

            return finalRm;
        }
    }
}