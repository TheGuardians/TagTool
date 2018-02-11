using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x18)]
    public class AiMissionDialogueLineVariant
    {
        [TagField(Label = true)]
        public StringId Designation;
        public CachedTagInstance Sound;
        public StringId SoundEffect;
    }
}
