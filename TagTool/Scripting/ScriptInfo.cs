using TagTool.Cache;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TagTool.Scripting
{
    public struct ScriptInfo : IList<ScriptInfo.ArgumentInfo>
    {
        public ScriptValueType.Halo3ODSTValue Type;
        public string Name;
        public List<ArgumentInfo> Arguments;

        public int Count => Arguments.Count;
        public bool IsReadOnly => false;

        public ScriptInfo(ScriptValueType.Halo3ODSTValue type) : this(type, "") { }

        public ScriptInfo(ScriptValueType.Halo3ODSTValue type, string name) : this(type, name, new List<ArgumentInfo>()) { }

        public ScriptInfo(ScriptValueType.Halo3ODSTValue type, string name, IEnumerable<ArgumentInfo> arguments)
        {
            Type = type;
            Name = name;
            Arguments = arguments.ToList();
        }

        public ArgumentInfo this[int index] { get => Arguments[index]; set => Arguments[index] = value; }

        public int IndexOf(ArgumentInfo item) => Arguments.IndexOf(item);
        public void Insert(int index, ArgumentInfo item) => Arguments.Insert(index, item);
        public void RemoveAt(int index) => Arguments.RemoveAt(index);
        public void Add(ArgumentInfo item) => Arguments.Add(item);
        public void Clear() => Arguments.Clear();
        public bool Contains(ArgumentInfo item) => Arguments.Contains(item);
        public void CopyTo(ArgumentInfo[] array, int arrayIndex) => Arguments.CopyTo(array, arrayIndex);
        public bool Remove(ArgumentInfo item) => Arguments.Remove(item);
        public IEnumerator<ArgumentInfo> GetEnumerator() => Arguments.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Arguments.GetEnumerator();

        public struct ArgumentInfo
        {
            public ScriptValueType.Halo3ODSTValue Type;
            public string Name;

            public ArgumentInfo(ScriptValueType.Halo3ODSTValue type) : this(type, "") { }

            public ArgumentInfo(ScriptValueType.Halo3ODSTValue type, string name)
            {
                Type = type;
                Name = name;
            }
        }
        public static int GetScriptExpressionDataLength(ScriptExpression expr)
        {
            switch (expr.ExpressionType)
            {
                case ScriptExpressionType.Expression:
                    return ValueTypeSizes[expr.ValueType.HaloOnline];

                default:
                    return 4;
            }
        }

        public static Dictionary<ScriptValueType.HaloOnlineValue, int> ValueTypeSizes { get; } = new Dictionary<ScriptValueType.HaloOnlineValue, int>
        {
            [ScriptValueType.HaloOnlineValue.Invalid] = 4,
            [ScriptValueType.HaloOnlineValue.Unparsed] = 0,
            [ScriptValueType.HaloOnlineValue.SpecialForm] = 0,
            [ScriptValueType.HaloOnlineValue.FunctionName] = 4,
            [ScriptValueType.HaloOnlineValue.Passthrough] = 0,
            [ScriptValueType.HaloOnlineValue.Void] = 4,
            [ScriptValueType.HaloOnlineValue.Boolean] = 1,
            [ScriptValueType.HaloOnlineValue.Real] = 4,
            [ScriptValueType.HaloOnlineValue.Short] = 2,
            [ScriptValueType.HaloOnlineValue.Long] = 4,
            [ScriptValueType.HaloOnlineValue.String] = 4,
            [ScriptValueType.HaloOnlineValue.Script] = 2,
            [ScriptValueType.HaloOnlineValue.StringId] = 4,
            [ScriptValueType.HaloOnlineValue.UnitSeatMapping] = 4,
            [ScriptValueType.HaloOnlineValue.TriggerVolume] = 2,
            [ScriptValueType.HaloOnlineValue.CutsceneFlag] = 2,
            [ScriptValueType.HaloOnlineValue.CutsceneCameraPoint] = 2,
            [ScriptValueType.HaloOnlineValue.CutsceneTitle] = 2,
            [ScriptValueType.HaloOnlineValue.CutsceneRecording] = 2,
            [ScriptValueType.HaloOnlineValue.DeviceGroup] = 4,
            [ScriptValueType.HaloOnlineValue.Ai] = 4,
            [ScriptValueType.HaloOnlineValue.AiCommandList] = 2,
            [ScriptValueType.HaloOnlineValue.AiCommandScript] = 2,
            [ScriptValueType.HaloOnlineValue.AiBehavior] = 2,
            [ScriptValueType.HaloOnlineValue.AiOrders] = 2,
            [ScriptValueType.HaloOnlineValue.AiLine] = 4,
            [ScriptValueType.HaloOnlineValue.StartingProfile] = 2,
            [ScriptValueType.HaloOnlineValue.Conversation] = 2,
            [ScriptValueType.HaloOnlineValue.ZoneSet] = 2,
            [ScriptValueType.HaloOnlineValue.DesignerZone] = 2,
            [ScriptValueType.HaloOnlineValue.PointReference] = 4,
            [ScriptValueType.HaloOnlineValue.Style] = 4,
            [ScriptValueType.HaloOnlineValue.ObjectList] = 4,
            [ScriptValueType.HaloOnlineValue.Folder] = 4,
            [ScriptValueType.HaloOnlineValue.Sound] = 4,
            [ScriptValueType.HaloOnlineValue.Effect] = 4,
            [ScriptValueType.HaloOnlineValue.Damage] = 4,
            [ScriptValueType.HaloOnlineValue.LoopingSound] = 4,
            [ScriptValueType.HaloOnlineValue.AnimationGraph] = 4,
            [ScriptValueType.HaloOnlineValue.DamageEffect] = 4,
            [ScriptValueType.HaloOnlineValue.ObjectDefinition] = 4,
            [ScriptValueType.HaloOnlineValue.Bitmap] = 4,
            [ScriptValueType.HaloOnlineValue.Shader] = 4,
            [ScriptValueType.HaloOnlineValue.RenderModel] = 4,
            [ScriptValueType.HaloOnlineValue.StructureDefinition] = 4,
            [ScriptValueType.HaloOnlineValue.LightmapDefinition] = 4,
            [ScriptValueType.HaloOnlineValue.CinematicDefinition] = 4,
            [ScriptValueType.HaloOnlineValue.CinematicSceneDefinition] = 4,
            [ScriptValueType.HaloOnlineValue.BinkDefinition] = 4,
            [ScriptValueType.HaloOnlineValue.AnyTag] = 4,
            [ScriptValueType.HaloOnlineValue.AnyTagNotResolving] = 4,
            [ScriptValueType.HaloOnlineValue.GameDifficulty] = 2,
            [ScriptValueType.HaloOnlineValue.Team] = 2,
            [ScriptValueType.HaloOnlineValue.MpTeam] = 2,
            [ScriptValueType.HaloOnlineValue.Controller] = 2,
            [ScriptValueType.HaloOnlineValue.ButtonPreset] = 2,
            [ScriptValueType.HaloOnlineValue.JoystickPreset] = 2,
            [ScriptValueType.HaloOnlineValue.PlayerCharacterType] = 2,
            [ScriptValueType.HaloOnlineValue.VoiceOutputSetting] = 2,
            [ScriptValueType.HaloOnlineValue.VoiceMask] = 2,
            [ScriptValueType.HaloOnlineValue.SubtitleSetting] = 2,
            [ScriptValueType.HaloOnlineValue.ActorType] = 2,
            [ScriptValueType.HaloOnlineValue.ModelState] = 2,
            [ScriptValueType.HaloOnlineValue.Event] = 2,
            [ScriptValueType.HaloOnlineValue.CharacterPhysics] = 2,
            [ScriptValueType.HaloOnlineValue.PrimarySkull] = 2,
            [ScriptValueType.HaloOnlineValue.SecondarySkull] = 4,
            [ScriptValueType.HaloOnlineValue.Object] = 4,
            [ScriptValueType.HaloOnlineValue.Unit] = 4,
            [ScriptValueType.HaloOnlineValue.Vehicle] = 4,
            [ScriptValueType.HaloOnlineValue.Weapon] = 4,
            [ScriptValueType.HaloOnlineValue.Device] = 4,
            [ScriptValueType.HaloOnlineValue.Scenery] = 4,
            [ScriptValueType.HaloOnlineValue.EffectScenery] = 4,
            [ScriptValueType.HaloOnlineValue.ObjectName] = 2,
            [ScriptValueType.HaloOnlineValue.UnitName] = 2,
            [ScriptValueType.HaloOnlineValue.VehicleName] = 2,
            [ScriptValueType.HaloOnlineValue.WeaponName] = 2,
            [ScriptValueType.HaloOnlineValue.DeviceName] = 2,
            [ScriptValueType.HaloOnlineValue.SceneryName] = 2,
            [ScriptValueType.HaloOnlineValue.EffectSceneryName] = 4,
            [ScriptValueType.HaloOnlineValue.CinematicLightprobe] = 4,
            [ScriptValueType.HaloOnlineValue.AnimationBudgetReference] = 4,
            [ScriptValueType.HaloOnlineValue.LoopingSoundBudgetReference] = 4,
            [ScriptValueType.HaloOnlineValue.SoundBudgetReference] = 4
        };

        public static Dictionary<CacheVersion, Dictionary<int, ScriptInfo>> Scripts { get; } = new Dictionary<CacheVersion, Dictionary<int, ScriptInfo>>
        {
            [CacheVersion.Halo3Retail] = new Dictionary<int, ScriptInfo>
            {
                [0x000] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "begin"),
                [0x001] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "begin_random"),
                [0x002] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "if"),
                [0x003] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "cond"),
                [0x004] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "set"),
                [0x005] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "and"),
                [0x006] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "or"),
                [0x007] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "+"),
                [0x008] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "-"),
                [0x009] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "*"),
                [0x00A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "/"),
                [0x00B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "min"),
                [0x00C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "max"),
                [0x00D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "="),
                [0x00E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "!="),
                [0x00F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, ">"),
                [0x010] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "<"),
                [0x011] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, ">="),
                [0x012] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "<="),
                [0x013] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sleep"),
                [0x014] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sleep_forever"),
                [0x015] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "sleep_until"),
                [0x016] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "wake"),
                [0x017] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "inspect"),
                [0x018] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "unit"),
                [0x019] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "not")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x01A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "pin")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x01B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "print")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x01C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "log_print")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x01D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference2")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x01E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference3")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x01F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference4")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x020] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference5")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x021] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference7")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },

                [0x022] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_active_scripts"),
                [0x023] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "get_executing_running_thread"),
                [0x024] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_thread")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x025] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "script_started")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x026] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "script_finished")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x027] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "players"),
                [0x028] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_volume_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x029] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_volume_disable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "volume_teleport_players_not_inside")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x02B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x02C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x02D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_objects_all")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x02E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_players")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_players_all")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x030] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "volume_return_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x031] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "volume_return_objects_by_type")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x032] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "zone_set_trigger_volume_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x033] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "list_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x034] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "list_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x035] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "list_count_not_dead")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x036] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x037] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_random")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x038] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_at_ai_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x039] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_on_object_marker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x03A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_on_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x03B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_new")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x03C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_object_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x03D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_objects_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x03E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x03F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x040] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_players")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                },
                [0x041] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "soft_ceiling_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x042] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x043] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_clone")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x044] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_anew")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x045] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_if_necessary")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x046] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x047] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_clone_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x048] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_anew_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x049] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_folder")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Folder),
                },
                [0x04A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x04B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x04C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_all"),
                [0x04D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_type_mask")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x04E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_delete_by_definition")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x04F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_folder")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Folder),
                },
                [0x050] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_hide")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x051] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shadowless")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x052] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "object_buckling_magnitude_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x053] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_function_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x054] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_function_variable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x055] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_clear_function_variable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x056] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_clear_all_function_variables")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x057] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_dynamic_simulation_disable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x058] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_phantom_power")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x059] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_wake_physics")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x05A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_ranged_attack_inhibited")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x05B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_melee_attack_inhibited")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x05C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_dump_memory"),
                [0x05D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "object_get_health")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x05E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "object_get_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x05F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x060] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_physics")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x061] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "object_get_parent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x062] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_attach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x063] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "object_at_marker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x064] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_detach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x065] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x066] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x067] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown1")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x068] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_collision_damage_armor_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x069] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x06A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_deleted_when_deactivated")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x06B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_copy_player_appearance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x06C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "object_model_target_destroyed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x06D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "object_model_targets_destroyed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x06E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_damage_damage_section")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x06F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cannot_die")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x070] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "object_vitality_pinned")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x071] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "garbage_collect_now"),
                [0x072] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "garbage_collect_unsafe"),
                [0x073] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "garbage_collect_multiplayer"),
                [0x074] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cannot_take_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x075] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_can_take_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x076] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cinematic_lod")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x077] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cinematic_collision")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x078] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cinematic_visibility")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x079] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x07A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_predict_high")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x07B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_predict_low")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x07C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_type_predict_high")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x07D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_type_predict_low")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x07E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_type_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x07F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x080] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_teleport_to_ai_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x081] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_facing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x082] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x083] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_stun")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x084] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_stun_infinite")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x085] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_permutation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x086] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_region_state")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ModelState),
                },
                [0x087] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "objects_can_see_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x088] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "objects_can_see_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x089] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "objects_distance_to_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x08A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "objects_distance_to_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x08B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "map_info"),
                [0x08C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "position_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x08D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "shader_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Shader),
                },
                [0x08E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "bitmap_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Bitmap),
                },
                [0x08F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "script_recompile"),
                [0x090] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "script_doc"),
                [0x091] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "help")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x092] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "game_engine_objects"),
                [0x093] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "random_range")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x094] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "real_random_range")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x095] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_constants_reset"),
                [0x096] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_set_gravity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x097] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_set_velocity_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x098] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_disable_character_ground_adhesion_forces")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x099] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_debug_start"),
                [0x09A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_dump_world")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x09B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_dump_world_close_movie"),
                [0x09C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_profile_start"),
                [0x09D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_profile_end"),
                [0x09F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_reset_allocated_state"),
                [0x0A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "breakable_surfaces_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "breakable_surfaces_reset"),
                [0x0A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "recording_play")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "recording_play_and_delete")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "recording_play_and_hover")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0A5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "recording_kill")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0A6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "recording_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "render_lights")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0A8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "print_light_state"),
                [0x0A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_lights_enable_cinematic_shadow")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0AA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_object_marker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0AB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0AC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_target")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_on"),
                [0x0B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_off"),
                [0x0B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_aspect_ratio")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0B8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_enable_dynamic_lights")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_all_fog_planes")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0C1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_all_cluster_errors")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0C2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_line_opacity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0C3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_text_opacity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0C4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_opacity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0CC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0CD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0CE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0CF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_relative_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0D0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_relative_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_idle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                },
                [0x0D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "scenery_get_animation_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                },
                [0x0D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_can_blink")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0D5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_active_camo")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0D6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_open")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_close")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0D8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_kill")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0D9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_kill_silent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0DA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_is_emitting")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "unit_get_custom_animation_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_stop_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_relative_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0E1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_list")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0E2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_custom_animation_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_is_playing_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_custom_animations_hold_on_last_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_custom_animations_prevent_lipsync_head_movement")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_actively_controlled")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "unit_get_team_index")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0EB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_aim_without_turning")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_enterable_by_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_get_enterable_by_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_only_takes_damage_from_players_team")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0EF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_enter_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_falling_damage_disable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "object_get_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_board_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_emotion_by_?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_emotion_by_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_enable_eye_tracking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_integrated_flashlight")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_in_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_test_seat_list")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x0FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_test_seat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_prefer_tight_camera_track")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0FE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_maximum_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0FF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "units_set_maximum_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x100] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_current_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x101] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "units_set_current_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x102] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "vehicle_load_magic")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x103] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "vehicle_unload")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x104] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_animation_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x105] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "magic_melee_attack"),
                [0x106] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "vehicle_riders")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x107] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "vehicle_driver")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x108] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "vehicle_gunner")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x109] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "unit_get_health")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x10A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "unit_get_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x10B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "unit_get_total_grenade_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x10C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_has_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x10D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_has_weapon_readied")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x110] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_lower_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x111] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_raise_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x112] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_drop_support_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x113] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_spew_action")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x114] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_animation_forced_seat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x115] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_doesnt_drop_items")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x116] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_impervious")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x117] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_suspended")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x118] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_add_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StartingProfile),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x119] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "weapon_hold_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Weapon),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x11A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "weapon_enable_warthog_chaingun_light")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x11B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_set_never_appears_locked")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x11C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_set_power")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x11D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "device_get_power")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                },
                [0x11E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_set_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x11F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "device_get_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                },
                [0x120] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_set_position_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x121] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "device_group_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                },
                [0x122] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_group_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x123] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_group_set_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x124] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_one_sided_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x126] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_operates_automatically_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x127] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_closes_automatically_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x128] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_group_change_only_once_more_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x129] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_set_position_track")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x12A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_set_overlay_track")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x12B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_animate_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x12C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_animate_overlay")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x12D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_all_powerups"),
                [0x12E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_all_weapons"),
                [0x12F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_all_vehicles"),
                [0x130] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_teleport_to_camera"),
                [0x131] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_active_camouflage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x132] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_active_camouflage_by_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x133] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheats_load"),
                [0x135] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "drop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x136] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x137] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_enabled"),
                [0x138] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x139] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x13A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown7")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x13B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_log_reset"),
                [0x13C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_log_dump")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x13D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "ai_get_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x13E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "ai_get_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x13F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_get_squad")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x140] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_get_turret_ai")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x141] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.PointReference, "ai_random_smart_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x142] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.PointReference, "ai_nearest_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x143] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "ai_get_point_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x144] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.PointReference, "ai_point_set_get_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x145] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_attach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x146] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_attach_units")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x147] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_detach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x148] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_detach_units")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x149] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x14A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x14B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_place_in_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x14C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_cannot_die")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x14D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_vitality_pinned")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x14E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_resurrect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x14F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_kill")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x150] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_kill_silent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x151] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_erase")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x152] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_erase_all"),
                [0x153] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_disposable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x154] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_select")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x155] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_deselect"),
                [0x156] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_deaf")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x157] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_blind")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x158] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_weapon_up")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x159] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_flood_disperse")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x15A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_magically_see")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x15B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_magically_see_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x15C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_active_camo")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x15D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_suppress_combat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x15E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_migrate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x15F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allegiance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x160] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allegiance_remove")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x161] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allegiance_break")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x162] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_braindead")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x163] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_braindead_by_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x164] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_disregard")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x165] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_prefer_target")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x166] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_prefer_target_team")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x167] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_prefer_target_ai")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x168] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x169] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x16A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_teleport_to_starting_location_if_outside_bsp")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x16B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x16C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_bring_forward")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x16F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "biped_morph")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x170] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_renew")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x171] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_force_active")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x172] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_force_active_by_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x173] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_playfight")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x174] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_reconnect"),
                [0x175] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_berserk")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x176] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_team")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x177] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allow_dormant")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x178] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_is_attacking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x179] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_fighting_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x17A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_living_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x17B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_living_fraction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x17C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_in_vehicle_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x17D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_body_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x17E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_strength")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x17F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_swarm_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x180] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_nonswarm_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x181] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "ai_actors")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x182] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_allegiance_broken")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x183] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_spawn_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x184] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "object_get_ai")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x187] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_set_task")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x188] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_set_objective")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x189] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_task_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x18A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_set_task_condition")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x18D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_task_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x190] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_activity_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x191] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_activity_abort")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x192] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "ai_vehicle_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x193] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_starting_location")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x194] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve_seat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x195] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x196] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_player_get_vehicle_squad")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x199] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_in_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x19A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_in_vehicle?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x19B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_player_any_needs_vehicle"),
                [0x19C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x19D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x19E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x19F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x1A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_enter_squad_vehicles")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_overturned")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x1A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_flip")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x1A5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1A8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_create")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_delete")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_verify_tags"),
                [0x1AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_wall_lean")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x1B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line_at_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x1B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line_on_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x1B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line_on_object_allied")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x1B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1B4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1B5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_time_pause")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1B6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_points")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1B7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_primary_skull")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1B8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_secondary_skull")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1B9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_metagame_enabled"),
                [0x1BA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_easy"),
                [0x1BB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_normal"),
                [0x1BC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_heroic"),
                [0x1BD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_legendary"),
                [0x1BE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_run_command_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1BF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_queue_command_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stack_command_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1C1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1C2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1C3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1C4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1C5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1C6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1C7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1C8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1C9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1CA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "vs_role")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1CB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1CC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1CD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1CE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1CF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1D0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1D5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1D6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1D8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_set_cleanup_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Script),
                },
                [0x1DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_release")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_release_all"),
                [0x1DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "cs_command_script_running")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "cs_command_script_queued")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "cs_number_queued")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom_movement")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom_?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1E7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1EB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1EF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x1FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_and_posture")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1FE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_and_posture")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1FF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_nearest")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x200] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_nearest")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x201] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_in_direction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x202] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_in_direction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x203] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_towards?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x204] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_towards?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x205] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_towards")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x206] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_towards")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x207] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_swarm_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x208] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_swarm_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x209] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_swarm_from")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x20A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_swarm_from")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x20B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_pause")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x20C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_pause")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x20D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_grenade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x20E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_grenade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x20F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x210] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x211] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_jump")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x212] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_jump")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x213] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_jump_to_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x214] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_jump_to_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x215] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vocalize")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x216] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vocalize")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x217] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x218] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x219] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_action")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x21E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_action")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x21F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_action_at_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x220] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_action_at_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x221] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_action_at_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x222] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_action_at_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x223] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x224] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x225] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x226] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x227] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x228] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x229] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x22A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x22B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x22C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x22D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x22E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x22F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_line")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x230] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_line")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x233] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_deploy_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x234] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_deploy_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x235] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_approach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x236] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_approach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x237] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_approach_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x238] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_approach_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x239] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x23A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x23B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x23C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x23D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_set_style")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Style),
                },
                [0x23E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_set_style")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Style),
                },
                [0x23F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_force_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x240] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_force_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x241] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_targeting")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x242] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_targeting")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x243] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_looking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x244] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_looking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x245] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_moving")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x246] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_moving")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x247] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x248] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x249] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_suppress_activity_termination")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x24A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_suppress_activity_termination")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x24B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x24C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x24D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_look")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x24E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_look")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x24F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_look_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x250] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_look_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x251] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_look_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x252] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_look_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x253] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_aim")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x254] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_aim")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x255] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_aim_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x256] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_aim_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x257] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_aim_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x258] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_aim_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x259] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x25A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x25B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_face_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x25C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_face_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x25D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_face_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x25E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_face_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x25F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x260] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x261] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x262] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x263] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x264] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x265] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x266] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x267] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_lower_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x268] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_lower_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x269] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vehicle_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x26A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vehicle_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x26B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x26C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x26D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vehicle_boost")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x26E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vehicle_boost")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x26F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_turn_sharpness?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x270] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_turn_sharpness?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x271] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x272] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x275] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_ignore_obstacles")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x276] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_ignore_obstacles")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x277] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_approach_stop"),
                [0x278] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_approach_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x279] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_movement_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x27A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_movement_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x27B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x27E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x27F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_walk")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x280] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_walk")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x281] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_posture_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x282] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_posture_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x283] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_posture_exit"),
                [0x284] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_posture_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x285] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stow"),
                [0x286] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_stow")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x287] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_draw"),
                [0x288] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_draw")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x289] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x28A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x28B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stop_custom_animation"),
                [0x28C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_stop_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x28D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stop_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x28E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_stop_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x295] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_control")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x296] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x297] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x298] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x299] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x29A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_with_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_?boolean")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x29D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_?boolean_real")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_predict_resources_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x29F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_predict_resources_at_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_first_person")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x2A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_cinematic"),
                [0x2A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_cinematic_scene")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x2A5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "camera_time"),
                [0x2A6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_field_of_view")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2A8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_camera_set_easing_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_print")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2AA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_pan")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2AB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_pan")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2AC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_save"),
                [0x2AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_load"),
                [0x2AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_save_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_load_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "director_debug_camera")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get"),
                [0x2B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get_real"),
                [0x2B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "game_insertion_point_get"),
                [0x2B4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_insertion_point_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2B5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pvs_set_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x2B6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pvs_set_camera")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2B7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pvs_clear"),
                [0x2BA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_enable_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2BB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_disable_movement")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2BD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_flashlight_on"),
                [0x2BE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_active_camouflage_on"),
                [0x2BF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_camera_control")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_action_test_reset"),
                [0x2C1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_jump"),
                [0x2C2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_primary_trigger"),
                [0x2C3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_grenade_trigger"),
                [0x2C4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_vision_trigger"),
                [0x2C5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_zoom"),
                [0x2C6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_rotate_weapons"),
                [0x2C7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_rotate_grenades"),
                [0x2C8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_melee"),
                [0x2C9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_action"),
                [0x2CA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_accept"),
                [0x2CB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_cancel"),
                [0x2CC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_up"),
                [0x2CD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_down"),
                [0x2CE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_left"),
                [0x2CF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_right"),
                [0x2D0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_all_directions"),
                [0x2D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_move_relative_all_directions"),
                [0x2D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_cinematic_skip"),
                [0x2D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_start"),
                [0x2D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_back"),
                [0x2D5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_unknown_float1"),
                [0x2D6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_unknown_float2"),
                [0x2D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player0_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x2D8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player1_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x2D9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player2_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x2DA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player3_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x2DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_action_test_look_up_begin")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_action_test_look_down_begin")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_action_test_look_pitch_end"),
                [0x2DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_lookstick_forward"),
                [0x2DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_lookstick_backward"),
                [0x2E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_teleport_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x2E1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "map_reset"),
                [0x2E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "switch_zone_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ZoneSet),
                },
                [0x2E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "current_zone_set"),
                [0x2E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "current_zone_set_fully_active"),
                [0x2E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "crash")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "version"),
                [0x2EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "status"),
                [0x2EB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "record_movie")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "record_movie_distributed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x2ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "screenshot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2EF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "screenshot_big")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "screenshot_big_jittered")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "UnknownString")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "main_menu"),
                [0x2F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "main_halt"),
                [0x2F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "map_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },

                [0x2F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_multiplayer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_splitscreen")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x2FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_difficulty")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.GameDifficulty),
                },
                [0x301] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x304] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_rate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x308] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_memory"),
                [0x309] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_memory_by_file"),
                [0x30A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_memory_for_file")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x30B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_tags"),
                [0x30C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "tags_verify_all"),
                [0x31A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_control_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x31B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_control_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x31D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_break_on_vocalization")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x31E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "fade_in")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x31F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "fade_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x320] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_start"),
                [0x321] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_stop"),
                [0x322] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_skip_start_internal"),
                [0x323] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_skip_stop_internal"),
                [0x324] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_show_letterbox")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x325] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_show_letterbox_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x326] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_title")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x327] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_title_delayed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x328] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_suppress_bsp_object_creation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x329] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_subtitle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x32A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicDefinition),
                },
                [0x32B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_shot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x32D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_early_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x32E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "cinematic_get_early_exit"),
                [0x331] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_object_create_cinematic_anchor")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x332] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_object_destroy")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x333] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_destroy"),
                [0x334] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_clips_initialize_for_shot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x335] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_clips_destroy"),
                [0x336] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lights_initialize_for_shot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x337] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lights_destroy"),
                [0x338] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_light_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicLightprobe),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x33A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lighting_rebuild_all"),
                [0x33B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "cinematic_object_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x33C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "cinematic_object_get_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x33D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Scenery, "cinematic_object_get_scenery")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x33E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.EffectScenery, "cinematic_object_get_effect_scenery")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x340] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_briefing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x341] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.CinematicDefinition, "cinematic_tag_reference_get_cinematic")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x342] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition, "cinematic_tag_reference_get_scene")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x343] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Effect, "cinematic_tag_reference_get_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x344] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x345] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound2")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x346] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.LoopingSound, "cinematic_tag_reference_get_looping_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x347] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph, "cinematic_tag_reference_get_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x348] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "cinematic_tag_reference_get_?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x349] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_fade_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x34D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x34E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x34F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_music")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x350] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x351] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_stop_music")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x353] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x354] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_object_no_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x356] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_play_cortana_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x357] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "attract_mode_start"),
                [0x358] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "attract_mode_set_seconds")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x359] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_won"),
                [0x35A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_lost")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x35B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_revert"),
                [0x35C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_is_cooperative"),
                [0x35D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_is_playtest"),
                [0x35E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_can_use_flashlights")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x35F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_and_quit"),
                [0x360] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_unsafe"),
                [0x361] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_insertion_point_unlock")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x362] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_insertion_point_lock?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x388] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load"),
                [0x389] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x38A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_save"),
                [0x38B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_save_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x38C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load_game"),
                [0x38D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load_game_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x38E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_regular_upload_to_debug_server")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x38F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_set_upload_option")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x392] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_safe_to_save"),
                [0x393] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_safe_to_speak"),
                [0x394] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_all_quiet"),
                [0x395] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save"),
                [0x396] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_cancel"),
                [0x397] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_no_timeout"),
                [0x398] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_immediate"),
                [0x399] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_saving"),
                [0x39A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_reverted"),
                [0x39B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_set_tag_parameter_unsafe")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x39C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x39D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x39E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x39F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_cinematic")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_with_subtitle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "sound_impulse_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "sound_impulse_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "sound_impulse_language_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3A5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3A6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_3d")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_mark_as_outro")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                },
                [0x3AA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3AB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                },
                [0x3AC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_stop_immediately")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                },
                [0x3AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_set_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_set_alternate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_loop_spam"),
                [0x3B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_show_channel")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_debug_sound_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_sounds_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_set_gain")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x3B4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_set_gain_db")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x3B5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_enable_ducker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3B6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_sound_environment_parameter")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3B7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_set_global_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3B8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_set_global_effect_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3B9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_auto_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3BA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_hover")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3BB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "vehicle_count_bipeds_killed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x3BC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "biped_ragdoll")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x3BE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_show_training_text")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3BF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_set_training_text")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_enable_training")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3C1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_flashlight"),
                [0x3C2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_crouch"),
                [0x3C3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_stealth"),
                [0x3C4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_?"),
                [0x3C5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_jump"),
                [0x3C6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_activate_team_nav_point_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3C7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_deactivate_team_nav_point_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x3C8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_cortana_suck")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3CB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "play_cortana_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3CD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_weapon_stats")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3CE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_health")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3CF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3D0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_messages")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_motion_sensor")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_spike_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_fire_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3D5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_cinematic_fade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3D6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cls"),
                [0x3D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_overflow_suppression")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3D8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_show")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x3D9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_hide")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x3DA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_show_all"),
                [0x3DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_hide_all"),
                [0x3DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_list"),
                [0x3DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_translation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_rotation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_rumble")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3E1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3E2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "time_code_show")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "time_code_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "time_code_reset"),
                [0x3E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "script_screen_effect_set_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3E7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade2")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_stop"),
                [0x3EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_near_clip_distance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3EB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_far_clip_distance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_atmosphere_fog")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "motion_blur")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_weather")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3EF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_patchy_fog")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_environment_map_attenuation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_environment_map_bitmap")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Bitmap),
                },
                [0x3F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_bitmap"),
                [0x3F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_environment_map_tint")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_tint"),
                [0x3F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_layer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_has_skills"),
                [0x3F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_has_mad_secret_skills")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "controller_invert_look"),
                [0x3F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "controller_look_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x3FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "controller_set_look_invert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "controller_get_look_invert"),
                [0x3FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "controller_unlock_solo_levels")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x44A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_clear"),
                [0x44B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_show_up_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x44C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_finish_up_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x44D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_secondary_show")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x44E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_primary_unavailable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x44F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_secondary_unavailable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x46C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_delegate_host")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x46D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_delegate_leader")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x46E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_map_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x470] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_campaign_difficulty")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x471] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_player_color")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x489] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_set_mission_segment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x48A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_display_mission_segment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x48B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_insert"),
                [0x48D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_upload"),
                [0x48E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_playback")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x490] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4BB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "bug_now")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x4BC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "bug_now_lite")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x4BD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "bug_now_auto")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4BE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "object_list_children")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x4BF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "voice_set_outgoing_channel_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "voice_set_voice_repeater_peer_index")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4C7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x4C8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_start_smooth")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x4C9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x4CA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_restart")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x4CB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "interpolator_is_active")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x4CC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "interpolator_is_finished")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x4CD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_set_current_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x4CE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_current_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4CF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_start_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4D0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_final_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_current_phase")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x4D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_current_time_fraction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x4D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_start_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x4D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_final_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x4D5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4D6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_fraction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4D8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_delta")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4D9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "interpolator_stop_all"),
                [0x4DA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "interpolator_restart_all"),
                [0x4DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "interpolator_flip"),
                [0x4DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "animation_cache_stats_reset"),
                [0x4DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_clone_players_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x4DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_move_attached_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x4E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_enable_ghost_effects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4E1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "set_global_sound_environment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x4E2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "reset_dsp_image"),
                [0x4E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_cinematic_skip"),
                [0x4E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_outro_start"),
                [0x4E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_enable_ambience_details")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_suppress_ambience_update_on_revert"),
                [0x4F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_exposure_fade_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x4F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_exposure")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x4F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_autoexposure_instant")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_depth_of_field_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x4FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_depth_of_field")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x500] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_disable?"),
                [0x501] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_enable?"),
                [0x502] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "predict_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x503] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "game_team_get_players?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x504] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "game_team_get_player_count?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x508] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "mp_ai_allegiance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x509] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "mp_allegiance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x534] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "game_coop_player_count"),
                [0x539] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "add_recycling_volume")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x556] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_always_active")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x55C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_persistent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x57E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_zone_activate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x57F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_zone_deactivate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x583] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_limit_lipsync_to_mouth_only")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x58A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "prepare_to_switch_to_zone_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ZoneSet),
                },
                [0x58B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_zone_activate_for_debugging")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x58C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_play_random_ping")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x58D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_fade_out_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x58E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_fade_in_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x58F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_lock_gaze")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x590] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_unlock_gaze")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x591] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_scale_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x598] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_custom_animation_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x599] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x59A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "film_manager_set_reproduction_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x59B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_arbiter_ai_navpoint")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x59F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "biped_force_ground_fitting_on")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x5A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_chud_objective")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x5A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "terminal_is_being_read"),
                [0x5A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "terminal_was_accessed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x5A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "terminal_was_completed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x5A5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "NULL"),
                [0x5A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_award_level_complete_achievements"),
                [0x5A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_safe_to_respawn")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x5AA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cortana_effect_kill"),
                [0x5AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_cortana_effect_cinematic"),
                [0x5AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_migrate_infanty")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x5AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_cinematic_motion_blur")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x5B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dont_do_avoidance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x5B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_clean_up")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x5B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_erase_inactive")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x5B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "stop_bink_movie"),
                [0x5B4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "play_credits_unskippable"),

                [0x5B5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown9")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.SoundBudgetReference),
                },
                [0x5B6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown10")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Controller),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean)
                },
                [0x5B7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown11"),
                [0x5B8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown12")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                }
            },
            [CacheVersion.Halo3ODST] = new Dictionary<int, ScriptInfo>
            {
                [0x000] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "begin"),
                [0x001] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "begin_random"),
                [0x002] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "if"),
                [0x003] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "cond"),
                [0x004] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "set"),
                [0x005] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "and"),
                [0x006] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "or"),
                [0x007] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "+"),
                [0x008] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "-"),
                [0x009] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "*"),
                [0x00A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "/"),
                [0x00B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "min"),
                [0x00C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "max"),
                [0x00D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "="),
                [0x00E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "!="),
                [0x00F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, ">"),
                [0x010] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "<"),
                [0x011] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, ">="),
                [0x012] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "<="),
                [0x013] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sleep"),
                [0x014] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sleep_forever"),
                [0x015] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "sleep_until"),
                [0x016] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "wake"),
                [0x017] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "inspect"),
                [0x018] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "unit"),
                [0x019] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "evaluate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Script),
                },
                [0x01A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "not")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x01B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "pin")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x01C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "print")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x01D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference1")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x01E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference2")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x01F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference3")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x020] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference4")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x021] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference5")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x022] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference6")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x023] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference7")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x024] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference8")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x025] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_active_scripts"),
                [0x026] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "get_executing_running_thread"),
                [0x027] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_thread")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x028] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "script_started")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x029] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "script_finished")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x02A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "players"),
                [0x02B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "player_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x02C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_volume_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_volume_disable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "volume_teleport_players_not_inside")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x02F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x030] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x031] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_objects_all")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x032] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_players")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x033] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_players_all")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x034] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "volume_return_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x035] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "volume_return_objects_by_type")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x036] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "zone_set_trigger_volume_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x037] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "list_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x038] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "list_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x039] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "list_count_not_dead")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x03A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x03B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_random")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x03C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_at_ai_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x03D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_on_object_marker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x03E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_on_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x03F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_new")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x040] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_object_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x041] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_objects_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x042] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x043] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x044] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_players")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                },
                [0x045] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "soft_ceiling_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x046] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x047] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_clone")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x048] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_anew")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x049] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_if_necessary")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x04A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x04B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_clone_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x04C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_anew_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x04D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_folder")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Folder),
                },
                [0x04E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_folder_anew")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Folder),
                },
                [0x04F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x050] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x051] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_all"),
                [0x052] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_type_mask")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x053] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_delete_by_definition")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x054] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_folder")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Folder),
                },
                [0x055] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_hide")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x056] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shadowless")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x057] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "object_buckling_magnitude_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x058] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_function_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x059] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_function_variable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x05A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_clear_function_variable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x05B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_clear_all_function_variables")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x05C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_dynamic_simulation_disable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x05D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_phantom_power")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x05E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_wake_physics")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x05F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_ranged_attack_inhibited")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x060] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_melee_attack_inhibited")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x061] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_dump_memory"),
                [0x062] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "object_get_health")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x063] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "object_get_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x064] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x065] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_physics")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x066] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "object_get_parent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x067] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_attach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x068] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "object_at_marker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x069] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_detach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x06A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x06B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x06C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown1")// survival_bonus_round
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x06D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_collision_damage_armor_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x06E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x06F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_deleted_when_deactivated")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x070] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_copy_player_appearance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x071] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "object_model_target_destroyed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x072] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "object_model_targets_destroyed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x073] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_damage_damage_section")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x074] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cannot_die")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x075] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "object_vitality_pinned")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x076] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "garbage_collect_now"),
                [0x077] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "garbage_collect_unsafe"),
                [0x078] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "garbage_collect_multiplayer"),
                [0x079] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cannot_take_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x07A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_can_take_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x07B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cinematic_lod")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x07C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cinematic_collision")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x07D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cinematic_visibility")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x07E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x07F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_predict_high")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x080] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_predict_low")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x081] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_type_predict_high")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x082] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_type_predict_low")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x083] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_type_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x084] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x085] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_teleport_to_ai_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x086] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_facing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x087] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x088] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_normalized")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x089] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_stun")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x08A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_stun_infinite")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x08B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_permutation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x08C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_variant")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x08D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_region_state")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ModelState),
                },
                [0x08E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "objects_can_see_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x08F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "objects_can_see_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x090] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "objects_distance_to_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x091] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "objects_distance_to_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x092] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "map_info"),
                [0x093] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "position_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x094] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "shader_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Shader),
                },
                [0x095] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "bitmap_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Bitmap),
                },
                [0x096] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "script_recompile"),
                [0x097] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "script_doc"),
                [0x098] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "help")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x099] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "game_engine_objects"),
                [0x09A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "random_range")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x09B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "real_random_range")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x09C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_constants_reset"),
                [0x09D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_set_gravity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x09E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_set_velocity_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x09F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_disable_character_ground_adhesion_forces")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_debug_start"),
                [0x0A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_dump_world")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_dump_world_close_movie"),
                [0x0A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_profile_start"),
                [0x0A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_profile_end"),
                [0x0A6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_reset_allocated_state"),
                [0x0A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "breakable_surfaces_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0A8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "breakable_surfaces_reset"),
                [0x0A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "recording_play")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "recording_play_and_delete")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "recording_play_and_hover")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "recording_kill")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "recording_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "render_lights")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_lights_enable_cinematic_shadow")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_object_marker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ambient_track")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real)
                },
                [0x0B7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_on"),
                [0x0B8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown2"),
                [0x0B9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_off"),
                [0x0BA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_aspect_ratio")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_all_fog_planes")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_all_cluster_errors")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0D5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_line_opacity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0D6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_text_opacity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_opacity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_relative_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0E7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_relative_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_idle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                },
                [0x0EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "scenery_get_animation_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                },
                [0x0EB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_can_blink")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_active_camo")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_open")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_close")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0EF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_kill")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_kill_silent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_is_emitting")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "unit_get_custom_animation_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_stop_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_relative_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_list")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_custom_animation_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_is_playing_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_custom_animations_hold_on_last_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_custom_animations_prevent_lipsync_head_movement")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x100] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_actively_controlled")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x101] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "unit_get_team_index")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x102] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_aim_without_turning")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x103] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_enterable_by_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x104] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_get_enterable_by_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x105] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_only_takes_damage_from_players_team")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x106] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_enter_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x107] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_falling_damage_disable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x108] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_in_vehicle_type")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x10A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "object_get_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x10B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_board_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x10C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_emotion_by_?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x10D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_emotion_by_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x10E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_enable_eye_tracking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x10F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_integrated_flashlight")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x110] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown3")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnyTag),
                },
                [0x111] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_enable_vision_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x112] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_in_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x113] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_test_seat_list")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x114] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_test_seat_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x115] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_test_seat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x116] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_prefer_tight_camera_track")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x117] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x118] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x119] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_maximum_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x11A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "units_set_maximum_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x11B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_current_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x11C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "units_set_current_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x11D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "vehicle_load_magic")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x11E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "vehicle_unload")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x11F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_animation_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x120] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "magic_melee_attack"),
                [0x121] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "vehicle_riders")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x122] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "vehicle_driver")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x123] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "vehicle_gunner")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x124] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "unit_get_health")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x125] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "unit_get_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x126] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "unit_get_total_grenade_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x127] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_has_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x128] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_has_weapon_readied")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x12A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_has_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x12B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_lower_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x12C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_raise_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x12D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_drop_support_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x130] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown4"),
                [0x131] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_animation_forced_seat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x132] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_doesnt_drop_items")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x133] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_impervious")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x134] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_suspended")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x135] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_add_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StartingProfile),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x136] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "weapon_hold_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Weapon),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x137] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "weapon_enable_warthog_chaingun_light")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x138] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_set_never_appears_locked")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x139] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_set_power")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x13A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "device_get_power")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                },
                [0x13B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_set_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x13C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "device_get_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                },
                [0x13D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_set_position_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x13E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "device_group_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                },
                [0x13F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_group_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x140] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_group_set_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x141] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_one_sided_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x143] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_operates_automatically_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x144] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_closes_automatically_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x145] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_group_change_only_once_more_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x146] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_set_position_track")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x147] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_set_overlay_track")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x148] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_animate_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x149] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_animate_overlay")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x14A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_all_powerups"),
                [0x14B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_all_weapons"),
                [0x14C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_all_vehicles"),
                [0x14D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_teleport_to_camera"),
                [0x14E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_active_camouflage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x14F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_active_camouflage_by_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x150] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheats_load"),
                [0x151] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown5")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnyTag),
                },
                [0x152] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "drop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x154] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x155] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_enabled"),
                [0x156] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x157] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x158] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_player_dialogue_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x15A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_fast_and_dumb")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x15B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_log_reset"),
                [0x15C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_log_dump")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x15D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "ai_get_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x15E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "ai_get_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x15F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_get_squad")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x160] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_get_turret_ai")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x161] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.PointReference, "ai_random_smart_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x162] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.PointReference, "ai_nearest_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x163] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "ai_get_point_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x164] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.PointReference, "ai_point_set_get_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x165] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x166] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x167] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_place_in_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x168] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_cannot_die")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x169] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_vitality_pinned")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x16A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_index_from_spawn_formation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x16B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_resurrect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x16C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_kill")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x16D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_kill_silent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x16E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_erase")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x16F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_erase_all"),
                [0x170] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_disposable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x171] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_select")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x172] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_deselect"),
                [0x173] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_deaf")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x174] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_blind")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x175] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_weapon_up")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x176] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_flood_disperse")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x177] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_magically_see")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x178] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_magically_see_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x179] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_active_camo")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x17A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_suppress_combat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x17C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_grunt_kamikaze")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x17D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_migrate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x17E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allegiance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x17F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allegiance_remove")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x180] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allegiance_break")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x181] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_braindead")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x182] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_braindead_by_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x183] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_disregard")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x184] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_prefer_target")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x185] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_prefer_target_team")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x186] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_prefer_target_ai")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x187] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x188] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x189] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_teleport_to_starting_location_if_outside_bsp")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x18A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_teleport_to_starting_location_if_outside_bsp?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x18B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x18C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_bring_forward")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x18F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "biped_morph")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x190] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_renew")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x191] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_force_active")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x192] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_force_active_by_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x193] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_playfight")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x194] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_reconnect"),
                [0x195] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_berserk")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x196] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_team")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x197] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allow_dormant")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x198] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_is_attacking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x199] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_fighting_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_living_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_living_fraction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_in_vehicle_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_body_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_strength")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_swarm_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_nonswarm_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "ai_actors")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_allegiance_broken")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x1A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_spawn_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "object_get_ai")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x1AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_set_task")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_set_objective")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_task_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_set_task_condition")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1B5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_task_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1B6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_reset_objective")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1B7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_squad_patrol_objective_disallow")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1BA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_activity_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1BB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_activity_abort")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1BC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "ai_vehicle_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1BD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_starting_location")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1BE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_spawn_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1BF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_vehicle_get_from_starting_location")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_squad")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1C1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve_seat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1C2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1C3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_player_get_vehicle_squad")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x1C6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_in_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x1C7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_in_vehicle?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x1C8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_player_any_needs_vehicle"),
                [0x1C9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1CA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x1CB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1CC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x1CD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_enter_squad_vehicles")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1CE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1CF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1D0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_overturned")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x1D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_flip")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x1D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_ai_navpoint")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x1D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1D5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1D6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_create")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_delete")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_verify_tags"),
                [0x1DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_wall_lean")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x1DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line_at_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x1DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line_on_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x1E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line_on_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x1E1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1E2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_time_pause")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_points")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_primary_skull")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PrimarySkull),
                },
                [0x1E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_secondary_skull")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.SecondarySkull),
                },
                [0x1E7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_event")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_metagame_enabled"),
                [0x1E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_survival_enabled"),
                [0x1EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_easy"),
                [0x1EB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_normal"),
                [0x1EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_heroic"),
                [0x1ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_legendary"),
                [0x1EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_run_command_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1EF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_queue_command_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stack_command_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "vs_role")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1FE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1FF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x200] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x201] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x202] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x203] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x204] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x205] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x206] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x207] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x208] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x20B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_set_cleanup_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Script),
                },
                [0x20C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_release")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x20D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_release_all"),
                [0x20E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "cs_command_script_running")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x20F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "cs_command_script_queued")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x210] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "cs_number_queued")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x213] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x214] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom_movement")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x215] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom_?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x216] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x217] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x218] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x219] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x21C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x21D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x220] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x221] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x222] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x223] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x224] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x225] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x226] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x227] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x228] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x229] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x22A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x22B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x22C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x22D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_and_posture")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x22E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_and_posture")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x22F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_nearest")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x230] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_nearest")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x231] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_in_direction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x232] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_in_direction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x233] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_towards?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x234] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_towards?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x235] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_towards")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x236] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_towards")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x237] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_towards_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x238] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_towards_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x239] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_swarm_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_swarm_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_swarm_from")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_swarm_from")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_pause")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_pause")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_grenade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x240] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_grenade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x241] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x242] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x243] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_jump")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x244] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_jump")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x245] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_jump_to_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x246] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_jump_to_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x247] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vocalize")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x248] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vocalize")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x249] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x24A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x24B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x24C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x24D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x24E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x24F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_action")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x250] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_action")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x251] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_action_at_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x252] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_action_at_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x253] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_action_at_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x254] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_action_at_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x255] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x256] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x257] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x258] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x259] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x25A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x25B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x25C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x25D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x25E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x25F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x260] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x261] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_line")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x262] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_line")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x265] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_deploy_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x266] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_deploy_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x267] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_approach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x268] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_approach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x269] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_approach_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x26A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_approach_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x26B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x26C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x26D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x26E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x26F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_set_style")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Style),
                },
                [0x270] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_set_style")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Style),
                },
                [0x271] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_force_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x272] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_force_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x273] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_targeting")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x274] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_targeting")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x275] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_looking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x276] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_looking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x277] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_moving")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x278] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_moving")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x279] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_suppress_activity_termination")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_suppress_activity_termination")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_look")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x280] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_look")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x281] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_look_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x282] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_look_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x283] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_look_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x284] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_look_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x285] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_aim")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x286] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_aim")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x287] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_aim_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x288] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_aim_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x289] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_aim_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x28A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_aim_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x28B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x28C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x28D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_face_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x28E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_face_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x28F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_face_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x290] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_face_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x291] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x292] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x293] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x294] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x295] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x296] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x297] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x298] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x299] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_lower_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x29A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_lower_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x29B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vehicle_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vehicle_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vehicle_boost")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vehicle_boost")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_turn_sharpness?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_turn_sharpness?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_ignore_obstacles")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_ignore_obstacles")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_approach_stop"),
                [0x2AA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_approach_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x2AB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_movement_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2AC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_movement_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_walk")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_walk")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_posture_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2B4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_posture_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2B5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_posture_exit"),
                [0x2B6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_posture_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x2B7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stow"),
                [0x2B8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_stow")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x2B9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_draw"),
                [0x2BA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_draw")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x2BB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x2BC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x2BD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stop_custom_animation"),
                [0x2BE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_stop_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x2BF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stop_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x2C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_stop_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x2C7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_control")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2C8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2C9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x2CA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x2CB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x2CC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_with_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2CD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2CE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_?boolean")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2CF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_?boolean_real")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2D0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_predict_resources_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x2D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_predict_resources_at_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_first_person")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x2D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_cinematic"),
                [0x2D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_cinematic_scene")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x2D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "camera_time"),
                [0x2D8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_field_of_view")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2DA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_camera_set_easing_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_print")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_pan")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_pan")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_save"),
                [0x2DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_load"),
                [0x2E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_save_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2E1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_load_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2E2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "director_debug_camera")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get"),
                [0x2E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get_real"),
                [0x2E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "game_insertion_point_get"),
                [0x2E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_insertion_point_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2E7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pvs_set_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x2E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pvs_set_camera")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pvs_clear"),
                [0x2EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_enable_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_disable_movement")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2EF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_flashlight_on"),
                [0x2F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_active_camouflage_on"),
                [0x2F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_camera_control")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_action_test_reset"),
                [0x2F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_primary_trigger"),
                [0x2F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_grenade_trigger"),
                [0x2F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_vision_trigger"),
                [0x2F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_rotate_weapons"),
                [0x2F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_rotate_grenades"),
                [0x2F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_melee"),
                [0x2F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_action"),
                [0x2FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_accept"),
                [0x2FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_cancel"),
                [0x2FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_up"),
                [0x2FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_down"),
                [0x2FE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_left"),
                [0x2FF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_right"),
                [0x300] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_all_directions"),
                [0x301] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_move_relative_all_directions"),
                [0x302] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_unknown1"),
                [0x303] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_start"),
                [0x304] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_back"),
                [0x305] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_dpad_left?"),
                [0x306] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_dpad_right?"),
                [0x307] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_dpad_up"),
                [0x308] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_dpad_down"),
                [0x309] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_x"),
                [0x30A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_y"),
                [0x30B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_left_shoulder"),
                [0x30C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_right_shoulder"),
                [0x30D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_action_test_reset")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x30E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_primary_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x30F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_grenade_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x310] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_vision_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x311] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_rotate_weapons")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x312] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_rotate_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x313] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_melee")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x314] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_action")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x315] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_accept")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x316] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_cancel")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x317] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_up")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x318] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_down")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x319] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_left")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_right")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_all_directions")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_move_relative_all_directions")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_unknown1")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_unknown2")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_back")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x320] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_left?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x321] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_right?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x322] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_up")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x323] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_down")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x324] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_x")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x325] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_y")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x326] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_left_shoulder")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x327] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_right_shoulder")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x328] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_unknown_float1"),
                [0x329] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_unknown_float2"),
                [0x32A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player0_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x32B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player1_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x32C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player2_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x32D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player3_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x330] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_teleport_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x331] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "map_reset"),
                [0x334] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "switch_zone_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ZoneSet),
                },
                [0x335] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "current_zone_set"),
                [0x336] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "current_zone_set_fully_active"),
                [0x338] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "crash")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x339] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "version"),
                [0x33A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "status"),
                [0x33B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "record_movie")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x33C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "record_movie_distributed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x33D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "screenshot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x33F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "screenshot_big")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x340] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "screenshot_big_jittered")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x345] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "main_menu"),
                [0x346] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "main_halt"),
                [0x348] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "map_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x349] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_multiplayer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x34A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_splitscreen")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x34B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_difficulty")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.GameDifficulty),
                },
                [0x351] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x354] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_rate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x358] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_memory"),
                [0x359] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_memory_by_file"),
                [0x35A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_memory_for_file")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x35B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_tags"),
                [0x35C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "tags_verify_all"),
                [0x36A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_control_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x36B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_control_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x36D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_break_on_vocalization")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x36E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "fade_in")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x36F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "fade_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x370] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_start"),
                [0x371] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_stop"),
                [0x372] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_skip_start_internal"),
                [0x373] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_skip_stop_internal"),
                [0x374] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_show_letterbox")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x375] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_show_letterbox_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x376] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_title")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x377] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_title_delayed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x378] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_suppress_bsp_object_creation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x379] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_subtitle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x37A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicDefinition),
                },
                [0x37B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_shot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x37D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_early_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x37E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "cinematic_get_early_exit"),
                [0x381] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_object_create_cinematic_anchor")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x382] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_object_destroy")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x383] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_destroy"),
                [0x384] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_clips_initialize_for_shot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x385] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_clips_destroy"),
                [0x386] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lights_initialize_for_shot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x387] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lights_destroy"),
                [0x388] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lights_destroy_shot"),
                [0x389] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_light_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicLightprobe),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x38B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lighting_rebuild_all"),
                [0x38E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "cinematic_object_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x390] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_briefing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x391] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.CinematicDefinition, "cinematic_tag_reference_get_cinematic")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x392] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition, "cinematic_tag_reference_get_scene")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x393] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Effect, "cinematic_tag_reference_get_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x394] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x395] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound2")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x396] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.LoopingSound, "cinematic_tag_reference_get_looping_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x397] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph, "cinematic_tag_reference_get_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x398] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "cinematic_tag_reference_get_?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x399] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_fade_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x39C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_create_object?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x39D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x39E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x39F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_music")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x3A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_stop_music")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_object_no_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3A5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object_no_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3A6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_play_cortana_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "attract_mode_start"),
                [0x3A8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "attract_mode_set_seconds")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_level_advance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3AA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_won"),
                [0x3AB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_lost")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3AC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_revert"),
                [0x3AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_is_cooperative"),
                [0x3AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_is_playtest"),
                [0x3AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_can_use_flashlights")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_and_quit"),
                [0x3B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_unsafe"),
                [0x3B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_insertion_point_unlock")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x3B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_insertion_point_lock?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x3B9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "achievement_grant_to_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3BA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "achievement_grant_to_all_players")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3D9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load"),
                [0x3DA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x3DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_save"),
                [0x3DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_save_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x3DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load_game"),
                [0x3DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load_game_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x3DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_regular_upload_to_debug_server")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_set_upload_option")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x3E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_safe_to_save"),
                [0x3E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_safe_to_speak"),
                [0x3E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_all_quiet"),
                [0x3E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save"),
                [0x3E7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_cancel"),
                [0x3E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_no_timeout"),
                [0x3E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_immediate"),
                [0x3EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_saving"),
                [0x3EB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_reverted"),
                [0x3EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_set_tag_parameter_unsafe")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3EF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_cinematic")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_with_subtitle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "sound_impulse_language_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_3d")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_mark_as_outro")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                },
                [0x3F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                },
                [0x3FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_stop_immediately")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                },
                [0x3FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_set_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_set_alternate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3FE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_loop_spam"),
                [0x3FF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_show_channel")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x400] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_debug_sound_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x401] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_sounds_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x402] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_set_gain")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x403] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_set_gain_db")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x404] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_enable_ducker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x405] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_sound_environment_parameter")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x406] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_set_global_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x407] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_set_global_effect_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x408] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_auto_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x409] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_hover")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x40A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "vehicle_count_bipeds_killed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x40B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "biped_ragdoll")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x40D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_show_training_text")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x40E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_set_training_text")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x40F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_enable_training")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x410] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_flashlight"),
                [0x411] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_crouch"),
                [0x412] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_stealth"),
                [0x413] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_?"),
                [0x414] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_jump"),
                [0x416] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_activate_team_nav_point_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x417] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_deactivate_team_nav_point_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x418] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_cortana_suck")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x41B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "play_cortana_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x41E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_crosshair")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x41F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x420] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x421] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_messages")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x422] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_motion_sensor")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x423] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_spike_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x424] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_fire_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x425] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_compass")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x426] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_cinematic_fade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x427] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_display_pda_minimap_message")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x428] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_display_pda_objectives_message")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x429] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_display_pda_communications_message")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x42B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_object_navpoint")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x42C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_bonus_round_show_timer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x42D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_bonus_round_start_timer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x42E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_bonus_round_set_timer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x42F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cls"),
                [0x430] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_overflow_suppression")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x431] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_show")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x432] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_hide")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x433] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_show_all"),
                [0x434] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_hide_all"),
                [0x435] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_list"),
                [0x436] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_translation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x437] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_rotation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x438] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_rumble")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x439] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_translation_for_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_rotation_for_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_rumble_for_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_start_for_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_stop_for_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x440] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "time_code_show")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x441] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "time_code_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x442] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "time_code_reset"),
                [0x443] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "script_screen_effect_set_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x444] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x445] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x446] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x447] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_stop"),
                [0x448] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_near_clip_distance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x449] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_far_clip_distance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x44A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_atmosphere_fog")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x44B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "atmosphere_fog_override_fade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x44C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "motion_blur")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x44D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_weather")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x44E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_patchy_fog")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x450] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_environment_map_attenuation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x451] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_environment_map_bitmap")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Bitmap),
                },
                [0x452] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_bitmap"),
                [0x453] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_environment_map_tint")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x454] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_tint"),
                [0x455] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_layer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x456] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_has_skills"),
                [0x457] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_has_mad_secret_skills")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x458] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "controller_invert_look"),
                [0x459] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "controller_look_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x45A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "controller_set_look_invert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x45B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "controller_get_look_invert"),
                [0x45C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "controller_unlock_solo_levels")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x4AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_clear"),
                [0x4B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_show_up_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_finish_up_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_show")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_finish")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4D5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_delegate_host")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4D6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_delegate_leader")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_map_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x4D9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_campaign_difficulty")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x4DA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_player_color")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_set_mission_segment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x4F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_display_mission_segment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x4F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_insert"),
                [0x4F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_upload"),
                [0x4F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_playback")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x4F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_enable"),
                [0x526] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "bug_now")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x527] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "bug_now_lite")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x528] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "bug_now_auto")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x529] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "object_list_children")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x52A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "voice_set_outgoing_channel_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x52B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "voice_set_voice_repeater_peer_index")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x532] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x533] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_start_smooth")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x534] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x535] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_restart")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x536] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "interpolator_is_active")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x537] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "interpolator_is_finished")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x538] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_set_current_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x539] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_current_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x53A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_start_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x53B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_final_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x53C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_current_phase")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x53D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_current_time_fraction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x53E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_start_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x53F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_final_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x540] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x541] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_fraction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x542] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x543] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_delta")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x544] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "interpolator_stop_all"),
                [0x545] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "interpolator_restart_all"),
                [0x546] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "interpolator_flip"),
                [0x548] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "animation_cache_stats_reset"),
                [0x549] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_clone_players_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x54A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_move_attached_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x54B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_enable_ghost_effects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x54C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "set_global_sound_environment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x54D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "reset_dsp_image"),
                [0x54E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_cinematic_skip"),
                [0x54F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_outro_start"),
                [0x550] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_enable_ambience_details")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x55E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_suppress_ambience_update_on_revert"),
                [0x562] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_exposure_fade_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x563] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_exposure")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x564] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_autoexposure_instant")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x566] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_depth_of_field_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x56B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_disable?"),
                [0x56C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_enable?"),
                [0x56D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "predict_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x56E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "game_team_get_players?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x56F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "game_team_get_player_count?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x573] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "mp_ai_allegiance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x574] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "mp_allegiance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x59A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "predict_bink_movie")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x5A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "game_coop_player_count"),
                [0x5A5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "add_recycling_volume")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x5AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "game_tick_get"),
                [0x5C2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_always_active")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x5C8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_persistent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x5ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_zone_activate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x5EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_zone_deactivate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x5F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_limit_lipsync_to_mouth_only")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x5F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "prepare_to_switch_to_zone_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ZoneSet),
                },
                [0x5FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_zone_activate_for_debugging")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x5FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_play_random_ping")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x5FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_fade_out_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x5FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_fade_in_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x5FE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_control_fade_out_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x5FF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_control_fade_in_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x600] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_lock_gaze")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x601] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_unlock_gaze")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x602] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_scale_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x609] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_custom_animation_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x60A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x60B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "film_manager_set_reproduction_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x60F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "biped_force_ground_fitting_on")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x610] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_chud_objective")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x611] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_cinematic_title")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x612] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "terminal_is_being_read"),
                [0x613] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "terminal_was_accessed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x614] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "terminal_was_completed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x618] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_award_level_complete_achievements"),
                [0x61A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_safe_to_respawn")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x61B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cortana_effect_kill"),
                [0x61E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_cortana_effect_cinematic"),
                [0x61F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_migrate_infanty")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x620] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_cinematic_motion_blur")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x621] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dont_do_avoidance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x622] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_clean_up")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x623] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_erase_inactive")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x624] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_survival_cleanup")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x625] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "stop_bink_movie"),
                [0x626] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "play_credits_unskippable"),
                [0x62B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_debug_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x62C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "cinematic_scripting_get_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x62E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "gp_integer_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x62F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "gp_integer_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x630] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "gp_boolean_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x631] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "gp_boolean_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x637] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_screen_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x638] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_stop_screen_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x639] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_level_prepare")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x63A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "prepare_game_level")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x63B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_activate_beacon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x63D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_activate_marker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x63E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_activate_marker_named")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x63F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "pda_beacon_is_selected")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x640] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_inside_active_beacon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x64B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_set_user_input_constraints")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x64C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "skull_primary_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PrimarySkull),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x64D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "skull_secondary_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.SecondarySkull),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x651] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_respawn_dead_players"),
                [0x652] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "survival_mode_lives_get"),
                [0x653] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_lives_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x654] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "survival_mode_set_get"),
                [0x655] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_set_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x656] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "survival_mode_round_get"),
                [0x657] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_round_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x658] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "survival_mode_wave_get"),
                [0x659] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_wave_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x65A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "survival_mode_set_multiplier_get"),
                [0x65B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_set_multiplier_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x65C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "survival_mode_round_multiplier_get?"),
                [0x65D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_round_multiplier_set?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x65F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_set_rounds_per_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x660] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_set_waves_per_round")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x662] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_event_new")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x663] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_begin_new_set"),
                [0x664] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_begin_new_round"),
                [0x665] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_begin_new_wave"),
                [0x666] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_award_hero_medal"),
                [0x667] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "campaign_metagame_get_player_score")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x668] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_add_footprint")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x66A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "pda_is_active_deterministic")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x66B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_set_pda_enabled")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x66C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_set_nav_enabled")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x66D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_set_fourth_wall_enabled")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x66E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_set_objectives_enabled")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x66F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_force_pda")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x670] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_close_pda")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x671] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_set_footprint_dead_zone")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x674] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_play_arg_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x675] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_stop_arg_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x676] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_set_look_training_hack")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x677] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_set_active_pda_definition")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x678] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_arg_has_been_touched_by_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x67B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_input_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x67F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_input_enable_a")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x680] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_input_enable_dismiss")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x681] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_input_enable_x")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x682] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_input_enable_y")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x683] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_input_enable_dpad")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x684] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pda_input_enable_analog_sticks")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x68A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "achievement_post_chud_progression")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x68B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_vision_mode_render_default")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x68C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_navpoint")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x690] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_confirm_message")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x691] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_confirm_cancel_message")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x692] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_confirm_y_button")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x693] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "player_get_kills_by_type")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x694] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_flashlight_on")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x695] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "clear_command_buffer_cache_from_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x696] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_resume")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x697] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_bonus_round_set_target_score")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },


                [0x333] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x333"),
                [0x38A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x38A"),
                [0x3A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x3A2"),
                [0x3B5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x3B5"),
                [0x3BC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x3BC"),
                [0x42A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x42A"),
                [0x4AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x4AE"),
                [0x4EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x4EC"),
                [0x556] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x556"),
                [0x55A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x55A"),
                [0x55B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x55B"),
                [0x55C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x55C"),
                [0x5E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x5E5"),
                [0x5E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x5E6"),
                [0x5F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x5F4"),
                [0x5F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x5F5"),
                [0x608] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x608"),
                [0x61D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x61D"),
                [0x628] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x628"),
                [0x629] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "0x629"),
            },
            [CacheVersion.HaloOnline106708] = new Dictionary<int, ScriptInfo>
            {
                [0x000] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "begin"),        
                [0x001] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "begin_random"), 
                [0x002] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "if"),           
                [0x003] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "cond"),         
                [0x004] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Passthrough, "set"),          
                [0x005] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "and"),              
                [0x006] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "or"),               
                [0x007] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "+"),                   
                [0x008] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "-"),                   
                [0x009] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "*"),
                [0x00A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "/"),
                [0x00B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "inc"),                 
                [0x00C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "min"),                 
                [0x00D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "max"),
                [0x00E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "="),
                [0x00F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "!="),                
                [0x010] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, ">"),                
                [0x011] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "<"),               
                [0x012] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, ">="),               
                [0x013] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "<="),
                [0x014] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sleep"),               
                [0x015] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sleep_forever"),       
                [0x016] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "sleep_until"),      
                [0x017] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "wake"),                
                [0x018] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "inspect"),             
                [0x019] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "unit"),                
                [0x01A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "evaluate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Script),
                },         
                [0x01B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "not")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },           
                [0x01C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "pin")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },              
                [0x01D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "print")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x01E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference1")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x01F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference2")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x020] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference3")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x021] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference4")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x022] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference5")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x023] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference6")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x024] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference7")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x025] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "GlobalsReference8")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x026] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_active_scripts"),
                [0x027] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "get_executing_running_thread"),
                [0x028] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_thread")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x029] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "script_started")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x02A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "script_finished")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x02B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "players"),
                [0x02C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "player_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x02D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_volume_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "kill_volume_disable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "volume_teleport_players_not_inside")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x030] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x031] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x032] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_objects_all")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x033] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_players")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x034] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "volume_test_players_all")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x035] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "volume_return_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                },
                [0x036] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "volume_return_objects_by_type")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x037] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "zone_set_trigger_volume_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x038] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "list_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x039] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "list_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x03A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "list_count_not_dead")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x03B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x03C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_random")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x03D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_at_ai_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x03E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_on_object_marker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x03F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "effect_new_on_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x040] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_new")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x041] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_object_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x042] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_objects_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x043] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x044] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x045] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_players")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Damage),
                },
                [0x046] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "soft_ceiling_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x047] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x048] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_clone")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x049] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_anew")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x04A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_if_necessary")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x04B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x04C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_clone_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x04D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_anew_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x04E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_folder")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Folder),
                },
                [0x04F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_create_folder_anew")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Folder),
                },
                [0x050] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x051] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_containing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x052] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_all"),
                [0x053] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_type_mask")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x054] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_delete_by_definition")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x055] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_destroy_folder")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Folder),
                },
                [0x056] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_hide")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x057] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shadowless")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x058] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "object_buckling_magnitude_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x059] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_function_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x05A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_function_variable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x05B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_clear_function_variable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x05C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_clear_all_function_variables")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x05D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_dynamic_simulation_disable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x05E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_phantom_power")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x05F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_wake_physics")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x060] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_ranged_attack_inhibited")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x061] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_melee_attack_inhibited")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x062] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_dump_memory"),
                [0x063] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "object_get_health")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x064] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "object_get_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x065] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x066] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_physics")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x067] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "object_get_parent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x068] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_attach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x069] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "object_at_marker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x06A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_detach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x06B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x06C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x06D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown1")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x06E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_collision_damage_armor_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x06F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x070] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_deleted_when_deactivated")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x071] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "object_model_target_destroyed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x072] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "object_model_targets_destroyed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x073] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_damage_damage_section")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x074] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cannot_die")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x075] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "object_vitality_pinned")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x076] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "garbage_collect_now"),
                [0x077] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "garbage_collect_unsafe"),
                [0x078] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "garbage_collect_multiplayer"),
                [0x079] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cannot_take_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x07A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_can_take_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x07B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cinematic_lod")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x07C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cinematic_collision")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x07D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_cinematic_visibility")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x07E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x07F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_predict_high")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x080] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objects_predict_low")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x081] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_type_predict_high")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x082] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_type_predict_low")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x083] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_type_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x084] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x085] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_teleport_to_ai_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x086] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_facing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x087] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x088] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_normalized")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x089] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_stun")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x08A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_shield_stun_infinite")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x08B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_permutation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x08C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_variant")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x08D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_region_state")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ModelState),
                },
                [0x08E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "objects_can_see_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x08F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "objects_can_see_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x090] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "objects_distance_to_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x091] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "objects_distance_to_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x092] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "map_info"),
                [0x093] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "position_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x094] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "shader_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Shader),
                },
                [0x095] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "bitmap_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Bitmap),
                },
                [0x096] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "script_recompile"),
                [0x097] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "script_doc"),
                [0x098] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "help")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x099] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "game_engine_objects"),
                [0x09A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "random_range")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x09B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "real_random_range")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x09C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_constants_reset"),
                [0x09D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_set_gravity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x09E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_set_velocity_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x09F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "physics_disable_character_ground_adhesion_forces")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_debug_start"),
                [0x0A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_dump_world")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_dump_world_close_movie"),
                [0x0A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_profile_start"),
                [0x0A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_profile_stop"),
                [0x0A5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_profile_range")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0A6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "havok_reset_allocated_state"),
                [0x0A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "breakable_surfaces_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0A8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "breakable_surfaces_reset"),
                [0x0A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "recording_play")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "recording_play_and_delete")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "recording_play_and_hover")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "recording_kill")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "recording_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "render_lights")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "print_light_state"),
                [0x0B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_lights_enable_cinematic_shadow")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_object_marker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_target")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0B4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_attach_to_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0B5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_target_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0B6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_position_world_offset")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0B7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_on"),
                [0x0B8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_off"),
                [0x0B9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_aspect_ratio")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0BA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_resolution")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0BB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_render_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0BD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_fov")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0BE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_set_fov_frame_target")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0BF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "texture_camera_enable_dynamic_lights")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_on")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0C1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_off")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0C2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_set_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0C3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_set_target")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0C4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_attach_to_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0C5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_target_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0C6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_structure")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0C7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_highlight_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0C8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_clear_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0C9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_spin_around")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0CA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_from_player_view")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0CB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_camera_window")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0CD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_structure_cluster")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0CE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_cluster_visibility")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0CF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_cluster_fog")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0D0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_fog_plane")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_fog_plane_infinite_extent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_fog_zone")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x0D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_all_fog_planes")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_all_cluster_errors")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0D5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_line_opacity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0D6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_text_opacity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_opacity")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0D8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_non_occluded_fog_planes")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0D9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_lightmaps_use_pervertex"),
                [0x0DA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_lightmaps_use_reset"),
                [0x0DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_lightmaps_sample_enable"),
                [0x0DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_structure_lightmaps_sample_disable"),
                [0x0DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_query_object_bitmaps")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x0DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_query_bsp_resources")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_query_all_object_resources"),
                [0x0E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_object_list"),
                [0x0E1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_debug_text_using_simple_font")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0E2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_postprocess_color_tweaking_reset"),
                [0x0E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_relative_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0E7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_relative_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_idle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                },
                [0x0EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "scenery_get_animation_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                },
                [0x0EB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_can_blink")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_active_camo")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x0ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_open")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_close")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0EF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_kill")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_kill_silent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_is_emitting")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "unit_get_custom_animation_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_stop_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_relative_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x0F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "custom_animation_list")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_custom_animation_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_custom_animation_relative_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x0FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_is_playing_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x0FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_custom_animations_hold_on_last_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_custom_animations_prevent_lipsync_head_movement")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x0FE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "preferred_animation_list_add")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x0FF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "preferred_animation_list_clear"),
                [0x100] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_actively_controlled")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x101] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "unit_get_team_index")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x102] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_aim_without_turning")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x103] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_enterable_by_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x105] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_get_enterable_by_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x106] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_only_takes_damage_from_players_team")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x107] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_enter_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x108] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_falling_damage_disable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x109] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_in_vehicle_type")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x10A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "object_get_turret_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x10B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "object_get_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x10C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_board_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x10D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_emotion_by_?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x10E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_emotion_by_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x10F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_enable_eye_tracking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x110] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_integrated_flashlight")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x111] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown3")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnyTag),
                },
                [0x112] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_enable_vision_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x113] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_in_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x114] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_test_seat_list")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x115] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_test_seat_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x116] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_test_seat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x117] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_prefer_tight_camera_track")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x118] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x119] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x11A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_maximum_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x11B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "units_set_maximum_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x11C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_current_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x11D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "units_set_current_vitality")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x11E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "vehicle_load_magic")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x11F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "vehicle_unload")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x120] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_set_animation_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x121] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "magic_melee_attack"),
                [0x122] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "vehicle_riders")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x123] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "vehicle_driver")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x124] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "vehicle_gunner")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x125] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "unit_get_health")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x126] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "unit_get_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x127] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "unit_get_total_grenade_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x128] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_has_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x129] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_has_weapon_readied")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x12A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_has_any_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x12B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_has_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x12C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_lower_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x12D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_raise_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x12E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_drop_support_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x12F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_spew_action")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x130] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_force_reload")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x131] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown4"), //  animation_stats_dump
                [0x132] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_animation_forced_seat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x133] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_doesnt_drop_items")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                },
                [0x134] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_impervious")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x135] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_suspended")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x136] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_add_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StartingProfile),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x137] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "weapon_hold_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Weapon),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x138] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "weapon_enable_warthog_chaingun_light")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x139] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_set_never_appears_locked")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x13A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_set_power")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x13B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "device_get_power")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                },
                [0x13C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_set_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x13D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "device_get_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                },
                [0x13E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_set_position_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x13F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "device_group_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                },
                [0x140] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_group_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x141] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_group_set_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x142] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_one_sided_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x143] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_ignore_player_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x144] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_operates_automatically_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x145] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_closes_automatically_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x146] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_group_change_only_once_more_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x147] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_set_position_track")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x148] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "device_set_overlay_track")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x149] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_animate_position")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x14A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "device_animate_overlay")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Device),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x14B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_all_powerups"),
                [0x14C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_all_weapons"),
                [0x14D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_all_vehicles"),
                [0x14E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_teleport_to_camera"),
                [0x14F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_active_camouflage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x150] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheat_active_camouflage_by_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x151] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cheats_load"),
                [0x152] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown5")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnyTag),
                },
                [0x153] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "drop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x155] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x156] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_enabled"),
                [0x157] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x158] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x159] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_player_dialogue_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x15A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "Unknown7")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x15B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_fast_and_dumb")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x15C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_log_reset"),
                [0x15D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_log_dump")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x15E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "ai_get_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x15F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Unit, "ai_get_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x160] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_get_squad")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x161] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_get_turret_ai")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x162] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.PointReference, "ai_random_smart_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x163] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.PointReference, "ai_nearest_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x164] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "ai_get_point_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x165] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.PointReference, "ai_point_set_get_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x166] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x167] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x168] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_place_in_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x169] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_cannot_die")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x16A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_vitality_pinned")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x16B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_index_from_spawn_formation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x16C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_resurrect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x16D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_kill")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x16E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_kill_silent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x16F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_erase")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x170] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_erase_all"),
                [0x171] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_disposable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x172] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_select")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x173] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_deselect"),
                [0x174] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_deaf")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x175] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_blind")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x176] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_weapon_up")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x177] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_flood_disperse")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x178] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_magically_see")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x179] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_magically_see_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x17A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_active_camo")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x17B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_suppress_combat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x17D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_grunt_kamikaze")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x17E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_migrate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x17F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allegiance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x180] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allegiance_remove")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x181] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allegiance_break")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x182] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_braindead")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x183] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_braindead_by_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x184] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_disregard")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x185] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_prefer_target")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x186] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_prefer_target_team")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x187] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_prefer_target_ai")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x188] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x189] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x18A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_teleport_to_starting_location_if_outside_bsp")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x18B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_teleport_to_starting_location_if_outside_bsp?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x18C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x18D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_bring_forward")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x190] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "biped_morph")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x191] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_renew")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x192] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_force_active")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x193] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_force_active_by_unit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x194] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_playfight")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x195] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_reconnect"),
                [0x196] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_berserk")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x197] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_set_team")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x198] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_allow_dormant")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x199] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_is_attacking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_fighting_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_living_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_living_fraction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_in_vehicle_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_body_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x19F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_strength")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_swarm_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_nonswarm_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "ai_actors")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_allegiance_broken")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                },
                [0x1A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_spawn_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1A5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "object_get_ai")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x1B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_set_task")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_set_objective")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_task_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_set_task_condition")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1B6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_task_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1B7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_reset_objective")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1B8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_squad_patrol_objective_disallow")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1BB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_activity_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1BC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_activity_abort")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1BD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "ai_vehicle_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1BE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_starting_location")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1BF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_spawn_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_vehicle_get_from_starting_location")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1C1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_squad")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1C2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve_seat")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1C3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1C4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "ai_player_get_vehicle_squad")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x1C7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_in_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x1C8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_in_vehicle?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x1C9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_player_any_needs_vehicle"),
                [0x1CA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1CB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x1CC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1CD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x1CE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_enter_squad_vehicles")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1CF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1D0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vehicle_overturned")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x1D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_flip")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x1D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1D5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1D6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_create")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "flock_delete")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_verify_tags"),
                [0x1DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "ai_wall_lean")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x1DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x1DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line_at_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x1DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line_on_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x1E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "ai_play_line_on_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x1E1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1E2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_time_pause")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_points")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_primary_skull")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PrimarySkull),
                },
                [0x1E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_secondary_skull")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.SecondarySkull),
                },
                [0x1E7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "campaign_metagame_award_event")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_metagame_enabled"),
                [0x1E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_survival_enabled"),
                [0x1EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_easy"),
                [0x1EB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_normal"),
                [0x1EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_heroic"),
                [0x1ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "campaign_is_finished_legendary"),
                [0x1EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_run_command_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1EF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_queue_command_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stack_command_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x1FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Ai, "vs_role")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x1FE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x1FF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x200] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x201] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x202] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x203] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x204] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x205] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x206] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x207] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x208] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x20B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_set_cleanup_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Script),
                },
                [0x20C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_release")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x20D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_release_all"),
                [0x20E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "cs_command_script_running")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x20F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "cs_command_script_queued")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiCommandScript),
                },
                [0x210] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "cs_number_queued")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x213] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x214] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom_movement")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x215] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom_?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x216] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "vs_running_atom_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x217] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x218] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x219] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x21C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x21D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x21F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x220] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x221] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x222] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x223] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x224] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x225] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x226] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x227] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x228] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x229] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x22A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x22B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x22C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_and_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x22D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_and_posture")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x22E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_and_posture")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x22F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_nearest")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x230] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_nearest")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x231] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_in_direction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x232] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_in_direction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x233] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_towards?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x234] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_towards?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x235] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_towards")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x236] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_towards")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x237] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_move_towards_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x238] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_move_towards_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x239] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_swarm_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_swarm_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_swarm_from")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_swarm_from")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_pause")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_pause")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x23F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_grenade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x240] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_grenade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x241] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x242] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_equipment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x243] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_jump")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x244] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_jump")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x245] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_jump_to_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x246] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_jump_to_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x247] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vocalize")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x248] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vocalize")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x249] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x24A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x24B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x24C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x24D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x24E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x24F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_action")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x250] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_action")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x251] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_action_at_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x252] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_action_at_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x253] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_action_at_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x254] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_action_at_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x255] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x256] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x257] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x258] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x259] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x25A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x25B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x25C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x25D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x25E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x25F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x260] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x261] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_play_line")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x262] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_play_line")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AiLine),
                },
                [0x265] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_deploy_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x266] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_deploy_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x267] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_approach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x268] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_approach")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x269] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_approach_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x26A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_approach_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x26B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x26C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x26D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x26E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x26F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_set_style")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Style),
                },
                [0x270] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_set_style")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Style),
                },
                [0x271] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_force_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x272] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_force_combat_status")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x273] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_targeting")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x274] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_targeting")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x275] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_looking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x276] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_looking")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x277] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_moving")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x278] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_moving")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x279] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_suppress_activity_termination")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_suppress_activity_termination")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x27F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_look")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x280] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_look")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x281] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_look_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x282] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_look_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x283] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_look_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x284] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_look_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x285] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_aim")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x286] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_aim")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x287] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_aim_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x288] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_aim_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x289] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_aim_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x28A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_aim_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x28B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x28C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_face")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x28D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_face_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x28E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_face_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x28F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_face_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x290] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_face_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x291] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x292] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x293] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x294] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x295] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x296] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x297] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x298] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x299] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_lower_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x29A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_lower_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x29B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vehicle_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vehicle_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x29F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_vehicle_boost")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_vehicle_boost")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_turn_sharpness?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_turn_sharpness?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_ignore_obstacles")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_ignore_obstacles")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_approach_stop"),
                [0x2AA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_approach_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x2AB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_movement_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2AC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_movement_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_walk")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_walk")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_posture_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2B4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_posture_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2B5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_posture_exit"),
                [0x2B6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_posture_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x2B7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stow"),
                [0x2B8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_stow")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x2B9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_draw"),
                [0x2BA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_draw")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x2BB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x2BC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_teleport")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                },
                [0x2BD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stop_custom_animation"),
                [0x2BE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_stop_custom_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x2BF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cs_stop_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x2C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vs_stop_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x2C7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_control")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2C8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2C9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x2CA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x2CB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x2CC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_with_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2CD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2CE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_?boolean")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2CF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_?boolean_real")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2D0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_predict_resources_at_frame")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x2D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_predict_resources_at_point")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_first_person")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x2D3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_cinematic"),
                [0x2D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_cinematic_scene")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x2D7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "camera_time"),
                [0x2D8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_field_of_view")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2DA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_camera_set_easing_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2DB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_print")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_pan")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_pan")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x2DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_save"),
                [0x2DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_load"),
                [0x2E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_save_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2E1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_camera_load_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x2E2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "director_debug_camera")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get"),
                [0x2E4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get_real"),
                [0x2E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "game_insertion_point_get"),
                [0x2E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_insertion_point_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x2E7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pvs_set_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x2E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pvs_set_camera")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "pvs_clear"),
                [0x2EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_enable_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_disable_movement")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_flashlight_on"),
                [0x2F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_active_camouflage_on"),
                [0x2F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_camera_control")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x2F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_action_test_reset"),
				[0x2F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_jump"),
                [0x2F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_primary_trigger"),
                [0x2F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_grenade_trigger"),
                [0x2F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_zoom"),
                [0x2F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_rotate_weapons"),
                [0x2FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_rotate_grenades"),
                [0x2FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_melee"),
                [0x2FC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_action"),
                [0x2FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_accept"),
                [0x2FE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_cancel"),
                [0x2FF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_up"),
                [0x300] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_down"),
                [0x301] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_left"),
                [0x302] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_right"),
                [0x303] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_all_directions"),
                [0x304] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_move_relative_all_directions"),
                [0x305] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_back"),
                [0x306] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_dpad_left"),
                [0x307] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_dpad_right"),
                [0x308] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_dpad_up"),
                [0x309] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_dpad_down"),
                [0x30A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_x"),
                [0x30B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_y"),
                [0x30C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_left_shoulder"),
                [0x30D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_right_shoulder"),
                [0x30F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_action_test_reset")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x310] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_primary_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x311] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_vision_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x312] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_rotate_weapons")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x313] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_rotate_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x314] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_melee")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x315] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_action")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x316] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_accept")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x317] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_cancel")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x318] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_up")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x319] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_down")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_left")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_right")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_all_directions")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_move_relative_all_directions")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_back")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x31F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_left")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x320] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_right")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x321] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_up")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x322] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_down")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x323] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_x")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x324] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_y")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x325] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_left_shoulder")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x326] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_action_test_right_shoulder")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x327] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player0_looking_up"),
                [0x328] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player0_looking_down"),
                [0x329] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player0_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x32A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player1_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x32B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player2_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x32C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player3_set_pitch")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x32D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_lookstick_forward"),
                [0x32E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_action_test_lookstick_backward"),
                [0x32F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_teleport_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x330] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "map_reset"),
                [0x333] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "switch_zone_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ZoneSet),
                },
                [0x334] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "current_zone_set"),
                [0x335] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "current_zone_set_fully_active"),
                [0x337] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "crash")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x338] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "version"),
                [0x339] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "status"),
                [0x33A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "record_movie")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x33B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "record_movie_distributed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x33C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "screenshot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x33E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "screenshot_big")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x33F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "screenshot_big_jittered")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x343] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "UnknownString")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x344] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "main_menu"),
                [0x345] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "main_halt"),
                [0x34A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "map_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x34C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_multiplayer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x34D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_splitscreen")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x34E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_difficulty")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.GameDifficulty),
                },
                [0x354] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x357] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_rate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x35B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_memory"),
                [0x35C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_memory_by_file"),
                [0x35D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_memory_for_file")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x35E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_tags"),
                [0x35F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "tags_verify_all"),
                [0x36D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_control_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x36E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "damage_control_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x370] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dialogue_break_on_vocalization")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x371] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "fade_in")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x372] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "fade_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x373] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_start"),
                [0x374] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_stop"),
                [0x375] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_skip_start_internal"),
                [0x376] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_skip_stop_internal"),
                [0x377] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_show_letterbox")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x378] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_show_letterbox_immediate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x379] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_title")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x37A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_title_delayed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x37B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_suppress_bsp_object_creation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x37C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_subtitle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x37D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicDefinition),
                },
                [0x37E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_shot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x380] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_early_exit")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x381] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "cinematic_get_early_exit"),
                [0x384] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_object_create_cinematic_anchor")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x385] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_object_destroy")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x386] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_destroy"),
                [0x387] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_clips_initialize_for_shot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x388] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_clips_destroy"),
                [0x389] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lights_initialize_for_shot")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x38A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lights_destroy"),
                [0x38B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lights_destroy_shot"),
                [0x38C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_light_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CinematicLightprobe),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x38E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lighting_rebuild_all"),
                [0x391] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "cinematic_object_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x393] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "camera_set_briefing")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x394] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.CinematicDefinition, "cinematic_tag_reference_get_cinematic")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x395] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition, "cinematic_tag_reference_get_scene")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x396] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Effect, "cinematic_tag_reference_get_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x397] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x398] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound2")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x399] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.LoopingSound, "cinematic_tag_reference_get_looping_sound")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x39A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph, "cinematic_tag_reference_get_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x39B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "cinematic_tag_reference_get_?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x39C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_fade_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x39F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_create_object?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3A0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3A1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x3A2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_music")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3A3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_dialogue")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x3A4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_stop_music")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3A6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_object_no_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3A8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object_no_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3A9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_play_cortana_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3AA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "attract_mode_start"),
                [0x3AB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "attract_mode_set_seconds")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3AC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_level_advance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_won"),
                [0x3AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_lost")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_revert"),
                [0x3B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_is_cooperative"),
                [0x3B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_is_playtest"),
                [0x3B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_can_use_flashlights")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_and_quit"),
                [0x3B4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_unsafe"),
                [0x3B5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_insertion_point_unlock")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x3B6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_insertion_point_lock?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x3BC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "achievement_grant_to_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3BD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "achievement_grant_to_all_players")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3DC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load"),
                [0x3DD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x3DE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_save"),
                [0x3DF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_save_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x3E0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load_game"),
                [0x3E1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_load_game_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x3E2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_regular_upload_to_debug_server")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x3E3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "core_set_upload_option")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x3E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_safe_to_save"),
                [0x3E7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_safe_to_speak"),
                [0x3E8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_all_quiet"),
                [0x3E9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save"),
                [0x3EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_cancel"),
                [0x3EB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_no_timeout"),
                [0x3EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_immediate"),
                [0x3ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_saving"),
                [0x3EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "game_reverted"),
                [0x3F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_set_tag_parameter_unsafe")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_trigger")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x3F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_cinematic")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_with_subtitle")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x3F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "sound_impulse_language_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_start_3d")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3FB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_impulse_mark_as_outro")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Sound),
                },
                [0x3FD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_predict")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                },
                [0x3FE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x3FF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                },
                [0x400] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_stop_immediately")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                },
                [0x401] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_set_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x402] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_set_alternate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x403] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_loop_spam"),
                [0x404] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_show_channel")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x405] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_debug_sound_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x406] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_sounds_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x407] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_set_gain")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x408] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_set_gain_db")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x409] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_class_enable_ducker")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x40A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "debug_sound_environment_parameter")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x40B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_set_global_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x40C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_set_global_effect_scale")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x40D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_auto_turret")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x40E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_hover")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x40F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "vehicle_count_bipeds_killed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Vehicle),
                },
                [0x410] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "biped_ragdoll")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x412] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_show_training_text")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x413] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_set_training_text")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x414] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_enable_training")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x415] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_flashlight"),
                [0x416] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_crouch"),
                [0x417] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_stealth"),
                [0x418] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_?"),
                [0x419] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_training_activate_jump"),
                [0x41B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_activate_team_nav_point_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x41C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "hud_deactivate_team_nav_point_flag")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x41D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_cortana_suck")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x420] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "play_cortana_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x423] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_crosshair")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x424] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_shield")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x425] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x426] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_messages")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x427] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_motion_sensor")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x428] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_spike_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x429] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_fire_grenades")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x42A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_compass")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x42B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_stamina")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x42C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_energy_meters")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x42D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_consumables")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x42E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_cinematic_fade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x42F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_bonus_round_show_timer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x430] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_bonus_round_start_timer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x431] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_bonus_round_set_timer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x432] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cls"),
                [0x433] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_overflow_suppression")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x434] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_show")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x435] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_hide")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x436] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_show_all"),
                [0x437] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_hide_all"),
                [0x438] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "error_geometry_list"),
                [0x439] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_translation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_rotation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_rumble")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_translation_for_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x43F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_rotation_for_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x440] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_set_max_rumble_for_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x441] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_start_for_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x442] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_effect_stop_for_player")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x443] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "time_code_show")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x444] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "time_code_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x445] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "time_code_reset"),
                [0x446] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "script_screen_effect_set_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x447] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x448] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x449] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x44A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_screen_effect_stop"),
                [0x44B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_near_clip_distance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x44C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_far_clip_distance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x44D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_atmosphere_fog")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x44E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "atmosphere_fog_override_fade")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x44F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "motion_blur")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x450] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_weather")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x451] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_patchy_fog")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x452] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_environment_map_attenuation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x453] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_environment_map_bitmap")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Bitmap),
                },
                [0x454] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_bitmap"),
                [0x455] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_environment_map_tint")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x456] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_tint"),
                [0x457] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_layer")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x458] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "player_has_skills"),
                [0x459] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_has_mad_secret_skills")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x45A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "controller_invert_look"),
                [0x45B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "controller_look_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x45C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "controller_set_look_invert")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x45D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "controller_get_look_invert"),
                [0x45E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "controller_unlock_solo_levels")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x4AB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_clear"),
                [0x4AC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_show_up_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4AD] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_finish_up_to")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4AE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_show")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4AF] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_finish")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4B0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_unavailable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long)
                },
                [0x4B1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_secondary_show")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long)
                },
                [0x4B2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_secondary_finish")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long)
                },
                [0x4B3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "objectives_secondary_unavailable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long)
                },
                [0x4D1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_delegate_?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x4D2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_map_name")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x4D4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "net_campaign_difficulty")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x4EC] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_set_mission_segment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x4ED] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_display_mission_segment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x4EE] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_insert"),
                [0x4F0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_upload"),
                [0x4F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_playback")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x4F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "data_mine_enable"),
                [0x521] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "object_list_children")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x522] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "voice_set_outgoing_channel_count")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x523] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "voice_set_voice_repeater_peer_index")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x52A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_start")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x52B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_start_smooth")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x52C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_stop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x52D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_restart")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x52E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "interpolator_is_active")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x52F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "interpolator_is_finished")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x530] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "interpolator_set_current_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x531] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_current_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x532] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_start_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x533] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_final_value")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x534] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_current_phase")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x535] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_current_time_fraction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x536] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_start_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x537] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_get_final_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x538] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x539] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_fraction")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x53A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x53B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_delta")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x53C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "interpolator_stop_all"),
                [0x53D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "interpolator_restart_all"),
                [0x53E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "interpolator_flip"),
                [0x540] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "animation_cache_stats_reset"),
                [0x541] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_clone_players_weapon")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x542] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_move_attached_objects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x543] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "vehicle_enable_ghost_effects")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x544] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "set_global_sound_environment")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x545] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "reset_dsp_image"),
                [0x546] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_save_cinematic_skip"),
                [0x547] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_outro_start"),
                [0x548] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_enable_ambience_details")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x556] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_suppress_ambience_update_on_revert"),
                [0x55A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_exposure_fade_out")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x55B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_exposure")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x55C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_autoexposure_instant")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x55E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_depth_of_field_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x563] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_disable?"),
                [0x564] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_enable?"),
                [0x565] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "predict_animation")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x566] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.ObjectList, "game_team_get_players?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x567] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "game_team_get_player_count?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x56B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "mp_ai_allegiance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Team),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x56C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "mp_allegiance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.MpTeam),
                },
                [0x576] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "mp_object_create_anew")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ObjectName),
                },
                [0x577] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "mp_object_destroy")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x594] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "predict_bink_movie")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.String),
                },
                [0x59A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "game_coop_player_count"),
                [0x59F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "add_recycling_volume")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x5A7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "game_tick_get"),
                [0x5BB] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_always_active")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x5C0] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_persistent")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x5E5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_zone_activate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x5E6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_zone_deactivate")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x5EA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_limit_lipsync_to_mouth_only")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x5F1] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "prepare_to_switch_to_zone_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.ZoneSet),
                },
                [0x5F2] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_zone_activate_for_debugging")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x5F3] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_play_random_ping")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x5F4] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_fade_out_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x5F5] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_fade_in_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x5F6] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_control_fade_out_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x5F7] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_control_fade_in_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x5F8] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_lock_gaze")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x5F9] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_unlock_gaze")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x5FA] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "player_control_scale_all_input")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x601] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_custom_animation_speed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x602] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame_loop")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x603] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "film_manager_set_reproduction_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x607] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "biped_force_ground_fitting_on")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x608] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_chud_objective")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x609] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_cinematic_title")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x60A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "terminal_is_being_read"),
                [0x60B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "terminal_was_accessed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x60C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "terminal_was_completed")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                },
                [0x610] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_award_level_complete_achievements"),
                [0x612] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_safe_to_respawn")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x613] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cortana_effect_kill"),
                [0x616] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_cortana_effect_cinematic"),
                [0x617] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_migrate_infanty")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                },
                [0x618] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "render_cinematic_motion_blur")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x619] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_dont_do_avoidance")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x61A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_clean_up")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x61B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_erase_inactive")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x61C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "ai_survival_cleanup")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x61D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "stop_bink_movie"),
                [0x61E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "play_credits_unskippable"),
                [0x623] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_set_debug_mode")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x624] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Object, "cinematic_scripting_get_object")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x626] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "gp_integer_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x627] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "gp_integer_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x628] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "gp_boolean_get")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x629] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "gp_boolean_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x62F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_start_screen_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x630] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_stop_screen_effect")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x631] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "game_level_prepare")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x632] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "prepare_game_level")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x63D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "cinematic_scripting_set_user_input_constraints")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x63E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "skull_primary_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.PrimarySkull),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x63F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "skull_secondary_enable")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.SecondarySkull),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x643] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_respawn_dead_players"),
                [0x644] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "survival_mode_lives_get"),
                [0x645] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_lives_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x646] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "survival_mode_set_get"),
                [0x647] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_set_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x648] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "survival_mode_round_get"),
                [0x649] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_round_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x64A] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Short, "survival_mode_wave_get"),
                [0x64B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_wave_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x64C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "survival_mode_set_multiplier_get"),
                [0x64D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_set_multiplier_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x64E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Real, "survival_mode_round_multiplier_get?"),
                [0x64F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_round_multiplier_set?")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x651] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_set_rounds_per_set")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x652] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_set_waves_per_round")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Short),
                },
                [0x654] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_event_new")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                },
                [0x655] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_begin_new_set"),
                [0x656] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_begin_new_round"),
                [0x657] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_begin_new_wave"),
                [0x658] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "survival_mode_award_hero_medal"),
                [0x659] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "campaign_metagame_get_player_score")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x665] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "achievement_post_chud_progression")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x666] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "object_set_vision_mode_render_default")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x667] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_show_navpoint")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x66B] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_confirm_message")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x66C] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_confirm_cancel_message")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x66D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "unit_confirm_y_button")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x66E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "player_get_kills_by_type")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x66F] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Boolean, "unit_flashlight_on")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Unit),
                },
                [0x670] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "clear_command_buffer_cache_from_script")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Boolean),
                },
                [0x671] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "sound_looping_resume")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Real),
                },
                [0x672] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "chud_bonus_round_set_target_score")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Long),
                },
                [0x67D] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "ui_get_player_model_id"),
                [0x67E] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Long, "ui_get_music_id"),
                [0x680] = new ScriptInfo(ScriptValueType.Halo3ODSTValue.Void, "biped_set_armor")
                {
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.Object),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ArgumentInfo(ScriptValueType.Halo3ODSTValue.StringId),
                }
            }
        };

        public static Dictionary<CacheVersion, Dictionary<int, string>> ValueTypes { get; } = new Dictionary<CacheVersion, Dictionary<int, string>>
        {
            [CacheVersion.Halo3Retail] = new Dictionary<int, string>
            {
                [0x00] = "unparsed",
                [0x01] = "special_form",
                [0x02] = "function_name",
                [0x03] = "passthrough",
                [0x04] = "void",
                [0x05] = "boolean",
                [0x06] = "real",
                [0x07] = "short",
                [0x08] = "long",
                [0x09] = "string",
                [0x0A] = "script",
                [0x0B] = "string_id",
                [0x0C] = "unit_seat_mapping",
                [0x0D] = "trigger_volume",
                [0x0E] = "cutscene_flag",
                [0x0F] = "cutscene_camera_point",
                [0x10] = "cutscene_title",
                [0x11] = "cutscene_recording",
                [0x12] = "device_group",
                [0x13] = "ai",
                [0x14] = "ai_command_list",
                [0x15] = "ai_command_script",
                [0x16] = "ai_behavior",
                [0x17] = "ai_orders",
                [0x18] = "ai_line",
                [0x19] = "starting_profile",
                [0x1A] = "conversation",
                [0x1B] = "zone_set",
                [0x1C] = "designer_zone",
                [0x1D] = "point_reference",
                [0x1E] = "style",
                [0x1F] = "object_list",
                [0x20] = "folder",
                [0x21] = "sound",
                [0x22] = "effect",
                [0x23] = "damage",
                [0x24] = "looping_sound",
                [0x25] = "animation_graph",
                [0x26] = "damage_effect",
                [0x27] = "object_definition",
                [0x28] = "bitmap",
                [0x29] = "shader",
                [0x2A] = "render_model",
                [0x2B] = "structure_definition",
                [0x2C] = "lightmap_definition",
                [0x2D] = "cinematic_definition",
                [0x2E] = "cinematic_scene_definition",
                [0x2F] = "bink_definition",
                [0x30] = "any_tag",
                [0x31] = "any_tag_not_resolving",
                [0x32] = "game_difficulty",
                [0x33] = "team",
                [0x34] = "mp_team",
                [0x35] = "controller",
                [0x36] = "button_preset",
                [0x37] = "joystick_preset",
                [0x38] = "player_color",
                [0x39] = "player_character_type",
                [0x3A] = "voice_output_setting",
                [0x3B] = "voice_mask",
                [0x3C] = "subtitle_setting",
                [0x3D] = "actor_type",
                [0x3E] = "model_state",
                [0x3F] = "event",
                [0x40] = "character_physics",
                [0x41] = "object",
                [0x42] = "unit",
                [0x43] = "vehicle",
                [0x44] = "weapon",
                [0x45] = "device",
                [0x46] = "scenery",
                [0x47] = "effect_scenery",
                [0x48] = "object_name",
                [0x49] = "unit_name",
                [0x4A] = "vehicle_name",
                [0x4B] = "weapon_name",
                [0x4C] = "device_name",
                [0x4D] = "scenery_name",
                [0x4E] = "effect_scenery_name",
                [0x4F] = "cinematic_lightprobe",
                [0x50] = "animation_budget_reference",
                [0x51] = "looping_sound_budget_reference",
                [0x52] = "sound_budget_reference"
            },
            [CacheVersion.Halo3ODST] = new Dictionary<int, string>
            {
                [0x00] = "unparsed",
                [0x01] = "special_form",
                [0x02] = "function_name",
                [0x03] = "passthrough",
                [0x04] = "void",
                [0x05] = "boolean",
                [0x06] = "real",
                [0x07] = "short",
                [0x08] = "long",
                [0x09] = "string",
                [0x0A] = "script",
                [0x0B] = "string_id",
                [0x0C] = "unit_seat_mapping",
                [0x0D] = "trigger_volume",
                [0x0E] = "cutscene_flag",
                [0x0F] = "cutscene_camera_point",
                [0x10] = "cutscene_title",
                [0x11] = "cutscene_recording",
                [0x12] = "device_group",
                [0x13] = "ai",
                [0x14] = "ai_command_list",
                [0x15] = "ai_command_script",
                [0x16] = "ai_behavior",
                [0x17] = "ai_orders",
                [0x18] = "ai_line",
                [0x19] = "starting_profile",
                [0x1A] = "conversation",
                [0x1B] = "zone_set",
                [0x1C] = "designer_zone",
                [0x1D] = "point_reference",
                [0x1E] = "style",
                [0x1F] = "object_list",
                [0x20] = "folder",
                [0x21] = "sound",
                [0x22] = "effect",
                [0x23] = "damage",
                [0x24] = "looping_sound",
                [0x25] = "animation_graph",
                [0x26] = "damage_effect",
                [0x27] = "object_definition",
                [0x28] = "bitmap",
                [0x29] = "shader",
                [0x2A] = "render_model",
                [0x2B] = "structure_definition",
                [0x2C] = "lightmap_definition",
                [0x2D] = "cinematic_definition",
                [0x2E] = "cinematic_scene_definition",
                [0x2F] = "bink_definition",
                [0x30] = "any_tag",
                [0x31] = "any_tag_not_resolving",
                [0x32] = "game_difficulty",
                [0x33] = "team",
                [0x34] = "mp_team",
                [0x35] = "controller",
                [0x36] = "button_preset",
                [0x37] = "joystick_preset",
                [0x38] = "player_color",
                [0x39] = "player_character_type",
                [0x3A] = "voice_output_setting",
                [0x3B] = "voice_mask",
                [0x3C] = "subtitle_setting",
                [0x3D] = "actor_type",
                [0x3E] = "model_state",
                [0x3F] = "event",
                [0x40] = "character_physics",
                [0x41] = "primary_skull",
                [0x42] = "secondary_skull",
                [0x43] = "object",
                [0x44] = "unit",
                [0x45] = "vehicle",
                [0x46] = "weapon",
                [0x47] = "device",
                [0x48] = "scenery",
                [0x49] = "effect_scenery",
                [0x4A] = "object_name",
                [0x4B] = "unit_name",
                [0x4C] = "vehicle_name",
                [0x4D] = "weapon_name",
                [0x4E] = "device_name",
                [0x4F] = "scenery_name",
                [0x50] = "effect_scenery_name",
                [0x51] = "cinematic_lightprobe",
                [0x52] = "animation_budget_reference",
                [0x53] = "looping_sound_budget_reference",
                [0x54] = "sound_budget_reference"
            },
            [CacheVersion.HaloOnline106708] = new Dictionary<int, string>
            {
                [0x00] = "unparsed",
                [0x01] = "special_form",
                [0x02] = "function_name",
                [0x03] = "passthrough",
                [0x04] = "void",
                [0x05] = "boolean",
                [0x06] = "real",
                [0x07] = "short",
                [0x08] = "long",
                [0x09] = "string",
                [0x0A] = "script",
                [0x0B] = "string_id",
                [0x0C] = "unit_seat_mapping",
                [0x0D] = "trigger_volume",
                [0x0E] = "cutscene_flag",
                [0x0F] = "cutscene_camera_point",
                [0x10] = "cutscene_title",
                [0x11] = "cutscene_recording",
                [0x12] = "device_group",
                [0x13] = "ai",
                [0x14] = "ai_command_list",
                [0x15] = "ai_command_script",
                [0x16] = "ai_behavior",
                [0x17] = "ai_orders",
                [0x18] = "ai_line",
                [0x19] = "starting_profile",
                [0x1A] = "conversation",
                [0x1B] = "zone_set",
                [0x1C] = "designer_zone",
                [0x1D] = "point_reference",
                [0x1E] = "style",
                [0x1F] = "object_list",
                [0x20] = "folder",
                [0x21] = "sound",
                [0x22] = "effect",
                [0x23] = "damage",
                [0x24] = "looping_sound",
                [0x25] = "animation_graph",
                [0x26] = "damage_effect",
                [0x27] = "object_definition",
                [0x28] = "bitmap",
                [0x29] = "shader",
                [0x2A] = "render_model",
                [0x2B] = "structure_definition",
                [0x2C] = "lightmap_definition",
                [0x2D] = "cinematic_definition",
                [0x2E] = "cinematic_scene_definition",
                [0x2F] = "bink_definition",
                [0x30] = "any_tag",
                [0x31] = "any_tag_not_resolving",
                [0x32] = "game_difficulty",
                [0x33] = "team",
                [0x34] = "mp_team",
                [0x35] = "controller",
                [0x36] = "button_preset",
                [0x37] = "joystick_preset",
                [0x38] = "player_character_type",
                [0x39] = "voice_output_setting",
                [0x3A] = "voice_mask",
                [0x3B] = "subtitle_setting",
                [0x3C] = "actor_type",
                [0x3D] = "model_state",
                [0x3E] = "event",
                [0x3F] = "character_physics",
                [0x40] = "primary_skull",
                [0x41] = "secondary_skull",
                [0x42] = "object",
                [0x43] = "unit",
                [0x44] = "vehicle",
                [0x45] = "weapon",
                [0x46] = "device",
                [0x47] = "scenery",
                [0x48] = "effect_scenery",
                [0x49] = "object_name",
                [0x4A] = "unit_name",
                [0x4B] = "vehicle_name",
                [0x4C] = "weapon_name",
                [0x4D] = "device_name",
                [0x4E] = "scenery_name",
                [0x4F] = "effect_scenery_name",
                [0x50] = "cinematic_lightprobe",
                [0x51] = "animation_budget_reference",
                [0x52] = "looping_sound_budget_reference",
                [0x53] = "sound_budget_reference"
            }
        };
    }
}
