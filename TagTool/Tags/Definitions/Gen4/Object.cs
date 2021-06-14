using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "object", Tag = "obje", Size = 0x204)]
    public class GameObject : TagStructure
    {
        public short RuntimeObjectType;
        public NavMeshCuttingOverrideEnum NavMeshCutting;
        public BooleanOverrideEnum NavMeshObstacle;
        public RealVector3d NavMeshCuttingObbOffset;
        public RealVector3d NavMeshCuttingObbScale;
        public ObjectDefinitionFlags Flags;
        // If you edit this field manually, beware that the render model is no longer respected for radius calculation and
        // that you need to set the bounding offset as well.
        public float BoundingRadius;
        public RealPoint3d BoundingOffset;
        public float HorizontalAccelerationScale; // [0,+inf]
        public float VerticalAccelerationScale;
        public float AngularAccelerationScale;
        public LightmapShadowModeEnum LightmapShadowMode;
        public SweetenerSizeEnum SweetenerSize;
        public WaterDensityTypeEnum WaterDensity;
        public int RuntimeFlags;
        // sphere to use for dynamic lights and shadows. only used if not 0
        public float DynamicLightSphereRadius;
        // only used if radius not 0
        public RealPoint3d DynamicLightSphereOffset;
        public StringId GenericHudText;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag GenericNameList;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag GenericServiceTagList;
        public List<Sidecarblock> SourceSidecar;
        public StringId DefaultModelVariant;
        [TagField(ValidTags = new [] { "hlmt" })]
        public CachedTag Model;
        [TagField(ValidTags = new [] { "bloc" })]
        public CachedTag CrateObject;
        [TagField(ValidTags = new [] { "cddf" })]
        public CachedTag CollisionDamage;
        [TagField(ValidTags = new [] { "cddf" })]
        public CachedTag BrittleCollisionDamage;
        public List<ObjectEarlyMoverObbBlock> EarlyMoverObb;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag CreationEffect;
        [TagField(ValidTags = new [] { "foot" })]
        public CachedTag MaterialEffects;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        // this is the sound that is made when I am meleed.  This overrides the sweetener sound of my material.
        public CachedTag MeleeSound;
        // if non-zero, any instances of this object will destroy themselves after this many seconds.
        public float SelfDestructTime; // seconds
        public List<ObjectAiPropertiesBlock> AiProperties;
        public List<ObjectFunctionBlock> Functions;
        public List<ObjectRuntimeInterpolatorFunctionsBlock> RuntimeInterpolatorFunctions;
        public List<ObjectfunctionSwitchBlock> FunctionSwitches;
        public short HudTextMessageIndex;
        public ObjectDefinitionSecondaryFlags SecondaryFlags;
        public List<GlobalObjectAttachmentBlock> Attachments;
        public List<WaterPhysicsHullSurfaceDefinitionBlock> HullSurfaces;
        public List<JetwashDefinitionBlock> Jetwash;
        public List<ObjectWidgetBlock> Widgets;
        public List<ObjectChangeColors> ChangeColors;
        public List<GNullBlock> PredictedResources;
        public List<MultiplayerObjectBlock> MultiplayerObject;
        [TagField(ValidTags = new [] { "siin" })]
        // Set to a specific interpolation definition, or leave blank to inherit the default for the object type (there are
        // defaults for bipeds, vehicles, and crates in multiplayer_globals).  To disable interpolation on a particular object
        // whose type has interpolation by default, set this reference to the special
        // disable_interpolation.simulation_interpolation tag.
        public CachedTag SimulationInterpolation;
        public List<ObjectSpawnEffectsBlock> SpawnEffects;
        public List<ModeldissolveDataBlock> ModelDissolveData;
        public HsScriptDataStruct ScriptData;
        public List<HsReferencesBlock> ScriptTagalongs;
        public List<HsReferencesBlock> ScriptedDependencies;
        public ObjectAbandonmentStruct ObjectAbandonment;
        
        public enum NavMeshCuttingOverrideEnum : sbyte
        {
            Default,
            Cut,
            NotCut
        }
        
        public enum BooleanOverrideEnum : sbyte
        {
            Default,
            Yes,
            No
        }
        
        [Flags]
        public enum ObjectDefinitionFlags : uint
        {
            DoesNotCastShadow = 1 << 0,
            ChildrenDoNotCastShadow = 1 << 1,
            FirstClassChild = 1 << 2,
            ObjectSamplesLightprobesOnly = 1 << 3,
            ObjectUsesOnlyOwnStaticLightmap = 1 << 4,
            SearchCardinalDirectionLightmapsOnFailure = 1 << 5,
            PreservesInitialDamageOwner = 1 << 6,
            NotAPathfindingObstacle = 1 << 7,
            // object uses parent's markers
            ExtensionOfParent = 1 << 8,
            DoesNotCauseCollisionDamage = 1 << 9,
            EarlyMover = 1 << 10,
            EarlyMoverLocalizedPhysics = 1 << 11,
            ObjectScalesAttachments = 1 << 12,
            InheritsPlayerSAppearance = 1 << 13,
            NonPhysicalInMapEditor = 1 << 14,
            ObjectIsAlwaysOnTheCeiling = 1 << 15,
            SampleEnviromentLightingOnlyIgnoreObjectLighting = 1 << 16,
            EffectsCreatedByThisObjectDoNotSpawnObjectsInMultiplayer = 1 << 17,
            // force camera not to collide with object.  By default small sweetener objects do not collide
            DoesNotCollideWithCamera = 1 << 18,
            // force the camera to collide with this object,  By default small sweetener objects do not collide
            ForceCollideWithCamera = 1 << 19,
            // AOE damage being applied to this object does not test for obstrutions.
            DamageNotBlockedByObstructions = 1 << 20,
            DoesNotDamageBreakableSurfaces = 1 << 21,
            EarlyMoverLocalizeProjectiles = 1 << 22,
            RequiresShadowBoundsVisibilityTest = 1 << 23,
            GrabParentObjectInForgeEditing = 1 << 24,
            NeverUseImposterForShadowGeneration = 1 << 25,
            HoistableFromStand = 1 << 26,
            HoistableFromCrouch = 1 << 27,
            Vaultable = 1 << 28,
            CollidesWithOwnProjectiles = 1 << 29,
            ObjectRejectsDynamicDecals = 1 << 30
        }
        
        public enum LightmapShadowModeEnum : short
        {
            Default,
            Never,
            Always,
            Blur
        }
        
        public enum SweetenerSizeEnum : sbyte
        {
            Default,
            Small,
            Medium,
            Large
        }
        
        public enum WaterDensityTypeEnum : sbyte
        {
            Default,
            SuperFloater,
            Floater,
            Neutral,
            Sinker,
            SuperSinker,
            None
        }
        
        [Flags]
        public enum ObjectDefinitionSecondaryFlags : ushort
        {
            DoesNotAffectProjectileAiming = 1 << 0,
            UpdateStateAnimationAndKeyframeRigidBodiesEveryFrame = 1 << 1
        }
        
        [TagStructure(Size = 0x100)]
        public class Sidecarblock : TagStructure
        {
            [TagField(Length = 256)]
            public string SidecarPath;
        }
        
        [TagStructure(Size = 0x2C)]
        public class ObjectEarlyMoverObbBlock : TagStructure
        {
            // empty mean object space
            public StringId NodeName;
            public int RuntimeNodeIndex;
            public float X0;
            public float X1;
            public float Y0;
            public float Y1;
            public float Z0;
            public float Z1;
            public RealEulerAngles3d Angles;
        }
        
        [TagStructure(Size = 0x10)]
        public class ObjectAiPropertiesBlock : TagStructure
        {
            public AiPropertiesFlags AiFlags;
            // used for combat dialogue, etc.
            public StringId AiTypeName;
            // if you checked the consider for interaction flag, type what interaction the AI should do with this object (NYI -
            // you can use any)
            public StringId InteractionName;
            public AiSizeEnum AiSize;
            public GlobalAiJumpHeightEnum LeapJumpSpeed;
            
            [Flags]
            public enum AiPropertiesFlags : uint
            {
                DetroyableCover = 1 << 0,
                PathfindingIgnoreWhenDead = 1 << 1,
                DynamicCover = 1 << 2,
                NonFlightBlocking = 1 << 3,
                DynamicCoverFromCentre = 1 << 4,
                HasCornerMarkers = 1 << 5,
                IdleWhenFlying = 1 << 6,
                ConsiderForInteraction = 1 << 7
            }
            
            public enum AiSizeEnum : short
            {
                Default,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }
            
            public enum GlobalAiJumpHeightEnum : short
            {
                None,
                Down,
                Step,
                Crouch,
                Stand,
                Storey,
                Tower,
                Infinite
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class ObjectFunctionBlock : TagStructure
        {
            public ObjectFunctionFlags Flags;
            // if you leave this field blank then you can set this function's input value with the hs_function
            // object_set_function_variable
            public StringId ImportName;
            public StringId ExportName;
            // if the specified function is off, so is this function
            public StringId TurnOffWith;
            // if you have the ranged button checked
            public StringId RangedInterpolationName;
            // function must exceed this value (after mapping) to be active 0. means do nothing
            public float MinValue;
            public MappingFunction DefaultFunction;
            public StringId ScaleBy;
            public List<ObjectFunctionInterpolationBlock> Interpolation;
            public int RuntimeInterpolatorIndex;
            
            [Flags]
            public enum ObjectFunctionFlags : uint
            {
                // result of function is one minus actual result
                Invert = 1 << 0,
                // the curve mapping can make the function active/inactive
                MappingDoesNotControlsActive = 1 << 1,
                // function does not deactivate when at or below lower bound
                AlwaysActive = 1 << 2,
                // function offsets periodic function input by random value between 0 and 1
                RandomTimeOffset = 1 << 3,
                // when this function is deactivated, it still exports its value
                AlwaysExportsValue = 1 << 4,
                // the function will be turned off if the value of the turns_off_with function is 0
                TurnOffWithUsesMagnitude = 1 << 5
            }
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
            
            [TagStructure(Size = 0x18)]
            public class ObjectFunctionInterpolationBlock : TagStructure
            {
                public ObjectFunctionInterpolationModeEnum InterpolationMode;
                // used by constant velocity
                public float LinearTravelTime; // s
                // used by linear acceleration
                public float Acceleration; // 1/s/s
                // used by damped spring
                // determines acceleration by displacement
                public float SpringK;
                // used by damped spring
                // determines damping based on velocity
                public float SpringC;
                // used by fractional
                // how mush of the distance to the target to cover each update
                public float Fraction; // 0-1
                
                public enum ObjectFunctionInterpolationModeEnum : int
                {
                    ConstantVelocity,
                    LinearAcceleration,
                    DampedSpring,
                    // covers a fixed fraction of the distance to the target on each update
                    Fractional
                }
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class ObjectRuntimeInterpolatorFunctionsBlock : TagStructure
        {
            public int RuntimeInterpolatorToObjectFunctionMapping;
        }
        
        [TagStructure(Size = 0x14)]
        public class ObjectfunctionSwitchBlock : TagStructure
        {
            public StringId SwitchFunctionName;
            public StringId ExportName;
            public List<ObjectfunctionSwitchFunctionBlock> SwitchedFunctions;
            
            [TagStructure(Size = 0xC)]
            public class ObjectfunctionSwitchFunctionBlock : TagStructure
            {
                // if the switch function is between these values, this function will be picked
                public Bounds<float> SelectionBounds;
                public StringId FunctionName;
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class GlobalObjectAttachmentBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ligh","gldf","ltvl","effe","lsnd","lens","cpem","egfd","decs" })]
            public CachedTag Type;
            public StringId Marker;
            public GlobalObjectChangeColorEnum ChangeColor;
            public ObjectAttachmentFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId PrimaryScale;
            public StringId SecondaryScale;
            
            public enum GlobalObjectChangeColorEnum : sbyte
            {
                None,
                Primary,
                Secondary,
                Tertiary,
                Quaternary
            }
            
            [Flags]
            public enum ObjectAttachmentFlags : byte
            {
                // when this flag is clear we only create the attachment when the function object function is active or set to empty
                // string
                ForceAlwaysOn = 1 << 0,
                EffectSizeScaleFromObjectScale = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class WaterPhysicsHullSurfaceDefinitionBlock : TagStructure
        {
            public WaterPhysicsHullSurfaceDefinitionFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId MarkerName;
            public float Radius;
            public List<WaterPhysicsMaterialOverride> Drag;
            
            [Flags]
            public enum WaterPhysicsHullSurfaceDefinitionFlags : ushort
            {
                // drives on an extruded version of everything physical in your level
                WorksOnLand = 1 << 0,
                EffectsOnly = 1 << 1
            }
            
            [TagStructure(Size = 0x3C)]
            public class WaterPhysicsMaterialOverride : TagStructure
            {
                public StringId Material;
                public WaterPhysicsDragPropertiesStruct Drag;
                
                [TagStructure(Size = 0x38)]
                public class WaterPhysicsDragPropertiesStruct : TagStructure
                {
                    public PhysicsForceFunctionStruct Pressure;
                    public PhysicsForceFunctionStruct Suction;
                    public float LinearDamping;
                    public float AngularDamping;
                    
                    [TagStructure(Size = 0x18)]
                    public class PhysicsForceFunctionStruct : TagStructure
                    {
                        public MappingFunction VelocityToPressure;
                        public float MaxVelocity; // wu/s
                        
                        [TagStructure(Size = 0x14)]
                        public class MappingFunction : TagStructure
                        {
                            public byte[] Data;
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class JetwashDefinitionBlock : TagStructure
        {
            public StringId MarkerName;
            public float Radius;
            public int MaximumTraces; // traces per second
            public float MaximumEmissionLength; // world units
            public Bounds<Angle> TraceYawAngle; // degrees
            public Bounds<Angle> TracePitchAngle; // degrees
            public float ParticleOffset; // world units
        }
        
        [TagStructure(Size = 0x10)]
        public class ObjectWidgetBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ant!","clwd" })]
            public CachedTag Type;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x18)]
        public class ObjectChangeColors : TagStructure
        {
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<ObjectChangeColorInitialPermutation> InitialPermutations;
            public List<ObjectChangeColorFunction> Functions;
            
            [TagStructure(Size = 0x20)]
            public class ObjectChangeColorInitialPermutation : TagStructure
            {
                public float Weight;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                // if empty, may be used by any model variant
                public StringId VariantName;
            }
            
            [TagStructure(Size = 0x28)]
            public class ObjectChangeColorFunction : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public GlobalRgbInterpolationFlags ScaleFlags;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                public StringId DarkenBy;
                public StringId ScaleBy;
                
                [Flags]
                public enum GlobalRgbInterpolationFlags : uint
                {
                    // blends colors in hsv rather than rgb space
                    BlendInHsv = 1 << 0,
                    // blends colors through more hues (goes the long way around the color wheel)
                    MoreColors = 1 << 1
                }
            }
        }
        
        public class GNullBlock : TagStructure
        {
        }
        
        [TagStructure(Size = 0xCC)]
        public class MultiplayerObjectBlock : TagStructure
        {
            public GlobalGameEngineTypeFlags GameEngineFlags;
            public MultiplayerObjectType Type;
            public TeleporterPassabilityFlags TeleporterPassability; // used only for teleporters
            public MultiplayerObjectSpawnTimerTypes SpawnTimerType;
            public float BoundaryWidthRadius;
            public float BoundaryBoxLength;
            public float BoundaryHeight;
            public float BoundaryHeight1;
            public MultiplayerObjectBoundaryShape BoundaryShape;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short DefaultSpawnTime; // seconds
            public short DefaultAbandonmentTime; // seconds
            public MultiplayerObjectFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float NormalWeight; // aka natural weight
            // Multiplier applied to weight (domain is center to radius, range should be 0 to 1).
            public List<SpawnInfluenceWeightFalloffFunctionBlock> FalloffFunction;
            public StringId BoundaryCenterMarker;
            public StringId SpawnedObjectMarkerName;
            [TagField(ValidTags = new [] { "obje" })]
            public CachedTag SpawnedObject;
            public StringId NyiBoundaryMaterial;
            [TagField(ValidTags = new [] { "mat ","rm  " })]
            public CachedTag BoundaryStandardShader;
            [TagField(ValidTags = new [] { "mat ","rm  " })]
            public CachedTag BoundaryOpaqueShader;
            [TagField(ValidTags = new [] { "mat ","rm  " })]
            public CachedTag SphereStandardShader;
            [TagField(ValidTags = new [] { "mat ","rm  " })]
            public CachedTag SphereOpaqueShader;
            [TagField(ValidTags = new [] { "mat ","rm  " })]
            public CachedTag CylinderStandardShader;
            [TagField(ValidTags = new [] { "mat ","rm  " })]
            public CachedTag CylinderOpaqueShader;
            [TagField(ValidTags = new [] { "mat ","rm  " })]
            public CachedTag BoxStandardShader;
            [TagField(ValidTags = new [] { "mat ","rm  " })]
            public CachedTag BoxOpaqueShader;
            
            [Flags]
            public enum GlobalGameEngineTypeFlags : byte
            {
                None = 1 << 0,
                Sandbox = 1 << 1,
                Megalogamengine = 1 << 2,
                Campaign = 1 << 3,
                Survival = 1 << 4,
                Firefight = 1 << 5
            }
            
            public enum MultiplayerObjectType : sbyte
            {
                Ordinary,
                Weapon,
                Grenade,
                Projectile,
                Powerup,
                Equipment,
                AmmoPack,
                LightLandVehicle,
                HeavyLandVehicle,
                FlyingVehicle,
                Turret,
                Device,
                Dispenser,
                Teleporter2way,
                TeleporterSender,
                TeleporterReceiver,
                PlayerSpawnLocation,
                PlayerRespawnZone,
                SecondaryObjective,
                PrimaryObjective,
                NamedLocationArea,
                DangerZone,
                Fireteam1RespawnZone,
                Fireteam2RespawnZone,
                Fireteam3RespawnZone,
                Fireteam4RespawnZone,
                SafeVolume,
                KillVolume,
                CinematicCameraPosition,
                MoshEnemySpawnLocation,
                OrdnanceDropPoint,
                TraitZone,
                InitialOrdnanceDropPoint,
                RandomOrdnanceDropPoint,
                ObjectiveOrdnanceDropPoint,
                PersonalOrdnanceDropPoint
            }
            
            [Flags]
            public enum TeleporterPassabilityFlags : byte
            {
                DisallowPlayers = 1 << 0,
                AllowLightLandVehicles = 1 << 1,
                AllowHeavyLandVehicles = 1 << 2,
                AllowFlyingVehicles = 1 << 3,
                AllowProjectiles = 1 << 4
            }
            
            public enum MultiplayerObjectSpawnTimerTypes : sbyte
            {
                StartsOnDeath,
                StartsOnDisturbance
            }
            
            public enum MultiplayerObjectBoundaryShape : sbyte
            {
                Unused,
                Sphere,
                Cylinder,
                Box
            }
            
            [Flags]
            public enum MultiplayerObjectFlags : ushort
            {
                OnlyVisibleInEditor = 1 << 0,
                PhasedPhysicsInForge = 1 << 1,
                ValidInitialPlayerSpawn = 1 << 2,
                FixedBoundaryOrientation = 1 << 3,
                CandyMonitorShouldIgnore = 1 << 4,
                InheritOwningTeamColor = 1 << 5,
                BoundaryVolumeDoesnTKillImmediately = 1 << 6
            }
            
            [TagStructure(Size = 0x14)]
            public class SpawnInfluenceWeightFalloffFunctionBlock : TagStructure
            {
                public ScalarFunctionNamedStruct Function;
                
                [TagStructure(Size = 0x14)]
                public class ScalarFunctionNamedStruct : TagStructure
                {
                    public MappingFunction Function;
                    
                    [TagStructure(Size = 0x14)]
                    public class MappingFunction : TagStructure
                    {
                        public byte[] Data;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ObjectSpawnEffectsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "effe" })]
            // effect played when this object spawns in MP games
            public CachedTag MultiplayerSpawnEffect;
            [TagField(ValidTags = new [] { "effe" })]
            // effect played when this object spawns in Firefight games
            public CachedTag SurvivalSpawnEffect;
            [TagField(ValidTags = new [] { "effe" })]
            // effect played when this object spawns in Campaign games
            public CachedTag CampaignSpawnEffect;
        }
        
        [TagStructure(Size = 0x10)]
        public class ModeldissolveDataBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "mdsv" })]
            public CachedTag ModelDissolveData;
        }
        
        [TagStructure(Size = 0x28)]
        public class HsScriptDataStruct : TagStructure
        {
            public List<HsSourceReferenceBlock> SourceFileReferences;
            public List<HsSourceReferenceBlock> ExternalSourceReferences;
            [TagField(ValidTags = new [] { "hsdt" })]
            public CachedTag CompiledScript;
            
            [TagStructure(Size = 0x10)]
            public class HsSourceReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "hsc*" })]
                public CachedTag Reference;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class HsReferencesBlock : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0xC)]
        public class ObjectAbandonmentStruct : TagStructure
        {
            public float VitalityLimitToStartCountdown;
            public float CountdownTimeInSeconds;
            public ObjectAbandonmentFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum ObjectAbandonmentFlags : byte
            {
                EnableInMultiplayer = 1 << 0,
                EnableInSinglePlayer = 1 << 1
            }
        }
    }
}
