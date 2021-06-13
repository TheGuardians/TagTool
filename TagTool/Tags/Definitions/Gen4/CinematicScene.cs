using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cinematic_scene", Tag = "cisc", Size = 0x68)]
    public class CinematicScene : TagStructure
    {
        public StringId Name;
        public StringId Anchor;
        public SceneResetObjectLightingEnum ResetObjectLighting;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(ValidTags = new [] { "cisd" })]
        public CachedTag Data;
        public CinematicCustomScriptBlock Header;
        public List<CinematicSceneObjectBlockStruct> Objects;
        public List<CinematicShotBlockStruct> Shots;
        public List<CinematicstructureLightingBlock> Lights;
        public CinematicCustomScriptBlock Footer;
        
        public enum SceneResetObjectLightingEnum : short
        {
            Default,
            DonTResetLighting,
            ResetLighting
        }
        
        [TagStructure(Size = 0x14)]
        public class CinematicCustomScriptBlock : TagStructure
        {
            public byte[] Script;
        }
        
        [TagStructure(Size = 0x40)]
        public class CinematicSceneObjectBlockStruct : TagStructure
        {
            public StringId Name;
            public StringId VariantName;
            public SceneObjectFlags Flags;
            [TagField(Length = 2)]
            public GCinematicshotFlagArray[]  LightmapShadowFlags;
            [TagField(Length = 2)]
            public GCinematicshotFlagArray[]  HighResFlags;
            public CinematicCoopTypeFlags OverrideCreationFlags;
            public CinematicCustomScriptBlock CustomDonTCreateCondition;
            public List<SceneObjectAttachmentBlock> Attachments;
            
            [Flags]
            public enum SceneObjectFlags : uint
            {
                PlacedManuallyInSapien = 1 << 0,
                ObjectComesFromGame = 1 << 1,
                SpecialCase = 1 << 2,
                EffectObject = 1 << 3,
                NoLightmapShadow = 1 << 4,
                ApplyPlayerCustomization = 1 << 5,
                ApplyFirstPersonPlayerCustomization = 1 << 6,
                IWillAnimateTheEnglishLipsyncManually = 1 << 7,
                PrimaryCortana = 1 << 8,
                PreloadTextures = 1 << 9
            }
            
            [Flags]
            public enum CinematicCoopTypeFlags : uint
            {
                SinglePlayer = 1 << 0,
                _2PlayerCoOp = 1 << 1,
                _3PlayerCoOp = 1 << 2,
                _4PlayerCoOp = 1 << 3
            }
            
            [TagStructure(Size = 0x4)]
            public class GCinematicshotFlagArray : TagStructure
            {
                public uint ShotFlagData;
            }
            
            [TagStructure(Size = 0x20)]
            public class SceneObjectAttachmentBlock : TagStructure
            {
                public SceneObjectAttachmentFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId ObjectMarkerName;
                public StringId AttachmentObjectName;
                public StringId AttachmentMarkerName;
                [TagField(ValidTags = new [] { "obje","scen","efsc" })]
                public CachedTag AttachmentType;
                
                [Flags]
                public enum SceneObjectAttachmentFlags : byte
                {
                    Invisible = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0xC8)]
        public class CinematicShotBlockStruct : TagStructure
        {
            public CinematicCustomScriptBlock Header;
            public ShotFlags Flags;
            // this works best with auto-exposure off
            public float EnvironmentDarken; // 0 - 1
            // will disable auto-exposure
            public float ForcedExposure; // stops
            public SceneshotSettingsFlags SettingsFlags;
            public float LightmapDirectScalar;
            public float LightmapIndirectScalar;
            public float SunScalar;
            [TagField(ValidTags = new [] { "fogg" })]
            public CachedTag AtmosphereFog;
            [TagField(ValidTags = new [] { "cfxs" })]
            public CachedTag CameraEffects;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Cubemap;
            public List<CinematicShotLightingBlock> Lighting;
            public List<CinematicShotClipBlock> Clip;
            public List<CinematicShotMusicBlock> Music;
            public List<CinematicShotObjectFunctionBlock> ObjectFunctions;
            public List<CinematicShotScreenEffectBlock> ScreenEffects;
            public List<CinematicShotUserInputConstraintsBlock> UserInputConstraints;
            public List<CinematicshotTextureMovieBlock> TextureMovies;
            public CinematicCustomScriptBlock Footer;
            
            [Flags]
            public enum ShotFlags : uint
            {
                InstantAutoExposure = 1 << 0,
                ForceExposure = 1 << 1,
                GenerateLoopingScript = 1 << 2
            }
            
            [Flags]
            public enum SceneshotSettingsFlags : uint
            {
                LightmapScalarsSet = 1 << 0,
                LightmapScalarsClear = 1 << 1,
                LightmapScalarsPersistAcrossShots = 1 << 2,
                AtmosphereFogClear = 1 << 3,
                AtmosphereFogPersistAcrossShots = 1 << 4,
                CameraEffectsClear = 1 << 5,
                CameraEffectsPersistAcrossShots = 1 << 6,
                SunScalarSet = 1 << 7,
                SunScalarClear = 1 << 8,
                SunScalarPersistAcrossShots = 1 << 9,
                CubemapClear = 1 << 10,
                CubemapPersistAcrossShots = 1 << 11,
                DisableAllLightmapShadows = 1 << 12
            }
            
            [TagStructure(Size = 0x1C)]
            public class CinematicShotLightingBlock : TagStructure
            {
                public CinematicShotLightingFlags Flags;
                [TagField(ValidTags = new [] { "nclt" })]
                public CachedTag Lighting;
                public int Subject;
                public StringId Marker;
                
                [Flags]
                public enum CinematicShotLightingFlags : uint
                {
                    PersistsAcrossShots = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x2C)]
            public class CinematicShotClipBlock : TagStructure
            {
                public RealPoint3d PlaneCenter;
                public RealPoint3d PlaneDirection;
                public int FrameStart;
                public int FrameEnd;
                public List<CinematicShotClipSubjectBlock> SubjectObjects;
                
                [TagStructure(Size = 0x4)]
                public class CinematicShotClipSubjectBlock : TagStructure
                {
                    public int Index;
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class CinematicShotMusicBlock : TagStructure
            {
                public MusicFlagsEnum Flags;
                [TagField(ValidTags = new [] { "scmb","sndo","lsnd","snd!" })]
                public CachedTag MusicFoley;
                public int Frame;
                
                [Flags]
                public enum MusicFlagsEnum : uint
                {
                    StopMusicAtFrame = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class CinematicShotObjectFunctionBlock : TagStructure
            {
                public int Object;
                public StringId FunctionName;
                public List<CinematicShotObjectFunctionKeyframeBlock> Keyframes;
                
                [TagStructure(Size = 0x10)]
                public class CinematicShotObjectFunctionKeyframeBlock : TagStructure
                {
                    public CinematicShotObjectFunctionFlags Flags;
                    public int Frame;
                    public float Value;
                    public float InterpolationTime; // ticks
                    
                    [Flags]
                    public enum CinematicShotObjectFunctionFlags : uint
                    {
                        ClearFunction = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class CinematicShotScreenEffectBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "sefc" })]
                public CachedTag ScreenEffect;
                public int Frame;
                public int StopFrame;
                public CinematicshotScreenEffectFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum CinematicshotScreenEffectFlags : byte
                {
                    PersistEntireShot = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class CinematicShotUserInputConstraintsBlock : TagStructure
            {
                public int Frame;
                public int Ticks;
                public Rectangle2d MaximumLookAngles;
                public float FrictionalForce;
            }
            
            [TagStructure(Size = 0x18)]
            public class CinematicshotTextureMovieBlock : TagStructure
            {
                public TexturemovieFlags Flags;
                public int Frame;
                [TagField(ValidTags = new [] { "bink" })]
                public CachedTag BinkMovie;
                
                [Flags]
                public enum TexturemovieFlags : uint
                {
                    StopMovieAtFrame = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CinematicstructureLightingBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "stli" })]
            public CachedTag StructureLightingInfo;
        }
    }
}
