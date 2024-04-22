using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x7C)]
    public class CharacterChargeProperties : TagStructure
	{
        public CharacterChargeFlags Flags;
        public float MeleeConsiderRange;
        public float MeleeChance; // chance of initiating a melee within a 1 second period
        public float MeleeAttackRange;
        public float MeleeAbortRange;
        public float MeleeAttackTimeout; // Give up after given amount of time spent charging (seconds)
        public float MeleeAttackDelayTimer; // don't attempt again before given time since last melee (seconds)
        public Bounds<float> MeleeLeapRange;
        public float MeleeLeapChance;
        public float IdealLeapVelocity;
        public float MaxLeapVelocity;
        public float MeleeLeapBallistic;
        public float MeleeDelayTimer; // time between melee leaps (seconds)
        public float LeaderAbandonedBerserkChance; // chance for a leader to berserk when all his followers die (actually charge, NOT berserk, but I'm not changing the name of the variable)
        public Bounds<float> ShieldDownBerserkChance; // lower bound is chance to berserk at max range, upper bound is chance to berserk at min range, requires shield depleted berserk impulse
        public Bounds<float> ShielddownBerserkRanges;

        [TagField(Flags = Label)]
        public CachedTag BerserkWeapon; // when I berserk, I pull out a ...

        public float BeserkCooldown; // Time that I will stay in beserk after losing my target, and then revert back to normal (seconds)
        public float BrokenKamikazeChance; // Probability that I will run the kamikaze behaviour when my leader dies.
        public float PerimeterRange; // How far we will melee charge outside our firing points before starting perimeter (defaults to 5wu)
        public float PerimeterRangeClose; // How far we will melee charge outside our firing points before starting perimeter when the target is close to me (within 3wu) (defaults to 9wu)
        public float PerimeterDamageTimeout; // How long will we take damage from our target before either seeking cover or berserking (defaults to 3secs)
        public List<CharacterChargeDifficultyLimit> DifficultyLimits;
    }
}
