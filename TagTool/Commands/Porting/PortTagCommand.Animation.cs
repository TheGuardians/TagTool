using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public List<ModelAnimationGraph.ResourceGroup> ConvertModelAnimationGraphResourceGroups(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, List<ModelAnimationGraph.ResourceGroup> resourceGroups)
        {
            if (BlamCache.ResourceGestalt == null)
                BlamCache.LoadResourceTags();

            ModelAnimationTagResource resourceDefinition = null;

            foreach (var group in resourceGroups)
            {
                var resourceEntry = BlamCache.ResourceGestalt.TagResources[group.ZoneAssetHandle.Index];

                //
                // Create a new resource for the group
                //

                group.Resource = new PageableResource(TagResourceTypeGen3.Animation)
                {
                    Resource = new TagResourceGen3
                    {
                        DefinitionData = new byte[resourceEntry.FixupInformationLength],
                        DefinitionAddress = resourceEntry.DefinitionAddress,
                    }
                };

                //
                // Copy the definition data to the new group resource
                //

                Array.Copy(
                    BlamCache.ResourceGestalt.FixupInformation,
                    resourceEntry.FixupInformationOffset,
                    group.Resource.Resource.DefinitionData,
                    0,
                    resourceEntry.FixupInformationLength);

                //
                // Convert resource fixups and read the resource definition
                //

                using (var definitionStream = new MemoryStream(group.Resource.Resource.DefinitionData, true))
                using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
                using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.BigEndian))
                {
                    var dataContext = new DataSerializationContext(definitionReader, definitionWriter, CacheAddressType.Definition);

                    foreach (var fixup in resourceEntry.ResourceFixups)
                    {
                        var newFixup = new TagResourceGen3.ResourceFixup
                        {
                            BlockOffset = fixup.BlockOffset,
                            Address = new CacheAddress(CacheAddressType.Resource, fixup.Offset)
                        };

                        definitionStream.Position = newFixup.BlockOffset;
                        definitionWriter.Write(newFixup.Address.Value);

                        group.Resource.Resource.ResourceFixups.Add(newFixup);
                    }

                    // ODST's resource type is 4 when it's supposed to be 2 because the resource definition is in the tag and not as a raw resource
                    definitionStream.Position = group.Resource.Resource.DefinitionAddress.Offset + 0x4;
                    definitionWriter.Write(0x20000000);

                    definitionStream.Position = group.Resource.Resource.DefinitionAddress.Offset;
                    resourceDefinition = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource>(dataContext);
                }

                //
                // Get the resource group real size, which is probably not in the resource definition
                //

                var groupSize = 0;

                foreach (var groupMember in resourceDefinition.GroupMembers)
                {
                    groupSize += groupMember.AnimationData.Size;
                    while (groupSize % 0x10 != 0) // align to 0x10. 
                        groupSize += 4;
                }

                //
                // Attempt to get the resource data
                //

                var resourceData = BlamCache.GetRawFromID(group.ZoneAssetHandle, groupSize);

                if (resourceData == null)
                    return null;

                //
                // Convert the resource data
                //

                using (var blamResourceStream = new MemoryStream(resourceData))
                using (var resourceReader = new EndianReader(blamResourceStream, EndianFormat.BigEndian))
                using (var dataStream = new MemoryStream(new byte[groupSize]))
                using (var resourceWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
                {
                    var dataContext = new DataSerializationContext(resourceReader, resourceWriter);

                    //
                    // Convert each group member of the resource
                    //

                    var memberOffset = 0;

                    for (var memberIndex = 0; memberIndex < resourceDefinition.GroupMembers.Count; memberIndex++)
                    {
                        var member = resourceDefinition.GroupMembers[memberIndex];

                        //
                        // Read the default data
                        //

                        if (member.Sizes.DefaultData > 0)
                        {
                            dataStream.Position = blamResourceStream.Position = member.AnimationData.Address.Offset;

                            // Read the compression codec header
                            var codec = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.CompressionCodecData>(dataContext);
                            CacheContext.Serializer.Serialize(dataContext, codec);

                            // Read the default frame info
                            var defaultData = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.DefaultFrameInfo>(dataContext);
                            CacheContext.Serializer.Serialize(dataContext, defaultData);

                            // Read the default rotation frames
                            for (int i = 0; i < codec.RotationNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrameShort>(dataContext));

                            // Read the default position frames
                            dataStream.Position = blamResourceStream.Position =
                                member.AnimationData.Address.Offset + defaultData.RotationFramesEnd;

                            for (int i = 0; i < codec.PositionNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                            // Read the default scale frames
                            dataStream.Position = blamResourceStream.Position =
                                member.AnimationData.Address.Offset + defaultData.PositionFramesEnd;

                            for (int i = 0; i < codec.ScaleNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                        }

                        //
                        // Read the compressed data
                        //

                        if (member.Sizes.CompressedData > 0)
                        {
                            dataStream.Position = blamResourceStream.Position =
                                (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData;
                            
                            var codec = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.CompressionCodecData>(dataContext);
                            CacheContext.Serializer.Serialize(dataContext, codec);

                            switch (codec.Type)
                            {
                                case ModelAnimationTagResource.CompressionCodecType._8byteQuantizedRotationOnly:
                                    {
                                        var header = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.DefaultFrameInfo>(dataContext);
                                        CacheContext.Serializer.Serialize(dataContext, header);

                                        for (int nodeIndex = 0; nodeIndex < codec.RotationNodeCount; nodeIndex++)
                                        {
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                            {
                                                var rotationFrame = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrameShort>(dataContext);
                                                CacheContext.Serializer.Serialize(dataContext, rotationFrame);
                                            }
                                        }

                                        for (int nodeIndex = 0; nodeIndex < codec.PositionNodeCount; nodeIndex++)
                                        {
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                            {
                                                var positionFrame = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext);
                                                CacheContext.Serializer.Serialize(dataContext, positionFrame);
                                            }
                                        }

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + header.PositionFramesEnd;

                                        for (int nodeIndex = 0; nodeIndex < codec.ScaleNodeCount; nodeIndex++)
                                        {
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                            {
                                                var scaleFrame = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext);
                                                CacheContext.Serializer.Serialize(dataContext, scaleFrame);
                                            }
                                        }

                                        break;
                                    }

                                case ModelAnimationTagResource.CompressionCodecType.ByteKeyframeLightlyQuantized:
                                case ModelAnimationTagResource.CompressionCodecType.WordKeyframeLightlyQuantized:
                                case ModelAnimationTagResource.CompressionCodecType.ReverseByteKeyframeLightlyQuantized:
                                case ModelAnimationTagResource.CompressionCodecType.ReverseWordKeyframeLightlyQuantized:
                                    {
                                        var overlay = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Overlay>(dataContext);
                                        CacheContext.Serializer.Serialize(dataContext, overlay);

                                        #region Description
                                        // Description by DemonicSandwich from http://remnantmods.com/forums/viewtopic.php?f=13&t=1574 (matches my previous observations)

                                        // Format 6 uses Keyframes the way there are supposed to be used. As KEY frames, with the majority of the frames being Tweens.
                                        // 
                                        // This format adds two extra blocks of data to it's structure.
                                        // One block that determines how many Keyframes each Node will have, and an offset to to where it's Markers start from.
                                        // 
                                        // Advantages:
                                        // This format requires far fewer Keyframes to make a complex animation.
                                        // You do not need a keyframe for each render frame.
                                        // Disadvantages:
                                        // It's a bit more complex to work with.
                                        // Since it's Keyrame Markers are only 1 byte in size, you're animation cannot be longer than 256 frames, or ~8.5 seconds for non - machine objects. > 12 bits for gen3, max 0xFFF frames
                                        // Machines are still limited to 256 frames but the frames can be stretched out.
                                        #endregion

                                        //
                                        // Read the rotation frame info
                                        //

                                        var rotationFrameInfo = new List<ModelAnimationTagResource.GroupMember.FrameInfo>();

                                        for (int i = 0; i < codec.RotationNodeCount; i++)
                                        {
                                            var frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);
                                            CacheContext.Serializer.Serialize(dataContext, frameInfo);
                                            rotationFrameInfo.Add(frameInfo);
                                        }

                                        //
                                        // Read the position frame info
                                        //

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.RotationNodesEnd;

                                        var positionFrameInfo = new List<ModelAnimationTagResource.GroupMember.FrameInfo>();

                                        for (int i = 0; i < codec.PositionNodeCount; i++)
                                        {
                                            var frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);
                                            CacheContext.Serializer.Serialize(dataContext, frameInfo);
                                            positionFrameInfo.Add(frameInfo);
                                        }

                                        //
                                        // Read the scale frame info
                                        //

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.PositionNodesEnd;

                                        var scaleFrameInfo = new List<ModelAnimationTagResource.GroupMember.FrameInfo>();

                                        for (int i = 0; i < codec.ScaleNodeCount; i++)
                                        {
                                            var frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);
                                            CacheContext.Serializer.Serialize(dataContext, frameInfo);
                                            scaleFrameInfo.Add(frameInfo);
                                        }

                                        //
                                        // Read the rotation keyframes
                                        //

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.ScaleNodesEnd;

                                        foreach (var frameInfo in rotationFrameInfo)
                                        {
                                            for (int i = 0; i < frameInfo.KeyframeCount; i++)
                                            {
                                                switch (codec.Type)
                                                {
                                                    case ModelAnimationTagResource.CompressionCodecType.ByteKeyframeLightlyQuantized:
                                                    case ModelAnimationTagResource.CompressionCodecType.ReverseByteKeyframeLightlyQuantized:
                                                        CacheContext.Serializer.Serialize(dataContext,
                                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ByteKeyframe>(dataContext));
                                                        break;

                                                    case ModelAnimationTagResource.CompressionCodecType.WordKeyframeLightlyQuantized:
                                                    case ModelAnimationTagResource.CompressionCodecType.ReverseWordKeyframeLightlyQuantized:
                                                        CacheContext.Serializer.Serialize(dataContext,
                                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.WordKeyframe>(dataContext));
                                                        break;
                                                }
                                            }
                                        }

                                        //
                                        // Read the position keyframes
                                        //

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.RotationKeyframesEnd;

                                        foreach (var frameInfo in positionFrameInfo)
                                        {
                                            for (int i = 0; i < frameInfo.KeyframeCount; i++)
                                            {
                                                switch (codec.Type)
                                                {
                                                    case ModelAnimationTagResource.CompressionCodecType.ByteKeyframeLightlyQuantized:
                                                    case ModelAnimationTagResource.CompressionCodecType.ReverseByteKeyframeLightlyQuantized:
                                                        CacheContext.Serializer.Serialize(dataContext,
                                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ByteKeyframe>(dataContext));
                                                        break;

                                                    case ModelAnimationTagResource.CompressionCodecType.WordKeyframeLightlyQuantized:
                                                    case ModelAnimationTagResource.CompressionCodecType.ReverseWordKeyframeLightlyQuantized:
                                                        CacheContext.Serializer.Serialize(dataContext,
                                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.WordKeyframe>(dataContext));
                                                        break;
                                                }
                                            }
                                        }

                                        //
                                        // Read the scale keyframes
                                        //

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.PositionKeyframesEnd;

                                        foreach (var frameInfo in scaleFrameInfo)
                                        {
                                            for (int i = 0; i < frameInfo.KeyframeCount; i++)
                                            {
                                                switch (codec.Type)
                                                {
                                                    case ModelAnimationTagResource.CompressionCodecType.ByteKeyframeLightlyQuantized:
                                                    case ModelAnimationTagResource.CompressionCodecType.ReverseByteKeyframeLightlyQuantized:
                                                        CacheContext.Serializer.Serialize(dataContext,
                                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ByteKeyframe>(dataContext));
                                                        break;

                                                    case ModelAnimationTagResource.CompressionCodecType.WordKeyframeLightlyQuantized:
                                                    case ModelAnimationTagResource.CompressionCodecType.ReverseWordKeyframeLightlyQuantized:
                                                        CacheContext.Serializer.Serialize(dataContext,
                                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.WordKeyframe>(dataContext));
                                                        break;
                                                }
                                            }
                                        }

                                        //
                                        // Read the rotation frames data
                                        //

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.ScaleKeyframesEnd;

                                        foreach (var frameInfo in rotationFrameInfo)
                                        {
                                            for (int i = 0; i < frameInfo.KeyframeCount; i++)
                                                CacheContext.Serializer.Serialize(dataContext,
                                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrameShort>(dataContext));
                                        }

                                        //
                                        // Read the position frames data
                                        //

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.RotationFramesEnd;

                                        foreach (var frameInfo in positionFrameInfo)
                                        {
                                            for (int i = 0; i < frameInfo.KeyframeCount; i++)
                                                CacheContext.Serializer.Serialize(dataContext,
                                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));
                                        }

                                        //
                                        // Read the scale frames data
                                        //

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.PositionFramesEnd;

                                        foreach (var frameInfo in scaleFrameInfo)
                                        {
                                            for (int i = 0; i < frameInfo.KeyframeCount; i++)
                                                CacheContext.Serializer.Serialize(dataContext,
                                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                                        }

                                        break;
                                    }

                                case ModelAnimationTagResource.CompressionCodecType.BlendScreenCodec:
                                    {
                                        var blendScreen = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.BlendScreenData>(dataContext);
                                        CacheContext.Serializer.Serialize(dataContext, blendScreen);

                                        //
                                        // Read the rotation frames
                                        //

                                        for (int nodeIndex = 0; nodeIndex < codec.RotationNodeCount; nodeIndex++)
                                        {
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                            {
                                                var rotationFrame = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext);
                                                CacheContext.Serializer.Serialize(dataContext, rotationFrame);
                                            }
                                        }

                                        //
                                        // Read the position frames
                                        //

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + blendScreen.RotationFramesEnd;

                                        for (int nodeIndex = 0; nodeIndex < codec.PositionNodeCount; nodeIndex++)
                                        {
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                            {
                                                var positionFrame = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext);
                                                CacheContext.Serializer.Serialize(dataContext, positionFrame);
                                            }
                                        }

                                        //
                                        // Read the scale frames
                                        //

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + blendScreen.PositionFramesEnd;

                                        for (int nodeIndex = 0; nodeIndex < codec.ScaleNodeCount; nodeIndex++)
                                        {
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                            {
                                                var scaleFrame = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext);
                                                CacheContext.Serializer.Serialize(dataContext, scaleFrame);
                                            }
                                        }

                                        break;
                                    }

                                default:
                                    throw new NotSupportedException(codec.Type.ToString());
                            }
                        }

                        //
                        // Read the node flags
                        //

                        #region Static/Animated Node Flags
                        // DemonicSandwich - http://remnantmods.com/forums/viewtopic.php?f=13&t=1574
                        //
                        // Just a block of flags. Tick a flag and the respective node will be affected by animation.
                        // The size of this block should always be a multiple of 12. It's size is determined my the meta value Node List Size [byte, offset: 61] 
                        // When set to 12, the list can handle objects with a node count up to 32 (0-31).
                        // When set to 24, the object can have 64 nodes and so on.
                        // The block is split into 3 groups of flags.
                        // The first group determines what nodes are affected by rotation, the second group for position, and the third group for scale.
                        // 
                        // If looking at it in hex, the Node ticks for each group will be in order as follows:
                        // [7][6][5][4][3][2][1][0] - [15][14][13][12][11][10][9][8] - etc.
                        // Each flag corresponding to a Node index.
                        //
                        // There's one bitfield32 for every 32 nodes that are animated which i'll call a node flags. 
                        // There's at least 3 flags if the animation only has an overlay header, which i'll call a flag set.
                        // There's at least 6 flags if the animation has both a base header and an overlay header, so 2 sets.
                        // If the animated nodes count is over 32, then a new flags set is added.
                        // 1 set per header is added, such as 32 nodes = 1 set, 64 = 2 sets, 96 = 3 sets etc , 128-256 maybe max
                        #endregion

                        dataStream.Position = blamResourceStream.Position =
                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + member.Sizes.CompressedData;

                        if (member.Sizes.StaticNodeFlags > 0)
                        {
                            switch (member.Sizes.StaticNodeFlags)
                            {
                                case 0xC:
                                    var footer32 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.NodeFlags32>(dataContext);
                                    CacheContext.Serializer.Serialize(dataContext, footer32);
                                    break;

                                case 0x18:
                                    var footer64 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.NodeFlags64>(dataContext);
                                    CacheContext.Serializer.Serialize(dataContext, footer64);
                                    break;

                                default:
                                    throw new NotSupportedException($"{nameof(member.Sizes.StaticNodeFlags)} == {member.Sizes.StaticNodeFlags}");
                            }
                        }

                        if (member.Sizes.AnimatedNodeFlags > 0)
                        {
                            switch (member.Sizes.AnimatedNodeFlags)
                            {
                                case 0xC:
                                    var footer32 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.NodeFlags32>(dataContext);
                                    CacheContext.Serializer.Serialize(dataContext, footer32);
                                    break;

                                case 0x18:
                                    var footer64 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.NodeFlags64>(dataContext);
                                    CacheContext.Serializer.Serialize(dataContext, footer64);
                                    break;

                                default:
                                    throw new NotSupportedException($"{nameof(member.Sizes.StaticNodeFlags)} == {member.Sizes.StaticNodeFlags}");
                            }
                        }

                        //
                        // Read the movement data
                        //

                        if (member.Sizes.MovementData > 0)
                        {
                            for (int i = 0; i < member.FrameCount; i++)
                            {
                                switch (member.MovementDataType)
                                {
                                    case ModelAnimationTagResource.GroupMemberMovementDataType.dyaw:
                                        CacheContext.Serializer.Serialize(dataContext,
                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDyaw>(dataContext));
                                        break;

                                    case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy:
                                        CacheContext.Serializer.Serialize(dataContext,
                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDy>(dataContext));
                                        break;

                                    case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dyaw:
                                        CacheContext.Serializer.Serialize(dataContext,
                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                        break;

                                    case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dz_dyaw:
                                        CacheContext.Serializer.Serialize(dataContext,
                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDzDyaw>(dataContext));
                                        break;

                                    default:
                                        throw new NotSupportedException(member.MovementDataType.ToString());
                                }
                            }
                        }

                        //
                        // Read the pill offset data
                        //

                        if (member.Sizes.PillOffsetData > 0)
                        {
                            for (int i = 0; i < member.FrameCount; i++)
                            {
                                switch (member.MovementDataType)
                                {
                                    case ModelAnimationTagResource.GroupMemberMovementDataType.dyaw:
                                    case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy:
                                    case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dyaw:
                                        CacheContext.Serializer.Serialize(dataContext,
                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                        break;

                                    case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dz_dyaw:
                                        CacheContext.Serializer.Serialize(dataContext,
                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDzDyaw>(dataContext));
                                        break;

                                    default:
                                        throw new NotSupportedException(member.MovementDataType.ToString());
                                }
                            }
                        }

                        //
                        // TODO: fix this shit
                        //
                        {
                            dataStream.Position = memberOffset + member.AnimationData.Size;

                            // Before the next animation member, there's some padding that is garbage data in H3/ODST, but zeroed in HO.
                            // In order to compare converted to original raw easily, copy the original data.
                            while (blamResourceStream.Position % 0x10 != 0) // align to 0x10, useless padding of garbage data, it's zeroed in 1:1 HO raw, just read as 4 lame bytes
                            {
                                if (blamResourceStream.Position == blamResourceStream.Length)
                                    break;

                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                            }

                            var bullshitCount = dataStream.Position - (memberOffset + member.AnimationData.Size);

                            // Align the next animation member to 0x10. 
                            memberOffset += member.AnimationData.Size;
                            bullshitCount = memberOffset;
                            while (memberOffset % 0x10 != 0)
                                memberOffset += 4;
                            bullshitCount = memberOffset - bullshitCount;
                        }
                    }

                    //
                    // Cache the newly-converted resource
                    //

                    dataStream.Position = 0;

                    group.Resource.ChangeLocation(ResourceLocation.ResourcesB);

                    CacheContext.Serialize(group.Resource, resourceDefinition);
                    CacheContext.AddResource(group.Resource, dataStream);
                }
            }

            return resourceGroups;
        }

        public ModelAnimationGraph ConvertModelAnimationGraph(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, ModelAnimationGraph definition)
        {
            if (BlamCache.ResourceGestalt == null)
                BlamCache.LoadResourceTags();

            definition.ResourceGroups = ConvertModelAnimationGraphResourceGroups(cacheStream, resourceStreams, definition.ResourceGroups);
            definition.Modes = definition.Modes.OrderBy(a => a.Name.Set).ThenBy(a => a.Name.Index).ToList();

            foreach (var mode in definition.Modes)
            {
                mode.WeaponClass = mode.WeaponClass.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();

                foreach (var weaponClass in mode.WeaponClass)
                {
                    weaponClass.WeaponType = weaponClass.WeaponType.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();

                    foreach (var weaponType in weaponClass.WeaponType)
                    {
                        weaponType.Actions = weaponType.Actions.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();
                        weaponType.Overlays = weaponType.Overlays.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();
                        weaponType.DeathAndDamage = weaponType.DeathAndDamage.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();
                        weaponType.Transitions = weaponType.Transitions.OrderBy(a => a.FullName.Set).ThenBy(a => a.FullName.Index).ToList();

                        foreach (var transition in weaponType.Transitions)
                            transition.Destinations = transition.Destinations.OrderBy(a => a.FullName.Set).ThenBy(a => a.FullName.Index).ToList();
                    }
                }
            }

            return definition;
        }
    }
}