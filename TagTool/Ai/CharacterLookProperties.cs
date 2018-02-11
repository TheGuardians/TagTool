using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x50)]
    public class CharacterLookProperties
    {
        public RealEulerAngles2d MaximumAimingDeviation;
        public RealEulerAngles2d MaximumLookingDeviation;
        public RealEulerAngles2d RuntimeAimingDeviationCosines;
        public RealEulerAngles2d RuntimeLookingDeviationCosines;
        public Angle NoncombatLookDeltaLeft;
        public Angle NoncombatLookDeltaRight;
        public Angle CombatLookDeltaLeft;
        public Angle CombatLookDeltaRight;
        public Bounds<float> NoncombatIdleLooking;
        public Bounds<float> NoncombatIdleAiming;
        public Bounds<float> CombatIdleLooking;
        public Bounds<float> CombatIdleAiming;
    }
}
