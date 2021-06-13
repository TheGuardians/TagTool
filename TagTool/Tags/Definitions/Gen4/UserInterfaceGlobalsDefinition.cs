using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x134)]
    public class UserInterfaceGlobalsDefinition : TagStructure
    {
        public UserInterfaceTagGlobalsFlags Flags;
        [TagField(ValidTags = new [] { "wigl" })]
        public CachedTag SharedGlobals;
        [TagField(ValidTags = new [] { "goof" })]
        public CachedTag MpVariantSettingsUi;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag GameHopperDescriptions;
        public List<CuiComponentScreenReferenceBlock> CuiScreenWidgets;
        public List<CuiOverlayCameraBlock> CuiOverlayCameras;
        public List<CuiPlayerModelCameraSettingsDefinition> PlayerModelCameraSettings;
        public List<CuiPlayerModelControllerSettingsDefinition> PlayerModelInputSettings;
        public List<CuiPlayerModelTransitionSettingsDefinition> PlayerModelCameraTransitionSettings;
        [TagField(ValidTags = new [] { "cpgd" })]
        public CachedTag PurchaseGlobals;
        [TagField(ValidTags = new [] { "iuii" })]
        public CachedTag InfinityMissionImages;
        public List<CuiActiveRosterSettingsBlock> ActiveRosterSettings;
        [TagField(ValidTags = new [] { "pcec" })]
        public CachedTag PgcrCategoriesDefinitions;
        [TagField(ValidTags = new [] { "pdti" })]
        public CachedTag PgcrDamageTypesDefinitions;
        public List<CampaignStateScreenScriptBlock> CampaignStateScreenScripts;
        public float SpawnTimerCountdownRate; // counts/sec
        public List<UserInterfaceGameScreenSequenceStepDefinition> GameIntroSequence;
        public List<UserInterfaceGameScreenSequenceStepDefinition> GameRoundEndSequence;
        public List<UserInterfaceGameScreenSequenceStepDefinition> GameNextRoundSequence;
        public List<UserInterfaceGameScreenSequenceStepDefinition> GameEndSequence;
        public List<UserInterfaceGameScreenSequenceStepDefinition> GameEndWithKillcamSequence;
        [TagField(ValidTags = new [] { "uihg" })]
        // global settings for the HUD. Set this for ingame globals.
        public CachedTag HudGlobals;
        [TagField(ValidTags = new [] { "ppod" })]
        public CachedTag PortraitPoses;
        public List<SwapTagReferenceDefinition> SwapTags;
        
        [Flags]
        public enum UserInterfaceTagGlobalsFlags : uint
        {
            // show navpoints over ammo-crates and weapon-racks when below a clip-full of ammo
            ShowAmmoNavpoints = 1 << 0
        }
        
        [TagStructure(Size = 0x14)]
        public class CuiComponentScreenReferenceBlock : TagStructure
        {
            // for use in code
            public StringId Name;
            [TagField(ValidTags = new [] { "cusc" })]
            public CachedTag CuiScreenTag;
        }
        
        [TagStructure(Size = 0x18)]
        public class CuiOverlayCameraBlock : TagStructure
        {
            public StringId ResolutionName;
            public CuiCameraPivotCornerEnum PivotCorner;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // distance to near clipping plane
            public float ZNear;
            // distance to far clipping plane
            public float ZFar;
            // camera angle around the X axis
            public float XAngleDegrees;
            // camera angle around the Y axis
            public float YAngleDegrees;
            
            public enum CuiCameraPivotCornerEnum : sbyte
            {
                TopLeft,
                TopRight,
                BottomRight,
                BottomLeft
            }
        }
        
        [TagStructure(Size = 0x64)]
        public class CuiPlayerModelCameraSettingsDefinition : TagStructure
        {
            public StringId Name;
            // arbitrary location in the world to place the model
            public RealPoint3d ModelWorldPosition; // wu
            public RealPoint3d MinimumWorldPosition;
            public RealPoint3d MaximumWorldPosition;
            public StringId ViewedModelMarkerName;
            public RealPoint3d MinimumCameraOffset; // wu
            public RealPoint3d MinimumCameraFocalOffset; // wu
            public RealPoint3d MaximumCameraOffset; // wu
            public RealPoint3d MaximumCameraFocalOffset; // wu
            public float InitialZoom; // [0,1]
            public float Fov; // degrees
        }
        
        [TagStructure(Size = 0x38)]
        public class CuiPlayerModelControllerSettingsDefinition : TagStructure
        {
            public StringId Name;
            public float ZoomSpeed; // wu per tick
            public MappingFunction ZoomTransitionFunction;
            public RealEulerAngles2d InitialRotation; // degrees
            public RealEulerAngles2d MinimumRotation; // degrees
            public RealEulerAngles2d MaximumRotation; // degrees
            public float RotationSpeed; // degrees per tick
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CuiPlayerModelTransitionSettingsDefinition : TagStructure
        {
            public MappingFunction CameraTransitionFunction;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CuiActiveRosterSettingsBlock : TagStructure
        {
            public MappingFunction AnalogScrollFunction;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class CampaignStateScreenScriptBlock : TagStructure
        {
            public int MapId;
            public StringId ScreenScriptName;
            [TagField(ValidTags = new [] { "lsnd" })]
            public CachedTag MusicOverride;
        }
        
        [TagStructure(Size = 0x18)]
        public class UserInterfaceGameScreenSequenceStepDefinition : TagStructure
        {
            public UigameStartSequenceFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "cusc" })]
            public CachedTag Screen;
            public short Starttime; // ticks
            public short Endtime; // ticks
            
            [Flags]
            public enum UigameStartSequenceFlags : byte
            {
                ShowLoadoutMenu = 1 << 0,
                LoadoutMenuCloseEndsSequence = 1 << 1
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class SwapTagReferenceDefinition : TagStructure
        {
            public CachedTag OriginalTag;
            public CachedTag ReplacementTag;
        }
    }
}
