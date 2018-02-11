using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x7C)]
    public class CharacterChargeProperties
    {
        public CharacterChargeFlags Flags;
        public float MeleeConsiderRange;
        public float MeleeChance;
        public float MeleeAttackRange;
        public float MeleeAbortRange;
        public float MeleeAttackTimeout;
        public float MeleeAttackDelayTimer;
        public Bounds<float> MeleeLeapRange;
        public float MeleeLeapChance;
        public float IdealLeapVelocity;
        public float MaxLeapVelocity;
        public float MeleeLeapBallistic;
        public float MeleeDelayTimer;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        [TagField(Label = true)]
        public CachedTagInstance BerserkWeapon;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;
        public uint Unknown10;
        public List<CharacterChargeDifficultyLimit> DifficultyLimits;
    }
}
