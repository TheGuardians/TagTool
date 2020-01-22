using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x80)]
    public class CharacterVitalityProperties : TagStructure
	{
        public CharacterVitalityFlags Flags;
        public float NormalBodyVitality;
        public float NormalShieldVitality;
        public float LegendaryBodyVitality;
        public float LegendaryShieldVitality;
        public float BodyRechargeFraction;
        public float SoftPingShieldThreshold;
        public float SoftPingNoshieldThreshold;
        public float SoftPingCooldownTime;
        public float HardPingShieldThreshold;
        public float HardPingNoShieldThreshold;
        public float HardPingCooldownTime;
        public float CurrentDamageDecayDelay;
        public float CurrentDamageDecayTime;
        public float RecentDamageDecayDelay;
        public float RecentDamageDecayTime;
        public float BodyRechargeDelayTime;
        public float BodyRechargeTime;
        public float ShieldRechargeDelayTime;
        public float ShieldRechargeTime;
        public float StunThreshold;
        public Bounds<float> StunTimeBounds;
        public float ExtendedShieldDamageThreshold;
        public float ExtendedBodyDamageThreshold;
        public float SuicideRadius;
        public float RuntimeBodyRechargeVelocity;
        public float RuntimeShieldRechargeVelocity;
        public CachedTag ResurrectWeapon;
    }
}
