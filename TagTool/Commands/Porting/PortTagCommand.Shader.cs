using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.Collections.Generic;
using System.IO;
using TagTool.Serialization;
using TagTool.Shaders.ShaderMatching;
using System;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public ShaderMatcher Matcher = new ShaderMatcher();

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

        private RenderMethod ConvertRenderMethod(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, RenderMethod finalRm, string blamTagName)
        {
            // Verify that the ShaderMatcher is ready to use
            if (!Matcher.IsInitialized())
                Matcher.Init(cacheStream, CacheContext, BlamCache);

            // Set flags
            Matcher.SetMS30Flag(cacheStream, FlagIsSet(PortingFlags.Ms30));

            // finalRm.ShaderProperties[0].ShaderMaps are all ported bitmaps
            // finalRm.BaseRenderMethod is a H3 tag
            // finalRm.ShaderProperties[0].Template is a H3 tag

            // TODO hardcode shader values such as argument changes for specific shaders
            var bmMaps = new List<string>();
            var bmArgs = new List<string>();
            var edMaps = new List<string>();
            var edArgs = new List<string>();

            // Reset rmt2 preset
            var pRmt2 = 0;

            // Make a template of ShaderProperty, with the correct bitmaps and arguments counts. 
            var newShaderProperty = new RenderMethod.ShaderProperty
            {
                ShaderMaps = new List<RenderMethod.ShaderProperty.ShaderMap>(),
                Arguments = new List<RenderMethod.ShaderProperty.Argument>()
            };

            // Get a simple list of bitmaps and arguments names
            var bmRmt2Instance = BlamCache.IndexItems.Find(x => x.ID == finalRm.ShaderProperties[0].Template.Index);
            var blamContext = new CacheSerializationContext(ref BlamCache, bmRmt2Instance);
            var bmRmt2 = BlamCache.Deserializer.Deserialize<RenderMethodTemplate>(blamContext);

            // Get a simple list of H3 bitmaps and arguments names
            foreach (var a in bmRmt2.SamplerArguments)
                bmMaps.Add(BlamCache.Strings.GetItemByID(a.Name.Index));
            foreach (var a in bmRmt2.VectorArguments)
                bmArgs.Add(BlamCache.Strings.GetItemByID(a.Name.Index));

            // Find a HO equivalent rmt2
            var edRmt2Instance = Matcher.FixRmt2Reference(cacheStream, blamTagName, bmRmt2Instance, bmRmt2, bmMaps, bmArgs);

            if (edRmt2Instance == null)
                return CacheContext.Deserialize<Shader>(cacheStream, CacheContext.GetTag<Shader>(@"shaders\invalid"));

            var edRmt2Tagname = edRmt2Instance.Name ?? $"0x{edRmt2Instance.Index:X4}";

            // pRmsh pRmt2 now potentially have a new value
            if (pRmt2 != 0)
            {
                if (CacheContext.TagCache.Index.Contains(pRmt2))
                {
                    var a = CacheContext.GetTag(pRmt2);
                    if (a != null)
                        edRmt2Instance = a;
                }
            }

            var edRmt2 = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, edRmt2Instance);

            foreach (var a in edRmt2.SamplerArguments)
                edMaps.Add(CacheContext.StringIdCache.GetString(a.Name));
            foreach (var a in edRmt2.VectorArguments)
                edArgs.Add(CacheContext.StringIdCache.GetString(a.Name));

            // The bitmaps are default textures.
            // Arguments are probably default values. I took the values that appeared the most frequently, assuming they are the default value.
            foreach (var a in edMaps)
            {
                var newBitmap = Matcher.GetDefaultBitmapTag(a);

                if (!CacheContext.TagCache.Index.Contains(pRmt2))
                    newBitmap = @"shaders\default_bitmaps\bitmaps\default_detail"; // would only happen for removed shaders

                CachedTagInstance bitmap = null;

                try
                {
                    bitmap = CacheContext.GetTag<Bitmap>(newBitmap);
                }
                catch
                {
                    bitmap = ConvertTag(cacheStream, resourceStreams, ParseLegacyTag($"{newBitmap}.bitm")[0]);
                }

                newShaderProperty.ShaderMaps.Add(
                    new RenderMethod.ShaderProperty.ShaderMap
                    {
                        Bitmap = bitmap
                    });
            }

            foreach (var a in edArgs)
                newShaderProperty.Arguments.Add(Matcher.DefaultArgumentsValues(a));

            // Reorder blam bitmaps to match the HO rmt2 order
            // Reorder blam arguments to match the HO rmt2 order
            foreach (var eM in edMaps)
                foreach (var bM in bmMaps)
                    if (eM == bM)
                        newShaderProperty.ShaderMaps[edMaps.IndexOf(eM)] = finalRm.ShaderProperties[0].ShaderMaps[bmMaps.IndexOf(bM)];

            foreach (var eA in edArgs)
                foreach (var bA in bmArgs)
                    if (eA == bA)
                        newShaderProperty.Arguments[edArgs.IndexOf(eA)] = finalRm.ShaderProperties[0].Arguments[bmArgs.IndexOf(bA)];

            // Remove some tagblocks
            // finalRm.Unknown = new List<RenderMethod.UnknownBlock>(); // hopefully not used; this gives rmt2's name. They correspond to the first tagblocks in rmdf, they tell what the shader does
            finalRm.ImportData = new List<RenderMethod.ImportDatum>(); // most likely not used
            finalRm.ShaderProperties[0].Template = edRmt2Instance;
            finalRm.ShaderProperties[0].ShaderMaps = newShaderProperty.ShaderMaps;
            finalRm.ShaderProperties[0].Arguments = newShaderProperty.Arguments;

            Matcher.FixRmdfTagRef(finalRm);

            FixAnimationProperties(cacheStream, resourceStreams, BlamCache, CacheContext, finalRm, edRmt2, bmRmt2);

            // Fix any null bitmaps, caused by bitm port failure
            foreach (var a in finalRm.ShaderProperties[0].ShaderMaps)
            {
                if (a.Bitmap != null)
                    continue;

                var defaultBitmap = Matcher.GetDefaultBitmapTag(edMaps[finalRm.ShaderProperties[0].ShaderMaps.IndexOf(a)]);

                try
                {
                    a.Bitmap = CacheContext.GetTag<Bitmap>(defaultBitmap);
                }
                catch
                {
                    a.Bitmap = ConvertTag(cacheStream, resourceStreams, ParseLegacyTag($"{defaultBitmap}.bitm")[0]);
                }
            }

            if (Matcher.RmhgUnknownTemplates.Contains(edRmt2Instance.Name))
                if (finalRm.ShaderProperties[0].Unknown.Count == 0)
                    finalRm.ShaderProperties[0].Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>
                    {
                        new RenderMethod.ShaderProperty.UnknownBlock1
                        {
                            Unknown = 1
                        }
                    };

            return finalRm;
        }

        private RenderMethod FixAnimationProperties(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CacheFile blamCache, HaloOnlineCacheContext CacheContext, RenderMethod finalRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2)
        {
            // finalRm is a H3 rendermethod with ported bitmaps, 
            if (finalRm.ShaderProperties[0].AnimationProperties.Count == 0)
                return finalRm;

            foreach (var properties in finalRm.ShaderProperties[0].AnimationProperties)
            {
                if (BlamCache.Version < CacheVersion.Halo3ODST && !Enum.TryParse(properties.Type.Halo3Retail.ToString(), out properties.Type.Halo3Odst))
                    throw new NotSupportedException(properties.Type.Halo3Retail.ToString());

                properties.InputName = ConvertStringId(properties.InputName);
                properties.RangeName = ConvertStringId(properties.RangeName);

                ConvertTagFunction(properties.Function);
            }

            var pixlTag = CacheContext.Deserialize(cacheStream, edRmt2.PixelShader);
            var edPixl = (PixelShader)pixlTag;

            var blamContext = new CacheSerializationContext(ref BlamCache, blamCache.IndexItems.Find(x => x.ID == bmRmt2.PixelShader.Index));
            var bmPixl = BlamCache.Deserializer.Deserialize<PixelShader>(blamContext);

            // Make a collection of drawmodes and their DrawModeItem's
            // DrawModeItem are has info about all registers modified by functions for each drawmode.

            var bmPixlParameters = new Dictionary<int, List<ArgumentMapping>>(); // key is shader index

            // pixl side
            // For each drawmode, find its shader, and get all that shader's parameter.
            // Each parameter has a registerIndex, a registerType, and a registerName.
            // We'll use this to know which function acts on what shader and which registers

            var RegistersList = new Dictionary<int, string>();

            foreach (var a in finalRm.ShaderProperties[0].ArgumentMappings)
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
                    var ParameterName = BlamCache.Strings.GetItemByID(b.ParameterName.Index);

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
            foreach (var a in finalRm.ShaderProperties[0].DrawModes)
            {
                DrawModeIndex++;

                // These are not modes. This is an indireciton table of packed 10_6 shorts
                // from RMT2 ShaderDrawmodes to RegisterOffsets
                // register_offset = ShaderDrawmodes[current_drawmode].Offset
                var drawmodeRegisterOffset = (int)a.Offset;
                var drawmodeRegisterCount = (int)a.Count;


                var ArgumentMappingsIndexSampler = (byte)finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleSampler;
                var ArgumentMappingsCountSampler = finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleSampler >> 8;
                var ArgumentMappingsIndexUnknown = (byte)finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleUnknown;
                var ArgumentMappingsCountUnknown = finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleUnknown >> 8;
                var ArgumentMappingsIndexVector = (byte)finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleVector;
                var ArgumentMappingsCountVector = finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleVector >> 8;
                var ArgumentMappings = new List<ArgumentMapping>();

                for (int j = 0; j < ArgumentMappingsCountSampler / 4; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexSampler + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexSampler + j].ArgumentIndex, // i don't think i can use it to match stuf
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexSampler + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Sampler,
                        ShaderIndex = drawmodeRegisterOffset,
                        // WARNING i think drawmodes in rm aren't the same as in pixl, because rm drawmodes can point to a global shader .
                        // say rm.drawmodes[17]'s value is 13, pixl.drawmodes[17] would typically be 12
                    });
                }

                for (int j = 0; j < ArgumentMappingsCountUnknown / 4; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexUnknown + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexUnknown + j].ArgumentIndex,
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexUnknown + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Vector,
                        ShaderIndex = drawmodeRegisterOffset,
                        // it's something else, uses a global shader or some shit, one water shader pointed to a vtsh in rasg, but not in H3, maybe coincidence
                        // yeah guaranteed rmdf's glvs or rasg shaders
                    });
                }

                for (int j = 0; j < ArgumentMappingsCountVector / 4; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexVector + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexVector + j].ArgumentIndex,
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
                        var ParameterName = CacheContext.StringIdCache.GetString(c.ParameterName);

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
                    finalRm.ShaderProperties[0].ArgumentMappings[b.ArgumentMappingsTagblockIndex].RegisterIndex = (short)b.EDRegisterIndex;
            }

            // one final check
            // Gather all register indexes from pixl tag. Then check against all the converted register indexes. 
            // It should detect registers that are invalid and would crash, but it does not verify if the register is valid.
            var validEDRegisters = new List<int>();

            foreach (var a in edPixl.Shaders)
                foreach (var b in a.PCParameters)
                    if (!validEDRegisters.Contains(b.RegisterIndex))
                        validEDRegisters.Add(b.RegisterIndex);

            foreach (var a in finalRm.ShaderProperties[0].ArgumentMappings)
            {
                if (!validEDRegisters.Contains((a.RegisterIndex)))
                {
                    // Abort, disable functions
                    finalRm.ShaderProperties[0].Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>(); // no idea what it does, but it crashes sometimes if it's null. on Shrine, it's the shader loop effect
                    finalRm.ShaderProperties[0].AnimationProperties = new List<RenderMethod.AnimationPropertiesBlock>();
                    finalRm.ShaderProperties[0].ArgumentMappings = new List<RenderMethod.ShaderProperty.ArgumentMapping>();
                    finalRm.ShaderProperties[0].Unknown3 = new List<RenderMethod.ShaderProperty.UnknownBlock3>();
                    foreach (var b in edRmt2.RegisterOffsets) // stops crashing for some shaders if the drawmodes count is still the same
                        finalRm.ShaderProperties[0].Unknown3.Add(new RenderMethod.ShaderProperty.UnknownBlock3());

                    return finalRm;
                }
            }

            return finalRm;
        }

    }
}