using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.Shaders.ShaderMatching;
using System.IO;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Shaders.ShaderFunctions;
using TagTool.Shaders.ShaderGenerator;
using System.Diagnostics;
using static TagTool.Shaders.ShaderMatching.ShaderMatcherNew;
using static TagTool.Tags.Definitions.RenderMethod;
using static TagTool.Tags.Definitions.RenderMethodOption;
using static TagTool.Tags.Definitions.RenderMethod.RenderMethodPostprocessBlock;

namespace TagTool.Shaders.ShaderConverter
{
    public class ShaderConverter
    {
        private GameCache Cache;
        private GameCache BlamCache;

        private RenderMethod RenderMethod; // do not change fields in this definition
        private RenderMethod BlamRenderMethod;

        private CachedTag RmdfTag;

        private Rmt2Descriptor BaseRmt2Descriptor;
        private ShaderMatcherNew Matcher;

        private List<ParameterBlock> Parameters; // from all included rmop
        private RenderMethodDefinition Rmdf; // base
        private RenderMethodTemplate Rmt2; // base
        private RenderMethodTemplate BlamRmt2;
        private List<ParameterMapping> ParameterMappings; // for multiple types (these are porting cache indices)
        private List<int> BlamToBaseTextureIndices; // List index = blam index, stored index = base 
        private List<int> BlamToBaseRealIndices; // List index = blam index, stored index = base index

        private class ParameterMapping
        {
            public string Name;
            public ShaderParameter.RType Type;
            public int BlamIndex = -1;
            public int RealIndex = -1;
            public int BoolIndex = -1;
            public int IntIndex = -1;
        }

        public ShaderConverter(GameCache cache,
            GameCache blamCache,
            Stream stream,
            Stream blamStream,
            RenderMethod renderMethod,
            RenderMethod blamRenderMethod,
            ShaderMatcherNew matcher)
        {
            Cache = cache;
            BlamCache = blamCache;
            RenderMethod = renderMethod;
            BlamRenderMethod = blamRenderMethod;
            Matcher = matcher;
            RmdfTag = Matcher.FindRmdf(renderMethod.ShaderProperties[0].Template);
            Rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, RmdfTag);
            Rmt2 = Cache.Deserialize<RenderMethodTemplate>(stream, renderMethod.ShaderProperties[0].Template);
            BlamRmt2 = BlamCache.Deserialize<RenderMethodTemplate>(blamStream, blamRenderMethod.ShaderProperties[0].Template);
            BlamToBaseTextureIndices = new List<int>();
            BlamToBaseRealIndices = new List<int>();

            Rmt2Descriptor.TryParse(renderMethod.ShaderProperties[0].Template.Name,
                out BaseRmt2Descriptor);

            Parameters = ShaderGeneratorNew.GatherParameters(Cache, stream, Rmdf, BaseRmt2Descriptor.Options.ToList(), false);

            foreach (var blamTexture in BlamRmt2.TextureParameterNames)
            {
                int baseIndex = -1;
                string name = blamCache.StringTable.GetString(blamTexture.Name);
                for (int i = 0; i < Rmt2.TextureParameterNames.Count; i++)
                {
                    if (Cache.StringTable.GetString(Rmt2.TextureParameterNames[i].Name) == name)
                    {
                        baseIndex = i;
                        break;
                    }
                }
                BlamToBaseTextureIndices.Add(baseIndex);
            }
            foreach (var blamVector in BlamRmt2.RealParameterNames)
            {
                int baseIndex = -1;
                string name = blamCache.StringTable.GetString(blamVector.Name);
                for (int i = 0; i < Rmt2.RealParameterNames.Count; i++)
                {
                    if (Cache.StringTable.GetString(Rmt2.RealParameterNames[i].Name) == name)
                    {
                        baseIndex = i;
                        break;
                    }
                }
                BlamToBaseRealIndices.Add(baseIndex);
            }

            ParameterMappings = new List<ParameterMapping>();

            MatchTextures();
            MatchConstants();
        }

        // Not for use with textures (there is no possible overlap with textures)
        private void AddConstantToParameterMapping(string name, ShaderParameter.RType type, int blamIndex)
        {
            int index = ParameterMappings.FindIndex(x => x.Name == name);
            if (index != -1)
            {
                switch (type)
                {
                    case ShaderParameter.RType.Boolean:
                        Debug.Assert(ParameterMappings[index].BoolIndex == -1);
                        ParameterMappings[index].BoolIndex = blamIndex;
                        break;
                    case ShaderParameter.RType.Vector:
                        Debug.Assert(ParameterMappings[index].RealIndex == -1);
                        ParameterMappings[index].RealIndex = blamIndex;
                        break;
                    case ShaderParameter.RType.Integer:
                        Debug.Assert(ParameterMappings[index].IntIndex == -1);
                        ParameterMappings[index].IntIndex = blamIndex;
                        break;
                }
            }
            else
            {
                ParameterMappings.Add(new ParameterMapping
                {
                    Name = name,
                    Type = type,
                    BlamIndex = blamIndex
                });

                switch (type)
                {
                    case ShaderParameter.RType.Boolean:
                        ParameterMappings.Last().BoolIndex = blamIndex;
                        break;
                    case ShaderParameter.RType.Vector:
                        ParameterMappings.Last().RealIndex = blamIndex;
                        break;
                    case ShaderParameter.RType.Integer:
                        ParameterMappings.Last().IntIndex = blamIndex;
                        break;
                }
            }
        }

        private void MatchTextures()
        {
            for (int i = 0; i < BlamRmt2.TextureParameterNames.Count; i++)
            {
                ParameterMappings.Add(new ParameterMapping
                {
                    Name = BlamCache.StringTable.GetString(BlamRmt2.TextureParameterNames[i].Name),
                    Type = ShaderParameter.RType.Sampler,
                    BlamIndex = i
                });
            }
            // debug, remove later
            for (int i = 0; i < Rmt2.TextureParameterNames.Count; i++)
            {
                string name = Cache.StringTable.GetString(Rmt2.TextureParameterNames[i].Name);
                int index = ParameterMappings.FindIndex(x => x.Type == ShaderParameter.RType.Sampler && x.Name == name);

                if (index == -1)
                    new TagToolWarning($"Shader converter: could not match texture \"{name}\"");
            }
        }

        private void MatchConstants()
        {
            // Add all constants to list first as we'll need to link different types with each other
            for (int i = 0; i < BlamRmt2.RealParameterNames.Count; i++)
            {
                AddConstantToParameterMapping(BlamCache.StringTable.GetString(BlamRmt2.RealParameterNames[i].Name),
                    ShaderParameter.RType.Vector, i);
            }
            for (int i = 0; i < BlamRmt2.IntegerParameterNames.Count; i++)
            {
                AddConstantToParameterMapping(BlamCache.StringTable.GetString(BlamRmt2.IntegerParameterNames[i].Name),
                    ShaderParameter.RType.Integer, i);
            }
            for (int i = 0; i < BlamRmt2.BooleanParameterNames.Count; i++)
            {
                AddConstantToParameterMapping(BlamCache.StringTable.GetString(BlamRmt2.BooleanParameterNames[i].Name),
                    ShaderParameter.RType.Boolean, i);
            }
        }

        public RenderMethod ConvertRenderMethod()
        {
            RenderMethod newRm = new RenderMethod();

            newRm.BaseRenderMethod = RmdfTag;
            newRm.Options = BuildRenderMethodOptionIndices(BaseRmt2Descriptor);
            newRm.RenderFlags = RenderMethod.RenderFlags;
            newRm.SortLayer = RenderMethod.SortLayer;
            newRm.Version = RenderMethod.Version;
            newRm.CustomFogSettingIndex = RenderMethod.CustomFogSettingIndex;
            newRm.PredictionAtomIndex = RenderMethod.PredictionAtomIndex;
            newRm.Parameters = new List<RenderMethodParameterBlock>();
            newRm.ShaderProperties = new List<RenderMethodPostprocessBlock>();

            var shaderProperty = new RenderMethodPostprocessBlock
            {
                Template = RenderMethod.ShaderProperties[0].Template,
                TextureConstants = ConvertTextures(),
                RealConstants = ConvertRealConstants(),
                IntegerConstants = ConvertIntegerConstants(),
                BooleanConstants = ConvertBooleanConstants(),
                BlendMode = RenderMethod.ShaderProperties[0].BlendMode,
                Flags = RenderMethod.ShaderProperties[0].Flags,
                ImSoFiredPad = RenderMethod.ShaderProperties[0].ImSoFiredPad,
                QueryableProperties = ConvertQueryableProperties(),
                Functions = RenderMethod.ShaderProperties[0].Functions,
                EntryPoints = new List<TagBlockIndex>(),
                Passes = new List<RenderMethodPostprocessPassBlock>(),
                RoutingInfo = new List<RenderMethodRoutingInfoBlock>()
            };

            newRm.ShaderProperties.Add(shaderProperty);

            if (newRm.ShaderProperties[0].Functions.Count > 0) // if there are no functions, converting is pointless
            {
                var animatedParameters = ShaderFunctionHelper.GetAnimatedParameters(BlamCache, BlamRenderMethod, BlamRmt2);
                ShaderFunctionHelper.BuildAnimatedParameters(Cache, newRm, Rmt2, animatedParameters);
            }

            // We now disable ATOC completely, later games have conversion issues

            //// Check for ATOC materials and flag accordingly --
            //// ATOC materials changed in ODST. We use a base of H3, so we need to re-enable the ATOC flag where necessary.
            //if (rmdf.ContainsCategory(CacheContext, "alpha_test") &&
            //    !finalRm.CategoryOptionSelected(CacheContext, rmdf, "alpha_test", "none") &&
            //    !finalRm.CategoryOptionSelected(CacheContext, rmdf, "material_model", "cook_torrance") &&
            //    !finalRm.CategoryOptionSelected(CacheContext, rmdf, "material_model", "two_lobe_phong") &&
            //    !finalRm.CategoryOptionSelected(CacheContext, rmdf, "material_model", "default_skin") &&
            //    !finalRm.CategoryOptionSelected(CacheContext, rmdf, "material_model", "glass") &&
            //    !finalRm.CategoryOptionSelected(CacheContext, rmdf, "material_model", "organism"))
            //    newShaderProperty.Flags |= RenderMethodPostprocessFlags.EnableAlphaTest;
            //else
                newRm.ShaderProperties[0].Flags &= ~RenderMethodPostprocessFlags.EnableAlphaTest;

            return newRm;
        }

        private List<RenderMethodOptionIndex> BuildRenderMethodOptionIndices(Rmt2Descriptor rmt2Descriptor)
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

        private List<TextureConstant> ConvertTextures()
        {
            List<TextureConstant> textureConstants = new List<TextureConstant>();

            for (int i = 0; i < Rmt2.TextureParameterNames.Count; i++)
            {
                string name = Cache.StringTable.GetString(Rmt2.TextureParameterNames[i].Name);
                int parameterMappingIndex = ParameterMappings.FindIndex(x => x.Type == ShaderParameter.RType.Sampler && x.Name == name);
                //int xformMappingIndex = ParameterMappings.FindIndex(x => x.Type == ShaderParameter.RType.Vector && x.Name == name);

                if (parameterMappingIndex != -1)
                {
                    textureConstants.Add(RenderMethod.ShaderProperties[0].TextureConstants[ParameterMappings[parameterMappingIndex].BlamIndex]);
                    textureConstants.Last().TextureTransformConstantIndex = (sbyte)ParameterMappings[parameterMappingIndex].RealIndex;
                }
                else // use default value
                {
                    new TagToolWarning($"Shader converter: attempting default values for texture {name}");

                    int parameterIndex = Parameters.FindIndex(x => x.Type == ParameterBlock.OptionDataType.Bitmap &&
                        Cache.StringTable.GetString(x.Name) == name);

                    if (parameterIndex == -1) // this will crash for the meantime
                        new TagToolWarning($"Shader converter: texture parameter \"{name}\" does not exist in rmop");

                    textureConstants.Add(new TextureConstant
                    {
                        Bitmap = Parameters[parameterIndex].DefaultSamplerBitmap,
                        BitmapIndex = 0,
                        SamplerAddressMode = new TextureConstant.PackedSamplerAddressMode
                        {
                            AddressU = (TextureConstant.SamplerAddressModeEnum)Parameters[parameterIndex].DefaultAddressMode,
                            AddressV = (TextureConstant.SamplerAddressModeEnum)Parameters[parameterIndex].DefaultAddressMode
                        },
                        FilterMode = (TextureConstant.SamplerFilterMode)Parameters[parameterIndex].DefaultFilterMode,
                        ExternTextureMode = TextureConstant.RenderMethodExternTextureMode.UseBitmapAsNormal,
                        TextureTransformConstantIndex = -1,
                        TextureTransformOverlayIndices = new TagBlockIndex()
                    });
                }
            }

            // convert filter mode (apply to all)
            if (BlamCache.Version == CacheVersion.Halo3ODST || BlamCache.Version >= CacheVersion.HaloReach)
                RenderMethod.ShaderProperties[0].TextureConstants.ForEach(x => x.FilterMode = x.FilterModePacked.FilterMode);

            return textureConstants;
        }

        private List<RealConstant> ConvertRealConstants()
        {
            List<RealConstant> realConstants = new List<RealConstant>();

            for (int i = 0; i < Rmt2.RealParameterNames.Count; i++)
            {
                string name = Cache.StringTable.GetString(Rmt2.RealParameterNames[i].Name);
                int parameterMappingIndex = ParameterMappings.FindIndex(x => x.Name == name);

                if (parameterMappingIndex != -1)
                {
                    if (ParameterMappings[parameterMappingIndex].Type == ShaderParameter.RType.Sampler && 
                        ParameterMappings[parameterMappingIndex].RealIndex == -1) // sampler with no xform
                    {
                        realConstants.Add(new RealConstant
                        {
                            Arg0 = 1.0f,
                            Arg1 = 1.0f,
                            Arg2 = 0.0f,
                            Arg3 = 0.0f
                        });
                    }
                    else
                    {
                        if (ParameterMappings[parameterMappingIndex].RealIndex != -1)
                        {
                            realConstants.Add(RenderMethod.ShaderProperties[0].RealConstants[ParameterMappings[parameterMappingIndex].RealIndex]);
                        }
                        else if (ParameterMappings[parameterMappingIndex].IntIndex != -1)
                        {
                            float value = (float)RenderMethod.ShaderProperties[0].IntegerConstants[ParameterMappings[parameterMappingIndex].IntIndex];

                            realConstants.Add(new RealConstant
                            {
                                Arg0 = value,
                                Arg1 = value,
                                Arg2 = value,
                                Arg3 = value
                            });
                        }
                        else if (ParameterMappings[parameterMappingIndex].BoolIndex != -1)
                        {
                            int value = (((int)RenderMethod.ShaderProperties[0].BooleanConstants >> ParameterMappings[parameterMappingIndex].BoolIndex) & 1);

                            realConstants.Add(new RealConstant
                            {
                                Arg0 = value == 1 ? 1.0f : 0.0f,
                                Arg1 = value == 1 ? 1.0f : 0.0f,
                                Arg2 = value == 1 ? 1.0f : 0.0f,
                                Arg3 = value == 1 ? 1.0f : 0.0f
                            });
                        }
                        else
                        {
                            throw new Exception("shit code go fix it (real)");
                        }
                    }
                }
                else // use default value
                {
                    new TagToolWarning($"Shader converter: attempting default values for vector {name}");

                    int parameterIndex = Parameters.FindIndex(x => Cache.StringTable.GetString(x.Name) == name);

                    if (parameterIndex == -1) // this will crash for the meantime
                        new TagToolWarning($"Shader converter: real parameter \"{name}\" does not exist in rmop");

                    realConstants.Add(new RealConstant
                    {
                        Arg0 = Parameters[parameterIndex].DefaultFloatArgument,
                        Arg1 = Parameters[parameterIndex].DefaultFloatArgument,
                        Arg2 = Parameters[parameterIndex].DefaultFloatArgument,
                        Arg3 = Parameters[parameterIndex].DefaultFloatArgument
                    });
                }
            }

            return realConstants;
        }

        private List<uint> ConvertIntegerConstants()
        {
            List<uint> intConstants = new List<uint>();

            for (int i = 0; i < Rmt2.IntegerParameterNames.Count; i++)
            {
                string name = Cache.StringTable.GetString(Rmt2.IntegerParameterNames[i].Name);
                int parameterMappingIndex = ParameterMappings.FindIndex(x => x.Name == name);

                if (parameterMappingIndex != -1)
                {
                    if (ParameterMappings[parameterMappingIndex].IntIndex != -1)
                    {
                        intConstants.Add(RenderMethod.ShaderProperties[0].IntegerConstants[ParameterMappings[parameterMappingIndex].IntIndex]);
                    }
                    else if (ParameterMappings[parameterMappingIndex].RealIndex != -1)
                    {
                        intConstants.Add((uint)RenderMethod.ShaderProperties[0].RealConstants[ParameterMappings[parameterMappingIndex].RealIndex].Arg0);
                    }
                    else if (ParameterMappings[parameterMappingIndex].BoolIndex != -1)
                    {
                        int value = (((int)RenderMethod.ShaderProperties[0].BooleanConstants >> ParameterMappings[parameterMappingIndex].BoolIndex) & 1);
                        intConstants.Add((uint)value);
                    }
                    else
                    {
                        throw new Exception("shit code go fix it (int)");
                    }
                }
                else // use default value
                {
                    new TagToolWarning($"Shader converter: attempting default values for integer {name}");

                    int parameterIndex = Parameters.FindIndex(x => Cache.StringTable.GetString(x.Name) == name);

                    if (parameterIndex == -1) // this will crash for the meantime
                        new TagToolWarning($"Shader converter: real parameter \"{name}\" does not exist in rmop");

                    intConstants.Add(Parameters[parameterIndex].DefaultIntBoolArgument);
                }
            }

            return intConstants;
        }

        private uint ConvertBooleanConstants()
        {
            uint boolConstants = 0;

            for (int i = 0; i < Rmt2.IntegerParameterNames.Count; i++)
            {
                string name = Cache.StringTable.GetString(Rmt2.IntegerParameterNames[i].Name);
                int parameterMappingIndex = ParameterMappings.FindIndex(x => x.Name == name);

                if (parameterMappingIndex != -1)
                {
                    int value = 0;

                    if (ParameterMappings[parameterMappingIndex].BoolIndex != -1)
                        value = (((int)RenderMethod.ShaderProperties[0].BooleanConstants >> ParameterMappings[parameterMappingIndex].BoolIndex) & 1);
                    else if (ParameterMappings[parameterMappingIndex].RealIndex != -1)
                        value = (int)RenderMethod.ShaderProperties[0].RealConstants[ParameterMappings[parameterMappingIndex].RealIndex].Arg0;
                    else if (ParameterMappings[parameterMappingIndex].IntIndex != -1)
                        value = (int)RenderMethod.ShaderProperties[0].IntegerConstants[ParameterMappings[parameterMappingIndex].IntIndex];
                    else
                        throw new Exception("shit code go fix it (bool)");

                    boolConstants |= (uint)(value << i);
                }
                else // use default value
                {
                    new TagToolWarning($"Shader converter: attempting default values for bool {name}");

                    int parameterIndex = Parameters.FindIndex(x => Cache.StringTable.GetString(x.Name) == name);

                    if (parameterIndex == -1) // this will crash for the meantime
                        new TagToolWarning($"Shader converter: bool parameter \"{name}\" does not exist in rmop");

                    boolConstants |= (Parameters[parameterIndex].DefaultIntBoolArgument << i);
                }
            }

            return boolConstants;
        }

        private short[] ConvertQueryableProperties()
        {
            short[] queryableProperties = new short[8];
            short[] blamQueryableProperties = BlamCache.Version >= CacheVersion.HaloReach ? 
                RenderMethod.ShaderProperties[0].QueryablePropertiesReach : RenderMethod.ShaderProperties[0].QueryableProperties;

            for (int i = 0; i < queryableProperties.Length; i++)
            {
                if (blamQueryableProperties[i] == -1)
                {
                    queryableProperties[i] = -1;
                    continue;
                }

                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 5:
                        queryableProperties[i] = (short)BlamToBaseTextureIndices[blamQueryableProperties[i]];
                        break;
                    case 4:
                        queryableProperties[i] = (short)BlamToBaseRealIndices[blamQueryableProperties[i]];
                        break;
                    default:
                        queryableProperties[i] = -1;
                        break;
                }
            }

            return queryableProperties;
        }
    }
}
