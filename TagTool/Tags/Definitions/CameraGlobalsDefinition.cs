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

        public float FieldOfViewScale;
        public float YawScale;
        public float PitchScale;
        public float ForwardScale;
        public float SideScale;
        public float UpScale;

        public float DeathcamTransitionTime; // duration of deathcam movement from initial to final distance (seconds)
        public float FallDeathcamTransitionTime; // duration of falling deathcam movement from initial to final distance (seconds)
        public float DeathcamInitialDistance; // distance from your dead body on initial frame (world units)
        public float DeathcamFinalDistance; // how far from the body the camera will settle (world units)
        public float DeathcamZOffset; // how far above the body the camera focuses on (world units)
        public float DeathcamMaximumElevation; // (RADIANS) highest angle the camera can raise to (prevents it from flipping over the vertical axis)
        public float DeathcamTrackingDelay; // delay in tracking the killer (seconds)
        public float DeathcamOrbitDelay; // how long the death camera lasts before switching to orbiting camera (seconds)

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown46;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown47;

        public float FallDeathcamMinimumVelocity; // minimum velocity to switch to fell to death behavior (when biped is not actually falling to death)
        
        public float FlyingMaxBoostSpeed; // the scaling factor for the left stick when the left trigger is fully depressed
        public float FlyingTimeToMaximumBoost; // seconds. while pegging boost, time to reach maximum speed
        public FunctionType BoostFunction;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;

        public float FlyingZoomedFov; // (degrees) field of view when zoomed
        public float FlyingZoomedLookSpeedScale; // scaling factor for look speed when zoomed
        public float FlyingBoundingSphereRadius; // radius of sphere for collision
        public float FlyingMovementDelay; // how quickly the camera responds to the user's input (seconds)
        public float FlyingZoomTransitionTime; // how long it takes to zoom in or out (seconds)
        public float FlyingVerticalMaxSpeedTime;

        public FunctionType HoistFunction;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float SurvivalSwitchTime;

        public Bounds<float> OrbitCamDistanceBounds; // (world units)
        public float OrbitCamMovementDelay; // how quickly the camera responds to the user's input (seconds)
        public float OrbitCamZOffset; // how far above the object's root node to position the camera's focus point
        public Bounds<float> OrbitCamElevationAngleBounds;  // (RADIANS) lowest angle the camera can be moved to

        public float SavedFilmMaxPlaybackSpeed; // how fast the film plays when the trigger is fully depressed
        public float SavedFilmFadeOutTime; // (seconds)
        public float SavedFilmFadeInTime; // (seconds)

        public float EnterVehicleTransitionTime; // how long it takes the camera to move from first to third person when entering a vehicle (seconds)
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