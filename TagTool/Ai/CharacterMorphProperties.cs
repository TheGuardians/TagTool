using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xE4)]
    public class CharacterMorphProperties : TagStructure
	{
        public CachedTag RangedCharacter;
        public CachedTag TankCharacter;
        public CachedTag StealthCharacter;
        public CachedTag MorphMuffins;
        public CachedTag RangedWeapon;
        public CachedTag TankWeapon;
        public CachedTag StealthWeapon;
        public float DistanceDamageOuterRadius; // Considered damaging-outside-range when you START firing from outside this distance
        public float DistanceDamageInnerRadius; // Considered damaging-outside-range when you CONTINUE firing from outside this distance
        public float DistanceDamageTime; // Damaging tank guy from outside-range for this long causes a morph
        public float DistanceDamageResetTime; // Damage timer is reset after this long of not damaging him from outside-range
        public Bounds<float> ThrottleDistance; // Throttle the tank from running (far) to walking (near) across this range of distances. (defaults to 5 and 3)
        public float ProtectDamageAmount; // Once current damage reaches this amount, protect your special parts until no recent damage
        public float ProtectTime; // How long should we protect our special parts for?

        [TagField(Flags = Label)]
        public CachedTag SpewInfectionCharacter; // What character should I throw up all over my target? Carrots?

        public float SpewChance; // Probability of throwing up a bunch of infection forms when perimeterising
        public StringId SpewMarker; // From whence should the infection forms cometh?
        public Bounds<float> SpewFrequency; // Min/max time between spawning each infection form during spew. (defaults to 0.1 and 0.3)
        public float StealthMorphDistanceThreshold; // Morphing inside this range causes a tank guy, outside this range causes a ranged fella
        public float StealthMorphDamageThreshold; // Percentage of body health he has to be taken down in order to cause a morph
        public float StalkRangeMin; // We want to stalk our target from outside this radius
        public float StalkRangeMax; // We want to stalk our target from inside this radius
        public float StalkRangeHardMax; // We will never be able to pick a firing position more than this far from our target
        public float StalkChargeChance; // While stalking, charge randomly with this probability per second (also will charge when on periphery, this is just some spice)
        public float RangedProximityDistance; // Morph to tank/stalker when someone gets this close to me as a ranged form
        public float TurtleDamageThreshold; // amount of damage necessary to trigger a turtle
        public Bounds<float> TurtleTime; // when turtling, turtle for a random time with these bounds
        public float TurtleDistance; // when I turtle I send out a stimulus to friends within this radius to also turtle
        public float TurtleAbortDistance; // when my target get within this range, abort turtling
        public float GroupMorphRange; // Follow the morph of any other form within this distance
    }
}
