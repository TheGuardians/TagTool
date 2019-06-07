using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Animations.Codecs
{
    public class SharedStaticData : AnimationCodecData
    {
        public override void Read(EndianReader reader)
        {
            base.Read(reader);
        }

        public override void Write(EndianWriter writer)
        {
            base.Write(writer);
        }
    }
}
