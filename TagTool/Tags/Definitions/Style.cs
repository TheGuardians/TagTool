using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "style", Tag = "styl", Size = 0x5C, MinVersion = CacheVersion.Halo3Retail)]
    public class Style
    {
        [TagField(Length = 32)]
        public string Name;

        public CombatStatusDecayOptionsValue CombatStatusDecayOptions;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused = new byte[2];

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

        [Flags]
        public enum StyleControlFlags : int
        {
            None = 0,
            NewBehaviorsDefaultToOn = 1 << 0,
            DoNotForceOnDefaultBehaviors = 1 << 1
        }

        [TagStructure(Size = 0x4)]
        public class SpecialMovementBlock
        {
            public SpecialMovementFlags SpecialMovement1;

            [Flags]
            public enum SpecialMovementFlags : int
            {
                None = 0,
                Jump = 1 << 0,
                Climb = 1 << 1,
                Vault = 1 << 2,
                Mount = 1 << 3,
                Hoist = 1 << 4,
                WallJump = 1 << 5
            }
        }

        [TagStructure(Size = 0x20)]
        public class BehaviorListBlock
        {
            [TagField(Length = 32)]
            public string BehaviorName;
        }
    }
}