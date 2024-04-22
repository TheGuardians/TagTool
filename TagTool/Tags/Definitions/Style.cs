using TagTool.Cache;
using System.Collections.Generic;
using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "style", Tag = "styl", Size = 0x5C, MinVersion = CacheVersion.Halo3Retail)]
    public class Style : TagStructure
	{
        [TagField(Length = 32)]
        public string Name;

        public CombatStatusDecayOptionsValue CombatStatusDecayOptions;
        public StyleAttitudeEnum Attitude;
        public StyleControlFlags StyleControl;

        [TagField(Length = 7)]
        public int[] Behaviors = new int[7];

        public List<SpecialMovementBlock> SpecialMovement;
        public List<BehaviorListBlock> BehaviorList;

        public enum CombatStatusDecayOptionsValue : short
        {
            LatchAtIdle,
            LatchAtAlert,
            LatchAtCombat
        }

        public enum StyleAttitudeEnum : short
        {
            Normal,
            Timid,
            Aggressive
        }

        [Flags]
        public enum StyleControlFlags : int
        {
            None = 0,
            NewBehaviorsDefaultToOn = 1 << 0,
            DoNotForceOnDefaultBehaviors = 1 << 1
        }

        [TagStructure(Size = 0x4)]
        public class SpecialMovementBlock : TagStructure
		{
            public SpecialMovementFlags SpecialMovement1;

            [Flags]
            public enum SpecialMovementFlags : uint
            {
                None,
                Jump = 1 << 0,
                Climb = 1 << 1,
                Vault = 1 << 2,
                Mount = 1 << 3,
                Hoist = 1 << 4,
                WallJump = 1 << 5,
                Na = 1 << 6,
                Rail = 1 << 7,
                Seam = 1 << 8,
                Door = 1 << 9
            }
        }

        [TagStructure(Size = 0x20)]
        public class BehaviorListBlock : TagStructure
		{
            [TagField(Length = 32)]
            public string BehaviorName;
        }
    }
}