using TagTool.Cache;
using TagTool.Common;
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
    partial class PortTagCommand
    {
        private PageableResource ConvertStructureBspCacheFileTagResources(ScenarioStructureBsp bsp)
        {
            //
            // Set up ElDorado resource reference
            //

            bsp.PathfindingResource = new PageableResource
            {
                Page = new RawPage
                {
                    Index = -1
                },
                Resource = new TagResource
                {
                    Type = TagResourceType.Pathfinding,
                    DefinitionData = new byte[0x30],
                    DefinitionAddress = new CacheAddress(CacheAddressType.Definition, 0),
                    ResourceFixups = new List<TagResource.ResourceFixup>(),
                    ResourceDefinitionFixups = new List<TagResource.ResourceDefinitionFixup>(),
                    Unknown2 = 1
                }
            };

            //
            // Load Blam resource data
            //

            var resourceData = BlamCache.GetRawFromID(bsp.ZoneAssetIndex4);

            if (resourceData == null)
            {
                Console.WriteLine("Blam structure_bsp_cache_file_tag_resources contains no resource data. Created empty resource.");

                if (BlamCache.Version >= CacheVersion.Halo3ODST)
                    return bsp.PathfindingResource;

                resourceData = new byte[0x30];
            }

            //
            // Port Blam resource definition
            //

            Console.Write("Porting Blam structure_bsp_cache_file_tag_resources resource definition...");

            var blamDeserializer = new TagDeserializer(BlamCache.Version);

            StructureBspCacheFileTagResources resourceDefinition = null;

            if (BlamCache.Version >= CacheVersion.Halo3ODST)
            {
                var resourceEntry = BlamCache.ResourceGestalt.DefinitionEntries[bsp.ZoneAssetIndex4 & ushort.MaxValue];

                bsp.PathfindingResource.Resource.DefinitionAddress = new CacheAddress(CacheAddressType.Definition, resourceEntry.DefinitionAddress);
                bsp.PathfindingResource.Resource.DefinitionData = BlamCache.ResourceGestalt.DefinitionData.Skip(resourceEntry.Offset).Take(resourceEntry.Size).ToArray();

                using (var definitionStream = new MemoryStream(bsp.PathfindingResource.Resource.DefinitionData, true))
                using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
                using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.BigEndian))
                {
                    foreach (var fixup in resourceEntry.Fixups)
                    {
                        var newFixup = new TagResource.ResourceFixup
                        {
                            BlockOffset = (uint)fixup.BlockOffset,
                            Address = new CacheAddress(
                                fixup.FixupType == 4 ?
                                    CacheAddressType.Resource :
                                    CacheAddressType.Definition,
                                fixup.Offset)
                        };

                        definitionStream.Position = newFixup.BlockOffset;
                        definitionWriter.Write(newFixup.Address.Value);

                        bsp.PathfindingResource.Resource.ResourceFixups.Add(newFixup);
                    }

                    var dataContext = new DataSerializationContext(definitionReader, definitionWriter, CacheAddressType.Definition);

                    definitionStream.Position = bsp.PathfindingResource.Resource.DefinitionAddress.Offset;
                    resourceDefinition = blamDeserializer.Deserialize<StructureBspCacheFileTagResources>(dataContext);
                }
            }
            else
            {
                resourceDefinition = new StructureBspCacheFileTagResources()
                {
                    UnknownRaw6ths = new TagBlock<ScenarioStructureBsp.UnknownRaw6th>(bsp.UnknownRaw6ths.Count, new CacheAddress()),
                    UnknownRaw1sts = new TagBlock<ScenarioStructureBsp.UnknownRaw1st>(bsp.UnknownRaw1sts.Count, new CacheAddress()),
                    UnknownRaw7ths = new TagBlock<ScenarioStructureBsp.UnknownRaw7th>(bsp.UnknownRaw7ths.Count, new CacheAddress()),
                    PathfindingData = new List<StructureBspCacheFileTagResources.PathfindingDatum>() // TODO: copy from bsp.PathfindingData...
                };
            }

            Console.WriteLine("done.");

            //
            // Port Blam resource to ElDorado resource cache
            //

            Console.Write("Porting Blam structure_bsp_cache_file_tag_resources resource data...");

            using (var blamResourceStream = new MemoryStream(resourceData))
            using (var resourceReader = new EndianReader(blamResourceStream, EndianFormat.BigEndian))
            using (var edResourceStream = new MemoryStream())
            using (var resourceWriter = new EndianWriter(edResourceStream, EndianFormat.LittleEndian))
            {
                var dataContext = new DataSerializationContext(resourceReader, resourceWriter);

                StreamUtil.Align(edResourceStream, 0x4);
                blamResourceStream.Position = resourceDefinition.UnknownRaw6ths.Address.Offset;
                resourceDefinition.UnknownRaw6ths.Address = new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position);
                for (var i = 0; i < resourceDefinition.UnknownRaw6ths.Count; i++)
                {
                    var element = BlamCache.Version < CacheVersion.Halo3ODST ?
                        bsp.UnknownRaw6ths[i] :
                        blamDeserializer.Deserialize<ScenarioStructureBsp.UnknownRaw6th>(dataContext);

                    if (BlamCache.Version < CacheVersion.Halo3ODST)
                    {
                        element.Unknown1EntryCount = element.Unknown1EntryCountHalo3;
                        element.Unknown1StartIndex = element.Unknown1StartIndexHalo3;
                    }

                    CacheContext.Serializer.Serialize(dataContext, element);
                }

                StreamUtil.Align(edResourceStream, 0x4);
                blamResourceStream.Position = resourceDefinition.UnknownRaw1sts.Address.Offset;
                resourceDefinition.UnknownRaw1sts.Address = new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position);
                for (var i = 0; i < resourceDefinition.UnknownRaw1sts.Count; i++)
                    CacheContext.Serializer.Serialize(dataContext,
                        BlamCache.Version < CacheVersion.Halo3ODST ?
                        bsp.UnknownRaw1sts[i] :
                        blamDeserializer.Deserialize<ScenarioStructureBsp.UnknownRaw1st>(dataContext));

                StreamUtil.Align(edResourceStream, 0x4);
                blamResourceStream.Position = resourceDefinition.UnknownRaw7ths.Address.Offset;
                resourceDefinition.UnknownRaw7ths.Address = new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position);
                for (var i = 0; i < resourceDefinition.UnknownRaw7ths.Count; i++)
                    CacheContext.Serializer.Serialize(dataContext,
                        BlamCache.Version < CacheVersion.Halo3ODST ?
                        bsp.UnknownRaw7ths[i] :
                        blamDeserializer.Deserialize<ScenarioStructureBsp.UnknownRaw7th>(dataContext));

                if (BlamCache.Version < CacheVersion.Halo3ODST && bsp.PathfindingData.Count != 0)
                {
                    var pathfinding = new StructureBspCacheFileTagResources.PathfindingDatum()
                    {
                        StructureChecksum = bsp.PathfindingData[0].StructureChecksum,
                        ObjectReferences = new List<StructureBspCacheFileTagResources.PathfindingDatum.ObjectReference>(),
                        Unknown2s = new List<StructureBspCacheFileTagResources.PathfindingDatum.Unknown2Block>(),
                        Unknown3s = new List<StructureBspCacheFileTagResources.PathfindingDatum.Unknown3Block>()
                    };

                    foreach (var oldObjectReference in bsp.PathfindingData[0].ObjectReferences)
                    {
                        var objectReference = new StructureBspCacheFileTagResources.PathfindingDatum.ObjectReference
                        {
                            Unknown = oldObjectReference.Unknown,
                            Unknown2 = new List<StructureBspCacheFileTagResources.PathfindingDatum.ObjectReference.Unknown1Block>(),
                            Unknown3 = oldObjectReference.Unknown3,
                            Unknown4 = oldObjectReference.Unknown4,
                            Unknown5 = oldObjectReference.Unknown5
                        };

                        foreach (var oldUnknown in oldObjectReference.Unknown2)
                        {
                            objectReference.Unknown2.Add(new StructureBspCacheFileTagResources.PathfindingDatum.ObjectReference.Unknown1Block
                            {
                                Unknown1 = oldUnknown.Unknown1,
                                Unknown2 = oldUnknown.Unknown2,
                                Unknown3 = new TagBlock<StructureBspCacheFileTagResources.PathfindingDatum.ObjectReference.Unknown1Block.Unknown3Block>(oldUnknown.Unknown3.Count, new CacheAddress()),
                                Unknown4 = oldUnknown.Unknown4
                            });
                        }

                        pathfinding.ObjectReferences.Add(objectReference);
                    }

                    foreach (var oldUnknown2 in bsp.PathfindingData[0].Unknown2s)
                    {
                        pathfinding.Unknown2s.Add(new StructureBspCacheFileTagResources.PathfindingDatum.Unknown2Block
                        {
                            Unknown = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Unknown2Block.UnknownBlock>(
                                oldUnknown2.Unknown.Count, new CacheAddress())
                        });
                    }

                    foreach (var oldUnknown3 in bsp.PathfindingData[0].Unknown3s)
                    {
                        pathfinding.Unknown3s.Add(new StructureBspCacheFileTagResources.PathfindingDatum.Unknown3Block
                        {
                            Unknown1 = oldUnknown3.Unknown1,
                            Unknown2 = oldUnknown3.Unknown2,
                            Unknown3 = oldUnknown3.Unknown3,
                            Unknown4 = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Unknown3Block.UnknownBlock>(
                                oldUnknown3.Unknown4.Count, new CacheAddress())
                        });
                    }

                    resourceDefinition.PathfindingData.Add(pathfinding);
                }

                foreach (var pathfindingDatum in resourceDefinition.PathfindingData)
                {
                    StreamUtil.Align(edResourceStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Sectors.Address.Offset;
                    pathfindingDatum.Sectors = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Sector>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Sectors.Count : pathfindingDatum.Sectors.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position));
                    for (var i = 0; i < pathfindingDatum.Sectors.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Sectors[i] :
                            blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Sector>(dataContext));

                    StreamUtil.Align(edResourceStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Links.Address.Offset;
                    pathfindingDatum.Links = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Link>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Links.Count : pathfindingDatum.Links.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position));
                    for (var i = 0; i < pathfindingDatum.Links.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Links[i] :
                            blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Link>(dataContext));

                    StreamUtil.Align(edResourceStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.References.Address.Offset;
                    pathfindingDatum.References = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Reference>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].References.Count : pathfindingDatum.References.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position));
                    for (var i = 0; i < pathfindingDatum.References.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].References[i] :
                            blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Reference>(dataContext));

                    StreamUtil.Align(edResourceStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Bsp2dNodes.Address.Offset;
                    pathfindingDatum.Bsp2dNodes = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Bsp2dNodes.Count : pathfindingDatum.Bsp2dNodes.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position));
                    for (var i = 0; i < pathfindingDatum.Bsp2dNodes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Bsp2dNodes[i] :
                            blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode>(dataContext));

                    StreamUtil.Align(edResourceStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Vertices.Address.Offset;
                    pathfindingDatum.Vertices = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Vertex>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Vertices.Count : pathfindingDatum.Vertices.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position));
                    for (var i = 0; i < pathfindingDatum.Vertices.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Vertices[i] :
                            blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Vertex>(dataContext));

                    for (var objRefIdx = 0; objRefIdx < pathfindingDatum.ObjectReferences.Count; objRefIdx++)
                    {
                        for (var unk2Idx = 0; unk2Idx < pathfindingDatum.ObjectReferences[objRefIdx].Unknown2.Count; unk2Idx++)
                        {
                            var unknown2 = pathfindingDatum.ObjectReferences[objRefIdx].Unknown2[unk2Idx];

                            StreamUtil.Align(edResourceStream, 0x4);
                            if (BlamCache.Version >= CacheVersion.Halo3ODST)
                                blamResourceStream.Position = unknown2.Unknown3.Address.Offset;
                            unknown2.Unknown3.Address = new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position);

                            for (var unk3Idx = 0; unk3Idx < unknown2.Unknown3.Count; unk3Idx++)
                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Version < CacheVersion.Halo3ODST ?
                                    bsp.PathfindingData[0].ObjectReferences[objRefIdx].Unknown2[unk2Idx].Unknown3[unk3Idx] :
                                    blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.ObjectReference.UnknownBlock.UnknownBlock2>(dataContext));
                        }
                    }

                    StreamUtil.Align(edResourceStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.PathfindingHints.Address.Offset;
                    pathfindingDatum.PathfindingHints = new TagBlock<ScenarioStructureBsp.PathfindingDatum.PathfindingHint>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].PathfindingHints.Count : pathfindingDatum.PathfindingHints.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position));
                    for (var i = 0; i < pathfindingDatum.PathfindingHints.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].PathfindingHints[i] :
                            blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.PathfindingHint>(dataContext));

                    StreamUtil.Align(edResourceStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.InstancedGeometryReferences.Address.Offset;
                    pathfindingDatum.InstancedGeometryReferences = new TagBlock<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].InstancedGeometryReferences.Count : pathfindingDatum.InstancedGeometryReferences.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position));
                    for (var i = 0; i < pathfindingDatum.InstancedGeometryReferences.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].InstancedGeometryReferences[i] :
                            blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference>(dataContext));

                    StreamUtil.Align(edResourceStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Unknown1s.Address.Offset;
                    pathfindingDatum.Unknown1s = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Unknown1Block>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Unknown1s.Count : pathfindingDatum.Unknown1s.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position));
                    for (var i = 0; i < pathfindingDatum.Unknown1s.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Unknown1s[i] :
                            blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Unknown1Block>(dataContext));

                    for (var unk2Idx = 0; unk2Idx < pathfindingDatum.Unknown2s.Count; unk2Idx++)
                    {
                        var unknown2 = pathfindingDatum.Unknown2s[unk2Idx];

                        StreamUtil.Align(edResourceStream, 0x4);
                        if (BlamCache.Version >= CacheVersion.Halo3ODST)
                            blamResourceStream.Position = unknown2.Unknown.Address.Offset;
                        unknown2.Unknown.Address = new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position);
                        for (var unkIdx = 0; unkIdx < unknown2.Unknown.Count; unkIdx++)
                            CacheContext.Serializer.Serialize(dataContext,
                                BlamCache.Version < CacheVersion.Halo3ODST ?
                                bsp.PathfindingData[0].Unknown2s[unk2Idx].Unknown[unkIdx] :
                                blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Unknown2Block.UnknownBlock>(dataContext));
                    }

                    for (var unk33Idx = 0; unk33Idx < pathfindingDatum.Unknown3s.Count; unk33Idx++)
                    {
                        var unknown2 = pathfindingDatum.Unknown3s[unk33Idx];

                        StreamUtil.Align(edResourceStream, 0x4);
                        if (BlamCache.Version >= CacheVersion.Halo3ODST)
                            blamResourceStream.Position = unknown2.Unknown4.Address.Offset;
                        unknown2.Unknown4.Address = new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position);
                        for (var unk4Idx = 0; unk4Idx < unknown2.Unknown4.Count; unk4Idx++)
                            CacheContext.Serializer.Serialize(dataContext,
                                BlamCache.Version < CacheVersion.Halo3ODST ?
                                bsp.PathfindingData[0].Unknown3s[unk33Idx].Unknown4[unk4Idx] :
                                blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Unknown3Block.UnknownBlock>(dataContext));
                    }

                    StreamUtil.Align(edResourceStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Unknown4s.Address.Offset;
                    pathfindingDatum.Unknown4s = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Unknown4Block>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Unknown4s.Count : pathfindingDatum.Unknown4s.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)edResourceStream.Position));
                    for (var i = 0; i < pathfindingDatum.Unknown4s.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Unknown4s[i] :
                            blamDeserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Unknown4Block>(dataContext));
                }

                CacheContext.Serializer.Serialize(new ResourceSerializationContext(bsp.PathfindingResource), resourceDefinition);
                resourceWriter.BaseStream.Position = 0;
                edResourceStream.Position = 0;
                CacheContext.AddResource(bsp.PathfindingResource, ResourceLocation.ResourcesB, edResourceStream);
            }

            if (BlamCache.Version < CacheVersion.Halo3ODST)
            {
                bsp.UnknownRaw6ths.Clear();
                bsp.UnknownRaw1sts.Clear();
                bsp.UnknownRaw7ths.Clear();
                bsp.PathfindingData.Clear();
            }

            Console.WriteLine("done.");

            return bsp.PathfindingResource;
        }
    }
}

