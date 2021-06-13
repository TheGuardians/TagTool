using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "muffin", Tag = "mffn", Size = 0x38)]
    public class Muffin : TagStructure
    {
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag RenderModel;
        public List<MuffinPermutationNameBlock> RenderModelPermutationNames;
        public int RenderModelPermutationNameValidCount;
        // defines where muffins appear, how often, and which types
        public List<MuffinMarkerBlock> MuffinMarkers;
        public List<GlobalMuffinTypeStruct> MuffinTypes;
        
        [TagStructure(Size = 0x4)]
        public class MuffinPermutationNameBlock : TagStructure
        {
            public StringId Name;
        }
        
        [TagStructure(Size = 0x8)]
        public class MuffinMarkerBlock : TagStructure
        {
            public StringId Name;
            // how often muffins will appear on this marker
            public ushort MuffinageChance; // percent [0 - 100]
            public ushort AllowedMuffinTypes;
        }
        
        [TagStructure(Size = 0x70)]
        public class GlobalMuffinTypeStruct : TagStructure
        {
            public short Mesh;
            public MuffinTypeFlags Flags;
            // how long to wait before spawning these muffins
            public Bounds<float> SpawnDelay; // seconds
            public Bounds<float> RandomScale; // [0-1]
            public MuffinScalarFunctionStruct InitialGrowth;
            public float GrowthTime; // seconds
            public MuffinScalarFunctionStruct PeriodicNoise;
            // how much the muffin jiggles (0 = perfectly rigid, 1 = full jiggle, subject to parameters below)
            public float Jiggle; // [0.0 - 1.0]
            // spring length affects how the spring reacts to the model moving (this should be approximately how far the muffin
            // sticks out from the model)
            public float JiggleSpringLength; // world units
            // spring strength affects how fast the muffins jiggle (higher numbers jiggle faster)
            public float JiggleSpringStrength; // [0.01 - 1.0]
            // velocity damp affects how long the muffins jiggle (the higher numbers jiggle exponentially longer, 1.0 will never
            // stop jiggling)
            public float JiggleVelocityDamp; // [0.5 - 0.95]
            // this just clamps how fast the muffin can jiggle, to keep it from exploding
            public float JiggleMaxVelocity; // world units per frame
            // how long before the muffins die
            public Bounds<float> Lifetime; // seconds
            // the chance that any give muffin of this type will live forever
            public float ImmortalityChance; // [0.0 - 1.0]
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag DeathEffect;
            
            [Flags]
            public enum MuffinTypeFlags : ushort
            {
                DisableForDebug = 1 << 0,
                // forces the muffins to the new marker location when they are transferred to a new model, may cause popping if the
                // markers aren't aligned
                JumpToMarkerOnTransition = 1 << 1
            }
            
            [TagStructure(Size = 0x14)]
            public class MuffinScalarFunctionStruct : TagStructure
            {
                public MappingFunction Mapping;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
    }
}
