using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x50)]
    public class CharacterLookProperties : TagStructure
	{
        public RealEulerAngles2d MaximumAimingDeviation; // how far we can turn our weapon (degrees)
        public RealEulerAngles2d MaximumLookingDeviation; // how far we can turn our head (degrees)
        public RealEulerAngles2d RuntimeAimingDeviationCosines;
        public RealEulerAngles2d RuntimeLookingDeviationCosines;
        public Angle NoncombatLookDeltaL; // how far we can turn our head left away from our aiming vector when not in combat (degrees)
        public Angle NoncombatLookDeltaR; // how far we can turn our head right away from our aiming vector when not in combat (degrees)
        public Angle CombatLookDeltaL; // how far we can turn our head left away from our aiming vector when in combat (degrees)
        public Angle CombatLookDeltaR; // how far we can turn our head right away from our aiming vector when in combat (degrees)
        public Bounds<float> NoncombatIdleLooking; // rate at which we change look around randomly when not in combat (seconds)
        public Bounds<float> NoncombatIdleAiming; // rate at which we change aiming directions when looking around randomly when not in combat (seconds)
        public Bounds<float> CombatIdleLooking; // rate at which we change look around randomly when searching or in combat (seconds)
        public Bounds<float> CombatIdleAiming; // rate at which we change aiming directions when looking around randomly when searching or in combat (seconds)
    }
}
