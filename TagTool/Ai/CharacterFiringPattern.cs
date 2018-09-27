using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x40)]
    public class CharacterFiringPattern : TagStructure
	{
        public float RateOfFire;
        public float TargetTracking;
        public float TargetLeading;
        public float BurstOriginRadius;
        public Angle BurstOriginAngle;
        public Bounds<float> BurstReturnLengthBounds;
        public Angle BurstReturnAngle;
        public Bounds<float> BurstDurationBounds;
        public Bounds<float> BurstSeparationBounds;
        public float WeaponDamageModifier;
        public Angle ProjectileError;
        public Angle BurstAngularVelocity;
        public Angle MaximumErrorAngle;
    }
}
