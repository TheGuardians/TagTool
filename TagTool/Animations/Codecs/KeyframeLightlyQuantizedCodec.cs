using TagTool.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System;
using System.Numerics;
using System.Collections.Generic;

namespace TagTool.Animations.Codecs
{
    public class KeyframeLightlyQuantizedCodec : CodecBase
    {
        protected int KeySize { get; }

        public KeyframeLightlyQuantizedCodec(int framecount, int keysize)
          : base(framecount)
          => this.KeySize = keysize;

        public override void Read(EndianReader reader)
        {
            uint position = (uint)reader.BaseStream.Position;
            base.Read(reader);
            int num1 = (int)reader.ReadUInt32();
            int num2 = (int)reader.ReadUInt32();
            int num3 = (int)reader.ReadUInt32();
            int num4 = (int)reader.ReadUInt32();
            int num5 = (int)reader.ReadUInt32();
            this.Rotations = new Quaternion[(int)this.RotatedNodeCount][];
            this.Translations = new RealPoint3d[(int)this.TranslatedNodeCount][];
            this.Scales = new float[(int)this.ScaledNodeCount][];
            this.RotationKeyFrames = new List<List<int>>((int)this.RotatedNodeCount);
            this.TranslationKeyFrames = new List<List<int>>((int)this.TranslatedNodeCount);
            this.ScaleKeyFrames = new List<List<int>>((int)this.ScaledNodeCount);
            this.RotationDataOffset = position + reader.ReadUInt32();
            this.TranslationDataOffset = position + reader.ReadUInt32();
            this.ScaleDataOffset = position + reader.ReadUInt32();
            int num6 = (int)reader.ReadUInt32();
            for (int index = 0; index < (int)this.RotatedNodeCount; ++index)
            {
                int num7 = (int)reader.ReadUInt32();
            }
            for (int index = 0; index < (int)this.TranslatedNodeCount; ++index)
            {
                int num7 = (int)reader.ReadUInt32();
            }
            for (int index = 0; index < (int)this.ScaledNodeCount; ++index)
            {
                int num7 = (int)reader.ReadUInt32();
            }
            for (int index = 0; index < (int)this.RotatedNodeCount; ++index)
                this.RotationKeyFrames.Add(this.ReadKeyFrameData(this.KeySize, reader));
            for (int index = 0; index < (int)this.TranslatedNodeCount; ++index)
                this.TranslationKeyFrames.Add(this.ReadKeyFrameData(this.KeySize, reader));
            for (int index = 0; index < (int)this.ScaledNodeCount; ++index)
                this.ScaleKeyFrames.Add(this.ReadKeyFrameData(this.KeySize, reader));
            reader.BaseStream.Position = (long)this.RotationDataOffset;
            for (int index1 = 0; index1 < (int)this.RotatedNodeCount; ++index1)
            {
                this.Rotations[index1] = new Quaternion[this.RotationKeyFrames[index1].Count];
                for (int index2 = 0; index2 < this.RotationKeyFrames[index1].Count; ++index2)
                    this.Rotations[index1][index2] = Quaternion.Normalize(new Quaternion()
                    {
                        X = (float)reader.ReadInt16() / (float)short.MaxValue,
                        Y = (float)reader.ReadInt16() / (float)short.MaxValue,
                        Z = (float)reader.ReadInt16() / (float)short.MaxValue,
                        W = (float)reader.ReadInt16() / (float)short.MaxValue
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

        protected List<int> ReadKeyFrameData(int keysize, EndianReader reader)
        {
            List<int> intList = new List<int>();
            int num1 = 0;
            while (true)
            {
                int num2 = 0;
                switch (keysize)
                {
                    case 1:
                        num2 = (int)reader.ReadByte();
                        break;
                    case 2:
                        num2 = (int)reader.ReadUInt16();
                        break;
                }
                if (num1 <= 0 || intList[num1 - 1] <= num2 && this.FrameCount >= num2)
                {
                    intList.Add(num2);
                    ++num1;
                }
                else
                    break;
            }
            reader.BaseStream.Position -= (long)keysize;
            return intList;
        }
    }
}
