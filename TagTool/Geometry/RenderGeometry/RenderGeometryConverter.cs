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

            if (sourceResourceDefinition == null)
            {
                Console.Error.WriteLine("Render geometry does not have a valid resource definition, continuing anyway.");
                sourceResourceDefinition = new RenderGeometryApiResourceDefinitionTest
                {
                    VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>(CacheAddressType.Definition),
                    IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(CacheAddressType.Definition)
                };
            }

            geometry.SetResourceBuffers(sourceResourceDefinition);

            // do conversion (PARTICLE INDEX BUFFERS, WATER CONVERSION TO DO)

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

                if (mesh.VertexBufferIndices[6] != -1 && mesh.VertexBufferIndices[7] != -1)
                {
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
                var mesh = geometry.Meshes[0];

                var newVertexBuffer = new VertexBufferDefinition
                {
                    Format = VertexBufferFormat.ParticleModel,
                    VertexSize = (short)VertexStreamFactory.Create(HOCache.Version, null).GetVertexSize(VertexBufferFormat.ParticleModel),
                    Data = new TagData
                    {
                        Data = new byte[32],
                        AddressType = CacheAddressType.Data
                    }
                };

                mesh.ResourceVertexBuffers[0] = newVertexBuffer;
                mesh.ResourceIndexBuffers[0] = IndexBufferConverter.CreateIndexBuffer(3);
            }
            else
            {
                foreach (var mesh in geometry.Meshes)
                {
                    if(CacheVersionDetection.IsInGen(CacheGeneration.Third, SourceCache.Version))
                    {

                    }



                    foreach (var vertexBuffer in mesh.ResourceVertexBuffers)
                    {
                        if (vertexBuffer == null)
                            continue;

                        VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, HOCache.Version, vertexBuffer);
                    }

                    foreach (var indexBuffer in mesh.ResourceIndexBuffers)
                    {
                        if (indexBuffer == null)
                            continue;

                        IndexBufferConverter.ConvertIndexBuffer(SourceCache.Version, HOCache.Version, indexBuffer);
                    }

                    if (mesh.Flags.HasFlag(MeshFlags.MeshIsUnindexed))
                    {
                        var indexCount = 0;

                        foreach (var part in mesh.Parts)
                            indexCount += part.IndexCountOld;

                        mesh.ResourceIndexBuffers[0] = IndexBufferConverter.CreateIndexBuffer(indexCount);
                    }
                }
            }

            //
            // Finalize the new ElDorado geometry resource
            //

            var newResourceDefinition = geometry.GetResourceDefinition();
            var newReference = HOCache.ResourceCache.CreateRenderGeometryApiResource(newResourceDefinition);
            geometry.Resource = newReference;
            return geometry;
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