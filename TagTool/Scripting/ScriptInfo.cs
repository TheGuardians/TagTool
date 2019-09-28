using TagTool.Cache;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TagTool.Scripting
{
    public struct ScriptInfo : IList<ScriptInfo.ParameterInfo>
    {
        public HsType.Halo3ODSTValue Type;
        public string Name;
        public List<ParameterInfo> Parameters;

        public int Count => Parameters.Count;
        public bool IsReadOnly => false;

        public ScriptInfo(HsType.Halo3ODSTValue type) : this(type, "") { }

        public ScriptInfo(HsType.Halo3ODSTValue type, string name) : this(type, name, new List<ParameterInfo>()) { }

        public ScriptInfo(HsType.Halo3ODSTValue type, string name, IEnumerable<ParameterInfo> arguments)
        {
            Type = type;
            Name = name;
            Parameters = arguments.ToList();
        }

        public ParameterInfo this[int index] { get => Parameters[index]; set => Parameters[index] = value; }

        public int IndexOf(ParameterInfo item) => Parameters.IndexOf(item);
        public void Insert(int index, ParameterInfo item) => Parameters.Insert(index, item);
        public void RemoveAt(int index) => Parameters.RemoveAt(index);
        public void Add(ParameterInfo item) => Parameters.Add(item);
        public void Clear() => Parameters.Clear();
        public bool Contains(ParameterInfo item) => Parameters.Contains(item);
        public void CopyTo(ParameterInfo[] array, int arrayIndex) => Parameters.CopyTo(array, arrayIndex);
        public bool Remove(ParameterInfo item) => Parameters.Remove(item);
        public IEnumerator<ParameterInfo> GetEnumerator() => Parameters.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Parameters.GetEnumerator();

        public struct ParameterInfo
        {
            public HsType.Halo3ODSTValue Type;
            public string Name;

            public ParameterInfo(HsType.Halo3ODSTValue type) : this(type, "") { }

            public ParameterInfo(HsType.Halo3ODSTValue type, string name)
            {
                Type = type;
                Name = name;
            }
        }
        public static int GetScriptExpressionDataLength(HsSyntaxNode expr)
        {
            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.Expression:
                    return ValueTypeSizes[expr.ValueType.HaloOnline];

                default:
                    return 4;
            }
        }

        public static Dictionary<HsType.HaloOnlineValue, int> ValueTypeSizes { get; } = new Dictionary<HsType.HaloOnlineValue, int>
        {
            [HsType.HaloOnlineValue.Invalid] = 4,
            [HsType.HaloOnlineValue.Unparsed] = 0,
            [HsType.HaloOnlineValue.SpecialForm] = 0,
            [HsType.HaloOnlineValue.FunctionName] = 4,
            [HsType.HaloOnlineValue.Passthrough] = 0,
            [HsType.HaloOnlineValue.Void] = 4,
            [HsType.HaloOnlineValue.Boolean] = 1,
            [HsType.HaloOnlineValue.Real] = 4,
            [HsType.HaloOnlineValue.Short] = 2,
            [HsType.HaloOnlineValue.Long] = 4,
            [HsType.HaloOnlineValue.String] = 4,
            [HsType.HaloOnlineValue.Script] = 2,
            [HsType.HaloOnlineValue.StringId] = 4,
            [HsType.HaloOnlineValue.UnitSeatMapping] = 4,
            [HsType.HaloOnlineValue.TriggerVolume] = 2,
            [HsType.HaloOnlineValue.CutsceneFlag] = 2,
            [HsType.HaloOnlineValue.CutsceneCameraPoint] = 2,
            [HsType.HaloOnlineValue.CutsceneTitle] = 2,
            [HsType.HaloOnlineValue.CutsceneRecording] = 2,
            [HsType.HaloOnlineValue.DeviceGroup] = 4,
            [HsType.HaloOnlineValue.Ai] = 4,
            [HsType.HaloOnlineValue.AiCommandList] = 2,
            [HsType.HaloOnlineValue.AiCommandScript] = 2,
            [HsType.HaloOnlineValue.AiBehavior] = 2,
            [HsType.HaloOnlineValue.AiOrders] = 2,
            [HsType.HaloOnlineValue.AiLine] = 4,
            [HsType.HaloOnlineValue.StartingProfile] = 2,
            [HsType.HaloOnlineValue.Conversation] = 2,
            [HsType.HaloOnlineValue.ZoneSet] = 2,
            [HsType.HaloOnlineValue.DesignerZone] = 2,
            [HsType.HaloOnlineValue.PointReference] = 4,
            [HsType.HaloOnlineValue.Style] = 4,
            [HsType.HaloOnlineValue.ObjectList] = 4,
            [HsType.HaloOnlineValue.Folder] = 4,
            [HsType.HaloOnlineValue.Sound] = 4,
            [HsType.HaloOnlineValue.Effect] = 4,
            [HsType.HaloOnlineValue.Damage] = 4,
            [HsType.HaloOnlineValue.LoopingSound] = 4,
            [HsType.HaloOnlineValue.AnimationGraph] = 4,
            [HsType.HaloOnlineValue.DamageEffect] = 4,
            [HsType.HaloOnlineValue.ObjectDefinition] = 4,
            [HsType.HaloOnlineValue.Bitmap] = 4,
            [HsType.HaloOnlineValue.Shader] = 4,
            [HsType.HaloOnlineValue.RenderModel] = 4,
            [HsType.HaloOnlineValue.StructureDefinition] = 4,
            [HsType.HaloOnlineValue.LightmapDefinition] = 4,
            [HsType.HaloOnlineValue.CinematicDefinition] = 4,
            [HsType.HaloOnlineValue.CinematicSceneDefinition] = 4,
            [HsType.HaloOnlineValue.BinkDefinition] = 4,
            [HsType.HaloOnlineValue.AnyTag] = 4,
            [HsType.HaloOnlineValue.AnyTagNotResolving] = 4,
            [HsType.HaloOnlineValue.GameDifficulty] = 2,
            [HsType.HaloOnlineValue.Team] = 2,
            [HsType.HaloOnlineValue.MpTeam] = 2,
            [HsType.HaloOnlineValue.Controller] = 2,
            [HsType.HaloOnlineValue.ButtonPreset] = 2,
            [HsType.HaloOnlineValue.JoystickPreset] = 2,
            [HsType.HaloOnlineValue.PlayerCharacterType] = 2,
            [HsType.HaloOnlineValue.VoiceOutputSetting] = 2,
            [HsType.HaloOnlineValue.VoiceMask] = 2,
            [HsType.HaloOnlineValue.SubtitleSetting] = 2,
            [HsType.HaloOnlineValue.ActorType] = 2,
            [HsType.HaloOnlineValue.ModelState] = 2,
            [HsType.HaloOnlineValue.Event] = 2,
            [HsType.HaloOnlineValue.CharacterPhysics] = 2,
            [HsType.HaloOnlineValue.PrimarySkull] = 2,
            [HsType.HaloOnlineValue.SecondarySkull] = 4,
            [HsType.HaloOnlineValue.Object] = 4,
            [HsType.HaloOnlineValue.Unit] = 4,
            [HsType.HaloOnlineValue.Vehicle] = 4,
            [HsType.HaloOnlineValue.Weapon] = 4,
            [HsType.HaloOnlineValue.Device] = 4,
            [HsType.HaloOnlineValue.Scenery] = 4,
            [HsType.HaloOnlineValue.EffectScenery] = 4,
            [HsType.HaloOnlineValue.ObjectName] = 2,
            [HsType.HaloOnlineValue.UnitName] = 2,
            [HsType.HaloOnlineValue.VehicleName] = 2,
            [HsType.HaloOnlineValue.WeaponName] = 2,
            [HsType.HaloOnlineValue.DeviceName] = 2,
            [HsType.HaloOnlineValue.SceneryName] = 2,
            [HsType.HaloOnlineValue.EffectSceneryName] = 4,
            [HsType.HaloOnlineValue.CinematicLightprobe] = 4,
            [HsType.HaloOnlineValue.AnimationBudgetReference] = 4,
            [HsType.HaloOnlineValue.LoopingSoundBudgetReference] = 4,
            [HsType.HaloOnlineValue.SoundBudgetReference] = 4
        };

        public static Dictionary<CacheVersion, Dictionary<int, ScriptInfo>> Scripts { get; } = new Dictionary<CacheVersion, Dictionary<int, ScriptInfo>>
        {
            [CacheVersion.Halo3Retail] = new Dictionary<int, ScriptInfo>
            {
                [0x000] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "begin"),
                [0x001] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "begin_random"),
                [0x002] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "if"),
                [0x003] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "cond"),
                [0x004] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "set"),
                [0x005] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "and"),
                [0x006] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "or"),
                [0x007] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "+"),
                [0x008] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "-"),
                [0x009] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "*"),
                [0x00A] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "/"),
                [0x00B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "min"),
                [0x00C] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "max"),
                [0x00D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "="),
                [0x00E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "!="),
                [0x00F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, ">"),
                [0x010] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "<"),
                [0x011] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, ">="),
                [0x012] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "<="),
                [0x013] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sleep"),
                [0x014] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sleep_forever"),
                [0x015] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "sleep_until"),
                [0x016] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "wake"),
                [0x017] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "inspect"),
                [0x018] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "unit"),
                [0x019] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "not")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x01A] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "pin")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x01B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "print")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x01C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "log_print")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x01F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_scripting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x021] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "breakpoint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x022] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_active_scripts"),
                [0x023] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "get_executing_running_thread"),
                [0x024] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_thread")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x025] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "script_started")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x026] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "script_finished")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x027] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "players"),
                [0x028] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_volume_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x029] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_volume_disable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "volume_teleport_players_not_inside")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x02B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x02C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x02D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_objects_all")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x02E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_players")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_players_all")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x030] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "volume_return_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x031] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "volume_return_objects_by_type")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x032] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "zone_set_trigger_volume_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x033] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "list_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x034] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "list_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x035] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "list_count_not_dead")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x036] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x037] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_random")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x038] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_at_ai_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x039] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_on_object_marker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x03A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_on_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x03B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_new")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x03C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_object_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x03D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_objects_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x03E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x03F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x040] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_players")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                },
                [0x041] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "soft_ceiling_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x042] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x043] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_clone")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x044] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_anew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x045] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_if_necessary")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x046] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x047] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_clone_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x048] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_anew_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x049] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_folder")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Folder),
                },
                [0x04A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x04B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x04C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_all"),
                [0x04D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_type_mask")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x04E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_delete_by_definition")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x04F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_folder")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Folder),
                },
                [0x050] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_hide")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x051] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shadowless")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x052] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "object_buckling_magnitude_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x053] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_function_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x054] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_function_variable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x055] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_clear_function_variable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x056] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_clear_all_function_variables")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x057] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_dynamic_simulation_disable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x058] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_phantom_power")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x059] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_wake_physics")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x05A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_ranged_attack_inhibited")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x05B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_melee_attack_inhibited")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x05C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_dump_memory"),
                [0x05D] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "object_get_health")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x05E] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "object_get_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x05F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x060] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_physics")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x061] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "object_get_parent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x062] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_attach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x063] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "object_at_marker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x064] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_detach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x065] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x066] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x067] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_inertia_tensor_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x068] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_collision_damage_armor_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x069] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x06A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_deleted_when_deactivated")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x06B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_copy_player_appearance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x06C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "object_model_target_destroyed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x06D] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "object_model_targets_destroyed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x06E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_damage_damage_section")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x06F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cannot_die")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x070] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "object_vitality_pinned")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x071] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "garbage_collect_now"),
                [0x072] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "garbage_collect_unsafe"),
                [0x073] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "garbage_collect_multiplayer"),
                [0x074] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cannot_take_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x075] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_can_take_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x076] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cinematic_lod")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x077] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cinematic_collision")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x078] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cinematic_visibility")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x079] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x07A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_predict_high")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x07B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_predict_low")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x07C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_type_predict_high")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x07D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_type_predict_low")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x07E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_type_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x07F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x080] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_teleport_to_ai_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x081] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_facing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x082] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x083] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_stun")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x084] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_stun_infinite")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x085] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_permutation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x086] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_region_state")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ModelState),
                },
                [0x087] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "objects_can_see_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x088] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "objects_can_see_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x089] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "objects_distance_to_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x08A] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "objects_distance_to_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x08B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "map_info"),
                [0x08C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "position_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x08D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "shader_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Shader),
                },
                [0x08E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "bitmap_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Bitmap),
                },
                [0x08F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "script_recompile"),
                [0x090] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "script_doc"),
                [0x091] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "help")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x092] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "game_engine_objects"),
                [0x093] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "random_range")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x094] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "real_random_range")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x095] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_constants_reset"),
                [0x096] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_set_gravity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x097] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_set_velocity_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x098] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_disable_character_ground_adhesion_forces")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x099] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_debug_start"),
                [0x09A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_dump_world")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x09B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_dump_world_close_movie"),
                [0x09C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_profile_start"),
                [0x09D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_profile_end"),
                [0x09F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_reset_allocated_state"),
                [0x0A0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "breakable_surfaces_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0A1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "breakable_surfaces_reset"),
                [0x0A2] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "recording_play")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0A3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "recording_play_and_delete")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0A4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "recording_play_and_hover")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0A5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "recording_kill")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0A6] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "recording_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0A7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "render_lights")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0A9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_lights_enable_cinematic_shadow")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0AA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_set_object_marker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0AD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_attach_to_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0AE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_target_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_on"),
                [0x0B2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_off"),
                [0x0B3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_set_aspect_ratio")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_set_resolution")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x0B8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_enable_dynamic_lights")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0C0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_all_fog_planes")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0C1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_all_cluster_errors")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0C2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_line_opacity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0C3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_text_opacity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0C4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_opacity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0CB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_postprocess_color_tweaking_reset"),
                [0x0CC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0CD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0CE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0CF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_relative_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0D0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0D1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_relative_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0D2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_idle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                },
                [0x0D3] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "scenery_get_animation_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                },
                [0x0D4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_can_blink")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0D5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_active_camo")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0D6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_open")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0D7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_close")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0D8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_kill")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0D9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_kill_silent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0DA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_is_emitting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0DB] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "unit_get_custom_animation_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0DC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_stop_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0DD] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0DE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0DF] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0E0] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_relative_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0E1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_list")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0E2] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_custom_animation_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0E4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_is_playing_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0E5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_custom_animations_hold_on_last_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_custom_animations_prevent_lipsync_head_movement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_actively_controlled")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0EA] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "unit_get_team_index")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_aim_without_turning")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_enterable_by_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0ED] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_get_enterable_by_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_only_takes_damage_from_players_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_enter_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_falling_damage_disable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0F2] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "object_get_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0F3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_board_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0F4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_emotion_by_?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0F5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_emotion_by_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0F6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_enable_eye_tracking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0F7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_integrated_flashlight")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0F8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_in_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_test_seat_list")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x0FA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_test_seat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0FB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_prefer_tight_camera_track")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0FC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0FD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0FE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_maximum_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0FF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "units_set_maximum_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x100] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_current_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x101] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "units_set_current_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x102] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "vehicle_load_magic")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x103] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "vehicle_unload")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x104] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_animation_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x105] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "magic_melee_attack"),
                [0x106] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "vehicle_riders")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x107] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "vehicle_driver")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x108] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "vehicle_gunner")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x109] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "unit_get_health")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x10A] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "unit_get_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x10B] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "unit_get_total_grenade_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x10C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x10D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_weapon_readied")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x10E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_any_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x10F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x110] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_lower_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x111] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_raise_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x112] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_drop_support_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x114] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_animation_forced_seat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x115] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_doesnt_drop_items")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x116] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_impervious")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x117] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_suspended")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x118] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_add_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StartingProfile),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x119] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "weapon_hold_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Weapon),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x11A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "weapon_enable_warthog_chaingun_light")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x11B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_set_never_appears_locked")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x11C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_set_power")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x11D] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "device_get_power")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                },
                [0x11E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_set_position")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x11F] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "device_get_position")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                },
                [0x120] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_set_position_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x121] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "device_group_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                },
                [0x122] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_group_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x123] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_group_set_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x124] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_one_sided_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x126] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_operates_automatically_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x127] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_closes_automatically_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x128] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_group_change_only_once_more_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x129] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_set_position_track")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x12A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_set_overlay_track")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x12B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_animate_position")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x12C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_animate_overlay")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x12D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_all_powerups"),
                [0x12E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_all_weapons"),
                [0x12F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_all_vehicles"),
                [0x130] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_teleport_to_camera"),
                [0x131] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_active_camouflage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x132] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_active_camouflage_by_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x133] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheats_load"),
                [0x135] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "drop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x136] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x137] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_enabled"),
                [0x138] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x139] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x13B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_log_reset"),
                [0x13C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_log_dump")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x13D] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "ai_get_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x13E] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "ai_get_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x13F] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_get_squad")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x140] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_get_turret_ai")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x141] = new ScriptInfo(HsType.Halo3ODSTValue.PointReference, "ai_random_smart_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x142] = new ScriptInfo(HsType.Halo3ODSTValue.PointReference, "ai_nearest_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x143] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "ai_get_point_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x144] = new ScriptInfo(HsType.Halo3ODSTValue.PointReference, "ai_point_set_get_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x145] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_attach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x146] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_attach_units")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x147] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_detach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x148] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_detach_units")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x149] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x14A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x14B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_place_in_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x14C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_cannot_die")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x14D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_vitality_pinned")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x14E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_resurrect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x14F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_kill")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x150] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_kill_silent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x151] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_erase")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x152] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_erase_all"),
                [0x153] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_disposable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x154] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_select")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x155] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_deselect"),
                [0x156] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_deaf")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x157] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_blind")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x158] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_weapon_up")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x159] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_flood_disperse")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x15A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_magically_see")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x15B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_magically_see_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x15C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_active_camo")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x15D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_suppress_combat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x15E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_migrate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x15F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allegiance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x160] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allegiance_remove")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x161] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allegiance_break")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x162] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_braindead")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x163] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_braindead_by_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x164] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_disregard")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x165] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_prefer_target")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x166] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_prefer_target_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x167] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_prefer_target_ai")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x168] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x169] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x16A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_teleport_to_starting_location_if_outside_bsp")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x16B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x16C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_bring_forward")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x16D] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_migrate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x16F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "biped_morph")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x170] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_renew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x171] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_force_active")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x172] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_force_active_by_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x173] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_playfight")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x174] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_reconnect"),
                [0x175] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_berserk")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x176] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x177] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allow_dormant")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x178] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_is_attacking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x179] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_fighting_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x17A] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_living_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x17B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_living_fraction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x17C] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_in_vehicle_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x17D] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_body_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x17E] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_strength")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x17F] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_swarm_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x180] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_nonswarm_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x181] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "ai_actors")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x182] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_allegiance_broken")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x183] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_spawn_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x184] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "object_get_ai")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x187] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_set_task")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x188] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_set_objective")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x189] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_task_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x18A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_set_task_condition")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x18B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_leadership")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x18C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_leadership_all")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x18D] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_task_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x18E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "generate_pathfinding"),
                [0x18F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_render_paths_all"),
                [0x190] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_activity_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x191] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_activity_abort")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x192] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "ai_vehicle_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x193] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_starting_location")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x194] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve_seat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x195] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x196] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_player_get_vehicle_squad")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x197] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_vehicle_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x198] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_carrying_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x199] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_in_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x19A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_in_vehicle?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x19B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_player_any_needs_vehicle"),
                [0x19C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x19D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x19E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x19F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x1A0] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_enter_squad_vehicles")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1A2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_overturned")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x1A4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_flip")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x1A5] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1A7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1A8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_create")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1A9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_delete")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1AD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_verify_tags"),
                [0x1AE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_wall_lean")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1AF] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x1B0] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line_at_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x1B1] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line_on_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x1B2] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line_on_object_allied")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x1B3] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1B4] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1B5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_time_pause")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1B6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_points")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1B7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_primary_skull")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1B8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_secondary_skull")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1B9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_metagame_enabled"),
                [0x1BA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_easy"),
                [0x1BB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_normal"),
                [0x1BC] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_heroic"),
                [0x1BD] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_legendary"),
                [0x1BE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_run_command_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1BF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_queue_command_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1C0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stack_command_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1C1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1C2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1C3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1C4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1C5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1C6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1C7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1C8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1C9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1CA] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "vs_role")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1CB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1CC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1CD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1CE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1CF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1D0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1D1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1D2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1D3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1D4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1D5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1D6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1D7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1D8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1D9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1DA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1DB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_set_cleanup_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Script),
                },
                [0x1DC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_release")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1DD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_release_all"),
                [0x1DE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "cs_command_script_running")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1DF] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "cs_command_script_attached")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1E0] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "cs_number_queued")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1E3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1E4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom_movement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1E5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom_?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1E6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1E8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1EA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1F1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1F3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1F4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1F5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1F6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1F7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1F8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1F9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1FA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1FB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1FC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x1FD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_and_posture")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1FE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_and_posture")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1FF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_nearest")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x200] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_nearest")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x201] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_in_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x202] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_in_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x203] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_towards?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x204] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_towards?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x205] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_towards")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x206] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_towards")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x207] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_swarm_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x208] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_swarm_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x209] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_swarm_from")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x20A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_swarm_from")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x20B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_pause")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x20C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_pause")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x20D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_grenade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x20E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_grenade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x20F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x210] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x211] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_jump")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x212] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_jump")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x213] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_jump_to_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x214] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_jump_to_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x215] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vocalize")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x216] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vocalize")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x217] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x218] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x219] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_action")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x21E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_action")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x21F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_action_at_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x220] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_action_at_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x221] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_action_at_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x222] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_action_at_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x223] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x224] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x225] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x226] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x227] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x228] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x229] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x22A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x22B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x22C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x22D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x22E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x22F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_line")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x230] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_line")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x233] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_deploy_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x234] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_deploy_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x235] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_approach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x236] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_approach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x237] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_approach_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x238] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_approach_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x239] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x23A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x23B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x23C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x23D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_set_style")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Style),
                },
                [0x23E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_set_style")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Style),
                },
                [0x23F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_force_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x240] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_force_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x241] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_targeting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x242] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_targeting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x243] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_looking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x244] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_looking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x245] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_moving")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x246] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_moving")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x247] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x248] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x249] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_suppress_activity_termination")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x24A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_suppress_activity_termination")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x24B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x24C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x24D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_look")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x24E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_look")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x24F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_look_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x250] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_look_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x251] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_look_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x252] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_look_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x253] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_aim")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x254] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_aim")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x255] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_aim_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x256] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_aim_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x257] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_aim_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x258] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_aim_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x259] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x25A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x25B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_face_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x25C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_face_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x25D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_face_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x25E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_face_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x25F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x260] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x261] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x262] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x263] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x264] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x265] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x266] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x267] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_lower_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x268] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_lower_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x269] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vehicle_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x26A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vehicle_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x26B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x26C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x26D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vehicle_boost")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x26E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vehicle_boost")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x26F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_turn_sharpness?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x270] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_turn_sharpness?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x271] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x272] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x273] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_set_pathfinding_radius")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x274] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_set_pathfinding_radius")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x275] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_ignore_obstacles")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x276] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_ignore_obstacles")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x277] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_approach_stop"),
                [0x278] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_approach_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x279] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_movement_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x27A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_movement_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x27B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x27E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x27F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_walk")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x280] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_walk")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x281] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_posture_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x282] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_posture_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x283] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_posture_exit"),
                [0x284] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_posture_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x285] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stow"),
                [0x286] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_stow")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x287] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_draw"),
                [0x288] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_draw")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x289] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x28A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x28B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stop_custom_animation"),
                [0x28C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_stop_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x28D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stop_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x28E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_stop_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x28F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_player_melee")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x290] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_player_melee")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x291] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_melee_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x292] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_melee_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x295] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_control")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x296] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x297] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x298] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x299] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x29A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_with_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x29D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_loop_offset")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_predict_resources_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x29F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_predict_resources_at_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2A0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_first_person")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x2A1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_cinematic"),
                [0x2A2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_cinematic_scene")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x2A3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_place_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x2A4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_place_worldspace"),
                [0x2A5] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "camera_time"),
                [0x2A6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_field_of_view")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2A8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_camera_set_easing_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2A9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_print")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2AA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_pan")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2AB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_pan")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2AC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_save"),
                [0x2AD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_load"),
                [0x2AE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_save_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2AF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_load_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "director_debug_camera")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2B1] = new ScriptInfo(HsType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get"),
                [0x2B2] = new ScriptInfo(HsType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get_real"),
                [0x2B3] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "game_insertion_point_get"),
                [0x2B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_insertion_point_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2B5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pvs_set_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x2B6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pvs_set_camera")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2B7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pvs_clear"),
                [0x2B9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "players_unzoom_all"),
                [0x2BA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_enable_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2BB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_disable_movement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2BD] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_flashlight_on"),
                [0x2BE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_active_camouflage_on"),
                [0x2BF] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_camera_control")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2C0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_action_test_reset"),
                [0x2C1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_jump"),
                [0x2C2] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_primary_trigger"),
                [0x2C3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_grenade_trigger"),
                [0x2C4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_vision_trigger"),
                [0x2C5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_zoom"),
                [0x2C6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_rotate_weapons"),
                [0x2C7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_rotate_grenades"),
                [0x2C8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_melee"),
                [0x2C9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_action"),
                [0x2CA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_accept"),
                [0x2CB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_cancel"),
                [0x2CC] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_up"),
                [0x2CD] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_down"),
                [0x2CE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_left"),
                [0x2CF] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_right"),
                [0x2D0] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_all_directions"),
                [0x2D1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_move_relative_all_directions"),
                [0x2D2] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_cinematic_skip"),
                [0x2D3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_start"),
                [0x2D4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_back"),
                [0x2D5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player0_looking_up"),
                [0x2D6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player0_looking_down"),
                [0x2D7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player0_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2D8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player1_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2D9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player2_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2DA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player3_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2DB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_action_test_look_up_begin")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2DC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_action_test_look_down_begin")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2DD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_action_test_look_pitch_end"),
                [0x2DE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_lookstick_forward"),
                [0x2DF] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_lookstick_backward"),
                [0x2E0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_teleport_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2E1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "map_reset"),
                [0x2E3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "switch_zone_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2E4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "switch_zone_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ZoneSet),
                },
                [0x2E5] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "current_zone_set"),
                [0x2E6] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "current_zone_set_fully_active"),
                [0x2E8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "crash")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "version"),
                [0x2EA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "status"),
                [0x2EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "record_movie")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "record_movie_distributed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "screenshot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "screenshot_big")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "screenshot_big_jittered")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2F5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "main_menu"),
                [0x2F6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "main_halt"),
                [0x2F8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "map_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2F9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_multiplayer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2FA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_splitscreen")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2FB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_difficulty")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.GameDifficulty),
                },
                [0x301] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x304] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_rate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x308] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_memory"),
                [0x309] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_memory_by_file"),
                [0x30A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_memory_for_file")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x30B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_tags"),
                [0x30C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "tags_verify_all"),
                [0x31A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_control_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x31B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_control_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x31D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_break_on_vocalization")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x31E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "fade_in")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x31F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "fade_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x320] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_start"),
                [0x321] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_stop"),
                [0x322] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_skip_start_internal"),
                [0x323] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_skip_stop_internal"),
                [0x324] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_show_letterbox")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x325] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_show_letterbox_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x326] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_title")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x327] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_title_delayed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x328] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_suppress_bsp_object_creation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x329] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_subtitle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x32A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicDefinition),
                },
                [0x32B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_shot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x32D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_early_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x32E] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "cinematic_get_early_exit"),
                [0x330] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_object_create")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x331] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_object_create_cinematic_anchor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x332] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_object_destroy")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x333] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_destroy"),
                [0x334] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_clips_initialize_for_shot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x335] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_clips_destroy"),
                [0x336] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lights_initialize_for_shot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x337] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lights_destroy"),
                [0x338] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_light_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicLightprobe),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x339] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_light_object_off")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x33A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lighting_rebuild_all"),
                [0x33B] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "cinematic_object_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x33C] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "cinematic_object_get_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x33D] = new ScriptInfo(HsType.Halo3ODSTValue.Scenery, "cinematic_object_get_scenery")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x33E] = new ScriptInfo(HsType.Halo3ODSTValue.EffectScenery, "cinematic_object_get_effect_scenery")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x340] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_briefing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x341] = new ScriptInfo(HsType.Halo3ODSTValue.CinematicDefinition, "cinematic_tag_reference_get_cinematic")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x342] = new ScriptInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition, "cinematic_tag_reference_get_scene")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x343] = new ScriptInfo(HsType.Halo3ODSTValue.Effect, "cinematic_tag_reference_get_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x344] = new ScriptInfo(HsType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x345] = new ScriptInfo(HsType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound2")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x346] = new ScriptInfo(HsType.Halo3ODSTValue.LoopingSound, "cinematic_tag_reference_get_looping_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x347] = new ScriptInfo(HsType.Halo3ODSTValue.AnimationGraph, "cinematic_tag_reference_get_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x348] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "cinematic_tag_reference_get_?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x349] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_fade_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x34D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x34E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x34F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_music")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x350] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x351] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_stop_music")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x353] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x354] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_object_no_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x355] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object_no_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x356] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_play_cortana_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x357] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "attract_mode_start"),
                [0x358] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "attract_mode_set_seconds")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x359] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_won"),
                [0x35A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_lost")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x35B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_revert"),
                [0x35C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_is_cooperative"),
                [0x35D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_is_playtest"),
                [0x35E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_can_use_flashlights")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x35F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_and_quit"),
                [0x360] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_unsafe"),
                [0x361] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_insertion_point_unlock")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x362] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_insertion_point_lock")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x368] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_has_achievement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Controller),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x369] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_grant_achievement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Controller),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x388] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load"),
                [0x389] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x38A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_save"),
                [0x38B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_save_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x38C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load_game"),
                [0x38D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load_game_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x38E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_regular_upload_to_debug_server")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x38F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_set_upload_option")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x392] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_safe_to_save"),
                [0x393] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_safe_to_speak"),
                [0x394] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_all_quiet"),
                [0x395] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save"),
                [0x396] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_cancel"),
                [0x397] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_no_timeout"),
                [0x398] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_immediate"),
                [0x399] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_saving"),
                [0x39A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_reverted"),
                [0x39B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_set_tag_parameter_unsafe")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x39C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x39D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x39E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x39F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_cinematic")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3A0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3A1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_with_subtitle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3A2] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "sound_impulse_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3A3] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "sound_impulse_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3A4] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "sound_impulse_language_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3A5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3A6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_3d")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3A7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_mark_as_outro")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3A9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                },
                [0x3AA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3AB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                },
                [0x3AC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_stop_immediately")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                },
                [0x3AD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_set_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3AE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_set_alternate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3AF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_loop_spam"),
                [0x3B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_show_channel")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3B1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_debug_sound_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3B2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_sounds_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3B3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_set_gain")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x3B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_set_gain_db")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x3B5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_enable_ducker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3B6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_sound_environment_parameter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3B7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_set_global_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3B8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_set_global_effect_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3B9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_auto_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3BA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_hover")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3BB] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "vehicle_count_bipeds_killed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x3BC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "biped_ragdoll")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x3BE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_show_training_text")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3BF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_set_training_text")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3C0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_enable_training")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3C1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_flashlight"),
                [0x3C2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_crouch"),
                [0x3C3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_stealth"),
                [0x3C4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_?"),
                [0x3C5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_jump"),
                [0x3C6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_activate_team_nav_point_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3C7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_deactivate_team_nav_point_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x3C8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_cortana_suck")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3CB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_cortana_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3CD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_weapon_stats")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3CE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_health")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3CF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3D0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3D1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_messages")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3D2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_motion_sensor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3D3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_spike_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3D4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_fire_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3D5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_cinematic_fade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3D6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cls"),
                [0x3D7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_overflow_suppression")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3D8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x3D9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_hide")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x3DA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_show_all"),
                [0x3DB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_hide_all"),
                [0x3DC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_list"),
                [0x3DD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_translation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3DE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_rotation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3DF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_rumble")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3E0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3E1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3E2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "time_code_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3E3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "time_code_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3E4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "time_code_reset"),
                [0x3E5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "script_screen_effect_set_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3E8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade2")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_stop"),
                [0x3EA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_near_clip_distance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_far_clip_distance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_atmosphere_fog")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "motion_blur")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_weather")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_patchy_fog")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_environment_map_attenuation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3F1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_environment_map_bitmap")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Bitmap),
                },
                [0x3F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_bitmap"),
                [0x3F3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_environment_map_tint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3F4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_tint"),
                [0x3F5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_layer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3F6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_has_skills"),
                [0x3F7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_has_mad_secret_skills")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3F8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "controller_invert_look"),
                [0x3F9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "controller_look_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x3FA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "controller_set_look_invert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3FB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "controller_get_look_invert"),
                [0x3FC] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "controller_unlock_solo_levels")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x44A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_clear"),
                [0x44B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_show_up_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x44C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_finish_up_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x44D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_secondary_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x44E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_primary_unavailable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x44F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_secondary_unavailable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x450] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "input_suppress_rumble")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x46C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_delegate_host")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x46D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_delegate_leader")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x46E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_map_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x470] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_campaign_difficulty")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x471] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_player_color")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x482] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_bink_movie")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x483] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_bink_movie_from_tag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.BinkDefinition),
                },
                [0x484] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_credits"),
                [0x485] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "bink_time"),
                [0x486] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_global_doppler_factor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x487] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_global_mixbin_headroom")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x488] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_sound_environment_source_parameter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x489] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_set_mission_segment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x48A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_display_mission_segment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x48B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_insert"),
                [0x48D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_upload"),
                [0x48E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_playback")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x490] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4BB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "bug_now")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4BC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "bug_now_lite")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4BD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "bug_now_auto")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4BE] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "object_list_children")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x4BF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "voice_set_outgoing_channel_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4C0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "voice_set_voice_repeater_peer_index")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4C7] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4C8] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_start_smooth")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4C9] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x4CA] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_restart")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x4CB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "interpolator_is_active")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x4CC] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "interpolator_is_finished")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x4CD] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_set_current_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4CE] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_current_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4CF] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_start_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4D0] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_final_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4D1] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_current_phase")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x4D2] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_current_time_fraction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x4D3] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_start_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x4D4] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_final_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x4D5] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4D6] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_fraction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4D7] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4D8] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_delta")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4D9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "interpolator_stop_all"),
                [0x4DA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "interpolator_restart_all"),
                [0x4DB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "interpolator_flip"),
                [0x4DD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "animation_cache_stats_reset"),
                [0x4DE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_clone_players_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x4DF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_move_attached_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x4E0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_enable_ghost_effects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4E1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_global_sound_environment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4E2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "reset_dsp_image"),
                [0x4E3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_cinematic_skip"),
                [0x4E4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_outro_start"),
                [0x4E5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_enable_ambience_details")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_reset")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4E8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_blur_amount")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_threshold")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4EA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_brightness")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_box_factor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_max_factor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_silver_bullet")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_only")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_high_res")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_brightness_alpha")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4F1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_max_factor_alpha")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cache_block_for_one_frame"),
                [0x4F3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_suppress_ambience_update_on_revert"),
                [0x4F7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_exposure_fade_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4F8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_exposure")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4F9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_autoexposure_instant")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4FB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_depth_of_field_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4FC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_depth_of_field")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x500] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_disable"),
                [0x501] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_enable"),
                [0x502] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "predict_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x503] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "mp_players_by_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x504] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "mp_active_player_count_by_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x506] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_game_won")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x508] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_ai_allegiance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x509] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_allegiance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x513] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_object_create_anew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x514] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_object_destroy")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x52F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "predict_bink_movie")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x530] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "predict_bink_movie_from_tag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.BinkDefinition),
                },
                [0x532] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x534] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "game_coop_player_count"),
                [0x539] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "add_recycling_volume")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x541] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "game_tick_get"),
                [0x550] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "saved_film_set_playback_game_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x554] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "designer_zone_activate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DesignerZone),
                },
                [0x555] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "designer_zone_deactivate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DesignerZone),
                },
                [0x556] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_always_active")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x55C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_persistent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x57B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_performance_throttle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x57C] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "get_performance_throttle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x57E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_zone_activate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x57F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_zone_deactivate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x583] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_limit_lipsync_to_mouth_only")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x58A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "prepare_to_switch_to_zone_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ZoneSet),
                },
                [0x58B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_zone_activate_for_debugging")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x58C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_play_random_ping")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x58D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_fade_out_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x58E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_fade_in_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x58F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_lock_gaze")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x590] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_unlock_gaze")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x591] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_scale_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x595] = new ScriptInfo(HsType.Halo3ODSTValue.BinkDefinition, "cinematic_tag_reference_get_bink")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x598] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_custom_animation_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x599] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x59A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "film_manager_set_reproduction_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x59B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_arbiter_ai_navpoint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x59C] = new ScriptInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition, "cortana_effect_tag_reference_get_scene")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x59F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "biped_force_ground_fitting_on")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x5A0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_chud_objective")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x5A1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "terminal_is_being_read"),
                [0x5A2] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "terminal_was_accessed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x5A3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "terminal_was_completed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x5A4] = new ScriptInfo(HsType.Halo3ODSTValue.Weapon, "unit_get_primary_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x5A7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_award_level_complete_achievements"),
                [0x5A9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_safe_to_respawn")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x5AA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cortana_effect_kill"),
                [0x5AD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_cortana_effect_cinematic"),
                [0x5AE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_migrate_infanty")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x5AF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_cinematic_motion_blur")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x5B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dont_do_avoidance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x5B1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_clean_up")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5B2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_erase_inactive")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x5B3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "stop_bink_movie"),
                [0x5B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_credits_unskippable")
            },
            [CacheVersion.Halo3ODST] = new Dictionary<int, ScriptInfo>
            {
                [0x000] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "begin"),
                [0x001] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "begin_random"),
                [0x002] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "if"),
                [0x003] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "cond"),
                [0x004] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "set"),
                [0x005] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "and"),
                [0x006] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "or"),
                [0x007] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "+"),
                [0x008] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "-"),
                [0x009] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "*"),
                [0x00A] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "/"),
                [0x00B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "min"),
                [0x00C] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "max"),
                [0x00D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "="),
                [0x00E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "!="),
                [0x00F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, ">"),
                [0x010] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "<"),
                [0x011] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, ">="),
                [0x012] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "<="),
                [0x013] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sleep"),
                [0x014] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sleep_forever"),
                [0x015] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "sleep_until"),
                [0x016] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "wake"),
                [0x017] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "inspect"),
                [0x018] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "unit"),
                [0x019] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "evaluate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Script),
                },
                [0x01A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "not")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x01B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "pin")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x01C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "print")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x01D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "log_print")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x020] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_scripting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x024] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "breakpoint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x025] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_active_scripts"),
                [0x026] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "get_executing_running_thread"),
                [0x027] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_thread")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x028] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "script_started")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x029] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "script_finished")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x02A] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "players"),
                [0x02B] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "player_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x02C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_volume_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_volume_disable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "volume_teleport_players_not_inside")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x02F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x030] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x031] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_objects_all")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x032] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_players")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x033] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_players_all")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x034] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "volume_return_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x035] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "volume_return_objects_by_type")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x036] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "zone_set_trigger_volume_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x037] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "list_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x038] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "list_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x039] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "list_count_not_dead")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x03A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x03B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_random")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x03C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_at_ai_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x03D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_on_object_marker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x03E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_on_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x03F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_new")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x040] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_object_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x041] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_objects_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x042] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x043] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x044] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_players")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                },
                [0x045] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "soft_ceiling_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x046] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x047] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_clone")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x048] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_anew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x049] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_if_necessary")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x04A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x04B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_clone_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x04C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_anew_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x04D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_folder")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Folder),
                },
                [0x04E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_folder_anew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Folder),
                },
                [0x04F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x050] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x051] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_all"),
                [0x052] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_type_mask")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x053] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_delete_by_definition")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x054] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_folder")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Folder),
                },
                [0x055] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_hide")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x056] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shadowless")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x057] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "object_buckling_magnitude_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x058] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_function_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x059] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_function_variable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x05A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_clear_function_variable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x05B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_clear_all_function_variables")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x05C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_dynamic_simulation_disable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x05D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_phantom_power")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x05E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_wake_physics")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x05F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_ranged_attack_inhibited")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x060] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_melee_attack_inhibited")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x061] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_dump_memory"),
                [0x062] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "object_get_health")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x063] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "object_get_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x064] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x065] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_physics")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x066] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "object_get_parent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x067] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_attach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x068] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "object_at_marker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x069] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_detach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x06A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x06B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x06C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_inertia_tensor_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x06D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_collision_damage_armor_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x06E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x06F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_deleted_when_deactivated")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x070] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_copy_player_appearance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x071] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "object_model_target_destroyed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x072] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "object_model_targets_destroyed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x073] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_damage_damage_section")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x074] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cannot_die")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x075] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "object_vitality_pinned")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x076] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "garbage_collect_now"),
                [0x077] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "garbage_collect_unsafe"),
                [0x078] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "garbage_collect_multiplayer"),
                [0x079] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cannot_take_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x07A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_can_take_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x07B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cinematic_lod")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x07C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cinematic_collision")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x07D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cinematic_visibility")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x07E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x07F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_predict_high")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x080] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_predict_low")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x081] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_type_predict_high")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x082] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_type_predict_low")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x083] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_type_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x084] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x085] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_teleport_to_ai_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x086] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_facing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x087] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x088] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_normalized")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x089] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_stun")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x08A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_stun_infinite")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x08B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_permutation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x08C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_variant")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x08D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_region_state")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ModelState),
                },
                [0x08E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "objects_can_see_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x08F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "objects_can_see_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x090] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "objects_distance_to_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x091] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "objects_distance_to_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x092] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "map_info"),
                [0x093] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "position_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x094] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "shader_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Shader),
                },
                [0x095] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "bitmap_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Bitmap),
                },
                [0x096] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "script_recompile"),
                [0x097] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "script_doc"),
                [0x098] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "help")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x099] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "game_engine_objects"),
                [0x09A] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "random_range")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x09B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "real_random_range")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x09C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_constants_reset"),
                [0x09D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_set_gravity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x09E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_set_velocity_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x09F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_disable_character_ground_adhesion_forces")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0A0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_debug_start"),
                [0x0A1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_dump_world")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0A2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_dump_world_close_movie"),
                [0x0A3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_profile_start"),
                [0x0A4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_profile_end"),
                [0x0A6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_reset_allocated_state"),
                [0x0A7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "breakable_surfaces_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0A8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "breakable_surfaces_reset"),
                [0x0A9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "recording_play")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "recording_play_and_delete")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "recording_play_and_hover")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "recording_kill")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0AD] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "recording_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0AE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "render_lights")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_lights_enable_cinematic_shadow")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0B1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_set_object_marker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_attach_to_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0B5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_target_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0B7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_on"),
                [0x0B9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_off"),
                [0x0BA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_set_aspect_ratio")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0BB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_set_resolution")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x0BF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_enable_dynamic_lights")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0C4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_camera_attach_to_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0C5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_camera_target_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0C7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_camera_highlight_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0D3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_all_fog_planes")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0D4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_all_cluster_errors")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0D5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_line_opacity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0D6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_text_opacity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0D7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_opacity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0E2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_postprocess_color_tweaking_reset"),
                [0x0E3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0E4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0E5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_relative_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0E8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_relative_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_idle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                },
                [0x0EA] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "scenery_get_animation_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                },
                [0x0EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_can_blink")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_active_camo")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_open")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_close")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_kill")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_kill_silent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_is_emitting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F2] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "unit_get_custom_animation_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_stop_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0F5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0F6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0F7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_relative_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0F8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_list")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0F9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_custom_animation_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0FB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_is_playing_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0FC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_custom_animations_hold_on_last_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0FD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_custom_animations_prevent_lipsync_head_movement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x100] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_actively_controlled")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x101] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "unit_get_team_index")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x102] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_aim_without_turning")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x103] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_enterable_by_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x104] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_get_enterable_by_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x105] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_only_takes_damage_from_players_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x106] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_enter_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x107] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_falling_damage_disable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x108] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_in_vehicle_type")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x10A] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "object_get_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x10B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_board_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x10C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_emotion_by_?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x10D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_emotion_by_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x10E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_enable_eye_tracking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x10F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_integrated_flashlight")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x110] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnyTag),
                },
                [0x111] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_enable_vision_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x112] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_in_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x113] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_test_seat_list")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x114] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_test_seat_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x115] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_test_seat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x116] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_prefer_tight_camera_track")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x117] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x118] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x119] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_maximum_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x11A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "units_set_maximum_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x11B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_current_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x11C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "units_set_current_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x11D] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "vehicle_load_magic")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x11E] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "vehicle_unload")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x11F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_animation_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x120] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "magic_melee_attack"),
                [0x121] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "vehicle_riders")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x122] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "vehicle_driver")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x123] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "vehicle_gunner")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x124] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "unit_get_health")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x125] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "unit_get_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x126] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "unit_get_total_grenade_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x127] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x128] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_weapon_readied")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x129] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_any_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x12A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x12B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_lower_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x12C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_raise_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x12D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_drop_support_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x131] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_animation_forced_seat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x132] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_doesnt_drop_items")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x133] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_impervious")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x134] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_suspended")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x135] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_add_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StartingProfile),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x136] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "weapon_hold_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Weapon),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x137] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "weapon_enable_warthog_chaingun_light")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x138] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_set_never_appears_locked")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x139] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_set_power")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x13A] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "device_get_power")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                },
                [0x13B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_set_position")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x13C] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "device_get_position")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                },
                [0x13D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_set_position_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x13E] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "device_group_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                },
                [0x13F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_group_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x140] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_group_set_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x141] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_one_sided_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x143] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_operates_automatically_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x144] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_closes_automatically_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x145] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_group_change_only_once_more_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x146] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_set_position_track")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x147] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_set_overlay_track")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x148] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_animate_position")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x149] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_animate_overlay")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x14A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_all_powerups"),
                [0x14B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_all_weapons"),
                [0x14C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_all_vehicles"),
                [0x14D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_teleport_to_camera"),
                [0x14E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_active_camouflage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x14F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_active_camouflage_by_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x150] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheats_load"),
                [0x152] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "drop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x154] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x155] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_enabled"),
                [0x156] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x157] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x158] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_player_dialogue_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x15A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_fast_and_dumb")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x15B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_log_reset"),
                [0x15C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_log_dump")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x15D] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "ai_get_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x15E] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "ai_get_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x15F] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_get_squad")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x160] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_get_turret_ai")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x161] = new ScriptInfo(HsType.Halo3ODSTValue.PointReference, "ai_random_smart_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x162] = new ScriptInfo(HsType.Halo3ODSTValue.PointReference, "ai_nearest_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x163] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "ai_get_point_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x164] = new ScriptInfo(HsType.Halo3ODSTValue.PointReference, "ai_point_set_get_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x165] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x166] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x167] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_place_in_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x168] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_cannot_die")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x169] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_vitality_pinned")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x16A] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_index_from_spawn_formation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x16B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_resurrect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x16C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_kill")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x16D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_kill_silent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x16E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_erase")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x16F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_erase_all"),
                [0x170] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_disposable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x171] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_select")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x172] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_deselect"),
                [0x173] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_deaf")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x174] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_blind")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x175] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_weapon_up")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x176] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_flood_disperse")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x177] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_magically_see")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x178] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_magically_see_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x179] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_active_camo")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x17A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_suppress_combat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x17C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_grunt_kamikaze")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x17D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_migrate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x17E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allegiance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x17F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allegiance_remove")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x180] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allegiance_break")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x181] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_braindead")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x182] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_braindead_by_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x183] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_disregard")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x184] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_prefer_target")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x185] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_prefer_target_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x186] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_prefer_target_ai")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x187] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x188] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x189] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_teleport_to_starting_location_if_outside_bsp")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x18A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_teleport_to_spawn_point_if_outside_bsp")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x18B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x18C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_bring_forward")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x18D] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_migrate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x18F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "biped_morph")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x190] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_renew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x191] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_force_active")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x192] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_force_active_by_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x193] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_playfight")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x194] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_reconnect"),
                [0x195] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_berserk")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x196] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x197] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allow_dormant")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x198] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_is_attacking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x199] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_fighting_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19A] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_living_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_living_fraction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19C] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_in_vehicle_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19D] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_body_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19E] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_strength")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19F] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_swarm_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A0] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_nonswarm_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A1] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "ai_actors")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A2] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_allegiance_broken")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x1A3] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_spawn_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A4] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "object_get_ai")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x1AF] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_set_task")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1B0] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_set_objective")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1B1] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_task_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1B2] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_set_task_condition")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1B3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_leadership")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1B4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_leadership_all")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1B5] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_task_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1B6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_reset_objective")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1B7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_squad_patrol_objective_disallow")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1B8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "generate_pathfinding"),
                [0x1B9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_render_paths_all"),
                [0x1BA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_activity_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1BB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_activity_abort")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1BC] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "ai_vehicle_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1BD] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_starting_location")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1BE] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_spawn_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1BF] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_vehicle_get_squad_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1C0] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_squad")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1C1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve_seat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1C2] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1C3] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_player_get_vehicle_squad")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x1C4] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_vehicle_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1C5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_carrying_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1C6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_in_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x1C7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_in_vehicle?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x1C8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_player_any_needs_vehicle"),
                [0x1C9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1CA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x1CB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1CC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x1CD] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_enter_squad_vehicles")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1CE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1CF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1D0] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_overturned")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x1D1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_flip")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x1D2] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1D3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_ai_navpoint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x1D4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1D5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1D6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_create")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1D7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_delete")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1DB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_verify_tags"),
                [0x1DC] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_wall_lean")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1DD] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x1DE] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line_at_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x1DF] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line_on_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x1E0] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line_on_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x1E1] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1E2] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1E3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_time_pause")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1E4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_points")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1E5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_primary_skull")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PrimarySkull),
                },
                [0x1E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_secondary_skull")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.SecondarySkull),
                },
                [0x1E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_event")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1E8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_metagame_enabled"),
                [0x1E9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_survival_enabled"),
                [0x1EA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_easy"),
                [0x1EB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_normal"),
                [0x1EC] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_heroic"),
                [0x1ED] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_legendary"),
                [0x1EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_run_command_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_queue_command_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stack_command_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1F1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1F3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1FA] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "vs_role")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1FB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1FC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1FD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1FE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1FF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x200] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x201] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x202] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x203] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x204] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x205] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x206] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x207] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x208] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x209] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x20A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x20B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_set_cleanup_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Script),
                },
                [0x20C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_release")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x20D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_release_all"),
                [0x20E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "cs_command_script_running")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x20F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "cs_command_script_attached")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x210] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "cs_number_queued")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x213] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x214] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom_movement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x215] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom_?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x216] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x217] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x218] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x219] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x21C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x21D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x220] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x221] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x222] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x223] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x224] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x225] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x226] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x227] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x228] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x229] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x22A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x22B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x22C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x22D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_and_posture")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x22E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_and_posture")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x22F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_nearest")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x230] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_nearest")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x231] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_in_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x232] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_in_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x233] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_towards?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x234] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_towards?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x235] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_towards")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x236] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_towards")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x237] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_towards_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x238] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_towards_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x239] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_swarm_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_swarm_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_swarm_from")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_swarm_from")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_pause")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_pause")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_grenade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x240] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_grenade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x241] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x242] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x243] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_jump")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x244] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_jump")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x245] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_jump_to_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x246] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_jump_to_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x247] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vocalize")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x248] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vocalize")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x249] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x24A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x24B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x24C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x24D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x24E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x24F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_action")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x250] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_action")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x251] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_action_at_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x252] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_action_at_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x253] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_action_at_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x254] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_action_at_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x255] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x256] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x257] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x258] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x259] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x25A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x25B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x25C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x25D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x25E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x25F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x260] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x261] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_line")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x262] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_line")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x265] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_deploy_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x266] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_deploy_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x267] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_approach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x268] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_approach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x269] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_approach_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x26A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_approach_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x26B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x26C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x26D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x26E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x26F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_set_style")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Style),
                },
                [0x270] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_set_style")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Style),
                },
                [0x271] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_force_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x272] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_force_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x273] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_targeting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x274] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_targeting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x275] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_looking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x276] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_looking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x277] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_moving")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x278] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_moving")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x279] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_suppress_activity_termination")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_suppress_activity_termination")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_look")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x280] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_look")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x281] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_look_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x282] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_look_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x283] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_look_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x284] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_look_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x285] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_aim")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x286] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_aim")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x287] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_aim_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x288] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_aim_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x289] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_aim_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x28A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_aim_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x28B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x28C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x28D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_face_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x28E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_face_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x28F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_face_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x290] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_face_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x291] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x292] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x293] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x294] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x295] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x296] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x297] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x298] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x299] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_lower_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x29A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_lower_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x29B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vehicle_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vehicle_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vehicle_boost")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vehicle_boost")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_turn_sharpness?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2A2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_turn_sharpness?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2A3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_set_pathfinding_radius")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2A6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_set_pathfinding_radius")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2A7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_ignore_obstacles")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_ignore_obstacles")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_approach_stop"),
                [0x2AA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_approach_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x2AB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_movement_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2AC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_movement_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2AD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2AE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2AF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2B1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_walk")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2B2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_walk")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2B3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_posture_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_posture_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2B5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_posture_exit"),
                [0x2B6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_posture_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x2B7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stow"),
                [0x2B8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_stow")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x2B9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_draw"),
                [0x2BA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_draw")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x2BB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x2BC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x2BD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stop_custom_animation"),
                [0x2BE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_stop_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x2BF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stop_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x2C0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_stop_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x2C1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_player_melee")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2C2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_player_melee")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2C3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_melee_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2C4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_melee_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2C7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_control")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2C8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2C9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x2CA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x2CB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x2CC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_with_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2CD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2CE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2CF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_loop_offset")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2D0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_predict_resources_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2D1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_predict_resources_at_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2D2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_first_person")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x2D3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_cinematic"),
                [0x2D4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_cinematic_scene")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x2D5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_place_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x2D6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_place_worldspace"),
                [0x2D7] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "camera_time"),
                [0x2D8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_field_of_view")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2DA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_camera_set_easing_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2DB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_print")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2DC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_pan")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2DD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_pan")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2DE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_save"),
                [0x2DF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_load"),
                [0x2E0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_save_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2E1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_load_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2E2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "director_debug_camera")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2E3] = new ScriptInfo(HsType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get"),
                [0x2E4] = new ScriptInfo(HsType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get_real"),
                [0x2E5] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "game_insertion_point_get"),
                [0x2E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_insertion_point_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pvs_set_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x2E8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pvs_set_camera")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pvs_clear"),
                [0x2EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "players_unzoom_all"),
                [0x2EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_enable_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_disable_movement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2EF] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_flashlight_on"),
                [0x2F0] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_active_camouflage_on"),
                [0x2F1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_camera_control")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_action_test_reset"),
                [0x2F3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_primary_trigger"),
                [0x2F4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_grenade_trigger"),
                [0x2F5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_vision_trigger"),
                [0x2F6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_rotate_weapons"),
                [0x2F7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_rotate_grenades"),
                [0x2F8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_melee"),
                [0x2F9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_action"),
                [0x2FA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_accept"),
                [0x2FB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_cancel"),
                [0x2FC] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_up"),
                [0x2FD] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_down"),
                [0x2FE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_left"),
                [0x2FF] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_right"),
                [0x300] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_all_directions"),
                [0x301] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_move_relative_all_directions"),
                [0x302] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_cinematic_skip"),
                [0x303] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_start"),
                [0x304] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_back"),
                [0x305] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_dpad_left"),
                [0x306] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_dpad_right"),
                [0x307] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_dpad_up"),
                [0x308] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_dpad_down"),
                [0x309] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_x"),
                [0x30A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_y"),
                [0x30B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_left_shoulder"),
                [0x30C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_right_shoulder"),
                [0x30D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_action_test_reset")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x30E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_primary_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x30F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_grenade_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x310] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_vision_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x311] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_rotate_weapons")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x312] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_rotate_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x313] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_melee")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x314] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_action")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x315] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_accept")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x316] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_cancel")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x317] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_up")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x318] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_down")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x319] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_left")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_right")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_all_directions")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_move_relative_all_directions")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_cinematic_skip")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_back")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x320] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_left")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x321] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_right")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x322] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_up")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x323] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_down")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x324] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_x")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x325] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_y")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x326] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_left_shoulder")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x327] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_right_shoulder")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x328] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player0_looking_up"),
                [0x329] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player0_looking_down"),
                [0x32A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player0_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x32B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player1_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x32C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player2_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x32D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player3_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x32E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_lookstick_forward"),
                [0x32F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_lookstick_backward"),
                [0x330] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_teleport_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x331] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "map_reset"),
                [0x333] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "switch_zone_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x334] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "switch_zone_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ZoneSet),
                },
                [0x335] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "current_zone_set"),
                [0x336] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "current_zone_set_fully_active"),
                [0x338] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "crash")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x339] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "version"),
                [0x33A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "status"),
                [0x33B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "record_movie")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x33C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "record_movie_distributed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x33D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "screenshot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x33F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "screenshot_big")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x340] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "screenshot_big_jittered")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x345] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "main_menu"),
                [0x346] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "main_halt"),
                [0x348] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "map_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x349] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_multiplayer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x34A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_splitscreen")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x34B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_difficulty")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.GameDifficulty),
                },
                [0x351] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x354] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_rate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x358] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_memory"),
                [0x359] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_memory_by_file"),
                [0x35A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_memory_for_file")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x35B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_tags"),
                [0x35C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "tags_verify_all"),
                [0x36A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_control_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x36B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_control_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x36D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_break_on_vocalization")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x36E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "fade_in")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x36F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "fade_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x370] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_start"),
                [0x371] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_stop"),
                [0x372] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_skip_start_internal"),
                [0x373] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_skip_stop_internal"),
                [0x374] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_show_letterbox")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x375] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_show_letterbox_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x376] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_title")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x377] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_title_delayed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x378] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_suppress_bsp_object_creation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x379] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_subtitle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x37A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicDefinition),
                },
                [0x37B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_shot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x37D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_early_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x37E] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "cinematic_get_early_exit"),
                [0x380] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_object_create")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x381] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_object_create_cinematic_anchor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x382] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_object_destroy")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x383] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_destroy"),
                [0x384] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_clips_initialize_for_shot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x385] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_clips_destroy"),
                [0x386] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lights_initialize_for_shot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x387] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lights_destroy"),
                [0x388] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lights_destroy_shot"),
                [0x389] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_light_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicLightprobe),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x38A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_light_object_off")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x38B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lighting_rebuild_all"),
                [0x38E] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "cinematic_object_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x390] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_briefing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x391] = new ScriptInfo(HsType.Halo3ODSTValue.CinematicDefinition, "cinematic_tag_reference_get_cinematic")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x392] = new ScriptInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition, "cinematic_tag_reference_get_scene")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x393] = new ScriptInfo(HsType.Halo3ODSTValue.Effect, "cinematic_tag_reference_get_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x394] = new ScriptInfo(HsType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x395] = new ScriptInfo(HsType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound2")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x396] = new ScriptInfo(HsType.Halo3ODSTValue.LoopingSound, "cinematic_tag_reference_get_looping_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x397] = new ScriptInfo(HsType.Halo3ODSTValue.AnimationGraph, "cinematic_tag_reference_get_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x398] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "cinematic_tag_reference_get_?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x399] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_fade_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x39D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x39E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x39F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_music")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3A0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x3A1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_stop_music")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3A3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3A4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_object_no_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3A5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object_no_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3A6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_play_cortana_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3A7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "attract_mode_start"),
                [0x3A8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "attract_mode_set_seconds")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3A9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_level_advance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3AA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_won"),
                [0x3AB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_lost")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3AC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_revert"),
                [0x3AD] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_is_cooperative"),
                [0x3AE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_is_playtest"),
                [0x3AF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_can_use_flashlights")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_and_quit"),
                [0x3B1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_unsafe"),
                [0x3B2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_insertion_point_unlock")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x3B3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_insertion_point_lock")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x3B9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "achievement_grant_to_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3BA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "achievement_grant_to_all_players")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3D9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load"),
                [0x3DA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x3DB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_save"),
                [0x3DC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_save_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x3DD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load_game"),
                [0x3DE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load_game_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x3DF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_regular_upload_to_debug_server")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3E0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_set_upload_option")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x3E3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_safe_to_save"),
                [0x3E4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_safe_to_speak"),
                [0x3E5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_all_quiet"),
                [0x3E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save"),
                [0x3E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_cancel"),
                [0x3E8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_no_timeout"),
                [0x3E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_immediate"),
                [0x3EA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_saving"),
                [0x3EB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_reverted"),
                [0x3EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_set_tag_parameter_unsafe")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_cinematic")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3F1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_with_subtitle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3F3] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "sound_impulse_language_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3F4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3F5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_3d")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3F6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_mark_as_outro")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3F8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                },
                [0x3F9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3FA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                },
                [0x3FB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_stop_immediately")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                },
                [0x3FC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_set_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3FD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_set_alternate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3FE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_loop_spam"),
                [0x3FF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_show_channel")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x400] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_debug_sound_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x401] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_sounds_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x402] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_set_gain")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x403] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_set_gain_db")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x404] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_enable_ducker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x405] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_sound_environment_parameter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x406] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_set_global_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x407] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_set_global_effect_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x408] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_auto_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x409] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_hover")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x40A] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "vehicle_count_bipeds_killed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x40B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "biped_ragdoll")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x40D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_show_training_text")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x40E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_set_training_text")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x40F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_enable_training")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x410] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_flashlight"),
                [0x411] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_crouch"),
                [0x412] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_stealth"),
                [0x413] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_?"),
                [0x414] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_jump"),
                [0x416] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_activate_team_nav_point_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x417] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_deactivate_team_nav_point_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x418] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_cortana_suck")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x41B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_cortana_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x41E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_crosshair")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x41F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x420] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x421] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_messages")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x422] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_motion_sensor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x423] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_spike_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x424] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_fire_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x425] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_compass")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x426] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_cinematic_fade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x427] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_display_pda_minimap_message")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x428] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_display_pda_objectives_message")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x429] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_display_pda_communications_message")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x42B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_object_navpoint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x42C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_bonus_round_show_timer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x42D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_bonus_round_start_timer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x42E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_bonus_round_set_timer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x42F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cls"),
                [0x430] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_overflow_suppression")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x431] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x432] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_hide")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x433] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_show_all"),
                [0x434] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_hide_all"),
                [0x435] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_list"),
                [0x436] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_translation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x437] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_rotation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x438] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_rumble")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x439] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_translation_for_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_rotation_for_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_rumble_for_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_start_for_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_stop_for_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x440] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "time_code_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x441] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "time_code_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x442] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "time_code_reset"),
                [0x443] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "script_screen_effect_set_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x444] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x445] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x446] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x447] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_stop"),
                [0x448] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_near_clip_distance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x449] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_far_clip_distance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x44A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_atmosphere_fog")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x44B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "atmosphere_fog_override_fade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x44C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "motion_blur")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x44D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_weather")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x44E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_patchy_fog")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x450] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_environment_map_attenuation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x451] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_environment_map_bitmap")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Bitmap),
                },
                [0x452] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_bitmap"),
                [0x453] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_environment_map_tint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x454] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_tint"),
                [0x455] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_layer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x456] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_has_skills"),
                [0x457] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_has_mad_secret_skills")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x458] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "controller_invert_look"),
                [0x459] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "controller_look_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x45A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "controller_set_look_invert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x45B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "controller_get_look_invert"),
                [0x45C] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "controller_unlock_solo_levels")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x4AF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_clear"),
                [0x4B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_show_up_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_finish_up_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_finish")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_unavailable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_secondary_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_secondary_finish")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_secondary_unavailable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "input_suppress_rumble")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4D5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_delegate_host")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4D6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_delegate_leader")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4D7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_map_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4D9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_campaign_difficulty")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x4DA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_player_color")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_bink_movie")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_bink_movie_from_tag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.BinkDefinition),
                },
                [0x4ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_credits"),
                [0x4EE] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "bink_time"),
                [0x4EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_global_doppler_factor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_global_mixbin_headroom")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4F1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_sound_environment_source_parameter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_set_mission_segment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4F3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_display_mission_segment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4F4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_insert"),
                [0x4F6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_upload"),
                [0x4F7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_playback")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4F9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x526] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "bug_now")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x527] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "bug_now_lite")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x528] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "bug_now_auto")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x529] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "object_list_children")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x52A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "voice_set_outgoing_channel_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x52B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "voice_set_voice_repeater_peer_index")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x532] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x533] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_start_smooth")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x534] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x535] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_restart")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x536] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "interpolator_is_active")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x537] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "interpolator_is_finished")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x538] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_set_current_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x539] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_current_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x53A] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_start_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x53B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_final_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x53C] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_current_phase")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x53D] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_current_time_fraction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x53E] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_start_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x53F] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_final_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x540] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x541] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_fraction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x542] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x543] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_delta")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x544] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "interpolator_stop_all"),
                [0x545] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "interpolator_restart_all"),
                [0x546] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "interpolator_flip"),
                [0x548] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "animation_cache_stats_reset"),
                [0x549] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_clone_players_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x54A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_move_attached_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x54B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_enable_ghost_effects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x54C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_global_sound_environment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x54D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "reset_dsp_image"),
                [0x54E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_cinematic_skip"),
                [0x54F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_outro_start"),
                [0x550] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_enable_ambience_details")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x551] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x552] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_reset")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x553] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_blur_amount")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x554] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_threshold")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x555] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_brightness")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x556] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_box_factor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x557] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_max_factor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x558] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_silver_bullet")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x559] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_only")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x55A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_high_res")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x55B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_brightness_alpha")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x55C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_max_factor_alpha")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x55D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cache_block_for_one_frame"),
                [0x55E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_suppress_ambience_update_on_revert"),
                [0x562] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_exposure_fade_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x563] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_exposure")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x564] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_autoexposure_instant")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x566] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_depth_of_field_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x567] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_depth_of_field")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x56B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_disable"),
                [0x56C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_enable"),
                [0x56D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "predict_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x56E] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "mp_players_by_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x56F] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "mp_active_player_count_by_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x571] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_game_won")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x573] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_ai_allegiance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x574] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_allegiance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x57E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_object_create_anew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x57F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_object_destroy")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x59A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "predict_bink_movie")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x59B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "predict_bink_movie_from_tag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.BinkDefinition),
                },
                [0x59D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5A0] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "game_coop_player_count"),
                [0x5A5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "add_recycling_volume")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5AD] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "game_tick_get"),
                [0x5BC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "saved_film_set_playback_game_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5C0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "designer_zone_activate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DesignerZone),
                },
                [0x5C1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "designer_zone_deactivate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DesignerZone),
                },
                [0x5C2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_always_active")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x5C8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_persistent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x5EA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_performance_throttle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5EB] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "get_performance_throttle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_zone_activate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_zone_deactivate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_limit_lipsync_to_mouth_only")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x5F9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "prepare_to_switch_to_zone_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ZoneSet),
                },
                [0x5FA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_zone_activate_for_debugging")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5FB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_play_random_ping")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x5FC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_fade_out_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5FD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_fade_in_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5FE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_control_fade_out_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5FF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_control_fade_in_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x600] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_lock_gaze")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x601] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_unlock_gaze")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x602] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_scale_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x606] = new ScriptInfo(HsType.Halo3ODSTValue.BinkDefinition, "cinematic_tag_reference_get_bink")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x609] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_custom_animation_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x60A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x60B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "film_manager_set_reproduction_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x60C] = new ScriptInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition, "cortana_effect_tag_reference_get_scene")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x60F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "biped_force_ground_fitting_on")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x610] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_chud_objective")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x611] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_cinematic_title")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x612] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "terminal_is_being_read"),
                [0x613] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "terminal_was_accessed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x614] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "terminal_was_completed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x615] = new ScriptInfo(HsType.Halo3ODSTValue.Weapon, "unit_get_primary_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x618] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_award_level_complete_achievements"),
                [0x61A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_safe_to_respawn")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x61B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cortana_effect_kill"),
                [0x61E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_cortana_effect_cinematic"),
                [0x61F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_migrate_infanty")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x620] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_cinematic_motion_blur")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x621] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dont_do_avoidance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x622] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_clean_up")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x623] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_erase_inactive")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x624] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_survival_cleanup")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x625] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "stop_bink_movie"),
                [0x626] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_credits_unskippable"),
                [0x62B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_debug_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x62C] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "cinematic_scripting_get_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x62E] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "gp_integer_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x62F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "gp_integer_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x630] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "gp_boolean_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x631] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "gp_boolean_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x637] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_screen_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x638] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_stop_screen_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x639] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_level_prepare")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x63A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "prepare_game_level")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x63B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_activate_beacon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x63D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_activate_marker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x63E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_activate_marker_named")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x63F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "pda_beacon_is_selected")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x640] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_inside_active_beacon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x64B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_set_user_input_constraints")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x64C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "skull_primary_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PrimarySkull),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x64D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "skull_secondary_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.SecondarySkull),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x651] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_respawn_dead_players"),
                [0x652] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "survival_mode_lives_get"),
                [0x653] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_lives_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x654] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "survival_mode_set_get"),
                [0x655] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_set_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x656] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "survival_mode_round_get"),
                [0x657] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_round_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x658] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "survival_mode_wave_get"),
                [0x659] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_wave_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x65A] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "survival_mode_set_multiplier_get"),
                [0x65B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_set_multiplier_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x65C] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "survival_mode_bonus_multiplier_get"),
                [0x65D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_bonus_multiplier_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x65F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_set_rounds_per_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x660] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_set_waves_per_round")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x662] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_event_new")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x663] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_begin_new_set"),
                [0x664] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_begin_new_round"),
                [0x665] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_begin_new_wave"),
                [0x666] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_award_hero_medal"),
                [0x667] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "campaign_metagame_get_player_score")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x668] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_add_footprint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x66A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "pda_is_active_deterministic")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x66B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_set_pda_enabled")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x66C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_set_nav_enabled")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x66D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_set_fourth_wall_enabled")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x66E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_set_objectives_enabled")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x66F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_force_pda")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x670] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_close_pda")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x671] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_set_footprint_dead_zone")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x674] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_play_arg_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x675] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_stop_arg_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x676] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_set_look_training_hack")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x677] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_set_active_pda_definition")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x678] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_arg_has_been_touched_by_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x67B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_input_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x67F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_input_enable_a")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x680] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_input_enable_dismiss")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x681] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_input_enable_x")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x682] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_input_enable_y")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x683] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_input_enable_dpad")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x684] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pda_input_enable_analog_sticks")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x68A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "achievement_post_chud_progression")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x68B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_vision_mode_render_default")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x68C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_navpoint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x690] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_confirm_message")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x691] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_confirm_cancel_message")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x692] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_confirm_y_button")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x693] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "player_get_kills_by_type")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x694] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_flashlight_on")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x695] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "clear_command_buffer_cache_from_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x696] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_resume")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x697] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_bonus_round_set_target_score")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                }
            },
            [CacheVersion.HaloOnline106708] = new Dictionary<int, ScriptInfo>
            {
                [0x000] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "begin"),
                [0x001] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "begin_random"),
                [0x002] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "if"),
                [0x003] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "cond"),
                [0x004] = new ScriptInfo(HsType.Halo3ODSTValue.Passthrough, "set"),
                [0x005] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "and"),
                [0x006] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "or"),
                [0x007] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "+"),
                [0x008] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "-"),
                [0x009] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "*"),
                [0x00A] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "/"),
                [0x00B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "inc?"),
                [0x00C] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "min"),
                [0x00D] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "max"),
                [0x00E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "="),
                [0x00F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "!="),
                [0x010] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, ">"),
                [0x011] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "<"),
                [0x012] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, ">="),
                [0x013] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "<="),
                [0x014] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sleep"),
                [0x015] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sleep_forever"),
                [0x016] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "sleep_until"),
                [0x017] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "wake"),
                [0x018] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "inspect"),
                [0x019] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "unit"),
                [0x01A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "evaluate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Script),
                },
                [0x01B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "not")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x01C] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "pin")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x01D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "print")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x01E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "log_print")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x021] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_scripting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x025] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "breakpoint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x026] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_active_scripts"),
                [0x027] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "get_executing_running_thread"),
                [0x028] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_thread")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x029] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "script_started")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x02A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "script_finished")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x02B] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "players"),
                [0x02C] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "player_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x02D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_volume_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "kill_volume_disable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x02F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "volume_teleport_players_not_inside")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x030] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x031] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x032] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_objects_all")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x033] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_players")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x034] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "volume_test_players_all")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x035] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "volume_return_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                },
                [0x036] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "volume_return_objects_by_type")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x037] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "zone_set_trigger_volume_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x038] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "list_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x039] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "list_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x03A] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "list_count_not_dead")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x03B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x03C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_random")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x03D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_at_ai_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x03E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_on_object_marker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x03F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "effect_new_on_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Effect),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x040] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_new")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x041] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_object_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x042] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_objects_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x043] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x044] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x045] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_players")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Damage),
                },
                [0x046] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "soft_ceiling_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x047] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x048] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_clone")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x049] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_anew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x04A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_if_necessary")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x04B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x04C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_clone_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x04D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_anew_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x04E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_folder")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Folder),
                },
                [0x04F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_create_folder_anew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Folder),
                },
                [0x050] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x051] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_containing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x052] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_all"),
                [0x053] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_type_mask")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x054] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_delete_by_definition")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x055] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_destroy_folder")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Folder),
                },
                [0x056] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_hide")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x057] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shadowless")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x058] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "object_buckling_magnitude_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x059] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_function_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x05A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_function_variable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x05B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_clear_function_variable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x05C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_clear_all_function_variables")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x05D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_dynamic_simulation_disable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x05E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_phantom_power")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x05F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_wake_physics")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x060] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_ranged_attack_inhibited")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x061] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_melee_attack_inhibited")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x062] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_dump_memory"),
                [0x063] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "object_get_health")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x064] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "object_get_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x065] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x066] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_physics")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x067] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "object_get_parent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x068] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_attach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x069] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "object_at_marker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x06A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_detach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x06B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x06C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x06D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_inertia_tensor_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x06E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_collision_damage_armor_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x06F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_velocity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x070] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_deleted_when_deactivated")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x071] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "object_model_target_destroyed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x072] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "object_model_targets_destroyed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x073] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_damage_damage_section")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x074] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cannot_die")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x075] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "object_vitality_pinned")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x076] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "garbage_collect_now"),
                [0x077] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "garbage_collect_unsafe"),
                [0x078] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "garbage_collect_multiplayer"),
                [0x079] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cannot_take_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x07A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_can_take_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x07B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cinematic_lod")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x07C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cinematic_collision")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x07D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_cinematic_visibility")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x07E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x07F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_predict_high")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x080] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objects_predict_low")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x081] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_type_predict_high")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x082] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_type_predict_low")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x083] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_type_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x084] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x085] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_teleport_to_ai_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x086] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_facing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x087] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x088] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_normalized")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x089] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_stun")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x08A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_shield_stun_infinite")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x08B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_permutation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x08C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_variant")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x08D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_region_state")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ModelState),
                },
                [0x08E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "objects_can_see_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x08F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "objects_can_see_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x090] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "objects_distance_to_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x091] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "objects_distance_to_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x092] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "map_info"),
                [0x093] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "position_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x094] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "shader_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Shader),
                },
                [0x095] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "bitmap_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Bitmap),
                },
                [0x096] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "script_recompile"),
                [0x097] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "script_doc"),
                [0x098] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "help")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x099] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "game_engine_objects"),
                [0x09A] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "random_range")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x09B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "real_random_range")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x09C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_constants_reset"),
                [0x09D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_set_gravity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x09E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_set_velocity_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x09F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "physics_disable_character_ground_adhesion_forces")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0A0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_debug_start"),
                [0x0A1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_dump_world")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0A2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_dump_world_close_movie"),
                [0x0A3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_profile_start"),
                [0x0A4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_profile_end"),
                [0x0A6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "havok_reset_allocated_state"),
                [0x0A7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "breakable_surfaces_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0A8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "breakable_surfaces_reset"),
                [0x0A9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "recording_play")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "recording_play_and_delete")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "recording_play_and_hover")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneRecording),
                },
                [0x0AC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "recording_kill")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0AD] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "recording_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0AE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "render_lights")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_lights_enable_cinematic_shadow")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0B1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_set_object_marker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_attach_to_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0B5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_target_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0B7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_on"),
                [0x0B8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_off"),
                [0x0B9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_set_aspect_ratio")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0BA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_set_resolution")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x0BF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "texture_camera_enable_dynamic_lights")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0C4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_camera_attach_to_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0C5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_camera_target_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0C7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_camera_highlight_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0D3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_all_fog_planes")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0D4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_all_cluster_errors")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0D5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_line_opacity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0D6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_text_opacity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0D7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_debug_structure_opacity")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0E2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_postprocess_color_tweaking_reset"),
                [0x0E3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0E4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x0E5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_relative_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0E8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_relative_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_idle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                },
                [0x0EA] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "scenery_get_animation_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                },
                [0x0EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_can_blink")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_active_camo")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x0ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_open")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_close")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_kill")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_kill_silent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_is_emitting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F2] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "unit_get_custom_animation_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_stop_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0F4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0F5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0F6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0F7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_relative_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x0F8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "custom_animation_list")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0F9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_custom_animation_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x0FB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_is_playing_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x0FC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_custom_animations_hold_on_last_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x0FD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_custom_animations_prevent_lipsync_head_movement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x100] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_actively_controlled")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x101] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "unit_get_team_index")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x102] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_aim_without_turning")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x103] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_enterable_by_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x105] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_get_enterable_by_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x106] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_only_takes_damage_from_players_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x107] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_enter_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x108] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_falling_damage_disable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x109] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_in_vehicle_type")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x10B] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "object_get_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x10C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_board_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x10D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_emotion_by_?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x10E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_emotion_by_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x10F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_enable_eye_tracking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x110] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_integrated_flashlight")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x111] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnyTag),
                },
                [0x112] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_enable_vision_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x113] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_in_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x114] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_test_seat_list")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x115] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_test_seat_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x116] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_test_seat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x117] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_prefer_tight_camera_track")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x118] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x119] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_exit_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x11A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_maximum_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x11B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "units_set_maximum_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x11C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_current_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x11D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "units_set_current_vitality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x11E] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "vehicle_load_magic")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x11F] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "vehicle_unload")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x120] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_set_animation_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x121] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "magic_melee_attack"),
                [0x122] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "vehicle_riders")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x123] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "vehicle_driver")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x124] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "vehicle_gunner")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x125] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "unit_get_health")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x126] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "unit_get_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x127] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "unit_get_total_grenade_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x128] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x129] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_weapon_readied")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x12A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_any_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x12B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_has_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x12C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_lower_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x12D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_raise_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x12E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_drop_support_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x132] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_animation_forced_seat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x133] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_doesnt_drop_items")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                },
                [0x134] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_impervious")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x135] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_suspended")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x136] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_add_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StartingProfile),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x137] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "weapon_hold_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Weapon),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x138] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "weapon_enable_warthog_chaingun_light")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x139] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_set_never_appears_locked")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x13A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_set_power")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x13B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "device_get_power")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                },
                [0x13C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_set_position")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x13D] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "device_get_position")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                },
                [0x13E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_set_position_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x13F] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "device_group_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                },
                [0x140] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_group_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x141] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_group_set_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x142] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_one_sided_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x144] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_operates_automatically_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x145] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_closes_automatically_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x146] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_group_change_only_once_more_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DeviceGroup),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x147] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_set_position_track")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x148] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_set_overlay_track")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x149] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_animate_position")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x14A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "device_animate_overlay")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x14B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_all_powerups"),
                [0x14C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_all_weapons"),
                [0x14D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_all_vehicles"),
                [0x14E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_teleport_to_camera"),
                [0x14F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_active_camouflage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x150] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheat_active_camouflage_by_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x151] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cheats_load"),
                [0x153] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "drop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x155] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x156] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_enabled"),
                [0x157] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x158] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x159] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_player_dialogue_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x15B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_fast_and_dumb")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x15C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_log_reset"),
                [0x15D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_log_dump")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x15E] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "ai_get_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x15F] = new ScriptInfo(HsType.Halo3ODSTValue.Unit, "ai_get_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x160] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_get_squad")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x161] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_get_turret_ai")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x162] = new ScriptInfo(HsType.Halo3ODSTValue.PointReference, "ai_random_smart_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x163] = new ScriptInfo(HsType.Halo3ODSTValue.PointReference, "ai_nearest_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x164] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "ai_get_point_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x165] = new ScriptInfo(HsType.Halo3ODSTValue.PointReference, "ai_point_set_get_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x166] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x167] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_place")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x168] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_place_in_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x169] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_cannot_die")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x16A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_vitality_pinned")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x16B] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_index_from_spawn_formation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x16C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_resurrect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x16D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_kill")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x16E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_kill_silent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x16F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_erase")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x170] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_erase_all"),
                [0x171] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_disposable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x172] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_select")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x173] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_deselect"),
                [0x174] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_deaf")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x175] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_blind")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x176] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_weapon_up")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x177] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_flood_disperse")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x178] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_magically_see")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x179] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_magically_see_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x17A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_active_camo")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x17B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_suppress_combat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x17D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_grunt_kamikaze")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x17E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_migrate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x17F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allegiance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x180] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allegiance_remove")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x181] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allegiance_break")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x182] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_braindead")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x183] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_braindead_by_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x184] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_disregard")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x185] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_prefer_target")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectList),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x186] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_prefer_target_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x187] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_prefer_target_ai")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x188] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x189] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_targeting_group")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x18A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_teleport_to_starting_location_if_outside_bsp")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x18B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_teleport_to_spawn_point_if_outside_bsp")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x18C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x18D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_bring_forward")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x18E] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_migrate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x190] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "biped_morph")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x191] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_renew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x192] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_force_active")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x193] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_force_active_by_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x194] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_playfight")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x195] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_reconnect"),
                [0x196] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_berserk")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x197] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_set_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x198] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_allow_dormant")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x199] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_is_attacking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19A] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_fighting_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19B] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_living_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19C] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_living_fraction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19D] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_in_vehicle_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19E] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_body_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x19F] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_strength")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A0] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_swarm_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A1] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_nonswarm_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A2] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "ai_actors")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_allegiance_broken")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                },
                [0x1A4] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_spawn_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1A5] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "object_get_ai")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x1B0] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_set_task")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1B1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_set_objective")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1B2] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_task_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1B3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_set_task_condition")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1B4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_leadership")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1B5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_leadership_all")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1B6] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_task_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1B7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_reset_objective")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1B8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_squad_patrol_objective_disallow")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1B9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "generate_pathfinding"),
                [0x1BA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_render_paths_all"),
                [0x1BB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_activity_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1BC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_activity_abort")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1BD] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "ai_vehicle_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1BE] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_starting_location")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1BF] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_spawn_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1C0] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_vehicle_get_squad_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1C1] = new ScriptInfo(HsType.Halo3ODSTValue.Vehicle, "ai_vehicle_get_from_squad")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1C2] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve_seat")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1C3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_vehicle_reserve")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1C4] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "ai_player_get_vehicle_squad")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x1C5] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_vehicle_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1C6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_carrying_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1C7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_in_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x1C8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_in_vehicle?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x1C9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_player_any_needs_vehicle"),
                [0x1CA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1CB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x1CC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1CD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_enter_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x1CE] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_enter_squad_vehicles")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1CF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x1D0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1D1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vehicle_overturned")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x1D2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_flip")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x1D3] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1D4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1D5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1D6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_create")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1D7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "flock_delete")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1DB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_verify_tags"),
                [0x1DC] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "ai_wall_lean")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x1DD] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x1DE] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line_at_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x1DF] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line_on_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x1E0] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "ai_play_line_on_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x1E1] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1E2] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "ai_play_line_on_point_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1E3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_time_pause")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1E4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_points")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1E5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_primary_skull")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PrimarySkull),
                },
                [0x1E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_secondary_skull")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.SecondarySkull),
                },
                [0x1E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "campaign_metagame_award_event")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1E8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_metagame_enabled"),
                [0x1E9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_survival_enabled"),
                [0x1EA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_easy"),
                [0x1EB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_normal"),
                [0x1EC] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_heroic"),
                [0x1ED] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "campaign_is_finished_legendary"),
                [0x1EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_run_command_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1EF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_queue_command_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stack_command_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x1F1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_reserve")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1F3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F4] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1F9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_cast")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x1FA] = new ScriptInfo(HsType.Halo3ODSTValue.Ai, "vs_role")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1FB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1FC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1FD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x1FE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x1FF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x200] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x201] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x202] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x203] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x204] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_alert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x205] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x206] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_damage")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x207] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x208] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x209] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x20A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_abort_on_vehicle_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x20B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_set_cleanup_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Script),
                },
                [0x20C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_release")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x20D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_release_all"),
                [0x20E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "cs_command_script_running")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x20F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "cs_command_script_queued")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiCommandScript),
                },
                [0x210] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "cs_number_queued")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x213] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x214] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom_movement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x215] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom_?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x216] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "vs_running_atom_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x217] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x218] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x219] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x21C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x21D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x21F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x220] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x221] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x222] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_fly_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x223] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x224] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x225] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x226] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x227] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x228] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x229] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x22A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_by")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x22B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x22C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_and_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x22D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_and_posture")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x22E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_and_posture")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x22F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_nearest")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x230] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_nearest")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x231] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_in_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x232] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_in_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x233] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_towards?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x234] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_towards?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x235] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_towards")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x236] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_towards")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x237] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_move_towards_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x238] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_move_towards_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x239] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_swarm_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_swarm_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_swarm_from")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_swarm_from")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_pause")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_pause")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x23F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_grenade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x240] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_grenade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x241] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x242] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_equipment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x243] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_jump")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x244] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_jump")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x245] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_jump_to_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x246] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_jump_to_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x247] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vocalize")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x248] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vocalize")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x249] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x24A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x24B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x24C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x24D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x24E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x24F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_action")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x250] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_action")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x251] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_action_at_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x252] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_action_at_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x253] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_action_at_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x254] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_action_at_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x255] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x256] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x257] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x258] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x259] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x25A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x25B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x25C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_death")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x25D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x25E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x25F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x260] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_custom_animation_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x261] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_play_line")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x262] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_play_line")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AiLine),
                },
                [0x265] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_deploy_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x266] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_deploy_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x267] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_approach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x268] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_approach")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x269] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_approach_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x26A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_approach_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x26B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x26C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x26D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x26E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_go_to_vehicle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.UnitSeatMapping),
                },
                [0x26F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_set_style")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Style),
                },
                [0x270] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_set_style")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Style),
                },
                [0x271] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_force_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x272] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_force_combat_status")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x273] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_targeting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x274] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_targeting")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x275] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_looking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x276] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_looking")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x277] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_moving")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x278] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_moving")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x279] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_suppress_activity_termination")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_suppress_activity_termination")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_suppress_dialogue_global?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x27F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_look")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x280] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_look")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x281] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_look_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x282] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_look_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x283] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_look_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x284] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_look_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x285] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_aim")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x286] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_aim")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x287] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_aim_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x288] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_aim_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x289] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_aim_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x28A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_aim_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x28B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x28C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_face")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x28D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_face_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x28E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_face_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x28F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_face_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x290] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_face_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x291] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x292] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x293] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x294] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x295] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x296] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x297] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x298] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_shoot_secondary_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x299] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_lower_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x29A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_lower_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x29B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vehicle_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vehicle_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vehicle_speed_instantaneous")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x29F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_vehicle_boost")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_vehicle_boost")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_turn_sharpness?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2A2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_turn_sharpness?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2A3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_enable_pathfinding_failsafe")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_set_pathfinding_radius")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2A6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_set_pathfinding_radius")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2A7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_ignore_obstacles")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_ignore_obstacles")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2A9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_approach_stop"),
                [0x2AA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_approach_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x2AB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_movement_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2AC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_movement_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2AD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2AE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2AF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_crouch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2B1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_walk")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2B2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_walk")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2B3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_posture_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_posture_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2B5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_posture_exit"),
                [0x2B6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_posture_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x2B7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stow"),
                [0x2B8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_stow")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x2B9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_draw"),
                [0x2BA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_draw")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x2BB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x2BC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_teleport")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                },
                [0x2BD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stop_custom_animation"),
                [0x2BE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_stop_custom_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x2BF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_stop_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x2C0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_stop_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x2C1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_player_melee")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2C2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_player_melee")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2C3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cs_melee_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2C4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vs_melee_direction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2C7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_control")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2C8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2C9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x2CA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x2CB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x2CC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_with_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2CD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2CE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_?boolean")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2CF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_animation_relative_with_speed_?boolean_real")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2D0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_predict_resources_at_frame")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x2D1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_predict_resources_at_point")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2D2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_first_person")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x2D3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_cinematic"),
                [0x2D4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_cinematic_scene")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x2D5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_place_relative")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x2D6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_place_worldspace"),
                [0x2D7] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "camera_time"),
                [0x2D8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_field_of_view")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2DA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_camera_set_easing_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2DB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_print")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2DC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_pan")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2DD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_pan")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x2DE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_save"),
                [0x2DF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_load"),
                [0x2E0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_save_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2E1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_camera_load_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x2E2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "director_debug_camera")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2E3] = new ScriptInfo(HsType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get"),
                [0x2E4] = new ScriptInfo(HsType.Halo3ODSTValue.GameDifficulty, "game_difficulty_get_real"),
                [0x2E5] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "game_insertion_point_get"),
                [0x2E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_insertion_point_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x2E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pvs_set_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x2E8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pvs_set_camera")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x2E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "pvs_clear"),
                [0x2EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "players_unzoom_all"),
                [0x2EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_enable_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_disable_movement")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2F1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_flashlight_on"),
                [0x2F2] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_active_camouflage_on"),
                [0x2F3] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_camera_control")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x2F4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_action_test_reset"),
                [0x2F5] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_jump"),
                [0x2F6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_primary_trigger"),
                [0x2F7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_grenade_trigger"),
                [0x2F8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_zoom"),
                [0x2F9] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_rotate_weapons"),
                [0x2FA] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_rotate_grenades"),
                [0x2FB] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_melee"),
                [0x2FC] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_action"),
                [0x2FD] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_accept"),
                [0x2FE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_cancel"),
                [0x2FF] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_up"),
                [0x300] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_down"),
                [0x301] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_left"),
                [0x302] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_right"),
                [0x303] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_look_relative_all_directions"),
                [0x304] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_move_relative_all_directions"),
                [0x305] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_back"),
                [0x306] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_dpad_left"),
                [0x307] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_dpad_right"),
                [0x308] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_dpad_up"),
                [0x309] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_dpad_down"),
                [0x30A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_x"),
                [0x30B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_y"),
                [0x30C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_left_shoulder"),
                [0x30D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_right_shoulder"),
                [0x30F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_action_test_reset")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x310] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_grenade_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x311] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_vision_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x312] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_rotate_weapons")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x313] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_rotate_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x314] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_melee")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x315] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_action")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x316] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_accept")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x317] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_cancel")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x318] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_up")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x319] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_down")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_left")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_right")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_look_relative_all_directions")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_move_relative_all_directions")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_back")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x31F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_left")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x320] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_right")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x321] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_up")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x322] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_dpad_down")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x323] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_x")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x324] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_y")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x325] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_left_shoulder")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x326] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_action_test_right_shoulder")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x327] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player0_looking_up"),
                [0x328] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player0_looking_down"),
                [0x329] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player0_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x32A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player1_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x32B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player2_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x32C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player3_set_pitch")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x32D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_lookstick_forward"),
                [0x32E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_action_test_lookstick_backward"),
                [0x32F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_teleport_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x330] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "map_reset"),
                [0x332] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "switch_zone_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x333] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "switch_zone_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ZoneSet),
                },
                [0x334] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "current_zone_set"),
                [0x335] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "current_zone_set_fully_active"),
                [0x337] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "crash")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x338] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "version"),
                [0x339] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "status"),
                [0x33A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "record_movie")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x33B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "record_movie_distributed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x33C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "screenshot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x33E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "screenshot_big")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x33F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "screenshot_big_jittered")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x344] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "main_menu"),
                [0x345] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "main_halt"),
                [0x34A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "map_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x34C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_multiplayer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x34D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_splitscreen")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x34E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_difficulty")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.GameDifficulty),
                },
                [0x354] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x357] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_rate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x35B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_memory"),
                [0x35C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_memory_by_file"),
                [0x35D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_memory_for_file")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x35E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_tags"),
                [0x35F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "tags_verify_all"),
                [0x36D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_control_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x36E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "damage_control_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x370] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dialogue_break_on_vocalization")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x371] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "fade_in")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x372] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "fade_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x373] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_start"),
                [0x374] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_stop"),
                [0x375] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_skip_start_internal"),
                [0x376] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_skip_stop_internal"),
                [0x377] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_show_letterbox")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x378] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_show_letterbox_immediate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x379] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_title")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x37A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_title_delayed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x37B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_suppress_bsp_object_creation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x37C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_subtitle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x37D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicDefinition),
                },
                [0x37E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_shot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x380] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_early_exit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x381] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "cinematic_get_early_exit"),
                [0x383] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_object_create")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x384] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_object_create_cinematic_anchor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x385] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_object_destroy")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x386] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_destroy"),
                [0x387] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_clips_initialize_for_shot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x388] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_clips_destroy"),
                [0x389] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lights_initialize_for_shot")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x38A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lights_destroy"),
                [0x38B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lights_destroy_shot"),
                [0x38C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_light_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CinematicLightprobe),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneCameraPoint),
                },
                [0x38D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_light_object_off")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x38E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lighting_rebuild_all"),
                [0x391] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "cinematic_object_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x393] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_briefing")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x394] = new ScriptInfo(HsType.Halo3ODSTValue.CinematicDefinition, "cinematic_tag_reference_get_cinematic")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x395] = new ScriptInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition, "cinematic_tag_reference_get_scene")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x396] = new ScriptInfo(HsType.Halo3ODSTValue.Effect, "cinematic_tag_reference_get_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x397] = new ScriptInfo(HsType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x398] = new ScriptInfo(HsType.Halo3ODSTValue.Sound, "cinematic_tag_reference_get_sound2")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x399] = new ScriptInfo(HsType.Halo3ODSTValue.LoopingSound, "cinematic_tag_reference_get_looping_sound")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x39A] = new ScriptInfo(HsType.Halo3ODSTValue.AnimationGraph, "cinematic_tag_reference_get_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x39B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "cinematic_tag_reference_get_?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x39C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_fade_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x3A0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3A1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x3A2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_music")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3A3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_dialogue")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x3A4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_stop_music")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3A6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3A7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_object_no_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3A8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_create_and_animate_cinematic_object_no_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3A9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_play_cortana_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3AA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "attract_mode_start"),
                [0x3AB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "attract_mode_set_seconds")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3AC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_level_advance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3AD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_won"),
                [0x3AE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_lost")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3AF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_revert"),
                [0x3B0] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_is_cooperative"),
                [0x3B1] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_is_playtest"),
                [0x3B2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_can_use_flashlights")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3B3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_and_quit"),
                [0x3B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_unsafe"),
                [0x3B5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_insertion_point_unlock")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x3B6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_insertion_point_lock")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x3BC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "achievement_grant_to_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3BD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "achievement_grant_to_all_players")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3DC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load"),
                [0x3DD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x3DE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_save"),
                [0x3DF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_save_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x3E0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load_game"),
                [0x3E1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_load_game_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x3E2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_regular_upload_to_debug_server")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x3E3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "core_set_upload_option")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x3E6] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_safe_to_save"),
                [0x3E7] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_safe_to_speak"),
                [0x3E8] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_all_quiet"),
                [0x3E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save"),
                [0x3EA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_cancel"),
                [0x3EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_no_timeout"),
                [0x3EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_immediate"),
                [0x3ED] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_saving"),
                [0x3EE] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "game_reverted"),
                [0x3F1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_set_tag_parameter_unsafe")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3F3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_trigger")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x3F4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3F5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_cinematic")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3F6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3F7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_with_subtitle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x3F8] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "sound_impulse_language_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3F9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3FA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_start_3d")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3FB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_impulse_mark_as_outro")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Sound),
                },
                [0x3FD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_predict")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                },
                [0x3FE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x3FF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                },
                [0x400] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_stop_immediately")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                },
                [0x401] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_set_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x402] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_set_alternate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x403] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_loop_spam"),
                [0x404] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_show_channel")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x405] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_debug_sound_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x406] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_sounds_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x407] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_set_gain")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x408] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_set_gain_db")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x409] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_class_enable_ducker")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x40A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_sound_environment_parameter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x40B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_set_global_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x40C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_set_global_effect_scale")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x40D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_auto_turret")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x40E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_hover")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x40F] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "vehicle_count_bipeds_killed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Vehicle),
                },
                [0x410] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "biped_ragdoll")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x412] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_show_training_text")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x413] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_set_training_text")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x414] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_enable_training")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x415] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_flashlight"),
                [0x416] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_crouch"),
                [0x417] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_stealth"),
                [0x418] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_?"),
                [0x419] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_training_activate_jump"),
                [0x41B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_activate_team_nav_point_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x41C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "hud_deactivate_team_nav_point_flag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                },
                [0x41D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_cortana_suck")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x420] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_cortana_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x423] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_crosshair")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x424] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_shield")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x425] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x426] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_messages")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x427] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_motion_sensor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x428] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_spike_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x429] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_fire_grenades")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x42A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_compass")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x42B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_stamina")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x42C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_energy_meters")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x42D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_consumables")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x42E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_cinematic_fade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x42F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_bonus_round_show_timer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x430] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_bonus_round_start_timer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x431] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_bonus_round_set_timer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x432] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cls"),
                [0x433] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_overflow_suppression")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x434] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x435] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_hide")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x436] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_show_all"),
                [0x437] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_hide_all"),
                [0x438] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "error_geometry_list"),
                [0x439] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_translation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_rotation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_rumble")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_translation_for_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x43F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_rotation_for_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x440] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_set_max_rumble_for_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x441] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_start_for_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x442] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_effect_stop_for_player")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x443] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "time_code_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x444] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "time_code_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x445] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "time_code_reset"),
                [0x446] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "script_screen_effect_set_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x447] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x448] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x449] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_set_crossfade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x44A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_screen_effect_stop"),
                [0x44B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_near_clip_distance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x44C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_far_clip_distance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x44D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_atmosphere_fog")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x44E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "atmosphere_fog_override_fade")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x44F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "motion_blur")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x450] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_weather")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x451] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_patchy_fog")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x452] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_environment_map_attenuation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x453] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_environment_map_bitmap")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Bitmap),
                },
                [0x454] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_bitmap"),
                [0x455] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_environment_map_tint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x456] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_reset_environment_map_tint"),
                [0x457] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_layer")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x458] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "player_has_skills"),
                [0x459] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_has_mad_secret_skills")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x45A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "controller_invert_look"),
                [0x45B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "controller_look_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x45C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "controller_set_look_invert")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x45D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "controller_get_look_invert"),
                [0x45E] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "controller_unlock_solo_levels")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x4AB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_clear"),
                [0x4AC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_show_up_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4AD] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_finish_up_to")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4AE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4AF] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_finish")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_unavailable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_secondary_show")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_secondary_finish")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "objectives_secondary_unavailable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4B4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "input_suppress_rumble")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x4D1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_delegate_?")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4D2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_map_name")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4D4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "net_campaign_difficulty")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x4E5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_bink_movie")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_bink_movie_from_tag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.BinkDefinition),
                },
                [0x4E7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_credits"),
                [0x4E8] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "bink_time"),
                [0x4E9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_global_doppler_factor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4EA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_global_mixbin_headroom")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x4EB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "debug_sound_environment_source_parameter")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x4EC] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_set_mission_segment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4ED] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_display_mission_segment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4EE] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_insert"),
                [0x4F0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_upload"),
                [0x4F1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_playback")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x4F3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "data_mine_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x521] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "object_list_children")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectDefinition),
                },
                [0x522] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "voice_set_outgoing_channel_count")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x523] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "voice_set_voice_repeater_peer_index")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x52A] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_start")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x52B] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_start_smooth")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x52C] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_stop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x52D] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_restart")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x52E] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "interpolator_is_active")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x52F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "interpolator_is_finished")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x530] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "interpolator_set_current_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x531] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_current_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x532] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_start_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x533] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_final_value")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x534] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_current_phase")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x535] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_current_time_fraction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x536] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_start_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x537] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_get_final_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x538] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x539] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_fraction")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x53A] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x53B] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "interpolator_evaluate_at_time_delta")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x53C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "interpolator_stop_all"),
                [0x53D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "interpolator_restart_all"),
                [0x53E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "interpolator_flip"),
                [0x540] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "animation_cache_stats_reset"),
                [0x541] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_clone_players_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x542] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_move_attached_objects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x543] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "vehicle_enable_ghost_effects")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x544] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_global_sound_environment")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x545] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "reset_dsp_image"),
                [0x546] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_save_cinematic_skip"),
                [0x547] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_outro_start"),
                [0x548] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_enable_ambience_details")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x549] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x54A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_reset")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x54B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_blur_amount")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x54C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_threshold")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x54D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_brightness")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x54E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_box_factor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x54F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_max_factor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x550] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_silver_bullet")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x551] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_only")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x552] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_high_res")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x553] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_brightness_alpha")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x554] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "rasterizer_bloom_override_max_factor_alpha")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x555] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cache_block_for_one_frame"),
                [0x556] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_suppress_ambience_update_on_revert"),
                [0x55A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_exposure_fade_out")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x55B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_exposure")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x55C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_autoexposure_instant")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x55E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_depth_of_field_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x55F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_depth_of_field")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x563] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_disable"),
                [0x564] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_lightmap_shadow_enable"),
                [0x565] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "predict_animation")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x566] = new ScriptInfo(HsType.Halo3ODSTValue.ObjectList, "mp_players_by_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x567] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "mp_active_player_count_by_team")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x569] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_game_won")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x56B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_ai_allegiance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Team),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x56C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_allegiance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.MpTeam),
                },
                [0x576] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_object_create_anew")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ObjectName),
                },
                [0x577] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "mp_object_destroy")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x594] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "predict_bink_movie")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                },
                [0x595] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "predict_bink_movie_from_tag")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.BinkDefinition),
                },
                [0x597] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "camera_set_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x59A] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "game_coop_player_count"),
                [0x59F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "add_recycling_volume")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.TriggerVolume),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5A7] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "game_tick_get"),
                [0x5B9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "designer_zone_activate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DesignerZone),
                },
                [0x5BA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "designer_zone_deactivate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.DesignerZone),
                },
                [0x5BB] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_always_active")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x5C0] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_persistent")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x5E1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_performance_throttle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5E2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "set_quality")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5E3] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "get_performance_throttle")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.String),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5E5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_zone_activate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5E6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_zone_deactivate")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5EA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_limit_lipsync_to_mouth_only")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x5F1] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "prepare_to_switch_to_zone_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.ZoneSet),
                },
                [0x5F2] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_zone_activate_for_debugging")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x5F3] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_play_random_ping")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x5F4] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_fade_out_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5F5] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_fade_in_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5F6] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_control_fade_out_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5F7] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_control_fade_in_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5F8] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_lock_gaze")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PointReference),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5F9] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_unlock_gaze")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x5FA] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_control_scale_all_input")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x5FE] = new ScriptInfo(HsType.Halo3ODSTValue.BinkDefinition, "cinematic_tag_reference_get_bink")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x601] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_custom_animation_speed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x602] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "scenery_animation_start_at_frame_loop")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Scenery),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.AnimationGraph),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x603] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "film_manager_set_reproduction_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x604] = new ScriptInfo(HsType.Halo3ODSTValue.CinematicSceneDefinition, "cortana_effect_tag_reference_get_scene")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x607] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "biped_force_ground_fitting_on")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x608] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_chud_objective")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x609] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_cinematic_title")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneTitle),
                },
                [0x60A] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "terminal_is_being_read"),
                [0x60B] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "terminal_was_accessed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x60C] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "terminal_was_completed")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                },
                [0x60D] = new ScriptInfo(HsType.Halo3ODSTValue.Weapon, "unit_get_primary_weapon")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x610] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_award_level_complete_achievements"),
                [0x612] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_safe_to_respawn")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x613] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cortana_effect_kill"),
                [0x616] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_destroy_cortana_effect_cinematic"),
                [0x617] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_migrate_infanty")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                },
                [0x618] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "render_cinematic_motion_blur")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x619] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_dont_do_avoidance")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x61A] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_clean_up")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x61B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_erase_inactive")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x61C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "ai_survival_cleanup")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Ai),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x61D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "stop_bink_movie"),
                [0x61E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "play_credits_unskippable"),
                [0x623] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_set_debug_mode")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x624] = new ScriptInfo(HsType.Halo3ODSTValue.Object, "cinematic_scripting_get_object")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x626] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "gp_integer_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x627] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "gp_integer_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x628] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "gp_boolean_get")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x629] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "gp_boolean_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x62F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_start_screen_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x630] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_stop_screen_effect")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x631] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "game_level_prepare")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x632] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "prepare_game_level")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x63D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "cinematic_scripting_set_user_input_constraints")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x63E] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "skull_primary_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.PrimarySkull),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x63F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "skull_secondary_enable")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.SecondarySkull),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x643] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_respawn_dead_players"),
                [0x644] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "survival_mode_lives_get"),
                [0x645] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_lives_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x646] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "survival_mode_set_get"),
                [0x647] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_set_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x648] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "survival_mode_round_get"),
                [0x649] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_round_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x64A] = new ScriptInfo(HsType.Halo3ODSTValue.Short, "survival_mode_wave_get"),
                [0x64B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_wave_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x64C] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "survival_mode_set_multiplier_get"),
                [0x64D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_set_multiplier_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x64E] = new ScriptInfo(HsType.Halo3ODSTValue.Real, "survival_mode_bonus_multiplier_get"),
                [0x64F] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_bonus_multiplier_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x651] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_set_rounds_per_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x652] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_set_waves_per_round")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
                },
                [0x654] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_event_new")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x655] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_begin_new_set"),
                [0x656] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_begin_new_round"),
                [0x657] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_begin_new_wave"),
                [0x658] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "survival_mode_award_hero_medal"),
                [0x659] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "campaign_metagame_get_player_score")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x65C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "player_set_look_training_hack")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x65D] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "device_arg_has_been_touched_by_unit")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Device),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x665] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "achievement_post_chud_progression")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x666] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "object_set_vision_mode_render_default")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x667] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_show_navpoint")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.CutsceneFlag),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x66B] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_confirm_message")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x66C] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_confirm_cancel_message")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x66D] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "unit_confirm_y_button")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x66E] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "player_get_kills_by_type")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x66F] = new ScriptInfo(HsType.Halo3ODSTValue.Boolean, "unit_flashlight_on")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Unit),
                },
                [0x670] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "clear_command_buffer_cache_from_script")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Boolean),
                },
                [0x671] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "sound_looping_resume")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.LoopingSound),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Real),
                },
                [0x672] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "chud_bonus_round_set_target_score")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Long),
                },
                [0x67D] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "ui_get_player_model_id"),
                [0x67E] = new ScriptInfo(HsType.Halo3ODSTValue.Long, "ui_get_music_id"),
                [0x680] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "biped_set_armor")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Object),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.StringId),
                },
                [0x694] = new ScriptInfo(HsType.Halo3ODSTValue.Void, "background_set")
                {
                    new ScriptInfo.ParameterInfo(HsType.Halo3ODSTValue.Short),
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
