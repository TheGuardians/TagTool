using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "damage_response_definition", Tag = "drdf", Size = 0x18)]
    public class DamageResponseDefinition : TagStructure
    {
        public List<DamageResponseClassBlock> Classes;
        public List<AreaControlBlockStruct> AreaControl;
        
        [TagStructure(Size = 0xA8)]
        public class DamageResponseClassBlock : TagStructure
        {
            public DamageResponseClassTypeEnum Type;
            public DamageResponseClassFlags Flags;
            public DamageResponseDirectionalFlashStruct DirectionalFlash;
            public DamageResponseMotionSensorPing MotionSensorPing;
            [TagField(ValidTags = new [] { "rmbl" })]
            public CachedTag Rumble;
            [TagField(ValidTags = new [] { "csdt" })]
            public CachedTag CameraShake;
            [TagField(ValidTags = new [] { "csdt" })]
            // falls back on camerashake if untuned
            public CachedTag CameraShakeZoomed;
            [TagField(ValidTags = new [] { "sidt" })]
            public CachedTag SimulatedInput;
            [TagField(ValidTags = new [] { "sidt" })]
            // falls back on simulated input if untuned
            public CachedTag SimulatedInputZoomed;
            public List<DamageResponseGlobalSoundEffectBlockStruct> GlobalSoundEffect;
            
            public enum DamageResponseClassTypeEnum : short
            {
                Shielded,
                Unshielded,
                All
            }
            
            [Flags]
            public enum DamageResponseClassFlags : ushort
            {
                IgnoreOnNoDamage = 1 << 0,
                SuppressDirectionalDamageFlashes = 1 << 1,
                SuppressDirectionalDamageArrows = 1 << 2,
                // if target is zoomed
                OnlyWhenZoomed = 1 << 3,
                SoundEffectOnlyAppliedWhenScaleIsFull = 1 << 4
            }
            
            [TagStructure(Size = 0x44)]
            public class DamageResponseDirectionalFlashStruct : TagStructure
            {
                public float IndicatorDuration;
                public float FlashDuration;
                public GlobalReverseTransitionFunctionsEnum FadeFunction;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float CenterSize;
                public float OffscreenSize;
                public float CenterAlpha;
                public float OffscreenAlpha;
                public float InnerAlpha;
                public float OuterAlpha;
                public RealArgbColor FlashColor;
                public RealArgbColor ArrowColor;
                
                public enum GlobalReverseTransitionFunctionsEnum : short
                {
                    Linear,
                    Late,
                    VeryLate,
                    Early,
                    VeryEarly,
                    Cosine,
                    Zero,
                    One
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class DamageResponseMotionSensorPing : TagStructure
            {
                public short PingDuration; // ticks
                public short PingScale;
            }
            
            [TagStructure(Size = 0x18)]
            public class DamageResponseGlobalSoundEffectBlockStruct : TagStructure
            {
                public StringId EffectName;
                public MappingFunction ScaleDuration; // seconds
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
        
        [TagStructure(Size = 0x4C)]
        public class AreaControlBlockStruct : TagStructure
        {
            public AreaControlFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // the maximum distance this player feedback will affect
            public float MaximumDistance; // world units
            public AreaControlScalarFunctionStruct DistanceFalloff;
            public AreaControlScalarFunctionStruct AngleFalloff;
            public AreaControlScalarObjectFunctionStruct ObjectFalloff;
            
            [Flags]
            public enum AreaControlFlags : ushort
            {
                DistanceFalloff = 1 << 0,
                AngleFalloff = 1 << 1,
                ObjectFunctionFalloff = 1 << 2,
                // use the head position and facing vector of the unit instead of the player camera
                UseUnitPosition = 1 << 3
            }
            
            [TagStructure(Size = 0x14)]
            public class AreaControlScalarFunctionStruct : TagStructure
            {
                public MappingFunction Mapping;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class AreaControlScalarObjectFunctionStruct : TagStructure
            {
                public StringId InputVariable;
                public StringId RangeVariable;
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
