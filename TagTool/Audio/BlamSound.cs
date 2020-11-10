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
        public uint FirstSample;
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

        public BlamSound(SampleRate sampleRate, EncodingValue encoding, Compression compression, uint sampleCount, byte[] data, int bitsPerSample)
        {
            SampleRate = sampleRate;
            Encoding = encoding;
            Compression = compression;
            SampleCount = sampleCount;
            Data = data;
            RealPermutationIndex = 0;
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

            if(compression == Compression.PCM)
            {
                int channelCount;

                switch (Encoding)
                {
                    case EncodingValue.Mono:
                        channelCount = 1;
                        break;
                    case EncodingValue.Stereo:
                        channelCount = 2;
                        break;
                    case EncodingValue.Surround:
                        channelCount = 4;
                        break;
                    case EncodingValue._51Surround:
                        channelCount = 6;
                        break;
                    default:
                        channelCount = 2;
                        break;
                }
                var dataLength = data.Length;


                uint newSampleCount = (uint)(((dataLength * 8) / 16) / channelCount);
                SampleCount = newSampleCount;

            }

        }
    }
}
