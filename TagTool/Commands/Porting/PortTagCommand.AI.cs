using System.Collections.Generic;
using TagTool.Ai;
using TagTool.Cache;
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

        private Character ConvertCharacter(Character character)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                character.InspectProperties = new List<CharacterInspectProperties>
                {
                    new CharacterInspectProperties()
                };
                character.EngineerProperties = new List<CharacterEngineerProperties>
                {
                    new CharacterEngineerProperties()
                };
            }

            return character;
        }
    }
}