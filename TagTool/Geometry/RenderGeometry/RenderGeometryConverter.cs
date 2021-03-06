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
using TagTool.Commands.Porting;
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
            if(DestCache.Version == CacheVersion.HaloOnline106708)
            {
                if(CacheVersionDetection.IsBetween(SourceCache.Version, CacheVersion.Halo3Beta, CacheVersion.Halo3ODST))
                {
                    return ConvertHalo3(geometry, resourceDefinition);
                }
                else if(SourceCache.Version == CacheVersion.HaloReach)
                {
                    return ConvertHaloReach(geometry, resourceDefinition);
                }
            }

            return null;
        }

        private RenderGeometryApiResourceDefinition ConvertHalo3(RenderGeometry geometry, RenderGeometryApiResourceDefinition resourceDefinition)
        {
            //
            // Convert byte[] of UnknownBlock
            //

            foreach (var block in geometry.Unknown2)
            {
                var data = block.Unknown3;
                if (data != null || data.Length != 0)
                {
                    var result = new byte[data.Length];

                    using (var inputReader = new EndianReader(new MemoryStream(data), SourceCache.Endianness))
                    using (var outputWriter = new EndianWriter(new MemoryStream(result), DestCache.Endianness))
                    {
                        while (!inputReader.EOF)
                            outputWriter.Write(inputReader.ReadUInt32());

                        block.Unknown3 = result;
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
                    VertexSize = (short)VertexStreamFactory.Create(DestCache.Version, null).GetVertexSize(VertexBufferFormat.ParticleModel),
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
                        if (vertexBuffer.Format == VertexBufferFormat.Unknown1A || vertexBuffer.Format == VertexBufferFormat.Unknown1B)
                            continue;

                        VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, DestCache.Version, vertexBuffer);
                    }

                    // convert water vertex buffers
                    if(mesh.ResourceVertexBuffers[6] != null && mesh.ResourceVertexBuffers[7] != null)
                    {
                        // Get total amount of indices and prepare for water conversion

                        int indexCount = 0;
                        foreach (var subpart in mesh.SubParts)
                            indexCount += subpart.IndexCount;

                        WaterConversionData waterData = new WaterConversionData();

                        for (int j = 0; j < mesh.Parts.Count(); j++)
                        {
                            var part = mesh.Parts[j];
                            if(part.FlagsNew.HasFlag(Part.PartFlagsNew.IsWaterPart))
                                waterData.PartData.Add(new Tuple<int, int>(part.FirstIndexOld, part.IndexCountOld));
                        }

                        if(waterData.PartData.Count > 1)
                            waterData.Sort();

                        // read all world vertices, unknown1A and unknown1B into lists.
                        List<WorldVertex> worldVertices = new List<WorldVertex>();
                        List<Unknown1B> h3WaterParameters = new List<Unknown1B>();
                        List<Unknown1A> h3WaterIndices = new List<Unknown1A>();
                        List<StaticPerPixelData> staticPerPixel = new List<StaticPerPixelData>();

                        using (var stream = new MemoryStream(mesh.ResourceVertexBuffers[0].Data.Data))
                        {
                            var vertexStream = VertexStreamFactory.Create(DestCache.Version, stream);
                            for(int v = 0; v < mesh.ResourceVertexBuffers[0].Count; v++)
                                worldVertices.Add(vertexStream.ReadWorldVertex());
                        }
                        using (var stream = new MemoryStream(mesh.ResourceVertexBuffers[6].Data.Data))
                        {
                            var vertexStream = VertexStreamFactory.Create(SourceCache.Version, stream);
                            for (int v = 0; v < mesh.ResourceVertexBuffers[6].Count; v++)
                                h3WaterIndices.Add(vertexStream.ReadUnknown1A());
                        }
                        using (var stream = new MemoryStream(mesh.ResourceVertexBuffers[7].Data.Data))
                        {
                            var vertexStream = VertexStreamFactory.Create(SourceCache.Version, stream);
                            for (int v = 0; v < mesh.ResourceVertexBuffers[7].Count; v++)
                                h3WaterParameters.Add(vertexStream.ReadUnknown1B());
                        }
                        using (var stream = new MemoryStream(mesh.ResourceVertexBuffers[1].Data.Data))
                        {
                            var vertexStream = VertexStreamFactory.Create(DestCache.Version, stream);
                            for (int v = 0; v < mesh.ResourceVertexBuffers[1].Count; v++)
                                staticPerPixel.Add(vertexStream.ReadStaticPerPixelData());
                        }

                        // create vertex buffer for Unknown1A -> World
                        VertexBufferDefinition waterVertices = new VertexBufferDefinition
                        {
                            Count = indexCount,
                            Format = VertexBufferFormat.World,
                            Data = new TagData(),
                            VertexSize = 0x38   // this size is actually wrong but I replicate the errors in HO data, size should be 0x34
                            
                        };

                        // create vertex buffer for Unknown1B
                        VertexBufferDefinition waterParameters = new VertexBufferDefinition
                        {
                            Count = indexCount,
                            Format = VertexBufferFormat.Unknown1B,
                            Data = new TagData(),
                            VertexSize = 0x24   // wrong size, this is 0x18 on file, padded with zeroes.
                        };

                        using (var outputWorldWaterStream = new MemoryStream())
                        using(var outputWaterParametersStream = new MemoryStream())
                        {
                            var outWorldVertexStream = VertexStreamFactory.Create(DestCache.Version, outputWorldWaterStream);
                            var outWaterParameterVertexStream = VertexStreamFactory.Create(DestCache.Version, outputWaterParametersStream);

                            // fill vertex buffer to the right size HO expects, then write the vertex data at the actual proper position
                            VertexBufferConverter.DebugFill(outputWorldWaterStream, waterVertices.VertexSize * waterVertices.Count);
                            VertexBufferConverter.Fill(outputWaterParametersStream, waterParameters.VertexSize * waterParameters.Count);

                            var unknown1ABaseIndex = 0; // unknown1A are not separated into parts, if a mesh has multiple parts we need to get the right unknown1As

                            for (int k = 0; k < waterData.PartData.Count(); k++)
                            {
                                Tuple<int, int> currentPartData = waterData.PartData[k];

                                //seek to the right location in the buffer
                                outputWorldWaterStream.Position = 0x34 * currentPartData.Item1;
                                outputWaterParametersStream.Position = 0x18 * currentPartData.Item1;

                                for(int v = 0; v < currentPartData.Item2; v +=3)
                                {
                                    var unknown1A = h3WaterIndices[(v / 3) + unknown1ABaseIndex];
                                    for (int j = 0; j < 3; j++)
                                    {
                                        var worldVertex = worldVertices[unknown1A.Vertices[j]];
                                        var unknown1B = h3WaterParameters[unknown1A.Indices[j]];
                                        var spp = staticPerPixel[unknown1A.Vertices[j]];

                                        var worldWaterVertex = new WorldWaterVertex()
                                        {
                                            Position = worldVertex.Position,
                                            Binormal = worldVertex.Binormal,
                                            Normal = worldVertex.Normal,
                                            Tangent = worldVertex.Tangent,
                                            Texcoord = worldVertex.Texcoord,
                                            StaticPerPixel = spp.Texcoord
                                        };

                                        // conversion should happen here
                                        
                                        outWorldVertexStream.WriteWorldWaterVertex(worldWaterVertex);
                                        outWaterParameterVertexStream.WriteUnknown1B(unknown1B);
                                    }
                                }
                                unknown1ABaseIndex += currentPartData.Item2 / 3;    // tells next part we read those indices already
                            }
                            waterVertices.Data.Data = outputWorldWaterStream.ToArray();
                            waterParameters.Data.Data = outputWaterParametersStream.ToArray();
                        }

                        mesh.ResourceVertexBuffers[6] = waterVertices;
                        mesh.ResourceVertexBuffers[7] = waterParameters;
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
                            indexCount += part.IndexCountOld;

                        mesh.ResourceIndexBuffers[0] = IndexBufferConverter.CreateIndexBuffer(indexCount);
                    }
                    
                }
            }

            foreach (var perPixel in geometry.InstancedGeometryPerPixelLighting)
            {
                if(perPixel.VertexBuffer != null)
                    VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, DestCache.Version, perPixel.VertexBuffer);
            }

            return geometry.GetResourceDefinition();
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
            //
            // Update Mesh.Parts
            //

            foreach(var mesh in geometry.Meshes)
            {
                foreach(var part in mesh.Parts)
                {
                    part.FirstIndexOld = (ushort)part.FirstIndexNew;
                    part.IndexCountOld = (ushort)part.IndexCountNew;

                    if(part.IndexCountNew > 0xFFFF || part.FirstIndexNew > 0xFFFF)
                    {
                        Console.WriteLine("Warning: downgrade from uint to ushort for index count/first index in Mesh.Part exceeded ushort range. Unexpected behavior.");
                    }
                }
            }


            //
            // Convert byte[] of UnknownBlock
            //

            foreach (var block in geometry.Unknown2)
            {
                var data = block.Unknown3;
                if (data != null || data.Length != 0)
                {
                    var result = new byte[data.Length];

                    using (var inputReader = new EndianReader(new MemoryStream(data), SourceCache.Endianness))
                    using (var outputWriter = new EndianWriter(new MemoryStream(result), DestCache.Endianness))
                    {
                        while (!inputReader.EOF)
                            outputWriter.Write(inputReader.ReadUInt32());

                        block.Unknown3 = result;
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
                    VertexSize = (short)VertexStreamFactory.Create(DestCache.Version, null).GetVertexSize(VertexBufferFormat.ParticleModel),
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
                    mesh.PrtType = PrtSHType.None;
                    mesh.Type = ConvertReachVertexType(mesh.ReachType);


                    for(int i = 0; i < mesh.ResourceVertexBuffers.Length; i++)
                    {
                        var vertexBuffer = mesh.ResourceVertexBuffers[i];

                        if (vertexBuffer == null)
                            continue;

                        // Skip any kind of prt for now (VMF->SH basis conversion required), ambient also requires the fix for the vertex count
                        if (vertexBuffer.Format == VertexBufferFormat.AmbientPrt || vertexBuffer.Format == VertexBufferFormat.LinearPrt || vertexBuffer.Format == VertexBufferFormat.QuadraticPrt)
                        {
                            mesh.ResourceVertexBuffers[i] = null;
                            continue;
                        }

                        // Skip all lightmap related buffers due to VMF incompability with SH. StaticPerVertexColor is fine though.
                        if(vertexBuffer.Format == VertexBufferFormat.StaticPerPixel || vertexBuffer.Format == VertexBufferFormat.StaticPerVertex)
                        {
                            mesh.ResourceVertexBuffers[i] = null;
                            continue;
                        }

                        VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, DestCache.Version, vertexBuffer);
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
                            indexCount += part.IndexCountOld;

                        mesh.ResourceIndexBuffers[0] = IndexBufferConverter.CreateIndexBuffer(indexCount);
                    }

                }
            }

            foreach (var perPixel in geometry.InstancedGeometryPerPixelLighting)
            {
                if (perPixel.VertexBuffer != null)
                    VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, DestCache.Version, perPixel.VertexBuffer);
            }

            return geometry.GetResourceDefinition();
        }

        private class WaterConversionData
        {
            // offset, count of vertices to write
            public List<Tuple<int, int>> PartData;

            public WaterConversionData()
            {
                PartData = new List<Tuple<int, int>>();
            }

            public void Sort()
            {
                PartData.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            }
        }
    }
}