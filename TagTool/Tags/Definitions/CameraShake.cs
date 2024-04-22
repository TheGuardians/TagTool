using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "camera_shake", Tag = "csdt", Size = 0x34, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "camera_shake", Tag = "csdt", Size = 0x68, MinVersion = CacheVersion.HaloReach)]
    public class CameraShake : TagStructure
    {
        public CameraImpulseStruct CameraImpulse;
        public CameraShakeStruct CameraShakeData;
    }

    [TagStructure(Size = 0x18, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloReach)]
    public class CameraImpulseStruct : TagStructure
    {
        public float ImpulseDuration; // seconds

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public TransitionFunctionEnum FadeFunction;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding0;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagFunction FadeFunctionReach;

        public Angle Rotation; // degrees
        public float Pushback; // world units
        public Bounds<float> Jitter; // world units

        public enum TransitionFunctionEnum : short
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

    [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloReach)]
    public class CameraShakeStruct : TagStructure
    {
        // the effect will last for this duration.
        public float ShakeDuration; // seconds

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public TransitionFunctionEnum FadeFunction;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding0;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagFunction FadeFunctionReach;

        // random translation in all directions
        public float RandomTranslation; // world units
        // random rotation in all directions          
        public Angle RandomRotation; // degrees

        // a function to perturb the effect's behavior over time
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public GlobalPeriodicFunctionsEnum WobbleFunction;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding1;
        // and rotational magnitudes.  The less the weight, the less the effect wobble has.
        public float WobbleFunctionPeriod; // seconds
        // a value of 0.0 signifies that the wobble function has no effect; a value of 1.0 the wobble
        // function completely scales the translational and rotational magnitudes.The less the weight,
        // the less the effect wobble has.
        public float WobbleWeight;
        // a function to perturb the effect's behavior over time
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public GlobalPeriodicFunctionsEnum WobbleFunctionReach;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public AnimatedCameraShakePlaybackTypeEnum AnimatedShakePlayback;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public AnimatedCameraShakeWeightTypeEnum AnimatedShakeWeight;
        [TagField(ValidTags = new[] { "jmad" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag AnimationGraph;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId AnimationName;


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

        public enum TransitionFunctionEnum : short
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
}
