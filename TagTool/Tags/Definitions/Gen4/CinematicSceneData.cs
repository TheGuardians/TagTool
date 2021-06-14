using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cinematic_scene_data", Tag = "cisd", Size = 0x28)]
    public class CinematicSceneData : TagStructure
    {
        public List<CinematicsceneDataObjectBlockStruct> Objects;
        public List<CinematicdataShotBlock> Shots;
        public List<CinematicShotExtraCameraBlock> ExtraCameraFrameData;
        public int Version;
        
        [TagStructure(Size = 0x30)]
        public class CinematicsceneDataObjectBlockStruct : TagStructure
        {
            public StringId Name;
            public StringId Identifier;
            [TagField(ValidTags = new [] { "jmad" })]
            public CachedTag ModelAnimationGraph;
            [TagField(ValidTags = new [] { "obje","scen","efsc" })]
            public CachedTag ObjectType;
            [TagField(Length = 2)]
            public GCinematicshotFlagArray[]  ShotsActiveFlags;
            
            [TagStructure(Size = 0x4)]
            public class GCinematicshotFlagArray : TagStructure
            {
                public uint ShotFlagData;
            }
        }
        
        [TagStructure(Size = 0x4C)]
        public class CinematicdataShotBlock : TagStructure
        {
            public List<CinematicShotDialogueBlock> Dialogue;
            public List<CinematicShotEffectBlock> Effects;
            public List<CinematicShotCustomScriptBlock> CustomScript;
            public int FrameCount;
            public List<CinematicShotFrameBlock> FrameData;
            public List<CinematicShotFrameDynamicBlock> DynamicFrameData;
            public List<CinematicShotFrameConstantBlock> ConstantFrameData;
            
            [TagStructure(Size = 0x3C)]
            public class CinematicShotDialogueBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag Dialogue;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag FemaleDialogue;
                public int Frame;
                public float Scale;
                public StringId LipsyncActor;
                public StringId DefaultSoundEffect;
                public StringId Subtitle;
                public StringId FemaleSubtitle;
                public StringId Character;
            }
            
            [TagStructure(Size = 0x34)]
            public class CinematicShotEffectBlock : TagStructure
            {
                public CinematicShotEffectFlags Flags;
                public SceneshotEffectState State;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag Effect;
                public float SizeScale;
                public int Frame;
                public StringId MarkerName;
                public int MarkerParent;
                public StringId FunctionA;
                public StringId FunctionB;
                public int NodeId;
                public int SequenceId;
                
                [Flags]
                public enum CinematicShotEffectFlags : byte
                {
                    UseMayaValue = 1 << 0,
                    Looping = 1 << 1
                }
                
                public enum SceneshotEffectState : sbyte
                {
                    Start,
                    Stop,
                    Kill
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class CinematicShotCustomScriptBlock : TagStructure
            {
                public CinematicShotCustomScriptFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int Frame;
                public CinematicCustomScriptBlock Script;
                public int NodeId;
                public int SequenceId;
                
                [Flags]
                public enum CinematicShotCustomScriptFlags : byte
                {
                    UseMayaValue = 1 << 0
                }
                
                [TagStructure(Size = 0x14)]
                public class CinematicCustomScriptBlock : TagStructure
                {
                    public byte[] Script;
                }
            }
            
            [TagStructure(Size = 0x44)]
            public class CinematicShotFrameBlock : TagStructure
            {
                public CameraFrameStruct CameraFrame;
                
                [TagStructure(Size = 0x44)]
                public class CameraFrameStruct : TagStructure
                {
                    public CameraFrameDynamicStruct DynamicData;
                    public CameraFrameConstantStruct ConstantData;
                    
                    [TagStructure(Size = 0x24)]
                    public class CameraFrameDynamicStruct : TagStructure
                    {
                        public RealPoint3d CameraPosition;
                        public RealVector3d CameraForward;
                        public RealVector3d CameraUp;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class CameraFrameConstantStruct : TagStructure
                    {
                        public float FocalLength;
                        public int DepthOfField;
                        public float NearFocalPlaneDistance;
                        public float FarFocalPlaneDistance;
                        public float NearFocalDepth;
                        public float FarFocalDepth;
                        public float NearBlurAmount;
                        public float FarBlurAmount;
                    }
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class CinematicShotFrameDynamicBlock : TagStructure
            {
                public CameraFrameDynamicStruct DynamicCameraFrame;
                
                [TagStructure(Size = 0x24)]
                public class CameraFrameDynamicStruct : TagStructure
                {
                    public RealPoint3d CameraPosition;
                    public RealVector3d CameraForward;
                    public RealVector3d CameraUp;
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class CinematicShotFrameConstantBlock : TagStructure
            {
                public int FrameIndex;
                public CameraFrameConstantStruct ConstantCameraFrame;
                
                [TagStructure(Size = 0x20)]
                public class CameraFrameConstantStruct : TagStructure
                {
                    public float FocalLength;
                    public int DepthOfField;
                    public float NearFocalPlaneDistance;
                    public float FarFocalPlaneDistance;
                    public float NearFocalDepth;
                    public float FarFocalDepth;
                    public float NearBlurAmount;
                    public float FarBlurAmount;
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CinematicShotExtraCameraBlock : TagStructure
        {
            public StringId Name;
            public StringId Type;
            public List<CinematicExtraCameraShotBlock> Shots;
            
            [TagStructure(Size = 0xC)]
            public class CinematicExtraCameraShotBlock : TagStructure
            {
                public List<CinematicExtraCameraFrameBlock> FrameData;
                
                [TagStructure(Size = 0x48)]
                public class CinematicExtraCameraFrameBlock : TagStructure
                {
                    public CinematicExtraCameraFrameFlags Flags;
                    public CameraFrameStruct FrameData;
                    
                    [Flags]
                    public enum CinematicExtraCameraFrameFlags : uint
                    {
                        Enabled = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x44)]
                    public class CameraFrameStruct : TagStructure
                    {
                        public CameraFrameDynamicStruct DynamicData;
                        public CameraFrameConstantStruct ConstantData;
                        
                        [TagStructure(Size = 0x24)]
                        public class CameraFrameDynamicStruct : TagStructure
                        {
                            public RealPoint3d CameraPosition;
                            public RealVector3d CameraForward;
                            public RealVector3d CameraUp;
                        }
                        
                        [TagStructure(Size = 0x20)]
                        public class CameraFrameConstantStruct : TagStructure
                        {
                            public float FocalLength;
                            public int DepthOfField;
                            public float NearFocalPlaneDistance;
                            public float FarFocalPlaneDistance;
                            public float NearFocalDepth;
                            public float FarFocalDepth;
                            public float NearBlurAmount;
                            public float FarBlurAmount;
                        }
                    }
                }
            }
        }
    }
}
