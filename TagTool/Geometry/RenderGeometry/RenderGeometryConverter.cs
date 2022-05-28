using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Commands.Common;
using TagTool.Serialization;
using TagTool.Havok;

namespace TagTool.Geometry
{
    public class RenderGeometryConverter
    {
        private GameCache DestCache { get; }
        private GameCache SourceCache;

        public RenderGeometryConverter(GameCache destCache, GameCache sourceCache)
        {
            DestCache = destCache;
            SourceCache = sourceCache;
        }

        /// <summary>
        /// Converts RenderGeometry class in place and returns a new RenderGeometryApiResourceDefinition
        /// </summary>
        public RenderGeometryApiResourceDefinition Convert(RenderGeometry geometry, RenderGeometryApiResourceDefinition resourceDefinition)
        {
            if(CacheVersionDetection.IsBetween(DestCache.Version, CacheVersion.HaloOnlineED, CacheVersion.HaloOnline106708))
            {
                if (CacheVersionDetection.IsBetween(SourceCache.Version, CacheVersion.Halo3Beta, CacheVersion.Halo3ODST))
                {
                    return ConvertHalo3(geometry, resourceDefinition);
                }
                else if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, SourceCache.Version))
                {
                    return ConvertHaloOnline(geometry, resourceDefinition);
                }
                else if (SourceCache.Version >= CacheVersion.HaloReach)
                {
                    return ConvertHaloReach(geometry, resourceDefinition);
                }
            }

            return null;
        }

        private RenderGeometryApiResourceDefinition ConvertHaloOnline(RenderGeometry geometry, RenderGeometryApiResourceDefinition resourceDefinition)
        {
            // The format changed starting with version 235640
            if (SourceCache.Version <= CacheVersion.HaloOnline106708)
                return resourceDefinition;
            foreach(var buffer in resourceDefinition.VertexBuffers)
            {
                using (MemoryStream inStream = new MemoryStream(buffer.Definition.Data.Data), outStream = new MemoryStream())
                {
                    var inVertexStream = VertexStreamFactory.Create(SourceCache.Version, SourceCache.Platform, inStream);
                    var outVertexStream = VertexStreamFactory.Create(DestCache.Version, DestCache.Platform, outStream);
                    switch (buffer.Definition.Format)
                    {
                        case VertexBufferFormat.World:
                            ConvertVertices(buffer.Definition.Count, inVertexStream.ReadWorldVertex, v =>
                            {
                                //v.Binormal = new RealVector3d(v.Position.W, v.Tangent.W, 0); // Converted shaders use this
                                outVertexStream.WriteWorldVertex(v);
                            });
                            break;
                        case VertexBufferFormat.Rigid:
                            ConvertVertices(buffer.Definition.Count, inVertexStream.ReadRigidVertex, v =>
                            {
                                //v.Binormal = new RealVector3d(v.Position.W, v.Tangent.W, 0); // Converted shaders use this
                                outVertexStream.WriteRigidVertex(v);
                            });
                            break;
                        case VertexBufferFormat.Skinned:
                            ConvertVertices(buffer.Definition.Count, inVertexStream.ReadSkinnedVertex, v =>
                            {
                                //v.Binormal = new RealVector3d(v.Position.W, v.Tangent.W, 0); // Converted shaders use this
                                outVertexStream.WriteSkinnedVertex(v);
                            });
                            break;
                        case VertexBufferFormat.StaticPerPixel:
                            ConvertVertices(buffer.Definition.Count, inVertexStream.ReadStaticPerPixelData, outVertexStream.WriteStaticPerPixelData);
                            break;
                        case VertexBufferFormat.StaticPerVertex:
                            ConvertVertices(buffer.Definition.Count, inVertexStream.ReadStaticPerVertexData, outVertexStream.WriteStaticPerVertexData);
                            break;
                        case VertexBufferFormat.AmbientPrt:
                            ConvertVertices(buffer.Definition.Count, inVertexStream.ReadAmbientPrtData, outVertexStream.WriteAmbientPrtData);
                            break;
                        case VertexBufferFormat.LinearPrt:
                            ConvertVertices(buffer.Definition.Count, inVertexStream.ReadLinearPrtData, outVertexStream.WriteLinearPrtData);
                            break;
                        case VertexBufferFormat.QuadraticPrt:
                            ConvertVertices(buffer.Definition.Count, inVertexStream.ReadQuadraticPrtData, outVertexStream.WriteQuadraticPrtData);
                            break;
                        case VertexBufferFormat.StaticPerVertexColor:
                            ConvertVertices(buffer.Definition.Count, inVertexStream.ReadStaticPerVertexColorData, outVertexStream.WriteStaticPerVertexColorData);
                            break;
                        case VertexBufferFormat.Decorator:
                            ConvertVertices(buffer.Definition.Count, inVertexStream.ReadDecoratorVertex, outVertexStream.WriteDecoratorVertex);
                            break;
                        case VertexBufferFormat.World2:
                            buffer.Definition.Format = VertexBufferFormat.World;
                            goto default;
                        default:
                            // Just copy the raw buffer over and pray that it works...
                            var bufferData = new byte[buffer.Definition.Data.Size];
                            inStream.Read(bufferData, 0, bufferData.Length);
                            outStream.Write(bufferData, 0, bufferData.Length);
                            break;
                    }
                    buffer.Definition.Data.Data = outStream.ToArray();
                }
            }
            return resourceDefinition;
        }
        private void ConvertVertices<T>(int count, Func<T> readFunc, Action<T> writeFunc)
        {
            for (var i = 0; i < count; i++)
                writeFunc(readFunc());
        }

        private RenderGeometryApiResourceDefinition ConvertHalo3(RenderGeometry geometry, RenderGeometryApiResourceDefinition resourceDefinition)
        {
            //
            // Convert Userdata
            //

            foreach (var block in geometry.UserData)
            {
                var data = block.Data;
                if (data != null || data.Length != 0)
                {
                    var result = new byte[data.Length];

                    using (var inputReader = new EndianReader(new MemoryStream(data), SourceCache.Endianness))
                    using (var outputWriter = new EndianWriter(new MemoryStream(result), DestCache.Endianness))
                    {
                        while (!inputReader.EOF)
                            outputWriter.Write(inputReader.ReadUInt32());

                        block.Data = result;
                    }
                }
            }

            //
            // Convert mopps in cluster visibility
            //

            foreach(var clusterVisibility in geometry.MeshClusterVisibility)
                clusterVisibility.MoppData = HavokConverter.ConvertHkpMoppData(SourceCache.Version, DestCache.Version, SourceCache.Platform, DestCache.Platform, clusterVisibility.MoppData);

            //
            // Port resource definition
            //

            var wasNull = false;
            if (resourceDefinition == null)
            {
                wasNull = true;
                Console.Error.WriteLine("Render geometry does not have a valid resource definition, continuing anyway.");
                resourceDefinition = new RenderGeometryApiResourceDefinition
                {
                    VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>(CacheAddressType.Definition),
                    IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(CacheAddressType.Definition)
                };
            }

            geometry.SetResourceBuffers(resourceDefinition);

            // do conversion (PARTICLE INDEX BUFFERS, WATER CONVERSION TO DO)

            var generateParticles = false; // temp fix when pmdf geo is null

            if (wasNull)
            {
                if (geometry.Meshes.Count == 1 && geometry.Meshes[0].Type == VertexType.ParticleModel)
                {
                    generateParticles = true;
                }
                else
                {
                    geometry.Resource = DestCache.ResourceCache.CreateRenderGeometryApiResource(resourceDefinition);
                    geometry.Resource.HaloOnlinePageableResource.Resource.ResourceType = TagResourceTypeGen3.None;
                    return resourceDefinition;
                }
            }

            //
            // Convert Blam data to ElDorado data
            //

            if (generateParticles)
            {
                var mesh = geometry.Meshes[0];
                mesh.Flags |= MeshFlags.MeshIsUnindexed;
                mesh.PrtType = PrtSHType.None;

                var newVertexBuffer = new VertexBufferDefinition
                {
                    Format = VertexBufferFormat.ParticleModel,
                    VertexSize = (short)VertexStreamFactory.Create(DestCache.Version, DestCache.Platform, null).GetVertexSize(VertexBufferFormat.ParticleModel),
                    Data = new TagData
                    {
                        Data = new byte[32],
                        AddressType = CacheAddressType.Data
                    }
                };
                mesh.ResourceVertexBuffers[0] = newVertexBuffer;
            }
            else
            {
                foreach (var mesh in geometry.Meshes)
                {
                    foreach (var vertexBuffer in mesh.ResourceVertexBuffers)
                    {
                        if (vertexBuffer == null)
                            continue;

                        // Gen3 order 0 coefficients are stored in ints but should be read as bytes, 1 per vertex in the original buffer
                        if (vertexBuffer.Format == VertexBufferFormat.AmbientPrt)
                            vertexBuffer.Count = mesh.ResourceVertexBuffers[0].Count;

                        // skip conversion of water vertices, done right after the loop
                        if (vertexBuffer.Format == VertexBufferFormat.WaterTriangleIndices || vertexBuffer.Format == VertexBufferFormat.TesselatedWaterParameters)
                            continue;
                        if (SourceCache.Platform == CachePlatform.MCC && vertexBuffer.Format == VertexBufferFormat.Unknown1C)
                            continue;

                        VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, SourceCache.Platform, DestCache.Version, DestCache.Platform, vertexBuffer);
                    }

                    // convert water vertex buffers
                    if(mesh.ResourceVertexBuffers[6] != null && mesh.ResourceVertexBuffers[7] != null && SourceCache.Platform == CachePlatform.Original)
                    {
                        ConvertWaterMesh(mesh);
                    }

                    foreach (var indexBuffer in mesh.ResourceIndexBuffers)
                    {
                        if (indexBuffer == null)
                            continue;

                        IndexBufferConverter.ConvertIndexBuffer(SourceCache.Version, DestCache.Version, SourceCache.Platform, DestCache.Platform, indexBuffer);
                    }

                    // create index buffers for decorators, gen3 didn't have them
                    if (mesh.Flags.HasFlag(MeshFlags.MeshIsUnindexed) && mesh.Type == VertexType.Decorator)
                    {
                        mesh.Flags &= ~MeshFlags.MeshIsUnindexed;

                        var indexCount = 0;

                        foreach (var part in mesh.Parts)
                            indexCount += part.IndexCount;

                        mesh.ResourceIndexBuffers[0] = IndexBufferConverter.CreateIndexBuffer(indexCount);
                    }
                    
                }
            }

            foreach (var perPixel in geometry.InstancedGeometryPerPixelLighting)
            {
                if(perPixel.VertexBuffer != null)
                    VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, SourceCache.Platform, DestCache.Version, DestCache.Platform, perPixel.VertexBuffer);
            }

            return geometry.GetResourceDefinition();
        }

        private void ConvertWaterMesh(Mesh mesh)
        {
            // Get total amount of indices and prepare for water conversion
            int indexCount = 0;
            foreach (var subpart in mesh.SubParts)
                indexCount += subpart.IndexCount;

            WaterConversionData waterData = new WaterConversionData();

            for (int j = 0; j < mesh.Parts.Count(); j++)
            {
                var part = mesh.Parts[j];
                bool perVertexLighting = part.FlagsReach.HasFlag(Part.PartFlagsReach.PerVertexLightmapPart);
                if (part.FlagsNew.HasFlag(Part.PartFlagsNew.IsWaterSurface) || part.FlagsReach.HasFlag(Part.PartFlagsReach.IsWaterSurface))
                    waterData.PartData.Add(new Tuple<int, int, bool>(part.FirstIndex, part.IndexCount, perVertexLighting));
            }

            if (waterData.PartData.Count > 1)
                waterData.Sort();

            // read all vertices into lists.
            List<WorldVertex> worldVertices = new List<WorldVertex>();
            List<WaterTesselatedParameters> h3WaterParameters = new List<WaterTesselatedParameters>();
            List<WaterTriangleIndices> h3WaterIndices = new List<WaterTriangleIndices>();
            List<StaticPerPixelData> staticPerPixel = new List<StaticPerPixelData>();

            using (var stream = new MemoryStream(mesh.ResourceVertexBuffers[0].Data.Data))
            {
                var vertexStream = VertexStreamFactory.Create(DestCache.Version, DestCache.Platform, stream);
                for (int v = 0; v < mesh.ResourceVertexBuffers[0].Count; v++)
                    worldVertices.Add(vertexStream.ReadWorldVertex());
            }
            using (var stream = new MemoryStream(mesh.ResourceVertexBuffers[6].Data.Data))
            {
                var vertexStream = VertexStreamFactory.Create(SourceCache.Version, SourceCache.Platform, stream);
                for (int v = 0; v < mesh.ResourceVertexBuffers[6].Count; v++)
                    h3WaterIndices.Add(vertexStream.ReadWaterTriangleIndices());
            }
            using (var stream = new MemoryStream(mesh.ResourceVertexBuffers[7].Data.Data))
            {
                var vertexStream = VertexStreamFactory.Create(SourceCache.Version, SourceCache.Platform, stream);
                for (int v = 0; v < mesh.ResourceVertexBuffers[7].Count; v++)
                    h3WaterParameters.Add(vertexStream.ReadWaterTesselatedParameters());
            }

            if (mesh.ResourceVertexBuffers[1] != null)
            {
                using (var stream = new MemoryStream(mesh.ResourceVertexBuffers[1].Data.Data))
                {
                    var vertexStream = VertexStreamFactory.Create(DestCache.Version, DestCache.Platform, stream);
                    for (int v = 0; v < mesh.ResourceVertexBuffers[1].Count; v++)
                        staticPerPixel.Add(vertexStream.ReadStaticPerPixelData());
                }
            }

            // create vertex buffer for WaterTriangleIndices -> World
            VertexBufferDefinition waterVertices = new VertexBufferDefinition
            {
                Count = indexCount,
                Format = VertexBufferFormat.World,
                Data = new TagData(),
                VertexSize = 0x38   // this size is actually wrong but I replicate the errors in HO data, size should be 0x34

            };

            // create vertex buffer for TesselatedWaterParameters
            VertexBufferDefinition waterParameters = new VertexBufferDefinition
            {
                Count = indexCount,
                Format = VertexBufferFormat.TesselatedWaterParameters,
                Data = new TagData(),
                VertexSize = 0x24   // wrong size, this is 0x18 on file, padded with zeroes.
            };

            using (var outputWorldWaterStream = new MemoryStream())
            using (var outputWaterParametersStream = new MemoryStream())
            {
                var outWorldVertexStream = VertexStreamFactory.Create(DestCache.Version, DestCache.Platform, outputWorldWaterStream);
                var outWaterParameterVertexStream = VertexStreamFactory.Create(DestCache.Version, DestCache.Platform, outputWaterParametersStream);

                // fill vertex buffer to the right size HO expects, then write the vertex data at the actual proper position
                VertexBufferConverter.DebugFill(outputWorldWaterStream, waterVertices.VertexSize * waterVertices.Count);
                VertexBufferConverter.Fill(outputWaterParametersStream, waterParameters.VertexSize * waterParameters.Count);

                var triangleBaseIndex = 0; // WaterTriangleIndices are not separated into parts, if a mesh has multiple parts we need to get the right WaterTriangleIndices

                for (int k = 0; k < waterData.PartData.Count(); k++)
                {
                    Tuple<int, int, bool> currentPartData = waterData.PartData[k];

                    //seek to the right location in the buffer
                    outputWorldWaterStream.Position = 0x34 * currentPartData.Item1;
                    outputWaterParametersStream.Position = 0x18 * currentPartData.Item1;

                    for (int v = 0; v < currentPartData.Item2; v += 3)
                    {
                        var triangleIndices = h3WaterIndices[(v / 3) + triangleBaseIndex];
                        for (int j = 0; j < 3; j++)
                        {
                            var worldVertex = worldVertices[triangleIndices.MeshIndices[j]];
                            var tesselatedParameters = h3WaterParameters[triangleIndices.WaterIndices[j]];

                            StaticPerPixelData spp;
                            if (SourceCache.Version >= CacheVersion.HaloReach && (triangleIndices.MeshIndices[j] >= staticPerPixel.Count || !currentPartData.Item3))
                                spp = new StaticPerPixelData() { Texcoord = new RealVector2d(0, 0) };
                            else
                                spp = staticPerPixel[triangleIndices.MeshIndices[j]];

                            var worldWaterVertex = new WorldWaterVertex()
                            {
                                Position = worldVertex.Position,
                                Binormal = worldVertex.Binormal,
                                Normal = worldVertex.Normal,
                                Tangent = worldVertex.Tangent,
                                Texcoord = worldVertex.Texcoord,
                                StaticPerPixel = spp.Texcoord
                            };

                            outWorldVertexStream.WriteWorldWaterVertex(worldWaterVertex);
                            outWaterParameterVertexStream.WriteWaterTesselatedParameters(tesselatedParameters);
                        }
                    }
                    triangleBaseIndex += currentPartData.Item2 / 3;    // tells next part we read those indices already
                }
                waterVertices.Data.Data = outputWorldWaterStream.ToArray();
                waterParameters.Data.Data = outputWaterParametersStream.ToArray();
            }

            mesh.ResourceVertexBuffers[6] = waterVertices;
            mesh.ResourceVertexBuffers[7] = waterParameters;
        }

        private static VertexType ConvertReachVertexType(VertexTypeReach reachType)
        {
            switch (reachType)
            {
                case VertexTypeReach.World:
                case VertexTypeReach.WorldTesselated:
                    return VertexType.World;

                case VertexTypeReach.Rigid:
                case VertexTypeReach.RigidTesselated:
                case VertexTypeReach.RigidCompressed:
                    return VertexType.Rigid;

                case VertexTypeReach.Skinned:
                case VertexTypeReach.SkinnedTesselated:
                case VertexTypeReach.SkinnedCompressed:
                    return VertexType.Skinned;

                case VertexTypeReach.ParticleModel:
                    return VertexType.ParticleModel;
                case VertexTypeReach.FlatWorld:
                    return VertexType.FlatWorld;
                case VertexTypeReach.FlatRigid:
                    return VertexType.FlatRigid;
                case VertexTypeReach.FlatSkinned:
                    return VertexType.FlatSkinned;
                case VertexTypeReach.Screen:
                    return VertexType.Screen;
                case VertexTypeReach.Debug:
                    return VertexType.Debug;
                case VertexTypeReach.Transparent:
                    return VertexType.Transparent;
                case VertexTypeReach.Particle:
                    return VertexType.Particle;
                case VertexTypeReach.Contrail:
                    return VertexType.Contrail;
                case VertexTypeReach.LightVolume:
                    return VertexType.LightVolume;
                case VertexTypeReach.SimpleChud:
                    return VertexType.SimpleChud;
                case VertexTypeReach.FancyChud:
                    return VertexType.FancyChud;
                case VertexTypeReach.Decorator:
                    return VertexType.Decorator;
                case VertexTypeReach.TinyPosition:
                    return VertexType.TinyPosition;
                case VertexTypeReach.PatchyFog:
                    return VertexType.PatchyFog;
                case VertexTypeReach.Water:
                    return VertexType.Water;
                case VertexTypeReach.Ripple:
                    return VertexType.Ripple;

                case VertexTypeReach.Implicit:
                    return VertexType.Implicit;
                case VertexTypeReach.Beam:
                    return VertexType.Beam;

                default:
                case VertexTypeReach.ShaderCache:
                case VertexTypeReach.InstanceImposter:
                case VertexTypeReach.ObjectImposter:
                case VertexTypeReach.LightVolumePreCompiled:
                    throw new Exception($"Unsupported vertex format {reachType}, ask the nearest dev to look into it.");
            }
        }

        private RenderGeometryApiResourceDefinition ConvertHaloReach(RenderGeometry geometry, RenderGeometryApiResourceDefinition resourceDefinition)
        {
            // TODO: Find out why this flag is ticked, or if it is a definition change.
            geometry.RuntimeFlags &= ~RenderGeometryRuntimeFlags.DoNotUseCompressedVertexPositions;

            //
            // Update Mesh.Parts
            //

            foreach(var mesh in geometry.Meshes)
            {
                mesh.Flags = mesh.FlagsReach.ConvertLexical<MeshFlags>();

                foreach (var part in mesh.Parts)
                {
                    part.FlagsNew = part.FlagsReach.ConvertLexical<Part.PartFlagsNew>();
                }
            }


            //
            // Convert byte[] of UnknownBlock
            //

            foreach (var block in geometry.UserData)
            {
                var data = block.Data;
                if (data != null || data.Length != 0)
                {
                    var result = new byte[data.Length];

                    using (var inputReader = new EndianReader(new MemoryStream(data), SourceCache.Endianness))
                    using (var outputWriter = new EndianWriter(new MemoryStream(result), DestCache.Endianness))
                    {
                        while (!inputReader.EOF)
                            outputWriter.Write(inputReader.ReadUInt32());

                        block.Data = result;
                    }
                }
            }

            //
            // Convert mopps in cluster visibility
            //

            foreach (var clusterVisibility in geometry.MeshClusterVisibility)
                clusterVisibility.MoppData = HavokConverter.ConvertHkpMoppData(SourceCache.Version, DestCache.Version, SourceCache.Platform, DestCache.Platform, clusterVisibility.MoppData);

            //
            // Port resource definition
            //

            var wasNull = false;
            if (resourceDefinition == null)
            {
                wasNull = true;
                Console.Error.WriteLine("Render geometry does not have a valid resource definition, continuing anyway.");
                resourceDefinition = new RenderGeometryApiResourceDefinition
                {
                    VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>(CacheAddressType.Definition),
                    IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(CacheAddressType.Definition)
                };
            }

            geometry.SetResourceBuffers(resourceDefinition);

            var generateParticles = false; // temp fix when pmdf geo is null

            if (wasNull)
            {
                if (geometry.Meshes.Count == 1 && geometry.Meshes[0].Type == VertexType.ParticleModel)
                {
                    generateParticles = true;
                }
                else
                {
                    geometry.Resource = DestCache.ResourceCache.CreateRenderGeometryApiResource(resourceDefinition);
                    geometry.Resource.HaloOnlinePageableResource.Resource.ResourceType = TagResourceTypeGen3.None;
                    return resourceDefinition;
                }
            }

            //
            // Convert Blam data to ElDorado data
            //

            if (generateParticles)
            {
                var mesh = geometry.Meshes[0];
                mesh.Flags |= MeshFlags.MeshIsUnindexed;
                mesh.PrtType = PrtSHType.None;

                var newVertexBuffer = new VertexBufferDefinition
                {
                    Format = VertexBufferFormat.ParticleModel,
                    VertexSize = (short)VertexStreamFactory.Create(DestCache.Version, DestCache.Platform, null).GetVertexSize(VertexBufferFormat.ParticleModel),
                    Data = new TagData
                    {
                        Data = new byte[32],
                        AddressType = CacheAddressType.Data
                    }
                };
                mesh.ResourceVertexBuffers[0] = newVertexBuffer;
            }
            else
            {
                foreach (var mesh in geometry.Meshes)
                {
                    mesh.Type = ConvertReachVertexType(mesh.ReachType);
                    if(mesh.PrtType != PrtSHType.Ambient)
                        mesh.PrtType = PrtSHType.None;

                    for (int i = 0; i < mesh.ResourceVertexBuffers.Length; i++)
                    {
                        var vertexBuffer = mesh.ResourceVertexBuffers[i];

                        if (vertexBuffer == null)
                            continue;

                        // Gen3 order 0 coefficients are stored in ints but should be read as bytes, 1 per vertex in the original buffer
                        if (vertexBuffer.Format == VertexBufferFormat.AmbientPrt)
                            vertexBuffer.Count = mesh.ResourceVertexBuffers[0].Count;

                        if (vertexBuffer.Format == VertexBufferFormat.LinearPrt || vertexBuffer.Format == VertexBufferFormat.QuadraticPrt)
                        {
                            mesh.ResourceVertexBuffers[i] = null;
                            continue;
                        }

                        // skip conversion of water vertices, done right after the loop
                        if (vertexBuffer.Format == VertexBufferFormat.WaterTriangleIndices || vertexBuffer.Format == VertexBufferFormat.TesselatedWaterParameters)
                            continue;
 
                        if (SourceCache.Platform == CachePlatform.MCC && vertexBuffer.Format == VertexBufferFormat.Unknown1C)
                            continue;

                        VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, SourceCache.Platform, DestCache.Version, DestCache.Platform, vertexBuffer);
                    }

                    // convert water vertex buffers
                    if (mesh.ResourceVertexBuffers[6] != null && mesh.ResourceVertexBuffers[7] != null && SourceCache.Platform == CachePlatform.Original)
                    {
                        ConvertWaterMesh(mesh);
                    }

                    foreach (var indexBuffer in mesh.ResourceIndexBuffers)
                    {
                        if (indexBuffer == null)
                            continue;

                        IndexBufferConverter.ConvertIndexBuffer(SourceCache.Version, DestCache.Version, SourceCache.Platform, DestCache.Platform, indexBuffer);
                    }

                    // create index buffers for decorators, gen3 didn't have them
                    if (mesh.Flags.HasFlag(MeshFlags.MeshIsUnindexed) && mesh.Type == VertexType.Decorator)
                    {
                        mesh.Flags &= ~MeshFlags.MeshIsUnindexed;

                        var indexCount = 0;

                        foreach (var part in mesh.Parts)
                            indexCount += part.IndexCount;

                        mesh.ResourceIndexBuffers[0] = IndexBufferConverter.CreateIndexBuffer(indexCount);
                    }

                }
            }

            foreach (var perPixel in geometry.InstancedGeometryPerPixelLighting)
            {
                if (perPixel.VertexBuffer != null)
                    VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, SourceCache.Platform, DestCache.Version, DestCache.Platform, perPixel.VertexBuffer);
            }

            return geometry.GetResourceDefinition();
        }

        private class WaterConversionData
        {
            // offset, count of vertices to write, per vertex lighting
            public List<Tuple<int, int, bool>> PartData;

            public WaterConversionData()
            {
                PartData = new List<Tuple<int, int, bool>>();
            }

            public void Sort()
            {
                PartData.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            }
        }
    }
}