using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x20)]
    public class TagMapping : TagStructure
	{
        public VariableTypeValue InputVariable;
        public VariableTypeValue RangeVariable;
        public OutputModifierValue OutputModifier;
        public VariableTypeValue OutputModifierInput;

        public TagFunction Function = new TagFunction { Data = new byte[0] };

        public float RuntimeMConstantValue;
        public EditablePropertiesFlags RuntimeMFlags;

        [TagField(Flags = Padding, Length = 3)]
        public byte[] Unused = new byte[3];

        public enum VariableTypeValue : sbyte
        {
            ParticleAge,
            EmitterAge,
            ParticleRandom,
            EmitterRandom,
            ParticleRandom1,
            ParticleRandom2,
            ParticleRandom3,
            ParticleRandom4,
            EmitterRandom1,
            EmitterRandom2,
            EmitterTime,
            SystemLod,
            GameTime,
            EffectAScale,
            EffectBScale,
            PhysicsRotation,
            LocationRandom,
            DistanceFromEmitter,
            ParticleSimulationA,
            ParticleSimulationB,
            ParticleVelocity,
            Invalid
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
            Bit0 = 1 << 0,
            Bit1 = 1 << 1,
            Bit2 = 1 << 2,
            Bit3 = 1 << 3,
            Bit4 = 1 << 4,
            IsConstant = 1 << 5,
            Bit6 = 1 << 6,
            ConstantOverTime = 1 << 7
        }

        [Flags]
        public enum ForceFlagsValue : byte
        {
            None = 0,
            ForceConstant = 1 << 0
        }
    }
}
