using TagTool.IO;
using TagTool.Cache;
using TagTool.Common;
using System.Numerics;
using TagTool.Tags;
using System;

namespace TagTool.Animations.Codecs
{
    public class UncompressedStaticDataCodec : CodecBase
    {
        public UncompressedStaticDataCodec()
        {
        }

        public UncompressedStaticDataCodec(int framecount)
          : base(framecount)
        {
        }

        public override void Read(EndianReader reader)
        {
            base.Read(reader);
            this.TranslationDataOffset = reader.ReadUInt32();
            this.ScaleDataOffset = reader.ReadUInt32();
            this.RotatedNodeBlockSize = reader.ReadUInt32();
            this.TranslatedNodeBlockSize = reader.ReadUInt32();
            this.ScaledNodeBlockSize = reader.ReadUInt32();
            this.Rotations = new Quaternion[(int)this.RotatedNodeCount][];
            this.Translations = new RealPoint3d[(int)this.TranslatedNodeCount][];
            this.Scales = new float[(int)this.ScaledNodeCount][];
            for (int index = 0; index < (int)this.RotatedNodeCount; ++index)
            {
                Quaternion q = new Quaternion();
                q.X = (float)reader.ReadInt16() / (float)short.MaxValue;
                q.Y = (float)reader.ReadInt16() / (float)short.MaxValue;
                q.Z = (float)reader.ReadInt16() / (float)short.MaxValue;
                q.W = (float)reader.ReadInt16() / (float)short.MaxValue;
                this.Rotations[index] = new Quaternion[1];
                this.Rotations[index][0] = Quaternion.Normalize(q);
            }
            for (int index = 0; index < (int)this.TranslatedNodeCount; ++index)
            {
                RealPoint3d RealPoint3d = new RealPoint3d();
                RealPoint3d.X = reader.ReadSingle() * 100f;
                RealPoint3d.Y = reader.ReadSingle() * 100f;
                RealPoint3d.Z = reader.ReadSingle() * 100f;
                this.Translations[index] = new RealPoint3d[1];
                this.Translations[index][0] = RealPoint3d;
            }
            for (int index = 0; index < (int)this.ScaledNodeCount; ++index)
            {
                float real = reader.ReadSingle();
                this.Scales[index] = new float[1];
                this.Scales[index][0] = real;
            }
        }

        public override byte[] Write(GameCacheHaloOnlineBase CacheContext) => throw new NotImplementedException();
    }
}
