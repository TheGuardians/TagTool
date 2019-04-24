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
        public byte RuntimeMFlags;

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
        public enum ForceFlagsValue : byte
        {
            None = 0,
            ForceConstant = 1 << 0
        }
    }
}
