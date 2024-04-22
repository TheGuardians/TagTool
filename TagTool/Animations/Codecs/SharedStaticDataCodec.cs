using TagTool.IO;
using TagTool.Cache;
using TagTool.Common;
using System.Numerics;
using TagTool.Tags.Definitions.Gen4;
using System;

namespace TagTool.Animations.Codecs
{
    public class SharedStaticDataCodec : CodecBase
    {
        public ModelAnimationGraph.AnimationCodecDataStruct.SharedStaticDataCodecGraphDataStruct SharedStaticData { get; set; }
        public SharedStaticDataCodec()
        {
        }

        public SharedStaticDataCodec(int framecount)
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
                int block_index = reader.ReadInt16();
                var rotationblock = SharedStaticData.Rotations[block_index];
                Quaternion q = new Quaternion();
                q.X = (float)rotationblock.I / (float)short.MaxValue;
                q.Y = (float)rotationblock.J / (float)short.MaxValue;
                q.Z = (float)rotationblock.K / (float)short.MaxValue;
                q.W = (float)rotationblock.W / (float)short.MaxValue;
                this.Rotations[index] = new Quaternion[1];
                this.Rotations[index][0] = Quaternion.Normalize(q);
            }
            for (int index = 0; index < (int)this.TranslatedNodeCount; ++index)
            {
                int block_index = reader.ReadInt16();
                var translationblock = SharedStaticData.Translations[block_index];
                RealPoint3d RealPoint3d = new RealPoint3d();
                RealPoint3d.X = translationblock.X * 100f;
                RealPoint3d.Y = translationblock.Y * 100f;
                RealPoint3d.Z = translationblock.Z * 100f;
                this.Translations[index] = new RealPoint3d[1];
                this.Translations[index][0] = RealPoint3d;
            }
            for (int index = 0; index < (int)this.ScaledNodeCount; ++index)
            {
                int block_index = reader.ReadInt16();
                var scaleblock = SharedStaticData.Scale[block_index];
                float real = scaleblock.Scale;
                this.Scales[index] = new float[1];
                this.Scales[index][0] = real;
            }
        }

        public override byte[] Write(GameCacheHaloOnlineBase CacheContext) => throw new NotImplementedException();
    }
}
