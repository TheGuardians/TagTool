using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "flock", Tag = "flck", Size = 0x5C, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "flock", Tag = "flck", Size = 0x60, MinVersion = CacheVersion.HaloOnline106708)]
    public class Flock
    {
        public float ForwardScale;
        public float SinkScale;
        public float AverageThrottle;
        public float MaximumThrottle;
        public float MovementWeightThreshold;
        public float DangerRadius;
        public float DangerScale;
        public float TargetScale;
        public float TargetDistance;
        public float TargetDelayTime;
        public float TargetKillChance;
        public float AiDestroyChance;
        public float RandomOffsetScale;
        public Bounds<float> RandomOffsetPeriod;
        public float NeighborhoodRadius;
        public Angle PerceptionAngle;
        public float AvoidanceScale;
        public float AvoidanceRadius;
        public float AlignmentScale;
        public float PositionScale;
        public Bounds<float> PositionRadii;

        [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}