using TagTool.IO;
using TagTool.Common;
using System.Numerics;
using TagTool.Cache;
using TagTool.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TagTool.Serialization;

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

        public override byte[] Write(GameCacheHaloOnlineBase CacheContext)
        {
            using (MemoryStream stream = new MemoryStream())
            using (EndianWriter writer = new EndianWriter(stream, EndianFormat.LittleEndian))
            {
                var dataContext = new DataSerializationContext(writer, CacheAddressType.Memory, false);

                var datastartoffset = stream.Position.DeepClone();

                var codecheader = new AnimationCodecHeader
                {
                    Type = AnimationCodecType.BlendScreen,
                    RotationCount = (sbyte)this.RotatedNodeCount,
                    TranslationCount = (sbyte)this.TranslatedNodeCount,
                    ScaleCount = (sbyte)this.ScaledNodeCount,
                    PlaybackRate = this.CompressionRate,
                    ErrorPercentage = this.ErrorValue
                };
                CacheContext.Serializer.Serialize(dataContext, codecheader);

                var staticcodecheaderoffset = stream.Position.DeepClone();
                //write an empty static codec header for now, will return to this later 
                CacheContext.Serializer.Serialize(dataContext, new StaticCodecHeader());

                //write rotation frame data
                for (int index1 = 0; index1 < (int)this.RotatedNodeCount; ++index1)
                {
                    for (int index2 = 0; index2 < this.RotationKeyFrames[index1].Count; ++index2)
                    {
                        Quaternion rotation = this.Rotations[index1][index2];
                        writer.Write(rotation.X);
                        writer.Write(rotation.Y);
                        writer.Write(rotation.Z);
                        writer.Write(rotation.W);
                    };
                };

                //record translation data offset to write to header later on
                var translationdataoffset = stream.Position.DeepClone();
                //write translation frame data
                for (int index1 = 0; index1 < (int)this.TranslatedNodeCount; ++index1)
                {
                    for (int index2 = 0; index2 < this.TranslationKeyFrames[index1].Count; ++index2)
                    {
                        RealPoint3d translation = this.Translations[index1][index2];
                        writer.Write(translation.X);
                        writer.Write(translation.Y);
                        writer.Write(translation.Z);
                    };
                };

                //record scale data offset to write to header later on
                var scaledataoffset = stream.Position.DeepClone();
                //write scale frame data
                for (int index1 = 0; index1 < (int)this.ScaledNodeCount; ++index1)
                {
                    for (int index2 = 0; index2 < this.ScaleKeyFrames[index1].Count; ++index2)
                    {
                        float scale = this.Scales[index1][index2];
                        writer.Write(scale);
                    };
                };

                //record end of data offset
                var dataendoffset = stream.Position.DeepClone();
                //move back to header to write data sizes
                stream.Position = staticcodecheaderoffset;
                var staticcodecsizes = new StaticCodecHeader
                {
                    TranslationDataOffset = (int)translationdataoffset - (int)datastartoffset,
                    ScaleDataOffset = (int)scaledataoffset - (int)datastartoffset,
                    RotationFrameSize = 0x10 * this.FrameCount, //four shorts times number of frames in animation
                    TranslationFrameSize = 0xC * this.FrameCount, //three floats times number of frames in animation
                    ScaleFrameSize = 0x4 * this.FrameCount //one float times number of frames in animation
                };
                CacheContext.Serializer.Serialize(dataContext, staticcodecsizes);

                //return to the end of the stream
                stream.Position = stream.Length;
                return stream.ToArray();
            }
        }
    }
}
