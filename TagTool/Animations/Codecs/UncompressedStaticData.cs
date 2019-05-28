using System.Collections.Generic;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Resources;

namespace TagTool.Animations.Codecs
{
    public class UncompressedStaticData : AnimationData
    {
        public List<RealQuaternion> Rotations;
        public List<RealPoint3d> Translations;
        public List<float> Scales;

        public override void Read(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianReader reader)
        {
            Rotations = new List<RealQuaternion>();

            for (var i = 0; i < header.RotationNodeCount; i++)
                Rotations.Add(
                    new RealQuaternion(
                        reader.ReadInt16() / (float)short.MaxValue,
                        reader.ReadInt16() / (float)short.MaxValue,
                        reader.ReadInt16() / (float)short.MaxValue,
                        reader.ReadInt16() / (float)short.MaxValue)
                    .Normalize());

            Translations = new List<RealPoint3d>();

            for (var i = 0; i < header.TranslationNodeCount; i++)
                Translations.Add(
                    new RealPoint3d(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle()));

            Scales = new List<float>();

            for (var i = 0; i < header.ScaleNodeCount; i++)
                Scales.Add(reader.ReadSingle());
        }

        public override void Write(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianWriter writer)
        {
            header.RotationNodeCount = (byte)Rotations.Count;

            foreach (var rotation in Rotations)
            {
                writer.Write((short)(short.MaxValue * rotation.I));
                writer.Write((short)(short.MaxValue * rotation.J));
                writer.Write((short)(short.MaxValue * rotation.K));
                writer.Write((short)(short.MaxValue * rotation.W));
            }

            header.TranslationNodeCount = (byte)Translations.Count;

            foreach (var translation in Translations)
            {
                writer.Write(translation.X);
                writer.Write(translation.Y);
                writer.Write(translation.Z);
            }

            header.ScaleNodeCount = (byte)Scales.Count;

            foreach (var scale in Scales)
                writer.Write(scale);
        }
    }
}