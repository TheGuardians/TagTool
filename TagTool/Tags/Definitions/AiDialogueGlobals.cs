using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x4C, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x50, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x5C, MinVersion = CacheVersion.HaloOnline106708)]
    public class AiDialogueGlobals : TagStructure
    {
        public Bounds<float> StrikeDelayBounds;
        public float RemindDelay;
        public float CoverCurseChance;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float FaceFriendlyPlayerDistance;

        public List<AiVocalization> Vocalizations;

        public List<AiDialoguePattern> Patterns;

        [TagField(Padding = true, Length = 12)]
        public byte[] Unused1;

        public List<DialogDatum> DialogData;

        public List<InvoluntaryDatum> InvoluntaryData;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused2;
        
        [TagStructure(Size = 0x4)]
        public class DialogDatum : TagStructure
		{
            public short StartIndex;
            public short Length;
        }

        [TagStructure(Size = 0x4)]
        public class InvoluntaryDatum : TagStructure
		{
            public short InvoluntaryVocalizationIndex;

            [TagField(Padding = true, Length = 2)]
            public byte[] Unused;
        }
    }
}
