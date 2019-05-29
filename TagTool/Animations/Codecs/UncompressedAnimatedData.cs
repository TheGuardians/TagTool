using System.Collections.Generic;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Resources;

namespace TagTool.Animations.Codecs
{
    public class UncompressedAnimatedData : AnimationCodecData
    {
        public override void Read(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianReader reader)
        {
            RotationKeyframes = new List<List<int>>();
            Rotations = new List<List<RealQuaternion>>();

            for (var i = 0; i < header.RotationNodeCount; i++)
            {
                Rotations.Add(new List<RealQuaternion>
                {
                    new RealQuaternion(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle())
                    .Normalize()
                });
            }

            TranslationKeyframes = new List<List<int>>();
            Translations = new List<List<RealPoint3d>>();

            for (var i = 0; i < header.TranslationNodeCount; i++)
            {
                Translations.Add(new List<RealPoint3d>
                {
                    new RealPoint3d(
                        reader.ReadSingle() * 100.0f,
                        reader.ReadSingle() * 100.0f,
                        reader.ReadSingle() * 100.0f)
                });
            }

            ScaleKeyframes = new List<List<int>>();
            Scales = new List<List<float>>();

            for (var i = 0; i < header.ScaleNodeCount; i++)
            {
                Scales.Add(new List<float>
                {
                    reader.ReadSingle()
                });
            }
        }

        public override void Write(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianWriter writer)
        {
            groupMember.FrameCount = 0;

            RotationKeyframes = new List<List<int>>();
            TranslationKeyframes = new List<List<int>>();
            ScaleKeyframes = new List<List<int>>();

            header.RotationNodeCount = (byte)Rotations.Count;

            foreach (var rotations in Rotations)
            {
                var rotation = rotations[0];
                writer.Write(rotation.I);
                writer.Write(rotation.J);
                writer.Write(rotation.K);
                writer.Write(rotation.W);
            }

            header.TranslationNodeCount = (byte)Translations.Count;

            foreach (var translations in Translations)
            {
                var translation = translations[0];
                writer.Write(translation.X / 100.0f);
                writer.Write(translation.Y / 100.0f);
                writer.Write(translation.Z / 100.0f);
            }

            header.ScaleNodeCount = (byte)Scales.Count;

            foreach (var scales in Scales)
            {
                var scale = scales[0];
                writer.Write(scale);
            }
        }
    }
}
