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
        private GameCache HOCache { get; }
        private GameCache SourceCache;

        public RenderGeometryConverter(GameCache hoCache, GameCache sourceCache)
        {
            HOCache = hoCache;
            SourceCache = sourceCache;
        }

        /// <summary>
        /// Converts RenderGeometry class in place and returns a new RenderGeometryApiResourceDefinition
        /// </summary>
        public RenderGeometryApiResourceDefinitionTest Convert(RenderGeometry geometry, RenderGeometryApiResourceDefinitionTest resourceDefinition)
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
                    using (var outputWriter = new EndianWriter(new MemoryStream(result), HOCache.Endianness))
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
                using (var outWriter = new EndianWriter(outStream, HOCache.Endianness))
                using (var stream = new MemoryStream(dataref))
                using (var reader = new EndianReader(stream, SourceCache.Endianness))
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
            // Port resource definition
            //

            var wasNull = false;
            if (resourceDefinition == null)
            {
                wasNull = true;
                Console.Error.WriteLine("Render geometry does not have a valid resource definition, continuing anyway.");
                resourceDefinition = new RenderGeometryApiResourceDefinitionTest
                {
                    VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>(CacheAddressType.Definition),
                    IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(CacheAddressType.Definition)
                };
            }

            geometry.SetResourceBuffers(resourceDefinition);

            // do conversion (PARTICLE INDEX BUFFERS, WATER CONVERSION TO DO) AMBIENT PRT TOO

            var generateParticles = false; // temp fix when pmdf geo is null

            if (wasNull)
            {
                if (geometry.Meshes.Count == 1 && geometry.Meshes[0].Type == VertexType.ParticleModel)
                {
                    generateParticles = true;
                }
                else
                {
                    geometry.Resource = HOCache.ResourceCache.CreateRenderGeometryApiResource(resourceDefinition);
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
                    VertexSize = (short)VertexStreamFactory.Create(HOCache.Version, null).GetVertexSize(VertexBufferFormat.ParticleModel),
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
                        {
                            var count = mesh.ResourceVertexBuffers[0].Count;
                        }
                        // skip conversion of water vertices, done right after the loop
                        else if (vertexBuffer.Format == VertexBufferFormat.Unknown1A || vertexBuffer.Format == VertexBufferFormat.Unknown1B)
                            continue;

                        VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, HOCache.Version, vertexBuffer);
                    }

                    // convert water vertex buffers

                    // index 6 and 7 are water related
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
                            if(part.FlagsNew.HasFlag(Mesh.Part.PartFlagsNew.IsWaterPart))
                                waterData.PartData.Add(new Tuple<int, int>(part.FirstIndexOld, part.IndexCountOld));
                        }

                        if(waterData.PartData.Count > 1)
                            waterData.Sort();

                        // read all world vertices in current mesh into a list
                        List<WorldVertex> worldVertices = new List<WorldVertex>();

                        using(var stream = new MemoryStream(mesh.ResourceVertexBuffers[0].Data.Data))
                        {
                            var vertexStream = VertexStreamFactory.Create(HOCache.Version, stream);
                            for(int v = 0; v < mesh.ResourceVertexBuffers[0].Count; v++)
                                worldVertices.Add(vertexStream.ReadWorldVertex());
                        }

                        // Create list of indices for later use.
                        var unknown1BIndices = new List<ushort>();

                        // create vertex buffer for Unknown1A -> World
                        VertexBufferDefinition waterVertices = new VertexBufferDefinition
                        {
                            Count = indexCount,
                            Format = VertexBufferFormat.World,
                            VertexSize = 0x38   // this size is actually wrong but I replicate the errors in HO data, size should be 0x34
                        };

                        // create vertex buffer for Unknown1B
                        VertexBufferDefinition waterParameters = new VertexBufferDefinition
                        {
                            Count = indexCount,
                            Format = VertexBufferFormat.Unknown1B,
                            VertexSize = 0x24   // wrong size, this is 0x18 on file, padded with zeroes.
                        };

                        using (var outputStream = new MemoryStream())
                        using(var inputStream = new MemoryStream(mesh.ResourceVertexBuffers[6].Data.Data))
                        {
                            var inVertexStream = VertexStreamFactory.Create(SourceCache.Version, inputStream);
                            var outVertexStream = VertexStreamFactory.Create(HOCache.Version, outputStream);
                            
                            // fill vertex buffer to the right size HO expects, then write the vertex data at the actual proper position
                            VertexBufferConverter.DebugFill(outputStream, waterVertices.VertexSize * waterVertices.Count);

                            //sorted list of water parts, write the modified world vertex there
                            for (int k = 0; k < waterData.PartData.Count(); k++)
                            {
                                Tuple<int, int> currentPartData = waterData.PartData[k];

                                outputStream.Position = 0x34 * currentPartData.Item1;
                                VertexBufferConverter.ConvertVertices(currentPartData.Item2 / 3, inVertexStream.ReadUnknown1A, (v, i) =>
                                {
                                    for (int j = 0; j < 3; j++)
                                    {
                                        WorldVertex w = worldVertices[v.Vertices[j]];
                                        unknown1BIndices.Add(v.Indices[j]);
                                        outVertexStream.WriteWorldWaterVertex(w);
                                    }
                                });
                            }
                            waterVertices.Data = new TagData(outputStream.ToArray());
                        }

                        using (var outputStream = new MemoryStream())
                        using (var inputStream = new MemoryStream(mesh.ResourceVertexBuffers[7].Data.Data))
                        {
                            var inVertexStream = VertexStreamFactory.Create(SourceCache.Version, inputStream);
                            var outVertexStream = VertexStreamFactory.Create(HOCache.Version, outputStream);

                            // fill vertex buffer to the right size HO expects, then write the vertex data at the actual proper position
                            VertexBufferConverter.Fill(outputStream, waterParameters.VertexSize * waterParameters.Count);

                            //sorted list of water parts, write the modified world vertex there
                            var currentUnknown1BIndex = 0;
                            for (int k = 0; k < waterData.PartData.Count(); k++)
                            {
                                Tuple<int, int> currentPartData = waterData.PartData[k];
                                outputStream.Position = 0x18 * currentPartData.Item1;
                                for(int v = 0; v < currentPartData.Item2; v++)
                                {
                                    var unknown1BIndex = unknown1BIndices[currentUnknown1BIndex + v];
                                    inputStream.Position = unknown1BIndex * 0x24;
                                    var unknown1B = inVertexStream.ReadUnknown1B();
                                    // convert unknown1B
                                    outVertexStream.WriteUnknown1B(unknown1B);
                                }
                                currentUnknown1BIndex += currentPartData.Item2;
                            }
                            waterParameters.Data = new TagData(outputStream.ToArray());
                        }


                        // convert unknown1B and move to index 6

                        mesh.ResourceVertexBuffers[6] = waterParameters;
                        mesh.ResourceVertexBuffers[7] = waterVertices;

                    }



                    foreach (var indexBuffer in mesh.ResourceIndexBuffers)
                    {
                        if (indexBuffer == null)
                            continue;

                        IndexBufferConverter.ConvertIndexBuffer(SourceCache.Version, HOCache.Version, indexBuffer);
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
                    VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, HOCache.Version, perPixel.VertexBuffer);
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