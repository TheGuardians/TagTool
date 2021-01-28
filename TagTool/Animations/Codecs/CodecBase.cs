using TagTool.IO;
using TagTool.Common;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;

//Credit to Bonobo for this animation reading code

namespace TagTool.Animations.Codecs
{
    public class codec_base
    {
        public int FrameCount { get; set; }

        public Codec Codec { get; set; }

        public byte RotatedNodeCount { get; set; }

        public byte TranslatedNodeCount { get; set; }

        public byte ScaledNodeCount { get; set; }

        public float ErrorValue { get; set; }

        public float CompressionRate { get; set; }

        public uint RotationDataOffset { get; set; }

        public uint TranslationDataOffset { get; set; }

        public uint ScaleDataOffset { get; set; }

        public uint RotatedNodeBlockSize { get; set; }

        public uint TranslatedNodeBlockSize { get; set; }

        public uint ScaledNodeBlockSize { get; set; }

        public List<List<int>> RotationKeyFrames { get; set; }

        public List<List<int>> TranslationKeyFrames { get; set; }

        public List<List<int>> ScaleKeyFrames { get; set; }

        public Quaternion[][] Rotations { get; set; }

        public RealPoint3d[][] Translations { get; set; }

        public float[][] Scales { get; set; }

        public codec_base()
        {
        }

        public codec_base(int frameCount) => this.FrameCount = frameCount;

        public virtual void Read(EndianReader reader)
        {
            this.Codec = (Codec)reader.ReadByte();
            this.RotatedNodeCount = reader.ReadByte();
            this.TranslatedNodeCount = reader.ReadByte();
            this.ScaledNodeCount = reader.ReadByte();
            this.ErrorValue = reader.ReadSingle();
            this.CompressionRate = reader.ReadSingle() * 100f;
        }

        public virtual byte[] Write(GameCacheHaloOnlineBase CacheContext) => throw new NotImplementedException();
    }

    public enum Codec
    {
        _no_compression_codec,
        _uncompressed_static_data_codec,
        _uncompressed_animated_data_codec,
        _8byte_quantized_rotation_only_codec,
        byte_keyframe_lightly_quantized,
        word_keyframe_lightly_quantized,
        reverse_byte_keyframe_lightly_quantized,
        reverse_word_keyframe_lightly_quantized,
        _blend_screen_codec,
        _curve_codec,
    }
}
