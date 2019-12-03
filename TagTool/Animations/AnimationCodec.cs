using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Animations
{
    public abstract class AnimationCodec
    {
        public virtual byte[] ReadKeyframes(EndianReader reader) => null;

        public abstract IEnumerable<RealQuaternion> ReadRotations(EndianReader reader, int count);
        public abstract IEnumerable<RealPoint3d> RealTranslations(EndianReader reader, int count);
        public abstract IEnumerable<float> RealScales(EndianReader reader, int count);
    }
}
