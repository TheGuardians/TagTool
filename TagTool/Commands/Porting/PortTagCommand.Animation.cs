using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public ModelAnimationGraph ConvertModelAnimationGraph(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, ModelAnimationGraph definition)
        {
            if (BlamCache.ResourceGestalt == null)
                BlamCache.LoadResourceTags();
            
            // loop through every resource group

            // convert resource definition
            List<ModelAnimationTagResource> resourceDefinition = new List<ModelAnimationTagResource>();
            foreach (var group in definition.ResourceGroups)
            {
                var resourceEntry = BlamCache.ResourceGestalt.TagResources[group.ZoneAssetDatumIndex & ushort.MaxValue];

                group.Resource = new PageableResource
                {
                    Page = new RawPage
                    {
                        Index = -1,
                    },
                    Resource = new TagResource
                    {
                        Type = TagResourceType.Animation,
                        DefinitionData = BlamCache.ResourceGestalt.FixupInformation.Skip(resourceEntry.FixupInformationOffset).Take(resourceEntry.FixupInformationLength).ToArray(),
                        DefinitionAddress = new CacheAddress(CacheAddressType.Definition, resourceEntry.DefinitionAddress),
                        ResourceFixups = new List<TagResource.ResourceFixup>(),
                        ResourceDefinitionFixups = new List<TagResource.ResourceDefinitionFixup>(),
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
                            var newFixup = new TagResource.ResourceFixup
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
                        definitionWriter.Write(0x20000000); // hack of the year
                        // ODST's resource type is 4 when it's supposed to be 2 because the resource definition is in the tag and not as a raw resource 

                        definitionStream.Position = group.Resource.Resource.DefinitionAddress.Offset;

                        resourceDefinition.Add(BlamCache.Deserializer.Deserialize<ModelAnimationTagResource>(dataContext));
                    }
                }
            }

            // Convert raw: codec, header, rotation, position and scale frames, flags block, dx_dy block
            // ideally, i should move frame conversion to their own functions
            int resDefIndex = -1;
            foreach (var group in definition.ResourceGroups)
            {
                resDefIndex++;

                var resourceData = BlamCache.GetRawFromID(group.ZoneAssetDatumIndex);

                if (resourceData == null)
                    return null;

                // Convert raw: codec, header, rotation, position and scale frames, flags block, dx_dy block
                using (var blamResourceStream = new MemoryStream(resourceData))
                using (var resourceReader = new EndianReader(blamResourceStream, EndianFormat.BigEndian))
                using (var dataStream = new MemoryStream())
                using (var resourceWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
                {
                    var dataContext = new DataSerializationContext(resourceReader, resourceWriter);

                    // loop trough each member in the resource definition.
                    foreach (var member in resourceDefinition[resDefIndex].GroupMembers)
                    {
                        // there's no size difference between H3/ODST and HO raw animation data

                        ModelAnimationTagResource.GroupMember.Codec codec;
                        ModelAnimationTagResource.GroupMember.FrameInfo frameInfo;

                        // if the coder indicates a type1 animation, proceed to deserialize it
                        if ((byte)member.BaseHeader != 0)
                        {
                            blamResourceStream.Position = (long)member.AnimationData.Address.Offset;
                            dataStream.Position = (long)member.AnimationData.Address.Offset;

                            codec = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Codec>(dataContext);


                            CacheContext.Serializer.Serialize(dataContext, codec);

                            var Format1 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Format1>(dataContext);

                            CacheContext.Serializer.Serialize(dataContext, Format1);

                            // deserialize frames
                            // blamResourceStream.Position = (long)member.AnimationData.Address.Offset + headerSize;
                            // edResourceStream.Position = blamResourceStream.Position;
                            for (int i = 0; i < codec.RotationNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                            blamResourceStream.Position = (long)member.AnimationData.Address.Offset + Format1.DataStart;
                            dataStream.Position = blamResourceStream.Position;
                            for (int i = 0; i < codec.PositionNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                            blamResourceStream.Position = (long)member.AnimationData.Address.Offset + Format1.ScaleFramesOffset;
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
                            #region Type3 Type8
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
                            #endregion

                            #region Type4-5-6-7
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
                                // Since it's Keyrame Markers are only 1 byte in size, you're animation cannot be longer than 256 frames, or ~8.5 seconds for non - machine objects.
                                // Machines are still limited to 256 frames but the frames can be stretched out.
                                #endregion

                                // Keyframe info
                                // DESERIALIZATION SIMPLIFIED; in order to export the animation, a list of nodes and their data needs to be made
                                #region Potato Description
                                // One uint FrameInfo per node.
                                // Last byte in FrameInfo is the framecount for that node.
                                // First 3 bytes are offset for this node in Keyframes, KeyframesOffset as origin

                                // In Keyframe, one byte per frame.
                                // Type5's Keyframe is one short per frame
                                // The keyframe might be the ingame frame where this frame is supposed to be
                                #endregion

                                int RotationFrameCount = 0;
                                int PositionFrameCount = 0;
                                int ScaleFrameCount = 0;

                                // FrameInfo
                                for (int i = 0; i < codec.RotationNodeCount; i++)
                                {
                                    frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);

                                    CacheContext.Serializer.Serialize(dataContext, frameInfo);

                                    RotationFrameCount += (byte)frameInfo.FrameCount;
                                }

                                for (int i = 0; i < codec.PositionNodeCount; i++)
                                {
                                    frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);

                                    CacheContext.Serializer.Serialize(dataContext, frameInfo);

                                    PositionFrameCount += (byte)frameInfo.FrameCount;
                                }

                                for (int i = 0; i < codec.ScaleNodeCount; i++)
                                {
                                    frameInfo = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfo>(dataContext);

                                    CacheContext.Serializer.Serialize(dataContext, frameInfo);

                                    ScaleFrameCount += (byte)frameInfo.FrameCount;
                                }



                                // keyframes
                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.RotationKeyframesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                for (int i = 0; i < RotationFrameCount; i++)
                                    if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type5)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.KeyframeType5>(dataContext));
                                    else
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Keyframe>(dataContext));


                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.PositionKeyframesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                for (int i = 0; i < PositionFrameCount; i++)
                                    if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type5)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.KeyframeType5>(dataContext));
                                    else
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Keyframe>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.ScaleKeyframesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                for (int i = 0; i < ScaleFrameCount; i++) // ??
                                    if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type5)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.KeyframeType5>(dataContext));
                                    else
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Keyframe>(dataContext));



                                // frames
                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.RotationFramesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                for (int frameIndex = 0; frameIndex < RotationFrameCount; frameIndex++)
                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.PositionFramesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                for (int frameIndex = 0; frameIndex < PositionFrameCount; frameIndex++)
                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                blamResourceStream.Position = (long)member.AnimationData.Address.Offset + member.OverlayOffset + overlay.ScaleFramesOffset;
                                dataStream.Position = blamResourceStream.Position;
                                for (int frameIndex = 0; frameIndex < ScaleFrameCount; frameIndex++)
                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));


                                break;
                            #endregion

                            #region Type8
                            case ModelAnimationTagResource.AnimationCompressionFormats.Type8:
                                var Format8 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Format8>(dataContext);

                                CacheContext.Serializer.Serialize(dataContext, Format8);

                                // deserialize frames

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
                            #endregion

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
                        {
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                        }

                        var footerSizeOverlay = (byte)member.OverlayHeader / 4;
                        for (int flagsCount = 0; flagsCount < footerSizeOverlay; flagsCount++)
                        {
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                        }
                        #endregion

                        // TODO
                        #region dx_dy and dx_dy_dyaw block; why the hell is it after the footer/flag block?
                        // dx_dy seems to be 2x uint
                        // dx_dy_dyaw seems to be 3x uint

                        switch (member.MovementDataType)
                        {
                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy:
                                for (int i = 0; i < member.FrameCount; i++)
                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDy>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dyaw:
                                for (int i = 0; i < member.FrameCount; i++)
                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dz_dyaw: // NOT VERIFIED
                                for (int i = 0; i < member.FrameCount; i++)
                                    CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDzDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.None:
                                break;

                            default:
                                throw new NotImplementedException();
                        }

                        #endregion

                        // There's some mysterious data that isn't dx_dy, isn't considered in the member resource size, doesn't seem related to anything, maybe garbage data as padding? sounds unreasonable
                        // it's zeroed in HO's ported animations
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
                        resourceStreams[ResourceLocation.ResourcesB] = Flags.HasFlag(PortingFlags.Memory) ?
                            new MemoryStream() :
                            (Stream)CacheContext.OpenResourceCacheReadWrite(ResourceLocation.ResourcesB);

                        if (Flags.HasFlag(PortingFlags.Memory))
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
    }
}