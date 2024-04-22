using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xE0)]
    public class CharacterWeaponsProperties : TagStructure
	{
        public CharacterWeaponFlags Flags;

        [TagField(Flags = Label)]
        public CachedTag Weapon;

        public float MaximumFiringRange; // we can only fire our weapon at targets within this distance (world units)
        public float MinimumFiringRange; // weapon will not be fired at target closer than given distance (world units)
        public Bounds<float> NormalCombatRange; // (world units)
        public float BombardmentRange; // we offset our burst targets randomly by this range when firing at non-visible enemies (zero = never)
        public float MaxSpecialTargetDistance; // Specific target regions on a vehicle or unit will be fired upon only under the given distance
        public Bounds<float> TimidCombatRange; // (world units)
        public Bounds<float> AggressiveCombatRange; // (world units)
        public float SuperBallisticRange; // we try to aim our shots super-ballistically if target is outside this range (zero = never)
        public Bounds<float> BallisticFiringBounds; // At the min range, the min ballistic fraction is used, at the max, the max ballistic fraction is used (world units)
        public Bounds<float> BallisticFractionBounds; // Controls speed and degree of arc. 0 = high, slow, 1 = low, fast [0-1]
        public Bounds<float> FirstBurstDelayTimeBounds; // (seconds)
        public float SurpriseDelayTime; // (seconds)
        public float SurpriseFireWildlyTime; // (seconds)
        public float DeathFireWildlyChance; // [0-1]
        public float DeathFireWildlyTime; // (seconds)
        public RealVector3d CustomStandGunOffset; // custom standing gun offset for overriding the default in the base actor
        public RealVector3d CustomCrouchGunOffset; // custom crouching gun offset for overriding the default in the base actor
        public CharacterWeaponSpecialFireMode SpecialFireMode; // the type of special weapon fire that we can use
        public CharacterWeaponSpecialFireSituation SpecialFireSituation; // when we will decide to use our special weapon fire mode
        public float SpecialFireChance; // how likely we are to use our special weapon fire mode [0,1]
        public float SpecialFireDelay; // how long we must wait between uses of our special weapon fire mode (seconds)
        public float SpecialDamageModifier; // damage modifier for special weapon fire (applied in addition to the normal damage modifier. zero = no change) [0,1]
        public Angle SpecialProjectileError; // projectile error angle for special weapon fire (applied in addition to the normal error)
        public Bounds<float> DropWeaponLoadedBounds; // amount of ammo loaded into the weapon that we drop (in fractions of a clip, e.g. 0.3 to 0.5)
        public Bounds<short> DropWeaponAmmoBounds; // total number of rounds in the weapon that we drop (ignored for energy weapons)
        public Bounds<float> NormalAccuracyBounds; // indicates starting and ending accuracies at normal difficulty
        public float NormalAccuracyTime; // the amount of time it takes the accuracy to go from starting to ending
        public Bounds<float> HeroicAccuracyBounds; // indicates starting and ending accuracies at heroic difficulty
        public float HeroicAccuracyTime; // the amount of time it takes the accuracy to go from starting to ending
        public Bounds<float> LegendaryAccuracyBounds; // indicates starting and ending accuracies at legendary difficulty
        public float LegendaryAccuracyTime; // the amount of time it takes the accuracy to go from starting to ending
        public List<CharacterFiringPattern> FiringPatterns;
        public CachedTag WeaponMeleeDamage;
    }
}
