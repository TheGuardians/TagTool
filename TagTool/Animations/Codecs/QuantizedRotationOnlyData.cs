using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Resources;

namespace TagTool.Animations.Codecs
{
    public class QuantizedRotationOnlyData : AnimationData
    {
        public override void Read(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianReader reader)
        {
            RotationKeyframes = new List<List<int>>();
            RotationFrames = new List<List<RealQuaternion>>();

            for (var i = 0; i < header.RotationNodeCount; i++)
            {
                RotationKeyframes.Add(Enumerable.Range(0, groupMember.FrameCount).ToList());

                var rotations = new List<RealQuaternion>();

                for (var j = 0; j < groupMember.FrameCount; j++)
                    rotations.Add(
                        new RealQuaternion(
                            reader.ReadInt16() / (float)short.MaxValue,
                            reader.ReadInt16() / (float)short.MaxValue,
                            reader.ReadInt16() / (float)short.MaxValue,
                            reader.ReadInt16() / (float)short.MaxValue)
                        .Normalize());

                RotationFrames.Add(rotations);
            }

            TranslationKeyframes = new List<List<int>>();
            TranslationFrames = new List<List<RealPoint3d>>();

            for (var i = 0; i < header.TranslationNodeCount; i++)
            {
                TranslationKeyframes.Add(Enumerable.Range(0, groupMember.FrameCount).ToList());

                var translations = new List<RealPoint3d>();

                for (var j = 0; j < groupMember.FrameCount; j++)
                    translations.Add(
                        new RealPoint3d(
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle()));

                TranslationFrames.Add(translations);
            }

            ScaleKeyframes = new List<List<int>>();
            ScaleFrames = new List<List<float>>();

            for (var i = 0; i < header.ScaleNodeCount; i++)
            {
                ScaleKeyframes.Add(Enumerable.Range(0, groupMember.FrameCount).ToList());

                var scales = new List<float>();

                for (var j = 0; j < groupMember.FrameCount; j++)
                    scales.Add(reader.ReadSingle());

                ScaleFrames.Add(scales);
            }
        }

        public override void Write(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianWriter writer)
        {
            int? frameCount = null;

            header.RotationNodeCount = (byte)RotationFrames.Count;

            foreach (var rotations in RotationFrames)
            {
                if (!frameCount.HasValue)
                    frameCount = rotations.Count;
                else if (rotations.Count != frameCount.Value)
                    throw new FormatException($"Invalid rotation frame count: {rotations.Count}");

                foreach (var rotation in rotations)
                {
                    writer.Write((short)(short.MaxValue * rotation.I));
                    writer.Write((short)(short.MaxValue * rotation.J));
                    writer.Write((short)(short.MaxValue * rotation.K));
                    writer.Write((short)(short.MaxValue * rotation.W));
                }
            }

            header.TranslationNodeCount = (byte)TranslationFrames.Count;

            foreach (var translations in TranslationFrames)
            {
                if (!frameCount.HasValue)
                    frameCount = translations.Count;
                else if (translations.Count != frameCount.Value)
                    throw new FormatException($"Invalid translation frame count: {translations.Count}");

                foreach (var translation in translations)
                {
                    writer.Write(translation.X);
                    writer.Write(translation.Y);
                    writer.Write(translation.Z);
                }
            }

            header.ScaleNodeCount = (byte)ScaleFrames.Count;

            foreach (var scales in ScaleFrames)
            {
                if (!frameCount.HasValue)
                    frameCount = scales.Count;
                else if (scales.Count != frameCount.Value)
                    throw new FormatException($"Invalid scale frame count: {scales.Count}");

                foreach (var scale in scales)
                    writer.Write(scale);
            }

            if (frameCount.HasValue)
            {
                if (frameCount.Value < 0 || frameCount.Value > short.MaxValue)
                    throw new FormatException($"Invalid frame count: {frameCount.Value}");

                groupMember.FrameCount = (short)frameCount.Value;
            }
        }
    }
}
