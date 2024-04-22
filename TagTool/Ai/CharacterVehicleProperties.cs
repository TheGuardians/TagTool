using System;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xD0)]
    public class CharacterVehicleProperties : TagStructure
	{
        public CachedTag Unit;
        public CachedTag Style;
        public VehicleFlags Flags;
        public float AiPathfindingRadius; // Ground vehicles (world units)
        public float AiDestinationRadius; // Distance within which goal is considered reached (world units)
        public float AiDecelerationDistance; // Distance from goal at which AI starts to decelerate (world units)
        public float AiTurningRadius; // Idealized average turning radius (should reflect actual vehicle physics) (world units)
        public float AiInnerTurningRadius; // Idealized minimum turning radius (should reflect actual vehicle physics) (Warthogs)
        public float AiIdealTurningRadius; // Ideal turning radius for rounding turns (barring obstacles, etc.) (Warthogs, ghosts)
        public Angle AiBansheeSteeringMaxAngle; // (banshees)
        public float AiMaxSteeringAngle; // Maximum steering angle from forward (ultimately controls turning speed) degrees (warthogs, ghosts, wraiths)
        public float AiMaxSteeringDelta; // Maximum delta in steering angle from one tick to the next (ultimately controls turn acceleration) degrees (warthogs, dropships, ghosts, wraiths)
        public float AiOversteeringScale; // (warthogs, ghosts, wraiths)
        public Bounds<Angle> AiOversteeringBounds; // Angle to goal at which AI will oversteer (banshees)
        public float AiSideSlipDistance; // Distance within which Ai will strafe to target, rather than turning (ghosts, dropships)
        public float AiAvoidanceDistance; // Look-ahead distance for obstacle avoidance (banshees)
        public float AiMinimumUrgency; // The minimum urgency with which a turn can be made (urgency = percent of maximum steering delta) [0-1]
        public Angle DestinationBehindAngle; // The angle from facing that is considered to be behind us (we do the ugly floaty slidey turn to things behind us) (dropships)
        public float SkidScale; // When approaching a corner at speed, we may want to skid around that corner, by turning slightly too early. This is (roughly) how many seconds ahead we should start turning. (warthog)
        public float AiThrottleMaximum; // (0 - 1) (all vehicles)
        public float AiGoalMinThrottleScale; // scale on throttle when within 'ai deceleration distance' of goal (0...1) (warthogs, dropships, ghosts)
        public float AiTurnMinThrottleScale; // Scale on throttle due to nearness to a turn (0...1) (warthogs, dropships, ghosts)
        public float AiDirectionMinimumThrottleScale; // Scale on throttle due to facing away from intended direction (0...1) (warthogs, dropships, ghosts)
        public float AiAccelerationScale; // The maximum allowable change in throttle between ticks (0-1): (warthogs, ghosts)
        public float AiThrottleBlend; // The degree of throttle blending between one tick and the next (0 = no blending) (0-1): (dropships, sentinels)
        public float TheoreticalMaxSpeed; // About how fast I can go. wu/s (warthogs, dropships, ghosts)
        public float ErrorScale; // scale on the difference between desired and actual speed, applied to throttle (warthogs, dropships)
        public Angle AiAllowableAimDeviationAngle;
        public float AiChargeTightAngleDistance; // The distance at which the tight angle criterion is used for deciding to vehicle charge (world units)
        public float AiChargeTightAngle; // Angle cosine within which the target must be when target is closer than tight angle distance in order to charge [0-1] (all vehicles)
        public float AiChargeRepeatTimeout; // Time delay between vehicle charges (all vehicles)
        public float AiChargeLookAheadTime; // In deciding when to abort vehicle charge, look ahead these many seconds to predict time of contact (all vehicles)
        public float AiConsiderDistance; // Consider charging the target when it is within this range (0 = infinite distance) (all vehicles)
        public float AiChargeAbortDistance; // Abort the charge when the target get more than this far away (0 = never abort) (all vehicles)
        public float VehicleRamTimeout; // The ram behavior stops after a maximum of the given number of seconds
        public float RamParalysisTime; // The ram behavior freezes the vehicle for a given number of seconds after performing the ram
        public float AiCoverDamageThreshold; // Trigger a cover when recent damage is above given threshold (damage_vehicle_cover impulse) (all vehicles)
        public float AiCoverMinimumDistance; // When executing vehicle-cover, minimum distance from the target to flee to (all vehicles)
        public float AiCoverTime; // How long to stay away from the target/ (all vehicles)
        public float AiCoverMinimumBoostDistance; // Boosting allowed when distance to cover destination is greater then this. (all vehicles)
        public float TurtlingRecentDamageThreshold; // If vehicle turtling behavior is enabled, turtling is initiated if 'recent damage' surpasses the given threshold (percent)
        public float TurtlingMinimumTime; // If the vehicle turtling behavior is enabled, turtling occurs for at least the given time (seconds)
        public float TurtlingTimeout; // The turtled state times out after the given number of seconds (seconds)
        public AiSize ObstacleIgnoreSize;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;

        public short MaxVehicleCharge; // max number of this type of vehicle in a task who can vehicle charge at once
        public short MinVehicleCharge; // min number of this type of vehicle in a task who can vehicle charge at once (soft limit, just a desired number)

        [Flags]
        public enum VehicleFlags : uint
        {
            PassengersAdoptOriginalSquad = 1 << 0,
            SnapFacingToForwardghosts = 1 << 1,
            ThrottleToTargethornets = 1 << 2,
            StationaryFighttanks = 1 << 3,
            KeepMoving = 1 << 4
        }
    }
}
