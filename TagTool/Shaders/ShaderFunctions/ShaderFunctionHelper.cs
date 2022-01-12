using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders.ShaderFunctions
{
    class ShaderFunctionHelper
    {
        public enum ParameterType
        {
            Real,
            Texture,
            Int,
            Bool
        }

        public struct AnimatedParameter
        {
            public string Name;
            public ParameterType Type;
            public int FunctionIndex;
            public RenderMethod.RenderMethodAnimatedParameterBlock.FunctionType FunctionType;

            public int GetTemplateSourceIndex(GameCache cache, RenderMethodTemplate rmt2)
            {
                List<RenderMethodTemplate.ShaderArgument> block = null;

                switch (Type)
                {
                    case ParameterType.Real:
                        block = rmt2.RealParameterNames;
                        break;
                    case ParameterType.Int:
                        block = rmt2.IntegerParameterNames;
                        break;
                    case ParameterType.Bool:
                        block = rmt2.BooleanParameterNames;
                        break;
                    case ParameterType.Texture:
                        block = rmt2.TextureParameterNames;
                        break;
                }

                for (int i = 0; i < block.Count; i++)
                    if (cache.StringTable.GetString(block[i].Name) == Name)
                        return i;

                return -1;
            }

            public override bool Equals(object obj)
            {
                AnimatedParameter aP = (AnimatedParameter)obj;
                return Name == aP.Name && Type == aP.Type && FunctionIndex == aP.FunctionIndex && FunctionType == aP.FunctionType;
            }

            public override int GetHashCode() => (Name, Type, FunctionIndex, FunctionType).GetHashCode();

            public static bool operator ==(AnimatedParameter lhs, AnimatedParameter rhs) => lhs.Equals(rhs);

            public static bool operator !=(AnimatedParameter lhs, AnimatedParameter rhs) => !lhs.Equals(rhs);
        }

        /// <summary>
        /// Retrieves a list of all animated parameters in a RenderMethod.
        /// </summary>
        static public List<AnimatedParameter> GetAnimatedParameters(GameCache cache, RenderMethod renderMethod, RenderMethodTemplate template)
        {
            List<AnimatedParameter> result = new List<AnimatedParameter>();

            if (renderMethod.ShaderProperties.Count == 0 ||
                renderMethod.ShaderProperties[0].Functions.Count == 0 ||
                renderMethod.ShaderProperties[0].EntryPoints.Count == 0 ||
                renderMethod.ShaderProperties[0].Passes.Count == 0 ||
                renderMethod.ShaderProperties[0].RoutingInfo.Count == 0)
                return result;

            var properties = renderMethod.ShaderProperties[0];

            uint validEntries = EntryPointHelper.GetEntryMask(cache.Version, template);

            for (int i = 0; i < properties.EntryPoints.Count; i++)
            {
                if ((validEntries >> i & 1) == 0)
                    continue;

                var entry = properties.EntryPoints[i];
                if (entry.Count == 0 || entry.Offset >= properties.Passes.Count)
                    continue;

                for (int j = entry.Offset; j < entry.Offset + entry.Count; j++)
                {
                    var table = properties.Passes[j];
                    if (table.Texture.Count > 0)
                    {
                        for (int k = table.Texture.Offset; k < table.Texture.Offset + table.Texture.Count; k++)
                        {
                            var parameter = properties.RoutingInfo[k];
                            if (parameter.FunctionIndex < properties.Functions.Count)
                            {
                                string name = cache.StringTable.GetString(template.TextureParameterNames[parameter.SourceIndex].Name);

                                AnimatedParameter animatedParameter = new AnimatedParameter
                                {
                                    Name = name,
                                    Type = ParameterType.Texture,
                                    FunctionIndex = parameter.FunctionIndex,
                                    FunctionType = properties.Functions[parameter.FunctionIndex].Type
                                };

                                if (!result.Contains(animatedParameter))
                                {
                                    result.Add(animatedParameter);
                                }
                            }
                        }
                    }
                    if (table.RealPixel.Count > 0)
                    {
                        for (int k = table.RealPixel.Offset; k < table.RealPixel.Offset + table.RealPixel.Count; k++)
                        {
                            var parameter = properties.RoutingInfo[k];
                            if (parameter.FunctionIndex < properties.Functions.Count)
                            {
                                string name = cache.StringTable.GetString(template.RealParameterNames[parameter.SourceIndex].Name);

                                AnimatedParameter animatedParameter = new AnimatedParameter
                                {
                                    Name = name,
                                    Type = ParameterType.Real,
                                    FunctionIndex = parameter.FunctionIndex,
                                    FunctionType = properties.Functions[parameter.FunctionIndex].Type
                                };

                                if (!result.Contains(animatedParameter))
                                {
                                    result.Add(animatedParameter);
                                }
                            }
                        }
                    }
                    if (table.RealVertex.Count > 0)
                    {
                        for (int k = table.RealVertex.Offset; k < table.RealVertex.Offset + table.RealVertex.Count; k++)
                        {
                            var parameter = properties.RoutingInfo[k];
                            if (parameter.FunctionIndex < properties.Functions.Count)
                            {
                                string name = cache.StringTable.GetString(template.RealParameterNames[parameter.SourceIndex].Name);

                                AnimatedParameter animatedParameter = new AnimatedParameter
                                {
                                    Name = name,
                                    Type = ParameterType.Real,
                                    FunctionIndex = parameter.FunctionIndex,
                                    FunctionType = properties.Functions[parameter.FunctionIndex].Type
                                };

                                if (!result.Contains(animatedParameter))
                                {
                                    result.Add(animatedParameter);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Builds a list of animated parameters into a RenderMethod.
        /// </summary>
        static public bool BuildAnimatedParameters(GameCache cache, RenderMethod renderMethod, RenderMethodTemplate template, List<AnimatedParameter> animatedParameters)
        {
            var properties = renderMethod.ShaderProperties[0];
            uint validEntries = EntryPointHelper.GetEntryMask(cache.Version, template);

            properties.EntryPoints.Clear();
            properties.Passes.Clear();
            properties.RoutingInfo.Clear();

            for (int i = 0; i < template.EntryPoints.Count; i++)
                properties.EntryPoints.Add(new RenderMethodTemplate.TagBlockIndex());

            for (int i = 0; i < properties.EntryPoints.Count; i++)
            {
                if ((validEntries >> i & 1) == 0)
                    continue;

                properties.EntryPoints[i].Offset = (ushort)properties.Passes.Count;
                properties.EntryPoints[i].Count = 1; // one for now

                var table = new RenderMethod.RenderMethodPostprocessBlock.RenderMethodPostprocessPassBlock();

                List<RenderMethod.RenderMethodPostprocessBlock.RenderMethodRoutingInfoBlock> textureParameters = new List<RenderMethod.RenderMethodPostprocessBlock.RenderMethodRoutingInfoBlock>();
                List<RenderMethod.RenderMethodPostprocessBlock.RenderMethodRoutingInfoBlock> realPixelParameters = new List<RenderMethod.RenderMethodPostprocessBlock.RenderMethodRoutingInfoBlock>();
                List<RenderMethod.RenderMethodPostprocessBlock.RenderMethodRoutingInfoBlock> realVertexParameters = new List<RenderMethod.RenderMethodPostprocessBlock.RenderMethodRoutingInfoBlock>();

                foreach (var animParameter in animatedParameters)
                {
                    if (animParameter.Type == ParameterType.Texture)
                    {
                        var parameterMapping = GetParameterMapping(cache, template, animParameter.Name, (EntryPoint)i, ParameterUsage.Texture);

                        if (parameterMapping != null)
                        {
                            var rmParam = new RenderMethod.RenderMethodPostprocessBlock.RenderMethodRoutingInfoBlock
                            {
                                RegisterIndex = (short)parameterMapping.DestinationIndex,
                                SourceIndex = parameterMapping.SourceIndex,
                                FunctionIndex = (byte)animParameter.FunctionIndex
                            };

                            textureParameters.Add(rmParam);
                        }
                    }
                    else // real
                    {
                        bool PS = true;
                        var parameterMapping = GetParameterMapping(cache, template, animParameter.Name, (EntryPoint)i, ParameterUsage.PS_Real);
                        if (parameterMapping == null) // returns null if not found, so check for VS parameter (maybe need to store?)
                        {
                            PS = false;
                            parameterMapping = GetParameterMapping(cache, template, animParameter.Name, (EntryPoint)i, ParameterUsage.VS_Real);
                        }

                        if (parameterMapping != null)
                        {
                            var rmParam = new RenderMethod.RenderMethodPostprocessBlock.RenderMethodRoutingInfoBlock
                            {
                                RegisterIndex = (short)parameterMapping.DestinationIndex,
                                SourceIndex = parameterMapping.SourceIndex,
                                FunctionIndex = (byte)animParameter.FunctionIndex
                            };

                            if (PS)
                                realPixelParameters.Add(rmParam);
                            else
                                realVertexParameters.Add(rmParam);
                        }
                    }
                }

                // Texture
                int parametersOffset = properties.RoutingInfo.Count;
                properties.RoutingInfo.AddRange(textureParameters);

                if (properties.RoutingInfo.Count - parametersOffset > 0)
                {
                    table.Texture.Count = (ushort)(properties.RoutingInfo.Count - parametersOffset);
                    table.Texture.Offset = (ushort)parametersOffset;
                }

                // Real VS
                parametersOffset = properties.RoutingInfo.Count;
                properties.RoutingInfo.AddRange(realVertexParameters);

                if (properties.RoutingInfo.Count - parametersOffset > 0)
                {
                    table.RealVertex.Count = (ushort)(properties.RoutingInfo.Count - parametersOffset);
                    table.RealVertex.Offset = (ushort)parametersOffset;
                }

                // Real PS
                parametersOffset = properties.RoutingInfo.Count;
                properties.RoutingInfo.AddRange(realPixelParameters);

                if (properties.RoutingInfo.Count - parametersOffset > 0)
                {
                    table.RealPixel.Count = (ushort)(properties.RoutingInfo.Count - parametersOffset);
                    table.RealPixel.Offset = (ushort)parametersOffset;
                }

                properties.Passes.Add(table);
            }

            return true;
        }

        static private RenderMethodTemplate.RoutingInfoBlock GetParameterMapping(GameCache cache, RenderMethodTemplate template, string name, EntryPoint entry, ParameterUsage usage)
        {
            int maxIndex = template.EntryPoints[(int)entry].Offset + template.EntryPoints[(int)entry].Count;

            for (int i = template.EntryPoints[(int)entry].Offset; i < maxIndex; i++)
            {
                var value = template.Passes[i].Values[(int)usage];
                for (int j = value.Offset; j < value.Offset + value.Count; j++)
                {
                    RenderMethodTemplate.RoutingInfoBlock parameter = template.RoutingInfo[j];

                    string pName = null;
                    if (usage == ParameterUsage.Texture)
                        pName = cache.StringTable.GetString(template.TextureParameterNames[parameter.SourceIndex].Name);
                    else if (usage == ParameterUsage.PS_Real || usage == ParameterUsage.VS_Real)
                        pName = cache.StringTable.GetString(template.RealParameterNames[parameter.SourceIndex].Name);

                    if (pName == name)
                        return parameter;
                }
            }

            return null;
        }
    }
}
