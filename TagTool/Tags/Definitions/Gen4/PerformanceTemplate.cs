using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "performance_template", Tag = "pfmc", Size = 0x2C)]
    public class PerformanceTemplate : TagStructure
    {
        public StringId Name;
        // The name of a custom script used to drive the performance. If none is given, a default script is uses that goes
        // through the lines in sequence
        public StringId ScriptName;
        public List<PerformanceTemplateActorBlockStruct> Actors;
        public List<ScenarioPerformanceLineBlockStruct> Lines;
        public List<PerformanceTemplatePointBlockStruct> Points;
        
        [TagStructure(Size = 0x40)]
        public class PerformanceTemplateActorBlockStruct : TagStructure
        {
            public ScenarioPerformanceActorFlags Flags;
            public StringId ActorName;
            [TagField(ValidTags = new [] { "char" })]
            public CachedTag ActorType;
            [TagField(ValidTags = new [] { "vehi" })]
            public CachedTag VehicleType;
            public StringId VehicleSeatLabel;
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag WeaponType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short SpawnPoint;
            
            [Flags]
            public enum ScenarioPerformanceActorFlags : uint
            {
                ActorIsCritical = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x64)]
        public class ScenarioPerformanceLineBlockStruct : TagStructure
        {
            public StringId Name;
            public short Actor;
            public ScenarioPerformanceLineFlags Flags;
            public short SleepMinimum; // ticks
            public short SleepMaximum; // ticks
            public ScenarioPerformanceLineProgressDefinition LineProgressType;
            public List<ScenarioPerformanceLineScriptFragmentBlock> ScriptFragments;
            public List<ScenarioPerformanceLinePointInteractionBlockStruct> PointInteraction;
            public List<ScenarioPerformanceLineAnimationBlockStruct> Animations;
            public List<ScenarioPerformanceLineSyncActionBlockStruct> SyncActions;
            public List<ScenarioPerformanceLineScenerySyncActionBlockStruct> ScenerySyncActions;
            public List<ScenarioPerformanceLineDialogBlockStruct> DialogLines;
            public List<ScenarioPerformanceLineSoundBlockStruct> Sounds;
            
            [Flags]
            public enum ScenarioPerformanceLineFlags : ushort
            {
                Disable = 1 << 0
            }
            
            public enum ScenarioPerformanceLineProgressDefinition : int
            {
                Immediate,
                BlockUntilAllDone,
                BlockUntilLineDone,
                QueueBlocking,
                QueueImmediate
            }
            
            [TagStructure(Size = 0x204)]
            public class ScenarioPerformanceLineScriptFragmentBlock : TagStructure
            {
                public ScenarioPerformanceFragmentPlacementDefinition FragmentPlacement;
                public ScenarioPerformanceFragmentType FragmentType;
                // maximum 256 characters, type just branch condition (with brackets) in case of branches
                [TagField(Length = 256)]
                public string Fragment;
                // the script to branch to (with any arguments to it). Used only if type is branch
                [TagField(Length = 256)]
                public string BranchTarget;
                
                public enum ScenarioPerformanceFragmentPlacementDefinition : short
                {
                    PreLine,
                    PostLine
                }
                
                public enum ScenarioPerformanceFragmentType : short
                {
                    Default,
                    ConditionalSleep,
                    Branch
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ScenarioPerformanceLinePointInteractionBlockStruct : TagStructure
            {
                public ScenarioPerformanceLinePointInteractionType InteractionType;
                public short Point;
                public short Actor;
                public StringId ObjectName;
                public StringId ThrottleStyle;
                
                [Flags]
                public enum ScenarioPerformanceLinePointInteractionType : uint
                {
                    FacePoint = 1 << 0,
                    AimAtPoint = 1 << 1,
                    LookAtPoint = 1 << 2,
                    ShootAtPoint = 1 << 3,
                    GoByPoint = 1 << 4,
                    GoToPoint = 1 << 5,
                    GoToAndAlign = 1 << 6,
                    GoToThespianCenter = 1 << 7,
                    TeleportToPoint = 1 << 8
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class ScenarioPerformanceLineAnimationBlockStruct : TagStructure
            {
                public ScenarioPerformanceLineAnimationFlags Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId Stance;
                public StringId Animation;
                public float Duration;
                public float Probability;
                public float ThrottleTransitionTime; // seconds
                // The number of frames from the end of the animation to start transitioning out
                public int TransitionFrameCount;
                
                [Flags]
                public enum ScenarioPerformanceLineAnimationFlags : ushort
                {
                    Loop = 1 << 0,
                    LoopUntilTaskTransition = 1 << 1,
                    DieOnAnimationCompletion = 1 << 2
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioPerformanceLineSyncActionBlockStruct : TagStructure
            {
                public StringId SyncActionName;
                public float Probability;
                public short AttachToPoint;
                public ScenarioPerformanceLineSyncActionFlagType Flags;
                public List<ScenarioPerformanceLineSyncActionActorBlock> Actors;
                
                [Flags]
                public enum ScenarioPerformanceLineSyncActionFlagType : ushort
                {
                    ShareInitiatorStance = 1 << 0,
                    InitiatorIsOrigin = 1 << 1
                }
                
                [TagStructure(Size = 0x4)]
                public class ScenarioPerformanceLineSyncActionActorBlock : TagStructure
                {
                    public short ActorType;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class ScenarioPerformanceLineScenerySyncActionBlockStruct : TagStructure
            {
                public StringId SceneryObjectName;
                public StringId SyncActionName;
                public StringId StanceName;
                public float Probability;
                public ScenarioPerformanceLineScenerySyncActionFlagType Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<ScenarioPerformanceLineSyncActionActorBlock> Actors;
                
                [Flags]
                public enum ScenarioPerformanceLineScenerySyncActionFlagType : ushort
                {
                    ShareInitiatorStance = 1 << 0
                }
                
                [TagStructure(Size = 0x4)]
                public class ScenarioPerformanceLineSyncActionActorBlock : TagStructure
                {
                    public short ActorType;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioPerformanceLineDialogBlockStruct : TagStructure
            {
                public StringId Dialog;
                public float Probability;
            }
            
            [TagStructure(Size = 0x18)]
            public class ScenarioPerformanceLineSoundBlockStruct : TagStructure
            {
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundEffect;
                public short AttachToPoint;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId AttachToObjectNamed;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class PerformanceTemplatePointBlockStruct : TagStructure
        {
            public StringId PointName;
            // The offset of the performance point from the center of the performance
            public RealVector3d RelativePosition;
            // The facing at the point in the space of the performance
            public RealEulerAngles2d RelativeFacing;
        }
    }
}
