using TagTool.Animations.Codecs;
using TagTool.IO;
using System;
using System.Collections;

namespace TagTool.Animations.Data
{
    public class AnimationResourceData
    {
        public int FrameCount { get; set; }

        public int NodeCount { get; set; }

        public int NodeListChecksum { get; set; }

        public int NodeFlagsSize { get; set; }

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
          FrameInfoType frameInfoType)
        {
            this.FrameCount = frameCount;
            this.NodeCount = nodeCount;
            this.NodeListChecksum = nodeListChecksum;
            this.FrameInfoType = frameInfoType;
            this.NodeFlagsSize = (int)Math.Ceiling((double)nodeCount / 32.0) * 32 / 8 * 3;
        }

        public void Read(EndianReader reader)
        {
            while (this.Animation_Data == null)
            {
                AnimationCodecType codec = (AnimationCodecType)reader.ReadByte();
                --reader.BaseStream.Position;
                switch (codec)
                {
                    case AnimationCodecType.UncompressedStatic:
                        this.Static_Data = (CodecBase)new UncompressedStaticDataCodec(this.FrameCount);
                        this.Static_Data.Read(reader);
                        continue;
                    case AnimationCodecType._8ByteQuantizedRotationOnly:
                        this.Animation_Data = (CodecBase)new _8ByteQuantizedRotationOnlyCodec(this.FrameCount);
                        this.Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.ByteKeyframeLightlyQuantized:
                        this.Animation_Data = (CodecBase)new KeyframeLightlyQuantizedCodec(this.FrameCount, 1);
                        this.Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.WordKeyframeLightlyQuantized:
                        this.Animation_Data = (CodecBase)new KeyframeLightlyQuantizedCodec(this.FrameCount, 2);
                        this.Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.ReverseByteKeyframeLightlyQuantized:
                        this.Animation_Data = (CodecBase)new ReverseKeyframeLightlyQuantizedCodec(this.FrameCount, 1);
                        this.Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.ReverseWordKeyframeLightlyQuantized:
                        this.Animation_Data = (CodecBase)new ReverseKeyframeLightlyQuantizedCodec(this.FrameCount, 2);
                        this.Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.BlendScreen:
                        this.Animation_Data = (CodecBase)new BlendScreenCodec(this.FrameCount);
                        this.Animation_Data.Read(reader);
                        continue;
                    case AnimationCodecType.Curve:
                        this.Animation_Data = (CodecBase)new CurveCodec(this.FrameCount);
                        this.Animation_Data.Read(reader);
                        continue;
                    default:
                        throw new Exception("Animation codec not recognized or supported.");
                }
            }
            if (this.Static_Data != null)
            {
                int length = this.NodeFlagsSize / 3 / 4;
                int[] values1 = new int[length];
                int[] values2 = new int[length];
                int[] values3 = new int[length];
                for (int index = 0; index < length; ++index)
                    values1[index] = reader.ReadInt32();
                for (int index = 0; index < length; ++index)
                    values2[index] = reader.ReadInt32();
                for (int index = 0; index < length; ++index)
                    values3[index] = reader.ReadInt32();
                this.StaticRotatedNodeFlags = new BitArray(values1);
                this.StaticTranslatedNodeFlags = new BitArray(values2);
                this.StaticScaledNodeFlags = new BitArray(values3);
                this.StaticRotatedNodeFlags.Length = this.NodeCount;
                this.StaticTranslatedNodeFlags.Length = this.NodeCount;
                this.StaticScaledNodeFlags.Length = this.NodeCount;
            }
            if (this.Animation_Data != null)
            {
                int length = this.NodeFlagsSize / 3 / 4;
                int[] values1 = new int[length];
                int[] values2 = new int[length];
                int[] values3 = new int[length];
                for (int index = 0; index < length; ++index)
                    values1[index] = reader.ReadInt32();
                for (int index = 0; index < length; ++index)
                    values2[index] = reader.ReadInt32();
                for (int index = 0; index < length; ++index)
                    values3[index] = reader.ReadInt32();
                this.AnimatedRotatedNodeFlags = new BitArray(values1);
                this.AnimatedTranslatedNodeFlags = new BitArray(values2);
                this.AnimatedScaledNodeFlags = new BitArray(values3);
                this.AnimatedRotatedNodeFlags.Length = this.NodeCount;
                this.AnimatedTranslatedNodeFlags.Length = this.NodeCount;
                this.AnimatedScaledNodeFlags.Length = this.NodeCount;
            }
            if (this.FrameInfoType <= FrameInfoType.None)
                return;
            this.Movement_Data = new MovementData(this.FrameInfoType, this.FrameCount);
            this.Movement_Data.Read(reader);
        }

        public void Write(EndianWriter writer) => throw new NotImplementedException();
    }
}
