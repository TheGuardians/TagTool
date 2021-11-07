using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagTool.IO;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Cache;

namespace TagTool.Shaders
{
    public class CompiledShaderInfo
    {
        public bool PixelShader;
        public bool ValidConstantTable;
        public byte[] ShaderData;   //xbox
        public byte[] DebugData;    //xbox
        public byte[] ConstantData; //xbox
        public List<ShaderParameter> Parameters;

        public CompiledShaderInfo(bool pixl, bool validConstantTable, byte[] shaderData, byte[] debugData, byte[] constantData, List<ShaderParameter> parameters)
        {
            PixelShader = pixl;
            ShaderData = shaderData;
            DebugData = debugData;
            ConstantData = constantData;
            Parameters = parameters;
            ValidConstantTable = validConstantTable;
        }

        public CompiledShaderInfo(bool pixl, bool validConstantTable, PixelShaderReference shaderRef, List<ShaderParameter> parameters)
        {
            PixelShader = pixl;
            ShaderData = shaderRef.ShaderData;
            DebugData = shaderRef.DebugData;
            ConstantData = shaderRef.ConstantData;
            Parameters = parameters;
            ValidConstantTable = validConstantTable;
        }

        public CompiledShaderInfo(bool pixl, bool validConstantTable, VertexShaderReference shaderRef, List<ShaderParameter> parameters)
        {
            PixelShader = pixl;
            ShaderData = shaderRef.ShaderData;
            DebugData = shaderRef.DebugData;
            ConstantData = shaderRef.ConstantData;
            Parameters = parameters;
            ValidConstantTable = validConstantTable;
        }

        private void ReorderParameter(List<ShaderParameter> newParameters, ShaderParameter.RType rType, int rMax)
        {
            var tempParams = Parameters.Where(x => x.RegisterType == rType);

            for (int i = 0; i <= rMax; i++)
                foreach (var param in tempParams)
                    if (param.RegisterIndex == i)
                        newParameters.Add(param);
        }

        public List<ShaderParameter> ReorderParameters()
        {
            List<ShaderParameter> newParameters = new List<ShaderParameter>();

            ReorderParameter(newParameters, ShaderParameter.RType.Boolean, 128);
            ReorderParameter(newParameters, ShaderParameter.RType.Integer, 16);
            ReorderParameter(newParameters, ShaderParameter.RType.Vector, 256);
            ReorderParameter(newParameters, ShaderParameter.RType.Sampler, 16);

            return newParameters;
        }

        public List<RealQuaternion> GetXboxShaderConstants()
        {
            var constants = new List<RealQuaternion>();

            using (var stream = new MemoryStream(ConstantData))
            using (var reader = new EndianReader(stream, EndianFormat.BigEndian))
            {
                for (var i = 0; i < ConstantData.Length / 16; i++)
                {
                    constants.Add(new RealQuaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
                }
                constants.Reverse(); //they are stored in reverse order
            }

            return constants;
        }

        public void GenerateParametersFromTemplate(GameCache cache, Stream cacheStream, int entryPointIndex, RenderMethodTemplate rmt2, RenderMethodDefinition rmdf, List<byte> optionIndices)
        {
            // Add regular extern if needed, afaik only reach has missing constant table info
            Dictionary<RenderMethodExternReach, StringId> externNames = new Dictionary<RenderMethodExternReach, StringId>();

            if (rmdf.GlobalOptions != null)
            {
                var rmop = cache.Deserialize<RenderMethodOption>(cacheStream, rmdf.GlobalOptions);

                foreach (var optionParam in rmop.Parameters)
                {
                    if (optionParam.RenderMethodExternReach != RenderMethodExternReach.none)
                    {
                        if (externNames.ContainsKey(optionParam.RenderMethodExternReach))
                            Console.WriteLine($"Duplicate extern \"{optionParam.RenderMethodExternReach}\"");
                        externNames[optionParam.RenderMethodExternReach] = optionParam.Name;
                    }
                }
            }

            for (int i = 0; i < optionIndices.Count; i++)
            {
                var optionBlock = rmdf.Categories[i].ShaderOptions[optionIndices[i]];

                if (optionBlock.Option != null)
                {
                    var rmop = cache.Deserialize<RenderMethodOption>(cacheStream, optionBlock.Option);

                    foreach (var optionParam in rmop.Parameters)
                    {
                        if (optionParam.RenderMethodExternReach != RenderMethodExternReach.none)
                        {
                            if (externNames.ContainsKey(optionParam.RenderMethodExternReach))
                                Console.WriteLine($"Duplicate extern \"{optionParam.RenderMethodExternReach}\"");
                            externNames[optionParam.RenderMethodExternReach] = optionParam.Name;
                        }
                    }
                }
            }

            List<string> textureNames = new List<string>();

            Parameters.Clear();

            for (int i = rmt2.EntryPoints[entryPointIndex].Offset; i < rmt2.EntryPoints[entryPointIndex].Offset + 1/*rmt2.EntryPoints[entryPointIndex].Count*/; i++)
            {
                var table = rmt2.Passes[i];

                for (ParameterUsage j = ParameterUsage.Texture; j < ParameterUsage.Count; j++)
                {
                    ShaderParameter.RType rType = ShaderParameter.RType.Boolean;

                    switch (j)
                    {
                        case ParameterUsage.PS_Real when PixelShader:
                            rType = ShaderParameter.RType.Vector;
                            break;
                        case ParameterUsage.PS_Integer when PixelShader:
                            rType = ShaderParameter.RType.Integer;
                            break;
                        case ParameterUsage.PS_Boolean when PixelShader:
                            rType = ShaderParameter.RType.Boolean;
                            break;
                        case ParameterUsage.PS_RealExtern when PixelShader:
                            rType = ShaderParameter.RType.Vector;
                            break;
                        case ParameterUsage.PS_IntegerExtern when PixelShader:
                            rType = ShaderParameter.RType.Integer;
                            break;
                        case ParameterUsage.VS_Real when !PixelShader:
                            rType = ShaderParameter.RType.Vector;
                            break;
                        case ParameterUsage.VS_Integer when !PixelShader:
                            rType = ShaderParameter.RType.Integer;
                            break;
                        case ParameterUsage.VS_Boolean when !PixelShader:
                            rType = ShaderParameter.RType.Boolean;
                            break;
                        case ParameterUsage.VS_RealExtern when !PixelShader:
                            rType = ShaderParameter.RType.Vector;
                            break;
                        case ParameterUsage.VS_IntegerExtern when !PixelShader:
                            rType = ShaderParameter.RType.Integer;
                            break;
                        case ParameterUsage.Texture:
                        case ParameterUsage.TextureExtern:
                            rType = ShaderParameter.RType.Sampler;
                            break;
                        default:
                            continue;
                    }

                    var oC = table.Values[(int)j];

                    for (int k = oC.Offset; k < oC.Offset + oC.Count; k++)
                    {
                        var parameter = rmt2.RoutingInfo[k];

                        StringId name = StringId.Invalid;
                        ushort register = parameter.DestinationIndex;

                        switch (j)
                        {
                            case ParameterUsage.Texture:
                                name = rmt2.TextureParameterNames[parameter.SourceIndex].Name;
                                register = (ushort)((parameter.Flags & 1) == 1 ? parameter.DestinationIndex + 16 : register);
                                break;
                            case ParameterUsage.PS_Real:
                                name = rmt2.RealParameterNames[parameter.SourceIndex].Name;
                                break;
                            case ParameterUsage.PS_Integer:
                                name = rmt2.IntegerParameterNames[parameter.SourceIndex].Name;
                                break;
                            case ParameterUsage.PS_Boolean:
                                name = rmt2.BooleanParameterNames[parameter.SourceIndex].Name;
                                break;
                            case ParameterUsage.TextureExtern:
                                name = externNames[(RenderMethodExternReach)parameter.SourceIndex];
                                register = (ushort)((parameter.Flags & 1) == 1 ? parameter.DestinationIndex + 16 : register);
                                break;
                            case ParameterUsage.VS_RealExtern:
                            case ParameterUsage.VS_IntegerExtern:
                            case ParameterUsage.PS_RealExtern:
                            case ParameterUsage.PS_IntegerExtern:
                                name = externNames[(RenderMethodExternReach)parameter.SourceIndex];
                                break;
                        }

                        if ((j == ParameterUsage.Texture || j == ParameterUsage.TextureExtern)
                            && register >= 16 && PixelShader)
                            continue;

                        ShaderParameter shaderParameter = new ShaderParameter
                        {
                            ParameterName = name,
                            RegisterCount = 1, // might cause issue, not sure how to determine for now.
                            RegisterIndex = register,
                            RegisterType = rType
                        };

                        bool dup = false;
                        foreach (var param in Parameters)
                        {
                            if (param.ParameterName == name && param.RegisterType == rType)
                            {
                                dup = true;
                                break;
                            }
                        }

                        if (!dup)
                            Parameters.Add(shaderParameter);
                    }
                }
            }
        }
    }
}
