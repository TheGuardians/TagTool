using TagTool.Animations.Codecs;
using TagTool.IO;
using System;
using System.Collections;
using TagTool.Commands.Common;

namespace TagTool.Animations.Data
{
    public class AnimationResourceData
    {
        public int FrameCount { get; set; }

        public int NodeCount { get; set; }

        public int NodeListChecksum { get; set; }

        public int StaticFlagsSize { get; set; }
        public int AnimatedFlagsSize { get; set; }
        public FrameInfoType FrameInfoType { get; set; }

        public CodecBase Static_Data { get; set; }

        public CodecBase Animation_Data { get; set; }

        public MovementData Movement_Data { get; set; }

        public BitArray StaticRotatedNodeFlags { get; set; }

        public BitArray StaticTranslatedNodeFlags { get; set; }

        public BitArray StaticScaledNodeFlags { get; set; }

        public BitArray AnimatedRotatedNodeFlags { get; set; }

        public BitArray AnimatedTranslatedNodeFlags { get; set; }

        public BitArray AnimatedScaledNodeFlags { get; set; }

        public AnimationResourceData()
        {
        }

        public AnimationResourceData(
          int frameCount,
          int nodeCount,
          int nodeListChecksum,
          FrameInfoType frameInfoType,
          int staticflagssize,
          int animatedflagssize)
        {
            FrameCount = frameCount;
            NodeCount = nodeCount;
            NodeListChecksum = nodeListChecksum;
            FrameInfoType = frameInfoType;
            StaticFlagsSize = staticflagssize;
            AnimatedFlagsSize = animatedflagssize;
        }

        public bool Read(EndianReader reader)
        {
            while (Animation_Data == null)
            {
                AnimationCodecType codec = (AnimationCodecType)reader.ReadByte();
                --reader.BaseStream.Position;
                switch (codec)
                {
                    case AnimationCodecType.UncompressedStatic:
                        Static_Data = (CodecBase)new UncompressedStaticDataCodec(FrameCount);
                        Static_Data.Read(reader);
                        continue;
                    case AnimationCodecType._8ByteQuantizedRotationOnly:
                        Animation_Data = (CodecBase)new _8ByteQuantizedRotationOnlyCodec(FrameCount);
                        Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.ByteKeyframeLightlyQuantized:
                        Animation_Data = (CodecBase)new KeyframeLightlyQuantizedCodec(FrameCount, 1);
                        Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.WordKeyframeLightlyQuantized:
                        Animation_Data = (CodecBase)new KeyframeLightlyQuantizedCodec(FrameCount, 2);
                        Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.ReverseByteKeyframeLightlyQuantized:
                        Animation_Data = (CodecBase)new ReverseKeyframeLightlyQuantizedCodec(FrameCount, 1);
                        Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.ReverseWordKeyframeLightlyQuantized:
                        Animation_Data = (CodecBase)new ReverseKeyframeLightlyQuantizedCodec(FrameCount, 2);
                        Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.BlendScreen:
                        Animation_Data = (CodecBase)new BlendScreenCodec(FrameCount);
                        Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.Curve:
                        Animation_Data = (CodecBase)new CurveCodec(FrameCount);
                        Animation_Data.Read(reader);
                        continue;
                    default:
                        new TagToolWarning($"Animation codec {codec} not recognized or supported.");
                        return false;
                }
            }
            if (StaticFlagsSize != 0)
            {
                int length = StaticFlagsSize / 3 / 4;
                int[] values1 = new int[length];
                int[] values2 = new int[length];
                int[] values3 = new int[length];
                for (int index = 0; index < length; ++index)
                    values1[index] = reader.ReadInt32();
                for (int index = 0; index < length; ++index)
                    values2[index] = reader.ReadInt32();
                for (int index = 0; index < length; ++index)
                    values3[index] = reader.ReadInt32();
                StaticRotatedNodeFlags = new BitArray(values1);
                StaticTranslatedNodeFlags = new BitArray(values2);
                StaticScaledNodeFlags = new BitArray(values3);
                StaticRotatedNodeFlags.Length = NodeCount;
                StaticTranslatedNodeFlags.Length = NodeCount;
                StaticScaledNodeFlags.Length = NodeCount;
            }
            if (AnimatedFlagsSize != 0)
            {
                int length = AnimatedFlagsSize / 3 / 4;
                int[] values1 = new int[length];
                int[] values2 = new int[length];
                int[] values3 = new int[length];
                for (int index = 0; index < length; ++index)
                    values1[index] = reader.ReadInt32();
                for (int index = 0; index < length; ++index)
                    values2[index] = reader.ReadInt32();
                for (int index = 0; index < length; ++index)
                    values3[index] = reader.ReadInt32();
                AnimatedRotatedNodeFlags = new BitArray(values1);
                AnimatedTranslatedNodeFlags = new BitArray(values2);
                AnimatedScaledNodeFlags = new BitArray(values3);
                AnimatedRotatedNodeFlags.Length = NodeCount;
                AnimatedTranslatedNodeFlags.Length = NodeCount;
                AnimatedScaledNodeFlags.Length = NodeCount;
            }
            if (FrameInfoType <= FrameInfoType.None)
                return true;
            Movement_Data = new MovementData(FrameInfoType, FrameCount);
            Movement_Data.Read(reader);
            return true;
        }

        public void Write(EndianWriter writer) => throw new NotImplementedException();
    }
}
