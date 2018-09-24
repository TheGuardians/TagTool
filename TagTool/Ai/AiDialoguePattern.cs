using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x34)]
    public class AiDialoguePattern : TagStructure
    {
        public AiDialogueType DialogueType;
        public short VocalizationsIndex;
        [TagField(Label = true)]
        public StringId VocalizationName;
        public AiDialogueSpeakerType SpeakerType;
        public AiDialogueSpeakerType ListenerType;
        public AiDialoguePatternHostility Hostility;
        public AiDialoguePatternFlags Flags;
        public AiActorType CauseActorType;
        public short CauseType;
        public StringId CauseAiTypeName;
        public uint Unknown3;
        public short Unknown4;
        public short Unknown5;
        public short Attitude;
        public short Unknown6;
        public uint Conditions;
        public short SpacialRelationship;
        public short DamageType;
        public short Unknown7;
        public short SubjectType;
        public StringId SubjectAiTypeName;
    }
}
