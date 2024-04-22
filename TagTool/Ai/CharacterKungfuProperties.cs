using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8)]
    public class CharacterKungfuProperties : TagStructure
    {
        public float KungfuOverrideDistance; // If the player is within this distance, open fire, even if your task is kungfu-fight disallowed (wus)
        public float KungfuCoverDangerThreshold; // If you are kungfu disallowed and your danger is above this level, take cover (wus)
    }
}