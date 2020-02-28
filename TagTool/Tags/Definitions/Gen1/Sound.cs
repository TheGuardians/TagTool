using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0xA4, MaxVersion = CacheVersion.HaloCustomEdition)]
    public class Sound : TagStructure
    {
        public int Flags;

        public SoundClass SoundClass;
        [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
        public byte[] Unused1;

        public SampleRate SampleRate;
        [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
        public byte[] Unused2;

        public float Distance;
        public float SkipFraction;
        public Bounds<float> RandomPitchBounds;
        public Bounds<Angle> ConeAngleBounds;
        public float OuterConeGain;
        public float GainModifier;
        public float MaxBendPerSecond;

        public float SkipFractionModifier1;
        public float GainModifier1;
        public float PitchModifier1;
        public float SkipFractionModifier2;
        public float GainModifier2;
        public float PitchModifier2;
        public int Encoding;
        public int Compression;

        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown10;
        public int Unknown11;
        public int Unknown12;
        public int Unknown13;
        public int Unknown14;
        public int Unknown15;

        public CachedTag PromotionSound;
        public int PromotionCount;
        public int Unknown5;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int Unknown9;
        public List<int> PitchRanges;
    }
}
