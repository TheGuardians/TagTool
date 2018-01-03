using BlamCore.Cache;
using BlamCore.Commands;
using BlamCore.Common;
using BlamCore.IO;
using BlamCore.Serialization;
using BlamCore.TagDefinitions;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Files
{
    class CleanCacheFilesCommand : Command
    {
        public GameCacheContext CacheContext { get; }

        public CleanCacheFilesCommand(GameCacheContext cacheContext)
            : base(CommandFlags.None,

                  "CleanCacheFiles",
                  "Nulls and removes unused tags and resources from cache.",

                  "CleanCacheFiles",

                  "Nulls and removes unused tags and resources from cache.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                CleanMultiplayerGlobals(stream);

                var retainedTags = new HashSet<int>();
                LoadTagDependencies(CacheContext.TagCache.Index.FindFirstInGroup("cfgt").Index, ref retainedTags);

                foreach (var scnr in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
                {
                    CleanScenario(stream, scnr);
                    LoadTagDependencies(scnr.Index, ref retainedTags);
                }

                var resourceIndices = new Dictionary<ResourceLocation, HashSet<int>>
                {
                    [ResourceLocation.Audio] = new HashSet<int>(),
                    [ResourceLocation.Lightmaps] = new HashSet<int>(),
                    [ResourceLocation.RenderModels] = new HashSet<int>(),
                    [ResourceLocation.Resources] = new HashSet<int>(),
                    [ResourceLocation.ResourcesB] = new HashSet<int>(),
                    [ResourceLocation.Textures] = new HashSet<int>(),
                    [ResourceLocation.TexturesB] = new HashSet<int>()
                };

                for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
                {
                    var tag = CacheContext.TagCache.Index[i];

                    if (tag == null)
                        continue;

                    if (retainedTags.Contains(i))
                    {
                        using (var dataStream = new MemoryStream(CacheContext.TagCache.ExtractTagRaw(stream, tag)))
                        using (var reader = new EndianReader(dataStream))
                        {
                            var dataContext = new DataSerializationContext(reader, null, CacheAddressType.Resource);

                            foreach (var resourcePointerOffset in tag.ResourcePointerOffsets)
                            {
                                reader.BaseStream.Position = resourcePointerOffset;
                                var resourcePointer = reader.ReadUInt32();

                                reader.BaseStream.Position = tag.PointerToOffset(resourcePointer);
                                var resourceDefinition = CacheContext.Deserializer.Deserialize<PageableResource>(dataContext);

                                if (resourceDefinition.Page.Index == -1)
                                    continue;

                                var resourceLocation = resourceDefinition.GetLocation();

                                if (!resourceIndices[resourceLocation].Contains(resourceDefinition.Page.Index))
                                    resourceIndices[resourceLocation].Add(resourceDefinition.Page.Index);
                            }
                        }
                    }
                    else
                    {
                        var tagName = CacheContext.TagNames.ContainsKey(tag.Index) ? CacheContext.TagNames[tag.Index] : $"0x{tag.Index}";
                        var tagGroupName = CacheContext.GetString(tag.Group.Name);

                        Console.Write($"Nulling {tagName}.{tagGroupName}...");
                        CacheContext.TagCache.NullTag(stream, tag);
                        Console.WriteLine("done.");
                    }
                }

                foreach (var entry in resourceIndices)
                {
                    ResourceCache resourceCache = null;

                    try
                    {
                        resourceCache = CacheContext.GetResourceCache(entry.Key);
                    }
                    catch (FileNotFoundException)
                    {
                        continue;
                    }

                    using (var resourceStream = CacheContext.OpenResourceCacheReadWrite(entry.Key))
                    {
                        for (var i = resourceCache.Count - 1; i >= 0; i--)
                        {
                            if (entry.Value.Contains(i))
                                continue;

                            Console.Write($"Nulling {entry.Key} resource {i}...");
                            resourceCache.NullResource(resourceStream, i);
                            Console.WriteLine("done.");
                        }

                        resourceCache.UpdateResourceTable(resourceStream);
                    }
                }
            }

            return true;
        }

        private void LoadTagDependencies(int index, ref HashSet<int> tags)
        {
            var queue = new List<int> { index };

            while (queue.Count != 0)
            {
                var nextQueue = new List<int>();

                foreach (var entry in queue)
                {
                    if (!tags.Contains(entry))
                    {
                        if (CacheContext.TagCache.Index[entry] == null)
                            continue;

                        tags.Add(entry);

                        foreach (var dependency in CacheContext.TagCache.Index[entry].Dependencies)
                        {
                            if (dependency == entry)
                                continue;

                            if (!nextQueue.Contains(dependency))
                                nextQueue.Add(dependency);
                        }
                    }
                }

                queue = nextQueue;
            }
        }

        private List<int> WeaponIndices { get; } = new List<int>
        {
            0x14F7, 0x14F8, 0x14F9, 0x14FE, 0x14FF, 0x1500, 0x1504, 0x1509, 0x150C, 0x150E, 0x151E, 0x1525, 0x157C, 0x157D, 0x157E, 0x157F, 0x1580, 0x159E, 0x15A2, 0x15A3, 0x15A4, 0x15B1, 0x15B2, 0x15B3, 0x15B4, 0x15B5, 0x15B6, 0x15B7, 0x15B8, 0x15B9, 0x15BA, 0x1A45, 0x1A47, 0x1A48, 0x1A49, 0x1A4A, 0x1A4B, 0x1A4C, 0x1A4D, 0x1A4E, 0x1A4F, 0x1A53, 0x1A54, 0x1A55, 0x1A56, 0x1A57, 0x1A58, 0x30FF, 0x3647, 0x3C94, 0x4710, 0x53AE, 0x5682, 0x5690, 0x569F
        };

        private void CleanMultiplayerGlobals(Stream stream)
        {
            var mulgTag = CacheContext.TagCache.Index.FindFirstInGroup("mulg");
            var mulgContext = new TagSerializationContext(stream, CacheContext, mulgTag);
            var mulgDefinition = CacheContext.Deserializer.Deserialize<MultiplayerGlobals>(mulgContext);
            
            #region Universal GameVariantWeapons
            mulgDefinition.Universal[0].GameVariantWeapons = new List<MultiplayerGlobals.UniversalBlock.GameVariantWeapon>
            {
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("battle_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x157C]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("assault_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x151E]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("plasma_pistol"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x14F7]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("spike_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1500]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("smg"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x157D]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("carbine"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x14FE]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("energy_sword"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x159E]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("magnum"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x157E]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("needler"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x14F8]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("plasma_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1525]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("rocket_launcher"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x15B3]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("shotgun"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1A45]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("sniper_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x15B1]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("brute_shot"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x14FF]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("unarmed"),
                    RandomChance = 0,
                    Weapon = CacheContext.TagCache.Index[0x157F]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("beam_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1509]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("spartan_laser"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x15B2]
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
                    Weapon = CacheContext.TagCache.Index[0x150C]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("excavator"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1504]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("flamethrower"),
                    RandomChance = 0,
                    Weapon = CacheContext.TagCache.Index[0x1A55]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("missile_pod"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1A54]
                }
            };
            #endregion

            #region Runtime Weapons
            mulgDefinition.Runtime[0].MultiplayerConstants[0].Weapons = new List<MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon>
            {
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // battle_rifle
                    Weapon2 = CacheContext.TagCache.Index[0x157C],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // carbine
                    Weapon2 = CacheContext.TagCache.Index[0x14FE],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // sniper_rifle
                    Weapon2 = CacheContext.TagCache.Index[0x15B1],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // beam_rifle
                    Weapon2 = CacheContext.TagCache.Index[0x1509],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // spartan_laster
                    Weapon2 = CacheContext.TagCache.Index[0x15B2],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // rocket_launcher
                    Weapon2 = CacheContext.TagCache.Index[0x15B3],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // chaingun_turret
                    Weapon2 = CacheContext.TagCache.Index[0x15B4],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // machinegun_turret
                    Weapon2 = CacheContext.TagCache.Index[0x15B5],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // machinegun_turret_integrated
                    Weapon2 = CacheContext.TagCache.Index[0x15B6],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // plasma_cannon
                    Weapon2 = CacheContext.TagCache.Index[0x150E],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // plasma_cannon_integrated
                    Weapon2 = CacheContext.TagCache.Index[0x15B7],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // needler
                    Weapon2 = CacheContext.TagCache.Index[0x14F8],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // flak_cannon
                    Weapon2 = CacheContext.TagCache.Index[0x14F9],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // gauss_turret
                    Weapon2 = CacheContext.TagCache.Index[0x15B8],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // anti_infantry
                    Weapon2 = CacheContext.TagCache.Index[0x15B9],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // behemoth_chaingun_turret
                    Weapon2 = CacheContext.TagCache.Index[0x15BA],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                }
            };
            #endregion

            #region Universal GameVariantVehicles
            mulgDefinition.Universal[0].GameVariantVehicles = new List<MultiplayerGlobals.UniversalBlock.GameVariantVehicle>
            {
                new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                {
                    Name = CacheContext.GetStringId("warthog"),
                    Vehicle = CacheContext.TagCache.Index[0x151F]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                {
                    Name = CacheContext.GetStringId("ghost"),
                    Vehicle = CacheContext.TagCache.Index[0x1517]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                {
                    Name = CacheContext.GetStringId("scorpion"),
                    Vehicle = CacheContext.TagCache.Index[0x1520]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                {
                    Name = CacheContext.GetStringId("wraith"),
                    Vehicle = CacheContext.TagCache.Index[0x1519]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                {
                    Name = CacheContext.GetStringId("banshee"),
                    Vehicle = CacheContext.TagCache.Index[0x151A]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                {
                    Name = CacheContext.GetStringId("mongoose"),
                    Vehicle = CacheContext.TagCache.Index[0x1596]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                {
                    Name = CacheContext.GetStringId("chopper"),
                    Vehicle = CacheContext.TagCache.Index[0x1518]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                {
                    Name = CacheContext.GetStringId("mauler"),
                    Vehicle = null
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantVehicle
                {
                    Name = CacheContext.GetStringId("hornet"),
                    Vehicle = CacheContext.TagCache.Index[0x1598]
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
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("ghost"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("scorpion"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("wraith"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("mongoose"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("banshee"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("chopper"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("mauler"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("hornet"),
                            SubstitutedVehicle = StringId.Null
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
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("ghost"),
                            SubstitutedVehicle = CacheContext.GetStringId("mongoose")
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("scorpion"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("wraith"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("banshee"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("chopper"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("mauler"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("hornet"),
                            SubstitutedVehicle = StringId.Null
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
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("wraith"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("banshee"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("hornet"),
                            SubstitutedVehicle = StringId.Null
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
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("ghost"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("mongoose"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("banshee"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("chopper"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("mauler"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("hornet"),
                            SubstitutedVehicle = StringId.Null
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
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("ghost"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("scorpion"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("wraith"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("mongoose"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("chopper"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("mauler"),
                            SubstitutedVehicle = StringId.Null
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
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("ghost"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("mongoose"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("chopper"),
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("mauler"),
                            SubstitutedVehicle = StringId.Null
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
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("wraith"),
                            SubstitutedVehicle = StringId.Null
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
                            SubstitutedVehicle = StringId.Null
                        },
                        new MultiplayerGlobals.UniversalBlock.VehicleSet.Substitution
                        {
                            OriginalVehicle = CacheContext.GetStringId("hornet"),
                            SubstitutedVehicle = StringId.Null
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

            #region Runtime Vehicles
            mulgDefinition.Runtime[0].MultiplayerConstants[0].Vehicles = new List<MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Vehicle>
            {
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Vehicle
                {
                    Vehicle2 = CacheContext.TagCache.Index[0x1517],
                    Unknown1 = 2.0f,
                    Unknown2 = 1.5f,
                    Unknown3 = 0.5f,
                    Unknown4 = -1000.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Vehicle
                {
                    Vehicle2 = CacheContext.TagCache.Index[0x151F],
                    Unknown1 = 2.0f,
                    Unknown2 = 1.5f,
                    Unknown3 = 0.5f,
                    Unknown4 = -1000.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Vehicle
                {
                    Vehicle2 = CacheContext.TagCache.Index[0x151A],
                    Unknown1 = 2.0f,
                    Unknown2 = 1.5f,
                    Unknown3 = 0.5f,
                    Unknown4 = -1000.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Vehicle
                {
                    Vehicle2 = CacheContext.TagCache.Index[0x1596],
                    Unknown1 = 1.5f,
                    Unknown2 = 1.5f,
                    Unknown3 = 0.5f,
                    Unknown4 = -1000.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Vehicle
                {
                    Vehicle2 = CacheContext.TagCache.Index[0x1518],
                    Unknown1 = 3.0f,
                    Unknown2 = 1.5f,
                    Unknown3 = 0.25f,
                    Unknown4 = -1000.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Vehicle
                {
                    Vehicle2 = CacheContext.TagCache.Index[0x1598],
                    Unknown1 = 2.0f,
                    Unknown2 = 1.5f,
                    Unknown3 = 0.5f,
                    Unknown4 = -1000.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Vehicle
                {
                    Vehicle2 = CacheContext.TagCache.Index[0x1520],
                    Unknown1 = 3.0f,
                    Unknown2 = 1.5f,
                    Unknown3 = 0.5f,
                    Unknown4 = -1000.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Vehicle
                {
                    Vehicle2 = CacheContext.TagCache.Index[0x1519],
                    Unknown1 = 3.0f,
                    Unknown2 = 1.5f,
                    Unknown3 = 0.5f,
                    Unknown4 = -1000.0f
                },
            };
            #endregion

            CacheContext.Serializer.Serialize(mulgContext, mulgDefinition);
        }

        private void CleanScenario(FileStream stream, CachedTagInstance scnrTag)
        {
            var scnrContext = new TagSerializationContext(stream, CacheContext, scnrTag);
            var scnrDefinition = CacheContext.Deserializer.Deserialize<Scenario>(scnrContext);

            foreach (var weapon in scnrDefinition.WeaponPalette)
                if (!WeaponIndices.Contains(weapon.Object.Index))
                    weapon.Object = null;

            var sandboxWeapons = new List<Scenario.SandboxObject>();

            foreach (var weapon in scnrDefinition.SandboxWeapons)
                if (WeaponIndices.Contains(weapon.Object.Index))
                    sandboxWeapons.Add(weapon);

            scnrDefinition.SandboxWeapons = sandboxWeapons;

            CacheContext.Serializer.Serialize(scnrContext, scnrDefinition);
        }
    }
}