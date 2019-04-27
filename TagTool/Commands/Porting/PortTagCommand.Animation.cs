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

                group.Resource = new PageableResource(TagResourceTypeGen3.Animation)
                {
                    Resource = new TagResourceGen3
                    {
                        DefinitionData = BlamCache.ResourceGestalt.FixupInformation.Skip(resourceEntry.FixupInformationOffset).Take(resourceEntry.FixupInformationLength).ToArray(),
                        DefinitionAddress = resourceEntry.DefinitionAddress,
                    }
                };

                // Convert blam fixups
                using (var definitionStream = new MemoryStream(group.Resource.Resource.DefinitionData, true))
                using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
                using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.BigEndian))
                {
                    foreach (var fixup in resourceEntry.ResourceFixups)
                    {
                        var newFixup = new TagResourceGen3.ResourceFixup
                        {
                            BlockOffset = (uint)fixup.BlockOffset,
                            Address = new CacheAddress(CacheAddressType.Resource, fixup.Offset)
                        };

                        definitionStream.Position = newFixup.BlockOffset;
                        definitionWriter.Write(newFixup.Address.Value);

                        group.Resource.Resource.ResourceFixups.Add(newFixup);
                    }

                    var dataContext = new DataSerializationContext(definitionReader, definitionWriter, CacheAddressType.Definition);

                    definitionStream.Position = group.Resource.Resource.DefinitionAddress.Offset + 0x4;
                    definitionWriter.Write(0x20000000);
                    // ODST's resource type is 4 when it's supposed to be 2 because the resource definition is in the tag and not as a raw resource

                    definitionStream.Position = group.Resource.Resource.DefinitionAddress.Offset;

                    resourceDefinition = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource>(dataContext);
                }

                // Get the resource group real size, which is probably not in the resource definition
                var groupSize = 0;
                foreach (var groupMember in resourceDefinition.GroupMembers)
                {
                    groupSize += groupMember.AnimationData.Size;
                    while (groupSize % 0x10 != 0) // align to 0x10. 
                        groupSize += 4;
                }

                var resourceData = BlamCache.GetRawFromID(group.ZoneAssetHandle, groupSize);

                if (resourceData == null)
                    return null;

                using (var blamResourceStream = new MemoryStream(resourceData))
                using (var resourceReader = new EndianReader(blamResourceStream, EndianFormat.BigEndian))
                using (var dataStream = new MemoryStream(new byte[groupSize]))
                using (var resourceWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
                {
                    var dataContext = new DataSerializationContext(resourceReader, resourceWriter);
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
                            var codec = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.CompressionCodecData>(dataContext);
                            CacheContext.Serializer.Serialize(dataContext, codec);

                            // Read the default frame info
                            var defaultData = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.DefaultFrameInfo>(dataContext);
                            CacheContext.Serializer.Serialize(dataContext, defaultData);

                            // Read the default rotation frames
                            for (int i = 0; i < codec.RotationNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                            // Read the default position frames
                            dataStream.Position = blamResourceStream.Position =
                                member.AnimationData.Address.Offset + defaultData.PositionFramesOffset;

                            for (int i = 0; i < codec.PositionNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                            // Read the default scale frames
                            dataStream.Position = blamResourceStream.Position =
                                member.AnimationData.Address.Offset + defaultData.ScaleFramesOffset;

                            for (int i = 0; i < codec.ScaleNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                        }

                        //
                        // Read the compressed data
                        //

                        if (member.Sizes.CompressedData > 0)
                        {
                            // If the overlay header is alone, member.Sizes.DefaultData = 0
                            dataStream.Position = blamResourceStream.Position =
                                (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData;
                            
                            var codec = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.CompressionCodecData>(dataContext);
                            CacheContext.Serializer.Serialize(dataContext, codec);

                            // deserialize second header. or as first header if the type1/format1 header isn't used.
                            switch (codec.Type)
                            {
                                case ModelAnimationTagResource.CompressionCodecType._8byteQuantizedRotationOnly:
                                    {
                                        var header = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.DefaultFrameInfo>(dataContext);
                                        CacheContext.Serializer.Serialize(dataContext, header);

                                        for (int nodeIndex = 0; nodeIndex < codec.RotationNodeCount; nodeIndex++)
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                                CacheContext.Serializer.Serialize(dataContext,
                                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + header.PositionFramesOffset;

                                        for (int nodeIndex = 0; nodeIndex < codec.PositionNodeCount; nodeIndex++)
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                                CacheContext.Serializer.Serialize(dataContext,
                                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                        dataStream.Position = blamResourceStream.Position =
                                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + header.ScaleFramesOffset;

                                        for (int nodeIndex = 0; nodeIndex < codec.ScaleNodeCount; nodeIndex++)
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                                CacheContext.Serializer.Serialize(dataContext,
                                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));

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

                                        var RotationFrameCount = new List<uint>();
                                        var PositionFrameCount = new List<uint>();
                                        var ScaleFrameCount = new List<uint>();

                                        for (int i = 0; i < codec.RotationNodeCount; i++)
                                        {
                                            var frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);

                                            CacheContext.Serializer.Serialize(dataContext, frameInfo);

                                            var keyframesOffset = frameInfo.FrameCount & 0x00FFF000; // unused in this conversion
                                            var keyframes = frameInfo.FrameCount & 0x00000FFF;
                                            RotationFrameCount.Add(keyframes);
                                        }

                                        for (int i = 0; i < codec.PositionNodeCount; i++)
                                        {
                                            var frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);

                                            CacheContext.Serializer.Serialize(dataContext, frameInfo);

                                            var keyframesOffset = frameInfo.FrameCount & 0x00FFF000;
                                            var keyframes = frameInfo.FrameCount & 0x00000FFF;
                                            PositionFrameCount.Add(keyframes);
                                        }

                                        for (int i = 0; i < codec.ScaleNodeCount; i++)
                                        {
                                            var frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);

                                            CacheContext.Serializer.Serialize(dataContext, frameInfo);

                                            var keyframesOffset = frameInfo.FrameCount & 0x00FFF000;
                                            var keyframes = frameInfo.FrameCount & 0x00000FFF;
                                            ScaleFrameCount.Add(keyframes);
                                        }

                                        blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.RotationKeyframesOffset;
                                        dataStream.Position = blamResourceStream.Position;
                                        foreach (var framecount in RotationFrameCount)
                                            for (int i = 0; i < framecount; i++)
                                                if (codec.Type == ModelAnimationTagResource.CompressionCodecType.ByteKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ByteKeyframe>(dataContext));
                                                else if (codec.Type == ModelAnimationTagResource.CompressionCodecType.WordKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.WordKeyframe>(dataContext));
                                                else if (codec.Type == ModelAnimationTagResource.CompressionCodecType.ReverseByteKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ByteKeyframe>(dataContext));
                                                else if (codec.Type == ModelAnimationTagResource.CompressionCodecType.ReverseWordKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.WordKeyframe>(dataContext));

                                        blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.PositionKeyframesOffset;
                                        dataStream.Position = blamResourceStream.Position;
                                        foreach (var framecount in PositionFrameCount)
                                            for (int i = 0; i < framecount; i++)
                                                if (codec.Type == ModelAnimationTagResource.CompressionCodecType.ByteKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ByteKeyframe>(dataContext));
                                                else if (codec.Type == ModelAnimationTagResource.CompressionCodecType.WordKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.WordKeyframe>(dataContext));
                                                else if (codec.Type == ModelAnimationTagResource.CompressionCodecType.ReverseByteKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ByteKeyframe>(dataContext));
                                                else if (codec.Type == ModelAnimationTagResource.CompressionCodecType.ReverseWordKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.WordKeyframe>(dataContext));

                                        blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.ScaleKeyframesOffset;
                                        dataStream.Position = blamResourceStream.Position;
                                        foreach (var framecount in ScaleFrameCount)
                                            for (int i = 0; i < framecount; i++)
                                                if (codec.Type == ModelAnimationTagResource.CompressionCodecType.ByteKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ByteKeyframe>(dataContext));
                                                else if (codec.Type == ModelAnimationTagResource.CompressionCodecType.WordKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.WordKeyframe>(dataContext));
                                                else if (codec.Type == ModelAnimationTagResource.CompressionCodecType.ReverseByteKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ByteKeyframe>(dataContext));
                                                else if (codec.Type == ModelAnimationTagResource.CompressionCodecType.ReverseWordKeyframeLightlyQuantized)
                                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.WordKeyframe>(dataContext));

                                        blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.RotationFramesOffset;
                                        dataStream.Position = blamResourceStream.Position;
                                        foreach (var framecount in RotationFrameCount)
                                            for (int i = 0; i < framecount; i++)
                                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                                        blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.PositionFramesOffset;
                                        dataStream.Position = blamResourceStream.Position;
                                        foreach (var framecount in PositionFrameCount)
                                            for (int i = 0; i < framecount; i++)
                                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                        blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + overlay.ScaleFramesOffset;
                                        dataStream.Position = blamResourceStream.Position;
                                        foreach (var framecount in ScaleFrameCount)
                                            for (int i = 0; i < framecount; i++)
                                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                                        break;
                                    }

                                case ModelAnimationTagResource.CompressionCodecType.BlendScreenCodec:
                                    {
                                        var blendScreen = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.BlendScreenData>(dataContext);
                                        CacheContext.Serializer.Serialize(dataContext, blendScreen);

                                        for (int nodeIndex = 0; nodeIndex < codec.RotationNodeCount; nodeIndex++)
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                                CacheContext.Serializer.Serialize(dataContext,
                                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrameFloat>(dataContext));

                                        blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + blendScreen.PositionFramesOffset;
                                        dataStream.Position = blamResourceStream.Position;
                                        for (int nodeIndex = 0; nodeIndex < codec.PositionNodeCount; nodeIndex++)
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                                CacheContext.Serializer.Serialize(dataContext,
                                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                        blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + blendScreen.ScaleFramesOffset;
                                        dataStream.Position = blamResourceStream.Position;
                                        for (int nodeIndex = 0; nodeIndex < codec.ScaleNodeCount; nodeIndex++)
                                            for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                                CacheContext.Serializer.Serialize(dataContext,
                                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));

                                        break;
                                    }

                                default:
                                    throw new DataMisalignedException();
                            }
                        }

                        //
                        // Read the node flags
                        //

                        dataStream.Position = blamResourceStream.Position =
                            (long)member.AnimationData.Address.Offset + member.Sizes.DefaultData + member.Sizes.CompressedData;

                        if (member.Sizes.StaticNodeFlags > 0)
                        {
                            //
                            // TODO: fix this shit
                            //

                            var staticNodeFlagsCount = member.Sizes.StaticNodeFlags /
                                typeof(ModelAnimationTagResource.GroupMember.StaticNodeFlagsData).GetSize(BlamCache.Version);

                            for (int i = 0; i < staticNodeFlagsCount; i++)
                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.StaticNodeFlagsData>(dataContext));
                        }

                        if (member.Sizes.AnimatedNodeFlags > 0)
                        {
                            var animatedNodeFlagsCount = member.Sizes.AnimatedNodeFlags /
                                typeof(ModelAnimationTagResource.GroupMember.AnimatedNodeFlagsData).GetSize(BlamCache.Version);

                            for (int i = 0; i < animatedNodeFlagsCount; i++)
                                CacheContext.Serializer.Serialize(dataContext,
                                    BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.AnimatedNodeFlagsData>(dataContext));
                        }

                        //
                        // Read the movement data
                        //

                        switch (member.MovementDataType)
                        {
                            case ModelAnimationTagResource.GroupMemberMovementDataType.dyaw:
                                if (member.Sizes.MovementData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext,
                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDyaw>(dataContext));
                                if (member.Sizes.PillOffsetData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext,
                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy:
                                if (member.Sizes.MovementData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDy>(dataContext));
                                if (member.Sizes.PillOffsetData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dyaw:
                                if (member.Sizes.MovementData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                if (member.Sizes.PillOffsetData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dz_dyaw:
                                if (member.Sizes.MovementData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDzDyaw>(dataContext));
                                if (member.Sizes.PillOffsetData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDzDyaw>(dataContext));
                                break;

                            default:
                                throw new NotSupportedException(member.MovementDataType.ToString());
                        }

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

                        // Align the next animation member to 0x10. 
                        memberOffset += member.AnimationData.Size;
                        while (memberOffset % 0x10 != 0)
                            memberOffset += 4;
                    }

                    dataStream.Position = 0;

                    CacheContext.Serializer.Serialize(new ResourceSerializationContext(CacheContext, group.Resource), resourceDefinition);

                    group.Resource.ChangeLocation(ResourceLocation.ResourcesB);
                    var resource = group.Resource;

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