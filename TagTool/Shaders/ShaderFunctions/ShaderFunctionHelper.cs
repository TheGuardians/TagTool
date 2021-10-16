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
            public RenderMethod.ShaderFunction.FunctionType FunctionType;
        }

        /// <summary>
        /// Returns true if an animated parameter with the supplied information exists in the list.
        /// </summary>
        static public bool AnimatedParameterExists(List<AnimatedParameter> animatedParameters, string name, ParameterType type, RenderMethod.ShaderFunction.FunctionType functionType)
        {
            foreach (var parameter in animatedParameters)
                if (parameter.Name == name && parameter.Type == type && parameter.FunctionType == functionType)
                    return true;

            return false;
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
                renderMethod.ShaderProperties[0].ParameterTables.Count == 0 ||
                renderMethod.ShaderProperties[0].Parameters.Count == 0)
                return result;

            var properties = renderMethod.ShaderProperties[0];

            uint validEntries = EntryPointHelper.GetEntryMask(cache.Version, template);

            for (int i = 0; i < properties.EntryPoints.Count; i++)
            {
                if ((validEntries >> i & 1) == 0)
                    continue;

                var entry = properties.EntryPoints[i];
                if (entry.Count == 0 || entry.Offset >= properties.ParameterTables.Count)
                    continue;

                for (int j = entry.Offset; j < entry.Offset + entry.Count; j++)
                {
                    var table = properties.ParameterTables[j];
                    if (table.Texture.Count > 0)
                    {
                        for (int k = table.Texture.Offset; k < table.Texture.Offset + table.Texture.Count; k++)
                        {
                            var parameter = properties.Parameters[k];
                            if (parameter.FunctionIndex < properties.Functions.Count)
                            {
                                string name = cache.StringTable.GetString(template.TextureParameterNames[parameter.SourceIndex].Name);

                                if (!AnimatedParameterExists(result, name, ParameterType.Texture, properties.Functions[parameter.FunctionIndex].Type))
                                {
                                    AnimatedParameter animatedParameter = new AnimatedParameter
                                    {
                                        Name = name,
                                        Type = ParameterType.Texture,
                                        FunctionIndex = parameter.FunctionIndex,
                                        FunctionType = properties.Functions[parameter.FunctionIndex].Type
                                    };
                                    
                                    result.Add(animatedParameter);
                                }
                            }
                        }
                    }
                    if (table.RealPixel.Count > 0)
                    {
                        for (int k = table.RealPixel.Offset; k < table.RealPixel.Offset + table.RealPixel.Count; k++)
                        {
                            var parameter = properties.Parameters[k];
                            if (parameter.FunctionIndex < properties.Functions.Count)
                            {
                                string name = cache.StringTable.GetString(template.RealParameterNames[parameter.SourceIndex].Name);

                                if (!AnimatedParameterExists(result, name, ParameterType.Real, properties.Functions[parameter.FunctionIndex].Type))
                                {
                                    AnimatedParameter animatedParameter = new AnimatedParameter
                                    {
                                        Name = name,
                                        Type = ParameterType.Real,
                                        FunctionIndex = parameter.FunctionIndex,
                                        FunctionType = properties.Functions[parameter.FunctionIndex].Type
                                    };

                                    result.Add(animatedParameter);
                                }
                            }
                        }
                    }
                    if (table.RealVertex.Count > 0)
                    {
                        for (int k = table.RealVertex.Offset; k < table.RealVertex.Offset + table.RealVertex.Count; k++)
                        {
                            var parameter = properties.Parameters[k];
                            if (parameter.FunctionIndex < properties.Functions.Count)
                            {
                                string name = cache.StringTable.GetString(template.RealParameterNames[parameter.SourceIndex].Name);

                                if (!AnimatedParameterExists(result, name, ParameterType.Real, properties.Functions[parameter.FunctionIndex].Type))
                                {
                                    AnimatedParameter animatedParameter = new AnimatedParameter
                                    {
                                        Name = name,
                                        Type = ParameterType.Real,
                                        FunctionIndex = parameter.FunctionIndex,
                                        FunctionType = properties.Functions[parameter.FunctionIndex].Type
                                    };

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
        static public bool BuildAnimatedParameters(GameCache cache, Stream stream, RenderMethod renderMethod, RenderMethodTemplate template, List<AnimatedParameter> animatedParameters)
        {
            var properties = renderMethod.ShaderProperties[0];
            uint validEntries = EntryPointHelper.GetEntryMask(cache.Version, template);

            properties.EntryPoints.Clear();
            properties.ParameterTables.Clear();
            properties.Parameters.Clear();

            for (int i = 0; i < template.EntryPoints.Count; i++)
                properties.EntryPoints.Add(new RenderMethodTemplate.TagBlockIndex());

            for (int i = 0; i < properties.EntryPoints.Count; i++)
            {
                if ((validEntries >> i & 1) == 0)
                    continue;

                properties.EntryPoints[i].Offset = (ushort)properties.ParameterTables.Count;
                properties.EntryPoints[i].Count = 1; // one for now

                var table = new RenderMethod.ShaderProperty.ParameterTable();

                List<RenderMethod.ShaderProperty.ParameterMapping> textureParameters = new List<RenderMethod.ShaderProperty.ParameterMapping>();
                List<RenderMethod.ShaderProperty.ParameterMapping> realPixelParameters = new List<RenderMethod.ShaderProperty.ParameterMapping>();
                List<RenderMethod.ShaderProperty.ParameterMapping> realVertexParameters = new List<RenderMethod.ShaderProperty.ParameterMapping>();

                foreach (var animParameter in animatedParameters)
                {
                    if (animParameter.Type == ParameterType.Texture)
                    {
                        var parameterMapping = GetParameterMapping(cache, template, animParameter.Name, (EntryPoint)i, ParameterUsage.Texture);

                        if (parameterMapping != null)
                        {
                            var rmParam = new RenderMethod.ShaderProperty.ParameterMapping
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
                            var rmParam = new RenderMethod.ShaderProperty.ParameterMapping
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
                int parametersOffset = properties.Parameters.Count;
                properties.Parameters.AddRange(textureParameters);

                if (properties.Parameters.Count - parametersOffset > 0)
                {
                    table.Texture.Count = (ushort)(properties.Parameters.Count - parametersOffset);
                    table.Texture.Offset = (ushort)parametersOffset;
                }

                // Real VS
                parametersOffset = properties.Parameters.Count;
                properties.Parameters.AddRange(realVertexParameters);

                if (properties.Parameters.Count - parametersOffset > 0)
                {
                    table.RealVertex.Count = (ushort)(properties.Parameters.Count - parametersOffset);
                    table.RealVertex.Offset = (ushort)parametersOffset;
                }

                // Real PS
                parametersOffset = properties.Parameters.Count;
                properties.Parameters.AddRange(realPixelParameters);

                if (properties.Parameters.Count - parametersOffset > 0)
                {
                    table.RealPixel.Count = (ushort)(properties.Parameters.Count - parametersOffset);
                    table.RealPixel.Offset = (ushort)parametersOffset;
                }

                properties.ParameterTables.Add(table);
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
