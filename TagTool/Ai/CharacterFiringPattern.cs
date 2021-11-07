using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x40)]
    public class CharacterFiringPattern : TagStructure
	{
        public float RateOfFire; // how many times per second we pull the trigger (zero = continuously held down)
        public float TargetTracking; // how well our bursts track moving targets. 0.0= fire at the position they were standing when we started the burst. 1.0= fire at current position [0-1]
        public float TargetLeading; // how much we lead moving targets. 0.0= no prediction. 1.0= predict completely [0-1]
        public float BurstOriginRadius; // how far away from the target the starting point is (world units)
        public Angle BurstOriginAngle; // the range from the horizontal that our starting error can be
        public Bounds<float> BurstReturnLengthBounds; // how far the burst point moves back towards the target (could be negative) (world units)
        public Angle BurstReturnAngle; // the range from the horizontal that the return direction can be
        public Bounds<float> BurstDurationBounds; // how long each burst we fire is
        public Bounds<float> BurstSeparationBounds; // how long we wait between bursts
        public float WeaponDamageModifier; // what fraction of its normal damage our weapon inflicts (zero = no modifier)
        public Angle ProjectileError; // error added to every projectile we fire
        public Angle BurstAngularVelocity; // the maximum rate at which we can sweep our fire (zero = unlimited) (degrees per second)
        public Angle MaximumErrorAngle; // cap on the maximum angle by which we will miss target (restriction on burst origin radius
    }
}
