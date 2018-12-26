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

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public List<ModelAnimationGraph.ResourceGroup> ConvertModelAnimationGraphResourceGroups(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, List<ModelAnimationGraph.ResourceGroup> resourceGroups)
        {
            if (BlamCache.ResourceGestalt == null)
                BlamCache.LoadResourceTags();

            var resourceDefinition = new List<ModelAnimationTagResource>();

            foreach (var group in resourceGroups)
            {
                var resourceEntry = BlamCache.ResourceGestalt.TagResources[group.ZoneAssetDatumIndex & ushort.MaxValue];

                group.Resource = new PageableResource
                {
                    Page = new RawPage
                    {
                        Index = -1,
                    },
                    Resource = new TagResourceGen3
                    {
                        ResourceType = TagResourceTypeGen3.Animation,
                        DefinitionData = BlamCache.ResourceGestalt.FixupInformation.Skip(resourceEntry.FixupInformationOffset).Take(resourceEntry.FixupInformationLength).ToArray(),
                        DefinitionAddress = resourceEntry.DefinitionAddress,
                        ResourceFixups = new List<TagResourceGen3.ResourceFixup>(),
                        ResourceDefinitionFixups = new List<TagResourceGen3.ResourceDefinitionFixup>(),
                        Unknown2 = 1
                    }
                };

                // Convert blam fixups

                // get the list of members in this resourcegroup. this list contains address, various offsets, and other info about the member.
                if (group.Resource.Resource.DefinitionData.Length != 0)
                {
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

                        resourceDefinition.Add(BlamCache.Deserializer.Deserialize<ModelAnimationTagResource>(dataContext));
                    }
                }
            }

            var diffLines = new List<string>();
            var resDefIndex = -1;

            foreach (var group in resourceGroups)
            {
                resDefIndex++;

                if (resourceDefinition.Count < resDefIndex + 1)
                    continue; // rare cases, might break the game

                // Get the resource group real size, which is probably not in the resource definition
                var groupSize = 0;
                foreach (var groupMember in resourceDefinition[resDefIndex].GroupMembers)
                {
                    groupSize += groupMember.AnimationData.Size;
                    while (groupSize % 0x10 != 0) // align to 0x10. 
                        groupSize += 4;
                }

                var resourceData = BlamCache.GetRawFromID(group.ZoneAssetDatumIndex, groupSize);

                if (resourceData == null)
                    return null;

                using (var blamResourceStream = new MemoryStream(resourceData))
                using (var resourceReader = new EndianReader(blamResourceStream, EndianFormat.BigEndian))
                using (var dataStream = new MemoryStream(new byte[groupSize]))
                using (var resourceWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
                {
                    var dataContext = new DataSerializationContext(resourceReader, resourceWriter);

                    var memberIndex = -1;
                    var memberOffset = 0;
                    foreach (var member in resourceDefinition[resDefIndex].GroupMembers)
                    {
                        memberIndex++;

                        ModelAnimationTagResource.GroupMember.Codec codec;
                        ModelAnimationTagResource.GroupMember.FrameInfo frameInfo;

                        if ((byte)member.BaseHeader != 0)
                        {
                            blamResourceStream.Position = member.AnimationData.Address.Offset;
                            dataStream.Position = member.AnimationData.Address.Offset;

                            codec = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Codec>(dataContext);

                            CacheContext.Serializer.Serialize(dataContext, codec);

                            var Format1 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Format1>(dataContext);

                            CacheContext.Serializer.Serialize(dataContext, Format1);

                            // blamResourceStream.Position = (long)member.AnimationData.Address.Offset + headerSize;
                            // edResourceStream.Position = blamResourceStream.Position;
                            for (int i = 0; i < codec.RotationNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                            blamResourceStream.Position = member.AnimationData.Address.Offset + Format1.DataStart;
                            dataStream.Position = blamResourceStream.Position;
                            for (int i = 0; i < codec.PositionNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                            blamResourceStream.Position = member.AnimationData.Address.Offset + Format1.ScaleFramesOffset;
                            dataStream.Position = blamResourceStream.Position;
                            for (int i = 0; i < codec.ScaleNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));

                        }

                        // If the overlay header is alone, member.OverlayOffset = 0
                        blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset;
                        dataStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset;

                        codec = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Codec>(dataContext);
                        CacheContext.Serializer.Serialize(dataContext, codec);

                        // deserialize second header. or as first header if the type1/format1 header isn't used.
                        switch (codec.AnimationCodec)
                        {
                            case ModelAnimationTagResource.AnimationCompressionFormats.Type3: // should merge with type1
                                var header = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Format1>(dataContext);

                                CacheContext.Serializer.Serialize(dataContext, header);

                                for (int nodeIndex = 0; nodeIndex < codec.RotationNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + header.DataStart;
                                dataStream.Position = blamResourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.PositionNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + header.ScaleFramesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.ScaleNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));

                                break;

                            case ModelAnimationTagResource.AnimationCompressionFormats.Type4:
                            case ModelAnimationTagResource.AnimationCompressionFormats.Type5:
                            case ModelAnimationTagResource.AnimationCompressionFormats.Type6:
                            case ModelAnimationTagResource.AnimationCompressionFormats.Type7:
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
                                    frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);

                                    CacheContext.Serializer.Serialize(dataContext, frameInfo);

                                    var keyframesOffset = frameInfo.FrameCount & 0x00FFF000; // unused in this conversion
                                    var keyframes = frameInfo.FrameCount & 0x00000FFF;
                                    RotationFrameCount.Add(keyframes);
                                }

                                for (int i = 0; i < codec.PositionNodeCount; i++)
                                {
                                    frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);

                                    CacheContext.Serializer.Serialize(dataContext, frameInfo);

                                    var keyframesOffset = frameInfo.FrameCount & 0x00FFF000;
                                    var keyframes = frameInfo.FrameCount & 0x00000FFF;
                                    PositionFrameCount.Add(keyframes);
                                }

                                for (int i = 0; i < codec.ScaleNodeCount; i++)
                                {
                                    frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);

                                    CacheContext.Serializer.Serialize(dataContext, frameInfo);

                                    var keyframesOffset = frameInfo.FrameCount & 0x00FFF000;
                                    var keyframes = frameInfo.FrameCount & 0x00000FFF;
                                    ScaleFrameCount.Add(keyframes);
                                }

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.RotationKeyframesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                foreach (var framecount in RotationFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type4)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Keyframe>(dataContext));
                                        else if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type5)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.KeyframeType5>(dataContext));
                                        else if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type6)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Keyframe>(dataContext));
                                        else if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type7)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.KeyframeType5>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.PositionKeyframesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                foreach (var framecount in PositionFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type4)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Keyframe>(dataContext));
                                        else if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type5)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.KeyframeType5>(dataContext));
                                        else if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type6)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Keyframe>(dataContext));
                                        else if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type7)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.KeyframeType5>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.ScaleKeyframesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                foreach (var framecount in ScaleFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type4)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Keyframe>(dataContext));
                                        else if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type5)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.KeyframeType5>(dataContext));
                                        else if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type6)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Keyframe>(dataContext));
                                        else if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type7)
                                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.KeyframeType5>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.RotationFramesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                foreach (var framecount in RotationFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.PositionFramesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                foreach (var framecount in PositionFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.ScaleFramesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                foreach (var framecount in ScaleFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                                break;

                            case ModelAnimationTagResource.AnimationCompressionFormats.Type8:
                                // Type 8 is basically a type 3 but with rotation frames using 4 floats, or a realQuaternion
                                var Format8 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Format8>(dataContext);

                                CacheContext.Serializer.Serialize(dataContext, Format8);

                                for (int nodeIndex = 0; nodeIndex < codec.RotationNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrameFloat>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + Format8.PositionFramesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.PositionNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + Format8.ScaleFramesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.ScaleNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));

                                break;

                            default:
                                throw new DataMisalignedException();
                        }

                        #region How Footer/Flags works
                        // Better description by DemonicSandwich from http://remnantmods.com/forums/viewtopic.php?f=13&t=1574 : Node List Block: (matches my previous observations)
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
                        #endregion

                        #region Footer/Flag block
                        // There's one bitfield32 for every 32 nodes that are animated which i'll call a node flags. 
                        // There's at least 3 flags if the animation only has an overlay header, which i'll call a flag set.
                        // There's at least 6 flags if the animation has both a base header and an overlay header, so 2 sets.
                        // If the animated nodes count is over 32, then a new flags set is added.
                        // 1 set per header is added, such as 32 nodes = 1 set, 64 = 2 sets, 96 = 3 sets etc , 128-256 maybe max

                        blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + member.FlagsOffset;
                        dataStream.Position = blamResourceStream.Position;

                        var footerSizeBase = (byte)member.BaseHeader / 4;
                        for (int flagsCount = 0; flagsCount < footerSizeBase; flagsCount++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));

                        var footerSizeOverlay = (byte)member.OverlayHeader / 4;
                        for (int flagsCount = 0; flagsCount < footerSizeOverlay; flagsCount++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                        #endregion

                        switch (member.MovementDataType)
                        {
                            case ModelAnimationTagResource.GroupMemberMovementDataType.None:
                                if (member.Unknown1 > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDyaw>(dataContext));
                                if (member.Unknown2 > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy:
                                if (member.Unknown1 > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDy>(dataContext));
                                if (member.Unknown2 > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dyaw:
                                if (member.Unknown1 > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                if (member.Unknown2 > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dz_dyaw:
                                if (member.Unknown1 > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDzDyaw>(dataContext));
                                if (member.Unknown2 > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDzDyaw>(dataContext));
                                break;
                            default:
                                break;
                        }

                        dataStream.Position = memberOffset + member.AnimationData.Size;

                        // Before the next animation member, there's some padding that is garbage data in H3/ODST, but zeroed in HO.
                        // In order to compare converted to original raw easily, copy the original data.
                        while (blamResourceStream.Position % 0x10 != 0) // align to 0x10, useless padding of garbage data, it's zeroed in 1:1 HO raw, just read as 4 lame bytes
                        {
                            if (blamResourceStream.Position == blamResourceStream.Length)
                                break;

                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                        }

                        // Align the next animation member to 0x10. 
                        memberOffset += member.AnimationData.Size;
                        while (memberOffset % 0x10 != 0)
                            memberOffset += 4;
                    }

                    dataStream.Position = 0;

                    CacheContext.Serializer.Serialize(new ResourceSerializationContext(group.Resource), resourceDefinition[resDefIndex]);

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
            definition.Modes = definition.Modes.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();

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

        private List<string> CompareData(MemoryStream bmData_, MemoryStream edData_, long start, long length, List<string> diffLines)
        {
            if (start > edData_.Length || start > bmData_.Length)
                throw new Exception();

            var bmData = bmData_.ToArray();
            var edData = edData_.ToArray();

            for (long i = start; i < start + length; i = i + 4)
            {
                // var bmBytes = new byte[4] { bmData[i + 3], bmData[i + 2], bmData[i + 1], bmData[i + 0] };
                var bmBytes = new byte[4] { bmData[i + 0], bmData[i + 1], bmData[i + 2], bmData[i + 3] };
                var edBytes = new byte[4] { edData[i + 0], edData[i + 1], edData[i + 2], edData[i + 3] };

                var bmInt = BitConverter.ToUInt32(bmBytes, 0);
                var edInt = BitConverter.ToUInt32(edBytes, 0);

                // Console.WriteLine($"{i:X8},{bmInt:X8}");

                var template = $"" +
                    $"{bmInt:X8}," +
                    $"{edInt:X8}," +
                    $"{i:X8}," +
                    $"";

                if (bmInt == edInt) // check for bytes
                    continue;

                if (bmBytes[0] == edBytes[1] && // check for shorts
                    bmBytes[1] == edBytes[0] &&
                    bmBytes[2] == edBytes[3] &&
                    bmBytes[3] == edBytes[2])
                    continue;

                var edIntFlip = BitConverter.ToInt32(edBytes.Reverse().ToArray(), 0);
                if ((uint)bmInt == (uint)edIntFlip) // check for int
                    continue;

                if (bmInt != edInt) // if it's not bytes, or shorts, or int, then the value is completely different and the conversion failed
                {
                    // diffLines.Add($"{template}different");
                    // continue;
                    throw new Exception();
                }
            }

            return diffLines;

        }

        private HashSet<string> MergedAnimationGraphs { get; } = new HashSet<string>();
        private Dictionary<string, (Dictionary<string, (short, short)>, Dictionary<short, short>)> MergedAnimationData { get; }

        private void MergeAnimationTagReferences(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, List<ModelAnimationGraph.AnimationTagReference> edReferences, List<ModelAnimationGraph.AnimationTagReference> h3References)
        {
            for (var i = 0; i < edReferences.Count; i++)
            {
                var edReference = edReferences[i];
                var h3Reference = h3References[i];

                if (edReference.Reference != null || h3Reference.Reference == null)
                    continue;

                var h3Tag = BlamCache.GetIndexItemFromID(h3Reference.Reference.Index);
                edReference.Reference = ConvertTag(cacheStream, resourceStreams, h3Tag);
            }
        }

        private Dictionary<string, (short, short)> MergeAnimations(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CacheFile.IndexItem h3Tag, ModelAnimationGraph h3Def, List<ModelAnimationGraph.Animation> edAnimations)
        {
            var result = new Dictionary<string, (short, short)>(); // (h3Index, edIndex)

            foreach (var h3Animation in h3Def.Animations)
            {
                var animationName = BlamCache.Strings.GetString(h3Animation.Name);

                if (result.ContainsKey(animationName))
                    continue;

                var edAnimation = edAnimations.Find(a => animationName == CacheContext.GetString(a.Name));
                var edIndex = (short)(edAnimation != null ? edAnimations.IndexOf(edAnimation) : edAnimations.Count);

                result[animationName] = ((short)(edAnimation != null ? -1 : h3Def.Animations.IndexOf(h3Animation)), edIndex);

                if (edAnimation == null)
                {
                    edAnimation = (ModelAnimationGraph.Animation)ConvertData(cacheStream, resourceStreams, h3Animation.DeepClone(), h3Def, h3Tag.Name);
                    edAnimations.Add(edAnimation);
                }
            }

            foreach (var entry in result)
            {
                if (entry.Value.Item1 == -1)
                    continue;

                var edAnimation = edAnimations[entry.Value.Item2];

                if (edAnimation.PreviousVariantSiblingNew != -1)
                    edAnimation.PreviousVariantSiblingNew = result[BlamCache.Strings.GetString(h3Def.Animations[edAnimation.PreviousVariantSiblingNew].Name)].Item2;

                if (edAnimation.NextVariantSiblingNew != -1)
                    edAnimation.NextVariantSiblingNew = result[BlamCache.Strings.GetString(h3Def.Animations[edAnimation.NextVariantSiblingNew].Name)].Item2;
            }

            return result;
        }

        private List<ModelAnimationGraph.Mode> MergeModes(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CacheFile.IndexItem h3Tag, ModelAnimationGraph h3Def, List<ModelAnimationGraph.Mode> edModes, Dictionary<string, (short, short)> indices)
        {
            foreach (var h3Mode in h3Def.Modes)
            {
                var modeLabel = BlamCache.Strings.GetString(h3Mode.Label);
                var edMode = edModes.Find(m => modeLabel == CacheContext.GetString(m.Label));
                var edModeCreated = false;

                if (edMode == null)
                {
                    edMode = (ModelAnimationGraph.Mode)ConvertData(
                        cacheStream, resourceStreams, h3Mode.DeepClone(), h3Def, h3Tag.Name);
                    edModes.Add(edMode);
                    edModeCreated = true;
                }

                foreach (var h3WeaponClass in h3Mode.WeaponClass)
                {
                    var weaponClassLabel = BlamCache.Strings.GetString(h3WeaponClass.Label);
                    var edWeaponClass = edMode.WeaponClass.Find(wc => weaponClassLabel == CacheContext.GetString(wc.Label));
                    var edWeaponClassCreated = false;

                    if (edWeaponClass == null)
                    {
                        edWeaponClass = (ModelAnimationGraph.Mode.WeaponClassBlock)ConvertData(
                            cacheStream, resourceStreams, h3WeaponClass.DeepClone(), h3Def, h3Tag.Name);
                        edMode.WeaponClass.Add(edWeaponClass);
                        edWeaponClassCreated = true;
                    }

                    foreach (var h3WeaponType in h3WeaponClass.WeaponType)
                    {
                        var weaponTypeLabel = BlamCache.Strings.GetString(h3WeaponType.Label);
                        var edWeaponType = edWeaponClass.WeaponType.Find(wt => weaponTypeLabel == CacheContext.GetString(wt.Label));
                        var edWeaponTypeCreated = false;

                        if (edWeaponType == null)
                        {
                            edWeaponType = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock)ConvertData(
                                cacheStream, resourceStreams, h3WeaponType.DeepClone(), h3Def, h3Tag.Name);
                            edWeaponClass.WeaponType.Add(edWeaponType);
                            edWeaponTypeCreated = true;
                        }

                        foreach (var h3Action in h3WeaponType.Actions)
                        {
                            var actionLabel = BlamCache.Strings.GetString(h3Action.Label);
                            var edAction = edWeaponType.Actions.Find(a => actionLabel == CacheContext.GetString(a.Label));
                            var edActionCreated = false;

                            if (edAction == null)
                            {
                                edAction = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry)ConvertData(
                                    cacheStream, resourceStreams, h3Action.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.Actions.Add(edAction);
                                edActionCreated = true;
                            }

                            if (edActionCreated || edWeaponTypeCreated || edWeaponClassCreated || edModeCreated)
                            {
                                if (edAction.GraphIndex == -1)
                                {
                                    edAction.Animation = indices[BlamCache.Strings.GetString(h3Def.Animations[edAction.Animation].Name)].Item2;
                                }
                                else
                                {
                                    var inherited = BlamCache.GetIndexItemFromID(h3Def.InheritanceList[edAction.GraphIndex].InheritedGraph.Index);
                                    edAction.Animation = MergedAnimationData[inherited.Name].Item1.Values.ToList().Find(x => x.Item1 == edAction.Animation).Item2;
                                }
                            }
                        }

                        foreach (var h3Overlay in h3WeaponType.Overlays)
                        {
                            var overlayLabel = BlamCache.Strings.GetString(h3Overlay.Label);
                            var edOverlay = edWeaponType.Overlays.Find(a => overlayLabel == CacheContext.GetString(a.Label));
                            var edOverlayCreated = false;

                            if (edOverlay == null)
                            {
                                edOverlay = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry)ConvertData(
                                    cacheStream, resourceStreams, h3Overlay.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.Overlays.Add(edOverlay);
                                edOverlayCreated = true;
                            }

                            if (edOverlayCreated || edWeaponTypeCreated || edWeaponClassCreated || edModeCreated)
                            {
                                if (edOverlay.GraphIndex == -1)
                                {
                                    edOverlay.Animation = indices[BlamCache.Strings.GetString(h3Def.Animations[edOverlay.Animation].Name)].Item2;
                                }
                                else
                                {
                                    var inherited = BlamCache.GetIndexItemFromID(h3Def.InheritanceList[edOverlay.GraphIndex].InheritedGraph.Index);
                                    edOverlay.Animation = MergedAnimationData[inherited.Name].Item1.Values.ToList().Find(x => x.Item1 == edOverlay.Animation).Item2;
                                }
                            }
                        }

                        foreach (var h3Damage in h3WeaponType.DeathAndDamage)
                        {
                            var damageLabel = BlamCache.Strings.GetString(h3Damage.Label);
                            var edDamage = edWeaponType.DeathAndDamage.Find(d => damageLabel == CacheContext.GetString(d.Label));
                            var edDamageCreated = false;

                            if (edDamage == null)
                            {
                                edDamage = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock)ConvertData(
                                    cacheStream, resourceStreams, h3Damage.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.DeathAndDamage.Add(edDamage);
                                edDamageCreated = true;
                            }

                            if (edDamageCreated || edWeaponTypeCreated || edWeaponClassCreated || edModeCreated)
                            {
                                foreach (var direction in edDamage.Directions)
                                {
                                    foreach (var region in direction.Regions)
                                    {
                                        if (region.GraphIndex == -1)
                                        {
                                            region.Animation = indices[BlamCache.Strings.GetString(h3Def.Animations[region.Animation].Name)].Item2;
                                        }
                                        else
                                        {
                                            var inherited = BlamCache.GetIndexItemFromID(h3Def.InheritanceList[region.GraphIndex].InheritedGraph.Index);
                                            region.Animation = MergedAnimationData[inherited.Name].Item1.Values.ToList().Find(x => x.Item1 == region.Animation).Item2;
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var h3Transition in h3WeaponType.Transitions)
                        {
                            var transitionFullName = BlamCache.Strings.GetString(h3Transition.FullName);
                            var transitionStateName = BlamCache.Strings.GetString(h3Transition.StateName);

                            var edTransition = edWeaponType.Transitions.Find(t =>
                                transitionFullName == CacheContext.GetString(t.FullName) &&
                                transitionStateName == CacheContext.GetString(t.StateName));

                            var edTransitionCreated = false;

                            if (edTransition == null)
                            {
                                edTransition = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition)ConvertData(
                                    cacheStream, resourceStreams, h3Transition.DeepClone(), h3Def, h3Tag.Name);
                                edWeaponType.Transitions.Add(edTransition);
                                edTransitionCreated = true;
                            }

                            foreach (var h3Destination in h3Transition.Destinations)
                            {
                                var destinationFullName = BlamCache.Strings.GetString(h3Destination.FullName);
                                var destinationStateName = BlamCache.Strings.GetString(h3Destination.StateName);

                                var edDestination = edTransition.Destinations.Find(t =>
                                    destinationFullName == CacheContext.GetString(t.FullName) &&
                                    destinationStateName == CacheContext.GetString(t.StateName));

                                var edDestinationCreated = false;

                                if (edDestination == null)
                                {
                                    edDestination = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition.Destination)ConvertData(
                                        cacheStream, resourceStreams, h3Destination.DeepClone(), h3Def, h3Tag.Name);
                                    edTransition.Destinations.Add(edDestination);
                                    edDestinationCreated = true;
                                }

                                if (edDestinationCreated || edTransitionCreated || edWeaponTypeCreated || edWeaponClassCreated || edModeCreated)
                                {
                                    if (edDestination.GraphIndex == -1)
                                    {
                                        edDestination.Animation = indices[BlamCache.Strings.GetString(h3Def.Animations[edDestination.Animation].Name)].Item2;
                                    }
                                    else
                                    {
                                        var inherited = BlamCache.GetIndexItemFromID(h3Def.InheritanceList[edDestination.GraphIndex].InheritedGraph.Index);
                                        edDestination.Animation = MergedAnimationData[inherited.Name].Item1.Values.ToList().Find(x => x.Item1 == edDestination.Animation).Item2;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            edModes = edModes.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();

            foreach (var edMode in edModes)
            {
                edMode.WeaponClass = edMode.WeaponClass.OrderBy(a => a.Label.Set).ThenBy(a => a.Label.Index).ToList();

                foreach (var weaponClass in edMode.WeaponClass)
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

            return edModes;
        }

        private void MergeAnimationGraphs(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTagInstance edTag, CacheFile.IndexItem h3Tag)
        {
            var edDef = CacheContext.Deserialize<ModelAnimationGraph>(cacheStream, edTag);

            var h3Def = BlamCache.Deserializer.Deserialize<ModelAnimationGraph>(
                new CacheSerializationContext(ref BlamCache, h3Tag));

            if (edDef.ParentAnimationGraph != null && h3Def.ParentAnimationGraph != null)
                MergeAnimationGraphs(cacheStream, resourceStreams, edDef.ParentAnimationGraph, BlamCache.GetIndexItemFromID(h3Def.ParentAnimationGraph.Index));

            for (var i = 0; i < h3Def.InheritanceList.Count; i++)
                MergeAnimationGraphs(cacheStream, resourceStreams, edDef.InheritanceList[i].InheritedGraph, BlamCache.GetIndexItemFromID(h3Def.InheritanceList[i].InheritedGraph.Index));

            MergeAnimationTagReferences(cacheStream, resourceStreams, edDef.SoundReferences, h3Def.SoundReferences);
            MergeAnimationTagReferences(cacheStream, resourceStreams, edDef.EffectReferences, h3Def.EffectReferences);

            var animationIndices = MergeAnimations(cacheStream, resourceStreams, h3Tag, h3Def, edDef.Animations);
            edDef.Modes = MergeModes(cacheStream, resourceStreams, h3Tag, h3Def, edDef.Modes, animationIndices);

            //
            // Collect indices of missing resource groups
            //

            var resourceGroupIndices = new List<short>();

            foreach (var entry in animationIndices)
            {
                if (entry.Value.Item1 == -1)
                    continue;

                var h3Animation = h3Def.Animations[entry.Value.Item1];

                if (!resourceGroupIndices.Contains(h3Animation.ResourceGroupIndex))
                    resourceGroupIndices.Add(h3Animation.ResourceGroupIndex);
            }

            //
            // Add missing resource groups
            //

            var resourceGroups = new List<ModelAnimationGraph.ResourceGroup>();
            var resourceGroupData = new Dictionary<short, short>();

            for (var i = 0; i < resourceGroupIndices.Count; i++)
            {
                resourceGroups.Add(h3Def.ResourceGroups[resourceGroupIndices[i]]);

                foreach (var entry in animationIndices)
                    if (entry.Value.Item1 != -1)
                        resourceGroupData[edDef.Animations[entry.Value.Item2].ResourceGroupIndex] =
                            (edDef.Animations[entry.Value.Item2].ResourceGroupIndex = (short)(edDef.ResourceGroups.Count + i));
            }

            edDef.ResourceGroups.AddRange(ConvertModelAnimationGraphResourceGroups(cacheStream, resourceStreams, resourceGroups));

            //
            // Finalize
            //

            CacheContext.Serialize(cacheStream, edTag, edDef);

            MergedAnimationGraphs.Add(h3Tag.Name);
            MergedAnimationData[h3Tag.Name] = (animationIndices, resourceGroupData);
        }
    }
}