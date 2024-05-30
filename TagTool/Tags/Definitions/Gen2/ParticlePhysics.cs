using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;
using static TagTool.Tags.Definitions.Gen2.Effect.EffectEventBlock.ParticleSystemDefinitionBlockNew.ParticleSystemEmitterDefinitionBlock;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "particle_physics", Tag = "pmov", Size = 0x14)]
    public class ParticlePhysics : TagStructure
    {
        [TagField(ValidTags = new [] { "pmov" })]
        public CachedTag Template;
        public FlagsValue Flags;
        public List<ParticleController> Movements;
        
        [Flags]
        public enum FlagsValue : uint
        {
            Physics = 1 << 0,
            CollideWithStructure = 1 << 1,
            CollideWithMedia = 1 << 2,
            CollideWithScenery = 1 << 3,
            CollideWithVehicles = 1 << 4,
            CollideWithBipeds = 1 << 5,
            Swarm = 1 << 6,
            Wind = 1 << 7
        }
        
        [TagStructure(Size = 0x14)]
        public class ParticleController : TagStructure
        {
            public TypeValue Type;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<ParticleControllerParameters> Parameters;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            public enum TypeValue : short
            {
                Physics,
                Collider,
                Swarm,
                Wind
            }
            
            [TagStructure(Size = 0x14)]
            public class ParticleControllerParameters : TagStructure
            {
                public int ParameterId;
                public EditablePropertyBlockGen2 Property;
            }
        }
    }
}

