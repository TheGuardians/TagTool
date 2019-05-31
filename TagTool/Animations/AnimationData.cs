using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Animations
{
    public sealed class AnimationData
    {
        public ModelAnimationTagResource.GroupMember GroupMember { get; set; }
        public AnimationCodecHeader Header { get; set; }

        public int FrameCount => GroupMember.FrameCount;
        public AnimationCodecType Type => Header.CodecType;

        public List<List<int>> RotationKeyframes { get; set; }
        public List<List<int>> TranslationKeyframes { get; set; }
        public List<List<int>> ScaleKeyframes { get; set; }

        public List<List<RealQuaternion>> Rotations { get; set; }
        public List<List<RealPoint3d>> Translations { get; set; }
        public List<List<float>> Scales { get; set; }

        public AnimationData(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header)
        {
            GroupMember = groupMember;
            Header = header;
        }

        public void Read(EndianReader reader)
        {
            var explicitKeyframes = false;
            var compressedRotations = false;
            var reversed = false;

            uint? rotationsOffset = null;
            uint? translationsOffset = null;
            uint? scalesOffset = null;

            int frameCount;

            switch (Type)
            {
                case AnimationCodecType.UncompressedStatic:
                    compressedRotations = true;
                    frameCount = 1;
                    break;

                case AnimationCodecType.UncompressedAnimated:
                    frameCount = 1;
                    break;

                case AnimationCodecType.ReverseByteKeyframeLightlyQuantized:
                case AnimationCodecType.ReverseWordKeyframeLightlyQuantized:
                    reversed = true;
                    goto case AnimationCodecType.WordKeyframeLightlyQuantized;

                case AnimationCodecType.ByteKeyframeLightlyQuantized:
                case AnimationCodecType.WordKeyframeLightlyQuantized:
                    explicitKeyframes = true;
                    compressedRotations = true;
                    goto default;

                default:
                    frameCount = GroupMember.FrameCount;
                    break;
            }

            RotationKeyframes = new List<List<int>>();
            TranslationKeyframes = new List<List<int>>();
            ScaleKeyframes = new List<List<int>>();

            if (explicitKeyframes)
            {
                rotationsOffset = reader.ReadUInt32();
                translationsOffset = reader.ReadUInt32();
                scalesOffset = reader.ReadUInt32();

                reader.ReadUInt32();

                for (var i = 0; i < Header.RotationNodeCount; i++)
                    reader.ReadUInt32();

                for (var i = 0; i < Header.TranslationNodeCount; i++)
                    reader.ReadUInt32();

                for (var i = 0; i < Header.ScaleNodeCount; i++)
                    reader.ReadUInt32();
            }

            for (var i = 0; i < Header.RotationNodeCount; i++)
                RotationKeyframes.Add(ReadKeyframes(explicitKeyframes, frameCount, reader));

            for (var i = 0; i < Header.TranslationNodeCount; i++)
                TranslationKeyframes.Add(ReadKeyframes(explicitKeyframes, frameCount, reader));

            for (var i = 0; i < Header.ScaleNodeCount; i++)
                ScaleKeyframes.Add(ReadKeyframes(explicitKeyframes, frameCount, reader));

            if (rotationsOffset.HasValue)
                reader.SeekTo(GroupMember.AnimationData.Address.Offset + rotationsOffset.Value);

            Rotations = new List<List<Common.RealQuaternion>>();

            for (var i = 0; i < Header.RotationNodeCount; i++)
            {
                var rotations = new List<RealQuaternion>();

                for (var j = 0; j < frameCount; j++)
                {
                    rotations.Add(
                        new RealQuaternion(
                            reader.ReadSingle(compressedRotations ? TagFieldCompression.Int16 : TagFieldCompression.None),
                            reader.ReadSingle(compressedRotations ? TagFieldCompression.Int16 : TagFieldCompression.None),
                            reader.ReadSingle(compressedRotations ? TagFieldCompression.Int16 : TagFieldCompression.None),
                            reader.ReadSingle(compressedRotations ? TagFieldCompression.Int16 : TagFieldCompression.None))
                        .Normalize());
                }

                Rotations.Add(rotations);
            }

            if (translationsOffset.HasValue)
                reader.SeekTo(GroupMember.AnimationData.Address.Offset + translationsOffset.Value);

            Translations = new List<List<RealPoint3d>>();

            for (var i = 0; i < Header.TranslationNodeCount; i++)
            {
                var translations = new List<RealPoint3d>();

                for (var j = 0; j < frameCount; j++)
                {
                    translations.Add(
                        new RealPoint3d(
                            reader.ReadSingle() * 100.0f,
                            reader.ReadSingle() * 100.0f,
                            reader.ReadSingle() * 100.0f));
                }

                Translations.Add(translations);
            }

            if (scalesOffset.HasValue)
                reader.SeekTo(GroupMember.AnimationData.Address.Offset + scalesOffset.Value);

            Scales = new List<List<float>>();

            for (var i = 0; i < Header.ScaleNodeCount; i++)
            {
                var scales = new List<float>();

                for (var j = 0; j < frameCount; j++)
                    scales.Add(reader.ReadSingle());

                Scales.Add(scales);
            }

            switch (Type)
            {
                case AnimationCodecType.UncompressedStatic:
                case AnimationCodecType.UncompressedAnimated:
                    RotationKeyframes.Clear();
                    TranslationKeyframes.Clear();
                    ScaleKeyframes.Clear();
                    break;
            }

            if (reversed)
            {
                RotationKeyframes.Reverse();
                TranslationKeyframes.Reverse();
                ScaleKeyframes.Reverse();

                Rotations.Reverse();
                Translations.Reverse();
                Scales.Reverse();
            }
        }

        public void Write(EndianWriter writer)
        {
            throw new NotImplementedException();
        }

        private List<int> ReadKeyframes(bool explicitKeyframes, int frameCount, EndianReader reader)
        {
            if (!explicitKeyframes)
                return Enumerable.Range(0, frameCount).ToList();

            var keyframes = new List<int>();

            for (var num = 0; ; num++)
            {
                int keyframe;

                switch (Type)
                {
                    case AnimationCodecType.ByteKeyframeLightlyQuantized:
                    case AnimationCodecType.ReverseByteKeyframeLightlyQuantized:
                        keyframe = reader.ReadByte();
                        break;

                    case AnimationCodecType.WordKeyframeLightlyQuantized:
                    case AnimationCodecType.ReverseWordKeyframeLightlyQuantized:
                        keyframe = reader.ReadUInt16();
                        break;

                    default:
                        throw new NotSupportedException(Header.CodecType.ToString());
                }

                if (num > 0 && (keyframes[num - 1] > keyframe || frameCount < keyframe))
                    break;

                keyframes.Add(keyframe);
            }

            switch (Type)
            {
                case AnimationCodecType.ByteKeyframeLightlyQuantized:
                case AnimationCodecType.ReverseByteKeyframeLightlyQuantized:
                    reader.BaseStream.Position -= 1;
                    break;

                case AnimationCodecType.WordKeyframeLightlyQuantized:
                case AnimationCodecType.ReverseWordKeyframeLightlyQuantized:
                    reader.BaseStream.Position -= 2;
                    break;

                default:
                    throw new NotSupportedException(Header.CodecType.ToString());
            }

            return keyframes;
        }
    }
}