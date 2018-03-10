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
                //Note : Might have to hardcode some of the flags like fight_positining, kungfu, inspect and engineers from tag names. For now they are always off.

                /*
                 * squad : 0x4000 B1
                 * fight positioning : 0x10000000 B1
                 * kungfu : 0x80000 B3
                 * inspect : 0x80 B5
                 * Engineer start : 0xE0000000 B5
                 * Engineer end : 0x1F B6
                 * 
                 */
                //Style control is the same

                var H3B1 = style.Behaviors1;
                var H3B2 = style.Behaviors2;
                var H3B3 = style.Behaviors3;
                var H3B4 = style.Behaviors4;
                var H3B5 = style.Behaviors5;
                var H3B6 = style.Behaviors6;
                var H3B7 = style.Behaviors7;

                //                 First part      shift by one the rest        add 3 remaining of behavior 1

                style.Behaviors1 = (H3B1 & 0x3FFF) + ((H3B1 & 0x7FFC000) << 1) + ((H3B1 & 0x38000000) << 2);

                //                  Reuse 2 flags of H3B1       Use all of H3B2 except 2 last

                style.Behaviors2 = ((H3B1 & 0xC0000000) >> 30) + ((H3B2 & 0x3FFFFFFF) << 2);

                //                  Reuse 2 flags of H3B2        Some flags of H3B3     rest of flags

                style.Behaviors3 = ((H3B2 & 0xC0000000) >> 30) + ((H3B3 & 0x1FFFF) << 2) + ((H3B3 & 0x1FFE0000) << 3);

                //                  Reuse 3 flags of H3B3         Some flags of H3B4

                style.Behaviors4 = ((H3B3 & 0xE0000000) >> 29) + ((H3B4 & 0x1FFFFFFF) << 3);

                //                  Reuse 3 flags of H3B4         Some flags of H3B5     more flags of H3B5         

                style.Behaviors5 = ((H3B4 & 0xE0000000) >> 29) + ((H3B5 & 0x7F) << 3) + ((H3B5 & 0x1FFFF80) << 4);

                //                  rest of H3B5            20 first flags of H3B6

                style.Behaviors6 = ((H3B5 & 0xFE000000) >> 20) + ((H3B6 & 0xFFFFF) << 12);

                //              12 last flags of H3B6            5 flags of H3B7

                style.Behaviors7 = ((H3B6 & 0xFFF00000) >> 20) + ((H3B7 & 0xFFFFF) << 12);


                //Add the new behaviors to the list at the end of the tag:

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
            }

            return style;
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