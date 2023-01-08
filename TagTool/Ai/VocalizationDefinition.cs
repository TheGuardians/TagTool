using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x5C, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x60, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x64, MinVersion = CacheVersion.HaloReach)]
    public class VocalizationDefinition : TagStructure
	{
        [TagField(Flags = Label)]
        public StringId Vocalization;
        public short ParentIndex;
        public VocalizationPriority Priority;
        public VocalizationFlags Flags;
        public AiGlanceBehavior GlanceBehavior; // how does the speaker of this vocalization direct his gaze?
        public AiGlanceBehavior GlanceRecipientBehavior; // how does someone who hears me behave?
        public AiDialoguePerception PerceptionType;
        public AiCombatStatus MaxCombatStatus;
        public AiAnimationImpulse AnimationImpulse;
        public short ProxyDialogueIndex; 
        public float SoundRepetitionDelay; // Minimum delay in minutes between playing the same permutation (minutes)
        public float AllowableQueueDelay; // How long to wait to actually start the vocalization (seconds)
        public float PreVocalizationDelay; // How long to wait to actually start the vocalization (seconds)
        public float NotificationDelay; // How long into the vocalization the AI should be notified (seconds)
        public float PostVocalizationDelay; // How long speech is suppressed in the speaking unit after vocalizing (seconds)
        public float RepeatDelay; // How long before the same vocalization can be repeated (seconds)
        public float Weight; // Inherent weight of this vocalization
        public float SpeakerFreezeTime; // speaker won't move for the given amount of time
        public float ListenerFreezeTime; // listener won't move for the given amount of time (from start of vocalization)
        public AiEmotion SpeakerEmotion;
        public AiEmotion ListenerEmotion;
        public float PlayerSpeakerSkipFraction;
        public float PlayerSkipFraction;
        public float FloodSkipFraction;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float SkipFraction;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short MissionMinValue; // The lowest mission id that we play this line in
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short MissionMaxValue; // The highest mission id that we play this line in

        public StringId SampleLine;
        public List<AiVocalizationResponse> Responses;
    }
}
