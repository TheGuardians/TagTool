using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x5C, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x60, MinVersion = CacheVersion.Halo3ODST)]
    public class AiVocalization : TagStructure
	{
        [TagField(Flags = TagFieldFlags.Label)]
        public StringId Name;
        public short ParentIndex;
        public AiVocalizationPriority Priority;
        public AiVocalizationFlags Flags;
        public AiGlanceBehavior GlanceBehavior;
        public AiGlanceBehavior GlanceRecipient;
        public AiDialoguePerception PerceptionType;
        public AiCombatStatus MaxCombatStatus;
        public AiAnimationImpulse AnimationImpulse;
        public short ProxyDialogueIndex;
        public float SoundRepetitionDelay;
        public float AllowableQueueDelay;
        public float PreVocalizationDelay;
        public float NotificationDelay;
        public float PostVocalizationDelay;
        public float RepeatDelay;
        public float Weight;
        public float SpeakerFreezeTime;
        public float ListenerFreezeTime;
        public AiEmotion SpeakerEmotion;
        public AiEmotion ListenerEmotion;
        public float PlayerSpeakerSkipFraction;
        public float PlayerSkipFraction;
        public float FloodSkipFraction;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float SkipFraction;
        public StringId SampleLine;
        public List<AiVocalizationResponse> Responses;
    }
}
