using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "scenario", Tag = "scnr", Size = 0x5B0)]
    public class Scenario : TagStructure
    {
        [TagField(ValidTags = new [] { "sbsp" })]
        public CachedTag DonTUse;
        [TagField(ValidTags = new [] { "sbsp" })]
        public CachedTag WonTUse;
        /// <summary>
        /// set me!! me!!!  i do something cool!!!  you'll be happy forever if there this tag reference is filled in!  don't believe
        /// the lies!!! LIESSSS!!!!!!!!  YESS, MY PRECIOUSSSSS, LIESSSS...
        /// </summary>
        [TagField(ValidTags = new [] { "sky " })]
        public CachedTag CanTUse;
        public List<ScenarioSkyReferenceBlock> Skies;
        public TypeValue Type;
        public FlagsValue Flags;
        public List<ScenarioChildScenarioBlock> ChildScenarios;
        public Angle LocalNorth;
        [TagField(Length = 0x14)]
        public byte[] Padding;
        [TagField(Length = 0x88)]
        public byte[] Padding1;
        public List<PredictedResourceBlock> PredictedResources;
        public List<ScenarioFunctionBlock> Functions;
        public byte[] EditorScenarioData;
        public List<EditorCommentBlock> Comments;
        [TagField(Length = 0xE0)]
        public byte[] Padding2;
        public List<ScenarioObjectNamesBlock> ObjectNames;
        public List<ScenarioSceneryBlock> Scenery;
        public List<ScenarioSceneryPaletteBlock> SceneryPalette;
        public List<ScenarioBipedBlock> Bipeds;
        public List<ScenarioBipedPaletteBlock> BipedPalette;
        public List<ScenarioVehicleBlock> Vehicles;
        public List<ScenarioVehiclePaletteBlock> VehiclePalette;
        public List<ScenarioEquipmentBlock> Equipment;
        public List<ScenarioEquipmentPaletteBlock> EquipmentPalette;
        public List<ScenarioWeaponBlock> Weapons;
        public List<ScenarioWeaponPaletteBlock> WeaponPalette;
        public List<DeviceGroupBlock> DeviceGroups;
        public List<ScenarioMachineBlock> Machines;
        public List<ScenarioMachinePaletteBlock> MachinePalette;
        public List<ScenarioControlBlock> Controls;
        public List<ScenarioControlPaletteBlock> ControlPalette;
        public List<ScenarioLightFixtureBlock> LightFixtures;
        public List<ScenarioLightFixturePaletteBlock> LightFixturesPalette;
        public List<ScenarioSoundSceneryBlock> SoundScenery;
        public List<ScenarioSoundSceneryPaletteBlock> SoundSceneryPalette;
        [TagField(Length = 0x54)]
        public byte[] Padding3;
        public List<ScenarioProfilesBlock> PlayerStartingProfile;
        public List<ScenarioPlayersBlock> PlayerStartingLocations;
        public List<ScenarioTriggerVolumeBlock> TriggerVolumes;
        public List<RecordedAnimationBlock> RecordedAnimations;
        public List<ScenarioNetgameFlagsBlock> NetgameFlags;
        public List<ScenarioNetgameEquipmentBlock> NetgameEquipment;
        public List<ScenarioStartingEquipmentBlock> StartingEquipment;
        public List<ScenarioBspSwitchTriggerVolumeBlock> BspSwitchTriggerVolumes;
        public List<ScenarioDecalsBlock> Decals;
        public List<ScenarioDecalPaletteBlock> DecalPalette;
        public List<ScenarioDetailObjectCollectionPaletteBlock> DetailObjectCollectionPalette;
        [TagField(Length = 0x54)]
        public byte[] Padding4;
        public List<ActorPaletteBlock> ActorPalette;
        public List<EncounterBlock> Encounters;
        public List<AiCommandListBlock> CommandLists;
        public List<AiAnimationReferenceBlock> AiAnimationReferences;
        public List<AiScriptReferenceBlock> AiScriptReferences;
        public List<AiRecordingReferenceBlock> AiRecordingReferences;
        public List<AiConversationBlock> AiConversations;
        public byte[] ScriptSyntaxData;
        public byte[] ScriptStringData;
        public List<HsScriptsBlock> Scripts;
        public List<HsGlobalsBlock> Globals;
        public List<HsReferencesBlock> References;
        public List<HsSourceFilesBlock> SourceFiles;
        [TagField(Length = 0x18)]
        public byte[] Padding5;
        public List<ScenarioCutsceneFlagBlock> CutsceneFlags;
        public List<ScenarioCutsceneCameraPointBlock> CutsceneCameraPoints;
        public List<ScenarioCutsceneTitleBlock> CutsceneTitles;
        [TagField(Length = 0x6C)]
        public byte[] Padding6;
        [TagField(ValidTags = new [] { "ustr" })]
        public CachedTag CustomObjectNames;
        [TagField(ValidTags = new [] { "ustr" })]
        public CachedTag IngameHelpText;
        [TagField(ValidTags = new [] { "hmt " })]
        public CachedTag HudMessages;
        public List<ScenarioStructureBspsBlock> StructureBsps;
        
        [TagStructure(Size = 0x10)]
        public class ScenarioSkyReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sky " })]
            public CachedTag Sky;
        }
        
        public enum TypeValue : short
        {
            Solo,
            Multiplayer,
            MainMenu
        }
        
        [Flags]
        public enum FlagsValue : ushort
        {
            /// <summary>
            /// sort cortana in front of other transparent geometry
            /// </summary>
            CortanaHack = 1 << 0,
            /// <summary>
            /// uses alternate UI collection for demo
            /// </summary>
            UseDemoUi = 1 << 1
        }
        
        [TagStructure(Size = 0x20)]
        public class ScenarioChildScenarioBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scnr" })]
            public CachedTag ChildScenario;
            [TagField(Length = 0x10)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResourceBlock : TagStructure
        {
            public TypeValue Type;
            public short ResourceIndex;
            public int TagIndex;
            
            public enum TypeValue : short
            {
                Bitmap,
                Sound
            }
        }
        
        [TagStructure(Size = 0x78)]
        public class ScenarioFunctionBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 32)]
            public string Name;
            /// <summary>
            /// this is the period for the above function (lower values make the function oscillate quickly, higher values make it
            /// oscillate slowly)
            /// </summary>
            public float Period; // seconds
            /// <summary>
            /// multiply this function by the above period
            /// </summary>
            public short ScalePeriodBy;
            public FunctionValue Function;
            /// <summary>
            /// multiply this function by the result of the above function
            /// </summary>
            public short ScaleFunctionBy;
            /// <summary>
            /// the curve used for the wobble
            /// </summary>
            public WobbleFunctionValue WobbleFunction;
            /// <summary>
            /// the length of time it takes for the magnitude of this function to complete a wobble
            /// </summary>
            public float WobblePeriod; // seconds
            /// <summary>
            /// the amount of random wobble in the magnitude
            /// </summary>
            public float WobbleMagnitude; // percent
            /// <summary>
            /// if non-zero, all values above the square wave threshold are snapped to 1.0, and all values below it are snapped to 0.0 to
            /// create a square wave.
            /// </summary>
            public float SquareWaveThreshold;
            /// <summary>
            /// the number of discrete values to snap to (e.g., a step count of 5 would snap the function to 0.00,0.25,0.50,0.75 or 1.00)
            /// </summary>
            public short StepCount;
            public MapToValue MapTo;
            /// <summary>
            /// the number of times this function should repeat (e.g., a sawtooth count of 5 would give the function a value of 1.0 at
            /// each of 0.25,0.50,0.75 as well as at 1.0
            /// </summary>
            public short SawtoothCount;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            /// <summary>
            /// multiply this function (from a weapon, vehicle, etc.) final result of all of the above math
            /// </summary>
            public short ScaleResultBy;
            /// <summary>
            /// controls how the bounds, below, are used
            /// </summary>
            public BoundsModeValue BoundsMode;
            public Bounds<float> Bounds;
            [TagField(Length = 0x4)]
            public byte[] Padding1;
            [TagField(Length = 0x2)]
            public byte[] Padding2;
            /// <summary>
            /// if the specified function is off, so is this function
            /// </summary>
            public short TurnOffWith;
            [TagField(Length = 0x10)]
            public byte[] Padding3;
            [TagField(Length = 0x10)]
            public byte[] Padding4;
            
            [Flags]
            public enum FlagsValue : uint
            {
                /// <summary>
                /// the level script will set this value; the other settings here will be ignored.
                /// </summary>
                Scripted = 1 << 0,
                /// <summary>
                /// result of function is one minus actual result
                /// </summary>
                Invert = 1 << 1,
                Additive = 1 << 2,
                /// <summary>
                /// function does not deactivate when at or below lower bound
                /// </summary>
                AlwaysActive = 1 << 3
            }
            
            public enum FunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
            
            public enum WobbleFunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
            
            public enum MapToValue : short
            {
                Linear,
                Early,
                VeryEarly,
                Late,
                VeryLate,
                Cosine
            }
            
            public enum BoundsModeValue : short
            {
                Clip,
                ClipAndNormalize,
                ScaleToFit
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class EditorCommentBlock : TagStructure
        {
            public RealPoint3d Position;
            [TagField(Length = 0x10)]
            public byte[] Padding;
            public byte[] Comment;
        }
        
        [TagStructure(Size = 0x24)]
        public class ScenarioObjectNamesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 0x4)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x48)]
        public class ScenarioSceneryBlock : TagStructure
        {
            public short Type;
            public short Name;
            public NotPlacedValue NotPlaced;
            /// <summary>
            /// if non-zero, will try to use model permutations with names that end in that number, e.g. 7 would pick "body-7" and
            /// "head-7"
            /// </summary>
            public short DesiredPermutation;
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            [TagField(Length = 0x8)]
            public byte[] Padding2;
            [TagField(Length = 0x8)]
            public byte[] Padding3;
            
            [Flags]
            public enum NotPlacedValue : ushort
            {
                Automatically = 1 << 0,
                OnEasy = 1 << 1,
                OnNormal = 1 << 2,
                OnHard = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioSceneryPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scen" })]
            public CachedTag Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x78)]
        public class ScenarioBipedBlock : TagStructure
        {
            public short Type;
            public short Name;
            public NotPlacedValue NotPlaced;
            /// <summary>
            /// if non-zero, will try to use model permutations with names that end in that number, e.g. 7 would pick "body-7" and
            /// "head-7"
            /// </summary>
            public short DesiredPermutation;
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            [TagField(Length = 0x8)]
            public byte[] Padding2;
            [TagField(Length = 0x8)]
            public byte[] Padding3;
            public float BodyVitality; // [0,1]
            public FlagsValue Flags;
            [TagField(Length = 0x8)]
            public byte[] Padding4;
            [TagField(Length = 0x20)]
            public byte[] Padding5;
            
            [Flags]
            public enum NotPlacedValue : ushort
            {
                Automatically = 1 << 0,
                OnEasy = 1 << 1,
                OnNormal = 1 << 2,
                OnHard = 1 << 3
            }
            
            [Flags]
            public enum FlagsValue : uint
            {
                Dead = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioBipedPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bipd" })]
            public CachedTag Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x78)]
        public class ScenarioVehicleBlock : TagStructure
        {
            public short Type;
            public short Name;
            public NotPlacedValue NotPlaced;
            /// <summary>
            /// if non-zero, will try to use model permutations with names that end in that number, e.g. 7 would pick "body-7" and
            /// "head-7"
            /// </summary>
            public short DesiredPermutation;
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            [TagField(Length = 0x8)]
            public byte[] Padding2;
            [TagField(Length = 0x8)]
            public byte[] Padding3;
            public float BodyVitality; // [0,1]
            public FlagsValue Flags;
            [TagField(Length = 0x8)]
            public byte[] Padding4;
            /// <summary>
            /// on a multiplayer map, this determines which team the vehicle belongs to for custom vehicle sets.
            /// </summary>
            public sbyte MultiplayerTeamIndex;
            [TagField(Length = 0x1)]
            public byte[] Padding5;
            public MultiplayerSpawnFlagsValue MultiplayerSpawnFlags;
            [TagField(Length = 0x1C)]
            public byte[] Padding6;
            
            [Flags]
            public enum NotPlacedValue : ushort
            {
                Automatically = 1 << 0,
                OnEasy = 1 << 1,
                OnNormal = 1 << 2,
                OnHard = 1 << 3
            }
            
            [Flags]
            public enum FlagsValue : uint
            {
                Dead = 1 << 0
            }
            
            [Flags]
            public enum MultiplayerSpawnFlagsValue : ushort
            {
                /// <summary>
                /// vehicle will spawn when default vehicle set is used in a slayer game
                /// </summary>
                SlayerDefault = 1 << 0,
                /// <summary>
                /// vehicle will spawn when default vehicle set is used in a ctf game
                /// </summary>
                CtfDefault = 1 << 1,
                /// <summary>
                /// vehicle will spawn when default vehicle set is used in a king game
                /// </summary>
                KingDefault = 1 << 2,
                /// <summary>
                /// vehicle will spawn when default vehicle set is used in a oddball game
                /// </summary>
                OddballDefault = 1 << 3,
                /// <summary>
                /// unused
                /// </summary>
                Unused = 1 << 4,
                /// <summary>
                /// unused
                /// </summary>
                Unused1 = 1 << 5,
                /// <summary>
                /// unused
                /// </summary>
                Unused2 = 1 << 6,
                /// <summary>
                /// unused
                /// </summary>
                Unused3 = 1 << 7,
                /// <summary>
                /// vehicle can spawn in a slayer game
                /// </summary>
                SlayerAllowed = 1 << 8,
                /// <summary>
                /// vehicle can spawn in a ctf game
                /// </summary>
                CtfAllowed = 1 << 9,
                /// <summary>
                /// vehicle can spawn in a king game
                /// </summary>
                KingAllowed = 1 << 10,
                /// <summary>
                /// vehicle can spawn in a oddball game
                /// </summary>
                OddballAllowed = 1 << 11,
                /// <summary>
                /// unused
                /// </summary>
                Unused4 = 1 << 12,
                /// <summary>
                /// unused
                /// </summary>
                Unused5 = 1 << 13,
                /// <summary>
                /// unused
                /// </summary>
                Unused6 = 1 << 14,
                /// <summary>
                /// unused
                /// </summary>
                Unused7 = 1 << 15
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioVehiclePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "vehi" })]
            public CachedTag Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioEquipmentBlock : TagStructure
        {
            public short Type;
            public short Name;
            public NotPlacedValue NotPlaced;
            /// <summary>
            /// if non-zero, will try to use model permutations with names that end in that number, e.g. 7 would pick "body-7" and
            /// "head-7"
            /// </summary>
            public short DesiredPermutation;
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public MiscFlagsValue MiscFlags;
            [TagField(Length = 0x4)]
            public byte[] Padding1;
            
            [Flags]
            public enum NotPlacedValue : ushort
            {
                Automatically = 1 << 0,
                OnEasy = 1 << 1,
                OnNormal = 1 << 2,
                OnHard = 1 << 3
            }
            
            [Flags]
            public enum MiscFlagsValue : ushort
            {
                InitiallyAtRestDoesnTFall = 1 << 0,
                Obsolete = 1 << 1,
                DoesAccelerateMovesDueToExplosions = 1 << 2
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioEquipmentPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x5C)]
        public class ScenarioWeaponBlock : TagStructure
        {
            public short Type;
            public short Name;
            public NotPlacedValue NotPlaced;
            /// <summary>
            /// if non-zero, will try to use model permutations with names that end in that number, e.g. 7 would pick "body-7" and
            /// "head-7"
            /// </summary>
            public short DesiredPermutation;
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            [TagField(Length = 0x8)]
            public byte[] Padding2;
            [TagField(Length = 0x8)]
            public byte[] Padding3;
            public short RoundsLeft;
            public short RoundsLoaded;
            public FlagsValue Flags;
            [TagField(Length = 0x2)]
            public byte[] Padding4;
            [TagField(Length = 0xC)]
            public byte[] Padding5;
            
            [Flags]
            public enum NotPlacedValue : ushort
            {
                Automatically = 1 << 0,
                OnEasy = 1 << 1,
                OnNormal = 1 << 2,
                OnHard = 1 << 3
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                InitiallyAtRestDoesnTFall = 1 << 0,
                Obsolete = 1 << 1,
                DoesAccelerateMovesDueToExplosions = 1 << 2
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioWeaponPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x34)]
        public class DeviceGroupBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public float InitialValue; // [0,1]
            public FlagsValue Flags;
            [TagField(Length = 0xC)]
            public byte[] Padding;
            
            [Flags]
            public enum FlagsValue : uint
            {
                CanChangeOnlyOnce = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class ScenarioMachineBlock : TagStructure
        {
            public short Type;
            public short Name;
            public NotPlacedValue NotPlaced;
            /// <summary>
            /// if non-zero, will try to use model permutations with names that end in that number, e.g. 7 would pick "body-7" and
            /// "head-7"
            /// </summary>
            public short DesiredPermutation;
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            public short PowerGroup;
            public short PositionGroup;
            public FlagsValue Flags;
            public Flags1Value Flags1;
            [TagField(Length = 0xC)]
            public byte[] Padding1;
            
            [Flags]
            public enum NotPlacedValue : ushort
            {
                Automatically = 1 << 0,
                OnEasy = 1 << 1,
                OnNormal = 1 << 2,
                OnHard = 1 << 3
            }
            
            [Flags]
            public enum FlagsValue : uint
            {
                InitiallyOpen10 = 1 << 0,
                InitiallyOff00 = 1 << 1,
                CanChangeOnlyOnce = 1 << 2,
                PositionReversed = 1 << 3,
                NotUsableFromAnySide = 1 << 4
            }
            
            [Flags]
            public enum Flags1Value : uint
            {
                DoesNotOperateAutomatically = 1 << 0,
                OneSided = 1 << 1,
                NeverAppearsLocked = 1 << 2,
                OpenedByMeleeAttack = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioMachinePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "mach" })]
            public CachedTag Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x40)]
        public class ScenarioControlBlock : TagStructure
        {
            public short Type;
            public short Name;
            public NotPlacedValue NotPlaced;
            /// <summary>
            /// if non-zero, will try to use model permutations with names that end in that number, e.g. 7 would pick "body-7" and
            /// "head-7"
            /// </summary>
            public short DesiredPermutation;
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            public short PowerGroup;
            public short PositionGroup;
            public FlagsValue Flags;
            public Flags1Value Flags1;
            public short DonTTouchThis;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x8)]
            public byte[] Padding2;
            
            [Flags]
            public enum NotPlacedValue : ushort
            {
                Automatically = 1 << 0,
                OnEasy = 1 << 1,
                OnNormal = 1 << 2,
                OnHard = 1 << 3
            }
            
            [Flags]
            public enum FlagsValue : uint
            {
                InitiallyOpen10 = 1 << 0,
                InitiallyOff00 = 1 << 1,
                CanChangeOnlyOnce = 1 << 2,
                PositionReversed = 1 << 3,
                NotUsableFromAnySide = 1 << 4
            }
            
            [Flags]
            public enum Flags1Value : uint
            {
                UsableFromBothSides = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioControlPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ctrl" })]
            public CachedTag Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x58)]
        public class ScenarioLightFixtureBlock : TagStructure
        {
            public short Type;
            public short Name;
            public NotPlacedValue NotPlaced;
            /// <summary>
            /// if non-zero, will try to use model permutations with names that end in that number, e.g. 7 would pick "body-7" and
            /// "head-7"
            /// </summary>
            public short DesiredPermutation;
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            public short PowerGroup;
            public short PositionGroup;
            public FlagsValue Flags;
            public RealRgbColor Color;
            public float Intensity;
            public Angle FalloffAngle; // degrees
            public Angle CutoffAngle; // degrees
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            
            [Flags]
            public enum NotPlacedValue : ushort
            {
                Automatically = 1 << 0,
                OnEasy = 1 << 1,
                OnNormal = 1 << 2,
                OnHard = 1 << 3
            }
            
            [Flags]
            public enum FlagsValue : uint
            {
                InitiallyOpen10 = 1 << 0,
                InitiallyOff00 = 1 << 1,
                CanChangeOnlyOnce = 1 << 2,
                PositionReversed = 1 << 3,
                NotUsableFromAnySide = 1 << 4
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioLightFixturePaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "lifi" })]
            public CachedTag Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioSoundSceneryBlock : TagStructure
        {
            public short Type;
            public short Name;
            public NotPlacedValue NotPlaced;
            /// <summary>
            /// if non-zero, will try to use model permutations with names that end in that number, e.g. 7 would pick "body-7" and
            /// "head-7"
            /// </summary>
            public short DesiredPermutation;
            public RealPoint3d Position;
            public RealEulerAngles3d Rotation;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            
            [Flags]
            public enum NotPlacedValue : ushort
            {
                Automatically = 1 << 0,
                OnEasy = 1 << 1,
                OnNormal = 1 << 2,
                OnHard = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioSoundSceneryPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ssce" })]
            public CachedTag Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x68)]
        public class ScenarioProfilesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public float StartingHealthModifier; // [0,1]
            public float StartingShieldModifier; // [0,1]
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag PrimaryWeapon;
            public short RoundsLoaded;
            public short RoundsTotal;
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag SecondaryWeapon;
            public short RoundsLoaded1;
            public short RoundsTotal1;
            public sbyte StartingFragmentationGrenadeCount;
            public sbyte StartingPlasmaGrenadeCount;
            public sbyte StartingUnknownGrenadeCount;
            public sbyte StartingUnknownGrenadeCount1;
            [TagField(Length = 0x14)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x34)]
        public class ScenarioPlayersBlock : TagStructure
        {
            public RealPoint3d Position;
            public Angle Facing; // degrees
            public short TeamIndex;
            public short BspIndex;
            public Type0Value Type0;
            public Type1Value Type1;
            public Type2Value Type2;
            public Type3Value Type3;
            [TagField(Length = 0x18)]
            public byte[] Padding;
            
            public enum Type0Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
            
            public enum Type1Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
            
            public enum Type2Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
            
            public enum Type3Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
        }
        
        [TagStructure(Size = 0x60)]
        public class ScenarioTriggerVolumeBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Unknown;
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 15)]
            public float[] Unknown1;
        }
        
        [TagStructure(Size = 0x40)]
        public class RecordedAnimationBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public sbyte Version;
            public sbyte RawAnimationData;
            public sbyte UnitControlDataVersion;
            [TagField(Length = 0x1)]
            public byte[] Padding;
            public short LengthOfAnimation; // ticks
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x4)]
            public byte[] Padding2;
            public byte[] RecordedAnimationEventStream;
        }
        
        [TagStructure(Size = 0x94)]
        public class ScenarioNetgameFlagsBlock : TagStructure
        {
            public RealPoint3d Position;
            public Angle Facing; // degress
            public TypeValue Type;
            public short TeamIndex;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag WeaponGroup;
            [TagField(Length = 0x70)]
            public byte[] Padding;
            
            public enum TypeValue : short
            {
                CtfFlag,
                CtfVehicle,
                OddballBallSpawn,
                RaceTrack,
                RaceVehicle,
                VegasBank,
                TeleportFrom,
                TeleportTo,
                HillFlag
            }
        }
        
        [TagStructure(Size = 0x90)]
        public class ScenarioNetgameEquipmentBlock : TagStructure
        {
            public FlagsValue Flags;
            public Type0Value Type0;
            public Type1Value Type1;
            public Type2Value Type2;
            public Type3Value Type3;
            public short TeamIndex;
            public short SpawnTimeInSeconds0Default;
            [TagField(Length = 0x30)]
            public byte[] Padding;
            public RealPoint3d Position;
            public Angle Facing; // degress
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection;
            [TagField(Length = 0x30)]
            public byte[] Padding1;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Levitate = 1 << 0
            }
            
            public enum Type0Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
            
            public enum Type1Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
            
            public enum Type2Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
            
            public enum Type3Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
        }
        
        [TagStructure(Size = 0xCC)]
        public class ScenarioStartingEquipmentBlock : TagStructure
        {
            public FlagsValue Flags;
            public Type0Value Type0;
            public Type1Value Type1;
            public Type2Value Type2;
            public Type3Value Type3;
            [TagField(Length = 0x30)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection1;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection2;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection3;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection4;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection5;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag ItemCollection6;
            [TagField(Length = 0x30)]
            public byte[] Padding1;
            
            [Flags]
            public enum FlagsValue : uint
            {
                NoGrenades = 1 << 0,
                PlasmaGrenades = 1 << 1
            }
            
            public enum Type0Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
            
            public enum Type1Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
            
            public enum Type2Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
            
            public enum Type3Value : short
            {
                None,
                Ctf,
                Slayer,
                Oddball,
                KingOfTheHill,
                Race,
                Terminator,
                Stub,
                Ignored1,
                Ignored2,
                Ignored3,
                Ignored4,
                AllGames,
                AllExceptCtf,
                AllExceptRaceCtf
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class ScenarioBspSwitchTriggerVolumeBlock : TagStructure
        {
            public short TriggerVolume;
            public short Source;
            public short Destination;
            [TagField(Length = 0x2)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioDecalsBlock : TagStructure
        {
            public short DecalType;
            public sbyte Yaw127127;
            public sbyte Pitch127127;
            public RealPoint3d Position;
        }
        
        [TagStructure(Size = 0x10)]
        public class ScenarioDecalPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "deca" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioDetailObjectCollectionPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "dobc" })]
            public CachedTag Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x10)]
        public class ActorPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "actv" })]
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0xB0)]
        public class EncounterBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            public TeamIndexValue TeamIndex;
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            public SearchBehaviorValue SearchBehavior;
            /// <summary>
            /// structure bsp index that this encounter belongs to... ignored unless 'manual bsp index' flag is checked. if this flag is
            /// not checked, the structure bsp is found automatically (may fail in areas of overlapping geometry)
            /// </summary>
            public short ManualBspIndex;
            /// <summary>
            /// delay between respawning actors in this encounter
            /// </summary>
            public Bounds<float> RespawnDelay; // seconds
            [TagField(Length = 0x4C)]
            public byte[] Padding;
            public List<SquadsBlock> Squads;
            public List<PlatoonsBlock> Platoons;
            public List<FiringPositionsBlock> FiringPositions;
            public List<ScenarioPlayersBlock> PlayerStartingLocations;
            
            [Flags]
            public enum FlagsValue : uint
            {
                NotInitiallyCreated = 1 << 0,
                RespawnEnabled = 1 << 1,
                InitiallyBlind = 1 << 2,
                InitiallyDeaf = 1 << 3,
                InitiallyBraindead = 1 << 4,
                _3dFiringPositions = 1 << 5,
                ManualBspIndexSpecified = 1 << 6
            }
            
            public enum TeamIndexValue : short
            {
                _0DefaultByUnit,
                _1Player,
                _2Human,
                _3Covenant,
                _4Flood,
                _5Sentinel,
                _6Unused6,
                _7Unused7,
                _8Unused8,
                _9Unused9
            }
            
            public enum SearchBehaviorValue : short
            {
                Normal,
                Never,
                Tenacious
            }
            
            [TagStructure(Size = 0xE8)]
            public class SquadsBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public short ActorType;
                public short Platoon;
                /// <summary>
                /// state that this actor starts in
                /// </summary>
                public InitialStateValue InitialState;
                /// <summary>
                /// state that this actor will return to when it has nothing to do
                /// </summary>
                public ReturnStateValue ReturnState;
                public FlagsValue Flags;
                /// <summary>
                /// what kind of leader this squad has (e.g. a sarge for marines) - 'normal' is based on the size of the squad, 'random'
                /// always creates a leader, or you can specify an individual type
                /// </summary>
                public UniqueLeaderTypeValue UniqueLeaderType;
                [TagField(Length = 0x2)]
                public byte[] Padding;
                [TagField(Length = 0x1C)]
                public byte[] Padding1;
                [TagField(Length = 0x2)]
                public byte[] Padding2;
                public short ManeuverToSquad;
                public float SquadDelayTime; // seconds
                public AttackingValue Attacking;
                public AttackingSearchValue AttackingSearch;
                public AttackingGuardValue AttackingGuard;
                public DefendingValue Defending;
                public DefendingSearchValue DefendingSearch;
                public DefendingGuardValue DefendingGuard;
                public PursuingValue Pursuing;
                [TagField(Length = 0x4)]
                public byte[] Padding3;
                [TagField(Length = 0x8)]
                public byte[] Padding4;
                /// <summary>
                /// initial number of actors on normal difficulty
                /// </summary>
                public short NormalDiffCount;
                /// <summary>
                /// initial number of actors on insane difficulty (hard difficulty is midway between normal and insane)
                /// </summary>
                public short InsaneDiffCount;
                public MajorUpgradeValue MajorUpgrade;
                [TagField(Length = 0x2)]
                public byte[] Padding5;
                /// <summary>
                /// minimum number of actors alive at once (will spawn instantly if less than this number)
                /// </summary>
                public short RespawnMinActors;
                /// <summary>
                /// maximum number of actors alive at once (never spawns above this number)
                /// </summary>
                public short RespawnMaxActors;
                /// <summary>
                /// total number to respawn (zero = infinite)
                /// </summary>
                public short RespawnTotal;
                [TagField(Length = 0x2)]
                public byte[] Padding6;
                /// <summary>
                /// delay between respawning actors in this squad
                /// </summary>
                public Bounds<float> RespawnDelay; // seconds
                [TagField(Length = 0x30)]
                public byte[] Padding7;
                public List<MovePositionsBlock> MovePositions;
                public List<ActorStartingLocationsBlock> StartingLocations;
                [TagField(Length = 0xC)]
                public byte[] Padding8;
                
                public enum InitialStateValue : short
                {
                    None,
                    Sleeping,
                    Alert,
                    MovingRepeatSamePosition,
                    MovingLoop,
                    MovingLoopBackAndForth,
                    MovingLoopRandomly,
                    MovingRandomly,
                    Guarding,
                    GuardingAtGuardPosition,
                    Searching,
                    Fleeing
                }
                
                public enum ReturnStateValue : short
                {
                    None,
                    Sleeping,
                    Alert,
                    MovingRepeatSamePosition,
                    MovingLoop,
                    MovingLoopBackAndForth,
                    MovingLoopRandomly,
                    MovingRandomly,
                    Guarding,
                    GuardingAtGuardPosition,
                    Searching,
                    Fleeing
                }
                
                [Flags]
                public enum FlagsValue : uint
                {
                    Unused = 1 << 0,
                    NeverSearch = 1 << 1,
                    StartTimerImmediately = 1 << 2,
                    NoTimerDelayForever = 1 << 3,
                    MagicSightAfterTimer = 1 << 4,
                    AutomaticMigration = 1 << 5
                }
                
                public enum UniqueLeaderTypeValue : short
                {
                    Normal,
                    None,
                    Random,
                    SgtJohnson,
                    SgtLehto
                }
                
                [Flags]
                public enum AttackingValue : uint
                {
                    A = 1 << 0,
                    B = 1 << 1,
                    C = 1 << 2,
                    D = 1 << 3,
                    E = 1 << 4,
                    F = 1 << 5,
                    G = 1 << 6,
                    H = 1 << 7,
                    I = 1 << 8,
                    J = 1 << 9,
                    K = 1 << 10,
                    L = 1 << 11,
                    M = 1 << 12,
                    N = 1 << 13,
                    O = 1 << 14,
                    P = 1 << 15,
                    Q = 1 << 16,
                    R = 1 << 17,
                    S = 1 << 18,
                    T = 1 << 19,
                    U = 1 << 20,
                    V = 1 << 21,
                    W = 1 << 22,
                    X = 1 << 23,
                    Y = 1 << 24,
                    Z = 1 << 25
                }
                
                [Flags]
                public enum AttackingSearchValue : uint
                {
                    A = 1 << 0,
                    B = 1 << 1,
                    C = 1 << 2,
                    D = 1 << 3,
                    E = 1 << 4,
                    F = 1 << 5,
                    G = 1 << 6,
                    H = 1 << 7,
                    I = 1 << 8,
                    J = 1 << 9,
                    K = 1 << 10,
                    L = 1 << 11,
                    M = 1 << 12,
                    N = 1 << 13,
                    O = 1 << 14,
                    P = 1 << 15,
                    Q = 1 << 16,
                    R = 1 << 17,
                    S = 1 << 18,
                    T = 1 << 19,
                    U = 1 << 20,
                    V = 1 << 21,
                    W = 1 << 22,
                    X = 1 << 23,
                    Y = 1 << 24,
                    Z = 1 << 25
                }
                
                [Flags]
                public enum AttackingGuardValue : uint
                {
                    A = 1 << 0,
                    B = 1 << 1,
                    C = 1 << 2,
                    D = 1 << 3,
                    E = 1 << 4,
                    F = 1 << 5,
                    G = 1 << 6,
                    H = 1 << 7,
                    I = 1 << 8,
                    J = 1 << 9,
                    K = 1 << 10,
                    L = 1 << 11,
                    M = 1 << 12,
                    N = 1 << 13,
                    O = 1 << 14,
                    P = 1 << 15,
                    Q = 1 << 16,
                    R = 1 << 17,
                    S = 1 << 18,
                    T = 1 << 19,
                    U = 1 << 20,
                    V = 1 << 21,
                    W = 1 << 22,
                    X = 1 << 23,
                    Y = 1 << 24,
                    Z = 1 << 25
                }
                
                [Flags]
                public enum DefendingValue : uint
                {
                    A = 1 << 0,
                    B = 1 << 1,
                    C = 1 << 2,
                    D = 1 << 3,
                    E = 1 << 4,
                    F = 1 << 5,
                    G = 1 << 6,
                    H = 1 << 7,
                    I = 1 << 8,
                    J = 1 << 9,
                    K = 1 << 10,
                    L = 1 << 11,
                    M = 1 << 12,
                    N = 1 << 13,
                    O = 1 << 14,
                    P = 1 << 15,
                    Q = 1 << 16,
                    R = 1 << 17,
                    S = 1 << 18,
                    T = 1 << 19,
                    U = 1 << 20,
                    V = 1 << 21,
                    W = 1 << 22,
                    X = 1 << 23,
                    Y = 1 << 24,
                    Z = 1 << 25
                }
                
                [Flags]
                public enum DefendingSearchValue : uint
                {
                    A = 1 << 0,
                    B = 1 << 1,
                    C = 1 << 2,
                    D = 1 << 3,
                    E = 1 << 4,
                    F = 1 << 5,
                    G = 1 << 6,
                    H = 1 << 7,
                    I = 1 << 8,
                    J = 1 << 9,
                    K = 1 << 10,
                    L = 1 << 11,
                    M = 1 << 12,
                    N = 1 << 13,
                    O = 1 << 14,
                    P = 1 << 15,
                    Q = 1 << 16,
                    R = 1 << 17,
                    S = 1 << 18,
                    T = 1 << 19,
                    U = 1 << 20,
                    V = 1 << 21,
                    W = 1 << 22,
                    X = 1 << 23,
                    Y = 1 << 24,
                    Z = 1 << 25
                }
                
                [Flags]
                public enum DefendingGuardValue : uint
                {
                    A = 1 << 0,
                    B = 1 << 1,
                    C = 1 << 2,
                    D = 1 << 3,
                    E = 1 << 4,
                    F = 1 << 5,
                    G = 1 << 6,
                    H = 1 << 7,
                    I = 1 << 8,
                    J = 1 << 9,
                    K = 1 << 10,
                    L = 1 << 11,
                    M = 1 << 12,
                    N = 1 << 13,
                    O = 1 << 14,
                    P = 1 << 15,
                    Q = 1 << 16,
                    R = 1 << 17,
                    S = 1 << 18,
                    T = 1 << 19,
                    U = 1 << 20,
                    V = 1 << 21,
                    W = 1 << 22,
                    X = 1 << 23,
                    Y = 1 << 24,
                    Z = 1 << 25
                }
                
                [Flags]
                public enum PursuingValue : uint
                {
                    A = 1 << 0,
                    B = 1 << 1,
                    C = 1 << 2,
                    D = 1 << 3,
                    E = 1 << 4,
                    F = 1 << 5,
                    G = 1 << 6,
                    H = 1 << 7,
                    I = 1 << 8,
                    J = 1 << 9,
                    K = 1 << 10,
                    L = 1 << 11,
                    M = 1 << 12,
                    N = 1 << 13,
                    O = 1 << 14,
                    P = 1 << 15,
                    Q = 1 << 16,
                    R = 1 << 17,
                    S = 1 << 18,
                    T = 1 << 19,
                    U = 1 << 20,
                    V = 1 << 21,
                    W = 1 << 22,
                    X = 1 << 23,
                    Y = 1 << 24,
                    Z = 1 << 25
                }
                
                public enum MajorUpgradeValue : short
                {
                    Normal,
                    Few,
                    Many,
                    None,
                    All
                }
                
                [TagStructure(Size = 0x50)]
                public class MovePositionsBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public Angle Facing; // degrees
                    public float Weight;
                    public Bounds<float> Time; // seconds
                    public short Animation;
                    /// <summary>
                    /// identifies this move position as belonging to a sequence, only actors whose starting locations match this sequence ID can
                    /// use it (zero = no sequence)
                    /// </summary>
                    public sbyte SequenceId;
                    [TagField(Length = 0x1)]
                    public byte[] Padding;
                    [TagField(Length = 0x2C)]
                    public byte[] Padding1;
                    public int SurfaceIndex;
                }
                
                [TagStructure(Size = 0x1C)]
                public class ActorStartingLocationsBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public Angle Facing; // degrees
                    [TagField(Length = 0x2)]
                    public byte[] Padding;
                    /// <summary>
                    /// which move position sequence we can use (zero = no specific sequences)
                    /// </summary>
                    public sbyte SequenceId;
                    public FlagsValue Flags;
                    /// <summary>
                    /// state that this actor will return to when it has nothing to do
                    /// </summary>
                    public ReturnStateValue ReturnState;
                    /// <summary>
                    /// state that this actor starts in
                    /// </summary>
                    public InitialStateValue InitialState;
                    public short ActorType;
                    public short CommandList;
                    
                    [Flags]
                    public enum FlagsValue : byte
                    {
                        Required = 1 << 0
                    }
                    
                    public enum ReturnStateValue : short
                    {
                        None,
                        Sleeping,
                        Alert,
                        MovingRepeatSamePosition,
                        MovingLoop,
                        MovingLoopBackAndForth,
                        MovingLoopRandomly,
                        MovingRandomly,
                        Guarding,
                        GuardingAtGuardPosition,
                        Searching,
                        Fleeing
                    }
                    
                    public enum InitialStateValue : short
                    {
                        None,
                        Sleeping,
                        Alert,
                        MovingRepeatSamePosition,
                        MovingLoop,
                        MovingLoopBackAndForth,
                        MovingLoopRandomly,
                        MovingRandomly,
                        Guarding,
                        GuardingAtGuardPosition,
                        Searching,
                        Fleeing
                    }
                }
            }
            
            [TagStructure(Size = 0xAC)]
            public class PlatoonsBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public FlagsValue Flags;
                [TagField(Length = 0xC)]
                public byte[] Padding;
                public ChangeAttackingDefendingStateWhenValue ChangeAttackingDefendingStateWhen;
                public short HappensTo;
                [TagField(Length = 0x4)]
                public byte[] Padding1;
                [TagField(Length = 0x4)]
                public byte[] Padding2;
                public ManeuverWhenValue ManeuverWhen;
                public short HappensTo1;
                [TagField(Length = 0x4)]
                public byte[] Padding3;
                [TagField(Length = 0x4)]
                public byte[] Padding4;
                [TagField(Length = 0x40)]
                public byte[] Padding5;
                [TagField(Length = 0x24)]
                public byte[] Padding6;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    FleeWhenManeuvering = 1 << 0,
                    SayAdvancingWhenManeuver = 1 << 1,
                    StartInDefendingState = 1 << 2
                }
                
                public enum ChangeAttackingDefendingStateWhenValue : short
                {
                    Never,
                    _75Strength,
                    _50Strength,
                    _25Strength,
                    AnybodyDead,
                    _25Dead,
                    _50Dead,
                    _75Dead,
                    AllButOneDead,
                    AllDead
                }
                
                public enum ManeuverWhenValue : short
                {
                    Never,
                    _75Strength,
                    _50Strength,
                    _25Strength,
                    AnybodyDead,
                    _25Dead,
                    _50Dead,
                    _75Dead,
                    AllButOneDead,
                    AllDead
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class FiringPositionsBlock : TagStructure
            {
                public RealPoint3d Position;
                public GroupIndexValue GroupIndex;
                [TagField(Length = 0xA)]
                public byte[] Padding;
                
                public enum GroupIndexValue : short
                {
                    A,
                    B,
                    C,
                    D,
                    E,
                    F,
                    G,
                    H,
                    I,
                    J,
                    K,
                    L,
                    M,
                    N,
                    O,
                    P,
                    Q,
                    R,
                    S,
                    T,
                    U,
                    V,
                    W,
                    X,
                    Y,
                    Z
                }
            }
            
            [TagStructure(Size = 0x34)]
            public class ScenarioPlayersBlock : TagStructure
            {
                public RealPoint3d Position;
                public Angle Facing; // degrees
                public short TeamIndex;
                public short BspIndex;
                public Type0Value Type0;
                public Type1Value Type1;
                public Type2Value Type2;
                public Type3Value Type3;
                [TagField(Length = 0x18)]
                public byte[] Padding;
                
                public enum Type0Value : short
                {
                    None,
                    Ctf,
                    Slayer,
                    Oddball,
                    KingOfTheHill,
                    Race,
                    Terminator,
                    Stub,
                    Ignored1,
                    Ignored2,
                    Ignored3,
                    Ignored4,
                    AllGames,
                    AllExceptCtf,
                    AllExceptRaceCtf
                }
                
                public enum Type1Value : short
                {
                    None,
                    Ctf,
                    Slayer,
                    Oddball,
                    KingOfTheHill,
                    Race,
                    Terminator,
                    Stub,
                    Ignored1,
                    Ignored2,
                    Ignored3,
                    Ignored4,
                    AllGames,
                    AllExceptCtf,
                    AllExceptRaceCtf
                }
                
                public enum Type2Value : short
                {
                    None,
                    Ctf,
                    Slayer,
                    Oddball,
                    KingOfTheHill,
                    Race,
                    Terminator,
                    Stub,
                    Ignored1,
                    Ignored2,
                    Ignored3,
                    Ignored4,
                    AllGames,
                    AllExceptCtf,
                    AllExceptRaceCtf
                }
                
                public enum Type3Value : short
                {
                    None,
                    Ctf,
                    Slayer,
                    Oddball,
                    KingOfTheHill,
                    Race,
                    Terminator,
                    Stub,
                    Ignored1,
                    Ignored2,
                    Ignored3,
                    Ignored4,
                    AllGames,
                    AllExceptCtf,
                    AllExceptRaceCtf
                }
            }
        }
        
        [TagStructure(Size = 0x60)]
        public class AiCommandListBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            /// <summary>
            /// structure bsp index that this encounter belongs to... ignored unless 'manual bsp index' flag is checked. if this flag is
            /// not checked, the structure bsp is found automatically (may fail in areas of overlapping geometry)
            /// </summary>
            public short ManualBspIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            public List<AiCommandBlock> Commands;
            public List<AiCommandPointBlock> Points;
            [TagField(Length = 0x18)]
            public byte[] Padding2;
            
            [Flags]
            public enum FlagsValue : uint
            {
                /// <summary>
                /// lets an actor decide to stop following its commands and attack an enemy
                /// </summary>
                AllowInitiative = 1 << 0,
                /// <summary>
                /// lets an actor shoot at enemies while following its commands
                /// </summary>
                AllowTargeting = 1 << 1,
                /// <summary>
                /// stops an actor from turning, stopping or looking around in response to stimuli received while
                /// following its commands
                /// </summary>
                DisableLooking = 1 << 2,
                /// <summary>
                /// stops an actor from communicating while following its commands
                /// </summary>
                DisableCommunication = 1 << 3,
                /// <summary>
                /// makes an actor not take any damage from falling while following its commands
                /// </summary>
                DisableFallingDamage = 1 << 4,
                /// <summary>
                /// set if the command list is manually attached to a specific bsp
                /// </summary>
                ManualBspIndex = 1 << 5
            }
            
            [TagStructure(Size = 0x20)]
            public class AiCommandBlock : TagStructure
            {
                public AtomTypeValue AtomType;
                public short AtomModifier;
                public float Parameter1;
                public float Parameter2;
                public short Point1;
                public short Point2;
                public short Animation;
                public short Script;
                public short Recording;
                public short Command;
                public short ObjectName;
                [TagField(Length = 0x6)]
                public byte[] Padding;
                
                public enum AtomTypeValue : short
                {
                    Pause,
                    GoTo,
                    GoToAndFace,
                    MoveInDirection,
                    Look,
                    AnimationMode,
                    Crouch,
                    Shoot,
                    Grenade,
                    Vehicle,
                    RunningJump,
                    TargetedJump,
                    Script,
                    Animate,
                    Recording,
                    Action,
                    Vocalize,
                    Targeting,
                    Initiative,
                    Wait,
                    Loop,
                    Die,
                    MoveImmediate,
                    LookRandom,
                    LookPlayer,
                    LookObject,
                    SetRadius,
                    Teleport
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class AiCommandPointBlock : TagStructure
            {
                public RealPoint3d Position;
                [TagField(Length = 0x8)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class AiAnimationReferenceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string AnimationName;
            /// <summary>
            /// leave this blank to use the unit's normal animation graph
            /// </summary>
            [TagField(ValidTags = new [] { "antr" })]
            public CachedTag AnimationGraph;
            [TagField(Length = 0xC)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x28)]
        public class AiScriptReferenceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string ScriptName;
            [TagField(Length = 0x8)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x28)]
        public class AiRecordingReferenceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string RecordingName;
            [TagField(Length = 0x8)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x74)]
        public class AiConversationBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            /// <summary>
            /// distance the player must enter before the conversation can trigger
            /// </summary>
            public float TriggerDistance; // world units
            /// <summary>
            /// if the 'involves player' flag is set, when triggered the conversation's participant(s) will run to within this distance
            /// of the player
            /// </summary>
            public float RunToPlayerDist; // world units
            [TagField(Length = 0x24)]
            public byte[] Padding1;
            public List<AiConversationParticipantBlock> Participants;
            public List<AiConversationLineBlock> Lines;
            [TagField(Length = 0xC)]
            public byte[] Padding2;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                /// <summary>
                /// this conversation will be aborted if any participant dies
                /// </summary>
                StopIfDeath = 1 << 0,
                /// <summary>
                /// an actor will abort this conversation if they are damaged
                /// </summary>
                StopIfDamaged = 1 << 1,
                /// <summary>
                /// an actor will abort this conversation if they see an enemy
                /// </summary>
                StopIfVisibleEnemy = 1 << 2,
                /// <summary>
                /// an actor will abort this conversation if they suspect an enemy
                /// </summary>
                StopIfAlertedToEnemy = 1 << 3,
                /// <summary>
                /// this conversation cannot take place unless the participants can _see_ a nearby player
                /// </summary>
                PlayerMustBeVisible = 1 << 4,
                /// <summary>
                /// participants stop doing whatever they were doing in order to perform this conversation
                /// </summary>
                StopOtherActions = 1 << 5,
                /// <summary>
                /// if this conversation fails initially, it will keep testing to see when it can play
                /// </summary>
                KeepTryingToPlay = 1 << 6,
                /// <summary>
                /// this conversation will not start until the player is looking at one of the participants
                /// </summary>
                PlayerMustBeLooking = 1 << 7
            }
            
            [TagStructure(Size = 0x54)]
            public class AiConversationParticipantBlock : TagStructure
            {
                [TagField(Length = 0x2)]
                public byte[] Padding;
                public FlagsValue Flags;
                public SelectionTypeValue SelectionType;
                public ActorTypeValue ActorType;
                /// <summary>
                /// if a unit with this name exists, we try to pick them to start the conversation
                /// </summary>
                public short UseThisObject;
                /// <summary>
                /// once we pick a unit, we name it this
                /// </summary>
                public short SetNewName;
                [TagField(Length = 0xC)]
                public byte[] Padding1;
                [TagField(Length = 0xC)]
                public byte[] Padding2;
                [TagField(Length = 32)]
                public string EncounterName;
                [TagField(Length = 0x4)]
                public byte[] Padding3;
                [TagField(Length = 0xC)]
                public byte[] Padding4;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    /// <summary>
                    /// the conversation can continue even if nobody for this participant was found
                    /// </summary>
                    Optional = 1 << 0,
                    /// <summary>
                    /// if nobody for this participant can be found, we use a participant marked as 'is alternate' instead,
                    /// e.g. someone from over the radio
                    /// </summary>
                    HasAlternate = 1 << 1,
                    /// <summary>
                    /// this participant is only used if some participant in this conversation was marked as 'has alternate'
                    /// could not be found
                    /// </summary>
                    IsAlternate = 1 << 2
                }
                
                public enum SelectionTypeValue : short
                {
                    FriendlyActor,
                    Disembodied,
                    InPlayerSVehicle,
                    NotInAVehicle,
                    PreferSergeant,
                    AnyActor,
                    RadioUnit,
                    RadioSergeant
                }
                
                public enum ActorTypeValue : short
                {
                    Elite,
                    Jackal,
                    Grunt,
                    Hunter,
                    Engineer,
                    Assassin,
                    Player,
                    Marine,
                    Crew,
                    CombatForm,
                    InfectionForm,
                    CarrierForm,
                    Monitor,
                    Sentinel,
                    None,
                    MountedWeapon
                }
            }
            
            [TagStructure(Size = 0x7C)]
            public class AiConversationLineBlock : TagStructure
            {
                public FlagsValue Flags;
                public short Participant;
                public AddresseeValue Addressee;
                /// <summary>
                /// this field is only used if the addressee type is 'participant'
                /// </summary>
                public short AddresseeParticipant;
                [TagField(Length = 0x4)]
                public byte[] Padding;
                public float LineDelayTime;
                [TagField(Length = 0xC)]
                public byte[] Padding1;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant1;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant2;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant3;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant5;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Variant6;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    AddresseeLookAtSpeaker = 1 << 0,
                    EveryoneLookAtSpeaker = 1 << 1,
                    EveryoneLookAtAddressee = 1 << 2,
                    WaitAfterUntilToldToAdvance = 1 << 3,
                    WaitUntilSpeakerNearby = 1 << 4,
                    WaitUntilEveryoneNearby = 1 << 5
                }
                
                public enum AddresseeValue : short
                {
                    None,
                    Player,
                    Participant
                }
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class HsScriptsBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public ScriptTypeValue ScriptType;
            public ReturnTypeValue ReturnType;
            public int RootExpressionIndex;
            [TagField(Length = 0x34)]
            public byte[] Padding;
            
            public enum ScriptTypeValue : short
            {
                Startup,
                Dormant,
                Continuous,
                Static,
                Stub
            }
            
            public enum ReturnTypeValue : short
            {
                Unparsed,
                SpecialForm,
                FunctionName,
                Passthrough,
                Void,
                Boolean,
                Real,
                Short,
                Long,
                String,
                Script,
                TriggerVolume,
                CutsceneFlag,
                CutsceneCameraPoint,
                CutsceneTitle,
                CutsceneRecording,
                DeviceGroup,
                Ai,
                AiCommandList,
                StartingProfile,
                Conversation,
                Navpoint,
                HudMessage,
                ObjectList,
                Sound,
                Effect,
                Damage,
                LoopingSound,
                AnimationGraph,
                ActorVariant,
                DamageEffect,
                ObjectDefinition,
                GameDifficulty,
                Team,
                AiDefaultState,
                ActorType,
                HudCorner,
                Object,
                Unit,
                Vehicle,
                Weapon,
                Device,
                Scenery,
                ObjectName,
                UnitName,
                VehicleName,
                WeaponName,
                DeviceName,
                SceneryName
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class HsGlobalsBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public TypeValue Type;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x4)]
            public byte[] Padding1;
            public int InitializationExpressionIndex;
            [TagField(Length = 0x30)]
            public byte[] Padding2;
            
            public enum TypeValue : short
            {
                Unparsed,
                SpecialForm,
                FunctionName,
                Passthrough,
                Void,
                Boolean,
                Real,
                Short,
                Long,
                String,
                Script,
                TriggerVolume,
                CutsceneFlag,
                CutsceneCameraPoint,
                CutsceneTitle,
                CutsceneRecording,
                DeviceGroup,
                Ai,
                AiCommandList,
                StartingProfile,
                Conversation,
                Navpoint,
                HudMessage,
                ObjectList,
                Sound,
                Effect,
                Damage,
                LoopingSound,
                AnimationGraph,
                ActorVariant,
                DamageEffect,
                ObjectDefinition,
                GameDifficulty,
                Team,
                AiDefaultState,
                ActorType,
                HudCorner,
                Object,
                Unit,
                Vehicle,
                Weapon,
                Device,
                Scenery,
                ObjectName,
                UnitName,
                VehicleName,
                WeaponName,
                DeviceName,
                SceneryName
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class HsReferencesBlock : TagStructure
        {
            [TagField(Length = 0x18)]
            public byte[] Padding;
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0x34)]
        public class HsSourceFilesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public byte[] Source;
        }
        
        [TagStructure(Size = 0x5C)]
        public class ScenarioCutsceneFlagBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string Name;
            public RealPoint3d Position;
            public RealEulerAngles2d Facing;
            [TagField(Length = 0x24)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x68)]
        public class ScenarioCutsceneCameraPointBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 0x4)]
            public byte[] Padding1;
            public RealPoint3d Position;
            public RealEulerAngles3d Orientation;
            public Angle FieldOfView;
            [TagField(Length = 0x24)]
            public byte[] Padding2;
        }
        
        [TagStructure(Size = 0x60)]
        public class ScenarioCutsceneTitleBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 0x4)]
            public byte[] Padding1;
            public Rectangle2d TextBoundsOnScreen;
            public short StringIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding2;
            public JustificationValue Justification;
            [TagField(Length = 0x2)]
            public byte[] Padding3;
            [TagField(Length = 0x4)]
            public byte[] Padding4;
            public ArgbColor TextColor;
            public ArgbColor ShadowColor;
            public float FadeInTimeSeconds;
            public float UpTimeSeconds;
            public float FadeOutTimeSeconds;
            [TagField(Length = 0x10)]
            public byte[] Padding5;
            
            public enum JustificationValue : short
            {
                Left,
                Right,
                Center
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ScenarioStructureBspsBlock : TagStructure
        {
            [TagField(Length = 0x10)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "sbsp" })]
            public CachedTag StructureBsp;
        }
    }
}

