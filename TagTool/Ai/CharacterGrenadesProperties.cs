using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x3C)]
    public class CharacterGrenadesProperties : TagStructure
	{
        public int GrenadesFlags;
        [TagField(Label = true)]
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
