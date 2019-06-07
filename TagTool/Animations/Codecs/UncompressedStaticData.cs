using TagTool.Common;
using TagTool.IO;

namespace TagTool.Animations.Codecs
{
    public class UncompressedStaticData : AnimationCodecData
    {
        public override void Read(EndianReader reader)
        {
            base.Read(reader);

            Rotations = new RealQuaternion[RotatedNodeCount][];
            Translations = new RealPoint3d[TranslatedNodeCount][];
            Scales = new float[ScaledNodeCount][];

            for (var i = 0; i < RotatedNodeCount; i++)
            {
                Rotations[i] = new RealQuaternion[1];
                Rotations[i][0] = new RealQuaternion(
                    (float)reader.ReadInt16() / short.MaxValue,
                    (float)reader.ReadInt16() / short.MaxValue,
                    (float)reader.ReadInt16() / short.MaxValue,
                    (float)reader.ReadInt16() / short.MaxValue).Normalize();
            }

            for (var i = 0; i < TranslatedNodeCount; i++)
            {
                Translations[i] = new RealPoint3d[1];
                Translations[i][0] = new RealPoint3d(
                    reader.ReadSingle() * 100f,
                    reader.ReadSingle() * 100f,
                    reader.ReadSingle() * 100f);
            }

            for (var i = 0; i < ScaledNodeCount; i++)
            {
                Scales[i] = new float[1];
                Scales[i][0] = reader.ReadSingle();
            }
        }

        public override void Write(EndianWriter writer)
        {
            base.Write(writer);

            for (var i = 0; i < RotatedNodeCount; i++)
            {
                var rotation = Rotations[i][0].Normalize();

                writer.Write((short)(rotation.I * short.MaxValue));
                writer.Write((short)(rotation.J * short.MaxValue));
                writer.Write((short)(rotation.K * short.MaxValue));
                writer.Write((short)(rotation.W * short.MaxValue));
            }

            for (var i = 0; i < TranslatedNodeCount; i++)
            {
                var translation = Translations[i][0];

                writer.Write(translation.X / 100f);
                writer.Write(translation.Y / 100f);
                writer.Write(translation.Z / 100f);
            }

            for (var i = 0; i < ScaledNodeCount; i++)
                writer.Write(Scales[i][0]);
        }
    }
}
