using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Commands.Common;
using System.Linq;

namespace TagTool.Commands.ModelAnimationGraphs
{
    public class ListInheritanceCommand : Command
    {
        private GameCache CacheContext { get; }
        private ModelAnimationGraph Animation { get; set; }
        private CachedTag Jmad { get; set; }

        private int Index;

        public ListInheritanceCommand(GameCache cachecontext, ModelAnimationGraph animation, CachedTag jmad)
            : base(false,

                  "listInheritance",
                  "Print a list animations that are being inherited.",

                  "ListInheritance [index/tag]",

                  "Print a list of animations that are being inherited from tags in the Inheritance List.")
        {
            CacheContext = cachecontext;
            Animation = animation;
            Jmad = jmad;
        }

        public override object Execute(List<string> args)
        {
            var listToCheck = Animation.InheritanceList;
            bool specified = false;

            if (args.Count > 1)
                return new TagToolError(CommandError.ArgCount);

            if (args.Count == 1)
            {
                specified = true;
                if (!int.TryParse(args[0], out Index))
                {
                    if (!CacheContext.TagCache.TryGetCachedTag(args[0], out CachedTag tag))
                        return new TagToolError(CommandError.CustomError, $"{args[0]} is not a valid tag name.");

                    var blocka = Animation.InheritanceList.FirstOrDefault(x => x.InheritedGraph == tag);

                    if (blocka == null)
                        return new TagToolError(CommandError.CustomError, "Specified tag is not in the inheritance list.");
                    else
                        Index = Animation.InheritanceList.IndexOf(blocka);
                }
                else
                {
                    if (Index < 0 || Index > Animation.InheritanceList.Count())
                        return new TagToolError(CommandError.CustomError, $"{Index} is not a valid InheritanceList block index.");
                }
            }

            for (short i = 0; i < listToCheck.Count; i++)
            {
                if (specified && i != Index)
                    continue;

                string tagName = listToCheck[i].InheritedGraph?.ToString();
                List<string> inherited = new List<string> { };

                foreach (var mode in Animation.Modes)
                {
                    string modeName = CacheContext.StringTable.GetString(mode.Name);

                    foreach (var wClass in mode.WeaponClass)
                    {
                        string wClassName = CacheContext.StringTable.GetString(wClass.Label);

                        foreach (var wType in wClass.WeaponType)
                        {
                            string wTypeName = CacheContext.StringTable.GetString(wType.Label);

                            foreach (var action in wType.Set.Actions)
                            {
                                if (action.GraphIndex == i)
                                {
                                    string actionName = CacheContext.StringTable.GetString(action.Label);
                                    inherited.Add($"{modeName}:{wClassName}:{wTypeName}:{actionName}");
                                }
                            }

                            foreach (var overlay in wType.Set.Overlays)
                            {
                                if (overlay.GraphIndex == i)
                                {
                                    string overlayName = CacheContext.StringTable.GetString(overlay.Label);
                                    inherited.Add($"{modeName}:{wClassName}:{wTypeName}:{overlayName}");
                                }

                                foreach (var dd in wType.Set.DeathAndDamage)
                                    foreach (var dir in dd.Directions)
                                        foreach (var region in dir.Regions)
                                        {
                                            if (region.GraphIndex == i)
                                            {
                                                string ddName = CacheContext.StringTable.GetString(dd.Label);
                                                inherited.Add($"{modeName}:{wClassName}:{wTypeName}:{ddName}");
                                            }
                                        }

                                foreach (var trs in wType.Set.Transitions)
                                    foreach (var dest in trs.Destinations)
                                    {
                                        if (dest.GraphIndex == i)
                                        {
                                            string destName = CacheContext.StringTable.GetString(dest.FullName);
                                            inherited.Add($"{modeName}:{wClassName}:{wTypeName}:{destName}");
                                        }
                                    }
                            }
                        }
                    }
                }

                if (inherited.Any())
                {
                    Console.WriteLine($"({i}) {tagName}:");
                    foreach (var d in inherited)
                        Console.WriteLine("\t- " + d);
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Done.");
            return true;
        }
    }
}
