using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private int ConvertMOPPWorldIndex(int worldIndex)
        {
            int descriptor = 0x1;
            int flags = (worldIndex & 0x70000) >> 16;
            int surfaceIndex = worldIndex & 0xFFFF;

            int result = 0;
            result += descriptor << 29;
            result += flags << 26;
            result += surfaceIndex;
            return result;
        }

        private int ConvertMOPPInstancedGeometryIndex(int geoIndex)
        {
            int descriptor = 0x2;
            int surfaceIndex = (geoIndex & 0x1FFF0000) >> 16;
            int instancedGeometryIndex = geoIndex & 0xFFFF;

            int result = 0;
            result += descriptor << 29;
            result += surfaceIndex << 26;
            result += instancedGeometryIndex;
            return result;
        }

        private int ConvertMOPPType00(int worldIndex)
        {
            int descriptor = 0x0;
            int flags = (worldIndex & 0x70000) >> 16;
            int surfaceIndex = worldIndex & 0xFFFF;

            int result = 0;
            result += descriptor << 29;
            result += flags << 26;
            result += surfaceIndex;
            return result;
        }

        private int ConvertMOPPType11(int worldIndex)
        {
            int descriptor = 0x3;
            int flags = (worldIndex & 0x70000) >> 16;
            int surfaceIndex = worldIndex & 0xFFFF;

            int result = 0;
            result += descriptor << 29;
            result += flags << 26;
            result += surfaceIndex;
            return result;
        }

        private List<CollisionMoppCode.Datum> ConvertCollisionMoppData(List<CollisionMoppCode.Datum> moppData)
        {
            if (BlamCache.Version > CacheVersion.Halo3Retail)
                return moppData;

            for (var i = 0; i < moppData.Count; i++)
            {
                var moppOperator = moppData[i];

                switch (moppOperator.Value)
                {
                    case 0x00: // HK_MOPP_RETURN
                        break;

                    case 0x01: // HK_MOPP_SCALE1
                    case 0x02: // HK_MOPP_SCALE2
                    case 0x03: // HK_MOPP_SCALE3
                    case 0x04: // HK_MOPP_SCALE4
                        i += 3;
                        break;

                    case 0x05: // HK_MOPP_JUMP8
                        i += 1;
                        break;

                    case 0x06: // HK_MOPP_JUMP16
                        i += 2;
                        break;

                    case 0x07: // HK_MOPP_JUMP24
                        i += 3;
                        break;

                    /*case 0x08: // HK_MOPP_JUMP32 (NOT IMPLEMENTED)
                        Array.Reverse(moppData, i + 1, 4);
                        i += 4;
                        break;*/

                    case 0x09: // HK_MOPP_TERM_REOFFSET8
                        i += 1;
                        break;

                    case 0x0A: // HK_MOPP_TERM_REOFFSET16
                        i += 2;
                        break;

                    case 0x0C: // HK_MOPP_JUMP_CHUNK
                        i += 2;
                        break;

                    case 0x0D: // HK_MOPP_DATA_OFFSET
                        i += 5;
                        break;

                    /*case 0x0E: // UNUSED
                    case 0x0F: // UNUSED
                        break;*/

                    case 0x10: // HK_MOPP_SPLIT_X
                    case 0x11: // HK_MOPP_SPLIT_Y
                    case 0x12: // HK_MOPP_SPLIT_Z
                    case 0x13: // HK_MOPP_SPLIT_YZ
                    case 0x14: // HK_MOPP_SPLIT_YMZ
                    case 0x15: // HK_MOPP_SPLIT_XZ
                    case 0x16: // HK_MOPP_SPLIT_XMZ
                    case 0x17: // HK_MOPP_SPLIT_XY
                    case 0x18: // HK_MOPP_SPLIT_XMY
                    case 0x19: // HK_MOPP_SPLIT_XYZ
                    case 0x1A: // HK_MOPP_SPLIT_XYMZ
                    case 0x1B: // HK_MOPP_SPLIT_XMYZ
                    case 0x1C: // HK_MOPP_SPLIT_XMYMZ
                        i += 3;
                        break;

                    /*case 0x1D: // UNUSED
                    case 0x1E: // UNUSED
                    case 0x1F: // UNUSED
                        break;*/

                    case 0x20: // HK_MOPP_SINGLE_SPLIT_X
                    case 0x21: // HK_MOPP_SINGLE_SPLIT_Y
                    case 0x22: // HK_MOPP_SINGLE_SPLIT_Z
                        i += 2;
                        break;

                    case 0x23: // HK_MOPP_SPLIT_JUMP_X
                    case 0x24: // HK_MOPP_SPLIT_JUMP_Y
                    case 0x25: // HK_MOPP_SPLIT_JUMP_Z
                        i += 6;
                        break;


                    case 0x26: // HK_MOPP_DOUBLE_CUT_X
                    case 0x27: // HK_MOPP_DOUBLE_CUT_Y
                    case 0x28: // HK_MOPP_DOUBLE_CUT_Z
                        i += 2;
                        break;

                    case 0x29: // HK_MOPP_DOUBLE_CUT24_X
                    case 0x2A: // HK_MOPP_DOUBLE_CUT24_Y
                    case 0x2B: // HK_MOPP_DOUBLE_CUT24_Z
                        i += 6;
                        break;

                    /*case 0x2C: // UNUSED
                    case 0x2D: // UNUSED
                    case 0x2E: // UNUSED
                    case 0x2F: // UNUSED
                        break;*/

                    case 0x30: // HK_MOPP_TERM4_0
                    case 0x31: // HK_MOPP_TERM4_1
                    case 0x32: // HK_MOPP_TERM4_2
                    case 0x33: // HK_MOPP_TERM4_3
                    case 0x34: // HK_MOPP_TERM4_4
                    case 0x35: // HK_MOPP_TERM4_5
                    case 0x36: // HK_MOPP_TERM4_6
                    case 0x37: // HK_MOPP_TERM4_7
                    case 0x38: // HK_MOPP_TERM4_8
                    case 0x39: // HK_MOPP_TERM4_9
                    case 0x3A: // HK_MOPP_TERM4_A
                    case 0x3B: // HK_MOPP_TERM4_B
                    case 0x3C: // HK_MOPP_TERM4_C
                    case 0x3D: // HK_MOPP_TERM4_D
                    case 0x3E: // HK_MOPP_TERM4_E
                    case 0x3F: // HK_MOPP_TERM4_F
                    case 0x40: // HK_MOPP_TERM4_10
                    case 0x41: // HK_MOPP_TERM4_11
                    case 0x42: // HK_MOPP_TERM4_12
                    case 0x43: // HK_MOPP_TERM4_13
                    case 0x44: // HK_MOPP_TERM4_14
                    case 0x45: // HK_MOPP_TERM4_15
                    case 0x46: // HK_MOPP_TERM4_16
                    case 0x47: // HK_MOPP_TERM4_17
                    case 0x48: // HK_MOPP_TERM4_18
                    case 0x49: // HK_MOPP_TERM4_19
                    case 0x4A: // HK_MOPP_TERM4_1A
                    case 0x4B: // HK_MOPP_TERM4_1B
                    case 0x4C: // HK_MOPP_TERM4_1C
                    case 0x4D: // HK_MOPP_TERM4_1D
                    case 0x4E: // HK_MOPP_TERM4_1E
                    case 0x4F: // HK_MOPP_TERM4_1F
                               // TODO: Does this function take any operands?
                        break;

                    case 0x50: // HK_MOPP_TERM8
                        i += 1;
                        break;

                    case 0x51: // HK_MOPP_TERM16
                        i += 2;
                        break;

                    case 0x52: // HK_MOPP_TERM24
                        i += 3;
                        break;

                    case 0x0B: // HK_MOPP_TERM_REOFFSET32
                    case 0x53: // HK_MOPP_TERM32
                        int result = BitConverter.ToInt32(new byte[] { moppData[i + 4].Value, moppData[i + 3].Value, moppData[i + 2].Value, moppData[i + 1].Value }, 0);

                        if (moppData[i + 1].Value == 0x20)
                        {
                            result = ConvertMOPPWorldIndex(result);
                        }
                        else if (moppData[i + 1].Value == 0x40)
                        {
                            result = ConvertMOPPInstancedGeometryIndex(result);
                        }
                        else if (moppData[i + 1].Value == 0x00)
                        {
                            result = ConvertMOPPType00(result);
                        }
                        else if (moppData[i + 1].Value == 0x60)
                        {
                            result = ConvertMOPPType11(result);
                        }
                        else
                        {
                            throw new NotSupportedException($"Type of 0x{moppData[i + 1].Value:X2} {result:X8}");
                        }
                        moppData[i + 1] = new CollisionMoppCode.Datum { Value = (byte)((result & 0x7F000000) >> 24) };
                        moppData[i + 2] = new CollisionMoppCode.Datum { Value = (byte)((result & 0x00FF0000) >> 16) };
                        moppData[i + 3] = new CollisionMoppCode.Datum { Value = (byte)((result & 0x0000FF00) >> 8) };
                        moppData[i + 4] = new CollisionMoppCode.Datum { Value = (byte)(result & 0x000000FF) };

                        i += 4;
                        break;

                    case 0x54: // HK_MOPP_NTERM_8
                        i += 1;
                        break;

                    case 0x55: // HK_MOPP_NTERM_16
                        i += 2;
                        break;

                    case 0x56: // HK_MOPP_NTERM_24
                        i += 3;
                        break;

                    case 0x57: // HK_MOPP_NTERM_32
                        i += 4;
                        break;

                    /*case 0x58: // UNUSED
                    case 0x59: // UNUSED
                    case 0x5A: // UNUSED
                    case 0x5B: // UNUSED
                    case 0x5C: // UNUSED
                    case 0x5D: // UNUSED
                    case 0x5E: // UNUSED
                    case 0x5F: // UNUSED
                        break;*/

                    case 0x60: // HK_MOPP_PROPERTY8_0
                    case 0x61: // HK_MOPP_PROPERTY8_1
                    case 0x62: // HK_MOPP_PROPERTY8_2
                    case 0x63: // HK_MOPP_PROPERTY8_3
                        i += 1;
                        break;

                    case 0x64: // HK_MOPP_PROPERTY16_0
                    case 0x65: // HK_MOPP_PROPERTY16_1
                    case 0x66: // HK_MOPP_PROPERTY16_2
                    case 0x67: // HK_MOPP_PROPERTY16_3
                        i += 2;
                        break;

                    case 0x68: // HK_MOPP_PROPERTY32_0
                    case 0x69: // HK_MOPP_PROPERTY32_1
                    case 0x6A: // HK_MOPP_PROPERTY32_2
                    case 0x6B: // HK_MOPP_PROPERTY32_3
                        i += 4;
                        break;

                    default:
                        throw new NotSupportedException($"Opcode 0x{moppOperator:X2}");
                }
            }

            return moppData;
        }

        private PageableResource ConvertStructureBspTagResources(ScenarioStructureBsp bsp, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            //
            // Set up ElDorado resource reference
            //

            bsp.CollisionBspResource = new PageableResource
            {
                Page = new RawPage
                {
                    Index = -1
                },
                Resource = new TagResourceGen3
                {
                    ResourceType = TagResourceTypeGen3.Collision,
                    DefinitionData = new byte[0x30],
                    DefinitionAddress = new CacheAddress(CacheAddressType.Definition, 0),
                    ResourceFixups = new List<TagResourceGen3.ResourceFixup>(),
                    ResourceDefinitionFixups = new List<TagResourceGen3.ResourceDefinitionFixup>(),
                    Unknown2 = 1
                }
            };

            //
            // Port Blam resource definition
            //
            
            var resourceEntry = BlamCache.ResourceGestalt.TagResources[bsp.ZoneAssetIndex3 & ushort.MaxValue];

            bsp.CollisionBspResource.Resource.DefinitionAddress = resourceEntry.DefinitionAddress;
            bsp.CollisionBspResource.Resource.DefinitionData = BlamCache.ResourceGestalt.FixupInformation.Skip(resourceEntry.FixupInformationOffset).Take(resourceEntry.FixupInformationLength).ToArray();

            StructureBspTagResources resourceDefinition = null;

            using (var definitionStream = new MemoryStream(bsp.CollisionBspResource.Resource.DefinitionData, true))
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

                    bsp.CollisionBspResource.Resource.ResourceFixups.Add(newFixup);
                }

                var dataContext = new DataSerializationContext(definitionReader, definitionWriter, CacheAddressType.Definition);

                definitionStream.Position = bsp.CollisionBspResource.Resource.DefinitionAddress.Offset;
                resourceDefinition = BlamCache.Deserializer.Deserialize<StructureBspTagResources>(dataContext);

                //
                // Apply game-specific fixes to the resource definition
                //

                if (BlamCache.Version < CacheVersion.Halo3ODST)
                {
                    resourceDefinition.LargeCollisionBsps = new List<StructureBspTagResources.LargeCollisionBspBlock>();
                    resourceDefinition.HavokData = new List<StructureBspTagResources.HavokDatum>();
                }

                foreach (var instance in resourceDefinition.InstancedGeometry)
                {
                    instance.Unknown5 = new TagBlock<StructureBspTagResources.InstancedGeometryBlock.Unknown4Block>();
                    instance.Unknown2 = new TagBlock<StructureBspTagResources.InstancedGeometryBlock.Unknown2Block>();
                }
            }

            //
            // Load Blam resource data
            //

            var resourceData = BlamCache.GetRawFromID(bsp.ZoneAssetIndex3);

            if (resourceData == null)
            {
                CacheContext.Serializer.Serialize(new ResourceSerializationContext(bsp.CollisionBspResource), resourceDefinition);
                return bsp.CollisionBspResource;
            }

            //
            // Port Blam resource to ElDorado resource cache
            //

            using (var blamResourceStream = resourceData != null ? new MemoryStream(resourceData) : new MemoryStream())
            using (var resourceReader = new EndianReader(blamResourceStream, EndianFormat.BigEndian))
            using (var dataStream = new MemoryStream())
            using (var resourceWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
            {
                var dataContext = new DataSerializationContext(resourceReader, resourceWriter);

                foreach (var collisionBsp in resourceDefinition.CollisionBsps)
                {
                    StreamUtil.Align(dataStream, 0x8);
                    blamResourceStream.Position = collisionBsp.Bsp3dNodes.Address.Offset;
                    collisionBsp.Bsp3dNodes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < collisionBsp.Bsp3dNodes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Bsp3dNode>(dataContext));

                    StreamUtil.Align(dataStream, 0x10);
                    blamResourceStream.Position = collisionBsp.Planes.Address.Offset;
                    collisionBsp.Planes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < collisionBsp.Planes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Plane>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    blamResourceStream.Position = collisionBsp.Leaves.Address.Offset;
                    collisionBsp.Leaves.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < collisionBsp.Leaves.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Leaf>(dataContext));

                    StreamUtil.Align(dataStream, 0x4); // 0x4 > 0x10
                    blamResourceStream.Position = collisionBsp.Bsp2dReferences.Address.Offset;
                    collisionBsp.Bsp2dReferences.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < collisionBsp.Bsp2dReferences.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Bsp2dReference>(dataContext));

                    StreamUtil.Align(dataStream, 0x10);
                    blamResourceStream.Position = collisionBsp.Bsp2dNodes.Address.Offset;
                    collisionBsp.Bsp2dNodes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < collisionBsp.Bsp2dNodes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Bsp2dNode>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    blamResourceStream.Position = collisionBsp.Surfaces.Address.Offset;
                    collisionBsp.Surfaces.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < collisionBsp.Surfaces.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Surface>(dataContext));

                    StreamUtil.Align(dataStream, 0x4); // 0x4 > 0x10
                    blamResourceStream.Position = collisionBsp.Edges.Address.Offset;
                    collisionBsp.Edges.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < collisionBsp.Edges.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Edge>(dataContext));

                    StreamUtil.Align(dataStream, 0x10);
                    blamResourceStream.Position = collisionBsp.Vertices.Address.Offset;
                    collisionBsp.Vertices.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < collisionBsp.Vertices.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Vertex>(dataContext));
                }

                foreach (var largeCollisionBsp in resourceDefinition.LargeCollisionBsps)
                {
                    StreamUtil.Align(dataStream, 0x4);
                    blamResourceStream.Position = largeCollisionBsp.Bsp3dNodes.Address.Offset;
                    largeCollisionBsp.Bsp3dNodes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < largeCollisionBsp.Bsp3dNodes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<StructureBspTagResources.LargeCollisionBspBlock.Bsp3dNode>(dataContext));

                    StreamUtil.Align(dataStream, 0x10);
                    blamResourceStream.Position = largeCollisionBsp.Planes.Address.Offset;
                    largeCollisionBsp.Planes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < largeCollisionBsp.Planes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Plane>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    blamResourceStream.Position = largeCollisionBsp.Leaves.Address.Offset;
                    largeCollisionBsp.Leaves.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < largeCollisionBsp.Leaves.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Leaf>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    blamResourceStream.Position = largeCollisionBsp.Bsp2dReferences.Address.Offset;
                    largeCollisionBsp.Bsp2dReferences.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < largeCollisionBsp.Bsp2dReferences.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<StructureBspTagResources.LargeCollisionBspBlock.Bsp2dReference>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    blamResourceStream.Position = largeCollisionBsp.Bsp2dNodes.Address.Offset;
                    largeCollisionBsp.Bsp2dNodes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < largeCollisionBsp.Bsp2dNodes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<StructureBspTagResources.LargeCollisionBspBlock.Bsp2dNode>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    blamResourceStream.Position = largeCollisionBsp.Surfaces.Address.Offset;
                    largeCollisionBsp.Surfaces.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < largeCollisionBsp.Surfaces.Count; i++)
                    {
                        var surface = BlamCache.Deserializer.Deserialize<StructureBspTagResources.LargeCollisionBspBlock.Surface>(dataContext);
                        // surface.Material = PortGlobalMaterialIndex(CacheContext, BlamCache, surface.Material);
                        CacheContext.Serializer.Serialize(dataContext, surface);
                    }

                    StreamUtil.Align(dataStream, 0x10); // 0x4 > 0x10
                    blamResourceStream.Position = largeCollisionBsp.Edges.Address.Offset;
                    largeCollisionBsp.Edges.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < largeCollisionBsp.Edges.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<StructureBspTagResources.LargeCollisionBspBlock.Edge>(dataContext));

                    StreamUtil.Align(dataStream, 0x8);
                    blamResourceStream.Position = largeCollisionBsp.Vertices.Address.Offset;
                    largeCollisionBsp.Vertices.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < largeCollisionBsp.Vertices.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<StructureBspTagResources.LargeCollisionBspBlock.Vertex>(dataContext));
                }

                foreach (var instance in resourceDefinition.InstancedGeometry)
                {
                    StreamUtil.Align(dataStream, 0x8); // 0x8 > 0x10
                    blamResourceStream.Position = instance.CollisionBsp.Bsp3dNodes.Address.Offset;
                    instance.CollisionBsp.Bsp3dNodes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.CollisionBsp.Bsp3dNodes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Bsp3dNode>(dataContext));

                    StreamUtil.Align(dataStream, 0x10);
                    blamResourceStream.Position = instance.CollisionBsp.Planes.Address.Offset;
                    instance.CollisionBsp.Planes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.CollisionBsp.Planes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Plane>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    blamResourceStream.Position = instance.CollisionBsp.Leaves.Address.Offset;
                    instance.CollisionBsp.Leaves.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.CollisionBsp.Leaves.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Leaf>(dataContext));

                    StreamUtil.Align(dataStream, 0x4); // 0x4 > 0x10
                    blamResourceStream.Position = instance.CollisionBsp.Bsp2dReferences.Address.Offset;
                    instance.CollisionBsp.Bsp2dReferences.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.CollisionBsp.Bsp2dReferences.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Bsp2dReference>(dataContext));

                    StreamUtil.Align(dataStream, 0x10);
                    blamResourceStream.Position = instance.CollisionBsp.Bsp2dNodes.Address.Offset;
                    instance.CollisionBsp.Bsp2dNodes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.CollisionBsp.Bsp2dNodes.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Bsp2dNode>(dataContext));

                    StreamUtil.Align(dataStream, 0x4);
                    blamResourceStream.Position = instance.CollisionBsp.Surfaces.Address.Offset;
                    instance.CollisionBsp.Surfaces.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.CollisionBsp.Surfaces.Count; i++)
                    {
                        var surface = BlamCache.Deserializer.Deserialize<CollisionGeometry.Surface>(dataContext);
                        // surface.Material = PortGlobalMaterialIndex(CacheContext, BlamCache, surface.Material);
                        CacheContext.Serializer.Serialize(dataContext, surface);
                    }

                    StreamUtil.Align(dataStream, 0x4); // 0x4 > 0x10
                    blamResourceStream.Position = instance.CollisionBsp.Edges.Address.Offset;
                    instance.CollisionBsp.Edges.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.CollisionBsp.Edges.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Edge>(dataContext));

                    StreamUtil.Align(dataStream, 0x10);
                    blamResourceStream.Position = instance.CollisionBsp.Vertices.Address.Offset;
                    instance.CollisionBsp.Vertices.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.CollisionBsp.Vertices.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Vertex>(dataContext));

                    foreach (var collisionGeometry in instance.CollisionGeometries)
                    {
                        StreamUtil.Align(dataStream, 0x8); // 0x8 > 0x10
                        blamResourceStream.Position = collisionGeometry.Bsp3dNodes.Address.Offset;
                        collisionGeometry.Bsp3dNodes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                        for (var i = 0; i < collisionGeometry.Bsp3dNodes.Count; i++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Bsp3dNode>(dataContext));

                        StreamUtil.Align(dataStream, 0x10);
                        blamResourceStream.Position = collisionGeometry.Planes.Address.Offset;
                        collisionGeometry.Planes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                        for (var i = 0; i < collisionGeometry.Planes.Count; i++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Plane>(dataContext));

                        StreamUtil.Align(dataStream, 0x4);
                        blamResourceStream.Position = collisionGeometry.Leaves.Address.Offset;
                        collisionGeometry.Leaves.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                        for (var i = 0; i < collisionGeometry.Leaves.Count; i++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Leaf>(dataContext));

                        StreamUtil.Align(dataStream, 0x4);
                        blamResourceStream.Position = collisionGeometry.Bsp2dReferences.Address.Offset;
                        collisionGeometry.Bsp2dReferences.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                        for (var i = 0; i < collisionGeometry.Bsp2dReferences.Count; i++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Bsp2dReference>(dataContext));

                        StreamUtil.Align(dataStream, 0x10);
                        blamResourceStream.Position = collisionGeometry.Bsp2dNodes.Address.Offset;
                        collisionGeometry.Bsp2dNodes.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                        for (var i = 0; i < collisionGeometry.Bsp2dNodes.Count; i++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Bsp2dNode>(dataContext));

                        StreamUtil.Align(dataStream, 0x4);
                        blamResourceStream.Position = collisionGeometry.Surfaces.Address.Offset;
                        collisionGeometry.Surfaces.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                        for (var i = 0; i < collisionGeometry.Surfaces.Count; i++)
                        {
                            var surface = BlamCache.Deserializer.Deserialize<CollisionGeometry.Surface>(dataContext);
                            // surface.Material = PortGlobalMaterialIndex(CacheContext, BlamCache, surface.Material);
                            CacheContext.Serializer.Serialize(dataContext, surface);
                        }

                        StreamUtil.Align(dataStream, 0x4); // 0x4 > 0x10
                        blamResourceStream.Position = collisionGeometry.Edges.Address.Offset;
                        collisionGeometry.Edges.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                        for (var i = 0; i < collisionGeometry.Edges.Count; i++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Edge>(dataContext));

                        StreamUtil.Align(dataStream, 0x10);
                        blamResourceStream.Position = collisionGeometry.Vertices.Address.Offset;
                        collisionGeometry.Vertices.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                        for (var i = 0; i < collisionGeometry.Vertices.Count; i++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<CollisionGeometry.Vertex>(dataContext));
                    }

                    foreach (var moppCode in instance.CollisionMoppCodes)
                    {
                        StreamUtil.Align(dataStream, 0x4);
                        blamResourceStream.Position = moppCode.Data.Address.Offset;
                        moppCode.Data.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                        var moppData = resourceReader.ReadBytes(moppCode.Data.Count).Select(i => new CollisionMoppCode.Datum { Value = i }).ToList();
                        if (BlamCache.Version < CacheVersion.Halo3ODST)
                            moppData = ConvertCollisionMoppData(moppData);
                        resourceWriter.Write(moppData.Select(i => i.Value).ToArray());
                    }

                    StreamUtil.Align(dataStream, 0x4); // 0x4 > 0x10
                    blamResourceStream.Position = instance.Unknown1.Address.Offset;
                    instance.Unknown1.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.Unknown1.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<StructureBspTagResources.InstancedGeometryBlock.Unknown1Block>(dataContext));
                    
                    /*
                    StreamUtil.Align(dataStream, 0x4); // 0x4 > 0x10
                    blamResourceStream.Position = instance.Unknown2.Address.Offset;
                    instance.Unknown2.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.Unknown2.Count; i++)
                    {
                        var element = BlamCache.Deserializer.Deserialize<StructureBspTagResources.InstancedGeometryBlock.Unknown2Block>(dataContext);
                        if (BlamCache.Version <= CacheVersion.Halo3ODST)
                        {
                            element.Unknown1 = element.Unknown1_H3;
                            element.Unknown2 = element.Unknown2_H3;
                        }
                        CacheContext.Serializer.Serialize(dataContext, element);
                    */

                    StreamUtil.Align(dataStream, 0x4); // 0x4 > 0x10
                    blamResourceStream.Position = instance.Unknown3.Address.Offset;
                    instance.Unknown3.Address = new CacheAddress(CacheAddressType.Resource, (int)dataStream.Position);
                    for (var i = 0; i < instance.Unknown3.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<StructureBspTagResources.InstancedGeometryBlock.Unknown3Block>(dataContext));
                }

                dataStream.Position = 0;

                CacheContext.Serializer.Serialize(new ResourceSerializationContext(bsp.CollisionBspResource), resourceDefinition);

                bsp.CollisionBspResource.ChangeLocation(ResourceLocation.ResourcesB);
                var resource = bsp.CollisionBspResource;

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
            
            return bsp.CollisionBspResource;
        }
    }
}