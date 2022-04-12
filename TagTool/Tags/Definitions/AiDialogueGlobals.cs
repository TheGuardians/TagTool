using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x4C, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x50, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x5C, MinVersion = CacheVersion.HaloOnlineED)]
    public class AiDialogueGlobals : TagStructure
    {
        public Bounds<float> StrikeDelayBounds;
        public float RemindDelay;
        public float CoverCurseChance;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float PlayerVocalizationStaminaThreshold;

        public List<VocalizationDefinition> Vocalizations;

        public List<VocalizationPattern> Patterns;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] Unused;

        public List<DialogueDatum> DialogueData;

        public List<InvoluntaryDatum> InvoluntaryData;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED)]
        public byte[] Unused_;
        
        [TagStructure(Size = 0x4)]
        public class DialogueDatum : TagStructure
		{
            public short StartIndexPostProcess;
            public short LengthPostProcess;
        }

        [TagStructure(Size = 0x4)]
        public class InvoluntaryDatum : TagStructure
		{
            public short InvoluntaryVocalizationIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;
        }
    }
}
