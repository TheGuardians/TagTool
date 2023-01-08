using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Cache.HaloOnline;

namespace TagTool.Commands.Tags
{
    class ListTagsCommand : Command
    {
        private GameCache Cache { get; }

        public ListTagsCommand(GameCache cache)
            : base(true,

                  "ListTags",
                  "Lists tag instances that are of the specified tag groups.",

                  "ListTags [TagGroup]... {Is|From|To:} {SearchType:}...",

                  "Options:\n" +
                  "  Is: Choice\n" +
                  "    Null - Shows only null/empty tags/names/groups.\n" +
                  "    Orphan - Shows only unused tags. HO Only.\n" +
                  "    Duplicate - Shows only duplicate tags.\n\n" +

                  "  SearchType: String\n" +
                  "    Starts|Begins\n" +
                  "    Contains|Has|Named|Filter\n" +
                  "    Ends\n\n" +

                  "  From: To|Until: Numeric\n" +
                  "    Tries to parse as int, then hex, then throws error.\n\n" +

                  "Examples:\n" +
                  "  ListTags is:duplicate pixel_shader vtsh contains:1_2_2\n" +
                  "  ListTags ends: world scnr starts: levels\\dlc has: unk\n" +
                  "  ListTags is:orphan snd! from:426c to:66ab has:delta\n\n" +

                  "Notes:\n" +
                  "- If no tag group is specified, all tags will be listed.\n" +
                  "- Accepts multiple has/contains and tag group filters.\n" +
                  "- Matches against child/grandchild groups of those specified.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            var invalidArgs = new List<string>();
            var options = (TagGroups: new List<string>(), Starts: "", Contains: new List<string>(), Ends: "", Is: "mixed", From: uint.MinValue, To: uint.MaxValue);

            // Process Args
            while (args.Count >= 1)
            {
                var option = args[0].ToLower().Split(':');
                args.RemoveAt(0);

                // Is Option
                if (option.Length == 1)
                {
                    if (Cache.TagCache.TryParseGroupTag(option[0], out var tagGroup))
                    {
                        options.TagGroups.Add(tagGroup.ToString());
                    }
                    // no further arguments, interpret this as search term
                    else if (args.Count == 0)
                    {
                        options.Contains.Add(option[0]);
                    }
                    else
                        invalidArgs.Add(option[0]);
                }
                // Is Option: Value
                else
                {
                    // Grab option value from next arg if needed
                    if (option[1] == "")
                    {
                        if (args.Count == 0 || args[0].Contains(":"))
                        {
                            invalidArgs.Add(option[0] + ":");
                            continue;
                        }
                        else
                        {
                            option[1] = args[0].ToLower();
                            args.RemoveAt(0);
                        }
                    }

                    // Fill options
                    if (new string[] { "start", "begin" }.Any(needle => option[0].StartsWith(needle)))
                        options.Starts = option[1];
                    else if (new string[] { "filter", "name", "named", "contain", "contains", "containing", "has" }.Contains(option[0]))
                        options.Contains.Add(option[1]);
                    else if (option[0].StartsWith("end"))
                        options.Ends = option[1];
                    else if (option[0] == "is" && new string[] { "null", "orphan", "duplicate" }.Contains(option[1]))
                        options.Is = option[1];
                    else if (option[0] == "from" && StringToUInt(option[1], ref options.From))
                        continue;
                    else if (new string[] { "to", "until" }.Contains(option[0]) && StringToUInt(option[1], ref options.To))
                        continue;
                    else
                        invalidArgs.Add($"{option[0]}:{option[1]}");
                }
            }


            // Warn of invalid args
            if (invalidArgs.Count > 0)
                return new TagToolError(CommandError.ArgInvalid, $"\"{String.Join(", ", invalidArgs.ToArray())}\"");


            // Warn of incompatible options with current cache
            if (options.Is == "orphan" && !(Cache is GameCacheHaloOnlineBase))
                return new TagToolError(CommandError.CacheUnsupported, @"[is:orphan] unavailable with current cache file.");


            // Store dependency list if we need to orphan search
            var dependencyList = options.Is != "orphan" ?
                null : Cache.TagCache.NonNull().SelectMany(tag => ((CachedTagHaloOnline)tag).Dependencies.Where(dep => dep != tag.Index)).ToHashSet();


            // Loop tags or only duplicates if requested
            var index = 0;
            foreach (CachedTag tag in (options.Is != "duplicate") ?
                Cache.TagCache.TagTable : Cache.TagCache.NonNull().GroupBy(tag => $"{tag.Name}.{tag.Group}").Where(g => g.Skip(1).Any()).SelectMany(g => g))
            {
                var data = (
                    Index: tag?.Index ?? index++,
                    Offset: tag?.DefinitionOffset.ToString("X8"),
                    Name: tag?.Name,
                    Group: tag?.Group.ToString(),
                    Group4: tag?.Group?.Tag.ToString(),
                    IsMatch: true
                    );

                if (  options.TagGroups.Count != 0 &&
                      !options.TagGroups.Contains(data.Group4)
                   )
                   data.IsMatch = false;

                if (  (data.Name is null && (
                        options.Starts != "" ||
                        options.Contains.Count != 0 ||
                        options.Ends != "")) ||
                      (data.Name != null && (
                        !data.Name.ToLower().StartsWith(options.Starts) ||
                        !options.Contains.All(needle => data.Name.ToLower().Contains(needle)) || 
                        !data.Name.ToLower().EndsWith(options.Ends)))
                   )
                   data.IsMatch = false;
               
                // Print result if needed
                if (  data.IsMatch &&
                      options.From <= data.Index && options.To >= data.Index &&
                      !(options.Is == "null" && !HasNull(data) && data.IsMatch) &&
                      !(options.Is == "orphan" && tag != null && (new string[]{ "cfgt", "scnr" }.Contains(data.Group4) || dependencyList.Contains(data.Index)))
                   )
                   Console.WriteLine("[Index: 0x{0:X4}, Offset: 0x{1}] {2}.{3}",
                       data.Index, data.Offset ?? "NULL TAG", data.Name ?? "<NULL OR EMPTY NAME>", data.Group ?? "<NULL GROUP>");
            }
            return true;
        }

        private bool HasNull(ITuple tuple)
        {
            for (int i = 0; i < tuple.Length; i++)
                if (tuple[i] is null)
                    return true;

            return false;
        }

        private bool StringToUInt(string str, ref uint target) {
            
            Match match = new Regex(@"^(?:(?<int>[0-9]{1,9})|(?:.*[^A-F0-9]){0,1}(?<hex>[A-F0-9]{1,8}))$", RegexOptions.IgnoreCase).Match(str);
 
            if (match.Groups["int"].Success)
                target = Convert.ToUInt32(match.Groups["int"].Value);
            else if (match.Groups["hex"].Success)
                target = Convert.ToUInt32(match.Groups["hex"].Value, 16);

            return match.Success;
        }
    }
}