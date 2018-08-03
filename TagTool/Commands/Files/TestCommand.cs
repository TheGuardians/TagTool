using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Scripting;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using TagTool.Tags;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace TagTool.Commands.Files
{
    class TestCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private static bool debugConsoleWrite = true;
        private static List<string> csvQueue1 = new List<string>();
        private static List<string> csvQueue2 = new List<string>();

        public TestCommand(HaloOnlineCacheContext cacheContext) :
            base(true,

                "Test",
                "A test command.",

                "Test",

                "A test command. Used for various testing and temporary functionality.\n" +
                "Example setinvalidmaterials: 'Test setinvalidmaterials <ED mode or sbsp tag>'. Set all materials to 0x101F shaders\\invalid. \n\n")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count == 0)
                return false;

            var name = args[0].ToLower();
            args.RemoveAt(0);

            var commandsList = new Dictionary<string, string>
            {
                { "scriptingxml", "scriptingxml" },
                { "lensunknown", "lensunknown" },
                { "setinvalidmaterials", "Set all materials to shaders\\invalid or 0x101F to a provided mode or sbsp tag." },
                { "namemodetags", "Name all mode tags based on" },
                { "dumpforgepalettecommands", "Read a scnr tag's forge palettes and dump as a tagtool commands script." },
                { "dumpcommandsscript", "Extract all the tags of a mode or sbsp tag (rmt2, rm--) and generate a commands script. WIP" },
                { "shadowfix", "Hack/fix a weapon or forge object's shadow mesh." },
                { "namermt2", "Name all rmt2 tags based on their parent render method." },
                { "findconicaleffects", "" },
                { "mergeglobaltags", "Merges matg/mulg tags ported from legacy cache files into single Halo Online format matg/mulg tags." },
                { "cisc", "" },
                { "defaultbitmaptypes", "" },
                { "mergetagnames", "" }
            };

            switch (name)
            {
                case "scriptingxml": return ScriptingXml(args);
                case "lensunknown": return LensUnknown(args);
                case "setinvalidmaterials": return SetInvalidMaterials(args);
                case "dumpforgepalettecommands": return DumpForgePaletteCommands(args);
                case "dumpcommandsscript": return DumpCommandsScript(args);
                case "temp": return Temp(args);
                case "shadowfix": return ShadowFix(args);
                case "namermt2": return NameRmt2();
                case "findconicaleffects": return FindConicalEffects();
                case "mergeglobaltags": return MergeGlobalTags(args);
                case "cisc": return Cisc(args);
                case "defaultbitmaptypes": return DefaultBitmapTypes(args);
                case "mergetagnames": return MergeTagNames(args);
                case "adjustscriptsfromfile": return AdjustScriptsFromFile(args);
                case "batchtagdepadd": return BatchTagDepAdd(args);
                case "setupmulg": return SetupMulg();
                case "listprematchcameras": return ListPrematchCameras();
                case "findnullshaders": return FindNullShaders();
                case "nameglobalmaterials": return NameGlobalMaterials(args);
                case "nameanytagsubtags": return NameAnyTagSubtags(args);
                case "compareedtoblam": return CompareEDtoBlam(args);
                case "dsrc": return Dsrc();
                default:
                    Console.WriteLine($"Invalid command: {name}");
                    Console.WriteLine($"Available commands: {commandsList.Count}");
                    foreach (var a in commandsList)
                        Console.WriteLine($"{a.Key}: {a.Value}");
                    return false;
            }
        }

        private bool Dsrc()
        {
            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                var templateTagIndices = new HashSet<int>();

                foreach (var tag in CacheContext.TagCache.Index.FindAllInGroups("dsrc"))
                {
                    if (tag == null)
                        continue;

                    var dsrcContext = new TagSerializationContext(cacheStream, CacheContext, tag);
                    var dsrcDefinition = CacheContext.Deserialize<GuiDatasourceDefinition>(dsrcContext);

                    if (dsrcDefinition.Unknown4 != 0)
                    {
                        var tagName = CacheContext.TagNames.ContainsKey(tag.Index) ?
                            CacheContext.TagNames[tag.Index] :
                            $"0x{tag.Index:X4}";

                        Console.WriteLine($"[{tag.Group.Tag}, 0x{tag.Index:X4}] {tagName}.{CacheContext.GetString(tag.Group.Name)}");
                        Console.WriteLine($"{dsrcDefinition.Unknown4}");
                        Console.WriteLine();
                    }
                }
            }

            return true;
        }

        private bool FindNullShaders()
        {
            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                var templateTagIndices = new HashSet<int>();

                foreach (var tag in CacheContext.TagCache.Index.FindAllInGroups("rm  "))
                {
                    if (tag == null)
                        continue;

                    var tagContext = new TagSerializationContext(cacheStream, CacheContext, tag);
                    var rmDefinition = (RenderMethod)CacheContext.Deserialize(tagContext, TagDefinition.Find(tag.Group));

                    if (rmDefinition.ShaderProperties[0].Unknown.Count != 0)
                    {
                        var rmt2Instance = CacheContext.GetTag(rmDefinition.ShaderProperties[0].Template.Index);

                        var tagName = CacheContext.TagNames.ContainsKey(rmt2Instance.Index) ?
                            CacheContext.TagNames[rmt2Instance.Index] :
                            $"0x{rmt2Instance.Index:X4}";

                        Console.WriteLine($"[{rmt2Instance.Group.Tag}, 0x{rmt2Instance.Index:X4}] {tagName}.{CacheContext.GetString(rmt2Instance.Group.Name)}");
                        Console.WriteLine($"{rmDefinition.ShaderProperties[0].Unknown[0].Unknown}");
                        Console.WriteLine();
                    }
                }
            }

            return true;
        }

        private bool ListPrematchCameras()
        {
            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                foreach (var scnrTag in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
                {
                    if (scnrTag == null)
                        continue;

                    var scnrContext = new TagSerializationContext(cacheStream, CacheContext, scnrTag);
                    var scnrDefinition = CacheContext.Deserialize<Scenario>(scnrContext);

                    foreach (var cameraPoint in scnrDefinition.CutsceneCameraPoints)
                    {
                        if (cameraPoint.Name == "prematch_camera")
                        {
                            Console.WriteLine($"case @\"{CacheContext.TagNames[scnrTag.Index]}\":");
                            Console.WriteLine($"    createPrematchCamera = true;");
                            Console.WriteLine($"    position = new RealPoint3d({cameraPoint.Position.X}f, {cameraPoint.Position.Y}f, {cameraPoint.Position.Z}f);");
                            Console.WriteLine($"    orientation = new RealEulerAngles3d(Angle.FromDegrees({cameraPoint.Orientation.Yaw.Degrees}f), Angle.FromDegrees({cameraPoint.Orientation.Pitch.Degrees}f), Angle.FromDegrees({cameraPoint.Orientation.Roll.Degrees}f));");
                            Console.WriteLine($"    break;");
                            break;
                        }
                    }
                }
            }

            return true;
        }

        private bool SetupMulg()
        {
            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                var mulgContext = new TagSerializationContext(stream, CacheContext, CacheContext.TagCache.Index.FindFirstInGroup("mulg"));
                var mulgDefinition = CacheContext.Deserialize<MultiplayerGlobals>(mulgContext);

                mulgDefinition.Universal[0].GameVariantWeapons = new List<MultiplayerGlobals.UniversalBlock.GameVariantWeapon>
            {
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("battle_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\battle_rifle\battle_rifle")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("assault_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\assault_rifle\assault_rifle")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("plasma_pistol"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\pistol\plasma_pistol\plasma_pistol")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("spike_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\spike_rifle\spike_rifle")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("smg"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\smg\smg")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("carbine"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\covenant_carbine\covenant_carbine")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("energy_sword"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\melee\energy_blade\energy_blade")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("magnum"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\pistol\magnum\magnum")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("needler"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\pistol\needler\needler")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("plasma_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\plasma_rifle\plasma_rifle")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("rocket_launcher"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\support_high\rocket_launcher\rocket_launcher")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("shotgun"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\shotgun\shotgun")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("sniper_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\sniper_rifle\sniper_rifle")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("brute_shot"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\support_low\brute_shot\brute_shot")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("unarmed"),
                    RandomChance = 0,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\melee\energy_blade\energy_blade_useless")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("beam_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\beam_rifle\beam_rifle")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("spartan_laser"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\support_high\spartan_laser\spartan_laser")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("none"),
                    RandomChance = 0,
                    Weapon = null
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("gravity_hammer"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\melee\gravity_hammer\gravity_hammer")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("excavator"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\pistol\excavator\excavator")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("flamethrower"),
                    RandomChance = 0,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\turret\flamethrower\flamethrower")
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("missile_pod"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.GetTag<Weapon>(@"objects\weapons\turret\missile_pod\missile_pod")
                }
            };

                mulgDefinition.Runtime[0].MultiplayerConstants[0].Weapons = new List<MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon>
                {
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // battle_rifle
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\battle_rifle\battle_rifle"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // carbine
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\covenant_carbine\covenant_carbine"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // sniper_rifle
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\sniper_rifle\sniper_rifle"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // beam_rifle
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\beam_rifle\beam_rifle"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // spartan_laster
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\support_high\spartan_laser\spartan_laser"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // rocket_launcher
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\support_high\rocket_launcher\rocket_launcher"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // chaingun_turret
                        Type = CacheContext.GetTag<Weapon>(@"objects\vehicles\warthog\turrets\chaingun\weapon\chaingun_turret"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // machinegun_turret
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\turret\machinegun_turret\machinegun_turret"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // machinegun_turret_integrated
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\turret\machinegun_turret\machinegun_turret_integrated"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // plasma_cannon
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\turret\plasma_cannon\plasma_cannon"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // plasma_cannon_integrated
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\turret\plasma_cannon\plasma_cannon_integrated"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // needler
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\pistol\needler\needler"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // flak_cannon
                        Type = CacheContext.GetTag<Weapon>(@"objects\weapons\support_high\flak_cannon\flak_cannon"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // gauss_turret
                        Type = CacheContext.GetTag<Weapon>(@"objects\vehicles\warthog\turrets\gauss\weapon\gauss_turret"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // anti_infantry
                        Type = CacheContext.GetTag<Weapon>(@"objects\vehicles\mauler\anti_infantry\weapon\anti_infantry"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    },
                    new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                    {
                        // behemoth_chaingun_turret
                        Type = null,// CacheContext.GetTag<Weapon>(@"objects\levels\multi\shrine\behemoth\weapon\behemoth_chaingun_turret"),
                        Unknown1 = 5.0f,
                        Unknown2 = 15.0f,
                        Unknown3 = 5.0f,
                        Unknown4 = -10.0f
                    }
                };
                
                #region Universal GameVariantVehicles
                mulgDefinition.Universal[0].GameVariantVehicles = new List<MultiplayerGlobals.UniversalBlock.GameVariantVehicle>
                {
                    new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                    {
                        Name = CacheContext.GetStringId("warthog"),
                        Vehicle = CacheContext.GetTag<Vehicle>(@"objects\vehicles\warthog\warthog")
                    },
                    new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                    {
                        Name = CacheContext.GetStringId("ghost"),
                        Vehicle = CacheContext.GetTag<Vehicle>(@"objects\vehicles\ghost\ghost")
                    },
                    new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                    {
                        Name = CacheContext.GetStringId("scorpion"),
                        Vehicle = CacheContext.GetTag<Vehicle>(@"objects\vehicles\scorpion\scorpion")
                    },
                    new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                    {
                        Name = CacheContext.GetStringId("wraith"),
                        Vehicle = CacheContext.GetTag<Vehicle>(@"objects\vehicles\wraith\wraith")
                    },
                    new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                    {
                        Name = CacheContext.GetStringId("banshee"),
                        Vehicle = CacheContext.GetTag<Vehicle>(@"objects\vehicles\banshee\banshee")
                    },
                    new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                    {
                        Name = CacheContext.GetStringId("mongoose"),
                        Vehicle = CacheContext.GetTag<Vehicle>(@"objects\vehicles\mongoose\mongoose")
                    },
                    new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                    {
                        Name = CacheContext.GetStringId("chopper"),
                        Vehicle = CacheContext.GetTag<Vehicle>(@"objects\vehicles\brute_chopper\brute_chopper")
                    },
                    new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                    {
                        Name = CacheContext.GetStringId("mauler"),
                        Vehicle = null, //CacheContext.GetTagInstance<Vehicle>(@"objects\vehicles\mauler\mauler")
                    },
                    new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                    {
                        Name = CacheContext.GetStringId("hornet"),
                        Vehicle = CacheContext.GetTag<Vehicle>(@"objects\vehicles\hornet\hornet")
                    }
                };
                #endregion

                #region Universal VehicleSets
                mulgDefinition.Universal[0].VehicleSets = new List<MultiplayerGlobals.UniversalBlock.VehicleSet>
                {
                    new MultiplayerGlobals.UniversalBlock.VehicleSet
                    {
                        Name = CacheContext.GetStringId("default"),
                        Substitutions = new List<MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution>()
                    },
                    new MultiplayerGlobals.UniversalBlock.VehicleSet
                    {
                        Name = CacheContext.GetStringId("no_vehicles"),
                        #region Substitutions
                        Substitutions = new List<MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution>
                        {
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("warthog"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("ghost"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("scorpion"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("wraith"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("mongoose"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("banshee"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("chopper"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("mauler"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("hornet"),
                                SubstitutedVehicle = StringId.Invalid
                            }
                        }
                        #endregion
                    },
                    new MultiplayerGlobals.UniversalBlock.VehicleSet
                    {
                        Name = CacheContext.GetStringId("mongooses_only"),
                        #region Substitutions
                        Substitutions = new List<MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution>
                        {
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("warthog"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("ghost"),
                                SubstitutedVehicle = CacheContext.GetStringId("mongoose")
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("scorpion"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("wraith"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("banshee"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("chopper"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("mauler"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("hornet"),
                                SubstitutedVehicle = StringId.Invalid
                            }
                        }
                        #endregion
                    },
                    new MultiplayerGlobals.UniversalBlock.VehicleSet
                    {
                        Name = CacheContext.GetStringId("light_ground_only"),
                        #region Substitutions
                        Substitutions = new List<MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution>
                        {
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("scorpion"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("wraith"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("banshee"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("hornet"),
                                SubstitutedVehicle = StringId.Invalid
                            }
                        }
                        #endregion
                    },
                    new MultiplayerGlobals.UniversalBlock.VehicleSet
                    {
                        Name = CacheContext.GetStringId("tanks_only"),
                        #region Substitutions
                        Substitutions = new List<MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution>
                        {
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("warthog"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("ghost"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("mongoose"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("banshee"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("chopper"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("mauler"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("hornet"),
                                SubstitutedVehicle = StringId.Invalid
                            }
                        }
                        #endregion
                    },
                    new MultiplayerGlobals.UniversalBlock.VehicleSet
                    {
                        Name = CacheContext.GetStringId("aircraft_only"),
                        #region Substitutions
                        Substitutions = new List<MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution>
                        {
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("warthog"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("ghost"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("scorpion"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("wraith"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("mongoose"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("chopper"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("mauler"),
                                SubstitutedVehicle = StringId.Invalid
                            }
                        }
                        #endregion
                    },
                    new MultiplayerGlobals.UniversalBlock.VehicleSet
                    {
                        Name = CacheContext.GetStringId("no_light_ground"),
                        #region Substitutions
                        Substitutions = new List<MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution>
                        {
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("warthog"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("ghost"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("mongoose"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("chopper"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("mauler"),
                                SubstitutedVehicle = StringId.Invalid
                            }
                        }
                        #endregion
                    },
                    new MultiplayerGlobals.UniversalBlock.VehicleSet
                    {
                        Name = CacheContext.GetStringId("no_tanks"),
                        #region Substitutions
                        Substitutions = new List<MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution>
                        {
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("scorpion"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("wraith"),
                                SubstitutedVehicle = StringId.Invalid
                            }
                        }
                        #endregion
                    },
                    new MultiplayerGlobals.UniversalBlock.VehicleSet
                    {
                        Name = CacheContext.GetStringId("no_aircraft"),
                        #region Substitutions
                        Substitutions = new List<MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution>
                        {
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("banshee"),
                                SubstitutedVehicle = StringId.Invalid
                            },
                            new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                            {
                                OriginalVehicle = CacheContext.GetStringId("hornet"),
                                SubstitutedVehicle = StringId.Invalid
                            }
                        }
                        #endregion
                    },
                    new MultiplayerGlobals.UniversalBlock.VehicleSet
                    {
                        Name = CacheContext.GetStringId("all_vehicles"),
                        Substitutions = new List<MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution>()
                    }
                };
                #endregion

                CacheContext.Serialize(mulgContext, mulgDefinition);
            }

            return true;
        }

        private bool MergeTagNames(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var context = new HaloOnlineCacheContext(new DirectoryInfo(args[0]));
            context.LoadTagNames();

            foreach (var entry in context.TagNames)
            {
                if (entry.Key >= CacheContext.TagCache.Index.Count || CacheContext.TagCache.Index[entry.Key] == null)
                    continue;

                var srcTag = CacheContext.GetTag(entry.Key);
                var dstTag = context.GetTag(entry.Key);

                if (!srcTag.IsInGroup(dstTag.Group) || CacheContext.TagNames.ContainsKey(srcTag.Index))
                    continue;

                CacheContext.TagNames[srcTag.Index] = context.TagNames[dstTag.Index];
            }

            return true;
        }

        private bool DefaultBitmapTypes(List<string> args)
        {
            if (args.Count != 0)
                return false;

            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                var defaultBitmapNames = new List<string>
                {
                    @"shaders\default_bitmaps\bitmaps\gray_50_percent",
                    @"shaders\default_bitmaps\bitmaps\alpha_grey50",
                    @"shaders\default_bitmaps\bitmaps\color_white",
                    @"shaders\default_bitmaps\bitmaps\default_detail",
                    @"shaders\default_bitmaps\bitmaps\reference_grids",
                    @"shaders\default_bitmaps\bitmaps\default_vector",
                    @"shaders\default_bitmaps\bitmaps\default_alpha_test",
                    @"shaders\default_bitmaps\bitmaps\default_dynamic_cube_map",
                    @"shaders\default_bitmaps\bitmaps\color_red",
                    @"shaders\default_bitmaps\bitmaps\alpha_white",
                    @"shaders\default_bitmaps\bitmaps\monochrome_alpha_grid",
                    @"shaders\default_bitmaps\bitmaps\gray_50_percent_linear",
                    @"shaders\default_bitmaps\bitmaps\color_black_alpha_black",
                    @"shaders\default_bitmaps\bitmaps\dither_pattern",
                    @"shaders\default_bitmaps\bitmaps\bump_detail",
                    @"shaders\default_bitmaps\bitmaps\color_black",
                    @"shaders\default_bitmaps\bitmaps\auto_exposure_weight",
                    @"shaders\default_bitmaps\bitmaps\dither_pattern2",
                    @"shaders\default_bitmaps\bitmaps\random4_warp",
                    @"levels\shared\bitmaps\nature\water\water_ripples",
                    @"shaders\default_bitmaps\bitmaps\vision_mode_mask"
                };

                var defaultBitmapTypes = new Dictionary<string, List<string>>();

                foreach (var tag in CacheContext.TagCache.Index)
                {
                    if (tag == null || !(tag.IsInGroup("rm  ") || tag.IsInGroup("prt3")))
                        continue;

                    var tagContext = new TagSerializationContext(cacheStream, CacheContext, tag);
                    var tagDefinition = CacheContext.Deserialize(tagContext, TagDefinition.Find(tag.Group));

                    RenderMethod renderMethod = null;

                    switch (tagDefinition)
                    {
                        case RenderMethod rm:
                            renderMethod = rm;
                            break;

                        case Particle prt3:
                            renderMethod = prt3.RenderMethod;
                            break;
                    }

                    tagContext = new TagSerializationContext(cacheStream, CacheContext, renderMethod.ShaderProperties[0].Template);
                    var template = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(tagContext);

                    for (var i = 0; i < template.ShaderMaps.Count; i++)
                    {
                        var mapTemplate = template.ShaderMaps[i];
                        var mapName = CacheContext.GetString(mapTemplate.Name);

                        var mapShader = renderMethod.ShaderProperties[0].ShaderMaps[i];
                        var mapTagName = CacheContext.TagNames.ContainsKey(mapShader.Bitmap.Index) ?
                            CacheContext.TagNames[mapShader.Bitmap.Index] :
                            $"0x{mapShader.Bitmap.Index:X4}";

                        if (!mapTagName.StartsWith(@"shaders\default_bitmaps\"))
                            continue;

                        if (!defaultBitmapTypes.ContainsKey(mapTagName))
                            defaultBitmapTypes[mapTagName] = new List<string>();

                        if (!defaultBitmapTypes[mapTagName].Contains(mapName))
                            defaultBitmapTypes[mapTagName].Add(mapName);
                    }
                }

                foreach (var entry in defaultBitmapTypes)
                {
                    foreach (var type in entry.Value)
                        Console.WriteLine($"case \"{type}\":");
                    Console.WriteLine($"return @\"{entry.Key}\";");
                    Console.WriteLine("\tbreak;");
                    Console.WriteLine();
                }
            }

            return true;
        }

        private bool Cisc(List<string> args)
        {
            if (args.Count != 0)
                return false;

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                foreach (var tagInstance in CacheContext.TagCache.Index)
                {
                    if (tagInstance == null || !tagInstance.IsInGroup("cisc"))
                        continue;

                    var tagContext = new TagSerializationContext(cacheStream, CacheContext, tagInstance);
                    var tagDefinition = CacheContext.Deserialize<CinematicScene>(tagContext);

                    foreach (var shot in tagDefinition.Shots)
                    {
                        shot.LoadedFrameCount -= 1;

                        foreach (var sound in shot.Sounds)
                            sound.Frame = Math.Min(sound.Frame == 1 ? 1 : sound.Frame * 2, shot.LoadedFrameCount - 1);

                        foreach (var sound in shot.BackgroundSounds)
                            sound.Frame = Math.Min(sound.Frame == 1 ? 1 : sound.Frame * 2, shot.LoadedFrameCount - 1);

                        foreach (var effect in shot.Effects)
                            effect.Frame = Math.Min(effect.Frame == 1 ? 1 : effect.Frame * 2, shot.LoadedFrameCount - 1);

                        foreach (var effect in shot.ScreenEffects)
                        {
                            effect.StartFrame = Math.Min(effect.StartFrame == 1 ? 1 : effect.StartFrame * 2, shot.LoadedFrameCount - 1);
                            effect.EndFrame = Math.Min(effect.EndFrame == 1 ? 1 : effect.EndFrame * 2, shot.LoadedFrameCount - 1);
                        }

                        foreach (var script in shot.ImportScripts)
                            script.Frame = Math.Min(script.Frame == 1 ? 1 : script.Frame * 2, shot.LoadedFrameCount - 1);

                        for (var i = 0; i < shot.LoadedFrameCount; i++)
                        {
                            if (i + 2 >= shot.LoadedFrameCount)
                                break;

                            shot.Frames[i + 1].Flags = shot.Frames[i].Flags;
                        }
                    }

                    CacheContext.Serialize(tagContext, tagDefinition);
                }
            }

            return true;
        }

        private Globals MergeGlobals(List<Globals> matgs)
        {
            var matg = matgs[0];

            return matg;
        }

        private MultiplayerGlobals MergeMultiplayerGlobals(List<MultiplayerGlobals> mulgs)
        {
            var mulg = mulgs[0];

            return mulg;
        }

        private object MergeGlobalTags(List<string> args)
        {
            // initialize serialization contexts.
            var tagsContext = new TagSerializationContext(null, null, null);

            // find global tags
            var matg_tags = CacheContext.TagCache.Index.FindAllInGroup(new Tag("matg")).ToList();
            var mulg_tags = CacheContext.TagCache.Index.FindAllInGroup(new Tag("mulg")).ToList();

            using (var tagsStream = CacheContext.OpenTagCacheReadWrite())
            {
                var matgs = new List<Globals> { };
                var mulgs = new List<MultiplayerGlobals> { };

                // deserialize halo-online globals.
                foreach (var matg_tag in matg_tags)
                {
                    tagsContext = new TagSerializationContext(tagsStream, CacheContext, matg_tag);
                    matgs.Add(CacheContext.Deserializer.Deserialize<Globals>(tagsContext));
                }
                foreach (var mulg_tag in mulg_tags)
                {
                    tagsContext = new TagSerializationContext(tagsStream, CacheContext, mulg_tag);
                    mulgs.Add(CacheContext.Deserializer.Deserialize<MultiplayerGlobals>(tagsContext));
                }

                // merge global tags into the first global tag
                var matg = MergeGlobals(matgs);
                var mulg = MergeMultiplayerGlobals(mulgs);

                // serialize global tags
                tagsContext = new TagSerializationContext(tagsStream, CacheContext, matg_tags[0]);
                CacheContext.Serialize(tagsContext, matg);
                tagsContext = new TagSerializationContext(tagsStream, CacheContext, mulg_tags[0]);
                CacheContext.Serialize(tagsContext, mulg);
            }

            return true;
        }

        private bool FindConicalEffects()
        {
            using (var stream = CacheContext.TagCacheFile.OpenRead())
            using (var reader = new BinaryReader(stream))
            {
                for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
                {
                    var tag = CacheContext.GetTag(i);

                    if (tag == null || !tag.IsInGroup("effe"))
                        continue;

                    stream.Position = tag.HeaderOffset + tag.DefinitionOffset + 0x5C;
                    var conicalDistributionCount = reader.ReadInt32();

                    if (conicalDistributionCount <= 0)
                        continue;

                    var tagName = CacheContext.TagNames.ContainsKey(tag.Index) ?
                        $"0x{tag.Index:X4} - {CacheContext.TagNames[tag.Index]}" :
                        $"0x{tag.Index:X4}";

                    Console.WriteLine($"{tagName}.effect - {conicalDistributionCount} {(conicalDistributionCount == 1 ? "distribution" : "distributions")}");
                }
            }

            return true;
        }

        public void CsvDumpQueueToFile(List<string> in_, string file)
        {
            var fileOut = new FileInfo(file);
            if (File.Exists(file))
                File.Delete(file);

            int i = -1;
            using (var csvStream = fileOut.OpenWrite())
            using (var csvWriter = new StreamWriter(csvStream))
            {
                foreach (var a in in_)
                {
                    csvStream.Position = csvStream.Length;
                    csvWriter.WriteLine(a);
                    i++;
                }
            }
        }

        private CacheFile OpenCacheFile(string cacheArg)
        {
            FileInfo blamCacheFile = new FileInfo(cacheArg);

            // Console.WriteLine("Reading H3 cache file...");

            if (!blamCacheFile.Exists)
                throw new FileNotFoundException(blamCacheFile.FullName);

            CacheFile BlamCache = null;

            using (var fs = new FileStream(blamCacheFile.FullName, FileMode.Open, FileAccess.Read))
            {
                var reader = new EndianReader(fs, EndianFormat.BigEndian);

                var head = reader.ReadInt32();

                if (head == 1684104552)
                    reader.Format = EndianFormat.LittleEndian;

                var v = reader.ReadInt32();

                reader.SeekTo(284);
                var version = CacheVersionDetection.GetFromBuildName(reader.ReadString(32));

                switch (version)
                {
                    case CacheVersion.Halo2Xbox:
                    case CacheVersion.Halo2Vista:
                        BlamCache = new CacheFileGen2(CacheContext, blamCacheFile, version, false);
                        break;

                    case CacheVersion.Halo3Retail:
                    case CacheVersion.Halo3ODST:
                    case CacheVersion.HaloReach:
                        BlamCache = new CacheFileGen3(CacheContext, blamCacheFile, version, false);
                        break;

                    default:
                        throw new NotSupportedException(CacheVersionDetection.GetBuildName(version));
                }
            }

            // BlamCache.LoadResourceTags();

            return BlamCache;
        }

        private ScriptValueType.Halo3ODSTValue ParseScriptValueType(string value)
        {
            foreach (var option in Enum.GetNames(typeof(ScriptValueType.Halo3ODSTValue)))
                if (value.ToLower().Replace("_", "").Replace(" ", "") == option.ToLower().Replace("_", "").Replace(" ", ""))
                    return (ScriptValueType.Halo3ODSTValue)Enum.Parse(typeof(ScriptValueType.Halo3ODSTValue), option);

            throw new KeyNotFoundException(value);
        }

        private bool ScriptingXml(List<string> args)
        {
            if (args.Count != 0)
                return false;

            //
            // Load the lower-version scription xml file
            //


            Console.WriteLine();
            Console.WriteLine("Enter the path to the scripting xml:");
            Console.Write("> ");

            var xmlPath = Console.ReadLine();

            var xml = new XmlDocument();
            xml.Load(xmlPath);

            var scripts = new Dictionary<int, ScriptInfo>();

            foreach (XmlNode node in xml["BlamScript"]["functions"])
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                var script = new ScriptInfo(
                    ParseScriptValueType(node.Attributes["returnType"].InnerText),
                    node.Attributes["name"].InnerText);

                if (script.Name == "")
                    continue;

                if (node.HasChildNodes)
                {
                    foreach (XmlNode argumentNode in node.ChildNodes)
                    {
                        if (argumentNode.NodeType != XmlNodeType.Element)
                            continue;

                        script.Arguments.Add(new ScriptInfo.ArgumentInfo(ParseScriptValueType(argumentNode.Attributes["type"].InnerText)));
                    }
                }

                scripts[int.Parse(node.Attributes["opcode"].InnerText.Replace("0x", ""), NumberStyles.HexNumber)] = script;
            }

            Console.WriteLine();

            for (var opcode = 0; opcode < scripts.Keys.Max(); opcode++)
            {
                if (!scripts.ContainsKey(opcode))
                    continue;

                var script = scripts[opcode];

                if (script.Arguments.Count == 0)
                {
                    Console.WriteLine($"                [0x{opcode:X3}] = new ScriptInfo(ScriptValueType.{script.Type}, \"{script.Name}\"),");
                }
                else
                {
                    Console.WriteLine($"                [0x{opcode:X3}] = new ScriptInfo(ScriptValueType.{script.Type}, \"{script.Name}\")");
                    Console.WriteLine("                {");

                    foreach (var argument in script.Arguments)
                        Console.WriteLine($"                    new ScriptInfo.ArgumentInfo(ScriptValueType.{argument.Type}),");

                    Console.WriteLine("                },");
                }
            }

            Console.WriteLine();

            return true;
        }

        private bool LensUnknown(List<string> args)
        {
            if (args.Count != 0)
                return false;

            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("lens"))
                {
                    var context = new TagSerializationContext(cacheStream, CacheContext, instance);
                    var definition = CacheContext.Deserializer.Deserialize<LensFlare>(context);
                }
            }

            return true;
        }

        private static CachedTagInstance PortTagReference(HaloOnlineCacheContext cacheContext, CacheFile blamCache, int index)
        {
            if (index == -1)
                return null;

            var instance = blamCache.IndexItems.Find(i => i.ID == index);

            if (instance != null)
            {
                var tags = cacheContext.TagCache.Index.FindAllInGroup(instance.GroupTag);

                foreach (var tag in tags)
                {
                    if (!cacheContext.TagNames.ContainsKey(tag.Index))
                        continue;

                    if (instance.Name == cacheContext.TagNames[tag.Index])
                        return tag;
                }
            }

            return null;
        }

        public bool SetInvalidMaterials(List<string> args) // Set all mode or sbsp shaders to shaders\invalid 0x101F
        {
            Console.WriteLine("Required args: [0]ED tag; ");

            if (args.Count != 1)
                return false;

            string edTagArg = args[0];

            if (!CacheContext.TryGetTag(edTagArg, out var edTag))
                return false;

            if (edTag.IsInGroup("mode"))
            {
                RenderModel edMode;
                using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                    edMode = CacheContext.Deserializer.Deserialize<RenderModel>(edContext);
                }

                foreach (var a in edMode.Materials)
                    a.RenderMethod = CacheContext.GetTag(0x101F);

                using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var context = new TagSerializationContext(stream, CacheContext, edTag);
                    CacheContext.Serializer.Serialize(context, edMode);
                }
            }

            else if (edTag.IsInGroup("sbsp"))
            {
                ScenarioStructureBsp instance;
                using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                    instance = CacheContext.Deserializer.Deserialize<ScenarioStructureBsp>(edContext);
                }

                foreach (var a in instance.Materials)
                    a.RenderMethod = CacheContext.GetTag(0x101F);

                Console.WriteLine("Nuked shaders.");

                using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var context = new TagSerializationContext(stream, CacheContext, edTag);
                    CacheContext.Serializer.Serialize(context, instance);
                }
            }

            return true;
        }

        public bool DumpForgePaletteCommands(List<string> args) // Dump all the forge lists of a scnr to use as tagtool commands. Mainly to reorder the items easily
        {
            Console.WriteLine("Required args: [0]ED scnr tag; ");

            if (args.Count != 1 || !CacheContext.TryGetTag(args[0], out var edTag))
                return false;

            Scenario instance;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                instance = CacheContext.Deserializer.Deserialize<Scenario>(edContext);
            }

            Console.WriteLine($"RemoveBlockElements SandboxEquipment 0 *");
            foreach (var a in instance.SandboxEquipment)
            {
                Console.WriteLine($"AddBlockElements SandboxEquipment 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField SandboxEquipment[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField SandboxEquipment[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField SandboxEquipment[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            string type = "SandboxWeapons";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxWeapons)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            type = "SandboxVehicles";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxVehicles)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            type = "SandboxScenery";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxScenery)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            type = "SandboxSpawning";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxSpawning)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            type = "SandboxTeleporters";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxTeleporters)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            type = "SandboxGoalObjects";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxGoalObjects)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            return true;
        }

        public bool DumpCommandsScript(List<string> args)
        {
            // Role: extract all the tags of a mode or sbsp tag.
            // Extract all the shaders of that tag, rmt2, vtsh, pixl and bitmaps of all the shaders
            // Dump commands to make a mod out of it.
            // Dump commands to reimport into a new build.

            // rmdf, rmt2, vtsh, pixl, mode, shader tags NEED to be named.

            if (args.Count != 1 || !CacheContext.TryGetTag(args[0], out var instance))
                return false;

            string modName = args[0].Split("\\".ToCharArray()).Last();

            if (!instance.IsInGroup("mode"))
                throw new NotImplementedException();

            IEnumerable<CachedTagInstance> dependencies = CacheContext.TagCache.Index.FindDependencies(instance);

            List<string> commands = new List<string>();

            // Console.WriteLine("All deps:");
            foreach (var dep in dependencies)
            {
                // To avoid porting a ton of existing textures, bitmaps under 0x5726 should be ignored

                // For stability and first runs, extract all. Filter out potentially existing tags later.
                // if (dep.Group.ToString() == "bitm" && dep.Index < 0x5726)
                // {
                //     // Ignore default bitmaps for now
                // }

                // These are common for all the shaders, so chances are small to see they get removed.
                if (dep.Group.Tag == "rmdf" || dep.Group.Tag == "rmop" || dep.Group.Tag == "glps" || dep.Group.Tag == "glvs")
                    continue;

                string depname = CacheContext.TagNames.ContainsKey(dep.Index) ? CacheContext.TagNames[dep.Index] : $"0x{dep.Index:X4}";
                string exportedTagName = $"{dep.Index:X4}";

                // if (!CacheContext.TagNames.ContainsKey(dep.Index))
                //     throw new Exception($"0x{dep.Index:X4} isn't named.");

                Console.WriteLine($"extracttag 0x{dep.Index:X4} {exportedTagName}.{dep.Group.Tag}");

                commands.Add($"createtag cfgt");
                commands.Add($"NameTag * {depname}");
                commands.Add($"importtag * {exportedTagName}.{dep.Group.Tag}");

                // Console.WriteLine($"createtag cfgt");
                // Console.WriteLine($"NameTag * {depname}");
                // Console.WriteLine($"importtag * {exportedTagName}.{dep.Group.Tag}");

                // Console.WriteLine($"Echo If the program quits at this point, the tagname is invalid.");
                // Console.WriteLine($"EditTag {depname}.{dep.Group.Tag}");
                // Console.WriteLine($"Exit");
                // Console.WriteLine($"Dumplog {modName}.log");
            }

            Console.WriteLine("");
            foreach (var a in commands)
                Console.WriteLine(a);

            RenderModel modeTag;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, instance);
                modeTag = CacheContext.Deserializer.Deserialize<RenderModel>(edContext);
            }

            var modename = CacheContext.TagNames[instance.Index];

            List<CachedTagInstance> shadersList = new List<CachedTagInstance>();

            Console.WriteLine("");

            Console.WriteLine($"EditTag {modename}.{instance.Group.Tag}");

            int i = -1;
            foreach (var material in modeTag.Materials)
            {
                i++;
                var shadername = CacheContext.TagNames[material.RenderMethod.Index];
                Console.WriteLine($"SetField Materials[{i}].RenderMethod {shadername}.{material.RenderMethod.Group.Tag}");

                shadersList.Add(material.RenderMethod);
            }

            Console.WriteLine($"SaveTagChanges");
            Console.WriteLine($"ExitTo tags");

            foreach (var shaderInstance in shadersList)
            {
                ShaderDecal shaderTag;
                using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, shaderInstance);
                    shaderTag = CacheContext.Deserializer.Deserialize<ShaderDecal>(edContext);
                }

                var shaderName = CacheContext.TagNames[shaderInstance.Index];
                var rmdfName = CacheContext.TagNames.ContainsKey(shaderTag.BaseRenderMethod.Index) ? CacheContext.TagNames[shaderTag.BaseRenderMethod.Index] : $"0x{shaderTag.BaseRenderMethod.Index:X4}";
                var rmt2Name = CacheContext.TagNames[shaderTag.ShaderProperties[0].Template.Index];

                // Manage rmt2
                RenderMethodTemplate rmt2Tag;
                using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, shaderTag.ShaderProperties[0].Template);
                    rmt2Tag = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(edContext);
                }

                var vtshName = CacheContext.TagNames[rmt2Tag.VertexShader.Index];
                var pixlName = CacheContext.TagNames[rmt2Tag.PixelShader.Index];

                Console.WriteLine("");
                Console.WriteLine($"EditTag {rmt2Name}.rmt2");
                Console.WriteLine($"SetField VertexShader {vtshName}.vtsh");
                Console.WriteLine($"SetField PixelShader {pixlName}.pixl");
                Console.WriteLine($"SaveTagChanges");
                Console.WriteLine($"ExitTo tags");

                // Manage bitmaps
                int j = -1;

                Console.WriteLine("");
                Console.WriteLine($"EditTag {shaderName}.{shaderInstance.Group.Tag}");
                Console.WriteLine($"SetField BaseRenderMethod {rmdfName}.rmdf");
                Console.WriteLine($"SetField ShaderProperties[0].Template {rmt2Name}.rmt2");
                foreach (var a in shaderTag.ShaderProperties[0].ShaderMaps)
                {
                    j++;
                    var bitmapName = CacheContext.TagNames.ContainsKey(a.Bitmap.Index) ? CacheContext.TagNames[a.Bitmap.Index] : $"0x{a.Bitmap.Index:X4}";
                    Console.WriteLine($"SetField ShaderProperties[0].ShaderMaps[{j}].Bitmap {bitmapName}.bitm");
                }
                Console.WriteLine($"SaveTagChanges");
                Console.WriteLine($"ExitTo tags");
            }

            Console.WriteLine("");
            Console.WriteLine($"SaveTagNames");
            Console.WriteLine($"Dumplog {modName}.log");
            Console.WriteLine($"Exit");

            return true;
        }

        public bool Temp(List<string> args)
        {
            var tags = CacheContext.TagCache.Index.FindAllInGroup("rmt2");

            foreach (var tag in tags)
            {
                RenderMethodTemplate edRmt2;
                using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, tag);
                    edRmt2 = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(edContext);
                }

                Console.WriteLine($"A:{edRmt2.Arguments.Count:D2} S:{edRmt2.ShaderMaps.Count:D2} 0x{tag.Index:X4} ");
            }

            return true;
        }

        public bool ShadowFix(List<string> args)
        {
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                if (!CacheContext.TryGetTag<Model>(args[0], out var hlmtInstance))
                {
                    Console.WriteLine($"ERROR: tag group must be 'hlmt'. Supplied tag group was '{hlmtInstance.Group.Tag}'.");
                    return false;
                }

                var edContext = new TagSerializationContext(cacheStream, CacheContext, hlmtInstance);
                var hlmtDefinition = CacheContext.Deserializer.Deserialize<Model>(edContext);

                hlmtDefinition.CollisionRegions.Add(
                    new Model.CollisionRegion
                    {
                        Permutations = new List<Model.CollisionRegion.Permutation>
                        {
                            new Model.CollisionRegion.Permutation()
                        }
                    });

                edContext = new TagSerializationContext(cacheStream, CacheContext, hlmtInstance);
                CacheContext.Serializer.Serialize(edContext, hlmtDefinition);

                edContext = new TagSerializationContext(cacheStream, CacheContext, hlmtDefinition.RenderModel);
                var modeDefinition = CacheContext.Deserializer.Deserialize<RenderModel>(edContext);

                var resourceContext = new ResourceSerializationContext(modeDefinition.Geometry.Resource);
                var geometryResource = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

                geometryResource.IndexBuffers.Add(new TagStructureReference<IndexBufferDefinition>
                {
                    RuntimeAddress = 0,
                    DefinitionAddress = 0,
                    Definition = new IndexBufferDefinition
                    {
                        Format = IndexBufferFormat.TriangleStrip,
                        Data = new TagData
                        {
                            Size = 0x6,
                            Address = geometryResource.IndexBuffers[0].Definition.Data.Address
                        }
                    }
                });

                geometryResource.VertexBuffers.Add(new TagStructureReference<VertexBufferDefinition>
                {
                    Definition = new VertexBufferDefinition
                    {
                        Count = 3,
                        VertexSize = 0x38,
                        Data = new TagData
                        {
                            Size = 0xA8,
                            Address = geometryResource.VertexBuffers[0].Definition.Data.Address
                        }
                    }
                });

                geometryResource.VertexBuffers.Add(new TagStructureReference<VertexBufferDefinition>
                {
                    Definition = new VertexBufferDefinition
                    {
                        Count = 3,
                        VertexSize = 0x38,
                        Data = new TagData
                        {
                            Size = 0xA8,
                            Address = geometryResource.VertexBuffers[1].Definition.Data.Address
                        }
                    }
                });

                CacheContext.Serializer.Serialize(resourceContext, geometryResource);

                modeDefinition.Geometry.Meshes.Add(new Mesh
                {
                    VertexBufferIndices = new ushort[] { (ushort)(geometryResource.VertexBuffers.Count - 2), 0xFFFF, 0xFFFF, (ushort)(geometryResource.VertexBuffers.Count - 1), 0xFFFF, 0xFFFF, 0xFFFF, 0xFFFF },
                    IndexBufferIndices = new ushort[] { (ushort)(geometryResource.IndexBuffers.Count - 1), 0xFFFF },
                    Type = VertexType.Rigid,
                    PrtType = PrtType.Ambient,
                    IndexBufferType = PrimitiveType.TriangleStrip,
                    RigidNodeIndex = 0,
                    Parts = new List<Mesh.Part>
                    {
                        new Mesh.Part
                        {
                            TransparentSortingIndex = -1,
                            SubPartCount = 1,
                            TypeNew = Mesh.Part.PartTypeNew.OpaqueShadowCasting,
                            FlagsNew = Mesh.Part.PartFlagsNew.PerVertexLightmapPart,
                            VertexCount = 3
                        },
                    },
                    SubParts = new List<Mesh.SubPart>
                    {
                        new Mesh.SubPart
                        {
                            FirstIndex = 0,
                            IndexCount = 3,
                            PartIndex = 0,
                            VertexCount = 0
                        }
                    }
                });

                modeDefinition.Regions.Add(
                    new RenderModel.Region
                    {
                        Permutations = new List<RenderModel.Region.Permutation>
                        {
                            new RenderModel.Region.Permutation
                            {
                                MeshIndex = (short)(modeDefinition.Geometry.Meshes.Count - 1)
                            }
                        }
                    });

                edContext = new TagSerializationContext(cacheStream, CacheContext, hlmtDefinition.RenderModel);
                CacheContext.Serializer.Serialize(edContext, modeDefinition);
            }

            return true;
        }

        [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x20)]
        public class RenderMethodFast
        {
            public CachedTagInstance BaseRenderMethod;
            public List<RenderMethod.UnknownBlock> Unknown;
        }

        public bool NameRmt2()
        {
            var validShaders = new List<string> { "rmsh", "rmtr", "rmhg", "rmfl", "rmcs", "rmss", "rmd ", "rmw ", "rmzo", "ltvl", "prt3", "beam", "decs", "cntl", "rmzo", "rmct", "rmbk" };
            var newlyNamedRmt2 = new List<int>();
            var type = "invalid";
            var rmt2Instance = -1;

            // Unname rmt2 tags
            foreach (var edInstance in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
                CacheContext.TagNames[edInstance.Index] = "blank";

            foreach (var edInstance in CacheContext.TagCache.Index.NonNull())
            {
                object rm = null;
                RenderMethod renderMethod = null;

                // ignore tag groups not in validShaders
                if (!validShaders.Contains(edInstance.Group.Tag.ToString()))
                    continue;

                // Console.WriteLine($"Checking 0x{edInstance:x4} {edInstance.Group.Tag.ToString()}");

                // Get the tagname type per tag group
                switch (edInstance.Group.Tag.ToString())
                {
                    case "rmsh": type = "shader"; break;
                    case "rmtr": type = "terrain"; break;
                    case "rmhg": type = "halogram"; break;
                    case "rmfl": type = "foliage"; break;
                    case "rmss": type = "screen"; break;
                    case "rmcs": type = "custom"; break;
                    case "prt3": type = "particle"; break;
                    case "beam": type = "beam"; break;
                    case "cntl": type = "contrail"; break;
                    case "decs": type = "decal"; break;
                    case "ltvl": type = "light_volume"; break;
                    case "rmct": type = "cortana"; break;
                    case "rmbk": type = "black"; break;
                    case "rmzo": type = "zonly"; break;
                    case "rmd ": type = "decal"; break;
                    case "rmw ": type = "water"; break;
                }

                switch (edInstance.Group.Tag.ToString())
                {
                    case "rmsh":
                    case "rmhg":
                    case "rmtr":
                    case "rmcs":
                    case "rmfl":
                    case "rmss":
                    case "rmct":
                    case "rmzo":
                    case "rmbk":
                    case "rmd ":
                    case "rmw ":
                        using (var cacheStream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                        using (var cacheReader = new EndianReader(cacheStream))
                        {
                            var edContext = new TagSerializationContext(cacheStream, CacheContext, edInstance);
                            var edDefinition = CacheContext.Deserializer.Deserialize<RenderMethodFast>(new TagSerializationContext(cacheStream, CacheContext, edInstance));

                            if (edDefinition.Unknown == null || edDefinition.Unknown.Count == 0)
                                continue;

                            renderMethod = new RenderMethod
                            {
                                Unknown = edDefinition.Unknown
                            };
                        }

                        foreach (var a in edInstance.Dependencies)
                            if (CacheContext.GetTag(a).Group.ToString() == "rmt2")
                                rmt2Instance = CacheContext.GetTag(a).Index;

                        if (rmt2Instance == 0)
                            throw new Exception();

                        NameRmt2Part(type, renderMethod, edInstance, rmt2Instance, newlyNamedRmt2);
                        continue;

                    default:
                        break;
                }

                using (var cacheStream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, edInstance);
                    rm = CacheContext.Deserializer.Deserialize(new TagSerializationContext(cacheStream, CacheContext, edInstance), TagDefinition.Find(edInstance.Group.Tag));
                }

                switch (edInstance.Group.Tag.ToString())
                {
                    case "prt3": var e = (Particle)rm; NameRmt2Part(type, e.RenderMethod, edInstance, e.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "beam": var a = (BeamSystem)rm; foreach (var f in a.Beam) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "cntl": var b = (ContrailSystem)rm; foreach (var f in b.Contrail) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "decs": var c = (DecalSystem)rm; foreach (var f in c.Decal) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "ltvl": var d = (LightVolumeSystem)rm; foreach (var f in d.LightVolume) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;

                    default:
                        break;
                }
            }


            return true;
        }

        private void NameRmt2Part(string type, RenderMethod renderMethod, CachedTagInstance edInstance, int rmt2Instance, List<int> newlyNamedRmt2)
        {
            if (renderMethod.Unknown.Count == 0) // invalid shaders, most likely caused by ported shaders
                return;

            if (newlyNamedRmt2.Contains(rmt2Instance))
                return;
            else
                newlyNamedRmt2.Add(rmt2Instance);

            var newTagName = $"shaders\\{type}_templates\\";

            var rmdfRefValues = "";

            for (int i = 0; i < renderMethod.Unknown.Count; i++)
            {
                if (edInstance.Group.Tag.ToString() == "rmsh" && i > 9) // for better H3/ODST name matching
                    break;

                if (edInstance.Group.Tag.ToString() == "rmhg" && i > 6) // for better H3/ODST name matching
                    break;

                rmdfRefValues = $"{rmdfRefValues}_{renderMethod.Unknown[i].Unknown}";
            }

            newTagName = $"{newTagName}{rmdfRefValues}";

            CacheContext.TagNames[rmt2Instance] = newTagName;
            // Console.WriteLine($"0x{rmt2Instance:X4} {newTagName}");
        }

        public bool AdjustScripts(List<string> args)
        {
            var helpMessage =
                @"Usage: " +
                @"test AdjustScripts levels\multi\guardian\guardian";

            if (args.Count != 1)
            {
                Console.WriteLine(helpMessage);
                Console.WriteLine("args.Count != 1");
                return false;
            }

            var edTagArg = args[0];

            if (!CacheContext.TryGetTag(edTagArg, out var edTag))
            {
                Console.WriteLine($"ERROR: cannot find tag {edTag}");
                Console.WriteLine(helpMessage);
                return false;
            }

            if (!edTag.IsInGroup("scnr"))
            {
                Console.WriteLine($"ERROR: tag is not a scenario {edTag}");
                Console.WriteLine(helpMessage);
                return false;
            }

            if (!CacheContext.TagNames.ContainsKey(edTag.Index))
            {
                Console.WriteLine($"CacheContext.TagNames.ContainsKey(edTag.Index) {edTag.Index:X4}");
                return false;
            }

            var tagName = CacheContext.TagNames[edTag.Index].Split("\\".ToCharArray()).Last();

            if (!DisabledScriptsString.ContainsKey(tagName))
            {
                Console.WriteLine("!DisabledScriptsString.ContainsKey(tagName)");
                return false;
            }

            Scenario scnr;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                scnr = CacheContext.Deserializer.Deserialize<Scenario>(edContext);
            }

            foreach (var line in DisabledScriptsString[tagName])
            {
                var items = line.Split(",".ToCharArray());

                var scriptIndex = Convert.ToInt32(items[0]);

                uint.TryParse(items[2], NumberStyles.HexNumber, null, out uint NextExpressionHandle);
                ushort.TryParse(items[3], NumberStyles.HexNumber, null, out ushort Opcode);
                byte.TryParse(items[4].Substring(0, 2), NumberStyles.HexNumber, null, out byte data0);
                byte.TryParse(items[4].Substring(2, 2), NumberStyles.HexNumber, null, out byte data1);
                byte.TryParse(items[4].Substring(4, 2), NumberStyles.HexNumber, null, out byte data2);
                byte.TryParse(items[4].Substring(6, 2), NumberStyles.HexNumber, null, out byte data3);

                scnr.ScriptExpressions[scriptIndex].NextExpressionHandle = NextExpressionHandle;
                scnr.ScriptExpressions[scriptIndex].Opcode = Opcode;
                scnr.ScriptExpressions[scriptIndex].Data[0] = data0;
                scnr.ScriptExpressions[scriptIndex].Data[1] = data1;
                scnr.ScriptExpressions[scriptIndex].Data[2] = data2;
                scnr.ScriptExpressions[scriptIndex].Data[3] = data3;
            }

            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edTag);
                CacheContext.Serializer.Serialize(context, scnr);
            }

            return true;

        }

        public bool AdjustScriptsFromFile(List<string> args)
        {
            var helpMessage =
                @"Usage: " +
                @"test AdjustScripts levels\multi\guardian\guardian guardian.csv";

            if (args.Count != 2)
            {
                Console.WriteLine("ERROR: args.Count != 2");
                Console.WriteLine(helpMessage);
                return false;
            }

            var edTagArg = args[0];
            var file = args[1];

            if (!CacheContext.TryGetTag(edTagArg, out var edTag))
            {
                Console.WriteLine($"ERROR: cannot find tag {edTag}");
                Console.WriteLine(helpMessage);
                return false;
            }

            if (!edTag.IsInGroup("scnr"))
            {
                Console.WriteLine($"ERROR: tag is not a scenario {edTag}");
                Console.WriteLine(helpMessage);
                return false;
            }

            var file_ = new FileInfo(file);

            if (!File.Exists(file))
            {
                Console.WriteLine($"ERROR: file does not exist: {file}");
                Console.WriteLine(helpMessage);
                return false;
            }

            Scenario scnr;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                scnr = CacheContext.Deserializer.Deserialize<Scenario>(edContext);
            }

            var lines = new List<string>();

            using (var csvStream = file_.OpenRead())
            using (var csvReader = new StreamReader(csvStream))
            {
                var line = "";
                while (line != null)
                {
                    line = csvReader.ReadLine();

                    if (line == null)
                        break;

                    if (line == "// STOP")
                        break;

                    if (line.StartsWith("//"))
                        continue;

                    if (line == "")
                        continue;

                    lines.Add(line);
                }
            }

            foreach (var line in lines)
            {
                var items = line.Split(",".ToCharArray());

                var scriptIndex = Convert.ToInt32(items[0]);

                uint.TryParse(items[2], NumberStyles.HexNumber, null, out uint NextExpressionHandle);
                ushort.TryParse(items[3], NumberStyles.HexNumber, null, out ushort Opcode);
                byte.TryParse(items[4].Substring(0, 2), NumberStyles.HexNumber, null, out byte data0);
                byte.TryParse(items[4].Substring(2, 2), NumberStyles.HexNumber, null, out byte data1);
                byte.TryParse(items[4].Substring(4, 2), NumberStyles.HexNumber, null, out byte data2);
                byte.TryParse(items[4].Substring(6, 2), NumberStyles.HexNumber, null, out byte data3);

                scnr.ScriptExpressions[scriptIndex].NextExpressionHandle = NextExpressionHandle;
                scnr.ScriptExpressions[scriptIndex].Opcode = Opcode;
                scnr.ScriptExpressions[scriptIndex].Data[0] = data0;
                scnr.ScriptExpressions[scriptIndex].Data[1] = data1;
                scnr.ScriptExpressions[scriptIndex].Data[2] = data2;
                scnr.ScriptExpressions[scriptIndex].Data[3] = data3;
            }

            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edTag);
                CacheContext.Serializer.Serialize(context, scnr);
            }

            return true;
        }

        public bool BatchTagDepAdd(List<string> args)
        {
            var helpMessage =
                "Usage: " +
                "test BatchTagDepAdd 0x0 0x1234 0x4567 rmsh" +
                "test BatchTagDepAdd <main tag> <first tag dep> <last tag dep> <tag class>" +
                "Add new tag dependencies to the first specified tag. Add all the tags between the second and the last specified tags.";

            if (args.Count != 4)
            {
                Console.WriteLine(helpMessage);
                Console.WriteLine("args.Count != 4");
                return false;
            }

            var tag1arg = args[0];
            var tag2arg = args[1];
            var tag3arg = args[2];
            var tagClas = args[3];

            if (!CacheContext.TryGetTag(tag1arg, out var tag1))
            {
                Console.WriteLine($"ERROR: cannot find tag {tag1}");
                Console.WriteLine(helpMessage);
                return false;
            }

            if (!CacheContext.TryGetTag(tag2arg, out var tag2))
            {
                Console.WriteLine($"ERROR: cannot find tag {tag2}");
                Console.WriteLine(helpMessage);
                return false;
            }

            if (!CacheContext.TryGetTag(tag3arg, out var tag3))
            {
                Console.WriteLine($"ERROR: cannot find tag {tag3}");
                Console.WriteLine(helpMessage);
                return false;
            }

            var dependencies = new List<int>();

            // foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup(args[4]))
            foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup(tagClas))
            {
                if (tag.Index < tag2.Index)
                    continue;

                if (tag.Index > tag3.Index)
                    break;

                dependencies.Add(tag.Index);
            }

            // Based on TagDependencyCommand
            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                var data = CacheContext.TagCache.ExtractTag(stream, tag1);

                foreach (var dependency in dependencies)
                {
                    if (data.Dependencies.Add(dependency))
                        Console.WriteLine("Added dependency on tag {0:X8}.", dependency);
                    else
                        Console.Error.WriteLine("Tag {0:X8} already depends on tag {1:X8}.", tag1.Index, dependency);
                }

                CacheContext.TagCache.SetTagData(stream, tag1, data);
            }

            return true;
        }

        private static Dictionary<string, List<string>> DisabledScriptsString = new Dictionary<string, List<string>>
        {
            ["005_intro"] = new List<string>
            {
                // default scripts:
                "00000308,E4A70134,E4A90136,0376,3501A8E4,Group,Void,cinematic_skip_stop_internal,",
                
                // modified scripts:
                "00003019,EF3E0BCB,EF440BD1,0424,CC0B3FEF,Group,Void,chud_show_shield,",
                "00000319,E4B2013F,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,",
                "00002221,EC2008AD,EC2F08BC,0053,AE0821EC,ScriptReference,Void,",
            },
        };

        public static List<string> Halo3MPCommonCacheFiles = new List<string> {
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\guardian.map"   ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\riverworld.map" ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\bunkerworld.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\chill.map"      ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\cyberdyne.map"  ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\deadlock.map"   ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\shrine.map"     ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\zanzibar.map"   ,
        };

        public static List<string> Halo3MPUncommonCacheFiles = new List<string> {
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\armory.map"     ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\chillout.map"   ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\construct.map"  ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\descent.map"    ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\docks.map"      ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\fortress.map"   ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\ghosttown.map"  ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\isolation.map"  ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\lockout.map"    ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\midship.map"    ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\salvation.map"  ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\sandbox.map"    ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\sidewinder.map" ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\snowbound.map"  ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\spacecamp.map"  ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Mythic\maps\warehouse.map"  ,
        };

        public static List<string> Halo3CampaignCacheFiles = new List<string> {
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\005_intro.map"    ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\010_jungle.map"   ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\020_base.map"     ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\030_outskirts.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\040_voi.map"      ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\050_floodvoi.map" ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\070_waste.map"    ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\100_citadel.map"  ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\110_hc.map"       ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\120_halo.map"     ,
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\130_epilogue.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3Campaign\maps\mainmenu.map"   ,
        };

        public static List<string> Halo3ODSTCacheFiles = new List<string> {
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\mainmenu.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\c100.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\c200.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\h100.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\l200.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\l300.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\sc100.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\sc110.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\sc120.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\sc130.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\sc140.map",
            @"D:\FOLDERS\Xenia\ISO\Halo3ODST\maps\sc150.map",
        };

        private bool IsDiffOrNull(CachedTagInstance a, CachedTagInstance b, CacheFile BlamCache)
        {
            // a is ed tag, b is blam tag
            if ((a != null && b == null) ||
                (a == null && b != null))
                return false;

            if (a == null && b == null)
                return true;

            if (!CacheContext.TagNames.ContainsKey(a.Index))
            {
                CacheContext.TagNames[a.Index] = BlamCache.IndexItems.GetItemByID(b.Index).Name;

                if (debugConsoleWrite)
                    Console.WriteLine($"0x{a.Index:X4},{CacheContext.TagNames[a.Index]}");
            }

            return true;
        }

        private class Item
        {
            public int TagIndex;
            public string Tagname;
            public string ModeName;
            public uint Checksum;
        }

        [TagStructure(Name = "render_model", Tag = "mode", Size = 0x1CC, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Name = "render_model", Tag = "mode", Size = 0x1D0, MinVersion = CacheVersion.HaloOnline106708)]
        public class RenderModel_materials
        {
            public StringId Name;
            public int Padding0;
            public int Padding1;
            public int Padding2;
            public int Padding3;
            public int Padding4;
            public int Padding5;
            public int Padding6;
            public int Padding7;
            public int Padding8;
            public int Padding9;
            public int PaddingA;
            public int PaddingB;
            public int PaddingC;
            public int PaddingD;
            public int PaddingE;
            public int PaddingF;
            public int Padding10;
            public List<RenderMaterial> Materials;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        public bool NameGlobalMaterials(List<string> args)
        {
            debugConsoleWrite = true;

            var helpMessage = "Required arguments: 1." +
                    "Usage: use H3 matg tag to name all HO matg subtags (effects tagblock only)" +
                    "Usage:" +
                    "test NameGlobalMaterials \"D:\\FOLDERS\\Xenia\\ISO\\Halo3Campaign\\maps\"";

            if (args.Count < 1)
            {
                Console.WriteLine(helpMessage);
                return false;
            }

            var mainPath = args[0];

            if (!Directory.Exists(mainPath))
            {
                Console.WriteLine(helpMessage);
                return false;
            }

            var cacheFiles = Directory.EnumerateFiles(mainPath).ToList();
            if (cacheFiles.Count == 0)
            {
                Console.WriteLine(helpMessage);
                return false;
            }

            var BlamCache = OpenCacheFile(cacheFiles.Find(x => x.Contains("guardian"))); // don't pick [0], as it might pick mainmenu
            if (BlamCache == null)
                BlamCache = OpenCacheFile(cacheFiles.Find(x => x.Contains("010_jungle")));
            if (BlamCache == null)
                BlamCache = OpenCacheFile(cacheFiles.Find(x => x.Contains("h100")));
            if (BlamCache == null)
                BlamCache = OpenCacheFile(cacheFiles[0]);

            var blamDeserializer = new TagDeserializer(BlamCache.Version);

            foreach (var item in BlamCache.IndexItems)
            {
                if (item.Name != "globals\\globals")
                    continue;

                var blamContext = new CacheSerializationContext(ref BlamCache, item);
                var bmGlobals = blamDeserializer.Deserialize<Globals>(blamContext);

                if (!CacheContext.TryGetTag($"globals\\globals.matg", out var edInstance))
                    throw new Exception();

                if (edInstance == null)
                    throw new Exception();

                using (var cacheStream = CacheContext.OpenTagCacheRead())
                {
                    var tagContext = new TagSerializationContext(cacheStream, CacheContext, edInstance);
                    var edGlobals = CacheContext.Deserialize<Globals>(tagContext);

                    for (int i = 0; i < bmGlobals.Materials.Count; i++)
                    {
                        if (i >= edGlobals.Materials.Count)
                            continue;

                        IsDiffOrNull(edGlobals.Materials[i].BreakableSurface, bmGlobals.Materials[i].BreakableSurface, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].EffectSweetenerGrinding, bmGlobals.Materials[i].EffectSweetenerGrinding, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].EffectSweetenerLarge, bmGlobals.Materials[i].EffectSweetenerLarge, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].EffectSweetenerMedium, bmGlobals.Materials[i].EffectSweetenerMedium, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].EffectSweetenerMelee, bmGlobals.Materials[i].EffectSweetenerMelee, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].EffectSweetenerRolling, bmGlobals.Materials[i].EffectSweetenerRolling, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].EffectSweetenerSmall, bmGlobals.Materials[i].EffectSweetenerSmall, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].MaterialEffects, bmGlobals.Materials[i].MaterialEffects, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].SoundSweetenerGrinding, bmGlobals.Materials[i].SoundSweetenerGrinding, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].SoundSweetenerLarge, bmGlobals.Materials[i].SoundSweetenerLarge, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].SoundSweetenerMedium, bmGlobals.Materials[i].SoundSweetenerMedium, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].SoundSweetenerMeleeLarge, bmGlobals.Materials[i].SoundSweetenerMeleeLarge, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].SoundSweetenerMeleeMedium, bmGlobals.Materials[i].SoundSweetenerMeleeMedium, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].SoundSweetenerMeleeSmall, bmGlobals.Materials[i].SoundSweetenerMeleeSmall, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].SoundSweetenerRolling, bmGlobals.Materials[i].SoundSweetenerRolling, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].SoundSweetenerSmall, bmGlobals.Materials[i].SoundSweetenerSmall, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].WaterRippleLarge, bmGlobals.Materials[i].WaterRippleLarge, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].WaterRippleMedium, bmGlobals.Materials[i].WaterRippleMedium, BlamCache);
                        IsDiffOrNull(edGlobals.Materials[i].WaterRippleSmall, bmGlobals.Materials[i].WaterRippleSmall, BlamCache);
                    }
                }

                return true;
            }

            return true;
        }

        public bool NameAnyTagSubtags(List<string> args)
        {
            debugConsoleWrite = true;

            var helpMessage = "Required arguments: 2." +
                    "\nUsage: use H3/ODST cache files to name subtags of ED commonly named tags." +
                    "\nUsage:" +
                    "\ntest NameAnyTagSubtags <tag class> <H3/ODST maps path>" +
                    "\nExample: test NameAnyTagSubtags effe \"D:\\FOLDERS\\Xenia\\ISO\\Halo3Campaign\\maps\"";

            if (args.Count < 2)
            {
                Console.WriteLine("ERROR: args.Count < 2");
                Console.WriteLine(helpMessage);
                return false;
            }

            var tagClass = args[0];
            var mainPath = args[1];

            if (!Directory.Exists(mainPath))
            {
                Console.WriteLine($"ERROR: !Directory.Exists({mainPath})");
                Console.WriteLine(helpMessage);
                return false;
            }

            var cacheFiles = Directory.EnumerateFiles(mainPath).ToList();
            if (cacheFiles.Count == 0)
            {
                Console.WriteLine("ERROR: cacheFiles.Count == 0");
                Console.WriteLine(helpMessage);
                return false;
            }

            var verifiedBlamTags = new List<string>();

            CacheContext.TryParseGroupTag(tagClass, out var groupTag);

            foreach (var cacheFile in cacheFiles)
            {
                if (!cacheFile.Contains(".map"))
                    continue;

                if (cacheFile.Contains("campaign") || cacheFile.Contains("shared"))
                    continue;

                var BlamCache = OpenCacheFile(cacheFile);
                if (BlamCache == null)
                    continue;

                foreach (var bmInstance in BlamCache.IndexItems)
                {
                    if (bmInstance.ClassIndex == -1)
                        continue;

                    if (bmInstance.GroupTag.ToString() != groupTag.ToString())
                        continue;

                    if (verifiedBlamTags.Contains(bmInstance.Name))
                        continue;

                    verifiedBlamTags.Add(bmInstance.Name);

                    var tagname = $"{bmInstance.Name}.{bmInstance.GroupName}";

                    if (!CacheContext.TryGetTag(tagname, out var edInstance)) // a bit risky because some equivalent HO tags have different subtags
                        continue;

                    if (edInstance == null)
                        continue;

                    object edDef = null;
                    using (var stream = CacheContext.OpenTagCacheRead())
                        edDef = CacheContext.Deserializer.Deserialize(new TagSerializationContext(stream, CacheContext, edInstance), TagDefinition.Find(edInstance.Group.Tag));

                    var blamContext = new CacheSerializationContext(ref BlamCache, bmInstance);
                    var bmDef = BlamCache.Deserializer.Deserialize(blamContext, TagDefinition.Find(bmInstance.GroupTag));

                    CompareBlocksToNameTags(edDef, bmDef, CacheContext, BlamCache, "");

                }
            }

            Console.WriteLine($"Saved new tagnames.");
            CacheContext.SaveTagNames();
            return true;
        }

        public static void CompareBlocksToNameTags(object leftData, object rightData, HaloOnlineCacheContext edContext, CacheFile bmContext, String name)
        {
            if (leftData == null || rightData == null)
                return;

            if (name.Contains("ResourcePageIndex"))
                return;

            if (name == "Geometry")
                return;

            var type = leftData.GetType();

            if (type == typeof(CachedTagInstance))
            {
                var leftTag = (CachedTagInstance)leftData;
                var rightTag = (CachedTagInstance)rightData;

                var leftName = edContext.TagNames.ContainsKey(leftTag.Index) ? edContext.TagNames[leftTag.Index] : "";
                var rightName = bmContext.IndexItems.GetItemByID(rightTag.Index).Name;

                if (!edContext.TagNames.ContainsKey(leftTag.Index))
                {
                    if (debugConsoleWrite)
                        Console.WriteLine($"0x{leftTag.Index:X4},{rightName}");
                    edContext.TagNames[leftTag.Index] = rightName;
                }
            }
            else if (type.IsArray)
            {
                if (type.GetElementType().IsPrimitive)
                    return;

                var leftArray = (Array)leftData;
                var rightArray = (Array)rightData;

                if (leftArray.Length != rightArray.Length)
                    return;

                for (var i = 0; i < leftArray.Length; i++)
                    CompareBlocksToNameTags(leftArray.GetValue(i), rightArray.GetValue(i), edContext, bmContext, name);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var countProperty = type.GetProperty("Count");
                var leftCount = (int)countProperty.GetValue(leftData);
                var rightCount = (int)countProperty.GetValue(rightData);
                if (leftCount != rightCount) 
                    return;

                var getItem = type.GetMethod("get_Item");
                for (var i = 0; i < leftCount; i++)
                {
                    var leftItem = getItem.Invoke(leftData, new object[] { i });
                    var rightItem = getItem.Invoke(rightData, new object[] { i });
                    CompareBlocksToNameTags(leftItem, rightItem, edContext, bmContext, $"{name}[{i}].");
                }
            }
            else if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
            {
                // The objects are structures
                var left = new TagFieldEnumerator(new TagStructureInfo(leftData.GetType(), CacheVersion.HaloOnline106708));
                var right = new TagFieldEnumerator(new TagStructureInfo(rightData.GetType(), CacheVersion.HaloOnline106708));
                while (left.Next() && right.Next())
                {
                    // Keep going on the left until the field is on the right
                    while (!CacheVersionDetection.IsBetween(CacheVersion.HaloOnline106708, left.Attribute.MinVersion, left.Attribute.MaxVersion))
                    {
                        if (!left.Next())
                            return; // probably unused
                    }

                    // Keep going on the right until the field is on the left
                    while (!CacheVersionDetection.IsBetween(CacheVersion.HaloOnline106708, right.Attribute.MinVersion, right.Attribute.MaxVersion))
                    {
                        if (!right.Next())
                            return;
                    }
                    if (left.Field.MetadataToken != right.Field.MetadataToken)
                        throw new InvalidOperationException("WTF, left and right fields don't match!");

                    // Process the fields
                    var leftFieldData = left.Field.GetValue(leftData);
                    var rightFieldData = right.Field.GetValue(rightData);
                    CompareBlocksToNameTags(leftFieldData, rightFieldData, edContext, bmContext, $"{name}{left.Field.Name}");
                }
            }
        }

        public bool CompareEDtoBlam(List<string> args)
        {
            debugConsoleWrite = true;

            var helpMessage = "Required arguments: 3." +
                    "\nUsage: use H3/ODST cache files to name subtags of common named tags." +
                    "\nUsage: test NameAnyTagSubtags <tag class> <tags to compare count> <H3/ODST maps path>" +
                    "\nExample: test NameAnyTagSubtags effe 5 \"D:\\FOLDERS\\Xenia\\ISO\\Halo3Campaign\\maps\"";

            if (args.Count < 3)
            {
                Console.WriteLine("ERROR: args.Count < 3");
                Console.WriteLine(helpMessage);
                return false;
            }

            var tagClass = args[0];
            var comparedTagCount = args[1];
            var mainPath = args[2];

            uint.TryParse(comparedTagCount, NumberStyles.HexNumber, null, out uint comparedTagsCount);

            if (!Directory.Exists(mainPath))
            {
                Console.WriteLine($"ERROR: !Directory.Exists({mainPath})");
                Console.WriteLine(helpMessage);
                return false;
            }

            var cacheFiles = Directory.EnumerateFiles(mainPath).ToList();
            if (cacheFiles.Count == 0)
            {
                Console.WriteLine("ERROR: cacheFiles.Count == 0");
                Console.WriteLine(helpMessage);
                return false;
            }

            CacheContext.TryParseGroupTag(tagClass, out var groupTag);

            var verifiedBlamTags = new List<string>();
            var verifiedFields = new List<string>();

            var i = 0;
            foreach (var cacheFile in cacheFiles)
            {
                if (!cacheFile.Contains(".map"))
                    continue;

                if (cacheFile.Contains("campaign") || cacheFile.Contains("shared"))
                    continue;

                var BlamCache = OpenCacheFile(cacheFile);
                if (BlamCache == null)
                    continue;

                foreach (var bmInstance in BlamCache.IndexItems)
                {
                    if (bmInstance.ClassIndex == -1)
                        continue;

                    if (bmInstance.GroupTag.ToString() != tagClass)
                        continue;

                    var tagname = $"{bmInstance.Name}.{bmInstance.GroupName}";

                    if (verifiedBlamTags.Contains(tagname))
                        continue;

                    verifiedBlamTags.Add(tagname);

                    if (!CacheContext.TryGetTag(tagname, out var edInstance))
                        continue;

                    if (edInstance == null)
                        continue;

                    var blamContext = new CacheSerializationContext(ref BlamCache, bmInstance);
                    var bmDef = BlamCache.Deserializer.Deserialize(blamContext, TagDefinition.Find(bmInstance.GroupTag));

                    object edDef = null;
                    using (var stream = CacheContext.OpenTagCacheRead())
                        edDef = CacheContext.Deserializer.Deserialize(new TagSerializationContext(stream, CacheContext, edInstance), TagDefinition.Find(edInstance.Group.Tag));

                    CompareBlocksBlam(edDef, bmDef, CacheContext, BlamCache, "", verifiedFields);

                    if (verifiedFields.Count != 0)
                    {
                        Console.WriteLine($"");
                        Console.WriteLine($"List of missmatching fields for {tagname} in {cacheFile}");
                        foreach (var a in verifiedFields)
                            Console.WriteLine($"{a}");
                    }

                    i++;
                    if (i > comparedTagsCount)
                        continue;

                    verifiedFields = new List<string>();
                }
            }

            return true;
        }


        public static void CompareBlocksBlam(object leftData, object rightData, HaloOnlineCacheContext edContext, CacheFile bmContext, String name, List<string> verifiedFields)
        {
            // base on MatchTagsCommand

            if (leftData == null || rightData == null)
                return;

            var type = leftData.GetType();

            if (type.Name.Contains("ResourcePageIndex"))
                return;

            if (type.Name.Contains("ResourceGroups"))
                return;

            if (type.Name.Contains("LightmapAirprobe")) // there's an issue with it currently, and the tagblocks were very different anyway
                return;

            if (type.Name.Contains("Geometry")) // just ignore
                return;

            if (name.Contains("ResourcePageIndex"))
                return;

            if (name.Contains("ResourceGroups"))
                return;

            if (name.Contains("LightmapAirprobe")) // there's an issue with it currently, and the tagblocks were very different anyway
                return;

            if (name.Contains("Geometry")) // just ignore
                return;

            if (name.Contains("FunctionData") || name.Contains("Function2Data")) // endian flipped
                return;

            if (type == typeof(CachedTagInstance))
            {
                // If the objects are tags, then we've found a match
                var leftTag = (CachedTagInstance)leftData;
                var rightTag = (CachedTagInstance)rightData;

                var leftName = edContext.TagNames.ContainsKey(leftTag.Index) ? edContext.TagNames[leftTag.Index] : "";
                var rightName = bmContext.IndexItems.GetItemByID(rightTag.Index).Name;

                if (leftName != rightName)
                {
                    Console.WriteLine($"{name},{leftName},{rightName}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }

                if (leftTag.Group.Tag != rightTag.Group.Tag)
                {
                    Console.WriteLine($"{name},{leftName}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.IsArray)
            {
                // ?? 
                // if (type.GetElementType().IsPrimitive)
                // {
                //     switch (Type.GetTypeCode(type))
                //     {
                //         case TypeCode.Int32:
                //         case TypeCode.UInt32:
                //             break;
                // 
                //         default:
                //             break;
                //     }
                // 
                //     return false;
                // }

                // If the objects are arrays, then loop through each element
                var leftArray = (Array)leftData;
                var rightArray = (Array)rightData;

                if (leftArray.Length != rightArray.Length)
                {
                    Console.WriteLine($"{name},{name}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }

                for (var i = 0; i < leftArray.Length; i++)
                    if (i < rightArray.Length)
                        CompareBlocksBlam(leftArray.GetValue(i), rightArray.GetValue(i), edContext, bmContext, name, verifiedFields);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                // ??
                if (type.GenericTypeArguments[0].IsPrimitive)
                {
                }

                // If the objects are lists, then loop through each element
                var countProperty = type.GetProperty("Count");
                var leftCount = (int)countProperty.GetValue(leftData);
                var rightCount = (int)countProperty.GetValue(rightData);
                if (leftCount != rightCount) // If the sizes are different, we probably can't compare them
                {
                    Console.WriteLine($"{name},{leftCount} != {rightCount}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);

                    return;
                }

                var getItem = type.GetMethod("get_Item");
                for (var i = 0; i < leftCount; i++)
                {
                    var leftItem = getItem.Invoke(leftData, new object[] { i });
                    var rightItem = getItem.Invoke(rightData, new object[] { i });
                    CompareBlocksBlam(leftItem, rightItem, edContext, bmContext, $"{name}[{i}].", verifiedFields);
                }
            }
            else if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
            {
                // The objects are structures
                var left = new TagFieldEnumerator(new TagStructureInfo(leftData.GetType(), CacheVersion.HaloOnline106708));
                var right = new TagFieldEnumerator(new TagStructureInfo(rightData.GetType(), CacheVersion.HaloOnline106708));
                while (left.Next() && right.Next())
                {
                    // Keep going on the left until the field is on the right
                    while (!CacheVersionDetection.IsBetween(CacheVersion.HaloOnline106708, left.Attribute.MinVersion, left.Attribute.MaxVersion))
                    {
                        if (!left.Next())
                            return; // probably unused
                    }

                    // Keep going on the right until the field is on the left
                    while (!CacheVersionDetection.IsBetween(CacheVersion.HaloOnline106708, right.Attribute.MinVersion, right.Attribute.MaxVersion))
                    {
                        if (!right.Next())
                            return;
                    }
                    if (left.Field.MetadataToken != right.Field.MetadataToken)
                        throw new InvalidOperationException("WTF, left and right fields don't match!");

                    // Process the fields
                    var leftFieldData = left.Field.GetValue(leftData);
                    var rightFieldData = right.Field.GetValue(rightData);
                    CompareBlocksBlam(leftFieldData, rightFieldData, edContext, bmContext, $"{name}{left.Field.Name}", verifiedFields);
                }
            }
            else if (type.IsEnum)
            {
                var a = leftData.ToString();
                var b = rightData.ToString();
                if (a != b)
                {
                    Console.WriteLine($"{name},{a},{b}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.IsPrimitive)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.SByte:
                        if ((sbyte)leftData != (sbyte)rightData)
                        {
                            Console.WriteLine($"{name},{leftData},{rightData}");

                            if (!verifiedFields.Contains(name))
                                verifiedFields.Add(name);
                        }
                        break;
                    case TypeCode.Byte:
                        if ((byte)leftData != (byte)rightData)
                        {
                            Console.WriteLine($"{name},{leftData},{rightData}");

                            if (!verifiedFields.Contains(name))
                                verifiedFields.Add(name);
                        }
                        break;
                    case TypeCode.Int16:
                        if ((short)leftData != (short)rightData)
                        {
                            Console.WriteLine($"{name},{leftData},{rightData}");

                            if (!verifiedFields.Contains(name))
                                verifiedFields.Add(name);
                        }
                        break;
                    case TypeCode.UInt16:
                        if ((ushort)leftData != (ushort)rightData)
                        {
                            Console.WriteLine($"{name},{leftData},{rightData}");

                            if (!verifiedFields.Contains(name))
                                verifiedFields.Add(name);
                        }
                        break;
                    case TypeCode.Int32:
                        if ((int)leftData != (int)rightData)
                        {
                            Console.WriteLine($"{name},{leftData},{rightData}");

                            if (!verifiedFields.Contains(name))
                                verifiedFields.Add(name);
                        }
                        break;
                    case TypeCode.UInt32:
                        if ((uint)leftData != (uint)rightData)
                        {
                            Console.WriteLine($"{name},{leftData},{rightData}");

                            if (!verifiedFields.Contains(name))
                                verifiedFields.Add(name);
                        }
                        break;
                    case TypeCode.Single: // WARNING passes as error in any case
                        if ($"{(float)leftData}" != $"{(float)rightData}")
                        {
                            Console.WriteLine($"{name},{leftData},{rightData}");

                            if (!verifiedFields.Contains(name))
                                verifiedFields.Add(name);
                        }
                        break;

                    default:
                        break;
                }
            }
            else if (type.Name == "StringId")
            {
                var edString = edContext.StringIdCache.GetString((StringId)leftData);
                var bmString = bmContext.Strings.GetString((StringId)rightData);

                if (edString != bmString)
                {
                    Console.WriteLine($"{name},{edString},{bmString}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "Angle")
            {
                if ((Angle)leftData != (Angle)rightData)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.FullName.StartsWith("TagTool.Common.Bounds"))
            {
                ; // unsupported, needs a fix asap
            }
            else if (type.Name == "RealPoint2d")
            {
                var a = (RealPoint2d)leftData;
                var b = (RealPoint2d)rightData;
                if (a.X != b.X || a.Y != b.Y)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealPoint3d")
            {
                var a = (RealPoint3d)leftData;
                var b = (RealPoint3d)rightData;
                if (a.X != b.X || a.Y != b.Y || a.Z != b.Z)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealArgbColor")
            {
                var a = (RealArgbColor)leftData;
                var b = (RealArgbColor)rightData;
                if (a.Red != b.Red || a.Green != b.Green || a.Blue != b.Blue || a.Alpha != b.Alpha)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealRgbColor")
            {
                var a = (RealRgbColor)leftData;
                var b = (RealRgbColor)rightData;
                if (a.Red != b.Red || a.Green != b.Green || a.Blue != b.Blue)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealEulerAngles3d")
            {
                var a = (RealEulerAngles3d)leftData;
                var b = (RealEulerAngles3d)rightData;
                if (a.Pitch != b.Pitch || a.Roll != b.Roll || a.Yaw != b.Yaw)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealPlane2d")
            {
                var a = (RealPlane2d)leftData;
                var b = (RealPlane2d)rightData;
                if (a.I != b.I || a.J != b.J)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealPlane3d")
            {
                var a = (RealPlane3d)leftData;
                var b = (RealPlane3d)rightData;
                if (a.I != b.I || a.J != b.J || a.K != b.K)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealQuaternion")
            {
                var a = (RealQuaternion)leftData;
                var b = (RealQuaternion)rightData;
                if (a.I != b.I || a.J != b.J || a.K != b.K || a.W != b.W)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealVector2d")
            {
                var a = (RealVector2d)leftData;
                var b = (RealVector2d)rightData;
                if (a.I != b.I || a.J != b.J)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealVector3d")
            {
                var a = (RealVector3d)leftData;
                var b = (RealVector3d)rightData;
                if (a.I != b.I || a.J != b.J || a.K != b.K)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "Point2d")
            {
                var a = (Point2d)leftData;
                var b = (Point2d)rightData;
                if (a.X != b.X || a.Y != b.Y)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "Tag")
            {
                var a = (Tag)leftData;
                var b = (Tag)rightData;
                if (a.Value != b.Value)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "ArgbColor")
            {
                var a = (ArgbColor)leftData;
                var b = (ArgbColor)rightData;
                if (a.Red != b.Red || a.Green != b.Green || a.Blue != b.Blue || a.Alpha != b.Alpha)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealMatrix4x3")
            {
                var a = (RealMatrix4x3)leftData;
                var b = (RealMatrix4x3)rightData;
                if (a.m11 != b.m11 ||
                    a.m12 != b.m12 ||
                    a.m13 != b.m13 ||
                    a.m21 != b.m21 ||
                    a.m22 != b.m22 ||
                    a.m23 != b.m23 ||
                    a.m31 != b.m31 ||
                    a.m32 != b.m32 ||
                    a.m33 != b.m33 ||
                    a.m41 != b.m41 ||
                    a.m42 != b.m42 ||
                    a.m43 != b.m43)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "RealEulerAngles2d")
            {
                var a = (RealEulerAngles2d)leftData;
                var b = (RealEulerAngles2d)rightData;
                if (a.Pitch != b.Pitch || a.Yaw != b.Yaw)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else if (type.Name == "String")
            {
                if ((string)leftData != (string)rightData)
                {
                    Console.WriteLine($"{name},{leftData},{rightData}");

                    if (!verifiedFields.Contains(name))
                        verifiedFields.Add(name);
                }
            }
            else
            {
                Console.WriteLine($"ERROR: {type.Name},{type.FullName}");
            }
        }
    }
}