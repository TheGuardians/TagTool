using System.Collections.Generic;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Animations.Codecs
{
    public class UncompressedStaticCodec : AnimationCodec
    {
        public override IEnumerable<RealQuaternion> ReadRotations(EndianReader reader, int count)
        {
            for (var i = 0; i < count; i++)
                yield return new RealQuaternion(
                    (float)reader.ReadInt16() / short.MaxValue,
                    (float)reader.ReadInt16() / short.MaxValue,
                    (float)reader.ReadInt16() / short.MaxValue,
                    (float)reader.ReadInt16() / short.MaxValue);
        }

        public override IEnumerable<RealPoint3d> RealTranslations(EndianReader reader, int count)
        {
            for (var i = 0; i < count; i++)
                yield return new RealPoint3d(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle());
        }

        public override IEnumerable<float> RealScales(EndianReader reader, int count)
        {
            for (var i = 0; i < count; i++)
                yield return reader.ReadSingle();
        }
    }
}
