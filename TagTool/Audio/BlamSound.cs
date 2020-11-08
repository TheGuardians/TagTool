using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Audio
{
    public class BlamSound
    {
        public SampleRate SampleRate;
        public EncodingValue Encoding;
        public Compression Compression;
        public uint SampleCount;
        public int RealPermutationIndex;
        public byte[] Data;

        public BlamSound(Sound sound, int permutationGestaltIndex, byte[] data,  CacheVersion version, SoundCacheFileGestalt soundGestalt = null)
        {
            switch (version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                case CacheVersion.HaloReach:
                    InitGen3Sound(sound, soundGestalt, permutationGestaltIndex, data);
                    break;
                default:
                    throw new Exception($"Unsupported cache version {version}");
            }
        }

        private void InitGen3Sound(Sound sound, SoundCacheFileGestalt soundGestalt, int permutationGestaltIndex, byte[] data)
        {
            var platformCodec = soundGestalt.PlatformCodecs[sound.SoundReference.PlatformCodecIndex];
            var permutation = soundGestalt.Permutations[permutationGestaltIndex];
            Encoding = platformCodec.Encoding;
            SampleRate = platformCodec.SampleRate;
            SampleCount = permutation.SampleCount;
            RealPermutationIndex = permutation.OverallPermutationIndex;
            UpdateFormat(platformCodec.Compression, data);
        }

        public void UpdateFormat(Compression compression, byte[] data)
        {
            Compression = compression;
            Data = data;
        }
    }
}
