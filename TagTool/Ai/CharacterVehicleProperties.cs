using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xD0)]
    public class CharacterVehicleProperties : TagStructure
	{
        public CachedTagInstance Unit;
        public CachedTagInstance Style;
        public uint VehicleFlags;
        public float AiPathfindingRadius;
        public float AiDestinationRadius;
        public float AiDecelerationDistance;
        public float AiTurningRadius;
        public float AiInnerTurningRadius;
        public float AiIdealTurningRadius;
        public Angle AiBansheeSteeringMaximum;
        public float AiMaxSteeringAngle;
        public float AiMaxSteeringDelta;
        public float AiOversteeringScale;
        public Bounds<Angle> AiOversteeringBounds;
        public float AiSideSlipDistance;
        public float AiAvoidanceDistance;
        public float AiMinimumUrgency;
        public Angle Unknown;
        public uint Unknown2;
        public float AiThrottleMaximum;
        public float AiGoalMinimumThrottleScale;
        public float AiTurnMinimumThrottleScale;
        public float AiDirectionMinimumThrottleScale;
        public float AiAccelerationScale;
        public float AiThrottleBlend;
        public float TheoreticalMaxSpeed;
        public float ErrorScale;
        public Angle AiAllowableAimDeviationAngle;
        public float AiChargeTightAngleDistance;
        public float AiChargeTightAngle;
        public float AiChargeRepeatTimeout;
        public float AiChargeLookAheadTime;
        public float AiConsiderDistance;
        public float AiChargeAbortDistance;
        public float VehicleRamTimeout;
        public float RamParalysisTime;
        public float AiCoverDamageThreshold;
        public float AiCoverMinimumDistance;
        public float AiCoverTime;
        public float AiCoverMinimumBoostDistance;
        public float TurtlingRecentDamageThreshold;
        public float TurtlingMinimumTime;
        public float TurtlingTimeout;
        public AiSize ObstacleIgnoreSize;
        public short Unknown3;
        public short Unknown4;
        public short Unknown5;
    }
}
