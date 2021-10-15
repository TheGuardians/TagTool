using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x80)]
    public class CharacterVitalityProperties : TagStructure
	{
        public CharacterVitalityFlags Flags;
        public float NormalBodyVitality; // maximum body vitality of our unit
        public float NormalShieldVitality; // maximum shield vitality of our unit
        public float LegendaryBodyVitality; // maximum body vitality of our unit (on legendary)
        public float LegendaryShieldVitality; // maximum shield vitality of our unit (on legendary)
        public float BodyRechargeFraction; // fraction of body health that can be regained after damage
        public float SoftPingShieldedThreshold; // damage necessary to trigger a soft ping when shields are up
        public float SoftPingUnshieldedThreshold; // damage necessary to trigger a soft ping when shields are down
        public float SoftPingMinInterruptTime; // minimum time before a soft ping can be interrupted
        public float HardPingShieldedThreshold; // damage necessary to trigger a hard ping when shields are up
        public float HardPingUnshieldedThreshold; // damage necessary to trigger a hard ping when shields are down
        public float HardPingMinInterruptTime; // minimum time before a hard ping can be interrupted
        public float CurrentDamageDecayDelay; // current damage begins to fall after a time delay has passed since last the damage (MAX 4.1, because timer is stored in a char so 127 ticks maximum)
        public float CurrentDamageDecayTime; // amount of time it would take for 100% current damage to decay to 0
        public float RecentDamageDecayDelay; // recent damage begins to fall after a time delay has passed since last the damage (MAX 4.1, because timer is stored in a char so 127 ticks maximum)
        public float RecentDamageDecayTime; // amount of time it would take for 100% recent damage to decay to 0
        public float BodyRechargeDelayTime; // amount of time delay before a shield begins to recharge
        public float BodyRechargeTime; // amount of time for shields to recharge completely
        public float ShieldRechargeDelayTime; // amount of time delay before a shield begins to recharge
        public float ShieldRechargeTime; // amount of time for shields to recharge completely
        public float StunThreshold; // stun level that triggers the stunned state (currently, the 'stunned' behavior)
        public Bounds<float> StunTimeBounds; // seconds
        public float ExtendedShieldDamageThreshold; // Amount of shield damage sustained before it is considered 'extended' (percent)
        public float ExtendedBodyDamageThreshold; // Amount of body damage sustained before it is considered 'extended' (percent)
        public float SuicideRadius; // when I die and explode, I damage stuff within this distance of me.
        public float RuntimeBodyRechargeVelocity;
        public float RuntimeShieldRechargeVelocity;
        public CachedTag ResurrectWeapon; // If I'm being automatically resurrected then I pull out a ...
    }
}
