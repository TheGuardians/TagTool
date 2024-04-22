using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "flock", Tag = "flck", Size = 0x60)]
    public class Flock : TagStructure
    {
        // weight given to boid's desire to fly straight ahead
        public float ForwardWeight; // [0..1]
        // weight given to boids desire to fly level
        public float LevelingForceWeight; // [0..1]
        // weight given to boid's desire to fly towards its sinks
        public float DestinationWeight; // [0..1]
        // throttle at which boids will naturally fly
        public float AverageThrottle; // [0..1]
        // maximum throttle applicable
        public float MaximumThrottle; // [0..1]
        // The threshold of accumulated weight over which movement occurs
        public float MovementWeightThreshold;
        // distance within which boids will avoid a dangerous object (e.g. the player)
        public float DangerRadius; // wus
        // weight given to boid's desire to avoid danger
        public float DangerWeight;
        // weight given to boid's desire to attack fly after their target, if they have one
        public float TargetWeight; // [0..1]
        // distance within which we aggressively pursue a target
        public float TargetDistance; // wus
        // amount of time we need to be locked onto a target before we might start killing it
        public float TargetDelayTime; // seconds
        // probability of killing your target in one second
        public float TargetKillChance; // chance per second
        // if targetted by AI, the probability of dying in one second
        public float AiDestroyChance; // chance per second
        // weight given to boid's random heading offset
        public float RandomOffsetWeight; // [0..1]
        public Bounds<float> RandomOffsetPeriod; // seconds
        // distance within which one boid is affected by another
        public float NeighborhoodRadius; // world units
        // angle-from-forward within which one boid can perceive and react to another
        public Angle PerceptionAngle; // degrees
        // weight given to boid's desire to avoid collisions with other boids, when within the avoidance radius
        public float AvoidanceWeight; // [0..1]
        // distance that a boid tries to maintain from another
        public float AvoidanceRadius; // world units
        // weight given to boid's desire to align itself with neighboring boids
        public float AlignmentWeight; // [0..1]
        // weight given to boid's desire to be near flock center
        public float PositionWeight; // [0..1]
        // distance to flock center beyond which an attracting force is applied
        public float PositionMinRadius; // wus
        // distance to flock center at which the maximum attracting force is applied
        public float PositionMaxRadius; // wus
    }
}
