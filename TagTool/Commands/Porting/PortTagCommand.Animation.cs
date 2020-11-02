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
        public List<ModelAnimationGraph.ResourceGroup> ConvertModelAnimationGraphResourceGroups(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, List<ModelAnimationGraph.ResourceGroup> resourceGroups)
        {
            var resourceDefinitions = new List<ModelAnimationTagResource>();

            foreach (var group in resourceGroups)
            {
                var resourceDefinition = BlamCache.ResourceCache.GetModelAnimationTagResource(group.ResourceReference);

                if(resourceDefinition == null)
                {
                    group.ResourceReference = null;
                    continue;
                }

                for (var memberIndex = 0; memberIndex < resourceDefinition.GroupMembers.Count; memberIndex++)
                {
                    var member = resourceDefinition.GroupMembers[memberIndex];
                    var animationData = member.AnimationData.Data;

                    using(var sourceStream = new MemoryStream(animationData))
                    using(var sourceReader = new EndianReader(sourceStream, CacheVersionDetection.IsLittleEndian(BlamCache.Version) ? EndianFormat.LittleEndian : EndianFormat.BigEndian))
                    using(var destStream = new MemoryStream())
                    using(var destWriter = new EndianWriter(destStream, CacheVersionDetection.IsLittleEndian(CacheContext.Version) ? EndianFormat.LittleEndian : EndianFormat.BigEndian))
                    {
                        var dataContext = new DataSerializationContext(sourceReader, destWriter);

                        ModelAnimationTagResource.GroupMember.Codec codec;
                        ModelAnimationTagResource.GroupMember.FrameInfo frameInfo;

                        if (member.CompressedData != ModelAnimationTagResource.GroupMemberHeaderType.Overlay)
                        {
                            codec = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Codec>(dataContext);

                            CacheContext.Serializer.Serialize(dataContext, codec);

                            var Format1 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Format1>(dataContext);

                            CacheContext.Serializer.Serialize(dataContext, Format1);

                            for (int i = 0; i < codec.RotationNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                            sourceStream.Position = Format1.DataStart;
                            destStream.Position = sourceStream.Position;
                            for (int i = 0; i < codec.PositionNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                            sourceStream.Position = Format1.ScaleFramesOffset;
                            destStream.Position = sourceStream.Position;
                            for (int i = 0; i < codec.ScaleNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                        }

                        // If the overlay header is alone, member.MovementData = 0
                        sourceStream.Position = member.MovementData;
                        destStream.Position = member.MovementData;

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

                                sourceStream.Position = member.MovementData + header.DataStart;
                                destStream.Position = sourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.PositionNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                sourceStream.Position = member.MovementData + header.ScaleFramesOffset;
                                destStream.Position = sourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.ScaleNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext,
                                            BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));

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

                                sourceStream.Position = member.MovementData + overlay.RotationKeyframesOffset;
                                destStream.Position = sourceStream.Position;
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

                                sourceStream.Position = member.MovementData + overlay.PositionKeyframesOffset;
                                destStream.Position = sourceStream.Position;
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

                                sourceStream.Position = member.MovementData + overlay.ScaleKeyframesOffset;
                                destStream.Position = sourceStream.Position;
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

                                sourceStream.Position = member.MovementData + overlay.RotationFramesOffset;
                                destStream.Position = sourceStream.Position;
                                foreach (var framecount in RotationFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                                sourceStream.Position = member.MovementData + overlay.PositionFramesOffset;
                                destStream.Position = sourceStream.Position;
                                foreach (var framecount in PositionFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                sourceStream.Position = member.MovementData + overlay.ScaleFramesOffset;
                                destStream.Position = sourceStream.Position;
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

                                sourceStream.Position = member.MovementData + Format8.PositionFramesOffset;
                                destStream.Position = sourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.PositionNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                sourceStream.Position = member.MovementData + Format8.ScaleFramesOffset;
                                destStream.Position = sourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.ScaleNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));

                                break;


                            case ModelAnimationTagResource.AnimationCompressionFormats.None_:
                                // empty data, copy buffer and skip
                                member.AnimationData.Data = sourceStream.ToArray();
                                continue;
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

                        sourceStream.Position = member.MovementData + member.StaticNodeFlags;
                        destStream.Position = sourceStream.Position;

                        var footerSizeBase = (byte)member.CompressedData / 4;
                        for (int flagsCount = 0; flagsCount < footerSizeBase; flagsCount++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));

                        var footerSizeOverlay = (byte)member.UncompressedData / 4;
                        for (int flagsCount = 0; flagsCount < footerSizeOverlay; flagsCount++)
                            CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                        #endregion

                        switch (member.MovementDataType)
                        {
                            case ModelAnimationTagResource.GroupMemberMovementDataType.None:
                                if (member.DefaultData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDyaw>(dataContext));
                                if (member.PillOffsetData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy:
                                if (member.DefaultData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDy>(dataContext));
                                if (member.PillOffsetData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dyaw:
                                if (member.DefaultData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                if (member.PillOffsetData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dz_dyaw:
                                if (member.DefaultData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDzDyaw>(dataContext));
                                if (member.PillOffsetData > 0)
                                    for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDzDyaw>(dataContext));
                                break;
                            default:
                                break;
                        }

                        // set new data
                        member.AnimationData.Data = destStream.ToArray();
                    }
                }

                group.ResourceReference = CacheContext.ResourceCache.CreateModelAnimationGraphResource(resourceDefinition);
            }

            return resourceGroups;
        }

        public ModelAnimationGraph ConvertModelAnimationGraph(Stream cacheStream, Stream blamCacheStream,  Dictionary<ResourceLocation, Stream> resourceStreams, ModelAnimationGraph definition)
        {
            definition.ResourceGroups = ConvertModelAnimationGraphResourceGroups(cacheStream, blamCacheStream, resourceStreams, definition.ResourceGroups);
            var resolver = CacheContext.StringTable.Resolver;
            definition.Modes = definition.Modes.OrderBy(a => resolver.GetSet(a.Name)).ThenBy(a => resolver.GetIndex(a.Name)).ToList();

            foreach (var mode in definition.Modes)
            {
                mode.WeaponClass = mode.WeaponClass.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();

                foreach (var weaponClass in mode.WeaponClass)
                {
                    weaponClass.WeaponType = weaponClass.WeaponType.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();

                    foreach (var weaponType in weaponClass.WeaponType)
                    {
                        weaponType.Actions = weaponType.Actions.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Overlays = weaponType.Overlays.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.DeathAndDamage = weaponType.DeathAndDamage.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Transitions = weaponType.Transitions.OrderBy(a => resolver.GetSet(a.FullName)).ThenBy(a => resolver.GetIndex(a.FullName)).ToList();

                        foreach (var transition in weaponType.Transitions)
                            transition.Destinations = transition.Destinations.OrderBy(a => resolver.GetSet(a.FullName)).ThenBy(a => resolver.GetIndex(a.FullName)).ToList();
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
    }
}