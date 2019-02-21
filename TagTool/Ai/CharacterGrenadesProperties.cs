using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x3C)]
    public class CharacterGrenadesProperties : TagStructure
	{
        public int GrenadesFlags;
        [TagField(Flags = TagFieldFlags.Label)]
        public CharacterGrenadeType GrenadeType;
        public CharacterGrenadeTrajectoryType TrajectoryType;
        public int MinimumEnemyCount;
        public float EnemyRadius;
        public float GrenadeIdealVelocity;
        public float GrenadeVelocity;
        public Bounds<float> GrenadeRange;
        public float CollateralDamageRadius;
        public float GrenadeChance;
        public float GrenadeThrowDelay;
        public float GrenadeUncoverChance;
        public float AntiVehicleGrenadeChance;
        public Bounds<short> DroppedGrenadeCountBounds;
        public float DonTDropGrenadesChance;
    }
}
