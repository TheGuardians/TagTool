using TagTool.IO;
using TagTool.Common;
using System.Numerics;
using TagTool.Cache;
using TagTool.Tags;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagTool.Animations.Codecs
{
    public class BlendScreenCodec : CodecBase
    {
        public BlendScreenCodec(int framecount)
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
            this.RotationKeyFrames = new List<List<int>>((int)this.RotatedNodeCount);
            this.TranslationKeyFrames = new List<List<int>>((int)this.TranslatedNodeCount);
            this.ScaleKeyFrames = new List<List<int>>((int)this.ScaledNodeCount);
            this.RotationDataOffset = (uint)reader.BaseStream.Position;
            this.TranslationDataOffset = this.RotationDataOffset + this.RotatedNodeBlockSize * (uint)this.RotatedNodeCount;
            this.ScaleDataOffset = this.TranslationDataOffset + this.TranslatedNodeBlockSize * (uint)this.TranslatedNodeCount;
            for (int index = 0; index < (int)this.RotatedNodeCount; ++index)
                this.RotationKeyFrames.Add(Enumerable.Range(0, this.FrameCount).ToList<int>());
            for (int index = 0; index < (int)this.TranslatedNodeCount; ++index)
                this.TranslationKeyFrames.Add(Enumerable.Range(0, this.FrameCount).ToList<int>());
            for (int index = 0; index < (int)this.ScaledNodeCount; ++index)
                this.ScaleKeyFrames.Add(Enumerable.Range(0, this.FrameCount).ToList<int>());
            reader.BaseStream.Position = (long)this.RotationDataOffset;
            for (int index1 = 0; index1 < (int)this.RotatedNodeCount; ++index1)
            {
                this.Rotations[index1] = new Quaternion[this.RotationKeyFrames[index1].Count];
                for (int index2 = 0; index2 < this.RotationKeyFrames[index1].Count; ++index2)
                    this.Rotations[index1][index2] = Quaternion.Normalize(new Quaternion()
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle(),
                        W = reader.ReadSingle()
                    });
            }
            reader.BaseStream.Position = (long)this.TranslationDataOffset;
            for (int index1 = 0; index1 < (int)this.TranslatedNodeCount; ++index1)
            {
                this.Translations[index1] = new RealPoint3d[this.TranslationKeyFrames[index1].Count];
                for (int index2 = 0; index2 < this.TranslationKeyFrames[index1].Count; ++index2)
                    this.Translations[index1][index2] = new RealPoint3d()
                    {
                        X = reader.ReadSingle() * 100f,
                        Y = reader.ReadSingle() * 100f,
                        Z = reader.ReadSingle() * 100f
                    };
            }
            reader.BaseStream.Position = (long)this.ScaleDataOffset;
            for (int index1 = 0; index1 < (int)this.ScaledNodeCount; ++index1)
            {
                this.Scales[index1] = new float[this.ScaleKeyFrames[index1].Count];
                for (int index2 = 0; index2 < this.ScaleKeyFrames[index1].Count; ++index2)
                    this.Scales[index1][index2] = reader.ReadSingle();
            }
        }

        public override byte[] Write(GameCacheHaloOnlineBase CacheContext) => throw new NotImplementedException();
    }
}
