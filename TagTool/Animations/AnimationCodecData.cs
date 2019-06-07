using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Animations
{
    public class AnimationCodecData
    {
        public int FrameCount { get; set; }
        public float PlaybackSpeed { get; set; } = 1f;

        public AnimationCodecType CodecType { get; set; }

        public byte RotatedNodeCount { get; set; }
        public byte TranslatedNodeCount { get; set; }
        public byte ScaledNodeCount { get; set; }

        public List<List<int>> RotationKeyFrames { get; set; }
        public List<List<int>> TranslationKeyFrames { get; set; }
        public List<List<int>> ScaleKeyFrames { get; set; }

        public RealQuaternion[][] Rotations { get; set; }
        public RealPoint3d[][] Translations { get; set; }
        public float[][] Scales { get; set; }

        public AnimationCodecData()
        {
        }

        public AnimationCodecData(int frameCount)
        {
            FrameCount = frameCount;
        }

        public virtual void Read(EndianReader reader)
        {
            CodecType = reader.ReadEnum<AnimationCodecType>();
            RotatedNodeCount = reader.ReadByte();
            TranslatedNodeCount = reader.ReadByte();
            ScaledNodeCount = reader.ReadByte();
            reader.ReadSingle();
            PlaybackSpeed = reader.ReadSingle();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
        }

        public virtual void Write(EndianWriter writer)
        {
            writer.Write((sbyte)CodecType);
            writer.Write(RotatedNodeCount);
            writer.Write(TranslatedNodeCount);
            writer.Write(ScaledNodeCount);
            writer.Write(0f);
            writer.Write(PlaybackSpeed);
            writer.Write(0);
            writer.Write(0);
            writer.Write(0);
            writer.Write(0);
            writer.Write(0);
        }
    }
}
