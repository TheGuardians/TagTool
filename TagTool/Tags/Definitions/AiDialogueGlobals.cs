using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x4C, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x50, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x5C, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x7C, MinVersion = CacheVersion.HaloReach)]
    public class AiDialogueGlobals : TagStructure
    {
        public Bounds<float> StrikeDelayBounds;
        public float RemindDelay;
        public float CoverCurseChance;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public float PlayerVocalizationStaminaThreshold;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float PlayerLookMaxDistance; // defaults to 10 wu (wu)
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float PlayerLook; // defaults to 3 secs (secs)
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float PlayerLookLongTime; // defaults to 15 secs (secs)
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float SpartanNearbySearchDistance; // defaults to 7 wu (wu)
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float FaceFriendlyPlayerDistance; // 0: disable facing behavior (wu)
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId SpaceDialogueEffect; // used for dialog lines started by a pattern with "speaker in space" set

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<DefaultStimulusSuppressor> DefaultStimulusSuppressors;

        public List<VocalizationDefinition> Vocalizations;
        public List<VocalizationPattern> Patterns;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] Padding0;

        public List<DialogueDatum> DialogueData;
        public List<InvoluntaryDatum> InvoluntaryData;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<PredictedDataBlock> PredictedVocalizations;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding1;
        
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
            public byte[] Padding0;
        }

        [TagStructure(Size = 0x4)]
        public class DefaultStimulusSuppressor : TagStructure
        {
            public StringId Stimulus;
        }

        [TagStructure(Size = 0x4)]
        public class PredictedDataBlock : TagStructure
        {
            public int PredictedVocalizationIndex;
        }
    }
}
