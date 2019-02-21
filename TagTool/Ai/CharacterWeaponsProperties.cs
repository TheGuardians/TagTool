using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xE0)]
    public class CharacterWeaponsProperties : TagStructure
	{
        public CharacterWeaponFlags Flags;
        [TagField(Flags = TagFieldFlags.Label)]
        public CachedTagInstance Weapon;
        public float MaximumFiringRange;
        public float MinimumFiringRange;
        public Bounds<float> NormalCombatRange;
        public float BombardmentRange;
        public float MaxSpecialTargetDistance;
        public Bounds<float> TimidCombatRange;
        public Bounds<float> AggressiveCombatRange;
        public float SuperBallisticRange;
        public Bounds<float> BallisticFiringBounds;
        public Bounds<float> BallisticFractionBounds;
        public Bounds<float> FirstBurstDelayTimeBounds;
        public float SurpriseDelayTime;
        public float SurpriseFireWildlyTime;
        public float DeathFireWildlyChance;
        public float DeathFireWildlyTime;
        public RealVector3d CustomStandGunOffset;
        public RealVector3d CustomCrouchGunOffset;
        public CharacterWeaponSpecialFireMode SpecialFireMode;
        public CharacterWeaponSpecialFireSituation SpecialFireSituation;
        public float SpecialFireChance;
        public float SpecialFireDelay;
        public float SpecialDamageModifier;
        public Angle SpecialProjectileError;
        public Bounds<float> DropWeaponLoadedBounds;
        public Bounds<short> DropWeaponAmmoBounds;
        public Bounds<float> NormalAccuracyBounds;
        public float NormalAccuracyTime;
        public Bounds<float> HeroicAccuracyBounds;
        public float HeroicAccuracyTime;
        public Bounds<float> LegendaryAccuracyBounds;
        public float LegendaryAccuracyTime;
        public List<CharacterFiringPattern> FiringPatterns;
        public CachedTagInstance WeaponMeleeDamage;
    }
}
