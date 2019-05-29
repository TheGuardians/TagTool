using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sytem.IO;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Animations.Codecs
{
    public class UncompressedStaticData : AnimationCodecData
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
                        reader.ReadSingle(TagFieldCompression.Int16),
                        reader.ReadSingle(TagFieldCompression.Int16),
                        reader.ReadSingle(TagFieldCompression.Int16),
                        reader.ReadSingle(TagFieldCompression.Int16))
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
                writer.Write(rotation.I, TagFieldCompression.Int16);
                writer.Write(rotation.J, TagFieldCompression.Int16);
                writer.Write(rotation.K, TagFieldCompression.Int16);
                writer.Write(rotation.W, TagFieldCompression.Int16);
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
