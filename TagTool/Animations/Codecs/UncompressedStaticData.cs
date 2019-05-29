using System.Collections.Generic;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Resources;

namespace TagTool.Animations.Codecs
{
    public class UncompressedStaticData : AnimationData
    {
        public override void Read(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianReader reader)
        {
            RotationFrames = new List<List<RealQuaternion>>();

            for (var i = 0; i < header.RotationNodeCount; i++)
                RotationFrames.Add(
                    new List<RealQuaternion>
                    {
                        new RealQuaternion(
                            reader.ReadInt16() / (float)short.MaxValue,
                            reader.ReadInt16() / (float)short.MaxValue,
                            reader.ReadInt16() / (float)short.MaxValue,
                            reader.ReadInt16() / (float)short.MaxValue)
                        .Normalize()
                    });

            TranslationFrames = new List<List<RealPoint3d>>();

            for (var i = 0; i < header.TranslationNodeCount; i++)
                TranslationFrames.Add(
                    new List<RealPoint3d>
                    {
                        new RealPoint3d(
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle())
                    });

            ScaleFrames = new List<List<float>>();

            for (var i = 0; i < header.ScaleNodeCount; i++)
                ScaleFrames.Add(new List<float> { reader.ReadSingle() });
        }

        public override void Write(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianWriter writer)
        {
            header.RotationNodeCount = (byte)RotationFrames.Count;

            foreach (var rotations in RotationFrames)
            {
                var rotation = rotations[0];

                writer.Write((short)(short.MaxValue * rotation.I));
                writer.Write((short)(short.MaxValue * rotation.J));
                writer.Write((short)(short.MaxValue * rotation.K));
                writer.Write((short)(short.MaxValue * rotation.W));
            }

            header.TranslationNodeCount = (byte)TranslationFrames.Count;

            foreach (var translations in TranslationFrames)
            {
                var translation = translations[0];

                writer.Write(translation.X);
                writer.Write(translation.Y);
                writer.Write(translation.Z);
            }

            header.ScaleNodeCount = (byte)ScaleFrames.Count;

            foreach (var scales in ScaleFrames)
                writer.Write(scales[0]);
        }
    }
}