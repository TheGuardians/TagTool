using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Porting
{
    partial class PortingContext
    {
        public List<ModelAnimationGraph.ResourceGroup> ConvertModelAnimationGraphResourceGroups(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, List<ModelAnimationGraph.ResourceGroup> resourceGroups)
        {
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

                    //load the data sizes for verification of the ported data
                    int MovementDataSize = 0;
                    int StaticNodeFlagsSize = 0;
                    int AnimatedNodeFlagsSize = 0;
                    int CompressedDataSize = 0;
                    int StaticDataSize = 0;
                    int ExtraData = 0;
                    if(BlamCache.Version >= CacheVersion.HaloReach)
                    {
                        MovementDataSize = member.PackedDataSizesReach.MovementData;
                        StaticNodeFlagsSize = member.PackedDataSizesReach.StaticNodeFlags;
                        AnimatedNodeFlagsSize = member.PackedDataSizesReach.AnimatedNodeFlags;
                        CompressedDataSize = member.PackedDataSizesReach.CompressedDataSize;
                        StaticDataSize = member.PackedDataSizesReach.StaticDataSize;
                        for (var j = 0; j < member.PackedDataSizesReach.UnknownDataSizesReach.Length; j++)
                            ExtraData += member.PackedDataSizesReach.UnknownDataSizesReach[j];
                        //copy values to pre-Reach data sizes block
                        member.PackedDataSizes = new ModelAnimationTagResource.GroupMember.PackedDataSizesStructBlock
                        {
                            MovementData = (short)member.PackedDataSizesReach.MovementData,
                            StaticNodeFlags = (byte)member.PackedDataSizesReach.StaticNodeFlags,
                            AnimatedNodeFlags = (byte)member.PackedDataSizesReach.AnimatedNodeFlags,
                            CompressedDataSize = member.PackedDataSizesReach.CompressedDataSize,
                            StaticDataSize = (short)member.PackedDataSizesReach.StaticDataSize
                        };
                    }
                    else
                    {
                        MovementDataSize = member.PackedDataSizes.MovementData;
                        StaticNodeFlagsSize = member.PackedDataSizes.StaticNodeFlags;
                        AnimatedNodeFlagsSize = member.PackedDataSizes.AnimatedNodeFlags;
                        CompressedDataSize = member.PackedDataSizes.CompressedDataSize;
                        StaticDataSize = member.PackedDataSizes.StaticDataSize;
                        member.PackedDataSizesReach = new ModelAnimationTagResource.GroupMember.PackedDataSizesStructReach
                        {
                            UnknownDataSizesReach = new int[12]
                        };
                    }

                    using (var sourceStream = new MemoryStream(animationData))
                    using (var sourceReader = new EndianReader(sourceStream, CacheVersionDetection.IsLittleEndian(BlamCache.Version, BlamCache.Platform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian))
                    using (var destStream = new MemoryStream())
                    using (var destWriter = new EndianWriter(destStream, CacheVersionDetection.IsLittleEndian(CacheContext.Version, BlamCache.Platform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian))
                    {
                        var dataContext = new DataSerializationContext(sourceReader, destWriter, CacheAddressType.Memory, false);

                        ModelAnimationTagResource.GroupMember.Codec codec;
                        ModelAnimationTagResource.GroupMember.FrameInfo frameInfo;

                        codec = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Codec>(dataContext);
                        CacheContext.Serializer.Serialize(dataContext, codec);

                        if (codec.AnimationCodec == ModelAnimationTagResource.AnimationCompressionFormats.Type1)
                        {
                            var Format1 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.UncompressedHeader>(dataContext);

                            CacheContext.Serializer.Serialize(dataContext, Format1);

                            for (int i = 0; i < codec.RotationNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                            sourceStream.Position = Format1.TranslationDataOffset;
                            destStream.Position = sourceStream.Position;
                            for (int i = 0; i < codec.TranslationNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                            sourceStream.Position = Format1.ScaleDataOffset;
                            destStream.Position = sourceStream.Position;
                            for (int i = 0; i < codec.ScaleNodeCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));

                            if (sourceStream.Position != StaticDataSize || destStream.Position != StaticDataSize)
                                new TagToolError(CommandError.CustomError, "Static Data Size did not match data sizes struct!");

                            //read next codec header
                            codec = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.Codec>(dataContext);
                            CacheContext.Serializer.Serialize(dataContext, codec);
                        }

                        // deserialize second header. or as first header if the type1/format1 header isn't used.
                        switch (codec.AnimationCodec)
                        {
                            case ModelAnimationTagResource.AnimationCompressionFormats.Type3: // should merge with type1
                                var header = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.UncompressedHeader>(dataContext);
                                CacheContext.Serializer.Serialize(dataContext, header);

                                for (int nodeIndex = 0; nodeIndex < codec.RotationNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                                sourceStream.Position = header.TranslationDataOffset + StaticDataSize;
                                destStream.Position = sourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.TranslationNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                sourceStream.Position = header.ScaleDataOffset + StaticDataSize;
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
                                var compressedheader = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.CompressedHeader>(dataContext);
                                CacheContext.Serializer.Serialize(dataContext, compressedheader);

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
                                // Since it's Keyrame Markers are only 1 byte in size, your animation cannot be longer than 256 frames, or ~8.5 seconds for non - machine objects. > 12 bits for gen3, max 0xFFF frames
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

                                for (int i = 0; i < codec.TranslationNodeCount; i++)
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

                                sourceStream.Position = compressedheader.RotationKeyframesOffset + StaticDataSize;
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

                                sourceStream.Position = compressedheader.PositionKeyframesOffset + StaticDataSize;
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

                                sourceStream.Position = compressedheader.ScaleKeyframesOffset + StaticDataSize;
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

                                sourceStream.Position = compressedheader.RotationFramesOffset + StaticDataSize;
                                destStream.Position = sourceStream.Position;
                                foreach (var framecount in RotationFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrame>(dataContext));

                                sourceStream.Position = compressedheader.PositionFramesOffset + StaticDataSize;
                                destStream.Position = sourceStream.Position;
                                foreach (var framecount in PositionFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                sourceStream.Position = compressedheader.ScaleFramesOffset + StaticDataSize;
                                destStream.Position = sourceStream.Position;
                                foreach (var framecount in ScaleFrameCount)
                                    for (int i = 0; i < framecount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                                break;

                            case ModelAnimationTagResource.AnimationCompressionFormats.Type8:
                                // Type 8 is basically a type 3 but with rotation frames using 4 floats, or a realQuaternion
                                var Format8 = BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.UncompressedHeader>(dataContext);
                                CacheContext.Serializer.Serialize(dataContext, Format8);

                                for (int nodeIndex = 0; nodeIndex < codec.RotationNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.RotationFrameFloat>(dataContext));

                                sourceStream.Position = Format8.TranslationDataOffset + StaticDataSize;
                                destStream.Position = sourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.TranslationNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));

                                sourceStream.Position = Format8.ScaleDataOffset + StaticDataSize;
                                destStream.Position = sourceStream.Position;
                                for (int nodeIndex = 0; nodeIndex < codec.ScaleNodeCount; nodeIndex++)
                                    for (int frameIndex = 0; frameIndex < member.FrameCount; frameIndex++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                                break;

                            //highly experimental support for handling Reach type9 animations
                            //uses Bonobo code to read the type9 animation, and hybrid code to write a type3 codec in its place
                            case ModelAnimationTagResource.AnimationCompressionFormats.Type9:
                                sourceStream.Position = StaticDataSize;
                                Animations.Codecs.CurveCodec type9 = (Animations.Codecs.CurveCodec)new Animations.Codecs.CurveCodec(member.FrameCount);
                                type9.Read(sourceReader);

                                //create a new type3 codec from the parsed type9 data
                                Animations.Codecs._8ByteQuantizedRotationOnlyCodec type3 = new Animations.Codecs._8ByteQuantizedRotationOnlyCodec(member.FrameCount)
                                {
                                    Rotations = type9.Rotations,
                                    Translations = type9.Translations,
                                    Scales = type9.Scales,
                                    RotationKeyFrames = type9.RotationKeyFrames,
                                    TranslationKeyFrames = type9.TranslationKeyFrames,
                                    ScaleKeyFrames = type9.ScaleKeyFrames,
                                    RotatedNodeCount = type9.RotatedNodeCount,
                                    TranslatedNodeCount = type9.TranslatedNodeCount,
                                    ScaledNodeCount = type9.ScaledNodeCount,
                                    ErrorValue = type9.ErrorValue,
                                    CompressionRate = type9.CompressionRate
                                };

                                //undo scaling normally performed for export
                                for (var i = 0; i < type3.Translations.Count(); i++)
                                {
                                    for(var j = 0; j < type3.Translations[i].Count(); j++)
                                    {
                                        type3.Translations[i][j] = type3.Translations[i][j] / 100.0f;
                                    }
                                }                              

                                byte[] animationcodecdata = type3.Write(CacheContext);
                                member.PackedDataSizes.CompressedDataSize = animationcodecdata.Length;
                                //set out stream position to the start of the animated data block and write out the new codec
                                destStream.Position = StaticDataSize;
                                destWriter.WriteBlock(animationcodecdata);
                                break;

                            case ModelAnimationTagResource.AnimationCompressionFormats.None_:
                                // empty data, copy buffer and skip
                                member.AnimationData.Data = sourceStream.ToArray();
                                continue;
                            default:
                                new TagToolError(CommandError.CustomError, $"Animation codec {codec.AnimationCodec.ToString()} not supported!");
                                member.AnimationData.Data = null;
                                continue;
                        }

                        if ((sourceStream.Position != StaticDataSize + CompressedDataSize || destStream.Position != StaticDataSize + CompressedDataSize)
                            && codec.AnimationCodec != ModelAnimationTagResource.AnimationCompressionFormats.Type9)
                            new TagToolError(CommandError.CustomError, "Compressed Data Size did not match data sizes struct!");

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

                        //int NodeFlagsSize = (int)Math.Ceiling((double)member.NodeCount / 32.0) * 32 / 8 * 3;

                        if(StaticNodeFlagsSize > 0)
                        {
                            //var footerSizeBase = (byte)NodeFlagsSize / 4;
                            for (int flagsCount = 0; flagsCount < StaticNodeFlagsSize; flagsCount+=4)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                        }

                        if ((sourceStream.Position != StaticDataSize + CompressedDataSize + StaticNodeFlagsSize || destStream.Position != StaticDataSize + CompressedDataSize + StaticNodeFlagsSize)
                            && codec.AnimationCodec != ModelAnimationTagResource.AnimationCompressionFormats.Type9)
                            new TagToolError(CommandError.CustomError, "Static Node Flags Size did not match data sizes struct!");

                        if (AnimatedNodeFlagsSize > 0)
                        {
                            //var footerSizeBase = (byte)NodeFlagsSize / 4;
                            for (int flagsCount = 0; flagsCount < AnimatedNodeFlagsSize; flagsCount+=4)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.ScaleFrame>(dataContext));
                        }

                        if ((sourceStream.Position != StaticDataSize + CompressedDataSize + StaticNodeFlagsSize + AnimatedNodeFlagsSize || destStream.Position != StaticDataSize + CompressedDataSize + StaticNodeFlagsSize + AnimatedNodeFlagsSize)
                            && codec.AnimationCodec != ModelAnimationTagResource.AnimationCompressionFormats.Type9) 
                            new TagToolError(CommandError.CustomError, "Animated Node Flags Size did not match data sizes struct!");

                        #endregion

                        switch (member.MovementDataType)
                        {
                            case ModelAnimationTagResource.GroupMemberMovementDataType.None:
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy:
                                for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDy>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dyaw:
                                for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDyaw>(dataContext));
                                break;

                            case ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dz_dyaw:
                                for (int i = 0; i < member.FrameCount; i++)
                                        CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.FrameInfoDxDyDzDyaw>(dataContext));
                                break;
                            default:
                                break;
                        }

                        if ((sourceStream.Position != StaticDataSize + CompressedDataSize + StaticNodeFlagsSize + AnimatedNodeFlagsSize + MovementDataSize ||
                            destStream.Position != StaticDataSize + CompressedDataSize + StaticNodeFlagsSize + AnimatedNodeFlagsSize + MovementDataSize)
                            && codec.AnimationCodec != ModelAnimationTagResource.AnimationCompressionFormats.Type9)
                            new TagToolError(CommandError.CustomError, "Movement Data Size did not match data sizes struct!");

                        //convert pill offset data
                        if(member.PackedDataSizes.PillOffsetData > 0)
                        {
                            for (int i = 0; i < member.FrameCount; i++)
                                CacheContext.Serializer.Serialize(dataContext, BlamCache.Deserializer.Deserialize<ModelAnimationTagResource.GroupMember.PositionFrame>(dataContext));
                        }

                        //include the extra data in reach as a part of the data size calculations
                        int reachextradata = member.PackedDataSizesReach.UnknownDataSizesReach.Sum();

                        if ((member.AnimationData.Data.Length != destStream.ToArray().Length + reachextradata)
                            && codec.AnimationCodec != ModelAnimationTagResource.AnimationCompressionFormats.Type9)
                        {
                            new TagToolError(CommandError.CustomError, "Converted Animation Data was of a different length than the original!");
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
            
            if(BlamCache.Version <= CacheVersion.Halo3ODST)
            {
                foreach (var animation in definition.Animations)
                {
                    foreach (var frameevent in animation.AnimationData.FrameEvents)
                    {
                        Enum.TryParse(frameevent.Type.ToString(), out frameevent.TypeHO);
                    };
                }
            }

            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                //convert animations
                foreach (var animation in definition.Animations)
                {
                    if (animation.AnimationDataBlock.Count > 1)
                        new TagToolWarning("Reach animation has >1 animation data block, whereas HO only supports 1");
                    animation.AnimationData = animation.AnimationDataBlock[0];

                    if (animation.AnimationData.AnimationTypeReach == ModelAnimationGraph.FrameTypeReach.None)
                        animation.AnimationData.AnimationType = ModelAnimationGraph.FrameType.Base;
                    else
                        animation.AnimationData.AnimationType = (ModelAnimationGraph.FrameType)Enum.Parse(typeof(ModelAnimationGraph.FrameType), animation.AnimationData.AnimationTypeReach.ToString());

                    animation.AnimationData.ParentAnimation = animation.PreviousVariantSiblingReach;
                    animation.AnimationData.NextAnimation = animation.NextVariantSiblingReach;
                    animation.AnimationData.DesiredCompression = animation.AnimationData.DesiredCompressionReach;
                    animation.AnimationData.CurrentCompression = animation.AnimationData.CurrentCompressionReach;
                    animation.AnimationData.ProductionFlags = animation.ProductionFlagsReach;
                    animation.AnimationData.Heading = animation.AnimationData.HeadingReach;
                    animation.AnimationData.AveragePivotYaw = animation.AnimationData.AveragePivotYawReach;
                    animation.AnimationData.AverageTranslationMagnitude = animation.AnimationData.AverageTranslationMagnitudeReach;
                    animation.AnimationData.BlendScreen = animation.BlendScreenReach;
                    foreach (var soundevent in animation.AnimationData.SoundEvents)
                        soundevent.MarkerName = ConvertStringId(soundevent.MarkerName);
                    foreach (var effectevent in animation.AnimationData.EffectEvents)
                        effectevent.MarkerName = ConvertStringId(effectevent.MarkerName);
                    foreach(var frameevent in animation.AnimationData.FrameEvents)
                    {
                        Enum.TryParse(frameevent.ReachType.ToString(), out frameevent.TypeHO);
                    };

                }

                foreach (var sound in definition.SoundReferences)
                    Enum.TryParse(sound.FlagsReach.ToString(), out sound.Flags);

                foreach (var effect in definition.SoundReferences)
                    Enum.TryParse(effect.FlagsReach.ToString(), out effect.Flags);

                //convert weapon types
                foreach (var mode in definition.Modes)
                {
                    foreach (var weaponClass in mode.WeaponClass)
                    {
                        foreach (var weaponType in weaponClass.WeaponType)
                        {
                            if (weaponType.AnimationSetsReach.Count > 1)
                                new TagToolWarning("Reach animation has >1 weapon type sets block, whereas HO only supports 1");
                            weaponType.Set = weaponType.AnimationSetsReach[0];
                            //manually convert stringids from copied reach data
                            foreach(var action in weaponType.Set.Actions)
                            {
                                action.Label = ConvertStringId(action.Label);
                            }                               
                            foreach (var overlay in weaponType.Set.Overlays)
                            {
                                overlay.Label = ConvertStringId(overlay.Label);
                            }
                            foreach (var death in weaponType.Set.DeathAndDamage)
                            {
                                death.Label = ConvertStringId(death.Label);
                            }
                            foreach (var transition in weaponType.Set.Transitions)
                            {
                                transition.StateName = ConvertStringId(transition.StateName);
                                foreach(var destination in transition.Destinations)
                                {
                                    destination.ModeName = ConvertStringId(destination.ModeName);
                                    destination.StateName = ConvertStringId(destination.StateName);
                                }
                            }
                                
                        }
                    }
                }
            }

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
                        weaponType.Set.Actions = weaponType.Set.Actions.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Set.Overlays = weaponType.Set.Overlays.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Set.DeathAndDamage = weaponType.Set.DeathAndDamage.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Set.Transitions = weaponType.Set.Transitions.OrderBy(a => resolver.GetSet(a.FullName)).ThenBy(a => resolver.GetIndex(a.FullName)).ToList();

                        foreach (var transition in weaponType.Set.Transitions)
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