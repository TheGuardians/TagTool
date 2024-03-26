using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x20)]
    public class ParticlePropertyScalar : TagStructure
	{
        public ParticleStates InputVariable;
        public ParticleStates RangeVariable;
        public OutputModifierValue OutputModifier;
        public ParticleStates OutputModifierInput;

        public TagFunction Function = new TagFunction { Data = new byte[0] };

        public float RuntimeMConstantValue;

        [TagField(MaxVersion = Cache.CacheVersion.HaloOnline700123)]
        public EditablePropertiesFlags RuntimeMFlags;
        [TagField(Flags = Padding, Length = 3, MaxVersion = Cache.CacheVersion.HaloOnline700123)]
        public byte[] Padding1 = new byte[3];

        [TagField(MinVersion = Cache.CacheVersion.HaloReach)]
        public ushort RuntimeMFlagsReach;
        [TagField(MinVersion = Cache.CacheVersion.HaloReach)]
        public ForceFlagsValue ForceFlags;

        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding, MinVersion = Cache.CacheVersion.HaloReach)]
        public byte[] Padding2;

        public enum ParticleStates : byte
        {
            ParticleAge,
            SystemAgeEmitterAge,
            ParticleRandom,
            SystemRandomEmitterRandom,
            ParticleCorrelationRandom1,
            ParticleCorrelationRandom2,
            ParticleCorrelationRandom3,
            ParticleCorrelationRandom4,
            SystemCorrelationEmitterRandom1,
            SystemCorrelationEmitterRandom2,
            ParticleEmissionTimeEmitter,
            LocationLodSystemLod,
            GameTime,
            EffectAScale,
            EffectBScale,
            ParticleRotationPhysicsRotation,
            LocationRandom,
            DistanceFromEmitter,
            ExplosionAnimation,
            ExplosionRotation,
            Velocity,
            Random5,
            Random6,
            Random7,
            Random8,
            SystemRandom3,
            SystemRandom4,
            Invalid = 0xFF
        }

        [Flags]
        public enum ParticleStatesFlags : uint
        {
            None = 0,
            Age = 1 << 0,
            SystemAge = 1 << 1,
            RandomSeed = 1 << 2,
            SystemRandomSeed = 1 << 3,
            Random1 = 1 << 4,
            Random2 = 1 << 5,
            Random3 = 1 << 6,
            Random4 = 1 << 7,
            SystemRandom1 = 1 << 8,
            SystemRandom2 = 1 << 9,
            SystemTime = 1 << 10,
            SystemLod = 1 << 11,
            GameTime = 1 << 12,
            EffectAScale = 1 << 13,
            EffectBScale = 1 << 14,
            PhysicsRotation = 1 << 15,
            LocationRandom = 1 << 16,
            DistanceFromEmitter = 1 << 17,
            SimulationA = 1 << 18,
            SimulationB = 1 << 19,
            Velocity = 1 << 20,
            Random5 = 1 << 21,
            Random6 = 1 << 22,
            Random7 = 1 << 23,
            Random8 = 1 << 24,
            SystemRandom3 = 1 << 25,
            SystemRandom4 = 1 << 26,
        }

        public enum OutputModifierValue : sbyte
        {
            None,
            Plus,
            Times
        }

        [Flags]
        public enum EditablePropertiesFlags : byte
        {
            None = 0,
            RealPoint2dType = 1 << 0,
            RealPoint3dType = 1 << 1,
            RealVector3dType = 1 << 2,
            Bit3 = 1 << 3, // used in black_point check, enables an emitter flag
            Bit4 = 1 << 4,
            IsConstant = 1 << 5,
            ConstantPerEntity = 1 << 6,
            ConstantOverTime = 1 << 7
        }

        [Flags]
        public enum ForceFlagsValue : byte
        {
            None = 0,
            ForceConstant = 1 << 0
        }

        public bool IsRealPoint2d() => RuntimeMFlags.HasFlag(EditablePropertiesFlags.RealPoint2dType);
        public bool IsRealPoint3d() => RuntimeMFlags.HasFlag(EditablePropertiesFlags.RealPoint3dType);
        public bool IsRealVector3d() => RuntimeMFlags.HasFlag(EditablePropertiesFlags.RealVector3dType);
        public bool IsConstant() => RuntimeMFlags.HasFlag(EditablePropertiesFlags.IsConstant);
        public bool IsConstantPerParticle() => RuntimeMFlags.HasFlag(EditablePropertiesFlags.ConstantPerEntity);
        public bool IsConstantOverTime() => RuntimeMFlags.HasFlag(EditablePropertiesFlags.ConstantOverTime);
    }
}
