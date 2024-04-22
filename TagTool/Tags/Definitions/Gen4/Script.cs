using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "script", Tag = "hsdt", Size = 0x74)]
    public class Script : TagStructure
    {
        public List<HsSourceFiles> SourceFiles;
        public List<HsScriptsBlock> Scripts;
        public List<HsGlobalsBlock> Globals;
        public List<HsinstancedVariablesBlock> InstancedVariables;
        public List<HsReferencesBlock> References;
        public List<HsUnitSeatBlock> HsUnitSeats;
        public List<HsSyntaxDatumBlock> HsSyntaxDatums;
        public byte[] ScriptStringData;
        public List<HsimportManifestBlock> ImportManifest;
        
        [TagStructure(Size = 0x20)]
        public class HsScriptsBlock : TagStructure
        {
            public StringId Name;
            public HsScriptTypesEnum ScriptType;
            public ScriptflagsEnum ScriptFlags;
            public HsTypesEnum ReturnType;
            public int RootExpressionIndex;
            public int LocalsStackSpace;
            public List<HsScriptParametersBlock> Parameters;
            
            public enum HsScriptTypesEnum : short
            {
                Startup,
                Dormant,
                Continuous,
                Static,
                CommandScript,
                Stub
            }
            
            [Flags]
            public enum ScriptflagsEnum : ushort
            {
                Instanced = 1 << 0,
                Cinema = 1 << 1
            }
            
            public enum HsTypesEnum : int
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
                StringId,
                UnitSeatMapping,
                TriggerVolume,
                SoundEvent,
                CutsceneFlag,
                CutsceneCameraPoint,
                CutsceneTitle,
                CutsceneRecording,
                DeviceGroup,
                Ai,
                AiCommandList,
                AiCommandScript,
                AiBehavior,
                AiOrders,
                AiLine,
                StartingProfile,
                Conversation,
                Player,
                ZoneSet,
                DesignerZone,
                PointReference,
                PointSetReference,
                Style,
                ObjectList,
                Folder,
                Sound,
                Effect,
                Damage,
                LoopingSound,
                AnimationGraph,
                DamageEffect,
                ObjectDefinition,
                Bitmap,
                Shader,
                RenderModel,
                StructureDefinition,
                LightmapDefinition,
                CinematicDefinition,
                CinematicSceneDefinition,
                CinematicSceneDataDefinition,
                CinematicTransitionDefinition,
                BinkDefinition,
                CuiScreenDefinition,
                AnyTag,
                AnyTagNotResolving,
                GameDifficulty,
                Team,
                MpTeam,
                Controller,
                ButtonPreset,
                JoystickPreset,
                PlayerColor,
                PlayerModelChoice,
                VoiceOutputSetting,
                VoiceMask,
                SubtitleSetting,
                ActorType,
                ModelState,
                Event,
                CharacterPhysics,
                Skull,
                FiringPointEvaluator,
                DamageRegion,
                CurrencyType,
                DeliveryMethod,
                WaveDifficulty,
                FirefightGoal,
                FirefightWaveType,
                Font,
                TextJustification,
                TextAlignment,
                TextDropShadowType,
                HavokGroup,
                Object,
                Unit,
                Vehicle,
                Weapon,
                Device,
                Scenery,
                EffectScenery,
                ObjectName,
                UnitName,
                VehicleName,
                WeaponName,
                DeviceName,
                SceneryName,
                EffectSceneryName,
                CinematicLightprobe,
                AnimationBudgetReference,
                LoopingSoundBudgetReference,
                SoundBudgetReference
            }
            
            [TagStructure(Size = 0x24)]
            public class HsScriptParametersBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public HsTypesEnum ReturnType;
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class HsGlobalsBlock : TagStructure
        {
            public StringId Name;
            public HsTypesEnum Type;
            public int InitializationExpressionIndex;
            
            public enum HsTypesEnum : int
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
                StringId,
                UnitSeatMapping,
                TriggerVolume,
                SoundEvent,
                CutsceneFlag,
                CutsceneCameraPoint,
                CutsceneTitle,
                CutsceneRecording,
                DeviceGroup,
                Ai,
                AiCommandList,
                AiCommandScript,
                AiBehavior,
                AiOrders,
                AiLine,
                StartingProfile,
                Conversation,
                Player,
                ZoneSet,
                DesignerZone,
                PointReference,
                PointSetReference,
                Style,
                ObjectList,
                Folder,
                Sound,
                Effect,
                Damage,
                LoopingSound,
                AnimationGraph,
                DamageEffect,
                ObjectDefinition,
                Bitmap,
                Shader,
                RenderModel,
                StructureDefinition,
                LightmapDefinition,
                CinematicDefinition,
                CinematicSceneDefinition,
                CinematicSceneDataDefinition,
                CinematicTransitionDefinition,
                BinkDefinition,
                CuiScreenDefinition,
                AnyTag,
                AnyTagNotResolving,
                GameDifficulty,
                Team,
                MpTeam,
                Controller,
                ButtonPreset,
                JoystickPreset,
                PlayerColor,
                PlayerModelChoice,
                VoiceOutputSetting,
                VoiceMask,
                SubtitleSetting,
                ActorType,
                ModelState,
                Event,
                CharacterPhysics,
                Skull,
                FiringPointEvaluator,
                DamageRegion,
                CurrencyType,
                DeliveryMethod,
                WaveDifficulty,
                FirefightGoal,
                FirefightWaveType,
                Font,
                TextJustification,
                TextAlignment,
                TextDropShadowType,
                HavokGroup,
                Object,
                Unit,
                Vehicle,
                Weapon,
                Device,
                Scenery,
                EffectScenery,
                ObjectName,
                UnitName,
                VehicleName,
                WeaponName,
                DeviceName,
                SceneryName,
                EffectSceneryName,
                CinematicLightprobe,
                AnimationBudgetReference,
                LoopingSoundBudgetReference,
                SoundBudgetReference
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class HsinstancedVariablesBlock : TagStructure
        {
            public StringId Name;
            public HsTypesEnum Type;
            public int InitializationExpressionIndex;
            
            public enum HsTypesEnum : int
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
                StringId,
                UnitSeatMapping,
                TriggerVolume,
                SoundEvent,
                CutsceneFlag,
                CutsceneCameraPoint,
                CutsceneTitle,
                CutsceneRecording,
                DeviceGroup,
                Ai,
                AiCommandList,
                AiCommandScript,
                AiBehavior,
                AiOrders,
                AiLine,
                StartingProfile,
                Conversation,
                Player,
                ZoneSet,
                DesignerZone,
                PointReference,
                PointSetReference,
                Style,
                ObjectList,
                Folder,
                Sound,
                Effect,
                Damage,
                LoopingSound,
                AnimationGraph,
                DamageEffect,
                ObjectDefinition,
                Bitmap,
                Shader,
                RenderModel,
                StructureDefinition,
                LightmapDefinition,
                CinematicDefinition,
                CinematicSceneDefinition,
                CinematicSceneDataDefinition,
                CinematicTransitionDefinition,
                BinkDefinition,
                CuiScreenDefinition,
                AnyTag,
                AnyTagNotResolving,
                GameDifficulty,
                Team,
                MpTeam,
                Controller,
                ButtonPreset,
                JoystickPreset,
                PlayerColor,
                PlayerModelChoice,
                VoiceOutputSetting,
                VoiceMask,
                SubtitleSetting,
                ActorType,
                ModelState,
                Event,
                CharacterPhysics,
                Skull,
                FiringPointEvaluator,
                DamageRegion,
                CurrencyType,
                DeliveryMethod,
                WaveDifficulty,
                FirefightGoal,
                FirefightWaveType,
                Font,
                TextJustification,
                TextAlignment,
                TextDropShadowType,
                HavokGroup,
                Object,
                Unit,
                Vehicle,
                Weapon,
                Device,
                Scenery,
                EffectScenery,
                ObjectName,
                UnitName,
                VehicleName,
                WeaponName,
                DeviceName,
                SceneryName,
                EffectSceneryName,
                CinematicLightprobe,
                AnimationBudgetReference,
                LoopingSoundBudgetReference,
                SoundBudgetReference
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class HsReferencesBlock : TagStructure
        {
            public CachedTag Reference;
        }
        
        [TagStructure(Size = 0xC)]
        public class HsUnitSeatBlock : TagStructure
        {
            public int UnitDefinitionTagIndex;
            public int UnitSeats;
            public int UnitSeats2;
        }
        
        [TagStructure(Size = 0x1C)]
        public class HsSyntaxDatumBlock : TagStructure
        {
            public short DatumHeader;
            public short ScriptIndexFunctionIndexConstantTypeUnion;
            public int NextNode;
            public int SourceData;
            public int SourceOffsetLocation;
            public HsTypesEnum NodeExpressionType;
            public short Flags;
            public short SourceFileIndex;
            public int SourceFileOffset;
            
            public enum HsTypesEnum : int
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
                StringId,
                UnitSeatMapping,
                TriggerVolume,
                SoundEvent,
                CutsceneFlag,
                CutsceneCameraPoint,
                CutsceneTitle,
                CutsceneRecording,
                DeviceGroup,
                Ai,
                AiCommandList,
                AiCommandScript,
                AiBehavior,
                AiOrders,
                AiLine,
                StartingProfile,
                Conversation,
                Player,
                ZoneSet,
                DesignerZone,
                PointReference,
                PointSetReference,
                Style,
                ObjectList,
                Folder,
                Sound,
                Effect,
                Damage,
                LoopingSound,
                AnimationGraph,
                DamageEffect,
                ObjectDefinition,
                Bitmap,
                Shader,
                RenderModel,
                StructureDefinition,
                LightmapDefinition,
                CinematicDefinition,
                CinematicSceneDefinition,
                CinematicSceneDataDefinition,
                CinematicTransitionDefinition,
                BinkDefinition,
                CuiScreenDefinition,
                AnyTag,
                AnyTagNotResolving,
                GameDifficulty,
                Team,
                MpTeam,
                Controller,
                ButtonPreset,
                JoystickPreset,
                PlayerColor,
                PlayerModelChoice,
                VoiceOutputSetting,
                VoiceMask,
                SubtitleSetting,
                ActorType,
                ModelState,
                Event,
                CharacterPhysics,
                Skull,
                FiringPointEvaluator,
                DamageRegion,
                CurrencyType,
                DeliveryMethod,
                WaveDifficulty,
                FirefightGoal,
                FirefightWaveType,
                Font,
                TextJustification,
                TextAlignment,
                TextDropShadowType,
                HavokGroup,
                Object,
                Unit,
                Vehicle,
                Weapon,
                Device,
                Scenery,
                EffectScenery,
                ObjectName,
                UnitName,
                VehicleName,
                WeaponName,
                DeviceName,
                SceneryName,
                EffectSceneryName,
                CinematicLightprobe,
                AnimationBudgetReference,
                LoopingSoundBudgetReference,
                SoundBudgetReference
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class HsimportManifestBlock : TagStructure
        {
            public Tag CalleeTag;
            public List<HsimportManifestEntryBlock> ScriptTable;
            public List<HsimportManifestEntryBlock> VariableTable;
            
            [TagStructure(Size = 0xC)]
            public class HsimportManifestEntryBlock : TagStructure
            {
                public StringId ScriptName;
                public int Argcount;
                public int Index;
            }
        }
    }
}
