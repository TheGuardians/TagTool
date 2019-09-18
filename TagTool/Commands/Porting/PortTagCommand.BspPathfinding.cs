using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using static TagTool.Tags.Definitions.ScenarioStructureBsp.PathfindingDatum.PathfindingHint.HintTypeValue;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private PageableResource ConvertStructureBspCacheFileTagResources(ScenarioStructureBsp bsp, Dictionary<ResourceLocation, Stream> resourceStreams)
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
                Resource = new TagResourceGen3
                {
                    ResourceType = TagResourceTypeGen3.Pathfinding,
                    DefinitionData = new byte[0x30],
                    DefinitionAddress = new CacheAddress(CacheAddressType.Definition, 0),
                    ResourceFixups = new List<TagResourceGen3.ResourceFixup>(),
                    ResourceDefinitionFixups = new List<TagResourceGen3.ResourceDefinitionFixup>(),
                    Unknown2 = 1
                }
            };

            //
            // Load Blam resource data
            //

            var resourceData = BlamCache.Version > CacheVersion.Halo3Retail ?
                    BlamCache.GetRawFromID(bsp.ZoneAssetIndex4) :
                    null;

            if (resourceData == null)
            {
                if (BlamCache.Version >= CacheVersion.Halo3ODST)
                    return bsp.PathfindingResource;

                resourceData = new byte[0x30];
            }

            //
            // Port Blam resource definition
            //

            StructureBspCacheFileTagResources resourceDefinition = null;

            if (BlamCache.Version >= CacheVersion.Halo3ODST)
            {
                var resourceEntry = BlamCache.ResourceGestalt.TagResources[bsp.ZoneAssetIndex4.Index];

                bsp.PathfindingResource.Resource.DefinitionAddress = resourceEntry.DefinitionAddress;
                bsp.PathfindingResource.Resource.DefinitionData = BlamCache.ResourceGestalt.FixupInformation.Skip(resourceEntry.FixupInformationOffset).Take(resourceEntry.FixupInformationLength).ToArray();

                using (var definitionStream = new MemoryStream(bsp.PathfindingResource.Resource.DefinitionData, true))
                using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
                using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.BigEndian))
                {
                    foreach (var fixup in resourceEntry.ResourceFixups)
                    {
                        var newFixup = new TagResourceGen3.ResourceFixup
                        {
                            BlockOffset = (uint)fixup.BlockOffset,
                            Address = new CacheAddress(
                                fixup.Type == 4 ?
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
                    resourceDefinition = BlamCache.Deserializer.Deserialize<StructureBspCacheFileTagResources>(dataContext);
                }
            }
            else
            {
                resourceDefinition = new StructureBspCacheFileTagResources()
                {
                    SurfacePlanes = new TagBlock<ScenarioStructureBsp.SurfacesPlanes>(bsp.SurfacePlanes.Count, new CacheAddress()),
                    Planes = new TagBlock<ScenarioStructureBsp.Plane>(bsp.Planes.Count, new CacheAddress()),
                    EdgeToSeams = new TagBlock<ScenarioStructureBsp.EdgeToSeamMapping>(bsp.EdgeToSeams.Count, new CacheAddress()),
                    PathfindingData = new List<StructureBspCacheFileTagResources.PathfindingDatum>() // TODO: copy from bsp.PathfindingData...
                };
            }

            //
            // Port Blam resource to ElDorado resource cache
            //

            using (var blamResourceStream = new MemoryStream(resourceData))
            using (var resourceReader = new EndianReader(blamResourceStream, EndianFormat.BigEndian))
            using (var dataStream = new MemoryStream())
            using (var resourceWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
            {
                var dataContext = new DataSerializationContext(resourceReader, resourceWriter);

                //
                // Surfaces Planes 
                //

                StreamUtil.Align(dataStream, 0x4);

                if (BlamCache.Version >= CacheVersion.Halo3ODST)
                    blamResourceStream.Position = resourceDefinition.SurfacePlanes.Address.Offset;

                resourceDefinition.SurfacePlanes = new TagBlock<ScenarioStructureBsp.SurfacesPlanes>(
                    (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.SurfacePlanes.Count : resourceDefinition.SurfacePlanes.Count),
                    new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));

                for (var i = 0; i < resourceDefinition.SurfacePlanes.Count; i++)
                {
                    var element = BlamCache.Version < CacheVersion.Halo3ODST ?
                        bsp.SurfacePlanes[i] :
                        BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.SurfacesPlanes>(dataContext);

                    if (BlamCache.Version < CacheVersion.Halo3ODST)
                    {
                        element.PlaneIndexNew = element.PlaneIndexOld;
                        element.PlaneCountNew = element.PlaneCountOld;
                    }

                    CacheContext.Serializer.Serialize(dataContext, element);
                }

                //
                // UnknownRaw1sts
                //

                StreamUtil.Align(dataStream, 0x4);

                if (BlamCache.Version >= CacheVersion.Halo3ODST)
                    blamResourceStream.Position = resourceDefinition.Planes.Address.Offset;

                resourceDefinition.Planes = new TagBlock<ScenarioStructureBsp.Plane>(
                    (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.Planes.Count : resourceDefinition.Planes.Count),
                    new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));

                for (var i = 0; i < resourceDefinition.Planes.Count; i++)
                {
                    var element = BlamCache.Version < CacheVersion.Halo3ODST ?
                        bsp.Planes[i] :
                        BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.Plane>(dataContext);

                    CacheContext.Serializer.Serialize(dataContext, element);
                }

                //
                // UnknownRaw7ths
                //

                StreamUtil.Align(dataStream, 0x4);

                if (BlamCache.Version >= CacheVersion.Halo3ODST)
                    blamResourceStream.Position = resourceDefinition.EdgeToSeams.Address.Offset;

                resourceDefinition.EdgeToSeams = new TagBlock<ScenarioStructureBsp.EdgeToSeamMapping>(
                    (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.EdgeToSeams.Count : resourceDefinition.EdgeToSeams.Count),
                    new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));

                for (var i = 0; i < resourceDefinition.EdgeToSeams.Count; i++)
                {
                    var element = BlamCache.Version < CacheVersion.Halo3ODST ?
                        bsp.EdgeToSeams[i] :
                        BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.EdgeToSeamMapping>(dataContext);

                    CacheContext.Serializer.Serialize(dataContext, element);
                }

                if (BlamCache.Version < CacheVersion.Halo3ODST && bsp.PathfindingData.Count != 0)
                {
                    var pathfinding = new StructureBspCacheFileTagResources.PathfindingDatum()
                    {
                        StructureChecksum = bsp.PathfindingData[0].StructureChecksum,
                        ObjectReferences = new List<StructureBspCacheFileTagResources.PathfindingDatum.ObjectReference>(),
                        Seams = new List<StructureBspCacheFileTagResources.PathfindingDatum.Seam>(),
                        JumpSeams = new List<StructureBspCacheFileTagResources.PathfindingDatum.JumpSeam>()
                    };

                    foreach (var oldObjectReference in bsp.PathfindingData[0].ObjectReferences)
                    {
                        var objectReference = new StructureBspCacheFileTagResources.PathfindingDatum.ObjectReference
                        {
                            Flags = oldObjectReference.Flags,
                            Bsps = new List<StructureBspCacheFileTagResources.PathfindingDatum.ObjectReference.BspReference>(),
                            ObjectHandle = oldObjectReference.ObjectHandle,
                            OriginBspIndex = oldObjectReference.OriginBspIndex,
                            ObjectType = oldObjectReference.ObjectType.DeepClone(),
                            Source = oldObjectReference.Source
                        };

                        foreach (var bspRef in oldObjectReference.Bsps)
                        {
                            objectReference.Bsps.Add(new StructureBspCacheFileTagResources.PathfindingDatum.ObjectReference.BspReference
                            {
                                BspHandle = bspRef.BspHandle,
                                NodeIndex = bspRef.NodeIndex,
                                Bsp2dRefs = new TagBlock<ScenarioStructureBsp.PathfindingDatum.ObjectReference.BspReference.Bsp2dRef>(bspRef.Bsp2dRefs.Count, new CacheAddress()),
                                VertexOffset = bspRef.VertexOffset
                            });
                        }

                        pathfinding.ObjectReferences.Add(objectReference);
                    }

                    foreach (var oldSeam in bsp.PathfindingData[0].Seams)
                    {
                        pathfinding.Seams.Add(new StructureBspCacheFileTagResources.PathfindingDatum.Seam
                        {
                            LinkIndices = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Seam.LinkIndexBlock>(
                                oldSeam.LinkIndices.Count, new CacheAddress())
                        });
                    }

                    foreach (var oldJumpSeam in bsp.PathfindingData[0].JumpSeams)
                    {
                        pathfinding.JumpSeams.Add(new StructureBspCacheFileTagResources.PathfindingDatum.JumpSeam
                        {
                            UserJumpIndex = oldJumpSeam.UserJumpIndex,
                            DestOnly = oldJumpSeam.DestOnly,
                            Length = oldJumpSeam.Length,
                            JumpIndices = new TagBlock<ScenarioStructureBsp.PathfindingDatum.JumpSeam.JumpIndexBlock>(
                                oldJumpSeam.JumpIndices.Count, new CacheAddress())
                        });
                    }

                    resourceDefinition.PathfindingData.Add(pathfinding);
                }

                foreach (var pathfindingDatum in resourceDefinition.PathfindingData)
                {
                    StreamUtil.Align(dataStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Sectors.Address.Offset;
                    pathfindingDatum.Sectors = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Sector>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Sectors.Count : pathfindingDatum.Sectors.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));
                    for (var i = 0; i < pathfindingDatum.Sectors.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Sectors[i] :
                            BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Sector>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Links.Address.Offset;
                    pathfindingDatum.Links = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Link>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Links.Count : pathfindingDatum.Links.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));
                    for (var i = 0; i < pathfindingDatum.Links.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Links[i] :
                            BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Link>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.References.Address.Offset;
                    pathfindingDatum.References = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Reference>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].References.Count : pathfindingDatum.References.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));
                    for (var i = 0; i < pathfindingDatum.References.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].References[i] :
                            BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Reference>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Bsp2dNodes.Address.Offset;
                    pathfindingDatum.Bsp2dNodes = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Bsp2dNodes.Count : pathfindingDatum.Bsp2dNodes.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));
                    for (var i = 0; i < pathfindingDatum.Bsp2dNodes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Bsp2dNodes[i] :
                            BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Vertices.Address.Offset;
                    pathfindingDatum.Vertices = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Vertex>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Vertices.Count : pathfindingDatum.Vertices.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));
                    for (var i = 0; i < pathfindingDatum.Vertices.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Vertices[i] :
                            BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Vertex>(dataContext));

                    for (var objRefIdx = 0; objRefIdx < pathfindingDatum.ObjectReferences.Count; objRefIdx++)
                    {
                        for (var bspRefIdx = 0; bspRefIdx < pathfindingDatum.ObjectReferences[objRefIdx].Bsps.Count; bspRefIdx++)
                        {
                            var bspRef = pathfindingDatum.ObjectReferences[objRefIdx].Bsps[bspRefIdx];

                            StreamUtil.Align(dataStream, 0x4);
                            if (BlamCache.Version >= CacheVersion.Halo3ODST)
                                blamResourceStream.Position = bspRef.Bsp2dRefs.Address.Offset;
                            bspRef.Bsp2dRefs.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);

                            for (var bsp2dRefIdx = 0; bsp2dRefIdx < bspRef.Bsp2dRefs.Count; bsp2dRefIdx++)
                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Version < CacheVersion.Halo3ODST ?
                                        bsp.PathfindingData[0].ObjectReferences[objRefIdx].Bsps[bspRefIdx].Bsp2dRefs[bsp2dRefIdx] :
                                        BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.ObjectReference.BspReference.Bsp2dRef>(dataContext));
                        }
                    }

                    StreamUtil.Align(dataStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.PathfindingHints.Address.Offset;
                    pathfindingDatum.PathfindingHints = new TagBlock<ScenarioStructureBsp.PathfindingDatum.PathfindingHint>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].PathfindingHints.Count : pathfindingDatum.PathfindingHints.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));
                    for (var i = 0; i < pathfindingDatum.PathfindingHints.Count; i++)
                    {
                        var hint = BlamCache.Version < CacheVersion.Halo3ODST ?
                        bsp.PathfindingData[0].PathfindingHints[i] :
                        BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.PathfindingHint>(dataContext);

                        if (BlamCache.Version < CacheVersion.Halo3ODST && 
                            (hint.HintType == JumpLink || hint.HintType == WallJumpLink))
                        {
                            hint.Data[3] = (hint.Data[3] & ~ushort.MaxValue) | ((hint.Data[2] >> 16) & ushort.MaxValue);
                            hint.Data[2] = (hint.Data[2] & ~(ushort.MaxValue << 16)) << 8;
                        }

                        CacheContext.Serializer.Serialize(dataContext, hint);
                    }                                      

                    StreamUtil.Align(dataStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.InstancedGeometryReferences.Address.Offset;
                    pathfindingDatum.InstancedGeometryReferences = new TagBlock<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].InstancedGeometryReferences.Count : pathfindingDatum.InstancedGeometryReferences.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));
                    for (var i = 0; i < pathfindingDatum.InstancedGeometryReferences.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].InstancedGeometryReferences[i] :
                            BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.GiantPathfinding.Address.Offset;
                    pathfindingDatum.GiantPathfinding = new TagBlock<ScenarioStructureBsp.PathfindingDatum.GiantPathfindingBlock>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].GiantPathfinding.Count : pathfindingDatum.GiantPathfinding.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));
                    for (var i = 0; i < pathfindingDatum.GiantPathfinding.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].GiantPathfinding[i] :
                            BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.GiantPathfindingBlock>(dataContext));

                    for (var unk2Idx = 0; unk2Idx < pathfindingDatum.Seams.Count; unk2Idx++)
                    {
                        var unknown2 = pathfindingDatum.Seams[unk2Idx];

                        StreamUtil.Align(dataStream, 0x4);

                        if (BlamCache.Version >= CacheVersion.Halo3ODST)
                            blamResourceStream.Position = unknown2.LinkIndices.Address.Offset;

                        unknown2.LinkIndices = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Seam.LinkIndexBlock>(
                            (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Seams[unk2Idx].LinkIndices.Count : unknown2.LinkIndices.Count),
                            new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));

                        for (var unkIdx = 0; unkIdx < unknown2.LinkIndices.Count; unkIdx++)
                            CacheContext.Serializer.Serialize(dataContext,
                                BlamCache.Version < CacheVersion.Halo3ODST ?
                                    bsp.PathfindingData[0].Seams[unk2Idx].LinkIndices[unkIdx] :
                                    BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Seam.LinkIndexBlock>(dataContext));
                    }

                    for (var unk3Idx = 0; unk3Idx < pathfindingDatum.JumpSeams.Count; unk3Idx++)
                    {
                        var unknown3 = pathfindingDatum.JumpSeams[unk3Idx];

                        StreamUtil.Align(dataStream, 0x4);

                        if (BlamCache.Version >= CacheVersion.Halo3ODST)
                            blamResourceStream.Position = unknown3.JumpIndices.Address.Offset;

                        unknown3.JumpIndices = new TagBlock<ScenarioStructureBsp.PathfindingDatum.JumpSeam.JumpIndexBlock>(
                            (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].JumpSeams[unk3Idx].JumpIndices.Count : unknown3.JumpIndices.Count),
                            new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));

                        for (var unk4Idx = 0; unk4Idx < unknown3.JumpIndices.Count; unk4Idx++)
                            CacheContext.Serializer.Serialize(dataContext,
                                BlamCache.Version < CacheVersion.Halo3ODST ?
                                    bsp.PathfindingData[0].JumpSeams[unk3Idx].JumpIndices[unk4Idx] :
                                    BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.JumpSeam.JumpIndexBlock>(dataContext));
                    }

                    StreamUtil.Align(dataStream, 0x4);
                    if (BlamCache.Version >= CacheVersion.Halo3ODST)
                        blamResourceStream.Position = pathfindingDatum.Doors.Address.Offset;
                    pathfindingDatum.Doors = new TagBlock<ScenarioStructureBsp.PathfindingDatum.Door>(
                        (BlamCache.Version < CacheVersion.Halo3ODST ? bsp.PathfindingData[0].Doors.Count : pathfindingDatum.Doors.Count),
                        new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position));
                    for (var i = 0; i < pathfindingDatum.Doors.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext,
                            BlamCache.Version < CacheVersion.Halo3ODST ?
                            bsp.PathfindingData[0].Doors[i] :
                            BlamCache.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Door>(dataContext));
                }

                CacheContext.Serializer.Serialize(new ResourceSerializationContext(CacheContext, bsp.PathfindingResource), resourceDefinition);
                resourceWriter.BaseStream.Position = 0;
                dataStream.Position = 0;

                bsp.PathfindingResource.ChangeLocation(ResourceLocation.ResourcesB);
                var resource = bsp.PathfindingResource;

                if (resource == null)
                    throw new ArgumentNullException("resource");

                if (!dataStream.CanRead)
                    throw new ArgumentException("The input stream is not open for reading", "dataStream");

                var cache = CacheContext.GetResourceCache(ResourceLocation.ResourcesB);

                if (!resourceStreams.ContainsKey(ResourceLocation.ResourcesB))
                {
                    resourceStreams[ResourceLocation.ResourcesB] = FlagIsSet(PortingFlags.Memory) ?
                        new MemoryStream() :
                        (Stream)CacheContext.OpenResourceCacheReadWrite(ResourceLocation.ResourcesB);

                    if (FlagIsSet(PortingFlags.Memory))
                        using (var resourceStream = CacheContext.OpenResourceCacheRead(ResourceLocation.ResourcesB))
                            resourceStream.CopyTo(resourceStreams[ResourceLocation.ResourcesB]);
                }

                var dataSize = (int)(dataStream.Length - dataStream.Position);
                var data = new byte[dataSize];
                dataStream.Read(data, 0, dataSize);

                resource.Page.Index = cache.Add(resourceStreams[ResourceLocation.ResourcesB], data, out uint compressedSize);
                resource.Page.CompressedBlockSize = compressedSize;
                resource.Page.UncompressedBlockSize = (uint)dataSize;
                resource.DisableChecksum();
            }

            if (BlamCache.Version < CacheVersion.Halo3ODST)
            {
                bsp.SurfacePlanes.Clear();
                bsp.Planes.Clear();
                bsp.EdgeToSeams.Clear();
                bsp.PathfindingData.Clear();
            }

            return bsp.PathfindingResource;
        }
    }
}