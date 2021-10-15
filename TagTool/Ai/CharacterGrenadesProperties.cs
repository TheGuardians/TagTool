using System;
using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x3C)]
    public class CharacterGrenadesProperties : TagStructure
	{
        public GenericFlags GrenadesFlags;
        [TagField(Flags = Label)]
        public CharacterGrenadeType GrenadeType; // type of grenades that we throw^
        public CharacterGrenadeTrajectoryType TrajectoryType; // how we throw our grenades

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;

        public short MinimumEnemyCount; // how many enemies must be within the radius of the grenade before we will consider throwing there
        public float EnemyRadius; // we consider enemies within this radius when determining where to throw (world units)
        public float GrenadeIdealVelocity; // how fast we LIKE to throw our grenades (world units per second)
        public float GrenadeVelocity; // the fastest we can possibly throw our grenades (world units per second)
        public Bounds<float> GrenadeRange; // ranges within which we will consider throwing a grenade (world units)
        public float CollateralDamageRadius; // we won't throw if there are friendlies around our target within this range (world units)
        public float GrenadeChance; // how likely we are to throw a grenade in one second [0,1]
        public float GrenadeThrowCooldown; // How long we have to wait after throwing a grenade before we can throw another one (seconds)
        public float GrenadeUncoverChance; // how likely we are to throw a grenade to flush out a target in one second [0,1]
        public float AntiVehicleGrenadeChance; // how likely we are to throw a grenade against a vehicle [0,1]
        public Bounds<short> GrenadeCount; // number of grenades that we start with
        public float NoGrenadesDroppedChance; // how likely we are not to drop any grenades when we die, even if we still have some [0,1]

        [Flags]
        public enum GenericFlags : uint
        {
            Flag1 = 1 << 0
        }
    }
}
