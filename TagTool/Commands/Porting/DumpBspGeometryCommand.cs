using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;

namespace TagTool.Commands.Porting
{
    public class DumpBspGeometryCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }

        public DumpBspGeometryCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(CommandFlags.Inherit,

                "DumpBspGeometry",
                "Dumps bsp geometry in ascii format.",

                "DumpBspGeometry <Tag Name> <Output File>",

                "Dumps bsp geometry in ascii format.")
        {
            CacheContext = cacheContext;
            this.BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            //
            // Verify the Blam scenario_structure_bsp tag
            //

            var blamTagName = args[0];

            CacheFile.IndexItem blamTag = null;

            Console.WriteLine("Verifying Blam scenario_structure_bsp tag...");

            foreach (var tag in BlamCache.IndexItems)
            {
                if (tag.ClassCode == "sbsp" && tag.Filename == blamTagName)
                {
                    blamTag = tag;
                    break;
                }
            }

            if (blamTag == null)
            {
                Console.WriteLine("Blam tag does not exist: " + blamTagName);
                return false;
            }

            //
            // Load the Blam scenario_structure_bsp tag
            //
            
            var blamContext = new CacheSerializationContext(BlamCache, blamTag);
            var blamSbsp = BlamCache.Deserializer.Deserialize<ScenarioStructureBsp>(blamContext);

            //
            // Load blam ScenarioLightmapBspData to get geometry for geometry2
            //

            if (BlamCache.Version > CacheVersion.Halo3Retail)
            {
                CacheFile.IndexItem blamLbspTag = null;

                foreach (var tag in BlamCache.IndexItems)
                {
                    if (tag.ClassCode == "sbsp" && tag.Filename == blamTagName)
                    {
                        blamLbspTag = tag;
                        break;
                    }
                }

                var blamLbsp = BlamCache.Deserializer.Deserialize<ScenarioLightmapBspData>(new CacheSerializationContext(BlamCache, blamLbspTag));

                blamSbsp.Geometry2.ZoneAssetHandle = blamLbsp.Geometry.ZoneAssetHandle;
            }
            else
            {
                // H3:
                // Deserialize scnr to get each sbsp's index
                // Order of sbsp's in the scnr is the same as in the sLdT

                CacheFile.IndexItem blamScenarioTag = null;

                foreach (var tag in BlamCache.IndexItems)
                {
                    if (tag.ClassCode == "scnr")
                    {
                        blamScenarioTag = tag;
                        break;
                    }
                }

                var blamScenario = BlamCache.Deserializer.Deserialize<Scenario>(new CacheSerializationContext(BlamCache, blamScenarioTag));

                int sbspIndex = 0;
                for (int i = 0; i < blamScenario.StructureBsps.Count; i++)
                {
                    if (blamScenario.StructureBsps[i].StructureBsp.Index == blamTag.ID)
                    {
                        sbspIndex = i;
                        break;
                    }
                }

                //
                // Get sLdT
                //

                CacheFile.IndexItem blamsLdTTag = null;

                foreach (var tag in BlamCache.IndexItems)
                {
                    if (tag.ClassCode == "sLdT")
                    {
                        blamsLdTTag = tag;
                        break;
                    }
                }

                var blamsLdT = BlamCache.Deserializer.Deserialize<ScenarioLightmap>(new CacheSerializationContext(BlamCache, blamsLdTTag));

                blamSbsp.Geometry2.ZoneAssetHandle = blamsLdT.Lightmaps[sbspIndex].Geometry.ZoneAssetHandle;
            }

            //
            // Load Blam resource data
            //

            var geometry = blamSbsp.Geometry;
            var resourceData = BlamCache.GetRawFromID(geometry.ZoneAssetHandle);

            if (resourceData == null)
            {
                Console.WriteLine("Blam render_geometry resource contains no data. Created empty resource.");
                return false;
            }

            //
            // Load Blam resource definition
            //

            Console.Write("Loading Blam render_geometry resource definition...");

            var definitionEntry = BlamCache.ResourceGestalt.TagResources[geometry.ZoneAssetHandle & ushort.MaxValue];

            var resourceDefinition = new RenderGeometryApiResourceDefinition
            {
                VertexBuffers = new List<D3DPointer<VertexBufferDefinition>>(),
                IndexBuffers = new List<D3DPointer<IndexBufferDefinition>>()
            };

            using (var definitionStream = new MemoryStream(BlamCache.ResourceGestalt.FixupInformation))
            using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
            {
                var dataContext = new DataSerializationContext(definitionReader, null, CacheAddressType.Definition);

                definitionReader.SeekTo(definitionEntry.FixupInformationOffset + (definitionEntry.FixupInformationLength - 24));

                var vertexBufferCount = definitionReader.ReadInt32();
                definitionReader.Skip(8);
                var indexBufferCount = definitionReader.ReadInt32();

                definitionReader.SeekTo(definitionEntry.FixupInformationOffset);

                for (var i = 0; i < vertexBufferCount; i++)
                {
                    resourceDefinition.VertexBuffers.Add(new D3DPointer<VertexBufferDefinition>
                    {
                        Definition = new VertexBufferDefinition
                        {
                            Count = definitionReader.ReadInt32(),
                            Format = (VertexBufferFormat)definitionReader.ReadInt16(),
                            VertexSize = definitionReader.ReadInt16(),
                            Data = new TagData
                            {
                                Size = definitionReader.ReadInt32(),
                                Unused4 = definitionReader.ReadInt32(),
                                Unused8 = definitionReader.ReadInt32(),
                                Address = new CacheAddress(CacheAddressType.Memory, definitionReader.ReadInt32()),
                                Unused10 = definitionReader.ReadInt32()
                            }
                        }
                    });
                }

                definitionReader.Skip(vertexBufferCount * 12);

                for (var i = 0; i < indexBufferCount; i++)
                {
                    resourceDefinition.IndexBuffers.Add(new D3DPointer<IndexBufferDefinition>
                    {
                        Definition = new IndexBufferDefinition
                        {
                            Format = (IndexBufferFormat)definitionReader.ReadInt32(),
                            Data = new TagData
                            {
                                Size = definitionReader.ReadInt32(),
                                Unused4 = definitionReader.ReadInt32(),
                                Unused8 = definitionReader.ReadInt32(),
                                Address = new CacheAddress(CacheAddressType.Memory, definitionReader.ReadInt32()),
                                Unused10 = definitionReader.ReadInt32()
                            }
                        }
                    });
                }
            }

            Console.WriteLine("done.");

            //
            // Convert Blam data to ElDorado data
            //

            using (var fileStream = File.Create(args[1]))
            using (var fileWriter = new StreamWriter(fileStream))
            {
                using (var blamResourceStream = new MemoryStream(resourceData))
                using (var blamResourceReader = new EndianReader(blamResourceStream, EndianFormat.LittleEndian))
                {
                    //
                    // Convert Blam vertex buffers
                    //

                    Console.Write("Converting vertex buffers...");

                    

                    for (var i = 0; i < resourceDefinition.VertexBuffers.Count; i++)
                    {
                        blamResourceStream.Position = definitionEntry.ResourceFixups[i].Offset;

                        var vertexBuffer = resourceDefinition.VertexBuffers[i].Definition;

                        fileWriter.WriteLine($"Offset = {vertexBuffer.Data.Address.Offset.ToString("X8")} Count = {vertexBuffer.Count} Size = {vertexBuffer.VertexSize}, Format = {vertexBuffer.Format.ToString()}");

                        switch (vertexBuffer.Format)
                        {
                            case VertexBufferFormat.TinyPosition:
                                for (var j = 0; j < vertexBuffer.Count; j++)
                                {
                                    fileWriter.WriteLine($"(I,J,K,W) = ({blamResourceReader.ReadUInt32().ToString("X2")},{blamResourceReader.ReadUInt32().ToString("X2")},{blamResourceReader.ReadUInt32().ToString("X2")},{blamResourceReader.ReadUInt32().ToString("X2")})");
                                }
                                break;
                        }
                    }
                    
                }
            }
            return true;
        }

        private static string ConvertTexcoord(uint input)
        {
            RealQuaternion vector = new RealQuaternion(BitConverter.GetBytes(input).Select(e => ConvertByte(e)).ToArray());

            return vector.ToString();
        }

        private static float ConvertByte(byte e)
        {
            var element =e;
            float result;
            if (element < 0)
                result = (float)element / (float)sbyte.MinValue;
            else if (element > 0)
                result = (float)element / (float)sbyte.MaxValue;
            else
                result = 0.0f;

            result = (result + 1.0f) / 2.0f;
            return result;
        }
    }
}

