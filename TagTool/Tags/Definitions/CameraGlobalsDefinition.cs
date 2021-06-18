using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "camera_globals_definition", Tag = "glca", Size = 0xA0, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "camera_globals_definition", Tag = "glca", Size = 0xA4, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "camera_globals_definition", Tag = "glca", Size = 0xD8, MinVersion = CacheVersion.HaloReach)]
    public class CameraGlobalsDefinition : TagStructure
	{
        [TagField(Flags = Label, ValidTags = new[] { "trak" })]
        public CachedTag DefaultUnitCameraTrack;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagFunction Unknown;

        public float FieldOfView;
        public float YawScale;
        public float PitchScale;
        public float ForwardScale;
        public float SideScale;
        public float UpScale;
        public float DeathCamTransitionTime;
        public float FallingDeathCamTransitionTime;
        public float DeathCamInitialDistance;
        public float DeathCamFinalDistance;
        public float DeathCamZOffset;
        public float DeathCamMaximumElevation;
        public float DeathCamTrackingDelay;
        public float DeathCamAutoOrbitingDelay;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown46;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown47;

        public float FallingDeathMinimumVelocity;

        public float FlyingMaxBoostSpeed;
        public float FlyingBoostAccelerationTime;
        public float Unknown18;
        public float FlyingZoomedFov;
        public float FlyingZoomedLookSpeedScale;
        public float FlyingBoundingSphereRadius;
        public float FlyingMovementDelay;
        public float FlyingZoomTransitionTime;
        public float FlyingVerticalAccelerationTime;

        public FunctionType BoostFunction;
        public FunctionType HoistFunction;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float SurvivalSwitchTime;

        public Bounds<float> OrbitingDistance;
        public float OrbitCamMovementDelay;
        public float OrbitCamZOffset;
        public Bounds<float> OrbitCamElevationBounds;

        public float SavedFilmMaxPlaybackSpeed;
        public float SavedFilmFadeOutTime;
        public float SavedFilmFadeInTime;

        public float EnterVehicleTransitionTime;
        public float ExitVehicleTransitionTime;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown39;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public Angle Unknown41;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown42;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown43;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown44;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown45;
    }
}