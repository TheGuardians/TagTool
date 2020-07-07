using System;
using System.Collections.Generic;
using TagTool.Commands.Common;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.ModelAnimationGraphs
{
    class ApplySprintFixupsCommand : Command
    {
        private GameCache Cache { get; }

        public ApplySprintFixupsCommand(GameCache cache) :
            base(true,
                
                "ApplySprintFixups",
                "",
                
                "ApplySprintFixups",
                
                "")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return new TagToolError(CommandError.ArgCount);

            Console.WriteLine("Please specify the tags to be used (enter an empty line to finish):");

            string line;
            var tagIndices = new List<int>();

            while ((line = Console.ReadLine().TrimStart().TrimEnd()) != "")
                if (Cache.TagCache.TryGetCachedTag(line, out var instance) && instance != null && !tagIndices.Contains(instance.Index))
                    tagIndices.Add(instance.Index);

            using (var stream = Cache.OpenCacheReadWrite())
            {
                foreach (var tagIndex in tagIndices)
                {
                    var instance = Cache.TagCache.GetTag(tagIndex);

                    if (instance == null || !instance.IsInGroup("jmad"))
                        continue;

                    var jmad = Cache.Deserialize<ModelAnimationGraph>(stream, instance);

                    var putAwayIndex = -1;
                    var readyIndex = -1;

                    for (var i = 0; i < jmad.Animations.Count; i++)
                    {
                        var animation = jmad.Animations[i];
                        var name = Cache.StringTable.GetString(animation.Name);

                        if (name.EndsWith(":put_away"))
                        {
                            animation.LoopFrameIndexNew = (short)(animation.FrameCount - 1);
                            putAwayIndex = i;
                        }
                        else if (name.EndsWith(":ready"))
                        {
                            readyIndex = i;
                        }
                    }

                    foreach (var mode in jmad.Modes)
                    {
                        foreach (var weaponClass in mode.WeaponClass)
                        {
                            foreach (var weaponType in weaponClass.WeaponType)
                            {
                                var sprintEnterFound = false;
                                var sprintLoopFound = false;
                                var sprintLoopAirborneFound = false;
                                var sprintExitFound = false;

                                foreach (var action in weaponType.Actions)
                                {
                                    var label = Cache.StringTable.GetString(action.Label);

                                    switch (label)
                                    {
                                        case "sprint_enter":
                                            action.Animation = (short)putAwayIndex;
                                            sprintEnterFound = true;
                                            break;

                                        case "sprint_loop":
                                            action.Animation = (short)putAwayIndex;
                                            sprintLoopFound = true;
                                            break;

                                        case "sprint_loop_airborne":
                                            action.Animation = (short)putAwayIndex;
                                            sprintLoopAirborneFound = true;
                                            break;

                                        case "sprint_exit":
                                            action.Animation = (short)readyIndex;
                                            sprintExitFound = true;
                                            break;
                                    }
                                }

                                if (!sprintEnterFound)
                                    weaponType.Actions.Add(new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry
                                    {
                                        Label = Cache.StringTable.GetStringId("sprint_enter"),
                                        GraphIndex = -1,
                                        Animation = (short)putAwayIndex
                                    });

                                if (!sprintLoopFound)
                                    weaponType.Actions.Add(new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry
                                    {
                                        Label = Cache.StringTable.GetStringId("sprint_loop"),
                                        GraphIndex = -1,
                                        Animation = (short)putAwayIndex
                                    });

                                if (!sprintLoopAirborneFound)
                                    weaponType.Actions.Add(new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry
                                    {
                                        Label = Cache.StringTable.GetStringId("sprint_loop_airborne"),
                                        GraphIndex = -1,
                                        Animation = (short)putAwayIndex
                                    });

                                if (!sprintExitFound)
                                    weaponType.Actions.Add(new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry
                                    {
                                        Label = Cache.StringTable.GetStringId("sprint_exit"),
                                        GraphIndex = -1,
                                        Animation = (short)readyIndex
                                    });
                            }
                        }
                    }

                    Cache.Serialize(stream, instance, jmad);
                }
            }

            return true;
        }
    }
}
