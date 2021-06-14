using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "camera_shake", Tag = "csdt", Size = 0x48)]
    public class CameraShake : TagStructure
    {
        // the effect will last for this duration.
        public float ShakeDuration; // seconds
        public MappingFunction Mapping;
        // random translation in all directions
        public float RandomTranslation; // world units
        // random rotation in all directions
        public Angle RandomRotation; // degrees
        public float WobbleFunctionPeriod; // seconds
        // a value of 0.0 signifies that the wobble function has no effect; a value of 1.0 the wobble function completely
        // scales the translational
        // and rotational magnitudes.  The less the weight, the less the effect wobble has.
        public float WobbleWeight;
        // a function to perturb the effect's behavior over time
        public GlobalPeriodicFunctionsEnum WobbleFunction;
        public AnimatedCameraShakePlaybackTypeEnum AnimatedShakePlayback;
        public AnimatedCameraShakeWeightTypeEnum AnimatedShakeWeight;
        [TagField(ValidTags = new [] { "jmad" })]
        public CachedTag AnimationGraph;
        public StringId AnimationName;
        // multiplier penalty that increases linearly with zoom over 1
        public float ZoomPenaltyLinear;
        // multiplier penalty that increases with square root of zoom over 1
        public float ZoomPenaltySquareroot;
        
        public enum GlobalPeriodicFunctionsEnum : short
        {
            One,
            Zero,
            Cosine,
            Cosine1,
            DiagonalWave,
            DiagonalWave1,
            Slide,
            Slide1,
            Noise,
            Jitter,
            Wander,
            Spark
        }
        
        public enum AnimatedCameraShakePlaybackTypeEnum : sbyte
        {
            Looping,
            FrameRatio
        }
        
        public enum AnimatedCameraShakeWeightTypeEnum : sbyte
        {
            EffectScale,
            Full
        }
        
        [TagStructure(Size = 0x28)]
        public class CameraImpulseStruct : TagStructure
        {
            public float ImpulseDuration; // seconds
            public MappingFunction Mapping;
            public Angle Rotation; // degrees
            public float Pushback; // world units
            public Bounds<float> Jitter; // world units
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class MappingFunction : TagStructure
        {
            public byte[] Data;
        }
    }
}
