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

        public static string GetFormtFileExtension(Compression format)
        {
            switch (format)
            {
                case Compression.XMA:
                    return "xma";
                case Compression.OGG:
                    return "ogg";
                case Compression.Tagtool_WAV:
                case Compression.PCM: // should really be 'raw', but leaving for now
                    return "wav";
                case Compression.MP3:
                    return "mp3";
                case Compression.FSB4:
                    return "fsb";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
