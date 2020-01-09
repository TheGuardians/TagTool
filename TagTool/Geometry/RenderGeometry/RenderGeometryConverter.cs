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

namespace TagTool.Geometry
{
    public class RenderGeometryConverter
    {
        private GameCacheContextHaloOnline HOCache { get; }
        private GameCache SourceCache;
        private List<long> OriginalBufferOffsets;
        private List<ushort> Unknown1BIndices;
        private List<WaterConversionData> WaterData;
        private int CurrentWaterBuffer;

        public RenderGeometryConverter(GameCacheContextHaloOnline hoCache, GameCache sourceCache)
        {
            HOCache = hoCache;
            SourceCache = sourceCache;
            OriginalBufferOffsets = new List<long>();
            Unknown1BIndices = new List<ushort>();
            WaterData = new List<WaterConversionData>();
            CurrentWaterBuffer = 0;
        }

        public RenderGeometry Convert(Stream cacheStream, RenderGeometry geometry, Dictionary<ResourceLocation, Stream> resourceStreams, PortTagCommand.PortingFlags portingFlags)
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

                    using (var inputReader = new EndianReader(new MemoryStream(data), EndianFormat.BigEndian))
                    using (var outputWriter = new EndianWriter(new MemoryStream(result), EndianFormat.LittleEndian))
                    {
                        while (!inputReader.EOF)
                            outputWriter.Write(inputReader.ReadUInt32());

                        block.Unknown3 = result;
                    }
                }
            }

            //
            // Convert UnknownSection.Unknown byte[] endian
            //

            for (int i = 0; i < geometry.UnknownSections.Count; i++)
            {
                byte[] dataref = geometry.UnknownSections[i].Unknown;

                if (dataref.Length == 0)
                    continue;

                using (var outStream = new MemoryStream())
                using (var outReader = new BinaryReader(outStream))
                using (var outWriter = new EndianWriter(outStream, EndianFormat.LittleEndian))
                using (var stream = new MemoryStream(dataref))
                using (var reader = new EndianReader(stream, EndianFormat.BigEndian))
                {
                    var dataContext = new DataSerializationContext(reader, outWriter);
                    var header = HOCache.Deserializer.Deserialize<ScenarioLightmapBspDataSection.Header>(dataContext);

                    var section = new ScenarioLightmapBspDataSection
                    {
                        Headers = new List<ScenarioLightmapBspDataSection.Header>
                        {
                            header
                        },
                        VertexLists = new ScenarioLightmapBspDataSection.VertexList
                        {
                            Vertex = new List<ScenarioLightmapBspDataSection.VertexList.Datum>()
                        }
                    };

                    HOCache.Serializer.Serialize(dataContext, header);

                    while (reader.BaseStream.Position < dataref.Length) // read the rest of dataref
                    {
                        if (section.Headers.Count == 2) // remove "wrongfully" added ones
                            section.Headers.RemoveAt(1);

                        section.Headers.Add(HOCache.Deserializer.Deserialize<ScenarioLightmapBspDataSection.Header>(dataContext));

                        // if some values match header1, continue
                        if (section.Headers[0].Position == section.Headers[1].Position)
                        {
                            header = section.Headers[1];
                            HOCache.Serializer.Serialize(dataContext, header);

                            while (reader.BaseStream.Position < dataref.Length)
                            {
                                section.VertexLists.Vertex.Add(new ScenarioLightmapBspDataSection.VertexList.Datum { Value = reader.ReadByte() });
                                outWriter.Write(section.VertexLists.Vertex[section.VertexLists.Vertex.Count - 1].Value);
                            }
                        }
                        else // if read data doesn't match, go back and just read 4 bytes
                        {
                            reader.BaseStream.Position = reader.BaseStream.Position - 0x2C; // if read data doesn't match, go back and serialize 

                            section.VertexLists.Vertex.Add(new ScenarioLightmapBspDataSection.VertexList.Datum { Value = reader.ReadByte() });
                            outWriter.Write(section.VertexLists.Vertex[section.VertexLists.Vertex.Count - 1].Value);

                            section.VertexLists.Vertex.Add(new ScenarioLightmapBspDataSection.VertexList.Datum { Value = reader.ReadByte() });
                            outWriter.Write(section.VertexLists.Vertex[section.VertexLists.Vertex.Count - 1].Value);

                            section.VertexLists.Vertex.Add(new ScenarioLightmapBspDataSection.VertexList.Datum { Value = reader.ReadByte() });
                            outWriter.Write(section.VertexLists.Vertex[section.VertexLists.Vertex.Count - 1].Value);

                            section.VertexLists.Vertex.Add(new ScenarioLightmapBspDataSection.VertexList.Datum { Value = reader.ReadByte() });
                            outWriter.Write(section.VertexLists.Vertex[section.VertexLists.Vertex.Count - 1].Value);
                        }
                    }

                    // Write back to tag
                    outStream.Position = 0;
                    geometry.UnknownSections[i].Unknown = outStream.ToArray();
                }
            }

            //
            // Port Blam resource definition
            //

            var sourceResourceDefinition = SourceCache.ResourceCache.GetRenderGeometryApiResourceDefinition(geometry.Resource);
            RenderGeometryApiResourceDefinitionTest resourceDefinition;

            if (sourceResourceDefinition == null)
            {
                resourceDefinition = new RenderGeometryApiResourceDefinitionTest
                {
                    VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>(CacheAddressType.Definition),
                    IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(CacheAddressType.Definition)
                };
            }
            else
            {
                resourceDefinition = sourceResourceDefinition;
            }

            //
            // Load Blam resource data
            //


            var generateParticles = false;

            if (sourceResourceDefinition == null)
            {
                if (geometry.Meshes.Count == 1 && geometry.Meshes[0].Type == VertexType.ParticleModel)
                {
                    generateParticles = true;
                }
                else
                {
                    geometry.Resource.HaloOnlinePageableResource.Resource.ResourceType = TagResourceTypeGen3.None;
                    return geometry;
                }
            }

            //
            // Convert Blam data to ElDorado data
            //

            for (int i = 0; i < geometry.Meshes.Count(); i++)
            {
                var mesh = geometry.Meshes[i];

                if (mesh.VertexBufferIndices[6] != 0xFFFF && mesh.VertexBufferIndices[7] != 0xFFFF)
                {
                    ushort temp = mesh.VertexBufferIndices[6];
                    mesh.VertexBufferIndices[6] = mesh.VertexBufferIndices[7];
                    mesh.VertexBufferIndices[7] = temp;

                    // Get total amount of indices

                    int indexCount = 0;

                    foreach (var subpart in mesh.SubParts)
                        indexCount += subpart.IndexCount;

                    WaterConversionData waterData = new WaterConversionData()
                    {
                        IndexBufferLength = indexCount,
                    };

                    for (int j = 0; j < mesh.Parts.Count(); j++)
                    {
                        var part = mesh.Parts[j];
                        waterData.PartData.Add(new Tuple<int, int, bool>(part.FirstIndexOld, part.IndexCountOld, part.FlagsNew.HasFlag(Mesh.Part.PartFlagsNew.CanBeRenderedInDrawBundles)));
                    }
                    waterData.Sort();
                    WaterData.Add(waterData);
                }
            }

            if (generateParticles)
            {
                resourceDefinition.VertexBuffers.Add(new D3DStructure<VertexBufferDefinition>
                {
                    Definition = new VertexBufferDefinition
                    {
                        Format = VertexBufferFormat.ParticleModel,
                        Data = new TagData
                        {
                            Data = new byte[32],
                            AddressType = CacheAddressType.Data
                        }
                    }
                });

                geometry.Meshes[0].VertexBufferIndices[0] = (ushort)resourceDefinition.VertexBuffers.IndexOf(resourceDefinition.VertexBuffers.Last());
                geometry.Meshes[0].IndexBufferIndices[0] = CreateIndexBuffer(resourceDefinition, 3);
            }
            else
            {
                for (int i = 0, prevVertCount = -1; i < resourceDefinition.VertexBuffers.Count; i++, prevVertCount = resourceDefinition.VertexBuffers[i - 1].Definition.Count)
                {
                    using(var destStream = new MemoryStream())
                    using(var stream = new MemoryStream(resourceDefinition.VertexBuffers[i].Definition.Data.Data))
                    {
                        ConvertVertexBuffer(resourceDefinition, stream, destStream, i, prevVertCount);
                        resourceDefinition.VertexBuffers[i].Definition.Data.Data = destStream.ToArray();
                    }   
                }

                for (var i = 0; i < resourceDefinition.IndexBuffers.Count; i++)
                {
                    using (var destStream = new MemoryStream())
                    using (var stream = new MemoryStream(resourceDefinition.IndexBuffers[i].Definition.Data.Data))
                    {
                        ConvertIndexBuffer(resourceDefinition, stream, destStream, i);
                        resourceDefinition.IndexBuffers[i].Definition.Data.Data = destStream.ToArray();
                    }
                }

                foreach (var mesh in geometry.Meshes)
                {
                    if (!mesh.Flags.HasFlag(MeshFlags.MeshIsUnindexed))
                        continue;

                    var indexCount = 0;

                    foreach (var part in mesh.Parts)
                        indexCount += part.IndexCountOld;

                    mesh.IndexBufferIndices[0] = CreateIndexBuffer(resourceDefinition, dataStream, indexCount);
                }
            }

            //
            // Swap order of water vertex buffers
            //

            for (var i = 0; i < resourceDefinition.VertexBuffers.Count; i++)
            {
                var vertexBuffer = resourceDefinition.VertexBuffers[i];

                if (vertexBuffer.Definition.Format == VertexBufferFormat.Unknown1B)
                {
                    TagStructureReference<VertexBufferDefinition> temp = vertexBuffer;
                    resourceDefinition.VertexBuffers[i] = resourceDefinition.VertexBuffers[i - 1];
                    resourceDefinition.VertexBuffers[i - 1] = temp;
                }
            }

            //
            // Finalize the new ElDorado geometry resource
            //

            var cache = CacheContext.GetResourceCache(ResourceLocation.Resources);

            if (!resourceStreams.ContainsKey(ResourceLocation.Resources))
            {
                resourceStreams[ResourceLocation.Resources] = portingFlags.HasFlag(PortTagCommand.PortingFlags.Memory) ?
                    new MemoryStream() :
                    (Stream)CacheContext.OpenResourceCacheReadWrite(ResourceLocation.Resources);

                if (portingFlags.HasFlag(PortTagCommand.PortingFlags.Memory))
                    using (var resourceStream = CacheContext.OpenResourceCacheRead(ResourceLocation.Resources))
                        resourceStream.CopyTo(resourceStreams[ResourceLocation.Resources]);
            }

            geometry.Resource.HaloOnlinePageableResource.ChangeLocation(ResourceLocation.Resources);

            geometry.Resource.HaloOnlinePageableResource.Page.Index = cache.Add(resourceStreams[ResourceLocation.Resources], dataStream.ToArray(), out uint compressedSize);
            geometry.Resource.HaloOnlinePageableResource.Page.CompressedBlockSize = compressedSize;
            geometry.Resource.HaloOnlinePageableResource.Page.UncompressedBlockSize = (uint)dataStream.Length;
            geometry.Resource.HaloOnlinePageableResource.DisableChecksum();

            var resourceContext = new ResourceSerializationContext(CacheContext, geometry.Resource.HaloOnlinePageableResource);
            CacheContext.Serializer.Serialize(resourceContext, resourceDefinition);

            return geometry;
        }

        private static void ConvertVertices<T>(int count, Func<T> readVertex, Action<T, int> writeVertex)
        {
            for (var i = 0; i < count; i++)
                writeVertex(readVertex(), i);
        }

        public void ConvertVertexBuffer(RenderGeometryApiResourceDefinitionTest resourceDefinition, Stream inputStream, Stream outputStream, int vertexBufferIndex, int previousVertexBufferCount)
        {
            var vertexBuffer = resourceDefinition.VertexBuffers[vertexBufferIndex].Definition;
            var count = vertexBuffer.Count;

            var startPos = (int)outputStream.Position;
            vertexBuffer.Data.Address = new CacheAddress(CacheAddressType.Data, startPos);

            var inVertexStream = VertexStreamFactory.Create(SourceCache.Version, inputStream);
            var outVertexStream = VertexStreamFactory.Create(HOCache.Version, outputStream);

            OriginalBufferOffsets.Add(inputStream.Position);
            switch (vertexBuffer.Format)
            {
                case VertexBufferFormat.World:
                    ConvertVertices(count, inVertexStream.ReadWorldVertex, (v, i) =>
                    {
                        v.Normal = VertexBufferUtils.ConvertVectorSpace(v.Normal);
                        v.Tangent = VertexBufferUtils.ConvertVectorSpace(v.Tangent);
                        v.Binormal = VertexBufferUtils.ConvertVectorSpace(v.Binormal);
                        outVertexStream.WriteWorldVertex(v);
                    });
                    break;

                case VertexBufferFormat.Rigid:
                    ConvertVertices(count, inVertexStream.ReadRigidVertex, (v, i) =>
                    {
                        v.Normal = VertexBufferUtils.ConvertVectorSpace(v.Normal);
                        v.Tangent = VertexBufferUtils.ConvertVectorSpace(v.Tangent);
                        v.Binormal = VertexBufferUtils.ConvertVectorSpace(v.Binormal);
                        outVertexStream.WriteRigidVertex(v);
                    });
                    break;

                case VertexBufferFormat.Skinned:
                    ConvertVertices(count, inVertexStream.ReadSkinnedVertex, (v, i) =>
                    {
                        v.Normal = VertexBufferUtils.ConvertVectorSpace(v.Normal);
                        v.Tangent = VertexBufferUtils.ConvertVectorSpace(v.Tangent);
                        v.Binormal = VertexBufferUtils.ConvertVectorSpace(v.Binormal);
                        outVertexStream.WriteSkinnedVertex(v);
                    });
                    break;

                case VertexBufferFormat.StaticPerPixel:
                    ConvertVertices(count, inVertexStream.ReadStaticPerPixelData, (v, i) => outVertexStream.WriteStaticPerPixelData(v));
                    break;

                case VertexBufferFormat.StaticPerVertex:
                    ConvertVertices(count, inVertexStream.ReadStaticPerVertexData, (v, i) =>
                    {
                        v.Color1 = VertexBufferUtils.ConvertColorSpace(v.Color1);
                        v.Color2 = VertexBufferUtils.ConvertColorSpace(v.Color2);
                        v.Color3 = VertexBufferUtils.ConvertColorSpace(v.Color3);
                        v.Color4 = VertexBufferUtils.ConvertColorSpace(v.Color4);
                        v.Color5 = VertexBufferUtils.ConvertColorSpace(v.Color5);
                        outVertexStream.WriteStaticPerVertexData(v);
                    });
                    break;

                case VertexBufferFormat.AmbientPrt:
                    ConvertVertices(vertexBuffer.Count = previousVertexBufferCount, inVertexStream.ReadAmbientPrtData, (v, i) => outVertexStream.WriteAmbientPrtData(v));
                    break;

                case VertexBufferFormat.LinearPrt:
                    ConvertVertices(count, inVertexStream.ReadLinearPrtData, (v, i) =>
                    {
                        v.BlendWeight = VertexBufferUtils.ConvertNormal(v.BlendWeight);
                        outVertexStream.WriteLinearPrtData(v);
                    });
                    break;

                case VertexBufferFormat.QuadraticPrt:
                    ConvertVertices(count, inVertexStream.ReadQuadraticPrtData, (v, i) => outVertexStream.WriteQuadraticPrtData(v));
                    break;

                case VertexBufferFormat.StaticPerVertexColor:
                    ConvertVertices(count, inVertexStream.ReadStaticPerVertexColorData, (v, i) => outVertexStream.WriteStaticPerVertexColorData(v));
                    break;

                case VertexBufferFormat.Decorator:
                    ConvertVertices(count, inVertexStream.ReadDecoratorVertex, (v, i) =>
                    {
                        v.Normal = VertexBufferUtils.ConvertVectorSpace(v.Normal);
                        outVertexStream.WriteDecoratorVertex(v);
                    });
                    break;

                case VertexBufferFormat.World2:
                    vertexBuffer.Format = VertexBufferFormat.World;
                    goto case VertexBufferFormat.World;

                case VertexBufferFormat.Unknown1A:

                    var waterData = WaterData[CurrentWaterBuffer];

                    // Reformat Vertex Buffer
                    vertexBuffer.Format = VertexBufferFormat.World;
                    vertexBuffer.VertexSize = 0x34;
                    vertexBuffer.Count = waterData.IndexBufferLength;

                    // Create list of indices for later use.
                    Unknown1BIndices = new List<ushort>();

                    for (int k = 0; k < waterData.PartData.Count(); k++)
                    {
                        Tuple<int, int, bool> currentPartData = waterData.PartData[k];

                        // Not water, add garbage data
                        if (currentPartData.Item3 == false)
                        {
                            for (int j = 0; j < currentPartData.Item2; j++)
                                WriteUnusedWorldWaterData(outputStream);
                        }
                        else
                        {
                            ConvertVertices(currentPartData.Item2 / 3, inVertexStream.ReadUnknown1A, (v, i) =>
                            {
                                // Store current stream position
                                var tempStreamPosition = inputStream.Position;

                                // Open previous world buffer (H3)
                                var worldVertexBufferBasePosition = OriginalBufferOffsets[OriginalBufferOffsets.Count() - 3];
                                inputStream.Position = worldVertexBufferBasePosition;

                                for (int j = 0; j < 3; j++)
                                {
                                    inputStream.Position = 0x20 * v.Vertices[j] + worldVertexBufferBasePosition;

                                    WorldVertex w = inVertexStream.ReadWorldVertex();

                                    Unknown1BIndices.Add(v.Indices[j]);

                                    // The last 2 floats in WorldWater are unknown.

                                    outVertexStream.WriteWorldWaterVertex(w);
                                }

                                // Restore position for reading the next vertex correctly
                                inputStream.Position = tempStreamPosition;
                            });
                        }
                    }



                    break;

                case VertexBufferFormat.Unknown1B:

                    var waterDataB = WaterData[CurrentWaterBuffer];

                    // Adjust vertex size to match HO. Set count of vertices

                    vertexBuffer.VertexSize = 0x18;

                    var originalCount = vertexBuffer.Count;
                    vertexBuffer.Count = waterDataB.IndexBufferLength;

                    var basePosition = inputStream.Position;
                    var unknown1BPosition = 0;

                    for (int k = 0; k < waterDataB.PartData.Count(); k++)
                    {
                        Tuple<int, int, bool> currentPartData = waterDataB.PartData[k];

                        // Not water, add garbage data
                        if (currentPartData.Item3 == false)
                        {
                            for (int j = 0; j < currentPartData.Item2; j++)
                                WriteUnusedUnknown1BData(outputStream);
                        }
                        else
                        {
                            for (int j = unknown1BPosition; j < Unknown1BIndices.Count() && j - unknown1BPosition < currentPartData.Item2; j++)
                            {
                                inputStream.Position = basePosition + 0x24 * Unknown1BIndices[j];
                                ConvertVertices(1, inVertexStream.ReadUnknown1B, (v, i) => outVertexStream.WriteUnknown1B(v));
                                unknown1BPosition++;
                            }
                        }
                    }

                    // Get to the end of Unknown1B in H3 data
                    inputStream.Position = basePosition + originalCount * 0x24;

                    CurrentWaterBuffer++;

                    break;

                case VertexBufferFormat.ParticleModel:
                    ConvertVertices(count, inVertexStream.ReadParticleModelVertex, (v, i) =>
                    {
                        v.Normal = VertexBufferUtils.ConvertVectorSpace(v.Normal);
                        outVertexStream.WriteParticleModelVertex(v);
                    });
                    break;

                case VertexBufferFormat.TinyPosition:
                    ConvertVertices(count, inVertexStream.ReadTinyPositionVertex, (v, i) =>
                    {
                        v.Position = VertexBufferUtils.ConvertPositionShort(v.Position);
                        v.Variant = (ushort)((v.Variant >> 8) & 0xFF);
                        v.Normal = VertexBufferUtils.ConvertNormal(v.Normal);
                        outVertexStream.WriteTinyPositionVertex(v);
                    });
                    break;

                default:
                    throw new NotSupportedException(vertexBuffer.Format.ToString());
            }

            vertexBuffer.Data.Size = (int)outputStream.Position - startPos;
            vertexBuffer.VertexSize = (short)(vertexBuffer.Data.Size / vertexBuffer.Count);
        }

        public void WriteUnusedWorldWaterData(Stream outputStream)
        {
            byte[] data = new byte[4] { 0xCD, 0xCD, 0xCD, 0xCD };
            for (int i = 0; i < 13; i++)
            {
                outputStream.Write(data, 0, 4);
            }
        }

        public void WriteUnusedUnknown1BData(Stream outputStream)
        {
            byte[] data = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
            for (int i = 0; i < 6; i++)
            {
                outputStream.Write(data, 0, 4);
            }
        }

        public void ConvertIndexBuffer(RenderGeometryApiResourceDefinitionTest resourceDefinition, Stream inputStream, Stream outputStream, int indexBufferIndex)
        {
            var indexBuffer = resourceDefinition.IndexBuffers[indexBufferIndex].Definition;

            var indexCount = indexBuffer.Data.Data.Length / 2;

            var inIndexStream = new IndexBufferStream(
                inputStream,
                EndianFormat.BigEndian);

            var outIndexStream = new IndexBufferStream(
                outputStream,
                EndianFormat.LittleEndian);

            StreamUtil.Align(outputStream, 4);
            indexBuffer.Data.Address = new CacheAddress(CacheAddressType.Data, (int)outputStream.Position);

            for (var j = 0; j < indexCount; j++)
                outIndexStream.WriteIndex(inIndexStream.ReadIndex());
        }

        public ushort CreateIndexBuffer(RenderGeometryApiResourceDefinitionTest resourceDefinition, int count)
        {
            resourceDefinition.IndexBuffers.Add(new D3DStructure<IndexBufferDefinition>
            {
                Definition = new IndexBufferDefinition
                {
                    Format = IndexBufferFormat.TriangleStrip,
                    Data = new TagData
                    {
                        AddressType = CacheAddressType.Data
                    }
                }
            });

            var indexBuffer = resourceDefinition.IndexBuffers.Last().Definition;

            using(var stream = new MemoryStream())
            using(var writer = new EndianWriter(stream, EndianFormat.LittleEndian))
            {
                for (var j = 0; j < count; j++)
                    writer.Write((short)j);
                indexBuffer.Data.Data = stream.ToArray();
            }
            
            return (ushort)resourceDefinition.IndexBuffers.IndexOf(resourceDefinition.IndexBuffers.Last());
        }

        public ushort CreateIndexBuffer(RenderGeometryApiResourceDefinition resourceDefinition, Stream outputStream, List<ushort> buffer)
        {
            resourceDefinition.IndexBuffers.Add(new TagStructureReference<IndexBufferDefinition>
            {
                Definition = new IndexBufferDefinition
                {
                    Format = IndexBufferFormat.TriangleStrip,
                    Data = new TagData
                    {
                        Size = buffer.Count() * 2,
                        Unused4 = 0,
                        Unused8 = 0,
                        Address = new CacheAddress(),
                        Unused10 = 0
                    }
                }
            });

            var indexBuffer = resourceDefinition.IndexBuffers.Last().Definition;

            var indexCount = indexBuffer.Data.Size / 2;

            var outIndexStream = new EndianWriter(
                outputStream,
                EndianFormat.LittleEndian);

            StreamUtil.Align(outputStream, 4);
            indexBuffer.Data.Address = new CacheAddress(CacheAddressType.Data, (int)outputStream.Position);

            for (var j = 0; j < indexCount; j++)
                outIndexStream.Write((short)buffer[j]);

            return (ushort)resourceDefinition.IndexBuffers.IndexOf(resourceDefinition.IndexBuffers.Last());
        }

        private class WaterConversionData
        {
            // offset, count, isWater
            public List<Tuple<int, int, bool>> PartData;
            public int IndexBufferLength;

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