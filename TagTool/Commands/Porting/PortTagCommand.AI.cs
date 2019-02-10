using System.Collections.Generic;
using System.IO;
using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private Style ConvertStyle(Style style)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                //
                // Collect halo 3 behavior states
                //

                var behaviors = new Dictionary<string, bool>();

                for (var i = 0; i < style.Behaviors.Length; i++)
                    for (var j = 0; j < 32 && ((i * 32) + j) < style.BehaviorList.Count; j++)
                        behaviors[style.BehaviorList[(i * 32) + j].BehaviorName] = ((style.Behaviors[i] & (1 << j)) != 0);

                //
                // Clear halo 3 behaviors
                //

                for (var i = 0; i < style.Behaviors.Length; i++)
                    style.Behaviors[i] = 0;

                //
                // Insert odst styles into the halo 3 behavior list
                //

                style.BehaviorList.Insert(14, new Style.BehaviorListBlock { BehaviorName = "squad_patrol_behavior" });
                style.BehaviorList.Insert(28, new Style.BehaviorListBlock { BehaviorName = "fight_positioning" });

                style.BehaviorList.Insert(83, new Style.BehaviorListBlock { BehaviorName = "kungfu_cover" });

                style.BehaviorList.Insert(138, new Style.BehaviorListBlock { BehaviorName = "inspect" });

                style.BehaviorList.Insert(157, new Style.BehaviorListBlock { BehaviorName = "------ENGINEER------" });
                style.BehaviorList.Insert(158, new Style.BehaviorListBlock { BehaviorName = "engineer_control" });
                style.BehaviorList.Insert(159, new Style.BehaviorListBlock { BehaviorName = "engineer_control@slave" });
                style.BehaviorList.Insert(160, new Style.BehaviorListBlock { BehaviorName = "engineer_control@free" });
                style.BehaviorList.Insert(161, new Style.BehaviorListBlock { BehaviorName = "engineer_control@equipment" });
                style.BehaviorList.Insert(162, new Style.BehaviorListBlock { BehaviorName = "engineer_explode" });
                style.BehaviorList.Insert(163, new Style.BehaviorListBlock { BehaviorName = "engineer_broken_detonation" });
                style.BehaviorList.Insert(164, new Style.BehaviorListBlock { BehaviorName = "boost_allies" });

                //
                // Set odst behaviors
                //

                for (var i = 0; i < style.Behaviors.Length; i++)
                {
                    for (var j = 0; j < 32 && ((i * 32) + j) < style.BehaviorList.Count; j++)
                    {
                        var behavior = style.BehaviorList[(i * 32) + j].BehaviorName;

                        if (!behaviors.ContainsKey(behavior) || !behaviors[behavior])
                            continue;

                        style.Behaviors[i] |= 1 << j;
                    }
                }
            }

            return style;
        }

        private void MergeCharacter(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTagInstance edTag, CacheFile.IndexItem h3Tag)
        {
            var edDef = CacheContext.Deserialize<Character>(cacheStream, edTag);

            var h3Def = BlamCache.Deserializer.Deserialize<Character>(
                new CacheSerializationContext(ref BlamCache, h3Tag));

            if (edDef.WeaponsProperties.Count == h3Def.WeaponsProperties.Count)
            {
                for (var i = 0; i < edDef.WeaponsProperties.Count; i++)
                {
                    if (edDef.WeaponsProperties[i].Weapon != null)
                        continue;

                    edDef.WeaponsProperties[i].Weapon = ConvertTag(cacheStream, resourceStreams,
                        BlamCache.GetIndexItemFromID(h3Def.WeaponsProperties[i].Weapon.Index));
                }
            }

            if (edDef.VehicleProperties.Count == h3Def.VehicleProperties.Count)
            {
                for (var i = 0; i < edDef.VehicleProperties.Count; i++)
                {
                    if (edDef.VehicleProperties[i].Unit != null)
                        continue;

                    edDef.VehicleProperties[i].Unit = ConvertTag(cacheStream, resourceStreams,
                        BlamCache.GetIndexItemFromID(h3Def.VehicleProperties[i].Unit.Index));
                }
            }

            if (edDef.EquipmentProperties.Count == h3Def.EquipmentProperties.Count)
            {
                for (var i = 0; i < edDef.EquipmentProperties.Count; i++)
                {
                    if (edDef.EquipmentProperties[i].Equipment != null)
                        continue;

                    edDef.EquipmentProperties[i].Equipment = ConvertTag(cacheStream, resourceStreams,
                        BlamCache.GetIndexItemFromID(h3Def.EquipmentProperties[i].Equipment.Index));
                }
            }
        }

        private Character ConvertCharacter(Character character)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                character.InspectProperties = new List<CharacterInspectProperties>();
                character.EngineerProperties = new List<CharacterEngineerProperties>();
            }

            return character;
        }
    }
}