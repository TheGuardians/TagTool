using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Animations.Codecs
{
    public class _8ByteQuantizedRotationOnlyData : AnimationCodecData
    {
        public override void Read(EndianReader reader)
        {
            base.Read(reader);

            Rotations = new RealQuaternion[RotatedNodeCount][];
            Translations = new RealPoint3d[TranslatedNodeCount][];
            Scales = new float[ScaledNodeCount][];

            RotationKeyFrames = new List<List<int>>(RotatedNodeCount);
            TranslationKeyFrames = new List<List<int>>(TranslatedNodeCount);
            ScaleKeyFrames = new List<List<int>>(ScaledNodeCount);

            for (var i = 0; i < RotatedNodeCount; i++)
                RotationKeyFrames.Add(Enumerable.Range(0, FrameCount).ToList());

            for (var i = 0; i < TranslatedNodeCount; i++)
                TranslationKeyFrames.Add(Enumerable.Range(0, FrameCount).ToList());

            for (var i = 0; i < ScaledNodeCount; i++)
                ScaleKeyFrames.Add(Enumerable.Range(0, FrameCount).ToList());

            for (var i = 0; i < RotatedNodeCount; i++)
            {
                Rotations[i] = new RealQuaternion[RotationKeyFrames[i].Count];

                for (var j = 0; j < RotationKeyFrames[i].Count; j++)
                    Rotations[i][j] = new RealQuaternion(
                        (float)reader.ReadInt16() / short.MaxValue,
                        (float)reader.ReadInt16() / short.MaxValue,
                        (float)reader.ReadInt16() / short.MaxValue,
                        (float)reader.ReadInt16() / short.MaxValue).Normalize();
            }

            for (var i = 0; i < TranslatedNodeCount; i++)
            {
                Translations[i] = new RealPoint3d[TranslationKeyFrames[i].Count];

                for (var j = 0; j < TranslationKeyFrames[i].Count; j++)
                    Translations[i][j] = new RealPoint3d(
                        reader.ReadSingle() * 100f,
                        reader.ReadSingle() * 100f,
                        reader.ReadSingle() * 100f);
            }

            for (var i = 0; i < ScaledNodeCount; i++)
            {
                Scales[i] = new float[ScaleKeyFrames[i].Count];

                for (var j = 0; j < ScaleKeyFrames[i].Count; j++)
                    Scales[i][j] = reader.ReadSingle();
            }
        }

        public override void Write(EndianWriter writer)
        {
            base.Write(writer);

            for (var i = 0; i < RotatedNodeCount; i++)
            {
                for (var j = 0; j < RotationKeyFrames[i].Count; j++)
                {
                    var rotation = Rotations[i][j].Normalize();

                    writer.Write((short)(rotation.I * short.MaxValue));
                    writer.Write((short)(rotation.J * short.MaxValue));
                    writer.Write((short)(rotation.K * short.MaxValue));
                    writer.Write((short)(rotation.W * short.MaxValue));
                }
            }

            for (var i = 0; i < TranslatedNodeCount; i++)
            {
                for (var j = 0; j < TranslationKeyFrames[i].Count; j++)
                {
                    var translation = Translations[i][j];

                    writer.Write(translation.X / 100f);
                    writer.Write(translation.Y / 100f);
                    writer.Write(translation.Z / 100f);
                }
            }

            for (var i = 0; i < ScaledNodeCount; i++)
                for (var j = 0; j < ScaleKeyFrames[i].Count; j++)
                    writer.Write(Scales[i][j]);
        }
    }
}