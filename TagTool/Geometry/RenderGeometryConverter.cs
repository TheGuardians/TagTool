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
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache;
        private List<long> OriginalBufferOffsets;
        private List<ushort> Unknown1BIndices;
        private List<WaterConversionData> WaterData;
        private int CurrentWaterBuffer;

        public RenderGeometryConverter(HaloOnlineCacheContext cacheContext, CacheFile blamCache)
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            OriginalBufferOffsets = new List<long>();
            Unknown1BIndices = new List<ushort>();
            WaterData = new List<WaterConversionData>();
            CurrentWaterBuffer = 0;
        }

        public RenderGeometry Convert(Stream cacheStream, RenderGeometry geometry, Dictionary<ResourceLocation, Stream> resourceStreams, PortTagCommand.PortingFlags portingFlags)
        {
            if (BlamCache.ResourceGestalt == null || BlamCache.ResourceLayoutTable == null)
                BlamCache.LoadResourceTags();

            //
            // Set up ElDorado resource reference
            //

            geometry.Resource = new PageableResource<RenderGeometryApiResourceDefinition>(TagResourceTypeGen3.RenderGeometry, CacheContext.Version);

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
                    var header = CacheContext.Deserializer.Deserialize<ScenarioLightmapBspDataSection.Header>(dataContext);

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

                    CacheContext.Serializer.Serialize(dataContext, header);

                    while (reader.BaseStream.Position < dataref.Length) // read the rest of dataref
                    {
                        if (section.Headers.Count == 2) // remove "wrongfully" added ones
                            section.Headers.RemoveAt(1);

                        section.Headers.Add(CacheContext.Deserializer.Deserialize<ScenarioLightmapBspDataSection.Header>(dataContext));

                        // if some values match header1, continue
                        if (section.Headers[0].Position == section.Headers[1].Position)
                        {
                            header = section.Headers[1];
                            CacheContext.Serializer.Serialize(dataContext, header);

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

            var resourceEntry = BlamCache.ResourceGestalt.TagResources[geometry.ZoneAssetHandle.Index];

            geometry.Resource.Resource.DefinitionAddress = resourceEntry.DefinitionAddress;
            geometry.Resource.Resource.DefinitionData = BlamCache.ResourceGestalt.FixupInformation.Skip(resourceEntry.FixupInformationOffset).Take(resourceEntry.FixupInformationLength).ToArray();

            RenderGeometryApiResourceDefinition resourceDefinition = null;

            if (geometry.Resource.Resource.DefinitionData.Length < 0x30)
            {
                resourceDefinition = new RenderGeometryApiResourceDefinition
                {
                    VertexBuffers = new List<TagStructureReference<VertexBufferDefinition>>(),
                    IndexBuffers = new List<TagStructureReference<IndexBufferDefinition>>()
                };
            }
            else
            {
                using (var definitionStream = new MemoryStream(geometry.Resource.Resource.DefinitionData, true))
                using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
                using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.BigEndian))
                {
                    foreach (var fixup in resourceEntry.ResourceFixups)
                    {
                        definitionStream.Position = fixup.BlockOffset;
                        definitionWriter.Write(fixup.Address.Value);

                        geometry.Resource.Resource.ResourceFixups.Add(fixup);
                    }

                    foreach (var definitionFixup in resourceEntry.ResourceDefinitionFixups)
                    {
                        var newDefinitionFixup = new TagResourceGen3.ResourceDefinitionFixup
                        {
                            Address = definitionFixup.Address,
                            ResourceStructureTypeIndex = definitionFixup.ResourceStructureTypeIndex
                        };

                        geometry.Resource.Resource.ResourceDefinitionFixups.Add(newDefinitionFixup);
                    }

                    var dataContext = new DataSerializationContext(definitionReader, definitionWriter, CacheAddressType.Definition);

                    definitionStream.Position = geometry.Resource.Resource.DefinitionAddress.Offset;
                    resourceDefinition = BlamCache.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(dataContext);
                }
            }

            //
            // Load Blam resource data
            //

            var resourceData = BlamCache.GetRawFromID(geometry.ZoneAssetHandle);
            var generateParticles = false;

            if (resourceData == null)
            {
                if (geometry.Meshes.Count == 1 && geometry.Meshes[0].Type == VertexType.ParticleModel)
                {
                    generateParticles = true;
                    resourceData = new byte[0];
                }
                else
                {
                    geometry.Resource.Resource.ResourceType = TagResourceTypeGen3.None;
                    return geometry;
                }
            }

            //
            // Convert Blam data to ElDorado data
            //

            using (var dataStream = new MemoryStream())
            using (var blamResourceStream = new MemoryStream(resourceData))
            {
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
                    var outVertexStream = VertexStreamFactory.Create(CacheContext.Version, dataStream);

                    StreamUtil.Align(dataStream, 4);

                    resourceDefinition.VertexBuffers.Add(new TagStructureReference<VertexBufferDefinition>
                    {
                        Definition = new VertexBufferDefinition
                        {
                            Format = VertexBufferFormat.ParticleModel,
                            Data = new TagData
                            {
                                Size = 32,
                                Unused4 = 0,
                                Unused8 = 0,
                                Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position),
                                Unused10 = 0
                            }
                        }
                    });

                    var vertexBuffer = resourceDefinition.VertexBuffers.Last().Definition;

                    for (var j = 0; j < 3; j++)
                        outVertexStream.WriteParticleModelVertex(new ParticleModelVertex
                        {
                            Position = new RealVector3d(),
                            Texcoord = new RealVector2d(),
                            Normal = new RealVector3d()
                        });

                    geometry.Meshes[0].VertexBufferIndices[0] = (ushort)resourceDefinition.VertexBuffers.IndexOf(resourceDefinition.VertexBuffers.Last());
                    geometry.Meshes[0].IndexBufferIndices[0] = CreateIndexBuffer(resourceDefinition, dataStream, 3);
                }
                else
                {
                    for (int i = 0, prevVertCount = -1; i < resourceDefinition.VertexBuffers.Count; i++, prevVertCount = resourceDefinition.VertexBuffers[i - 1].Definition.Count)
                    {
                        blamResourceStream.Position = resourceDefinition.VertexBuffers[i].Definition.Data.Address.Offset; // resourceEntry.ResourceFixups[i].Offset;
                        ConvertVertexBuffer(resourceDefinition, blamResourceStream, dataStream, i, prevVertCount);
                    }

                    for (var i = 0; i < resourceDefinition.IndexBuffers.Count; i++)
                    {
                        blamResourceStream.Position = resourceDefinition.IndexBuffers[i].Definition.Data.Address.Offset; // resourceEntry.ResourceFixups[resourceDefinition.VertexBuffers.Count * 2 + i].Offset;
                        ConvertIndexBuffer(resourceDefinition, blamResourceStream, dataStream, i);
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

                geometry.Resource.ChangeLocation(ResourceLocation.Resources);

                geometry.Resource.Page.Index = cache.Add(resourceStreams[ResourceLocation.Resources], dataStream.ToArray(), out uint compressedSize);
                geometry.Resource.Page.CompressedBlockSize = compressedSize;
                geometry.Resource.Page.UncompressedBlockSize = (uint)dataStream.Length;
                geometry.Resource.DisableChecksum();

                var resourceContext = new ResourceSerializationContext(CacheContext, geometry.Resource);
                CacheContext.Serializer.Serialize(resourceContext, resourceDefinition);
            }

            return geometry;
        }

        private static void ConvertVertices<T>(int count, Func<T> readVertex, Action<T, int> writeVertex)
        {
            for (var i = 0; i < count; i++)
                writeVertex(readVertex(), i);
        }

        public void ConvertVertexBuffer(RenderGeometryApiResourceDefinition resourceDefinition, Stream inputStream, Stream outputStream, int vertexBufferIndex, int previousVertexBufferCount)
        {
            var vertexBuffer = resourceDefinition.VertexBuffers[vertexBufferIndex].Definition;
            var count = vertexBuffer.Count;

            var startPos = (int)outputStream.Position;
            vertexBuffer.Data.Address = new CacheAddress(CacheAddressType.Resource, startPos);

            var inVertexStream = VertexStreamFactory.Create(BlamCache.Version, inputStream);
            var outVertexStream = VertexStreamFactory.Create(CacheContext.Version, outputStream);

            OriginalBufferOffsets.Add(inputStream.Position);

            switch (vertexBuffer.Format)
            {
                case VertexBufferFormat.World:
                    ConvertVertices(count, inVertexStream.ReadWorldVertex, (v, i) =>
                    {
                        v.Normal = ConvertVectorSpace(v.Normal);
                        v.Tangent = ConvertVectorSpace(v.Tangent);
                        v.Binormal = ConvertVectorSpace(v.Binormal);
                        outVertexStream.WriteWorldVertex(v);
                    });
                    break;

                case VertexBufferFormat.Rigid:
                    ConvertVertices(count, inVertexStream.ReadRigidVertex, (v, i) =>
                    {
                        v.Normal = ConvertVectorSpace(v.Normal);
                        v.Tangent = ConvertVectorSpace(v.Tangent);
                        v.Binormal = ConvertVectorSpace(v.Binormal);
                        outVertexStream.WriteRigidVertex(v);
                    });
                    break;

                case VertexBufferFormat.Skinned:
                    ConvertVertices(count, inVertexStream.ReadSkinnedVertex, (v, i) =>
                    {
                        v.Normal = ConvertVectorSpace(v.Normal);
                        v.Tangent = ConvertVectorSpace(v.Tangent);
                        v.Binormal = ConvertVectorSpace(v.Binormal);
                        outVertexStream.WriteSkinnedVertex(v);
                    });
                    break;

                case VertexBufferFormat.StaticPerPixel:
                    ConvertVertices(count, inVertexStream.ReadStaticPerPixelData, (v, i) => outVertexStream.WriteStaticPerPixelData(v));
                    break;

                case VertexBufferFormat.StaticPerVertex:
                    ConvertVertices(count, inVertexStream.ReadStaticPerVertexData, (v, i) =>
                    {
                        v.Texcoord1 = ConvertNormal(v.Texcoord1);
                        v.Texcoord2 = ConvertNormal(v.Texcoord2);
                        v.Texcoord3 = ConvertNormal(v.Texcoord3);
                        v.Texcoord4 = ConvertNormal(v.Texcoord4);
                        v.Texcoord5 = ConvertNormal(v.Texcoord5);
                        outVertexStream.WriteStaticPerVertexData(v);
                    });
                    break;

                case VertexBufferFormat.AmbientPrt:
                    ConvertVertices(vertexBuffer.Count = previousVertexBufferCount, inVertexStream.ReadAmbientPrtData, (v, i) => outVertexStream.WriteAmbientPrtData(v));
                    break;

                case VertexBufferFormat.LinearPrt:
                    ConvertVertices(count, inVertexStream.ReadLinearPrtData, (v, i) =>
                    {
                        v.BlendWeight = ConvertNormal(v.BlendWeight);
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
                        v.Normal = ConvertVectorSpace(v.Normal);
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
                        v.Normal = ConvertVectorSpace(v.Normal);
                        outVertexStream.WriteParticleModelVertex(v);
                    });
                    break;

                case VertexBufferFormat.TinyPosition:
                    ConvertVertices(count, inVertexStream.ReadTinyPositionVertex, (v, i) =>
                    {
                        v.Position = ConvertPositionShort(v.Position);
                        v.Variant = (ushort)((v.Variant >> 8) & 0xFF);
                        v.Normal = ConvertNormal(v.Normal);
                        outVertexStream.WriteTinyPositionVertex(v);
                    });
                    break;

                default:
                    throw new NotSupportedException(vertexBuffer.Format.ToString());
            }

            vertexBuffer.Data.Size = (int)outputStream.Position - startPos;
            vertexBuffer.VertexSize = (short)(vertexBuffer.Data.Size / vertexBuffer.Count);

            resourceDefinition.VertexBuffers[vertexBufferIndex].DefinitionAddress = 0;
            resourceDefinition.VertexBuffers[vertexBufferIndex].RuntimeAddress = 0;
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

        public void ConvertIndexBuffer(RenderGeometryApiResourceDefinition resourceDefinition, Stream inputStream, Stream outputStream, int indexBufferIndex)
        {
            var indexBuffer = resourceDefinition.IndexBuffers[indexBufferIndex].Definition;

            var indexCount = indexBuffer.Data.Size / 2;

            var inIndexStream = new IndexBufferStream(
                inputStream,
                EndianFormat.BigEndian);

            var outIndexStream = new IndexBufferStream(
                outputStream,
                EndianFormat.LittleEndian);

            StreamUtil.Align(outputStream, 4);
            indexBuffer.Data.Address = new CacheAddress(CacheAddressType.Resource, (int)outputStream.Position);

            for (var j = 0; j < indexCount; j++)
                outIndexStream.WriteIndex(inIndexStream.ReadIndex());

            resourceDefinition.IndexBuffers[indexBufferIndex].DefinitionAddress = 0;
            resourceDefinition.IndexBuffers[indexBufferIndex].RuntimeAddress = 0;
        }

        public ushort CreateIndexBuffer(RenderGeometryApiResourceDefinition resourceDefinition, Stream outputStream, int count)
        {
            resourceDefinition.IndexBuffers.Add(new TagStructureReference<IndexBufferDefinition>
            {
                Definition = new IndexBufferDefinition
                {
                    Format = IndexBufferFormat.TriangleStrip,
                    Data = new TagData
                    {
                        Size = count * 2,
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
            indexBuffer.Data.Address = new CacheAddress(CacheAddressType.Resource, (int)outputStream.Position);

            for (var j = 0; j < indexCount; j++)
                outIndexStream.Write((short)j);

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
            indexBuffer.Data.Address = new CacheAddress(CacheAddressType.Resource, (int)outputStream.Position);

            for (var j = 0; j < indexCount; j++)
                outIndexStream.Write((short)buffer[j]);

            return (ushort)resourceDefinition.IndexBuffers.IndexOf(resourceDefinition.IndexBuffers.Last());
        }

        /// <summary> 
        /// Change basis [0,1] to [-1,1] uniformly
        /// </summary>
        private static float ConvertFromNormalBasis(float value)
        {
            value = VertexElementStream.Clamp(value, 0.0f, 1.0f);
            return 2.0f * (value - 0.5f);
        }

        /// <summary> 
        /// Change basis [-1,1] to [0,1] uniformly
        /// </summary>
        private static float ConvertToNormalBasis(float value)
        {
            value = VertexElementStream.Clamp(value);
            return (value / 2.0f) + 0.5f;
        }

        /// <summary>
        /// Modify value to account for some rounding error
        /// </summary>
        private static float FixRoundingShort(float value)
        {
            value = VertexElementStream.Clamp(value);
            if (value > 0 && value < 1)
            {
                value += 1.0f / 32767.0f;
                value = VertexElementStream.Clamp(value);
            }
            return value;
        }

        private static float ConvertVectorSpace(float value)
        {
            return value <= 0.5? 2.0f * value : (value - 1.0f) * 2.0f;
        }

        private static RealVector3d ConvertVectorSpace(RealVector3d vector)
        {
            return new RealVector3d(ConvertVectorSpace(vector.I), ConvertVectorSpace(vector.J), ConvertVectorSpace(vector.K));
        }

        private static RealQuaternion ConvertVectorSpace(RealQuaternion vector)
        {
            return new RealQuaternion(ConvertVectorSpace(vector.I), ConvertVectorSpace(vector.J), ConvertVectorSpace(vector.K), vector.W);
        }

        /// <summary>
        /// Convert H3 normal to HO normal for tinyposition vertex
        /// </summary>
        public RealQuaternion ConvertNormal(RealQuaternion normal)
        {
            return new RealQuaternion(normal.ToArray().Select(e => ConvertToNormalBasis(e)).Reverse());
        }

        /// <summary>
        /// Convert H3 position to HO position including rounding error for tinyposition vertex
        /// </summary>
        public RealVector3d ConvertPositionShort(RealVector3d position)
        {
            return new RealVector3d(position.ToArray().Select(e => FixRoundingShort(ConvertFromNormalBasis(e))).ToArray());
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