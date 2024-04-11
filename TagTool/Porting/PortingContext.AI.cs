using System.Collections.Generic;
using System.IO;
using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Porting
{
    public partial class PortingContext
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

        private void MergeCharacter(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTag edTag, CachedTag h3Tag)
        {
            var edDef = CacheContext.Deserialize<Character>(cacheStream, edTag);
            var h3Def = BlamCache.Deserialize<Character>(blamCacheStream, h3Tag);

            var merged = false;

            if (edDef.WeaponsProperties.Count == h3Def.WeaponsProperties.Count)
            {
                for (var i = 0; i < edDef.WeaponsProperties.Count; i++)
                {
                    if (edDef.WeaponsProperties[i].Weapon != null || h3Def.WeaponsProperties[i].Weapon == null)
                        continue;

                    edDef.WeaponsProperties[i].Weapon = ConvertTag(cacheStream, blamCacheStream, resourceStreams, h3Def.WeaponsProperties[i].Weapon);

                    merged = true;
                }
            }

            if (edDef.VehicleProperties.Count == h3Def.VehicleProperties.Count)
            {
                for (var i = 0; i < edDef.VehicleProperties.Count; i++)
                {
                    if (edDef.VehicleProperties[i].Unit != null || h3Def.VehicleProperties[i].Unit == null)
                        continue;

                    edDef.VehicleProperties[i].Unit = ConvertTag(cacheStream, blamCacheStream, resourceStreams, h3Def.VehicleProperties[i].Unit);

                    merged = true;
                }
            }

            if (edDef.EquipmentProperties.Count == h3Def.EquipmentProperties.Count)
            {
                for (var i = 0; i < edDef.EquipmentProperties.Count; i++)
                {
                    if (edDef.EquipmentProperties[i].Equipment != null || h3Def.EquipmentProperties[i].Equipment == null)
                        continue;

                    edDef.EquipmentProperties[i].Equipment = ConvertTag(cacheStream, blamCacheStream, resourceStreams, h3Def.EquipmentProperties[i].Equipment);

                    merged = true;
                }
            }

            if (edDef.FiringPatternProperties.Count == h3Def.FiringPatternProperties.Count)
            {
                for (var i = 0; i < edDef.FiringPatternProperties.Count; i++)
                {
                    if (edDef.FiringPatternProperties[i].Weapon != null || h3Def.FiringPatternProperties[i].Weapon == null)
                        continue;

                    edDef.FiringPatternProperties[i].Weapon = ConvertTag(cacheStream, blamCacheStream, resourceStreams, h3Def.FiringPatternProperties[i].Weapon);

                    merged = true;
                }
            }

            if (edDef.ActivityObjects.Count == h3Def.ActivityObjects.Count)
            {
                for (var i = 0; i < edDef.ActivityObjects.Count; i++)
                {
                    if (edDef.ActivityObjects[i].Crate != null || h3Def.ActivityObjects[i].Crate == null)
                        continue;

                    edDef.ActivityObjects[i].Crate = ConvertTag(cacheStream, blamCacheStream, resourceStreams, h3Def.ActivityObjects[i].Crate);

                    merged = true;
                }
            }

            if (merged)
                CacheContext.Serialize(cacheStream, edTag, edDef);
        }

        private Character ConvertCharacter(Stream cacheStream, Character character)
        {
            if (character.Style == null && character.ParentCharacter != null)
            {
                var parent = CacheContext.Deserialize<Character>(cacheStream, character.ParentCharacter);

                if(parent.Style != null)
                    character.Style = parent.Style;
            }           
            return character;
        }
    }
}