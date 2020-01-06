using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Audio
{
    public static class AudioUtils
    {
        public static SoundResourceDefinition CreateSoundResourceDefinition(byte[] data)
        {
            return new SoundResourceDefinition
            {
                Data = new TagData(data)
            };
        }

        public static SoundResourceDefinition CreateEmptySoundResourceDefinition()
        {
            return new SoundResourceDefinition
            {
                Data = new TagData()
            };
        }
    }
}
